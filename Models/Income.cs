using System;

namespace FinoraTracker.Models
{
    public class Income
    {
        public int IncomeId { get; set; }        // Auto-increment INT in DB
        public string UserId { get; set; } = ""; // Matches User.UserId
        public decimal Amount { get; set; }
        public string Category { get; set; } = "";
        public DateTime IncomeDate { get; set; }
        public string Description { get; set; } = "";
        public string AccountSource { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
