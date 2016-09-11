namespace GGFanGame.GameJolt.API
{
    /// <summary>
    /// A class to manage global GameJolt API settings.
    /// </summary>
    internal class APIManager
    {
        //The API Manager will be needed as singleton:
        private static APIManager _singleton;

        /// <summary>
        /// Returns the singleton instance of the APIManager.
        /// </summary>
        public static APIManager getInstance()
        {
            if (_singleton == null)
            {
                _singleton = new APIManager();
            }

            return _singleton;
        }

        public string username { get; set; }

        public string userToken { get; set; }

        public bool loggedIn { get; set; }

    }
}