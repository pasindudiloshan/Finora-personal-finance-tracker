using System;
using System.Collections.Generic;
using System.Linq;
using FinoraTracker.Models;
using FinoraTracker.DAOs;

namespace FinoraTracker.Controllers
{
    public class ExpenseController
    {
        private readonly ExpenseDAO expenseDAO;

        public ExpenseController()
        {
            expenseDAO = new ExpenseDAO();
        }

        // 🔹 Add new expense
        public bool AddExpense(Expense expense)
        {
            if (expense.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(expense.Category))
                throw new Exception("Select a category.");
            if (expense.ExpenseDate == default)
                throw new Exception("Select a valid date.");
            if (string.IsNullOrWhiteSpace(expense.PaymentMethod))
                throw new Exception("Select a payment method.");

            return expenseDAO.AddExpense(expense);
        }

        // 🔹 Get all expenses by user
        public List<Expense> GetExpensesByUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("User ID cannot be empty.");
            return expenseDAO.GetExpensesByUser(userId);
        }

        // 🔹 Delete expense
        public bool DeleteExpense(int expenseId)
        {
            if (expenseId <= 0)
                throw new Exception("Invalid expense ID.");
            return expenseDAO.DeleteExpense(expenseId);
        }

        // 🔹 Update expense
        public bool UpdateExpense(Expense expense)
        {
            if (expense.ExpenseId <= 0)
                throw new Exception("Invalid expense ID.");
            if (expense.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(expense.Category))
                throw new Exception("Select a category.");
            if (expense.ExpenseDate == default)
                throw new Exception("Select a valid date.");
            if (string.IsNullOrWhiteSpace(expense.PaymentMethod))
                throw new Exception("Select a payment method.");

            return expenseDAO.UpdateExpense(expense);
        }

        // 🔹 Get last N days expenses
        public List<Expense> GetRecentExpenses(string userId, int days = 30)
        {
            var expenses = GetExpensesByUser(userId);
            return expenses.Where(e => e.ExpenseDate >= DateTime.Now.AddDays(-days)).ToList();
        }

        // 🔹 Chart: Total expenses by category
        public Dictionary<string, decimal> GetExpensesByCategory(string userId, int days = 30)
        {
            var expenses = GetRecentExpenses(userId, days);
            return expenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        }

        // 🔹 Chart: Total expenses by payment method
        public Dictionary<string, decimal> GetExpensesByPaymentMethod(string userId, int days = 30)
        {
            var expenses = GetRecentExpenses(userId, days);
            return expenses
                .GroupBy(e => e.PaymentMethod)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        }
    }
}
