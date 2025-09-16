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
    public class IncomeDAOTests
    {
        private Mock<IDBConnectionProvider> _mockConnectionProvider = null!;
        private Mock<ICommandFactory> _mockCommandFactory = null!;
        private IncomeDAO _incomeDao = null!;

        [SetUp]
        public void Setup()
        {
            _mockConnectionProvider = new Mock<IDBConnectionProvider>();
            _mockCommandFactory = new Mock<ICommandFactory>();
            _incomeDao = new IncomeDAO(_mockConnectionProvider.Object, _mockCommandFactory.Object);
        }

        [Test]
        public void AddIncome_ShouldReturnTrue_WhenInsertSucceeds()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var income = new Income
            {
                UserId = "1",
                Amount = 500,
                Category = "Salary",
                IncomeDate = DateTime.Now,
                Description = "Monthly Salary",
                AccountSource = "Bank",
                CreatedAt = DateTime.Now
            };

            var result = _incomeDao.AddIncome(income);

            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteIncome_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var result = _incomeDao.DeleteIncome(1);

            Assert.That(result, Is.True);
        }

        [Test]
        public void UpdateIncome_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var income = new Income
            {
                IncomeId = 1,
                Amount = 600,
                Category = "Freelance",
                IncomeDate = DateTime.Now,
                Description = "Project Payment",
                AccountSource = "PayPal"
            };

            var result = _incomeDao.UpdateIncome(income);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetIncomeByUser_ShouldReturnListOfIncomes()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockReader = new Mock<MySqlDataReader>();
            mockReader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            mockReader.Setup(r => r["IncomeId"]).Returns(1);
            mockReader.Setup(r => r["UserId"]).Returns("1");
            mockReader.Setup(r => r["Amount"]).Returns(500m);
            mockReader.Setup(r => r["Category"]).Returns("Salary");
            mockReader.Setup(r => r["IncomeDate"]).Returns(DateTime.Today);
            mockReader.Setup(r => r["Description"]).Returns("Monthly Salary");
            mockReader.Setup(r => r["AccountSource"]).Returns("Bank");
            mockReader.Setup(r => r["CreatedAt"]).Returns(DateTime.Now);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var result = _incomeDao.GetIncomeByUser("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Category, Is.EqualTo("Salary"));
        }

        [Test]
        public void GetIncomeByUserLast30Days_ShouldReturnListOfIncomes()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockReader = new Mock<MySqlDataReader>();
            mockReader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            mockReader.Setup(r => r["IncomeId"]).Returns(2);
            mockReader.Setup(r => r["UserId"]).Returns("1");
            mockReader.Setup(r => r["Amount"]).Returns(200m);
            mockReader.Setup(r => r["Category"]).Returns("Freelance");
            mockReader.Setup(r => r["IncomeDate"]).Returns(DateTime.Today);
            mockReader.Setup(r => r["Description"]).Returns("Project");
            mockReader.Setup(r => r["AccountSource"]).Returns("PayPal");
            mockReader.Setup(r => r["CreatedAt"]).Returns(DateTime.Now);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var result = _incomeDao.GetIncomeByUserLast30Days("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Category, Is.EqualTo("Freelance"));
        }
    }
}
