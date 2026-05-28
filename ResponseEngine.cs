namespace CybersecurityChatbotGUI
{
    /// <param name="memory">Current user memory state.</param>
    /// <returns>A response string.</returns>
    public delegate string ResponseGenerator(UserMemory memory);

    public class ResponseEngine
    {
        private readonly Random _random = new();
        private readonly UserMemory _memory;

        private readonly Dictionary<string, List<string>> _keywordResponses = new()
        {
            ["password"] =
            [
                "Make sure to use strong, unique passwords for each account. " +
                "A strong password is at least 12 characters long and includes upper and lowercase letters, numbers, and symbols.",
                "Never reuse passwords across different websites. If one account is compromised, attackers will try the same password on others.",
                "Consider using a reputable password manager to generate and store complex passwords securely.",
                "Avoid using personal details like your name, birthday, or pet's name in passwords — these are easy to guess.",
                "Enable two-factor authentication (2FA) wherever possible for an extra layer of protection."
            ],
            ["phishing"] =
            [
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations like banks or government departments.",
                "Always check the sender's email address carefully. Phishing emails often use addresses that look similar to legitimate ones but contain subtle misspellings.",
                "Never click links in unsolicited emails. Instead, navigate directly to the website by typing the URL into your browser.",
                "Legitimate organisations will NEVER ask for your password or banking details via email or SMS.",
                "If an offer in an email seems too good to be true, it almost certainly is. Report suspicious emails to your IT department or email provider."
            ],
            ["scam"] =
            [
                "Online scams are increasingly sophisticated. Always verify the identity of anyone requesting money or personal information.",
                "Be wary of unsolicited calls claiming to be from banks, government agencies, or tech support — hang up and call the official number yourself.",
                "Romance scams are a growing threat in South Africa. Never send money to someone you've only met online.",
                "Investment scams promising high returns with no risk are almost always fraudulent. Verify any investment opportunity with the FSCA.",
                "If you suspect you've been scammed, report it to the South African Police Service (SAPS) and the Southern African Fraud Prevention Service (SAFPS)."
            ],
            ["privacy"] =
            [
                "Review the privacy settings on all your social media accounts regularly. Limit who can see your personal information.",
                "Be careful about what personal information you share online. Your full name, address, ID number, and phone number can be used for identity theft.",
                "Use a VPN (Virtual Private Network) when connecting to public Wi-Fi to protect your data from eavesdroppers.",
                "Read the privacy policy before installing new apps — some apps request unnecessary access to your contacts, camera, or location.",
                "Regularly audit which third-party apps have access to your social media accounts and revoke access to any you no longer use."
            ],
            ["malware"] =
            [
                "Malware is malicious software designed to disrupt, damage, or gain unauthorised access to your device. Keep your antivirus software up to date.",
                "Never download software from unofficial websites or click on pop-up ads offering free downloads — these are common malware delivery methods.",
                "Keep your operating system and applications updated. Many malware attacks exploit known vulnerabilities that patches have already fixed.",
                "Ransomware is a type of malware that encrypts your files and demands payment. Regularly back up your data to an external drive or secure cloud storage.",
                "Be cautious with USB drives from unknown sources — they can be pre-loaded with malware designed to infect your device automatically."
            ],
            ["safe browsing"] =
            [
                "Only visit websites that use HTTPS (the padlock icon in the address bar). HTTP sites transmit data without encryption.",
                "Avoid clicking on ads, especially pop-ups. Use an ad blocker to reduce exposure to malicious advertisements (malvertising).",
                "Be cautious on public Wi-Fi — avoid accessing banking or sensitive accounts unless you're using a VPN.",
                "Clear your browser cookies and cache regularly to remove stored tracking data and session tokens.",
                "Use a privacy-focused browser or extensions that block trackers and scripts that monitor your online activity."
            ],
            ["social engineering"] =
            [
                "Social engineering manipulates people into revealing confidential information. Always verify the identity of anyone asking for sensitive data.",
                "Attackers may impersonate colleagues, IT staff, or authority figures. When in doubt, verify through a separate, trusted channel.",
                "Be wary of urgent requests that pressure you to act quickly without thinking — urgency is a classic social engineering tactic.",
                "Never share your login credentials with anyone, even if they claim to be from your company's IT department.",
                "Regularly train yourself and your team to recognise social engineering attempts — awareness is the best defence."
            ],
            ["two-factor"] =
            [
                "Two-factor authentication (2FA) adds a second layer of security beyond your password. Enable it on all important accounts.",
                "Use an authenticator app like Google Authenticator or Microsoft Authenticator rather than SMS-based 2FA, which can be intercepted.",
                "Even if an attacker steals your password, 2FA prevents them from logging in without the second factor.",
                "Back up your 2FA recovery codes in a safe place — losing your phone without recovery codes can lock you out of your accounts.",
                "Most major platforms — Google, Facebook, banking apps — support 2FA. Enable it today to significantly improve your account security."
            ]
        };

        private readonly Dictionary<string, string> _generalResponses = new()
        {
            ["how are you"]   = "I'm always alert and ready to help you stay safe online! How can I assist you today?",
            ["purpose"]       = "My purpose is to educate South African citizens about cybersecurity threats and how to protect themselves online.",
            ["what can i ask"] = "You can ask me about: passwords, phishing, scams, privacy, malware, safe browsing, social engineering, and two-factor authentication.",
            ["help"]          = "I'm here to help! Ask me about cybersecurity topics like phishing, passwords, scams, or privacy.",
            ["hello"]         = "Hello! Ready to learn about staying safe online? What would you like to know?",
            ["hi"]            = "Hi there! Let's talk cybersecurity. Ask me anything!",
            ["thank"]         = "You're welcome! Staying informed is your best defence. Is there anything else you'd like to know?",
            ["bye"]           = "Stay safe online! Remember to keep your passwords strong and your guard up.",
            ["goodbye"]       = "Goodbye! Stay cyber-safe and remember — when in doubt, don't click!"
        };

        private readonly Dictionary<string, ResponseGenerator> _followUpGenerators;

        public ResponseEngine(UserMemory memory)
        {
            _memory = memory;

            _followUpGenerators = new Dictionary<string, ResponseGenerator>
            {
                ["tell me more"] = mem =>
                    mem.HasLastTopic
                        ? GetRandomKeywordResponse(mem.LastTopic)
                        : "What topic would you like me to elaborate on? I can help with phishing, passwords, scams, malware, and more.",

                ["another tip"] = mem =>
                    mem.HasLastTopic
                        ? $"Here's another tip about {mem.LastTopic}: {GetRandomKeywordResponse(mem.LastTopic)}"
                        : "Sure! What topic would you like a tip on?",

                ["explain more"] = mem =>
                    mem.HasLastTopic
                        ? $"Let me go deeper on {mem.LastTopic}: {GetRandomKeywordResponse(mem.LastTopic)}"
                        : "Of course! Which topic would you like explained in more detail?",

                ["give me another"] = mem =>
                    mem.HasLastTopic
                        ? GetRandomKeywordResponse(mem.LastTopic)
                        : "Happy to share more! Which topic interests you?",

                ["more"] = mem =>
                    mem.HasLastTopic
                        ? GetRandomKeywordResponse(mem.LastTopic)
                        : "More information on what? Ask me about phishing, passwords, scams, or privacy!"
            };
        }

        /// <param name="userInput">Raw text entered by the user.</param>
        /// <returns>The chatbot's response string.</returns>
        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please type something — I'm here to help!";

            string lower = userInput.ToLower().Trim();

            string sentiment = SentimentDetector.Detect(lower);
            _memory.LastSentiment = sentiment;
            string empathyPrefix = SentimentDetector.GetEmpathyPrefix(sentiment, _memory.UserName);

            foreach (var followUp in _followUpGenerators)
            {
                if (lower.Contains(followUp.Key))
                {
                    string followUpResponse = followUp.Value(_memory); 
                    return empathyPrefix + followUpResponse;
                }
            }

            if (lower.Contains("interested in") || lower.Contains("i like") || lower.Contains("i want to learn about"))
            {
                foreach (string topic in _keywordResponses.Keys)
                {
                    if (lower.Contains(topic))
                    {
                        _memory.FavouriteTopic = topic;
                        _memory.LastTopic = topic;
                        string topicResponse = GetRandomKeywordResponse(topic);
                        return $"Great! I'll remember that you're interested in {topic}. It's a crucial part of staying safe online. " +
                               $"Here's what you should know:\n\n{topicResponse}";
                    }
                }
            }

            if ((lower.Contains("remind me") || lower.Contains("what do i like") || lower.Contains("my topic") || lower.Contains("my interest"))
                && _memory.HasFavouriteTopic)
            {
                return $"As someone interested in {_memory.FavouriteTopic}, here's a relevant tip:\n\n" +
                       GetRandomKeywordResponse(_memory.FavouriteTopic);
            }

            foreach (string keyword in _keywordResponses.Keys)
            {
                if (lower.Contains(keyword))
                {
                    _memory.LastTopic = keyword;
                    string keywordResponse = GetRandomKeywordResponse(keyword);

                    if (_memory.HasFavouriteTopic && _memory.FavouriteTopic == keyword)
                    {
                        return empathyPrefix +
                               $"As someone interested in {keyword}, here's something important:\n\n{keywordResponse}";
                    }

                    return empathyPrefix + keywordResponse;
                }
            }

            foreach (var general in _generalResponses)
            {
                if (lower.Contains(general.Key))
                    return empathyPrefix + general.Value;
            }

            return "I'm not sure I understand. Could you try rephrasing? " +
                   "You can ask me about phishing, passwords, scams, privacy, malware, safe browsing, " +
                   "social engineering, or two-factor authentication.";
        }

        /// <param name="keyword">The matched cybersecurity topic keyword.</param>
        /// <returns>A randomly selected response string.</returns>
        private string GetRandomKeywordResponse(string keyword)
        {
            if (_keywordResponses.TryGetValue(keyword, out List<string>? responses) && responses.Count > 0)
                return responses[_random.Next(responses.Count)];

            return "I have information on that topic, but let me look into it more. " +
                   "Try asking about phishing, passwords, or scams for now.";
        }
    }
}
