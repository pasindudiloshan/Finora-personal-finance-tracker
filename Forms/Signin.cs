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

namespace FinoraTracker.Forms
{
    public partial class Signin : Form
    {
        public Signin()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Hide Signin form
            this.Hide();

            // Open Signup form
            Signup signupForm = new Signup();
            signupForm.Show();
        }

        private void Signin_Load(object sender, EventArgs e)
        {
            label7.ForeColor = Color.DarkBlue;
            label7.Cursor = Cursors.Hand;
            label7.Font = new Font(label7.Font, FontStyle.Underline);
        }

        private void fname_TextChanged(object sender, EventArgs e)
        {

        }

        private void lname_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void email_TextChanged(object sender, EventArgs e)
        {

        }

        private void male_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void female_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void occupation_TextChanged(object sender, EventArgs e)
        {

        }

        private void incomecombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void investmentcombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void howknow_TextChanged(object sender, EventArgs e)
        {

        }

        private void password_TextChanged(object sender, EventArgs e)
        {

        }

        private void registerbtn_Click(object sender, EventArgs e)
        {
            try
            {
                string fullName = fname.Text.Trim() + " " + lname.Text.Trim();
                string gender = male.Checked ? "Male" : female.Checked ? "Female" : string.Empty;
                string incomeFrequency = incomecombo.SelectedItem?.ToString() ?? string.Empty;
                string investmentInterest = investmentcombo.SelectedItem?.ToString() ?? string.Empty;

                User newUser = new User
                {
                    FullName = fullName,
                    PhoneNumber = pnumber.Text.Trim(),
                    Email = email.Text.Trim(),
                    Gender = gender,
                    Occupation = occupation.Text.Trim(),
                    IncomeFrequency = incomeFrequency,
                    InvestmentInterest = investmentInterest,
                    HowDidYouKnow = howknow.Text.Trim(),
                    Password = password.Text.Trim()
                };

                UserController controller = new UserController();
                bool success = controller.Register(newUser);

                if (success)
                {
                    MessageBox.Show("Registration successful!");
                    this.Hide();
                    Signin signinForm = new Signin();
                    signinForm.Show();
                }
                else
                {
                    MessageBox.Show("Registration failed!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
