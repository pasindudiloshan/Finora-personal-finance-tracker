using System;
using System.Windows.Forms;
using FinoraTracker.Models; // Make sure to include User class

namespace FinoraTracker.Forms
{
    public partial class Dashboard : Form
    {
        private User currentUser;

        // New constructor that accepts a User
        public Dashboard(User user)
        {
            InitializeComponent();
            currentUser = user;

            // Example: Display welcome message using a Label
            // Make sure you have a Label control on your form named "welcomeLabel"
            welcomeLabel.Text = $"Welcome, {currentUser.FullName}!";
        }

        private void welcomeLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
