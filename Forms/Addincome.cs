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

        public Addincome(User user)
        {
            InitializeComponent();
            currentUser = user;
            incomeController = new IncomeController();

            // Display welcome message
            label8.Text = currentUser.Email;
            label7.Text = currentUser.FullName;

            // 🔹 Fix NumericUpDown for large amounts
            amountbox.Maximum = 1000000;  // Set as needed
            amountbox.Minimum = 0;
            amountbox.DecimalPlaces = 2;   // Optional, for cents
            amountbox.Increment = 100;     // Optional, step with arrows
        }

        // 🔹 Add Income button click
        private void addincomebtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Create new Income object
                Income newIncome = new Income
                {
                    UserId = currentUser.UserId,
                    Amount = amountbox.Value,
                    Category = categorycombo.SelectedItem?.ToString() ?? "",
                    IncomeDate = dateTimePicker1.Value,
                    Description = descrption.Text.Trim(),
                    AccountSource = soucrecombo.SelectedItem?.ToString() ?? "",
                    CreatedAt = DateTime.Now
                };

                // Add to database
                bool success = incomeController.AddIncome(newIncome);

                if (success)
                {
                    MessageBox.Show("Income added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear form fields
                    amountbox.Value = 0;
                    categorycombo.SelectedIndex = -1;
                    dateTimePicker1.Value = DateTime.Today;
                    descrption.Clear();
                    soucrecombo.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔹 Navigation buttons
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
    }
}
