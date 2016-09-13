using System;
using System.Collections.Generic;
using System.Linq;
using GGFanGame.DataModel;
using GGFanGame.DataModel.Json.Game;
using Microsoft.Xna.Framework.Content;

namespace GGFanGame.Game
{
    internal static class StageFactory
    {
        private static Dictionary<string, Type> _stageObjectBuffer;
        private static StageListModel _stageList;

        private static void createBuffer()
        {
            if (_stageObjectBuffer == null)
            {
                _stageObjectBuffer = typeof(StageFactory).Assembly.GetTypes()
                    .Where(t => t.GetCustomAttributes(typeof(StageObjectAttribute), false).Length == 1)
                    .ToDictionary(t => ((StageObjectAttribute)t.GetCustomAttributes(typeof(StageObjectAttribute), false)[0]).stageInformation,
                                  t => t);
            }
        }
        
        private static void loadStageList(ContentManager content)
        {
            if (_stageList == null)
                _stageList = content.Load<StageListModel>(@"Levels\stages.json", DataType.All);
        }

        private static StageModel loadDataModel(ContentManager content, string worldId, string stageId)
        {
            loadStageList(content);

            var path = _stageList.stages.FirstOrDefault(e => e.stageId == stageId && e.worldId == worldId).path;
            if (path != null)
                return content.Load<StageModel>(path, DataType.All);
            else
                return new StageModel(); //TODO: throw.
        }

        public static Stage create(ContentManager content, string worldId, string stageId)
        {
            createBuffer();

            var dataModel = loadDataModel(content, worldId, stageId);

            var objects = dataModel.objects.Select(o =>
            {
                var type = _stageObjectBuffer[o.type];
                var obj = Activator.CreateInstance(type) as StageObject;
                obj.applyDataModel(o);

                return obj;
            });

            var stage = new Stage(content, objects);

            return stage;
        }
    }
}
