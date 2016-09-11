using System.IO;
using GGFanGame.DataModel.Json;
using GGFanGame.DataModel.Json.Game;

namespace GGFanGame
{
    /// <summary>
    /// This class represents a save game.
    /// </summary>
    internal class GameSession
    {
        //This stores a data model that can be loaded from json data in a file.
        private GameSessionModel _dataModel;

        /// <summary>
        /// The name of this save game as it appears in the menu.
        /// </summary>
        public string name
        {
            get { return _dataModel.name; }
            set { _dataModel.name = value; }
        }

        /// <summary>
        /// The progress (in %) the player has made.
        /// </summary>
        public decimal progress
        {
            get { return _dataModel.progress; }
            set { _dataModel.progress = value; }
        }

        /// <summary>
        /// The last grump used for this save.
        /// </summary>
        public string lastGrump
        {
            get { return _dataModel.lastGrump; }
            set { _dataModel.lastGrump = value; }
        }

        private bool _loadedCorrectly;

        /// <summary>
        /// Indicates wether this game session has been loaded correctly.
        /// </summary>
        public bool loadedCorrectly => _loadedCorrectly;

        public GameSession(string fileName)
        {
            var jsonData = File.ReadAllText(fileName);

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