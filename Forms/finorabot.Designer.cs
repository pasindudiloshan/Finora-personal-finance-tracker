namespace FinoraTracker.Forms
{
    partial class Finorabot
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
            closebtn = new Button();
            userinputtextbox = new TextBox();
            conversationHistory = new RichTextBox();
            modelcombobox = new ComboBox();
            sendbtn = new Button();
            SuspendLayout();
            // 
            // closebtn
            // 
            closebtn.FlatStyle = FlatStyle.Flat;
            closebtn.Font = new Font("Verdana", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            closebtn.ForeColor = Color.SeaGreen;
            closebtn.Location = new Point(887, -3);
            closebtn.Name = "closebtn";
            closebtn.Size = new Size(38, 34);
            closebtn.TabIndex = 1;
            closebtn.Text = "X";
            closebtn.UseVisualStyleBackColor = true;
            closebtn.Click += closebtn_Click;
            // 
            // userinputtextbox
            // 
            userinputtextbox.Location = new Point(272, 504);
            userinputtextbox.Multiline = true;
            userinputtextbox.Name = "userinputtextbox";
            userinputtextbox.Size = new Size(444, 33);
            userinputtextbox.TabIndex = 2;
            // 
            // conversationHistory
            // 
            conversationHistory.ForeColor = SystemColors.HotTrack;
            conversationHistory.Location = new Point(12, 37);
            conversationHistory.Name = "conversationHistory";
            conversationHistory.Size = new Size(898, 428);
            conversationHistory.TabIndex = 3;
            conversationHistory.Text = "";
            // 
            // modelcombobox
            // 
            modelcombobox.DropDownWidth = 500;
            modelcombobox.FormattingEnabled = true;
            modelcombobox.Items.AddRange(new object[] { "provider-4/gpt-4.1", "provider-4/deepseek-v3", "provider-4/claude-3,7-sonnet provider-1/deepseek-ai/DeepSeek-R1", "provider-1/deepseek-ai/DeepSeek-R1-0528", "provider-1/deepseek-ai/DeepSeek-V3", "provider-1/gpt-4.1-2025-04-14", "Llama-4-Maverick-17B-128E", "provider-3/deepseek-r1", "provider-3/gpt-4o provider-3/gpt-4o-2024-11-20" });
            modelcombobox.Location = new Point(12, 504);
            modelcombobox.Name = "modelcombobox";
            modelcombobox.Size = new Size(233, 33);
            modelcombobox.TabIndex = 4;
            // 
            // sendbtn
            // 
            sendbtn.Location = new Point(752, 498);
            sendbtn.Name = "sendbtn";
            sendbtn.Size = new Size(129, 39);
            sendbtn.TabIndex = 5;
            sendbtn.Text = "Send";
            sendbtn.UseVisualStyleBackColor = true;
            sendbtn.Click += sendbtn_Click;
            // 
            // Finorabot
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(922, 564);
            Controls.Add(sendbtn);
            Controls.Add(modelcombobox);
            Controls.Add(conversationHistory);
            Controls.Add(userinputtextbox);
            Controls.Add(closebtn);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Finorabot";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "finorabot";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button closebtn;
        private TextBox userinputtextbox;
        private RichTextBox conversationHistory;
        private ComboBox modelcombobox;
        private Button sendbtn;
    }
}