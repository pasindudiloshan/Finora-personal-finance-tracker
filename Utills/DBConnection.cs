using System;
using MySql.Data.MySqlClient;

namespace FinoraTracker.Utills
{
    internal class DBConnection
    {
        // 🔹 Change password to your MySQL root password
        private static readonly string connectionString =
            "Server=localhost;Database=finora;Uid=root;Pwd=;";

        // 🔹 Get an open MySQL connection
        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                Console.WriteLine("✅ Database connection established.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("❌ Database connection failed: " + ex.Message);
                throw; // Important: let caller know connection failed
            }
            return conn;
        }

        // 🔹 Optional: test connection
        public static bool TestConnection()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
