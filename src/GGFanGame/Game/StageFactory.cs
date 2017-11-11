using System;
using System.Collections.Generic;
using System.Linq;
using GGFanGame.DataModel;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework.Content;

namespace GGFanGame.Game
{
    /// <summary>
    /// Factory class to produce <see cref="Stage"/>s.
    /// </summary>
    internal static class StageFactory
    {
        private static Dictionary<string, Type> _stageObjectBuffer;
        private static StageListModel _stageList;

        private static void CreateBuffer()
        {
            // creates a buffer of all stage objects that can be loaded from a file.
            // search through all types of this assembly and find all classes with a StageObjectAttribute attached to them.

            if (_stageObjectBuffer == null)
            {
                _stageObjectBuffer = typeof(StageFactory).Assembly.GetTypes()
                    .Where(t => t.GetCustomAttributes(typeof(StageObjectAttribute), false).Length == 1)
                    .ToDictionary(t => ((StageObjectAttribute)t.GetCustomAttributes(typeof(StageObjectAttribute), false)[0]).StageInformation,
                                  t => t);
            }
        }
        
        private static void LoadStageList(ContentManager content)
        {
            if (_stageList == null)
                _stageList = content.Load<StageListModel>(@"Levels\stages", DataType.Json);
        }

        private static StageModel LoadDataModel(ContentManager content, string worldId, string stageId)
        {
            LoadStageList(content);

            var path = _stageList.Stages.FirstOrDefault(e => e.StageId == stageId && e.WorldId == worldId).Path;
            if (path != null)
                return content.Load<StageModel>(path, DataType.Json);
            else
                return new StageModel(); //TODO: throw.
        }

        /// <summary>
        /// Creates a new <see cref="Stage"/> instance.
        /// </summary>
        /// <param name="content">The content manager to be associated with the stage.</param>
        /// <param name="worldId">The world id of the stage.</param>
        /// <param name="stageId">The stage id of the stage.</param>
        public static Stage Create(ContentManager content, string worldId, string stageId)
        {
            CreateBuffer();

            var dataModel = LoadDataModel(content, worldId, stageId);

            var objects = dataModel.Scenes[0].Objects.Select(o =>
            {
                var type = _stageObjectBuffer[o.Type];
                var obj = Activator.CreateInstance(type) as StageObject;
                obj.ApplyDataModel(o);

                return obj;
            });

            var stage = new Stage(content, objects, dataModel);
            return stage;
        }
    }
}
