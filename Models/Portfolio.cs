using System;

namespace FinoraTracker.Models
{
    public class Portfolio
    {
        public int PortfolioId { get; set; }       // Auto-increment ID
        public required string UserId { get; set; }         // Linked to Users.UserId
        public required string CompanyName { get; set; }    // Company name
        public int Shares { get; set; }            // Number of shares
        public decimal SharePrice { get; set; }    // Current price per share
        public decimal Value { get; set; }         // (Shares * SharePrice)
        public decimal? PERatio { get; set; }      // Nullable: P/E ratio
        public decimal? TargetPrice { get; set; }  // Target price
        public decimal TargetValue { get; set; }   // (Shares * TargetPrice)
        public decimal GainPercent { get; set; }   // ((TargetValue - Value) / Value) * 100
        public DateTime CreatedAt { get; set; }    // Timestamp
    }
}
