using System;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Lighting
{
    internal struct Cone
    {
        private readonly Vector3 _apexPosition, _basePosition;
        private readonly float _aperture;

        internal Cone(Vector3 apexPosition, Vector3 basePosition, float aperture)
        {
            _apexPosition = apexPosition;
            _basePosition = basePosition;
            _aperture = aperture;
        }

        internal bool Contains(Vector3 point)
        {
            float halfAperture = _aperture / 2.0f;
            Vector3 apexToXVect = _apexPosition - point;
            Vector3 axisVect = _apexPosition - _basePosition;

            var isInInfiniteCone = Vector3.Dot(apexToXVect, axisVect)
                / Magn(apexToXVect) / Magn(axisVect)
                > Math.Cos(halfAperture);

            if (!isInInfiniteCone) return false;

            var isUnderRoundCap = Vector3.Dot(apexToXVect, axisVect) / Magn(axisVect) < Magn(axisVect);

            return isUnderRoundCap;
        }

        private static float Magn(Vector3 v)
            => (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
    }
}
