using System;
using System.Windows.Forms;
using CefSharp.WinForms;

namespace FinoraTracker.Forms
{
    public partial class Investments : Form
    {
        private Models.User currentUser;

        // List of YouTube video IDs
        private readonly string[] videoIds =
        {
            "FRG6J_r7jg8",
            "QKdbH4333wc",
            "rGNrQjJiAQg"
        };

        // Panels for each video
        private Panel[] videoPanels;

        public Investments(User user)
        {
            InitializeComponent();
            currentUser = user;

            // Create panels dynamically if not added in designer
            videoPanels = new Panel[videoIds.Length];
            for (int i = 0; i < videoIds.Length; i++)
            {
                videoPanels[i] = new Panel
                {
                    Width = 400,
                    Height = 250,
                    Margin = new Padding(10),
                    BorderStyle = BorderStyle.FixedSingle
                };
                flowLayoutPanel1.Controls.Add(videoPanels[i]);
            }

            // Load videos
            for (int i = 0; i < videoIds.Length; i++)
            {
                LoadYouTubeVideo(videoPanels[i], videoIds[i]);
            }
        }

        private void LoadYouTubeVideo(Panel panel, string videoId)
        {
            if (panel == null) return;

            panel.Controls.Clear();

            // Create Chromium browser
            var browser = new ChromiumWebBrowser($"https://www.youtube.com/embed/{videoId}?rel=0&autoplay=0")
            {
                Dock = DockStyle.Fill
            };

            panel.Controls.Add(browser);
        }

        private void logoutbtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
