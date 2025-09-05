using System;
using System.Collections.Generic;
using FinoraTracker.Models;
using FinoraTracker.DAOs;

namespace FinoraTracker.Controllers
{
    public class IncomeController
    {
        private readonly IncomeDAO incomeDAO;

        public IncomeController()
        {
            incomeDAO = new IncomeDAO();
        }

        // 🔹 Add a new income record with validation
        public bool AddIncome(Income income)
        {
            // Validate amount
            if (income.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");

            // Validate category
            if (string.IsNullOrWhiteSpace(income.Category))
                throw new Exception("Select a category.");

            // Validate date
            if (income.IncomeDate == default)
                throw new Exception("Select a valid date.");

            // Validate account/source
            if (string.IsNullOrWhiteSpace(income.AccountSource))
                throw new Exception("Select account/source.");

            // Call DAO to save to database
            return incomeDAO.AddIncome(income);
        }

        // 🔹 Get all incomes for a specific user
        public List<Income> GetIncomeByUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("User ID cannot be empty.");

            return incomeDAO.GetIncomeByUser(userId);
        }
    }
}
