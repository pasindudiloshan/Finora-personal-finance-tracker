using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FinoraTracker.Models;
using FinoraTracker.Utills;

namespace FinoraTracker.DAOs
{
    public class PortfolioDAO
    {
        private readonly IDBConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        // Default constructor for production
        public PortfolioDAO()
        {
            _connectionProvider = new DefaultDBConnectionProvider();
            _commandFactory = new DefaultCommandFactory();
        }

        // Constructor for unit testing (mocked dependencies)
        public PortfolioDAO(IDBConnectionProvider connectionProvider, ICommandFactory commandFactory)
        {
            _connectionProvider = connectionProvider;
            _commandFactory = commandFactory;
        }

        public bool AddPortfolio(Portfolio portfolio)
        {
            using (MySqlConnection conn = _connectionProvider.GetConnection())
            {
                string query = @"INSERT INTO Portfolio
                                (UserId, CompanyName, Shares, SharePrice, PERatio, TargetPrice, CreatedAt)
                                VALUES (@UserId, @CompanyName, @Shares, @SharePrice, @PERatio, @TargetPrice, @CreatedAt)";
                using (MySqlCommand cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", portfolio.UserId);
                    cmd.Parameters.AddWithValue("@CompanyName", portfolio.CompanyName);
                    cmd.Parameters.AddWithValue("@Shares", portfolio.Shares);
                    cmd.Parameters.AddWithValue("@SharePrice", portfolio.SharePrice);
                    cmd.Parameters.AddWithValue("@PERatio", portfolio.PERatio ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetPrice", portfolio.TargetPrice ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedAt", portfolio.CreatedAt);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Portfolio> GetPortfolioByUser(string userId)
        {
            List<Portfolio> portfolios = new List<Portfolio>();
            using (MySqlConnection conn = _connectionProvider.GetConnection())
            {
                string query = "SELECT * FROM Portfolio WHERE UserId = @UserId ORDER BY CreatedAt DESC";
                using (MySqlCommand cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            portfolios.Add(new Portfolio
                            {
                                PortfolioId = Convert.ToInt32(reader["PortfolioId"]),
                                UserId = reader["UserId"].ToString()!,
                                CompanyName = reader["CompanyName"].ToString()!,
                                Shares = Convert.ToInt32(reader["Shares"]),
                                SharePrice = Convert.ToDecimal(reader["SharePrice"]),
                                Value = Convert.ToDecimal(reader["Value"]),
                                PERatio = reader["PERatio"] != DBNull.Value ? Convert.ToDecimal(reader["PERatio"]) : (decimal?)null,
                                TargetPrice = reader["TargetPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TargetPrice"]) : (decimal?)null,
                                TargetValue = Convert.ToDecimal(reader["TargetValue"]),
                                GainPercent = Convert.ToDecimal(reader["GainPercent"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                            });
                        }
                    }
                }
            }
            return portfolios;
        }

        public bool DeletePortfolio(int portfolioId)
        {
            using (MySqlConnection conn = _connectionProvider.GetConnection())
            {
                string query = "DELETE FROM Portfolio WHERE PortfolioId = @PortfolioId";
                using (MySqlCommand cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PortfolioId", portfolioId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdatePortfolio(Portfolio portfolio)
        {
            using (MySqlConnection conn = _connectionProvider.GetConnection())
            {
                string query = @"UPDATE Portfolio SET
                                 CompanyName = @CompanyName,
                                 Shares = @Shares,
                                 SharePrice = @SharePrice,
                                 PERatio = @PERatio,
                                 TargetPrice = @TargetPrice
                                 WHERE PortfolioId = @PortfolioId";
                using (MySqlCommand cmd = _commandFactory.CreateCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyName", portfolio.CompanyName);
                    cmd.Parameters.AddWithValue("@Shares", portfolio.Shares);
                    cmd.Parameters.AddWithValue("@SharePrice", portfolio.SharePrice);
                    cmd.Parameters.AddWithValue("@PERatio", portfolio.PERatio ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetPrice", portfolio.TargetPrice ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PortfolioId", portfolio.PortfolioId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
