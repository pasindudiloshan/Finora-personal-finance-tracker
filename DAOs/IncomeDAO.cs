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

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
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
    }
}
