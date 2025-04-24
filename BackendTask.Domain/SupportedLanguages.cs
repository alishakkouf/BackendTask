namespace BackendTask.Domain
{
    public static class SupportedLanguages
    {
        public static readonly List<string> ListAll = new List<string>
        {
            English,
            Arabic
        };

        public const string English = "en-GB";
        public const string Arabic = "ar-SY";
    }
}
