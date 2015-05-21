using System;

namespace GGFanGame.GameJolt.API
{
    sealed partial class GameJoltRequest
    {
        //This file is the second part of the partial GameJoltRequest class.
        //The actual calls to the API with their url parameters will be defined here.

        private static bool loggedIn
        {
            get { return APIManager.getInstance().loggedIn; }
        }
        private static string username
        {
            get { return APIManager.getInstance().username; }
        }
        private static string user_token
        {
            get { return APIManager.getInstance().userToken; }
        }

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
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static GameJoltRequest VerifyUser(string username, string token)
        {
            GameJoltRequest request = new GameJoltRequest(RequestType.GET, "/users/auth/");

            request.addUrlParameter("username", username);
            request.addUrlParameter("user_token", token);

            return request;
        }

        //TODO: Add all the needed GameJolt requests here.
        //Documentation: https://github.com/gamejolt/doc-game-api/tree/master/v1.x

        #endregion
    }
}