using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace CybersecurityChatbotGUI
{
    public partial class MainForm : Form
    {
        private readonly UserMemory _memory = new();
        private ResponseEngine? _responseEngine;

        private bool _nameEntered = false;

        private static class Palette
        {
            public static readonly Color Background = Color.FromArgb(10, 14, 26);      // Deep navy
            public static readonly Color PanelBg = Color.FromArgb(16, 22, 40);      // Slightly lighter panel
            public static readonly Color AccentGreen = Color.FromArgb(0, 230, 118);     // Neon green
            public static readonly Color AccentCyan = Color.FromArgb(0, 188, 212);     // Cyan
            public static readonly Color AccentBlue = Color.FromArgb(33, 150, 243);    // Blue
            public static readonly Color BotBubble = Color.FromArgb(20, 30, 55);      // Bot chat bubble
            public static readonly Color UserBubble = Color.FromArgb(0, 77, 64);       // User chat bubble
            public static readonly Color TextLight = Color.FromArgb(220, 230, 255);   // Light text
            public static readonly Color TextDim = Color.FromArgb(120, 140, 180);   // Dim label text
            public static readonly Color InputBg = Color.FromArgb(22, 30, 50);      // Input field background
            public static readonly Color Border = Color.FromArgb(40, 60, 100);     // Subtle border
            public static readonly Color SentimentBg = Color.FromArgb(18, 28, 48);     // Sentiment bar background
        }

        private Panel _headerPanel = null!;
        private Label _logoLabel = null!;
        private Label _taglineLabel = null!;
        private Panel _chatPanel = null!;
        private FlowLayoutPanel _chatFlow = null!;
        private Panel _inputPanel = null!;
        private TextBox _inputBox = null!;
        private Button _sendButton = null!;
        private Label _sentimentLabel = null!;
        private Label _memoryLabel = null!;
        private Panel _statusBar = null!;
        private Label _statusLabel = null!;

        public MainForm()
        {
            InitialiseComponent();
            SetupEventHandlers();
        }
        private void InitialiseComponent()
        {
            this.Text = "Cybersecurity Awareness Chatbot — Part 2";
            this.Size = new Size(900, 720);
            this.MinimumSize = new Size(750, 600);
            this.BackColor = Palette.Background;
            this.ForeColor = Palette.TextLight;
            this.Font = new Font("Consolas", 10f, FontStyle.Regular);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Icon = SystemIcons.Shield;

            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 140,
                BackColor = Palette.PanelBg,
                Padding = new Padding(10, 8, 10, 8)
            };
            _headerPanel.Paint += HeaderPanel_Paint;

            _logoLabel = new Label
            {
                Text = GetAsciiLogoCompact(),
                Font = new Font("Consolas", 7f, FontStyle.Bold),
                ForeColor = Palette.AccentCyan,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            _taglineLabel = new Label
            {
                Text = "[ SAFE  •  SECURE  •  PROTECTED ]",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Palette.AccentGreen,
                AutoSize = false,
                Dock = DockStyle.Bottom,
                Height = 22,
                TextAlign = ContentAlignment.MiddleCenter
            };

            _headerPanel.Controls.Add(_logoLabel);
            _headerPanel.Controls.Add(_taglineLabel);

            _statusBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Palette.SentimentBg,
                Padding = new Padding(10, 0, 10, 0)
            };
            _statusBar.Paint += StatusBar_Paint;

            _memoryLabel = new Label
            {
                Text = "  Memory: No user data stored yet",
                Font = new Font("Consolas", 8.5f, FontStyle.Regular),
                ForeColor = Palette.TextDim,
                AutoSize = false,
                Dock = DockStyle.Left,
                Width = 400,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _sentimentLabel = new Label
            {
                Text = "Mood: —",
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = Palette.AccentBlue,
                AutoSize = false,
                Dock = DockStyle.Right,
                Width = 220,
                TextAlign = ContentAlignment.MiddleRight
            };

            _statusBar.Controls.Add(_sentimentLabel);
            _statusBar.Controls.Add(_memoryLabel);

            _chatPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Palette.Background,
                Padding = new Padding(8),
                AutoScroll = true
            };

            _chatFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Palette.Background,
                Padding = new Padding(6)
            };

            _chatPanel.Controls.Add(_chatFlow);

            _inputPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Palette.PanelBg,
                Padding = new Padding(10, 8, 10, 8)
            };

            _inputBox = new TextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Palette.InputBg,
                ForeColor = Palette.TextLight,
                Font = new Font("Consolas", 11f),
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Type your name to begin...",
                Multiline = false
            };

            _sendButton = new Button
            {
                Text = "SEND  ▶",
                Dock = DockStyle.Right,
                Width = 110,
                BackColor = Palette.AccentGreen,
                ForeColor = Color.Black,
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _sendButton.FlatAppearance.BorderSize = 0;

            _inputPanel.Controls.Add(_inputBox);
            _inputPanel.Controls.Add(_sendButton);

            _statusLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 22,
                Text = "  Cybersecurity Awareness Chatbot  |  Part 2  |  PROG6221",
                Font = new Font("Consolas", 7.5f),
                ForeColor = Palette.TextDim,
                BackColor = Color.FromArgb(8, 10, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            this.Controls.Add(_chatPanel);
            this.Controls.Add(_inputPanel);
            this.Controls.Add(_statusBar);
            this.Controls.Add(_headerPanel);
            this.Controls.Add(_statusLabel);
        }

        private void SetupEventHandlers()
        {
            _sendButton.Click += SendButton_Click;
            _inputBox.KeyDown += InputBox_KeyDown;
            this.Load += MainForm_Load;
            this.Resize += (s, e) => RefreshChatLayout();
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            AudioPlayer.PlayGreetingAsync();

            AppendBotMessage(
                "Welcome to the Cybersecurity Awareness Bot!\n\n" +
                "I'm here to educate you on staying safe online in South Africa.\n\n" +
                "To get started, please tell me your name:");

            _inputBox.Focus();
        }
        private void InputBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 
                ProcessInput();
            }
        }

        private void SendButton_Click(object? sender, EventArgs e)
        {
            ProcessInput();
        }
        private void ProcessInput()
        {
            string input = _inputBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                AppendBotMessage("Please type something before sending.");
                return;
            }

            _inputBox.Clear();

            if (!_nameEntered)
            {
                if (input.Length < 2 || input.All(char.IsDigit))
                {
                    AppendUserMessage(input);
                    AppendBotMessage("That doesn't look like a valid name. Please enter your name:");
                    return;
                }

                _memory.UserName = input;
                _nameEntered = true;
                _responseEngine = new ResponseEngine(_memory);

                AppendUserMessage(input);
                UpdateMemoryLabel();

                AppendBotMessage(
                    $"Hello, {_memory.UserName}! Great to meet you.\n\n" +
                    "I'm your Cybersecurity Awareness Assistant. Here are some things you can ask me:\n\n" +
                    "Passwords  Phishing\n" +
                    "Scams  Privacy\n" +
                    "Malware  Safe Browsing\n" +
                    "Social Engineering  Two-Factor Authentication\n\n" +
                    "You can also say things like:\n" +
                    "\"Tell me more\"\n" +
                    "\"Give me another tip\"\n" +
                    "\"I'm interested in privacy\"\n\n" +
                    "What would you like to learn about today?");

                _inputBox.PlaceholderText = $"Ask me anything, {_memory.UserName}...";
                _inputBox.Focus();
                return;
            }

            AppendUserMessage(input);

            string sentiment = SentimentDetector.Detect(input.ToLower());
            UpdateSentimentLabel(sentiment);

            string response = _responseEngine!.GetResponse(input);

            UpdateMemoryLabel();

            AppendBotMessage(response);
            _inputBox.Focus();
        }
        private void AppendUserMessage(string text)
        {
            var wrapper = new Panel
            {
                BackColor = Palette.Background,
                AutoSize = true,
                Margin = new Padding(0, 4, 0, 4),
                Width = _chatFlow.ClientSize.Width - 20
            };

            var bubble = new Panel
            {
                BackColor = Palette.UserBubble,
                AutoSize = true,
                Padding = new Padding(12, 8, 12, 8),
                MaximumSize = new Size((int)((_chatFlow.ClientSize.Width - 40) * 0.75), 0),
                Anchor = AnchorStyles.Right
            };
            bubble.Paint += (s, e) => DrawRoundedBorder(e.Graphics, bubble, Palette.AccentGreen, 8);

            var label = new Label
            {
                Text = $"You  {DateTime.Now:HH:mm}\n{text}",
                Font = new Font("Consolas", 9.5f),
                ForeColor = Color.White,
                AutoSize = true,
                Padding = new Padding(0)
            };

            bubble.Controls.Add(label);
            bubble.Left = wrapper.Width - bubble.Width - 10;

            var nameLabel = new Label
            {
                Text = $"👤 {_memory.UserName}",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Palette.AccentGreen,
                AutoSize = true,
                Left = bubble.Right - 5,
                Top = bubble.Top
            };

            wrapper.Controls.Add(bubble);
            _chatFlow.Controls.Add(wrapper);
            ScrollToBottom();
        }
        private void AppendBotMessage(string text)
        {
            var wrapper = new Panel
            {
                BackColor = Palette.Background,
                AutoSize = true,
                Margin = new Padding(0, 4, 0, 4),
                Width = _chatFlow.ClientSize.Width - 20
            };

            var headerLabel = new Label
            {
                Text = $"CyberBot  {DateTime.Now:HH:mm}",
                Font = new Font("Consolas", 7.5f, FontStyle.Bold),
                ForeColor = Palette.AccentCyan,
                AutoSize = true,
                Left = 4,
                Top = 0
            };

            var bubble = new Panel
            {
                BackColor = Palette.BotBubble,
                AutoSize = true,
                Padding = new Padding(12, 8, 12, 8),
                MaximumSize = new Size((int)((_chatFlow.ClientSize.Width - 40) * 0.82), 0),
                Left = 4,
                Top = headerLabel.Bottom + 2
            };
            bubble.Paint += (s, e) => DrawRoundedBorder(e.Graphics, bubble, Palette.AccentCyan, 8);

            var label = new Label
            {
                Text = text,
                Font = new Font("Consolas", 9.5f),
                ForeColor = Palette.TextLight,
                AutoSize = true,
                Padding = new Padding(0)
            };

            bubble.Controls.Add(label);
            wrapper.Controls.Add(headerLabel);
            wrapper.Controls.Add(bubble);
            _chatFlow.Controls.Add(wrapper);
            ScrollToBottom();
        }
        private void UpdateSentimentLabel(string sentiment)
        {
            (string text, Color colour) = sentiment switch
            {
                "worried" => ("Mood: Worried", Color.FromArgb(255, 160, 0)),
                "frustrated" => ("Mood: Frustrated", Color.FromArgb(239, 83, 80)),
                "curious" => ("Mood: Curious", Color.FromArgb(102, 187, 106)),
                "happy" => ("Mood: Happy", Palette.AccentGreen),
                _ => ("Mood: Neutral", Palette.AccentBlue)
            };

            _sentimentLabel.Text = text;
            _sentimentLabel.ForeColor = colour;
        }
        private void UpdateMemoryLabel()
        {
            string memText = $"  👤 {_memory.UserName}";

            if (_memory.HasFavouriteTopic)
                memText += $"  |  💾 Interest: {_memory.FavouriteTopic}";

            if (_memory.HasLastTopic)
                memText += $"  |  📌 Last topic: {_memory.LastTopic}";

            _memoryLabel.Text = memText;
            _memoryLabel.ForeColor = Palette.AccentCyan;
        }

        private void ScrollToBottom()
        {
            _chatFlow.ScrollControlIntoView(_chatFlow.Controls[_chatFlow.Controls.Count - 1]);
        }

        private void RefreshChatLayout()
        {
            _chatFlow.Invalidate();
        }

        private void HeaderPanel_Paint(object? sender, PaintEventArgs e)
        {
            using var brush = new LinearGradientBrush(
                _headerPanel.ClientRectangle,
                Color.FromArgb(10, 14, 26),
                Color.FromArgb(5, 20, 40),
                LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, _headerPanel.ClientRectangle);

            using var pen = new Pen(Palette.AccentCyan, 1.5f);
            e.Graphics.DrawLine(pen, 0, _headerPanel.Height - 1, _headerPanel.Width, _headerPanel.Height - 1);
        }

        private void StatusBar_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(Palette.Border, 1f);
            e.Graphics.DrawLine(pen, 0, _statusBar.Height - 1, _statusBar.Width, _statusBar.Height - 1);
        }

        private static void DrawRoundedBorder(Graphics g, Control control, Color colour, int radius)
        {
            using var pen = new Pen(colour, 1.2f);
            var rect = new Rectangle(0, 0, control.Width - 1, control.Height - 1);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = GetRoundedRectPath(rect, radius);
            g.DrawPath(pen, path);
        }

        private static System.Drawing.Drawing2D.GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
        private static string GetAsciiLogoCompact()
        {
            return
                " ██████╗██╗   ██╗██████╗ ███████╗██████╗     ██████╗  ██████╗ ████████╗\n" +
                "██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗    ██╔══██╗██╔═══██╗╚══██╔══╝\n" +
                "██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝    ██████╔╝██║   ██║   ██║   \n" +
                "██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗    ██╔══██╗██║   ██║   ██║   \n" +
                "╚██████╗   ██║   ██████╔╝███████╗██║  ██║    ██████╔╝╚██████╔╝   ██║   \n" +
                " ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝    ╚═════╝  ╚═════╝   ╚═╝   ";
        }
    }
}