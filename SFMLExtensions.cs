using SFML.System;

namespace MapTool
{
    public static class SFMLExtensions
    {
        public static void Clamp(this ref uint clampedValue, int value, uint min, uint max)
        {
            if (value < min) clampedValue = min;
            else if (value > max) clampedValue = max;
            else clampedValue = (uint)value;
        }

        public static void Clamp(this ref int clampedValue, int value, int min, int max)
        {
            if (value < min) clampedValue = min;
            else if (value > max) clampedValue = max;
            else clampedValue = value;
        }

        public static void Clamp(this ref uint clampedValue, uint value, uint min, uint max)
        {
            if (value < min) clampedValue = min;
            else if (value > max) clampedValue = max;
            else clampedValue = value;
        }

        public static void Clamp(this ref float clampedValue, float value, float min, float max)
        {
            if (value < min) clampedValue = min;
            else if (value > max) clampedValue = max;
            else clampedValue = value;
        }

        public static void Clamp(this ref Vector2f vec, in Vector2f value, in Vector2f min, in Vector2f max)
        {
            vec.X.Clamp(value.X, min.X, max.X);
            vec.Y.Clamp(value.Y, min.Y, max.Y);
        }

        public static float Magnitude(this in Vector2f vec) => MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y);

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
