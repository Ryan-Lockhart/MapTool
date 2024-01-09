using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

namespace MapTool
{
    public static class SFMLExtensions
    {
        public static class Vector2fExtensions
        {
            public static Vector2f Min(Vector2f vec, in Vector2f min)
            {
                vec.X = float.Min(vec.X, min.X);
                vec.Y = float.Min(vec.Y, min.Y);

                return vec;
            }

            public static Vector2f Max(Vector2f vec, in Vector2f max)
            {
                vec.X = float.Max(vec.X, max.X);
                vec.Y = float.Max(vec.Y, max.Y);

                return vec;
            }
            public static Vector2f Clamp(Vector2f vec, in Vector2f min, in Vector2f max)
            {
                if (min == max) return min;

                vec.X = float.Clamp(vec.X, min.X, max.X);
                vec.Y = float.Clamp(vec.Y, min.Y, max.Y);

                return vec;
            }

            public static Vector2f Average(params Vector2f[] vectors) => vectors.Aggregate((a, b) => a + b) / vectors.Length;
            public static Vector2f Average(in Vector2f[] vectors) => vectors.Aggregate((a, b) => a + b) / vectors.Length;
            public static Vector2f Average(List<Vector2f> vectors) => vectors.Aggregate((a, b) => a + b) / vectors.Count;
            public static Vector2f Average(HashSet<Vector2f> vectors) => vectors.Aggregate((a, b) => a + b) / vectors.Count;
        }

        public static class Vector2uExtensions
        {
            public static Vector2u Min(Vector2u vec, in Vector2u min)
            {
                vec.X = uint.Min(vec.X, min.X);
                vec.Y = uint.Min(vec.Y, min.Y);

                return vec;
            }

            public static Vector2u Max(Vector2u vec, in Vector2u max)
            {
                vec.X = uint.Max(vec.X, max.X);
                vec.Y = uint.Max(vec.Y, max.Y);

                return vec;
            }
            public static Vector2u Clamp(Vector2u vec, in Vector2u min, in Vector2u max)
            {
                if (min == max) return min;

                vec.X = uint.Clamp(vec.X, min.X, max.X);
                vec.Y = uint.Clamp(vec.Y, min.Y, max.Y);

                return vec;
            }

            public static Vector2f Average(params Vector2u[] vectors) => (Vector2f)vectors.Aggregate((a, b) => a + b) / vectors.Length;
            public static Vector2f Average(in Vector2u[] vectors) => (Vector2f)vectors.Aggregate((a, b) => a + b) / vectors.Length;
            public static Vector2f Average(List<Vector2u> vectors) => (Vector2f)vectors.Aggregate((a, b) => a + b) / vectors.Count;
            public static Vector2f Average(HashSet<Vector2u> vectors) => (Vector2f)vectors.Aggregate((a, b) => a + b) / vectors.Count;
        }

        public static bool Exceeds(this ref Vector2f vec, in Vector2f max) => vec.X == float.Max(vec.X, max.X) || vec.Y == float.Max(vec.Y, max.Y);

        public static void Min(this ref Vector2f vec, in Vector2f min)
        {
            vec.X = float.Min(vec.X, min.X);
            vec.Y = float.Min(vec.Y, min.Y);
        }

        public static void Max(this ref Vector2f vec, in Vector2f max)
        {
            vec.X = float.Min(vec.X, max.X);
            vec.Y = float.Min(vec.Y, max.Y);
        }

        public static void Clamp(this ref Vector2f vec, in Vector2f min, in Vector2f max)
        {
            if (min == max) vec = min;

            vec.X = float.Clamp(vec.X, min.X, max.X);
            vec.Y = float.Clamp(vec.Y, min.Y, max.Y);
        }

        public static float Magnitude(this in Vector2f vec) => MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
        public static float Magnitude(this in Vector2u vec) => MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
        public static float SqrMagnitude(this in Vector2f vec) => vec.X * vec.X + vec.Y * vec.Y;
        public static float SqrMagnitude(this in Vector2u vec) => vec.X * vec.X + vec.Y * vec.Y;

        public static float Angle(this in Vector2f vec) => MathF.Atan2(vec.Y, vec.X);
        public static float Angle(this in Vector2u vec) => MathF.Atan2(vec.Y, vec.X);

        public static (float, Vector2f) MagnitudeAndDirection(this Vector2f vec)
        {
            float magnitude = vec.Magnitude();

            vec.X /= magnitude;
            vec.Y /= magnitude;

            return (magnitude, vec);
        }

        public static void Normalize(this ref Vector2f vec)
        {
            float magnitude = vec.Magnitude();

            vec.X /= magnitude;
            vec.Y /= magnitude;
        }

        public static Vector2f Normalized(this Vector2f vec)
        {
            float magnitude = vec.Magnitude();

            vec.X /= magnitude;
            vec.Y /= magnitude;

            return vec;
        }
    }
}
