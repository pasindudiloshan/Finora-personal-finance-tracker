using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FinoraTracker.Models;
using FinoraTracker.Controllers;

namespace FinoraTracker.Forms
{
    public partial class Expenses : Form
    {
        private User currentUser;
        private readonly ExpenseController expenseController;
        private List<Expense> userExpenses = new List<Expense>();

        public Expenses(User user)
        {
            InitializeComponent();
            currentUser = user;
            expenseController = new ExpenseController();

            label7.Text = currentUser.FullName;
            label8.Text = currentUser.Email;

            dataGridExpenses.AutoGenerateColumns = false;
            InitializeDataGridColumns();
            InitializeCategoryCombo();

            LoadUserExpenses();
            UpdateExpenseSummary(); // Show last 30 days expense
        }

        // -------------------- DataGrid Columns --------------------
        private void InitializeDataGridColumns()
        {
            dataGridExpenses.Columns.Clear();

            dataGridExpenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Amount",
                DataPropertyName = "Amount",
                Width = 100
            });
            dataGridExpenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Category",
                DataPropertyName = "Category",
                Width = 120
            });
            dataGridExpenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Date",
                DataPropertyName = "ExpenseDate",
                Width = 100,
                DefaultCellStyle = { Format = "yyyy-MM-dd" }
            });
            dataGridExpenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Payment Method",
                DataPropertyName = "PaymentMethod",
                Width = 120
            });
            dataGridExpenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200
            });

            dataGridExpenses.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Edit",
                Text = "Edit",
                UseColumnTextForButtonValue = true
            });
            dataGridExpenses.Columns.Add(new DataGridViewButtonColumn
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
                "Food", "Rent", "Bills", "Transport", "Shopping", "Health", "Entertainment", "Other"
            });
            serchbox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // -------------------- Load User Expenses --------------------
        private void LoadUserExpenses()
        {
            try
            {
                userExpenses = expenseController.GetExpensesByUser(currentUser.UserId);
                dataGridExpenses.DataSource = userExpenses.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load expenses: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------------------- Update Last 30 Days Expense Summary --------------------
        private void UpdateExpenseSummary()
        {
            try
            {
                var last30DaysExpenses = userExpenses
                    .Where(e => e.ExpenseDate >= DateTime.Now.AddDays(-30))
                    .ToList();

                decimal totalLast30Days = last30DaysExpenses.Sum(e => e.Amount);

                // Display exact amount with commas
                textBox1.Text = $"Last 30 days expenses: {totalLast30Days:N0} LKR";
            }
            catch
            {
                textBox1.Text = "Last 30 days expenses: 0 LKR";
            }
        }

        // -------------------- DataGrid Cell Click --------------------
        private void dataGridExpenses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Expense selectedExpense = userExpenses[e.RowIndex];

            if (dataGridExpenses.Columns[e.ColumnIndex].HeaderText == "Edit")
            {
                Addexpenses editForm = new Addexpenses(currentUser, selectedExpense);
                editForm.FormClosed += (s, args) => { LoadUserExpenses(); UpdateExpenseSummary(); };
                editForm.Show();
            }
            else if (dataGridExpenses.Columns[e.ColumnIndex].HeaderText == "Delete")
            {
                var confirm = MessageBox.Show("Are you sure you want to delete this expense?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        expenseController.DeleteExpense(selectedExpense.ExpenseId);
                        LoadUserExpenses();
                        UpdateExpenseSummary();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Delete failed: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // -------------------- Add Expense Page --------------------
        private void addexpensespage_Click(object sender, EventArgs e)
        {
            Addexpenses addForm = new Addexpenses(currentUser);
            addForm.FormClosed += (s, args) => { LoadUserExpenses(); UpdateExpenseSummary(); };
            addForm.Show();
        }

        // -------------------- Navigation --------------------
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

        private void logoutbtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // -------------------- Search --------------------
        private void searchbtn_Click(object sender, EventArgs e)
        {
            try
            {
                string? selectedCategory = serchbox.SelectedItem?.ToString();
                bool filterByCategory = !string.IsNullOrEmpty(selectedCategory);
                bool filterByDate = datebox.Checked;

                var filtered = userExpenses.AsEnumerable();

                if (filterByCategory)
                    filtered = filtered.Where(i => i.Category.Trim().Equals(selectedCategory, StringComparison.OrdinalIgnoreCase));

                if (filterByDate)
                    filtered = filtered.Where(i => i.ExpenseDate.Date == datebox.Value.Date);

                dataGridExpenses.DataSource = null;
                dataGridExpenses.DataSource = filtered.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
