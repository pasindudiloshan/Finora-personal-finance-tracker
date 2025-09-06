using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using FinoraTracker.Models;
using FinoraTracker.Controllers;

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
            DateTime start = DateTime.Now.AddDays(-30);
            DateTime end = DateTime.Now;

            var filteredIncomes = allIncomes
                .Where(i => i.IncomeDate.Date >= start && i.IncomeDate.Date <= end)
                .ToList();

            var filteredExpenses = allExpenses
                .Where(e => e.ExpenseDate.Date >= start && e.ExpenseDate.Date <= end)
                .ToList();

            UpdateCharts(filteredIncomes, filteredExpenses);
            UpdateDataGrids(filteredIncomes, filteredExpenses);
        }

        // -------------------- Filter Button --------------------
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime start = dateTimePicker1.Value.Date;
            DateTime end = dateTimePicker3.Value.Date;

            var filteredIncomes = allIncomes
                .Where(i => i.IncomeDate.Date >= start && i.IncomeDate.Date <= end)
                .ToList();

            var filteredExpenses = allExpenses
                .Where(e => e.ExpenseDate.Date >= start && e.ExpenseDate.Date <= end)
                .ToList();

            UpdateCharts(filteredIncomes, filteredExpenses);
            UpdateDataGrids(filteredIncomes, filteredExpenses);
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

            Legend legend = new Legend("Legend1");
            legend.Docking = Docking.Top;
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
                decimal expenseSum = expenses.Where(e => e.ExpenseDate.Date == date).Sum(i => i.Amount);

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

            // -------- Income Grid --------
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

            // -------- Expense Grid --------
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
    }
}
