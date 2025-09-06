using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FinoraTracker.Models;
using FinoraTracker.Utills;

namespace FinoraTracker.DAOs
{
    public class IncomeDAO
    {
        // 🔹 Add a new income record
        public bool AddIncome(Income income)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"INSERT INTO Income
                                (UserId, Amount, Category, IncomeDate, Description, AccountSource, CreatedAt)
                                VALUES (@UserId, @Amount, @Category, @IncomeDate, @Description, @AccountSource, @CreatedAt)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", income.UserId);
                    cmd.Parameters.AddWithValue("@Amount", income.Amount);
                    cmd.Parameters.AddWithValue("@Category", income.Category);
                    cmd.Parameters.AddWithValue("@IncomeDate", income.IncomeDate);
                    cmd.Parameters.AddWithValue("@Description", income.Description);
                    cmd.Parameters.AddWithValue("@AccountSource", income.AccountSource);
                    cmd.Parameters.AddWithValue("@CreatedAt", income.CreatedAt);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // 🔹 Get all incomes for a specific user
        public List<Income> GetIncomeByUser(string userId)
        {
            List<Income> incomes = new List<Income>();
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Income WHERE UserId = @UserId ORDER BY IncomeDate DESC";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            incomes.Add(new Income
                            {
                                IncomeId = reader.GetInt32("IncomeId"),
                                UserId = reader.GetString("UserId"),
                                Amount = reader.GetDecimal("Amount"),
                                Category = reader.GetString("Category"),
                                IncomeDate = reader.GetDateTime("IncomeDate"),
                                Description = reader["Description"] != DBNull.Value ? reader.GetString("Description") : "",
                                AccountSource = reader["AccountSource"] != DBNull.Value ? reader.GetString("AccountSource") : "",
                                CreatedAt = reader.GetDateTime("CreatedAt")
                            });
                        }
                    }
                }
            }
            return incomes;
        }

        // 🔹 Get last 30 days incomes for chart
        public List<Income> GetIncomeByUserLast30Days(string userId)
        {
            List<Income> incomes = new List<Income>();
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"SELECT * FROM Income 
                                 WHERE UserId = @UserId AND IncomeDate >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                                 ORDER BY IncomeDate ASC";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            incomes.Add(new Income
                            {
                                IncomeId = reader.GetInt32("IncomeId"),
                                UserId = reader.GetString("UserId"),
                                Amount = reader.GetDecimal("Amount"),
                                Category = reader.GetString("Category"),
                                IncomeDate = reader.GetDateTime("IncomeDate"),
                                Description = reader["Description"] != DBNull.Value ? reader.GetString("Description") : "",
                                AccountSource = reader["AccountSource"] != DBNull.Value ? reader.GetString("AccountSource") : "",
                                CreatedAt = reader.GetDateTime("CreatedAt")
                            });
                        }
                    }
                }
            }
            return incomes;
        }

        // 🔹 Delete an income by ID
        public bool DeleteIncome(int incomeId)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Income WHERE IncomeId = @IncomeId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IncomeId", incomeId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // 🔹 Update an existing income
        public bool UpdateIncome(Income income)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"UPDATE Income SET
                                 Amount = @Amount,
                                 Category = @Category,
                                 IncomeDate = @IncomeDate,
                                 Description = @Description,
                                 AccountSource = @AccountSource
                                 WHERE IncomeId = @IncomeId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Amount", income.Amount);
                    cmd.Parameters.AddWithValue("@Category", income.Category);
                    cmd.Parameters.AddWithValue("@IncomeDate", income.IncomeDate);
                    cmd.Parameters.AddWithValue("@Description", income.Description);
                    cmd.Parameters.AddWithValue("@AccountSource", income.AccountSource);
                    cmd.Parameters.AddWithValue("@IncomeId", income.IncomeId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
