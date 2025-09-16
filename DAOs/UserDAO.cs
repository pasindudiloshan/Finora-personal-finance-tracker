using System;
using FinoraTracker.Models;
using MySql.Data.MySqlClient;
using FinoraTracker.Utills;
using BCrypt.Net;

namespace FinoraTracker.DAOs
{
    // Interface for DB connection provider
    public interface IDBConnectionProvider
    {
        MySqlConnection GetConnection();
    }

    // Interface for creating commands (so we can mock them in tests)
    public interface ICommandFactory
    {
        MySqlCommand CreateCommand(string query, MySqlConnection connection);
    }

    public class UserDAO
    {
        private readonly IDBConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        public UserDAO(IDBConnectionProvider connectionProvider, ICommandFactory commandFactory)
        {
            _connectionProvider = connectionProvider;
            _commandFactory = commandFactory;
        }

        public bool RegisterUser(User user)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                // Check duplicate email
                var checkCmd = _commandFactory.CreateCommand("SELECT COUNT(*) FROM Users WHERE Email=@Email", conn);
                checkCmd.Parameters.AddWithValue("@Email", user.Email);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                    throw new Exception("Email already exists!");

                // Hash password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                var query = @"INSERT INTO Users 
                              (FullName, PhoneNumber, Email, Gender, Occupation, 
                               IncomeFrequency, InvestmentInterest, HowDidYouKnow, Password)
                              VALUES 
                              (@FullName, @PhoneNumber, @Email, @Gender, @Occupation, 
                               @IncomeFrequency, @InvestmentInterest, @HowDidYouKnow, @Password)";

                var cmd = _commandFactory.CreateCommand(query, conn);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Gender", user.Gender);
                cmd.Parameters.AddWithValue("@Occupation", user.Occupation);
                cmd.Parameters.AddWithValue("@IncomeFrequency", user.IncomeFrequency);
                cmd.Parameters.AddWithValue("@InvestmentInterest", user.InvestmentInterest);
                cmd.Parameters.AddWithValue("@HowDidYouKnow", user.HowDidYouKnow);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public User? Login(string email, string password)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                var cmd = _commandFactory.CreateCommand("SELECT * FROM Users WHERE Email=@Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHash = reader["Password"].ToString() ?? "";
                        if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                        {
                            return new User
                            {
                                UserId = reader["UserId"].ToString() ?? string.Empty,
                                FullName = reader["FullName"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                PhoneNumber = reader["PhoneNumber"].ToString() ?? string.Empty,
                                Gender = reader["Gender"].ToString() ?? string.Empty,
                                Occupation = reader["Occupation"].ToString() ?? string.Empty,
                                IncomeFrequency = reader["IncomeFrequency"].ToString() ?? string.Empty,
                                InvestmentInterest = reader["InvestmentInterest"].ToString() ?? string.Empty,
                                HowDidYouKnow = reader["HowDidYouKnow"].ToString() ?? string.Empty
                            };
                        }
                    }
                }
            }
            return null;
        }
    }

    // Default DB connection provider
    public class DefaultDBConnectionProvider : IDBConnectionProvider
    {
        public MySqlConnection GetConnection() => DBConnection.GetConnection();
    }

    // Default command factory
    public class DefaultCommandFactory : ICommandFactory
    {
        public MySqlCommand CreateCommand(string query, MySqlConnection connection) =>
            new MySqlCommand(query, connection);
    }
}
