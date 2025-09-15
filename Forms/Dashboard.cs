using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using FinoraTracker.Models;
using FinoraTracker.Controllers;

namespace FinoraTracker.Forms
{
    public partial class Dashboard : Form
    {
        private User currentUser;
        private readonly IncomeController incomeController;
        private readonly ExpenseController expenseController;

        public Dashboard(User user)
        {
            InitializeComponent();
            currentUser = user;

            incomeController = new IncomeController();
            expenseController = new ExpenseController();

            // Display welcome message
            welcomeLabel.Text = $"Hi!, {currentUser.FullName}";
            label8.Text = $"{currentUser.Email}";
            label7.Text = $"{currentUser.FullName}";

            // Load charts
            LoadLineChart();
            LoadExpenseCategoryPieChart();

            // Load stat cards
            LoadStatCards();
        }

        // ---------------- Load Line Chart ----------------
        private void LoadLineChart()
        {
            try
            {
                var incomes = incomeController.GetIncomeByUser(currentUser.UserId)
                    .Where(i => i.IncomeDate >= DateTime.Now.AddDays(-30)).ToList();

                var expenses = expenseController.GetExpensesByUser(currentUser.UserId)
                    .Where(e => e.ExpenseDate >= DateTime.Now.AddDays(-30)).ToList();

                chart1.Series.Clear();
                chart1.ChartAreas.Clear();
                chart1.Legends.Clear();

                ChartArea chartArea = new ChartArea("ChartArea1");
                chart1.ChartAreas.Add(chartArea);

                Legend legend = new Legend("Legend1");
                chart1.Legends.Add(legend);

                Series incomeSeries = new Series("Income")
                {
                    ChartType = SeriesChartType.Line,
                    ChartArea = "ChartArea1",
                    BorderWidth = 3,
                    Color = System.Drawing.Color.Green
                };

                Series expenseSeries = new Series("Expenses")
                {
                    ChartType = SeriesChartType.Line,
                    ChartArea = "ChartArea1",
                    BorderWidth = 3,
                    Color = System.Drawing.Color.Red
                };

                var incomeGrouped = incomes
                    .GroupBy(i => i.IncomeDate.Date)
                    .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Amount) })
                    .OrderBy(x => x.Date);

                foreach (var item in incomeGrouped)
                    incomeSeries.Points.AddXY(item.Date, (double)item.Total);

                var expenseGrouped = expenses
                    .GroupBy(e => e.ExpenseDate.Date)
                    .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Amount) })
                    .OrderBy(x => x.Date);

                foreach (var item in expenseGrouped)
                    expenseSeries.Points.AddXY(item.Date, (double)item.Total);

                chart1.Series.Add(incomeSeries);
                chart1.Series.Add(expenseSeries);

                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM-dd";
                chart1.ChartAreas[0].AxisX.Interval = 2;
                chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;

                chart1.Titles.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load line chart data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------- Load Pie Chart ----------------
        private void LoadExpenseCategoryPieChart()
        {
            try
            {
                var expenses = expenseController.GetExpensesByUser(currentUser.UserId)
                    .Where(e => e.ExpenseDate >= DateTime.Now.AddDays(-30)).ToList();

                var categoryGroups = expenses
                    .GroupBy(e => e.Category)
                    .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                    .ToList();

                chart2.Series.Clear();
                chart2.ChartAreas.Clear();
                chart2.Legends.Clear();

                ChartArea chartArea2 = new ChartArea("ChartArea1");
                chart2.ChartAreas.Add(chartArea2);

                Legend legend2 = new Legend("Legend1");
                chart2.Legends.Add(legend2);

                Series series2 = new Series("Expenses")
                {
                    ChartType = SeriesChartType.Pie,
                    ChartArea = "ChartArea1",
                    Legend = "Legend1"
                };

                // Disable labels inside the pie chart
                series2["PieLabelStyle"] = "Disabled";

                foreach (var item in categoryGroups)
                    series2.Points.AddXY(item.Category, (double)item.Total);

                chart2.Series.Add(series2);
                chart2.Palette = ChartColorPalette.SemiTransparent;

                chart2.Titles.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load pie chart data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------- Load Stat Cards ----------------
        // Helper to format large numbers
        private string FormatAmount(decimal amount)
        {
            if (amount >= 1_000_000)
                return $"{amount / 1_000_000:F1}M LKR";
            else if (amount >= 1_000)
                return $"{amount / 1_000:F0}K LKR";
            else
                return $"{amount:N0} LKR";
        }

        // Helper to set label color and arrow based on percentage
        private void SetChangeLabel(Label label, decimal change)
        {
            if (change >= 0)
            {
                label.BackColor = System.Drawing.Color.SeaGreen;
                label.Text = $"⬆ {change:F1}%";
            }
            else
            {
                label.BackColor = System.Drawing.Color.Red;
                label.Text = $"⬇ {Math.Abs(change):F1}%";
            }
        }

        // ---------------- Load Stat Cards ----------------
        private void LoadStatCards()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            // Daily Income
            var todayIncome = incomeController.GetIncomeByUser(currentUser.UserId)
                .Where(i => i.IncomeDate.Date == today).Sum(i => i.Amount);

            var yesterdayIncome = incomeController.GetIncomeByUser(currentUser.UserId)
                .Where(i => i.IncomeDate.Date == yesterday).Sum(i => i.Amount);

            var incomeChange = yesterdayIncome == 0 ? 100 : ((todayIncome - yesterdayIncome) / yesterdayIncome) * 100;

            label17.Text = FormatAmount(todayIncome);       // Amount
            SetChangeLabel(label16, incomeChange);          // % change + color + arrow

            // Daily Expenses
            var todayExpenses = expenseController.GetExpensesByUser(currentUser.UserId)
                .Where(e => e.ExpenseDate.Date == today).Sum(e => e.Amount);

            var yesterdayExpenses = expenseController.GetExpensesByUser(currentUser.UserId)
                .Where(e => e.ExpenseDate.Date == yesterday).Sum(e => e.Amount);

            var expenseChange = yesterdayExpenses == 0 ? 100 : ((todayExpenses - yesterdayExpenses) / yesterdayExpenses) * 100;

            label20.Text = FormatAmount(todayExpenses);       // Amount
            SetChangeLabel(label19, expenseChange);           // % change + color + arrow

            // Net Worth (Total Income - Total Expenses)
            var totalIncome = incomeController.GetIncomeByUser(currentUser.UserId).Sum(i => i.Amount);
            var totalExpenses = expenseController.GetExpensesByUser(currentUser.UserId).Sum(e => e.Amount);

            var netWorth = totalIncome - totalExpenses;

            // For percentage change, compare to yesterday's net worth
            var yesterdayNetWorth = incomeController.GetIncomeByUser(currentUser.UserId)
                .Where(i => i.IncomeDate.Date <= yesterday).Sum(i => i.Amount)
                - expenseController.GetExpensesByUser(currentUser.UserId)
                .Where(e => e.ExpenseDate.Date <= yesterday).Sum(e => e.Amount);

            var netWorthChange = yesterdayNetWorth == 0 ? 100 : ((netWorth - yesterdayNetWorth) / yesterdayNetWorth) * 100;

            label14.Text = FormatAmount(netWorth);
            SetChangeLabel(label15, netWorthChange);
        }

        // ---------------- Navigation ----------------
        private void logoutbtn_Click(object sender, EventArgs e) => Application.Exit();

        private void dashboardbtn_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard(this.currentUser);
            dashboard.Show();
            this.Close();
        }

        private void incomebtn_Click(object sender, EventArgs e)
        {
            IncomeForm incomeForm = new IncomeForm(this.currentUser);
            incomeForm.Show();
            this.Hide();
        }

        private void expensesbtn_Click(object sender, EventArgs e)
        {
            Expenses expenseForm = new Expenses(this.currentUser);
            expenseForm.Show();
            this.Hide();
        }

        private void reportbtn_Click(object sender, EventArgs e)
        {
            Reports reportsForm = new Reports(currentUser);
            reportsForm.Show();
            this.Hide();
        }

        private void portfoliobtn_Click(object sender, EventArgs e)
        {
            Portfolios portfolioForm = new Portfolios(currentUser);
            portfolioForm.Show();
            this.Hide();
        }

        private void investmentbtn_Click(object sender, EventArgs e)
        {
            Investments InvestmentsForm = new Investments(currentUser);
            InvestmentsForm.Show();
            this.Hide();
        }
    }
}
