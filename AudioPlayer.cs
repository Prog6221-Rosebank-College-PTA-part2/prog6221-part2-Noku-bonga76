using System.Media;
using System.Runtime.InteropServices;

namespace CybersecurityChatbotGUI
{
    public static class AudioPlayer
    {
        public static void PlayGreetingAsync()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return; 

            Task.Run(() =>
            {
                try
                { 
                    string wavPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Greeting.wav");

                    if (File.Exists(wavPath))
                    {
                        using SoundPlayer player = new(wavPath);
                        player.PlaySync();
                    }
                }
                catch
                {
                   
                }
            });
        }
    }
}
