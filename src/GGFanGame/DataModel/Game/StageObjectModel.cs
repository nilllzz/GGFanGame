using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using System;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace GGFanGame.DataModel.Game
{
    /// <summary>
    /// Model describing a <see cref="GGFanGame.Game.StageObject"/>.
    /// </summary>
    [DataContract]
    internal class StageObjectModel : DataModel<StageObjectModel>
    {
        [DataMember(Name = "x")]
        public double X;
        [DataMember(Name = "y")]
        public double Y;
        [DataMember(Name = "z")]
        public double Z;
        [DataMember(Name = "type")]
        public string Type;
        [DataMember(Name = "args")]
        public StageObjectArgumentModel[] Arguments;

        public Vector3 Position => new Vector3((float)X, (float)Y, (float)Z);

        internal bool HasArg(string key)
            => Arguments != null && Arguments.Any(a => a.Key == key);

        internal string GetArgValue(string key)
            => Arguments.First(a => a.Key == key).Value;

        internal (bool success, T result) TryGetArg<T>(string key, T fallback = default(T))
        {
            if (!HasArg(key))
            {
                return (false, fallback);
            }

            var strArg = GetArgValue(key);
            var tType = typeof(T);

            try
            {
                T arg;
                switch (System.Type.GetTypeCode(tType))
                {
                    case TypeCode.String:
                        arg = (T)Convert.ChangeType(strArg, tType);
                        break;
                    case TypeCode.Boolean when bool.TryParse(strArg, out var b):
                        arg = (T)Convert.ChangeType(b, tType);
                        break;
                    case TypeCode.Int32 when int.TryParse(strArg, out var i):
                        arg = (T)Convert.ChangeType(i, tType);
                        break;
                    case TypeCode.Double when double.TryParse(strArg, out var d):
                        arg = (T)Convert.ChangeType(d, tType);
                        break;
                    case TypeCode.Single when float.TryParse(strArg, out var s):
                        arg = (T)Convert.ChangeType(s, tType);
                        break;
                    case TypeCode.DateTime when DateTime.TryParse(strArg, out var dt):
                        arg = (T)Convert.ChangeType(dt, tType);
                        break;
                    case TypeCode.Object:
                        arg = (T)Convert.ChangeType(ParseObject(strArg, tType), tType);
                        break;

                    default:
                        arg = fallback;
                        break;
                }

                return (true, arg);
            }
            catch
            {
                return (false, fallback);
            }
        }

        private static object ParseObject(string value, Type tType)
        {
            switch (tType)
            {
                case Type vec3Type when vec3Type == typeof(Vector3):
                    {
                        var values = value.Split(',').Select(s => float.Parse(s)).ToArray();
                        return new Vector3(values[0], values[1], values[2]);
                    }
                case Type colorType when colorType == typeof(Color):
                    {
                        var values = value.Split(',').Select(s => int.Parse(s)).ToArray();
                        if (values.Length == 3)
                            return new Color(values[0], values[1], values[2]);
                        else
                            return new Color(values[0], values[1], values[2], values[3]);
                    }
            }

            throw new Exception();
        }
    }
}
