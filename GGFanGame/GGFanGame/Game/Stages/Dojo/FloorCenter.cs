﻿using GGFanGame.Content;
using GGFanGame.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Dojo
{
    [StageObject("floorSide", "grumpSpace", "dojo")]
    internal class FloorSide : SceneryObject
    {
        public FloorSide()
        {
            Size = new Vector3(64, 1, 64);
            DrawShadow = false;
            Collision = true;
            CanLandOn = true;
            GravityAffected = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 64), 100));
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet1 = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Levels.Dojo.WoodFloor));
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(RectangleComposer.Create(1f, 1f));
        }
    }
}
