
namespace CybersecurityChatbotGUI
{ 
    public class UserMemory
    {
        public string UserName { get; set; } = string.Empty;

        public string FavouriteTopic { get; set; } = string.Empty;
 
        public string LastTopic { get; set; } = string.Empty;

        public string LastSentiment { get; set; } = string.Empty;

        public bool HasFavouriteTopic => !string.IsNullOrWhiteSpace(FavouriteTopic);

        public bool HasLastTopic => !string.IsNullOrWhiteSpace(LastTopic);
    }
}
