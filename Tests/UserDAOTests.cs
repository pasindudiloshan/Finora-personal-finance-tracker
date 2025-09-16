using System;
using NUnit.Framework;
using Moq;
using FinoraTracker.DAOs;
using FinoraTracker.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using BCrypt.Net;

namespace FinoraTracker.Tests
{
    [TestFixture]
    public class UserDAOTests
    {
        private Mock<IDBConnectionProvider> _mockConnectionProvider = null!;
        private Mock<ICommandFactory> _mockCommandFactory = null!;
        private UserDAO _userDao = null!;

        [SetUp]
        public void Setup()
        {
            _mockConnectionProvider = new Mock<IDBConnectionProvider>();
            _mockCommandFactory = new Mock<ICommandFactory>();
            _userDao = new UserDAO(_mockConnectionProvider.Object, _mockCommandFactory.Object);
        }

        [Test]
        public void RegisterUser_ShouldThrowException_WhenEmailExists()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteScalar()).Returns(1); // Email exists
            _mockCommandFactory.Setup(f => f.CreateCommand(It.IsAny<string>(), It.IsAny<MySqlConnection>()))
                               .Returns(mockCmd.Object);

            var user = new User { FullName = "Test User", Email = "existing@example.com", Password = "Password123" };

            var ex = Assert.Throws<Exception>(() => _userDao.RegisterUser(user));
            Assert.That(ex!.Message, Is.EqualTo("Email already exists!"));
        }

        [Test]
        public void RegisterUser_ShouldReturnTrue_WhenInsertSucceeds()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteScalar()).Returns(0); // No duplicate
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1); // Insert success
            _mockCommandFactory.Setup(f => f.CreateCommand(It.IsAny<string>(), It.IsAny<MySqlConnection>()))
                               .Returns(mockCmd.Object);

            var user = new User
            {
                FullName = "Test User",
                Email = "newuser@example.com",
                Password = "Password123",
                PhoneNumber = "1234567890",
                Gender = "Male",
                Occupation = "Developer",
                IncomeFrequency = "Monthly",
                InvestmentInterest = "Stocks",
                HowDidYouKnow = "Friend"
            };

            bool result = _userDao.RegisterUser(user);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Login_ShouldReturnUser_WhenPasswordMatches()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123");

            var mockReader = new Mock<MySqlDataReader>();
            mockReader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            mockReader.Setup(r => r["Password"]).Returns(hashedPassword);
            mockReader.Setup(r => r["UserId"]).Returns("1");
            mockReader.Setup(r => r["FullName"]).Returns("Test User");
            mockReader.Setup(r => r["Email"]).Returns("test@example.com");
            mockReader.Setup(r => r["PhoneNumber"]).Returns("1234567890");
            mockReader.Setup(r => r["Gender"]).Returns("Male");
            mockReader.Setup(r => r["Occupation"]).Returns("Developer");
            mockReader.Setup(r => r["IncomeFrequency"]).Returns("Monthly");
            mockReader.Setup(r => r["InvestmentInterest"]).Returns("Stocks");
            mockReader.Setup(r => r["HowDidYouKnow"]).Returns("Friend");

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);
            _mockCommandFactory.Setup(f => f.CreateCommand(It.IsAny<string>(), It.IsAny<MySqlConnection>()))
                               .Returns(mockCmd.Object);

            var user = _userDao.Login("test@example.com", "Password123");

            Assert.That(user, Is.Not.Null);
            Assert.That(user!.FullName, Is.EqualTo("Test User"));
        }

        [Test]
        public void Login_ShouldReturnNull_WhenPasswordDoesNotMatch()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("CorrectPassword");

            var mockReader = new Mock<MySqlDataReader>();
            mockReader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            mockReader.Setup(r => r["Password"]).Returns(hashedPassword);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);
            _mockCommandFactory.Setup(f => f.CreateCommand(It.IsAny<string>(), It.IsAny<MySqlConnection>()))
                               .Returns(mockCmd.Object);

            var user = _userDao.Login("test@example.com", "WrongPassword");
            Assert.That(user, Is.Null);
        }
    }
}
