using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FinoraTracker.Models;
using FinoraTracker.Utills;

namespace FinoraTracker.DAOs
{
    public class IncomeDAO
    {
        private readonly IDBConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        // ✅ Dependency Injection Constructor
        public IncomeDAO(IDBConnectionProvider connectionProvider, ICommandFactory commandFactory)
        {
            _connectionProvider = connectionProvider;
            _commandFactory = commandFactory;
        }

        // 🔹 Add a new income record
        public bool AddIncome(Income income)
        {
            using (MySqlConnection conn = _connectionProvider.GetConnection())
            {
                string query = @"INSERT INTO Income
                                (UserId, Amount, Category, IncomeDate, Description, AccountSource, CreatedAt)
                                VALUES (@UserId, @Amount, @Category, @IncomeDate, @Description, @AccountSource, @CreatedAt)";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", income.UserId);
                    cmd.Parameters.AddWithValue("@Amount", income.Amount);
                    cmd.Parameters.AddWithValue("@Category", income.Category);
                    cmd.Parameters.AddWithValue("@IncomeDate", income.IncomeDate);
                    cmd.Parameters.AddWithValue("@Description", income.Description ?? "");
                    cmd.Parameters.AddWithValue("@AccountSource", income.AccountSource ?? "");
                    cmd.Parameters.AddWithValue("@CreatedAt", income.CreatedAt);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // 🔹 Get all incomes for a specific user
        public List<Income> GetIncomeByUser(string userId)
        {
            List<Income> incomes = new List<Income>();
            using (MySqlConnection conn = _connectionProvider.GetConnection())
            {
                string query = "SELECT * FROM Income WHERE UserId = @UserId ORDER BY IncomeDate DESC";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
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
            using (MySqlConnection conn = _connectionProvider.GetConnection())
            {
                string query = @"SELECT * FROM Income 
                                 WHERE UserId = @UserId AND IncomeDate >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                                 ORDER BY IncomeDate ASC";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
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
            using (MySqlConnection conn = _connectionProvider.GetConnection())
            {
                string query = "DELETE FROM Income WHERE IncomeId = @IncomeId";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IncomeId", incomeId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // 🔹 Update an existing income
        public bool UpdateIncome(Income income)
        {
            using (MySqlConnection conn = _connectionProvider.GetConnection())
            {
                string query = @"UPDATE Income SET
                                 Amount = @Amount,
                                 Category = @Category,
                                 IncomeDate = @IncomeDate,
                                 Description = @Description,
                                 AccountSource = @AccountSource
                                 WHERE IncomeId = @IncomeId";
                using (var cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Amount", income.Amount);
                    cmd.Parameters.AddWithValue("@Category", income.Category);
                    cmd.Parameters.AddWithValue("@IncomeDate", income.IncomeDate);
                    cmd.Parameters.AddWithValue("@Description", income.Description ?? "");
                    cmd.Parameters.AddWithValue("@AccountSource", income.AccountSource ?? "");
                    cmd.Parameters.AddWithValue("@IncomeId", income.IncomeId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
