using FinoraTracker.Controllers;
using FinoraTracker.Models;
using System;
using System.Windows.Forms;

namespace FinoraTracker.Forms
{
    public partial class Addincome : Form
    {
        private readonly User currentUser;
        private readonly IncomeController incomeController;
        private readonly Income? existingIncome; // nullable for add/edit distinction

        // Constructor for adding new income
        public Addincome(User user)
        {
            InitializeComponent();
            currentUser = user;
            incomeController = new IncomeController();
            existingIncome = null;

            InitializeForm();
        }

        // Constructor for editing existing income
        public Addincome(User user, Income incomeToEdit)
        {
            InitializeComponent();
            currentUser = user;
            incomeController = new IncomeController();
            existingIncome = incomeToEdit;

            InitializeForm();

            // Pre-fill fields with existing income
            amountbox.Value = existingIncome.Amount;
            categorycombo.SelectedItem = existingIncome.Category;
            dateTimePicker1.Value = existingIncome.IncomeDate;
            descrption.Text = existingIncome.Description;
            soucrecombo.SelectedItem = existingIncome.AccountSource;

            addincomebtn.Text = "Update Income"; // Change button text
        }

        // Common initialization
        private void InitializeForm()
        {
            label8.Text = currentUser.Email;
            label7.Text = currentUser.FullName;

            amountbox.Maximum = 1000000;
            amountbox.Minimum = 0;
            amountbox.DecimalPlaces = 2;
            amountbox.Increment = 100;

            // Populate combo boxes
            categorycombo.Items.Clear();
            categorycombo.Items.AddRange(new string[] { "Salary", "Freelance", "Investment", "Gift", "Other" });

            soucrecombo.Items.Clear();
            soucrecombo.Items.AddRange(new string[] { "Cash", "Bank Account", "PayPal", "Credit Card" });
        }

        // Add or Update button click
        private void addincomebtn_Click(object sender, EventArgs e)
        {
            try
            {
                Income income = new Income
                {
                    UserId = currentUser.UserId,
                    Amount = amountbox.Value,
                    Category = categorycombo.SelectedItem?.ToString() ?? "",
                    IncomeDate = dateTimePicker1.Value,
                    Description = descrption.Text.Trim(),
                    AccountSource = soucrecombo.SelectedItem?.ToString() ?? "",
                    CreatedAt = existingIncome?.CreatedAt ?? DateTime.Now
                };

                bool success;
                if (existingIncome != null)
                {
                    income.IncomeId = existingIncome.IncomeId;
                    success = incomeController.UpdateIncome(income);
                }
                else
                {
                    success = incomeController.AddIncome(income);
                }

                if (success)
                {
                    MessageBox.Show(existingIncome != null ? "Income updated!" : "Income added!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (existingIncome == null)
                    {
                        amountbox.Value = 0;
                        categorycombo.SelectedIndex = -1;
                        dateTimePicker1.Value = DateTime.Today;
                        descrption.Clear();
                        soucrecombo.SelectedIndex = -1;
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Navigation buttons
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
            IncomeForm incomeForm = new IncomeForm(currentUser);
            incomeForm.Show();
            this.Hide();
        }

        private void logoutbtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            IncomeForm incomeForm = new IncomeForm(currentUser);
            incomeForm.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
