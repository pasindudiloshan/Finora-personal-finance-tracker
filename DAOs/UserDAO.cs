using System;
using FinoraTracker.Models;
using MySql.Data.MySqlClient;
using FinoraTracker.Utills;
using BCrypt.Net;

namespace FinoraTracker.DAOs
{
    public class UserDAO
    {
        public bool RegisterUser(User user)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                // Check duplicate email
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email=@Email";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", user.Email);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                    throw new Exception("Email already exists!");

                // Hash password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                string query = @"INSERT INTO Users 
                                 (FullName, PhoneNumber, Email, Gender, Occupation, 
                                  IncomeFrequency, InvestmentInterest, HowDidYouKnow, Password)
                                 VALUES 
                                 (@FullName, @PhoneNumber, @Email, @Gender, @Occupation, 
                                  @IncomeFrequency, @InvestmentInterest, @HowDidYouKnow, @Password)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Gender", user.Gender);
                cmd.Parameters.AddWithValue("@Occupation", user.Occupation);
                cmd.Parameters.AddWithValue("@IncomeFrequency", user.IncomeFrequency);
                cmd.Parameters.AddWithValue("@InvestmentInterest", user.InvestmentInterest);
                cmd.Parameters.AddWithValue("@HowDidYouKnow", user.HowDidYouKnow);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public User? Login(string email, string password)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Users WHERE Email=@Email";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                using (MySqlDataReader reader = cmd.ExecuteReader())
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
}
