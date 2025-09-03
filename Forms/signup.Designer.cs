namespace FinoraTracker.Forms
{
    partial class Signup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label3 = new Label();
            label4 = new Label();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label1 = new Label();
            panel5 = new Panel();
            label8 = new Label();
            label9 = new Label();
            button2 = new Button();
            panel6 = new Panel();
            textBox3 = new TextBox();
            pictureBox4 = new PictureBox();
            panel7 = new Panel();
            textBox4 = new TextBox();
            pictureBox5 = new PictureBox();
            label10 = new Label();
            button3 = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 8F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(183, 452);
            label3.Name = "label3";
            label3.Size = new Size(97, 21);
            label3.TabIndex = 0;
            label3.Text = "Develop By";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 8F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(141, 482);
            label4.Name = "label4";
            label4.Size = new Size(139, 21);
            label4.TabIndex = 2;
            label4.Text = "Pasindu Diloshan";
            // 
            // panel1
            // 
            panel1.BackColor = Color.SeaGreen;
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(300, 530);
            panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.finora_logo;
            pictureBox1.Location = new Point(71, 23);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(165, 128);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Bookman Old Style", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ControlLight;
            label2.Location = new Point(71, 193);
            label2.Name = "label2";
            label2.Size = new Size(176, 20);
            label2.TabIndex = 5;
            label2.Text = "Save, Build, Conquer";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Sitka Text", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(76, 146);
            label1.Name = "label1";
            label1.Size = new Size(162, 52);
            label1.TabIndex = 4;
            label1.Text = "FINORA";
            // 
            // panel5
            // 
            panel5.BackColor = SystemColors.ControlLight;
            panel5.Controls.Add(label8);
            panel5.Controls.Add(label9);
            panel5.Controls.Add(button2);
            panel5.Controls.Add(panel6);
            panel5.Controls.Add(panel7);
            panel5.Controls.Add(label10);
            panel5.Controls.Add(button3);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(300, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(450, 530);
            panel5.TabIndex = 10;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Sitka Text", 8F, FontStyle.Bold);
            label8.ForeColor = SystemColors.Highlight;
            label8.Location = new Point(128, 427);
            label8.Name = "label8";
            label8.Size = new Size(193, 46);
            label8.TabIndex = 9;
            label8.Text = "Don’t have an account?\r\nClick here to Sign up";
            label8.TextAlign = ContentAlignment.BottomCenter;
            label8.Click += label8_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Sitka Text", 8F, FontStyle.Bold);
            label9.ForeColor = Color.SeaGreen;
            label9.Location = new Point(213, 308);
            label9.Name = "label9";
            label9.Size = new Size(151, 23);
            label9.TabIndex = 8;
            label9.Text = "Forget Password?";
            // 
            // button2
            // 
            button2.BackColor = Color.SeaGreen;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Sitka Text", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.White;
            button2.Location = new Point(26, 297);
            button2.Name = "button2";
            button2.Size = new Size(161, 40);
            button2.TabIndex = 6;
            button2.Text = "LOGIN";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // panel6
            // 
            panel6.BackColor = SystemColors.ControlLight;
            panel6.Controls.Add(textBox3);
            panel6.Controls.Add(pictureBox4);
            panel6.ForeColor = Color.White;
            panel6.Location = new Point(0, 226);
            panel6.Name = "panel6";
            panel6.Size = new Size(450, 45);
            panel6.TabIndex = 5;
            // 
            // textBox3
            // 
            textBox3.BackColor = SystemColors.ControlLight;
            textBox3.BorderStyle = BorderStyle.None;
            textBox3.Font = new Font("Constantia", 10F);
            textBox3.ForeColor = Color.SeaGreen;
            textBox3.Location = new Point(56, 9);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(358, 30);
            textBox3.TabIndex = 5;
            textBox3.Text = "Enter Password";
            textBox3.UseSystemPasswordChar = true;
            textBox3.Enter += textBox3_Enter;
            textBox3.Leave += textBox3_Leave;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources._lock;
            pictureBox4.Location = new Point(11, 10);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(29, 27);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 4;
            pictureBox4.TabStop = false;
            // 
            // panel7
            // 
            panel7.BackColor = Color.White;
            panel7.Controls.Add(textBox4);
            panel7.Controls.Add(pictureBox5);
            panel7.ForeColor = Color.White;
            panel7.Location = new Point(0, 165);
            panel7.Name = "panel7";
            panel7.Size = new Size(450, 45);
            panel7.TabIndex = 4;
            // 
            // textBox4
            // 
            textBox4.BackColor = Color.White;
            textBox4.BorderStyle = BorderStyle.None;
            textBox4.Font = new Font("Constantia", 10F);
            textBox4.ForeColor = Color.SeaGreen;
            textBox4.Location = new Point(56, 11);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(358, 30);
            textBox4.TabIndex = 4;
            textBox4.Text = "Enter Email";
            textBox4.Enter += textBox4_Enter;
            textBox4.Leave += textBox4_Leave;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.user;
            pictureBox5.Location = new Point(11, 9);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(29, 27);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 3;
            pictureBox5.TabStop = false;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Font = new Font("Sitka Text", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.SeaGreen;
            label10.Location = new Point(26, 73);
            label10.Name = "label10";
            label10.Size = new Size(322, 40);
            label10.TabIndex = 3;
            label10.Text = "Login to your account";
            label10.Click += label10_Click;
            // 
            // button3
            // 
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Verdana", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.SeaGreen;
            button3.Location = new Point(412, 0);
            button3.Name = "button3";
            button3.Size = new Size(38, 34);
            button3.TabIndex = 0;
            button3.Text = "X";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Signup
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(750, 530);
            Controls.Add(panel5);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Signup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "signup";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label label3;
        private Label label4;
        private Panel panel1;
        private PictureBox pictureBox1;
        private Label label2;
        private Label label1;
        private Panel panel5;
        private Label label8;
        private Label label9;
        private Button button2;
        private Panel panel6;
        private TextBox textBox3;
        private PictureBox pictureBox4;
        private Panel panel7;
        private TextBox textBox4;
        private PictureBox pictureBox5;
        private Label label10;
        private Button button3;
    }
}