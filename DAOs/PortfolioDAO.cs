using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FinoraTracker.Models;
using FinoraTracker.Utills;

namespace FinoraTracker.DAOs
{
    public class PortfolioDAO
    {
        public bool AddPortfolio(Portfolio portfolio)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"INSERT INTO Portfolio
                                (UserId, CompanyName, Shares, SharePrice, PERatio, TargetPrice, CreatedAt)
                                VALUES (@UserId, @CompanyName, @Shares, @SharePrice, @PERatio, @TargetPrice, @CreatedAt)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
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
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Portfolio WHERE UserId = @UserId ORDER BY CreatedAt DESC";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            portfolios.Add(new Portfolio
                            {
                                PortfolioId = reader.GetInt32("PortfolioId"),
                                UserId = reader.GetString("UserId"),
                                CompanyName = reader.GetString("CompanyName"),
                                Shares = reader.GetInt32("Shares"),
                                SharePrice = reader.GetDecimal("SharePrice"),
                                Value = reader.GetDecimal("Value"),
                                PERatio = reader["PERatio"] != DBNull.Value ? reader.GetDecimal("PERatio") : (decimal?)null,
                                TargetPrice = reader["TargetPrice"] != DBNull.Value ? reader.GetDecimal("TargetPrice") : (decimal?)null,
                                TargetValue = reader.GetDecimal("TargetValue"),
                                GainPercent = reader.GetDecimal("GainPercent"),
                                CreatedAt = reader.GetDateTime("CreatedAt")
                            });
                        }
                    }
                }
            }
            return portfolios;
        }

        public bool DeletePortfolio(int portfolioId)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Portfolio WHERE PortfolioId = @PortfolioId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PortfolioId", portfolioId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdatePortfolio(Portfolio portfolio)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"UPDATE Portfolio SET
                                 CompanyName = @CompanyName,
                                 Shares = @Shares,
                                 SharePrice = @SharePrice,
                                 PERatio = @PERatio,
                                 TargetPrice = @TargetPrice
                                 WHERE PortfolioId = @PortfolioId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
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
