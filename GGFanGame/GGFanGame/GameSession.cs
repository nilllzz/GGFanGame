using System.IO;
using GGFanGame.DataModel.Json;
using GGFanGame.DataModel.Json.Game;

namespace GGFanGame
{
    /// <summary>
    /// This class represents a save game.
    /// </summary>
    class GameSession
    {
        //This stores a data model that can be loaded from json data in a file.
        private GameSessionModel _dataModel = null;

        /// <summary>
        /// The name of this save game as it appears in the menu.
        /// </summary>
        /// <returns></returns>
        public string name
        {
            get { return _dataModel.name; }
            set { _dataModel.name = value; }
        }

        public decimal progress
        {
            get { return _dataModel.progress; }
            set { _dataModel.progress = value; }
        }

        private bool _loadedCorrectly = false;
        /// <summary>
        /// Indicates wether this game session has been loaded correctly.
        /// </summary>
        /// <returns></returns>
        public bool loadedCorrectly
        {
            get { return _loadedCorrectly; }
        }


        public GameSession(string fileName)
        {
            string jsonData = File.ReadAllText(fileName);

            try
            {
                _dataModel = JsonDataModel.fromString<GameSessionModel>(jsonData);
                _loadedCorrectly = true;
            }
            catch (JsonDataLoadException)
            {
                //TODO: Log error!
                _loadedCorrectly = false;
            }
        }
    }
}