using System.IO;
using GGFanGame.DataModel;
using GGFanGame.DataModel.Game;

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
        public string Name
        {
            get { return _dataModel.Name; }
            set { _dataModel.Name = value; }
        }

        /// <summary>
        /// The progress (in %) the player has made.
        /// </summary>
        public decimal Progress
        {
            get { return _dataModel.Progress; }
            set { _dataModel.Progress = value; }
        }

        /// <summary>
        /// The last grump used for this save.
        /// </summary>
        public string LastGrump
        {
            get { return _dataModel.LastGrump; }
            set { _dataModel.LastGrump = value; }
        }

        private bool _loadedCorrectly;

        /// <summary>
        /// Indicates wether this game session has been loaded correctly.
        /// </summary>
        public bool LoadedCorrectly => _loadedCorrectly;

        public GameSession(string fileName)
        {
            var jsonData = File.ReadAllText(fileName);

            try
            {
                _dataModel = DataModel<GameSessionModel>.FromString(jsonData, DataType.Json);
                _loadedCorrectly = true;
            }
            catch (DataLoadException)
            {
                //TODO: Log error!
                _loadedCorrectly = false;
            }
        }
    }
}