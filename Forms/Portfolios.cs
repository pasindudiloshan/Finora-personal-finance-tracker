using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FinoraTracker.Models;
using FinoraTracker.Controllers;

namespace FinoraTracker.Forms
{
    public partial class Portfolios : Form
    {
        private User currentUser;
        private readonly PortfolioController portfolioController;

        public Portfolios(User user)
        {
            InitializeComponent();
            currentUser = user;
            portfolioController = new PortfolioController();

            InitializeDataGridView(); // Setup columns
            LoadPortfolio();          // Load data and total investment
        }

        // -----------------------------
        // DataGridView Setup
        // -----------------------------
        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // PortfolioId (hidden)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PortfolioId",
                HeaderText = "ID",
                Visible = false
            });

            // Company Name
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CompanyName",
                HeaderText = "Company Name",
                Width = 150
            });

            // Shares
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Shares",
                HeaderText = "Shares",
                Width = 70
            });

            // Share Price
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SharePrice",
                HeaderText = "Share Price (LKR)",
                Width = 90
            });

            // Value
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Value",
                HeaderText = "Value (LKR)",
                Width = 100
            });

            // Target Price
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TargetPrice",
                HeaderText = "Target Price (LKR)",
                Width = 90
            });

            // Target Value
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TargetValue",
                HeaderText = "Target Value (LKR)",
                Width = 100
            });

            // P/E Ratio
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PERatio",
                HeaderText = "P/E Ratio",
                Width = 70
            });

            // Gain %
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GainPercent",
                HeaderText = "Gain %",
                Width = 70
            });

            // Edit Button
            dataGridView1.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Edit",
                HeaderText = "Edit",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                Width = 60
            });

            // Delete Button
            dataGridView1.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                Width = 60
            });

            // Fill all remaining width
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // -----------------------------
        // Load Portfolio and update total investment
        // -----------------------------
        private void LoadPortfolio()
        {
            var portfolios = portfolioController.GetPortfolios(currentUser.UserId);
            dataGridView1.Rows.Clear();

            foreach (var p in portfolios)
            {
                dataGridView1.Rows.Add(
                    p.PortfolioId,
                    p.CompanyName ?? string.Empty,
                    p.Shares,
                    p.SharePrice.ToString("N2"),
                    p.Value.ToString("N2"),
                    (p.TargetPrice ?? 0).ToString("N2"),
                    p.TargetValue.ToString("N2"),
                    (p.PERatio ?? 0).ToString("0.##"),
                    $"{p.GainPercent:0.##} %"
                );
            }

            // Update total investment label automatically
            UpdateTotalInvestmentLabel();
        }

        // -----------------------------
        // Update total investment label
        // -----------------------------
        private void UpdateTotalInvestmentLabel()
        {
            decimal total = portfolioController.GetTotalInvestment(currentUser.UserId);
            totalinvest.Text = $" {total:N2} LKR";
        }

        // -----------------------------
        // Add Portfolio Entry
        // -----------------------------
        private void addbtn_Click(object sender, EventArgs e)
        {
            try
            {
                var portfolio = new Portfolio
                {
                    UserId = currentUser.UserId,
                    CompanyName = comnameBox1.Text.Trim(),
                    Shares = (int)nosharebox1.Value,
                    SharePrice = decimal.Parse(sharepricebox.Text),
                    PERatio = string.IsNullOrWhiteSpace(pebox.Text) ? (decimal?)null : decimal.Parse(pebox.Text),
                    TargetPrice = string.IsNullOrWhiteSpace(targetpricebox.Text) ? (decimal?)null : decimal.Parse(targetpricebox.Text)
                };

                portfolioController.AddPortfolio(portfolio);
                MessageBox.Show("Portfolio entry added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload data and update total investment automatically
                LoadPortfolio();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -----------------------------
        // Edit/Delete Row
        // -----------------------------
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dataGridView1.Rows[e.RowIndex];

            var idValue = row.Cells["PortfolioId"].Value;
            if (idValue == null) return;
            int portfolioId = Convert.ToInt32(idValue);

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                comnameBox1.Text = row.Cells["CompanyName"].Value?.ToString() ?? "";
                nosharebox1.Value = Convert.ToDecimal(row.Cells["Shares"].Value ?? 0);
                sharepricebox.Text = row.Cells["SharePrice"].Value?.ToString() ?? "";
                targetpricebox.Text = row.Cells["TargetPrice"].Value?.ToString() ?? "";
                pebox.Text = row.Cells["PERatio"].Value?.ToString() ?? "";
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this entry?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    portfolioController.DeletePortfolio(portfolioId);
                    LoadPortfolio();
                }
            }
        }


        // -----------------------------
        // Navigation Buttons
        // -----------------------------


        private void dashboardbtn_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard(currentUser);
            dashboard.Show();
            this.Hide();
        }

        private void incomebtn_Click(object sender, EventArgs e)
        {
            IncomeForm incomeForm = new IncomeForm(currentUser);
            incomeForm.Show();
            this.Hide();
        }

        private void expensesbtn_Click(object sender, EventArgs e)
        {
            Expenses expenseForm = new Expenses(currentUser);
            expenseForm.Show();
            this.Hide();

        }

        private void reportbtn_Click(object sender, EventArgs e)
        {
            Reports reportsForm = new Reports(currentUser);
            reportsForm.Show();
            this.Hide();
        }

        private void logoutbtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void portfoliobtn_Click(object sender, EventArgs e)
        {
            LoadPortfolio(); // Refresh data
        }
    }
}
