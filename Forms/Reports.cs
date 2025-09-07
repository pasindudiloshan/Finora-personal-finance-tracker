using FinoraTracker.Controllers;
using FinoraTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

#nullable enable

namespace FinoraTracker.Forms
{
    public partial class Reports : Form
    {
        private User currentUser;
        private readonly IncomeController incomeController;
        private readonly ExpenseController expenseController;
        private List<Income> allIncomes;
        private List<Expense> allExpenses;

        public Reports(User user)
        {
            InitializeComponent();
            currentUser = user;

            incomeController = new IncomeController();
            expenseController = new ExpenseController();

            label7.Text = currentUser.FullName;
            label8.Text = currentUser.Email;
            label1.Text = $"{currentUser.FullName}'s Reports";

            allIncomes = incomeController.GetIncomeByUser(currentUser.UserId);
            allExpenses = expenseController.GetExpensesByUser(currentUser.UserId);

            // Ensure button is wired
            datefilterbtn.Click += Datefilterbtn_Click;

            LoadDefaultReports();
        }

        // -------------------- Navigation --------------------
        private void dashboardbtn_Click(object sender, EventArgs e)
        {
            new Dashboard(currentUser).Show();
            this.Hide();
        }

        private void incomebtn_Click(object sender, EventArgs e)
        {
            new IncomeForm(currentUser).Show();
            this.Hide();
        }

        private void expensesbtn_Click(object sender, EventArgs e)
        {
            new Expenses(currentUser).Show();
            this.Hide();
        }

        private void logoutbtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void reportbtn_Click(object sender, EventArgs e)
        {
            new Reports(currentUser).Show();
            this.Hide();
        }

        // -------------------- Default Load --------------------
        private void LoadDefaultReports()
        {
            DateTime start = DateTime.Now.AddDays(-30).Date;
            DateTime end = DateTime.Now.Date.AddDays(1).AddTicks(-1);

            FilterAndUpdateReports(start, end);
        }

        // -------------------- Date Filter --------------------
        private void Datefilterbtn_Click(object? sender, EventArgs e)
        {
            DateTime start = dateTimePicker1.Value.Date;
            DateTime end = dateTimePicker3.Value.Date.AddDays(1).AddTicks(-1);

            if (start > end)
            {
                MessageBox.Show("Start date cannot be after End date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FilterAndUpdateReports(start, end);
        }

        // -------------------- Filter & Update --------------------
        private void FilterAndUpdateReports(DateTime start, DateTime end)
        {
            // Filter incomes & expenses
            var filteredIncomes = allIncomes
                .Where(i => i.IncomeDate >= start && i.IncomeDate <= end)
                .ToList();

            var filteredExpenses = allExpenses
                .Where(e => e.ExpenseDate >= start && e.ExpenseDate <= end)
                .ToList();

            // Update charts, grids, stats
            UpdateCharts(filteredIncomes, filteredExpenses);
            UpdateDataGrids(filteredIncomes, filteredExpenses);
            LoadStatCards(filteredIncomes, filteredExpenses);
        }

        // -------------------- Update Charts --------------------
        private void UpdateCharts(List<Income> incomes, List<Expense> expenses)
        {
            // -------- chart1: Income vs Expenses --------
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Legends.Clear();

            ChartArea chartArea = new ChartArea("ChartArea1");
            chart1.ChartAreas.Add(chartArea);

            Legend legend = new Legend("Legend1") { Docking = Docking.Top };
            chart1.Legends.Add(legend);

            Series incomeSeries = new Series("Income")
            {
                ChartType = SeriesChartType.Spline,
                Color = System.Drawing.Color.Green,
                ChartArea = "ChartArea1",
                Legend = "Legend1"
            };

            Series expenseSeries = new Series("Expenses")
            {
                ChartType = SeriesChartType.Spline,
                Color = System.Drawing.Color.Red,
                ChartArea = "ChartArea1",
                Legend = "Legend1"
            };

            var dates = incomes.Select(i => i.IncomeDate.Date)
                               .Union(expenses.Select(e => e.ExpenseDate.Date))
                               .Distinct().OrderBy(d => d);

            foreach (var date in dates)
            {
                decimal incomeSum = incomes.Where(i => i.IncomeDate.Date == date).Sum(i => i.Amount);
                decimal expenseSum = expenses.Where(e => e.ExpenseDate.Date == date).Sum(e => e.Amount);

                incomeSeries.Points.AddXY(date, (double)incomeSum);
                expenseSeries.Points.AddXY(date, (double)expenseSum);
            }

            chart1.Series.Add(incomeSeries);
            chart1.Series.Add(expenseSeries);

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM-dd";
            chart1.ChartAreas[0].AxisX.Interval = 2;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart1.Titles.Clear();

            // -------- chart2: Expense Categories Pie --------
            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            chart2.Legends.Clear();

            ChartArea chartArea2 = new ChartArea("ChartArea2");
            chart2.ChartAreas.Add(chartArea2);

            Legend legend2 = new Legend("Legend2") { Docking = Docking.Right };
            chart2.Legends.Add(legend2);

            Series series2 = new Series("Expenses")
            {
                ChartType = SeriesChartType.Pie,
                Legend = "Legend2"
            };

            var groupedExpenses = expenses.GroupBy(e => e.Category)
                                          .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) });

            foreach (var item in groupedExpenses)
                series2.Points.AddXY(item.Category, (double)item.Total);

            series2["PieLabelStyle"] = "Disabled";
            series2["PieStartAngle"] = "270";
            chart2.Series.Add(series2);
            chart2.Palette = ChartColorPalette.SemiTransparent;
            chart2.Titles.Clear();

            // -------- chart3: Income Categories Doughnut --------
            chart3.Series.Clear();
            chart3.ChartAreas.Clear();
            chart3.Legends.Clear();

            ChartArea chartArea3 = new ChartArea("ChartArea3");
            chart3.ChartAreas.Add(chartArea3);

            Legend legend3 = new Legend("Legend3") { Docking = Docking.Right };
            chart3.Legends.Add(legend3);

            Series series3 = new Series("Income")
            {
                ChartType = SeriesChartType.Doughnut,
                Legend = "Legend3"
            };

            var groupedIncomes = incomes.GroupBy(i => i.Category)
                                        .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) });

            foreach (var item in groupedIncomes)
                series3.Points.AddXY(item.Category, (double)item.Total);

            series3["PieLabelStyle"] = "Disabled";
            chart3.Series.Add(series3);
            chart3.Palette = ChartColorPalette.SemiTransparent;
            chart3.Titles.Clear();
        }

        // -------------------- Update DataGrids --------------------
        private void UpdateDataGrids(List<Income> incomes, List<Expense> expenses)
        {
            void StyleDataGrid(DataGridView dgv)
            {
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SeaGreen;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9);
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.RowTemplate.Height = 35;
            }

            // Income Grid
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Amount",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "N2" }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Date",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "yyyy-MM-dd" }
            });

            foreach (var inc in incomes.OrderByDescending(i => i.Amount))
                dataGridView1.Rows.Add(inc.Amount, inc.Description, inc.IncomeDate.ToString("yyyy-MM-dd"));

            StyleDataGrid(dataGridView1);
            dataGridView1.AutoResizeColumns();

            // Expense Grid
            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.AutoGenerateColumns = false;

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Amount",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "N2" }
            });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Date",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "yyyy-MM-dd" }
            });

            foreach (var exp in expenses.OrderByDescending(e => e.Amount))
                dataGridView2.Rows.Add(exp.Amount, exp.Description, exp.ExpenseDate.ToString("yyyy-MM-dd"));

            StyleDataGrid(dataGridView2);
            dataGridView2.AutoResizeColumns();
        }

        // -------------------- Stat Cards --------------------
        private string FormatAmount(decimal amount)
        {
            if (amount >= 1_000_000)
                return $"{amount / 1_000_000:F1}M LKR";
            else if (amount >= 1_000)
                return $"{amount / 1_000:F0}K LKR";
            else
                return $"{amount:N0} LKR";
        }

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

        private void LoadStatCards(List<Income> incomes, List<Expense> expenses)
        {
            decimal totalIncome = incomes.Sum(i => i.Amount);
            decimal totalExpenses = expenses.Sum(e => e.Amount);
            decimal netWealth = totalIncome - totalExpenses;

            label6.Text = FormatAmount(netWealth);
            label6.ForeColor = netWealth >= 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            if (incomes.Count > 0 || expenses.Count > 0)
            {
                DateTime startDate = incomes.Concat<Income>(expenses.Select(e => new Income { IncomeDate = e.ExpenseDate, Amount = 0 })).Min(i => i.IncomeDate);
                DateTime endDate = incomes.Concat<Income>(expenses.Select(e => new Income { IncomeDate = e.ExpenseDate, Amount = 0 })).Max(i => i.IncomeDate);

                int days = (endDate - startDate).Days + 1;
                DateTime prevStart = startDate.AddDays(-days);
                DateTime prevEnd = startDate.AddDays(-1);

                decimal prevIncome = allIncomes.Where(i => i.IncomeDate >= prevStart && i.IncomeDate <= prevEnd).Sum(i => i.Amount);
                decimal prevExpenses = allExpenses.Where(e => e.ExpenseDate >= prevStart && e.ExpenseDate <= prevEnd).Sum(e => e.Amount);
                decimal prevNetWealth = prevIncome - prevExpenses;

                decimal growthPercent = prevNetWealth == 0 ? 100 : ((netWealth - prevNetWealth) / prevNetWealth) * 100;
                SetChangeLabel(label9, growthPercent);
            }
            else
            {
                label9.Text = "No data";
                label9.BackColor = System.Drawing.Color.Gray;
            }

            label14.Text = FormatAmount(totalIncome);
            label14.ForeColor = System.Drawing.Color.Green;

            label10.Text = FormatAmount(totalExpenses);
            label10.ForeColor = System.Drawing.Color.Red;

            UpdateTargetAchievement(totalIncome, 5_000_000M);
        }

        private void UpdateTargetAchievement(decimal totalIncome, decimal targetAmount)
        {
            decimal remaining = targetAmount - totalIncome;

            if (remaining <= 0)
                label16.Text = $"Target Achieved! ({FormatAmount(totalIncome)})";
            else
                label16.Text = $"{FormatAmount(remaining)}";

            label16.ForeColor = System.Drawing.Color.OrangeRed;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
    }
}
