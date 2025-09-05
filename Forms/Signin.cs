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
            // optional: logo click action
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Hide Signin form and show Signup
            this.Hide();
            Signup signupForm = new Signup();
            signupForm.Show();
        }

        private void Signin_Load(object sender, EventArgs e)
        {
            label7.ForeColor = Color.DarkBlue;
            label7.Cursor = Cursors.Hand;
            label7.Font = new Font(label7.Font, FontStyle.Underline);
        }

        private void registerbtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect input values
                string fullName = fname.Text.Trim() + " " + lname.Text.Trim();
                string gender = male.Checked ? "Male" : female.Checked ? "Female" : string.Empty;
                string incomeFrequency = incomecombo.SelectedItem?.ToString() ?? string.Empty;
                string investmentInterest = investmentcombo.SelectedItem?.ToString() ?? string.Empty;

                // Create user model
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
                    Password = password.Text.Trim()  // will be hashed in DAO
                };

                // Call controller
                UserController controller = new UserController();
                bool success = controller.Register(newUser);

                if (success)
                {
                    MessageBox.Show("🎉 Registration successful! Please log in now.",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open login form
                    this.Hide();
                    Signin signinForm = new Signin();
                    signinForm.Show();
                }
            }
            catch (Exception ex)
            {
                // All validation & duplicate email errors come here
                MessageBox.Show("❌ " + ex.Message,
                                "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
