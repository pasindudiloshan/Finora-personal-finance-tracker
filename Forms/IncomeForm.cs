using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FinoraTracker.Models;
using FinoraTracker.Controllers;

namespace FinoraTracker.Forms
{
    public partial class IncomeForm : Form
    {
        private User currentUser;
        private readonly IncomeController incomeController;
        private List<Income> userIncomes = new List<Income>();

        public IncomeForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            incomeController = new IncomeController();

            label7.Text = currentUser.FullName;
            label8.Text = currentUser.Email;

            dataGridexpens.AutoGenerateColumns = false;
            InitializeDataGridColumns();
            InitializeCategoryCombo();

            LoadUserIncomes();
            UpdateIncomeSummary(); // Show last 30 days income
        }

        // -------------------- DataGrid Columns --------------------
        private void InitializeDataGridColumns()
        {
            dataGridexpens.Columns.Clear();

            dataGridexpens.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Amount",
                DataPropertyName = "Amount",
                Width = 100
            });
            dataGridexpens.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Category",
                DataPropertyName = "Category",
                Width = 120
            });
            dataGridexpens.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Date",
                DataPropertyName = "IncomeDate",
                Width = 100,
                DefaultCellStyle = { Format = "yyyy-MM-dd" }
            });
            dataGridexpens.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Account Source",
                DataPropertyName = "AccountSource",
                Width = 120
            });
            dataGridexpens.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200
            });

            dataGridexpens.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Edit",
                Text = "Edit",
                UseColumnTextForButtonValue = true
            });
            dataGridexpens.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            });
        }

        // -------------------- Populate Category ComboBox --------------------
        private void InitializeCategoryCombo()
        {
            serchbox.Items.Clear();
            serchbox.Items.AddRange(new string[]
            {
                "Salary",
                "Business Profits",
                "Freelance",
                "Investment",
                "Dividends",
                "Rental Income",
                "Gifts / Donations",
                "Bonus / Incentives"
            });
            serchbox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // -------------------- Load User Incomes --------------------
        private void LoadUserIncomes()
        {
            try
            {
                userIncomes = incomeController.GetIncomeByUser(currentUser.UserId);
                dataGridexpens.DataSource = userIncomes.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load incomes: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------------------- Update Last 30 Days Income Summary --------------------
        private void UpdateIncomeSummary()
        {
            try
            {
                var last30DaysIncomes = userIncomes
                    .Where(i => i.IncomeDate >= DateTime.Now.AddDays(-30))
                    .ToList();

                decimal totalLast30Days = last30DaysIncomes.Sum(i => i.Amount);

                // Display exact amount with commas
                textBox1.Text = $"Last 30 days income: {totalLast30Days:N0} LKR";
            }
            catch
            {
                textBox1.Text = "Last 30 days income: 0 LKR";
            }
        }

        // -------------------- DataGrid Cell Click --------------------
        private void dataGridincome_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Income selectedIncome = userIncomes[e.RowIndex];

            if (dataGridexpens.Columns[e.ColumnIndex].HeaderText == "Edit")
            {
                Addincome editForm = new Addincome(currentUser, selectedIncome);
                editForm.FormClosed += (s, args) => { LoadUserIncomes(); UpdateIncomeSummary(); };
                editForm.Show();
            }
            else if (dataGridexpens.Columns[e.ColumnIndex].HeaderText == "Delete")
            {
                var confirm = MessageBox.Show("Are you sure you want to delete this income?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        incomeController.DeleteIncome(selectedIncome.IncomeId);
                        LoadUserIncomes();
                        UpdateIncomeSummary();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Delete failed: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // -------------------- Add Income Page --------------------
        private void addincomepage_Click(object sender, EventArgs e)
        {
            Addincome addForm = new Addincome(currentUser);
            addForm.FormClosed += (s, args) => { LoadUserIncomes(); UpdateIncomeSummary(); };
            addForm.Show();
        }

        // -------------------- Navigation --------------------
        private void dashboardbtn_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard(currentUser);
            dashboard.Show();
            this.Hide();
        }

        private void expensesbtn_Click(object sender, EventArgs e)
        {
            Expenses expenseForm = new Expenses(currentUser);
            expenseForm.Show();
            this.Hide();
        }

        private void logoutbtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // -------------------- Search Button --------------------
        private void searchbtn_Click(object sender, EventArgs e)
        {
            try
            {
                string? selectedCategory = serchbox.SelectedItem?.ToString();
                bool filterByCategory = !string.IsNullOrEmpty(selectedCategory);
                bool filterByDate = datebox.Checked;

                var filtered = userIncomes.AsEnumerable();

                if (filterByCategory)
                    filtered = filtered.Where(i => i.Category.Trim().Equals(selectedCategory, StringComparison.OrdinalIgnoreCase));

                if (filterByDate)
                    filtered = filtered.Where(i => i.IncomeDate.Date == datebox.Value.Date);

                dataGridexpens.DataSource = null;
                dataGridexpens.DataSource = filtered.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
