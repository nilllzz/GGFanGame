namespace GGFanGame.GameJolt.API
{
    /// <summary>
    /// A class to manage global GameJolt API settings.
    /// </summary>
    class APIManager
    {
        //The API Manager will be needed as singleton:
        private static APIManager _singleton = null;

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

        private string _username = "";
        private string _user_token = "";
        private bool _loggedIn = false;

        public string username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string userToken
        {
            get { return _user_token; }
            set { _user_token = value; }
        }

        public bool loggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; }
        }

    }
}