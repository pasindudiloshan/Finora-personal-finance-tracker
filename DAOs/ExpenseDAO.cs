using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FinoraTracker.Models;
using FinoraTracker.Utills;

namespace FinoraTracker.DAOs
{
    public class ExpenseDAO
    {
        // 🔹 Add a new expense record
        public bool AddExpense(Expense expense)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"INSERT INTO Expenses
                                (UserId, Amount, Category, ExpenseDate, Description, PaymentMethod, CreatedAt)
                                VALUES (@UserId, @Amount, @Category, @ExpenseDate, @Description, @PaymentMethod, @CreatedAt)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
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
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Expenses WHERE UserId = @UserId ORDER BY ExpenseDate DESC";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            expenses.Add(new Expense
                            {
                                ExpenseId = reader.GetInt32("ExpenseId"),
                                UserId = reader.GetString("UserId"),
                                Amount = reader.GetDecimal("Amount"),
                                Category = reader.GetString("Category"),
                                ExpenseDate = reader.GetDateTime("ExpenseDate"),
                                Description = reader["Description"] != DBNull.Value ? reader.GetString("Description") : "",
                                PaymentMethod = reader["PaymentMethod"] != DBNull.Value ? reader.GetString("PaymentMethod") : "",
                                CreatedAt = reader.GetDateTime("CreatedAt")
                            });
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
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"SELECT * FROM Expenses 
                                 WHERE UserId = @UserId AND ExpenseDate >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                                 ORDER BY ExpenseDate ASC";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            expenses.Add(new Expense
                            {
                                ExpenseId = reader.GetInt32("ExpenseId"),
                                UserId = reader.GetString("UserId"),
                                Amount = reader.GetDecimal("Amount"),
                                Category = reader.GetString("Category"),
                                ExpenseDate = reader.GetDateTime("ExpenseDate"),
                                Description = reader["Description"] != DBNull.Value ? reader.GetString("Description") : "",
                                PaymentMethod = reader["PaymentMethod"] != DBNull.Value ? reader.GetString("PaymentMethod") : "",
                                CreatedAt = reader.GetDateTime("CreatedAt")
                            });
                        }
                    }
                }
            }
            return expenses;
        }

        // 🔹 Delete an expense by ID
        public bool DeleteExpense(int expenseId)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Expenses WHERE ExpenseId = @ExpenseId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ExpenseId", expenseId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // 🔹 Update an existing expense
        public bool UpdateExpense(Expense expense)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"UPDATE Expenses SET
                                 Amount = @Amount,
                                 Category = @Category,
                                 ExpenseDate = @ExpenseDate,
                                 Description = @Description,
                                 PaymentMethod = @PaymentMethod
                                 WHERE ExpenseId = @ExpenseId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
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
    }
}
