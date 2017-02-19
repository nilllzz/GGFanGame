using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Dojo
{
    [StageObject("floorCenter", "grumpSpace", "dojo")]
    internal class FloorCenter : SceneryObject
    {
        public FloorCenter()
        {
            Size = new Vector3(32, 1, 96);
            DrawShadow = false;
            Collision = true;
            CanLandOn = true;
            GravityAffected = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(32, 96), 100));
            GroundRelation = GroundRelation.Flat;
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = ParentStage.Content.Load<Texture2D>(Resources.Levels.Dojo.Floor1);
        }
    }
}
