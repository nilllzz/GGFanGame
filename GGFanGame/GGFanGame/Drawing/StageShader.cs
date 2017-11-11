using GameDevCommon.Rendering;
using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using static Core;

namespace GGFanGame.Drawing
{
    internal class StageShader : Shader
    {
        private Matrix _world, _view, _projection;
        private List<PointLight> _pointLights = new List<PointLight>();
        private bool _pointLightsDirty = false;

        public override Matrix World
        {
            set
            {
                _world = value;
                Effect.Parameters["World"].SetValue(_world);
            }
        }
        public override Matrix View { set => _view = value; }
        public override Matrix Projection { set => _projection = value; }

        public StageShader(ContentManager content)
            : base(content.Load<Effect>(Resources.Shaders.Default))
        {
            // default light
            SetDirectionalLight(new DirectionalLightConfiguration { Direction = Vector3.Zero, Color = Color.White, Intensity = 1f });
        }

        public override void Prepare(Camera camera)
        {
            base.Prepare(camera);

            if (_pointLightsDirty || _pointLights.Any(p => p.IsDirty))
            {
                _pointLightsDirty = false;
                _pointLights.ForEach(p => p.IsDirty = false);

                Effect.Parameters["PointLightPosition"].SetValue(_pointLights.Select(l => l.Position).ToArray());
                Effect.Parameters["PointLightColor"].SetValue(_pointLights.Select(l => l.Color.ToVector4()).ToArray());
                Effect.Parameters["PointLightIntensity"].SetValue(_pointLights.Select(l => l.Intensity).ToArray());
                Effect.Parameters["PointLightRadius"].SetValue(_pointLights.Select(l => l.Radius).ToArray());
                Effect.Parameters["MaxLightsRendered"].SetValue(_pointLights.Count);
            }

            Effect.Parameters["CameraPosition"].SetValue(camera.Position);
        }

        private EffectTechnique GetTechnique(I3DObject obj)
        {
            if (obj.Tag is string s)
                return Effect.Techniques[s];

            return Effect.Techniques["Default"];
        }

        protected override void RenderVertices(I3DObject obj)
        {
            var worldViewProj = _world * _view * _projection;
            Effect.Parameters["WorldViewProj"].SetValue(worldViewProj);
            Effect.Parameters["Alpha"].SetValue(obj.Alpha);
            Effect.Parameters["DiffuseTexture"].SetValue(obj.Texture);

            var technique = GetTechnique(obj);
            if (technique.Name != Effect.CurrentTechnique.Name)
                Effect.CurrentTechnique = technique;

            base.RenderVertices(obj);
        }

        internal void SetDirectionalLight(DirectionalLightConfiguration config)
        {
            Effect.Parameters["SunLightDirection"].SetValue(config.Direction);
            Effect.Parameters["SunLightColor"].SetValue(config.Color.ToVector4());
            Effect.Parameters["SunLightIntensity"].SetValue(config.Intensity);
        }

        internal void AddPointLight(PointLight light)
        {
            _pointLights.Add(light);
            _pointLightsDirty = true;
        }

        internal void RemovePointLight(PointLight light)
        {
            _pointLights.Remove(light);
            _pointLightsDirty = true;
        }

        internal void ClearLights()
        {
            _pointLights.Clear();
            _pointLightsDirty = true;
        }
    }
}
