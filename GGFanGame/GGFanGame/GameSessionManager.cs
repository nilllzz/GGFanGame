using System;
using System.IO;
using GGFanGame.DataModel;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;

namespace GGFanGame
{
    /// <summary>
    /// This class represents a save game.
    /// </summary>
    internal class GameSessionManager : IGameComponent
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
        
        /// <summary>
        /// Indicates wether this game session has been loaded correctly.
        /// </summary>
        public bool LoadedCorrectly { get; private set; }

        private static string SaveFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save.json");
        
        internal void Load()
        {
            if (!File.Exists(SaveFilePath))
            {
                // create.
            }

            var jsonData = File.ReadAllText(SaveFilePath);

            try
            {
                _dataModel = DataModel<GameSessionModel>.FromString(jsonData, DataType.Json);
                LoadedCorrectly = true;
            }
            catch (DataLoadException)
            {
                //TODO: Log error!
                LoadedCorrectly = false;
            }
        }

        public void Initialize()
        { }
    }
}
