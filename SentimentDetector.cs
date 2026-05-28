namespace CybersecurityChatbotGUI
{
    public static class SentimentDetector
    {
        private static readonly Dictionary<string, List<string>> SentimentKeywords = new()
        {
            ["worried"] =
            [
                "worried", "scared", "afraid", "anxious", "nervous",
                "concerned", "fear", "frightened", "uneasy", "terrified"
            ],
            ["frustrated"] =
            [
                "frustrated", "annoyed", "angry", "upset", "irritated",
                "confused", "stuck", "lost", "overwhelmed", "fed up"
            ],
            ["curious"] =
            [
                "curious", "interested", "wondering", "want to know",
                "how does", "what is", "tell me", "explain", "learning"
            ],
            ["happy"] =
            [
                "great", "happy", "excited", "awesome", "wonderful",
                "good", "cool", "nice", "love", "enjoy"
            ]
        };

        /// <param name="input">Raw user input text.</param>
        /// <returns>One of: "worried", "frustrated", "curious", "happy", or "neutral".</returns>
        public static string Detect(string input)
        {
            string lower = input.ToLower();

            foreach (var sentiment in SentimentKeywords)
            {
                foreach (string keyword in sentiment.Value)
                {
                    if (lower.Contains(keyword))
                        return sentiment.Key;
                }
            }

            return "neutral";
        }

        /// <param name="sentiment">The detected sentiment string.</param>
        /// <param name="userName">User's name for personalisation.</param>
        /// <returns>A short empathetic sentence, or empty string for neutral.</returns>
        public static string GetEmpathyPrefix(string sentiment, string userName)
        {
            return sentiment switch
            {
                "worried" => $"It's completely understandable to feel worried, {userName}. " +
                             "Cyber threats can be very unsettling. Let me help you feel more confident. ",
                "frustrated" => $"I hear you, {userName} — cybersecurity can feel overwhelming at times. " +
                                "Let's work through this together. ",
                "curious" => $"I love your curiosity, {userName}! " +
                             "Asking questions is the first step to staying safe online. ",
                "happy" => $"Great to hear you're in good spirits, {userName}! " +
                           "Let's keep that energy going as we learn. ",
                _ => string.Empty
            };
        }
    }
}
