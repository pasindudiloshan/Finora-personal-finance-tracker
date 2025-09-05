namespace FinoraTracker.Models
{
    public class User
    {
        public string UserId { get; set; } = string.Empty; 
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string IncomeFrequency { get; set; } = string.Empty;
        public string InvestmentInterest { get; set; } = string.Empty;
        public string HowDidYouKnow { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
