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
    public class ExpenseDAOTests
    {
        private Mock<IDBConnectionProvider> _mockConnectionProvider = null!;
        private Mock<ICommandFactory> _mockCommandFactory = null!;
        private ExpenseDAO _expenseDao = null!;

        [SetUp]
        public void Setup()
        {
            _mockConnectionProvider = new Mock<IDBConnectionProvider>();
            _mockCommandFactory = new Mock<ICommandFactory>();
            _expenseDao = new ExpenseDAO(_mockConnectionProvider.Object, _mockCommandFactory.Object);
        }

        [Test]
        public void AddExpense_ShouldReturnTrue_WhenInsertSucceeds()
        {
            // Arrange
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var expense = new Expense
            {
                UserId = "1",
                Amount = 100,
                Category = "Food",
                ExpenseDate = DateTime.Now,
                Description = "Lunch",
                PaymentMethod = "Cash",
                CreatedAt = DateTime.Now
            };

            // Act
            var result = _expenseDao.AddExpense(expense);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteExpense_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var result = _expenseDao.DeleteExpense(1);

            Assert.That(result, Is.True);
        }

        [Test]
        public void UpdateExpense_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var expense = new Expense
            {
                ExpenseId = 1,
                Amount = 200,
                Category = "Transport",
                ExpenseDate = DateTime.Now,
                Description = "Taxi",
                PaymentMethod = "Card"
            };

            var result = _expenseDao.UpdateExpense(expense);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetExpensesByUser_ShouldReturnListOfExpenses()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockReader = new Mock<MySqlDataReader>();
            mockReader.SetupSequence(r => r.Read())
                      .Returns(true)
                      .Returns(false);
            mockReader.Setup(r => r["ExpenseId"]).Returns(1);
            mockReader.Setup(r => r["UserId"]).Returns("1");
            mockReader.Setup(r => r["Amount"]).Returns(50m);
            mockReader.Setup(r => r["Category"]).Returns("Food");
            mockReader.Setup(r => r["ExpenseDate"]).Returns(DateTime.Today);
            mockReader.Setup(r => r["Description"]).Returns("Breakfast");
            mockReader.Setup(r => r["PaymentMethod"]).Returns("Cash");
            mockReader.Setup(r => r["CreatedAt"]).Returns(DateTime.Now);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var result = _expenseDao.GetExpensesByUser("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Category, Is.EqualTo("Food"));
        }

        [Test]
        public void GetExpensesByUserLast30Days_ShouldReturnListOfExpenses()
        {
            var mockConn = new Mock<MySqlConnection>();
            _mockConnectionProvider.Setup(x => x.GetConnection()).Returns(mockConn.Object);

            var mockReader = new Mock<MySqlDataReader>();
            mockReader.SetupSequence(r => r.Read())
                      .Returns(true)
                      .Returns(false);
            mockReader.Setup(r => r["ExpenseId"]).Returns(1);
            mockReader.Setup(r => r["UserId"]).Returns("1");
            mockReader.Setup(r => r["Amount"]).Returns(70m);
            mockReader.Setup(r => r["Category"]).Returns("Transport");
            mockReader.Setup(r => r["ExpenseDate"]).Returns(DateTime.Today);
            mockReader.Setup(r => r["Description"]).Returns("Bus");
            mockReader.Setup(r => r["PaymentMethod"]).Returns("Cash");
            mockReader.Setup(r => r["CreatedAt"]).Returns(DateTime.Now);

            var mockCmd = new Mock<MySqlCommand>();
            mockCmd.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);

            _mockCommandFactory
                .Setup(f => f.CreateCommand(It.IsAny<string>(), mockConn.Object))
                .Returns(mockCmd.Object);

            var result = _expenseDao.GetExpensesByUserLast30Days("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Category, Is.EqualTo("Transport"));
        }
    }
}
