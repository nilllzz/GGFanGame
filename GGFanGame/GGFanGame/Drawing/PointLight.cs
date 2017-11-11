using Microsoft.Xna.Framework;

namespace GGFanGame.Drawing
{
    internal class PointLight
    {
        private float _intensity, _radius;
        private Color _color;
        private Vector3 _position;

        public bool IsDirty { get; set; }

        public float Intensity
        {
            get => _intensity;
            set
            {
                if (_intensity != value)
                {
                    _intensity = value;
                    IsDirty = true;
                }
            }
        }

        public float Radius
        {
            get => _radius;
            set
            {
                if (_radius != value)
                {
                    _radius = value;
                    IsDirty = true;
                }
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    IsDirty = true;
                }
            }
        }

        public Vector3 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    IsDirty = true;
                }
            }
        }

        public PointLight(Vector3 position = default(Vector3), Color color = default(Color), float radius = 1f, float intensity = 1f)
        {
            _position = position;
            _color = color;
            _radius = radius;
            _intensity = intensity;
        }
    }
}
