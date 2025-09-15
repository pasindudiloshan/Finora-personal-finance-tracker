using System;
using System.Collections.Generic;
using FinoraTracker.Models;
using FinoraTracker.DAOs;

namespace FinoraTracker.Controllers
{
    public class PortfolioController
    {
        private readonly PortfolioDAO portfolioDAO = new PortfolioDAO();

        public bool AddPortfolio(Portfolio portfolio)
        {
            if (string.IsNullOrWhiteSpace(portfolio.CompanyName))
                throw new Exception("Company name is required");
            if (portfolio.Shares <= 0)
                throw new Exception("Number of shares must be greater than 0");
            if (portfolio.SharePrice <= 0)
                throw new Exception("Share price must be greater than 0");

            portfolio.Value = portfolio.Shares * portfolio.SharePrice;
            portfolio.TargetValue = portfolio.TargetPrice.HasValue
                ? portfolio.Shares * portfolio.TargetPrice.Value
                : 0;

            portfolio.GainPercent = portfolio.TargetPrice.HasValue && portfolio.Value > 0
                ? ((portfolio.TargetValue - portfolio.Value) / portfolio.Value) * 100
                : 0;

            portfolio.CreatedAt = DateTime.Now;

            return portfolioDAO.AddPortfolio(portfolio);
        }

        public List<Portfolio> GetPortfolios(string userId) => portfolioDAO.GetPortfolioByUser(userId);

        public bool DeletePortfolio(int portfolioId) => portfolioDAO.DeletePortfolio(portfolioId);

        public bool UpdatePortfolio(Portfolio portfolio) => portfolioDAO.UpdatePortfolio(portfolio);

        public decimal GetTotalInvestment(string userId)
        {
            decimal total = 0;
            foreach (var p in portfolioDAO.GetPortfolioByUser(userId))
                total += p.Value;
            return total;
        }
    }
}
