using System;
using System.IO;
using System.Windows.Forms;

namespace ItchioButlerUtility
{
    public partial class MainForm : Form
    {
        private TextBox pathBox;
        private TextBox usernameBox;
        private TextBox gameNameBox;
        private TextBox tagBox;
        private ToolTip toolTip;
        private Button pushButton;
        private Button loginButton;
        private Button browseButton;
        private Button generateButton;
        private Label statusLabel;
        private FolderBrowserDialog folderBrowserDialog;

        private static readonly string CredentialsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config", "itch", "butler_creds");

        public MainForm()
        {
            InitializeComponent();
            InitializeToolTips();
            CheckLoginStatus();
            SetIconFromButler();
        }

        private void SetIconFromButler()
        {
            string butlerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "butler.exe");
            if (File.Exists(butlerPath))
                this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(butlerPath);
        }

        private void CheckLoginStatus()
        {
            if (File.Exists(CredentialsPath))
            {
                statusLabel.Text = "Status: Logged in";
                statusLabel.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                statusLabel.Text = "Status: Not logged in";
                statusLabel.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void InitializeToolTips()
        {
            // Create a new ToolTip instance
            this.toolTip = new ToolTip();

            // Set up the delays for the ToolTip
            this.toolTip.AutoPopDelay = 5000; // Time in milliseconds
            this.toolTip.InitialDelay = 1000; // Time before the tooltip appears
            this.toolTip.ReshowDelay = 500; // Time between consecutive tooltips

            // Set up the tooltip text for each control
            this.toolTip.SetToolTip(this.pathBox, "Enter the folder path where your game files are located.");
            this.toolTip.SetToolTip(this.usernameBox, "Enter your username for the Butler login.");
            this.toolTip.SetToolTip(this.gameNameBox, "Enter the name of your game. Has to be the same name as seen in the URL of the game page");
            this.toolTip.SetToolTip(this.tagBox, "Enter the tag for the platform (e.g., win-64, linux).");
            this.toolTip.SetToolTip(this.pushButton, "Click to do a butler push command.");
            this.toolTip.SetToolTip(this.loginButton, "Click to log in with Butler using the provided username.");
            this.toolTip.SetToolTip(this.generateButton, "Click to generate a batch file for the Butler command.");
        }

        private void InitializeComponent()
        {
            // Create controls
            this.pathBox = new TextBox();
            this.usernameBox = new TextBox();
            this.gameNameBox = new TextBox();
            this.tagBox = new TextBox();
            this.pushButton = new Button();
            this.loginButton = new Button();
            this.browseButton = new Button();
            this.generateButton = new Button();
            this.statusLabel = new Label();
            this.folderBrowserDialog = new FolderBrowserDialog();

            // Form settings
            this.Text = "Itch.io Butler Utility";
            this.ClientSize = new System.Drawing.Size(550, 330);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Path Label
            Label pathLabel = new Label();
            pathLabel.Text = "Build Folder Path:";
            pathLabel.Location = new System.Drawing.Point(20, 20);
            pathLabel.Size = new System.Drawing.Size(100, 30);

            // Path TextBox
            this.pathBox.Location = new System.Drawing.Point(130, 20);
            this.pathBox.Size = new System.Drawing.Size(300, 30);

            // Browse Button
            this.browseButton.Location = new System.Drawing.Point(430, 20);
            this.browseButton.Size = new System.Drawing.Size(75, 25);
            this.browseButton.Text = "Browse";
            this.browseButton.Click += new EventHandler(this.BrowseButton_Click);

            // Username Label
            Label usernameLabel = new Label();
            usernameLabel.Text = "Username:";
            usernameLabel.Location = new System.Drawing.Point(20, 60);
            usernameLabel.Size = new System.Drawing.Size(100, 30);

            // Username TextBox
            this.usernameBox.Location = new System.Drawing.Point(130, 60);
            this.usernameBox.Size = new System.Drawing.Size(300, 30);

            // Game Name Label
            Label gameNameLabel = new Label();
            gameNameLabel.Text = "Game Name:";
            gameNameLabel.Location = new System.Drawing.Point(20, 100);
            gameNameLabel.Size = new System.Drawing.Size(100, 30);

            // Game Name TextBox
            this.gameNameBox.Location = new System.Drawing.Point(130, 100);
            this.gameNameBox.Size = new System.Drawing.Size(300, 30);

            // Tag Label
            Label tagLabel = new Label();
            tagLabel.Text = "Tag (e.g. win-64, linux):";
            tagLabel.Location = new System.Drawing.Point(20, 140);
            tagLabel.Size = new System.Drawing.Size(200, 30);

            // Tag TextBox
            this.tagBox.Location = new System.Drawing.Point(230, 140);
            this.tagBox.Size = new System.Drawing.Size(200, 30);

            // Push Button
            this.pushButton.Location = new System.Drawing.Point(200, 180);
            this.pushButton.Size = new System.Drawing.Size(100, 30);
            this.pushButton.Text = "Push";
            this.pushButton.Click += new EventHandler(this.PushButton_Click);

            // Generate Button
            this.generateButton.Location = new System.Drawing.Point(200, 220); // New button position
            this.generateButton.Size = new System.Drawing.Size(120, 30);
            this.generateButton.Text = "Generate .bat file";
            this.generateButton.Click += new EventHandler(this.GenerateButton_Click);

            // Login Button
            this.loginButton.Location = new System.Drawing.Point(200, 260);
            this.loginButton.Size = new System.Drawing.Size(100, 30);
            this.loginButton.Text = "Login to Itch";
            this.loginButton.Click += new EventHandler(this.LoginButton_Click);

            // Status Label
            this.statusLabel.Location = new System.Drawing.Point(20, 300);
            this.statusLabel.Size = new System.Drawing.Size(510, 20);
            this.statusLabel.Text = "Status: Checking...";

            // Add controls to the form
            this.Controls.Add(pathLabel);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(usernameLabel);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(gameNameLabel);
            this.Controls.Add(this.gameNameBox);
            this.Controls.Add(tagLabel);
            this.Controls.Add(this.tagBox);
            this.Controls.Add(this.pushButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.statusLabel);
        }

        private void PushButton_Click(object sender, EventArgs e)
        {
            string path = pathBox.Text;
            string username = usernameBox.Text;
            string gameName = gameNameBox.Text;
            string tag = tagBox.Text;

            // Construct the command and arguments
            string command = "butler";
            string arguments = $"push \"{path}\" {username}/{gameName}:{tag}";

            // Create a new process to run the command
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = true; // Enable shell execution to show the console window
                process.StartInfo.CreateNoWindow = false; // Ensure the window is not created in hidden mode

                try
                {
                    process.Start();
                    process.WaitForExit(); // Wait for the process to complete

                    // Optionally, you can display a message based on the exit code if needed
                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show("Push operation completed successfully.", "Success", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Push operation failed with exit code {process.ExitCode}.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            string path = pathBox.Text;
            string username = usernameBox.Text;
            string gameName = gameNameBox.Text;
            string tag = tagBox.Text;

            // Define the content for the batch file
            string batchContent = $"@echo off\nbutler push \"{path}\" {username}/{gameName}:{tag}\necho Push command executed.\npause";

            // Prompt user to save the batch file
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Batch files (*.bat)|*.bat|All files (*.*)|*.*";
                saveFileDialog.Title = "Save Batch File";
                saveFileDialog.FileName = "butler_push.bat";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, batchContent);
                        MessageBox.Show("Batch file created successfully.", "Success", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            // Show the FolderBrowserDialog
            DialogResult result = this.folderBrowserDialog.ShowDialog();

            // Check if the user selected a folder
            if (result == DialogResult.OK)
            {
                // Update the pathBox with the selected folder path
                this.pathBox.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameBox.Text;

            // Construct the command and arguments for login
            string command = "butler";
            string arguments = $"login {username}";

            // Create a new process to run the command
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = true; // Enable shell execution to show the console window
                process.StartInfo.CreateNoWindow = false; // Ensure the window is not created in hidden mode

                try
                {
                    process.Start();
                    process.WaitForExit(); // Wait for the process to complete

                    // Optionally, you can display a message based on the exit code if needed
                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show("Login successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CheckLoginStatus();
                    }
                    else
                    {
                        MessageBox.Show($"Login failed with exit code {process.ExitCode}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}