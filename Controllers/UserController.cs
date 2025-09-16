using FinoraTracker.DAOs;
using FinoraTracker.Models;
using System.Text.RegularExpressions;

namespace FinoraTracker.Controllers
{
    public class UserController
    {
        private readonly UserDAO userDAO;

        public UserController()
        {
            var connectionProvider = new DefaultDBConnectionProvider();
            var commandFactory = new DefaultCommandFactory();
            userDAO = new UserDAO(connectionProvider, commandFactory);
        }

        public bool Register(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new Exception("Full name required.");

            if (!IsValidEmail(user.Email))
                throw new Exception("Invalid email.");

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8)
                throw new Exception("Password min 8 chars.");

            return userDAO.RegisterUser(user);
        }

        public User? Login(string email, string password)
        {
            if (!IsValidEmail(email))
                throw new Exception("Invalid email.");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password required.");

            return userDAO.Login(email, password);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
