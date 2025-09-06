using System;
using System.Windows.Forms;
using FinoraTracker.Models;
using FinoraTracker.Controllers;

namespace FinoraTracker.Forms
{
    public partial class Addexpenses : Form
    {
        private User currentUser;
        private readonly ExpenseController expenseController;
        private readonly Expense? existingExpense;

        public Addexpenses(User user)
        {
            InitializeComponent();
            currentUser = user;
            expenseController = new ExpenseController();
            existingExpense = null;
            InitializeForm();
        }

        public Addexpenses(User user, Expense expenseToEdit)
        {
            InitializeComponent();
            currentUser = user;
            expenseController = new ExpenseController();
            existingExpense = expenseToEdit;
            InitializeForm();

            // Pre-fill fields with existing expense
            amountbox1.Value = existingExpense.Amount;
            categorycombo2.SelectedItem = existingExpense.Category;
            dateTimePicker2.Value = existingExpense.ExpenseDate;
            descrption1.Text = existingExpense.Description;
            paymentcombo.SelectedItem = existingExpense.PaymentMethod;

            addexpensesbtn.Text = "Update Expense";
        }

        private void InitializeForm()
        {
            label7.Text = currentUser.FullName;
            label8.Text = currentUser.Email;

            amountbox1.Maximum = 1000000;
            amountbox1.Minimum = 0;
            amountbox1.DecimalPlaces = 2;
            amountbox1.Increment = 100;

            categorycombo2.Items.Clear();
            categorycombo2.Items.AddRange(new string[] { "Food", "Rent", "Bills", "Transport", "Shopping", "Health", "Entertainment", "Other" });

            paymentcombo.Items.Clear();
            paymentcombo.Items.AddRange(new string[] { "Cash", "Bank Account", "Credit Card", "PayPal" });
        }

        private void addexpensesbtn_Click(object sender, EventArgs e)
        {
            try
            {
                Expense expense = new Expense
                {
                    UserId = currentUser.UserId,
                    Amount = amountbox1.Value,
                    Category = categorycombo2.SelectedItem?.ToString() ?? "",
                    ExpenseDate = dateTimePicker2.Value,
                    Description = descrption1.Text.Trim(),
                    PaymentMethod = paymentcombo.SelectedItem?.ToString() ?? "",
                    CreatedAt = existingExpense?.CreatedAt ?? DateTime.Now
                };

                bool success;
                if (existingExpense != null)
                {
                    expense.ExpenseId = existingExpense.ExpenseId;
                    success = expenseController.UpdateExpense(expense);
                }
                else
                {
                    success = expenseController.AddExpense(expense);
                }

                if (success)
                {
                    MessageBox.Show(existingExpense != null ? "Expense updated!" : "Expense added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (existingExpense == null)
                    {
                        amountbox1.Value = 0;
                        categorycombo2.SelectedIndex = -1;
                        dateTimePicker2.Value = DateTime.Today;
                        descrption1.Clear();
                        paymentcombo.SelectedIndex = -1;
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void expensesbtn_Click(object sender, EventArgs e)
        {
            Expenses expenseForm = new Expenses(currentUser);
            expenseForm.Show();
            this.Hide();
        }

        private void backbtn_Click(object sender, EventArgs e)
        {
            Expenses expenseForm = new Expenses(currentUser);
            expenseForm.Show();
            this.Hide();
        }

        private void paymentcombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: handle selection change if needed
        }
    }
}
