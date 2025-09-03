using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinoraTracker.Models;       // for User
using FinoraTracker.Controllers;  // for UserController
using FinoraTracker.DAOs;          // for UserDAO if needed
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FinoraTracker.Forms
{
    public partial class Signup : Form
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (textBox4.Text == "Enter Email")
            {
                textBox4.Text = "";
                textBox4.ForeColor = Color.SeaGreen;
            }

            textBox4.BackColor = Color.White;
            panel7.BackColor = Color.White;
            panel6.BackColor = SystemColors.ControlLight;
            textBox3.BackColor = SystemColors.ControlLight;
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "Enter Password")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.SeaGreen;
            }

            textBox3.BackColor = Color.White;
            panel6.BackColor = Color.White;
            panel7.BackColor = SystemColors.ControlLight;
            textBox4.BackColor = SystemColors.ControlLight;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Hide Signin form
            this.Hide();

            // Open Signin form
            Signin signinForm = new Signin();
            signinForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.Text = "Enter Email";
                textBox4.ForeColor = Color.Silver;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "Enter Password";
                textBox3.ForeColor = Color.SeaGreen;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string emailInput = textBox4.Text.Trim();
            string passwordInput = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(emailInput) || string.IsNullOrEmpty(passwordInput))
            {
                MessageBox.Show("Enter both email and password.");
                return;
            }

            UserController controller = new UserController();
            User? loggedInUser = controller.Login(emailInput, passwordInput);

            if (loggedInUser != null)
            {
                MessageBox.Show($"Welcome, {loggedInUser.FullName}!");
                this.Hide();
                Dashboard dashboard = new Dashboard(loggedInUser);
                dashboard.Show();
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }

    }
}
