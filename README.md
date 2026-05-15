[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/Apa4hIya)
Cybersecurity Awareness Chatbot — Part 2 (GUI)

# What This Project Is
This is Part 2 of the Cybersecurity Awareness Chatbot POE. The console application from Part 1 has been fully redesigned as a Windows Forms (WinForms) GUI application with a dark cyber theme. The chatbot educates South African citizens on cybersecurity topics including phishing, passwords, scams, malware, and more.

# How to Run the Project
What You Need
Windows 10 or Windows 11
Visual Studio 2022
.NET 8 SDK — download at https://dotnet.microsoft.com/download/dotnet/8
Steps
Clone or download this repository
Open Visual Studio 2022
Click File → Open → Project/Solution
Select the CybersecurityChatbotGUI.csproj file
Right-click Greeting.wav in Solution Explorer → Properties → set Copy to Output Directory to Copy Always
Press F5 to build and run
Features
# 1. GUI Design
Full WinForms application with a dark navy and cyan cyberpunk theme
ASCII logo displayed in the header panel
Chat bubbles for bot and user messages
Sentiment and memory status bars update live during conversation
Voice greeting plays automatically when the app launches
# 2. Keyword Recognition
The chatbot recognises these cybersecurity topics:

| password | "Tell me about password safety" | | phishing | "What is phishing?" | | scam | "How do I avoid scams?" | | privacy | "Tell me about privacy" | | malware | "What is malware?" | | safe browsing | "Safe browsing tips" | | social engineering | "What is social engineering?" | | two-factor | "Tell me about two-factor authentication" |

# 3. Random Responses
Each keyword has 5 different responses. The chatbot randomly selects one each time so conversations feel varied and engaging.

# 4. Conversation Flow
The chatbot handles follow-up phrases without restarting the conversation:

"Tell me more"
"Give me another tip"
"Explain more"
"Another tip"
"More"

# 5. Memory and Recall
Stores the user's name entered at the start
Remembers the user's favourite topic when they say "I'm interested in privacy"
References stored information later in the conversation
Memory status bar shows what the bot currently remembers

# 6. Sentiment Detection
Detects the user's mood and responds with empathy before giving cybersecurity advice:

Worried, Frustrated, Curious, Happy and Neutral

# 7. Error Handling
Empty input gives a helpful prompt instead of crashing
Invalid name entries (too short or all numbers) are caught gracefully
Unrecognised input returns a friendly fallback message

Sample Conversation
Bot:   Welcome! Please tell me your name.
You:   Nokubonga
Bot:   Hello Nokubonga! What would you like to learn about today?

You:   I'm worried about phishing
Bot:   It's completely understandable to feel worried, Nokubonga.
       Be cautious of emails asking for personal information...

You:   I'm interested in privacy
Bot:   Great! I'll remember that you're interested in privacy.
       Review the privacy settings on all your social media accounts...

You:   Tell me more
Bot:   As someone interested in privacy, here's something important:
       Use a VPN when connecting to public Wi-Fi...
       
# Commit History
Commit Message

Initial commit: Set up WinForms project structure and added all source files |
Added MainForm GUI with dark cyberpunk theme, ASCII logo header and voice greeting on launch |
Implemented ResponseEngine with keyword recognition for password, phishing, scam, privacy and malware |
Added SentimentDetector to identify worried, frustrated, curious and happy moods with empathetic responses |
Implemented UserMemory to store user name and favourite topic with delegate-based follow-up conversation flow |
Added input validation, error handling for unknown inputs and documented all classes with XML comments |
Releases
| v2.0.0 | Part 2 - Initial GUI Release | WinForms GUI with ASCII logo, voice greeting, keyword recognition and dark theme | | v2.1.0 | Sentiment and Memory Added | SentimentDetector for mood detection and UserMemory for storing user details |
