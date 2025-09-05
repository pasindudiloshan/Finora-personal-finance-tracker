using System;
using System.Windows.Forms;
using FinoraTracker.Models; // Make sure User class is included

namespace FinoraTracker.Forms
{
    public partial class Dashboard : Form
    {
        private User currentUser;

        // Constructor that accepts a User
        public Dashboard(User user)
        {
            InitializeComponent();
            currentUser = user;

            // Display welcome message using a Label
            welcomeLabel.Text = $"Hi!, {currentUser.FullName}";
            label8.Text = $"{currentUser.Email}";
            label7.Text = $"{currentUser.FullName}";
        }

        // Logout button
        private void logoutbtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Dashboard button (reload current dashboard)
        private void dashboardbtn_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard(this.currentUser); // pass the same user
            dashboard.Show();
            this.Close();
        }

        // Income button
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
    }
}
