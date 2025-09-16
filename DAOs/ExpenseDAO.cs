using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FinoraTracker.Models;
using FinoraTracker.Utills;

namespace FinoraTracker.DAOs
{
    public class ExpenseDAO
    {
        private readonly IDBConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        public ExpenseDAO(IDBConnectionProvider connectionProvider, ICommandFactory commandFactory)
        {
            _connectionProvider = connectionProvider;
            _commandFactory = commandFactory;
        }

        // 🔹 Add a new expense record
        public bool AddExpense(Expense expense)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                var query = @"INSERT INTO Expenses
                                (UserId, Amount, Category, ExpenseDate, Description, PaymentMethod, CreatedAt)
                                VALUES (@UserId, @Amount, @Category, @ExpenseDate, @Description, @PaymentMethod, @CreatedAt)";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", expense.UserId);
                    cmd.Parameters.AddWithValue("@Amount", expense.Amount);
                    cmd.Parameters.AddWithValue("@Category", expense.Category);
                    cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate);
                    cmd.Parameters.AddWithValue("@Description", expense.Description);
                    cmd.Parameters.AddWithValue("@PaymentMethod", expense.PaymentMethod);
                    cmd.Parameters.AddWithValue("@CreatedAt", expense.CreatedAt);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // 🔹 Get all expenses for a specific user
        public List<Expense> GetExpensesByUser(string userId)
        {
            List<Expense> expenses = new List<Expense>();
            using (var conn = _connectionProvider.GetConnection())
            {
                var query = "SELECT * FROM Expenses WHERE UserId = @UserId ORDER BY ExpenseDate DESC";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            expenses.Add(MapReaderToExpense(reader));
                        }
                    }
                }
            }
            return expenses;
        }

        // 🔹 Get last 30 days expenses for chart
        public List<Expense> GetExpensesByUserLast30Days(string userId)
        {
            List<Expense> expenses = new List<Expense>();
            using (var conn = _connectionProvider.GetConnection())
            {
                var query = @"SELECT * FROM Expenses 
                              WHERE UserId = @UserId AND ExpenseDate >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                              ORDER BY ExpenseDate ASC";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            expenses.Add(MapReaderToExpense(reader));
                        }
                    }
                }
            }
            return expenses;
        }

        // 🔹 Delete an expense by ID
        public bool DeleteExpense(int expenseId)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                var query = "DELETE FROM Expenses WHERE ExpenseId = @ExpenseId";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ExpenseId", expenseId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // 🔹 Update an existing expense
        public bool UpdateExpense(Expense expense)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                var query = @"UPDATE Expenses SET
                                Amount = @Amount,
                                Category = @Category,
                                ExpenseDate = @ExpenseDate,
                                Description = @Description,
                                PaymentMethod = @PaymentMethod
                              WHERE ExpenseId = @ExpenseId";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Amount", expense.Amount);
                    cmd.Parameters.AddWithValue("@Category", expense.Category);
                    cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate);
                    cmd.Parameters.AddWithValue("@Description", expense.Description);
                    cmd.Parameters.AddWithValue("@PaymentMethod", expense.PaymentMethod);
                    cmd.Parameters.AddWithValue("@ExpenseId", expense.ExpenseId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // 🔹 Helper: Map MySqlDataReader row to Expense object
        private Expense MapReaderToExpense(MySqlDataReader reader)
        {
            return new Expense
            {
                ExpenseId = reader.GetInt32("ExpenseId"),
                UserId = reader.GetString("UserId"),
                Amount = reader.GetDecimal("Amount"),
                Category = reader.GetString("Category"),
                ExpenseDate = reader.GetDateTime("ExpenseDate"),
                Description = reader["Description"] != DBNull.Value ? reader.GetString("Description") : "",
                PaymentMethod = reader["PaymentMethod"] != DBNull.Value ? reader.GetString("PaymentMethod") : "",
                CreatedAt = reader.GetDateTime("CreatedAt")
            };
        }
    }
}
