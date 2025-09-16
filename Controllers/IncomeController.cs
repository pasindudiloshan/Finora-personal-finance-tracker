using System;
using System.Collections.Generic;
using System.Linq;
using FinoraTracker.Models;
using FinoraTracker.DAOs; // Use DAO namespace
using MySql.Data.MySqlClient;

namespace FinoraTracker.Controllers
{
    public class IncomeController
    {
        private readonly IncomeDAO incomeDAO;

        // 🔹 Default constructor for Forms / legacy code
        public IncomeController()
            : this(new DBConnectionProvider(), new CommandFactory())
        { }

        // 🔹 Constructor with DI for unit tests
        public IncomeController(IDBConnectionProvider connectionProvider, ICommandFactory commandFactory)
        {
            incomeDAO = new IncomeDAO(connectionProvider, commandFactory);
        }

        // 🔹 Add new income
        public bool AddIncome(Income income)
        {
            if (income.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(income.Category))
                throw new Exception("Select a category.");
            if (income.IncomeDate == default)
                throw new Exception("Select a valid date.");
            if (string.IsNullOrWhiteSpace(income.AccountSource))
                throw new Exception("Select account/source.");

            return incomeDAO.AddIncome(income);
        }

        // 🔹 Get all incomes by user
        public List<Income> GetIncomeByUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("User ID cannot be empty.");
            return incomeDAO.GetIncomeByUser(userId);
        }

        // 🔹 Delete income
        public bool DeleteIncome(int incomeId)
        {
            if (incomeId <= 0)
                throw new Exception("Invalid income ID.");
            return incomeDAO.DeleteIncome(incomeId);
        }

        // 🔹 Update income
        public bool UpdateIncome(Income income)
        {
            if (income.IncomeId <= 0)
                throw new Exception("Invalid income ID.");
            if (income.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(income.Category))
                throw new Exception("Select a category.");
            if (income.IncomeDate == default)
                throw new Exception("Select a valid date.");
            if (string.IsNullOrWhiteSpace(income.AccountSource))
                throw new Exception("Select account/source.");

            return incomeDAO.UpdateIncome(income);
        }

        // 🔹 Get last N days income
        public List<Income> GetRecentIncome(string userId, int days = 30)
        {
            var incomes = GetIncomeByUser(userId);
            return incomes.Where(i => i.IncomeDate >= DateTime.Now.AddDays(-days)).ToList();
        }

        // 🔹 Chart: Total income by category
        public Dictionary<string, decimal> GetIncomeByCategory(string userId, int days = 30)
        {
            var incomes = GetRecentIncome(userId, days);
            return incomes
                .GroupBy(i => i.Category)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Amount));
        }

        // 🔹 Chart: Total income by account source
        public Dictionary<string, decimal> GetIncomeByAccountSource(string userId, int days = 30)
        {
            var incomes = GetRecentIncome(userId, days);
            return incomes
                .GroupBy(i => i.AccountSource)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Amount));
        }
    }
}
