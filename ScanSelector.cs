using SFML.System;
using SFML.Graphics;

namespace MapTool
{
    /// <summary>
    /// A tool to select discontiguous regions of a map based on a color
    /// </summary>
    public class ScanSelector(Map map)
    {
        private readonly Map map = map;

        public HashSet<Vector2u> SelectFill(in Color color)
        {
            var selection = new HashSet<Vector2u>();

            for (uint j = 0; j < map.Size.Y; j++)
            {
                for (uint i = 0; i < map.Size.X; i++)
                {
                    Vector2u currentPosition = new Vector2u(i, j);
                    Color currentColor = map.SamplePosition(currentPosition);

					if (currentColor != color) continue;
                    else selection.Add(currentPosition);
                }
            }

            return selection;
        }
    }
}
