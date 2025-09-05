using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinoraTracker.Models;

namespace FinoraTracker.Forms
{
    public partial class Addexpenses : Form
    {
        private User currentUser;
        public Addexpenses(User user)
        {
            InitializeComponent();
            currentUser = user;

            // Display welcome message using a Label
            label8.Text = $"{currentUser.Email}";
            label7.Text = $"{currentUser.FullName}";
        }

        private void logoutbtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dashboardbtn_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard(this.currentUser);
            dashboard.Show();
            this.Hide();
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

    }

}