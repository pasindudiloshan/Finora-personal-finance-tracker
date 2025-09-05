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
    public partial class IncomeForm : Form
    {
        private User currentUser;

        public string UserId { get; internal set; }
        public decimal Amount { get; internal set; }
        public DateTime IncomeDate { get; internal set; }
        public string Category { get; internal set; }
        public string Description { get; internal set; }
        public string AccountSource { get; internal set; }
        public DateTime CreatedAt { get; internal set; }

        public IncomeForm(User user)
        {
            InitializeComponent();
            currentUser = user;

            // Display welcome message using a Label
            label8.Text = $"{currentUser.Email}";
            label7.Text = $"{currentUser.FullName}";
        }

        public IncomeForm()
        {
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
            this.Close();
        }

        private void expensesbtn_Click(object sender, EventArgs e)
        {
            Expenses expenseForm = new Expenses(this.currentUser);
            expenseForm.Show();
            this.Hide();
        }

        private void addincomepage_Click(object sender, EventArgs e)
        {
            Addincome addincomeForm = new Addincome(this.currentUser);
            addincomeForm.Show();
            this.Hide();
        }
    }
}
