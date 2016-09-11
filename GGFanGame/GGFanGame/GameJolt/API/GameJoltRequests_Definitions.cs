using System;

namespace GGFanGame.GameJolt.API
{
    internal sealed partial class GameJoltRequest
    {
        //This file is the second part of the partial GameJoltRequest class.
        //The actual calls to the API with their url parameters will be defined here.

        private static bool loggedIn => APIManager.getInstance().loggedIn;

        private static string username => APIManager.getInstance().username;

        private static string user_token => APIManager.getInstance().userToken;

        private static void throwUserNotLoggedInException()
        {
            throw new ArgumentException("An API request requires the user to be logged in.");
        }

        private static void addUserCredentials(ref GameJoltRequest request)
        {
            if (!loggedIn)
                throwUserNotLoggedInException();
            else
            {
                request.addUrlParameter("username", username);
                request.addUrlParameter("user_token", user_token);
            }
        }

        #region User

        /// <summary>
        /// Verifies if the input username and token are a correct username/user_token pair. 
        /// </summary>
        public static GameJoltRequest verifyUser(string username, string token)
        {
            var request = new GameJoltRequest(RequestType.GET, "/users/auth/");

            request.addUrlParameter("username", username);
            request.addUrlParameter("user_token", token);

            return request;
        }

        /// <summary>
        /// Fetches data for a user from the server.
        /// </summary>
        public static GameJoltRequest fetchUserData(string username)
        {
            var request = new GameJoltRequest(RequestType.GET, "/users/");

            request.addUrlParameter("username", username);

            return request;
        }

        /// <summary>
        /// Fetches data for a user from the server by their GameJolt Id.
        /// </summary>
        public static GameJoltRequest fetchUserDataById(string gameJoltId)
        {
            var request = new GameJoltRequest(RequestType.GET, "/users/");

            request.addUrlParameter("user_id", gameJoltId);

            return request;
        }

        #endregion

        #region Storage

        public static GameJoltRequest setStorageData(string key, string data, bool userSpace)
        {
            var request = new GameJoltRequest(RequestType.POST, "/data-store/set/");
            if (userSpace)
                addUserCredentials(ref request);

            request.addUrlParameter("key", key);
            request._postData = data;

            return request;
        }

        public static GameJoltRequest updateStorageData(string key, string oValue, string operation, bool userSpace)
        {
            var request = new GameJoltRequest(RequestType.GET, "/data-store/update/");
            if (userSpace)
                addUserCredentials(ref request);

            request.addUrlParameter("key", key);
            request.addUrlParameter("operation", operation);
            request.addUrlParameter("value", oValue);

            return request;
        }

        public static GameJoltRequest getStorageData(string key, bool userSpace)
        {
            var request = new GameJoltRequest(RequestType.GET, "/data-store/get/");
            if (userSpace)
                addUserCredentials(ref request);

            request.addUrlParameter("key", key);

            return request;
        }

        public static GameJoltRequest getKeys(string pattern, bool userSpace)
        {
            var request = new GameJoltRequest(RequestType.GET, "/data-store/get-keys/");
            if (userSpace)
                addUserCredentials(ref request);

            request.addUrlParameter("pattern", pattern);

            return request;
        }

        public static GameJoltRequest removeStorageData(string key, bool userSpace)
        {
            var request = new GameJoltRequest(RequestType.GET, "/data-store/remove/");
            if (userSpace)
                addUserCredentials(ref request);

            request.addUrlParameter("key", key);

            return request;
        }

        #endregion

        //TODO: Add all the needed GameJolt requests here.
        //Documentation: https://github.com/gamejolt/doc-game-api/tree/master/v1.x
    }
}