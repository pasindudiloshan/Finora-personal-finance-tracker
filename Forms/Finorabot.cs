using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FinoraTracker.Forms
{
    public partial class Finorabot : Form
    {
        public Finorabot(Models.User currentUser)
        {
            InitializeComponent();
        }

        // Main method to call the A4F API safely
        private async Task<string> GetA4FResponse(string userMessage, string selectedModel)
        {
            string apiKey = "ddc-a4f-84ddbd64e86349328d9263ec52973fa2";
            string apiUrl = "https://api.a4f.co/v1/chat/completions";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestBody = new
                {
                    model = selectedModel,
                    messages = new[]
                    {
                        new { role = "user", content = userMessage }
                    }
                };

                string jsonBody = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Response: " + responseBody); // Debug log

                    // Parse safely
                    using (JsonDocument jsonResponse = JsonDocument.Parse(responseBody))
                    {
                        var root = jsonResponse.RootElement;

                        if (!root.TryGetProperty("choices", out JsonElement choices) || choices.GetArrayLength() == 0)
                            return "Error: No response from bot.";

                        var firstChoice = choices[0];

                        string botMessage = "";

                        // Check if message exists
                        if (firstChoice.TryGetProperty("message", out JsonElement message))
                        {
                            string role = message.GetProperty("role").GetString();
                            botMessage = message.GetProperty("content").GetString();
                            string displayName = role == "assistant" ? "FinoraBot" : role;
                            return $"{displayName}: {botMessage}";
                        }
                        else if (firstChoice.TryGetProperty("text", out JsonElement text)) // fallback for older APIs
                        {
                            botMessage = text.GetString();
                            return $"FinoraBot: {botMessage}";
                        }
                        else
                        {
                            return "Error: Unexpected response format.";
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    return $"Error: API request failed: {ex.Message}";
                }
                catch (JsonException ex)
                {
                    return $"Error: JSON parsing failed: {ex.Message}";
                }
                catch (Exception ex)
                {
                    return $"Error: An unexpected error occurred: {ex.Message}";
                }
            }
        }

        // Send button click
        private async void sendbtn_Click(object sender, EventArgs e)
        {
            string userMessage = userinputtextbox.Text.Trim();
            string selectedModel = modelcombobox.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                MessageBox.Show("Please enter a message!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(selectedModel))
            {
                MessageBox.Show("Please select a model!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            conversationHistory.AppendText("You: " + userMessage + Environment.NewLine);
            userinputtextbox.Clear();

            string botMessage = await GetA4FResponse(userMessage, selectedModel);
            conversationHistory.AppendText(botMessage + Environment.NewLine);

            conversationHistory.SelectionStart = conversationHistory.Text.Length;
            conversationHistory.ScrollToCaret();
        }

        // Enter key press
        private void userinputtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendbtn_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        // Close button
        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
