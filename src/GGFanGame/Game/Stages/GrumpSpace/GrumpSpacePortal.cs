using GGFanGame.DataModel.Game;
using GGFanGame.Screens;
using GGFanGame.Screens.Game;
using Microsoft.Xna.Framework;
using static Core;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("portal", "grumpSpace", "main")]
    class GrumpSpacePortal : StageObject
    {
        private Vector3 _destPosition;
        private string _destination;
        private bool _canTrigger = true;

        public GrumpSpacePortal()
        {
            Collision = false;
            CanLandOn = false;
            IsVisualObject = false;
            Size = new Vector3(0.1f);
        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            base.ApplyDataModel(dataModel);

            _destPosition = dataModel.TryGetArg("toPos", Vector3.Zero).result;
            _destination = dataModel.TryGetArg("to", "").result;
        }

        public override void Update()
        {
            base.Update();
            if (ParentStage.OnePlayer.BoundingBox.Intersects(BoundingBox))
            {
                if (_canTrigger)
                {
                    _canTrigger = false;
                    ((GrumpSpaceScreen)GetComponent<ScreenManager>().CurrentScreen).InitiatePortal(_destPosition, _destination);
                }
            }
            else
            {
                _canTrigger = true;
            }
        }
    }
}
