using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinoraTracker.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public string UserId { get; set; } = "";
        public decimal Amount { get; set; }
        public string Category { get; set; } = "";
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; } = "";
        public string PaymentMethod { get; set; } = ""; // Updated
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
