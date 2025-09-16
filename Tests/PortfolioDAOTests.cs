using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Moq;
using NUnit.Framework;
using FinoraTracker.DAOs;
using FinoraTracker.Models;

namespace FinoraTracker.Tests
{
    [TestFixture]
    public class PortfolioDAOTests
    {
        private Mock<IDBConnectionProvider> _mockConnectionProvider = null!;
        private Mock<ICommandFactory> _mockCommandFactory = null!;
        private PortfolioDAO _portfolioDao = null!;

        [SetUp]
        public void Setup()
        {
            _mockConnectionProvider = new Mock<IDBConnectionProvider>();
            _mockCommandFactory = new Mock<ICommandFactory>();
            _portfolioDao = new PortfolioDAO(_mockConnectionProvider.Object, _mockCommandFactory.Object);
        }

        [Test]
        public void AddPortfolio_ShouldReturnTrue_WhenInsertSucceeds()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var portfolio = new Portfolio
            {
                UserId = "1",               // 👈 required member
                CompanyName = "ABC Corp",
                Shares = 100,
                SharePrice = 50.5m,
                PERatio = 15.2m,
                TargetPrice = 60.0m,
                CreatedAt = DateTime.Now
            };

            var result = _portfolioDao.AddPortfolio(portfolio);

            Assert.That(result, Is.True);
        }

        [Test]
        public void DeletePortfolio_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var result = _portfolioDao.DeletePortfolio(1);

            Assert.That(result, Is.True);
        }

        [Test]
        public void UpdatePortfolio_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var portfolio = new Portfolio
            {
                PortfolioId = 1,
                UserId = "1",               // 👈 required member
                CompanyName = "ABC Corp Updated",
                Shares = 150,
                SharePrice = 55.0m,
                PERatio = 16.5m,
                TargetPrice = 65.0m
            };

            var result = _portfolioDao.UpdatePortfolio(portfolio);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetPortfolioByUser_ShouldReturnListOfPortfolios()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockReader = new Mock<MySqlDataReader>();
            mockReader.SetupSequence(r => r.Read())
                      .Returns(true)
                      .Returns(false);
            mockReader.Setup(r => r["PortfolioId"]).Returns(1);
            mockReader.Setup(r => r["UserId"]).Returns("1");
            mockReader.Setup(r => r["CompanyName"]).Returns("ABC Corp");
            mockReader.Setup(r => r["Shares"]).Returns(100);
            mockReader.Setup(r => r["SharePrice"]).Returns(50.5m);
            mockReader.Setup(r => r["Value"]).Returns(5050m);
            mockReader.Setup(r => r["PERatio"]).Returns(15.2m);
            mockReader.Setup(r => r["TargetPrice"]).Returns(60.0m);
            mockReader.Setup(r => r["TargetValue"]).Returns(6000m);
            mockReader.Setup(r => r["GainPercent"]).Returns(18.8m);
            mockReader.Setup(r => r["CreatedAt"]).Returns(DateTime.Now);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var result = _portfolioDao.GetPortfolioByUser("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].CompanyName, Is.EqualTo("ABC Corp"));
        }
    }
}
