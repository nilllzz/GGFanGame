using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Stages.Default
{
    [StageObject("pointLight", "default", "lights")]
    internal class PointLight : SceneryObject
    {
        private Color _color;
        private float _radius;
        private float _intensity;

        public PointLight()
        {
            Collision = false;
            GravityAffected = false;
            IsVisualObject = false;
        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            base.ApplyDataModel(dataModel);

            _color = dataModel.TryGetArg("color", Color.White).result;
            _radius = dataModel.TryGetArg("radius", 1f).result;
            _intensity = dataModel.TryGetArg("intensity", 1f).result;
        }

        protected override void LoadContentInternal()
        {
            ParentStage.AddPointLight(new Drawing.PointLight(Position, _color, _radius, _intensity));
        }
    }
}
