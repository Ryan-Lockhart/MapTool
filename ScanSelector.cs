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

            for (uint y = 0; y < map.Size.Y; y++)
            {
                for (uint x = 0; x < map.Size.X; x++)
                {
                    Vector2u currentPosition = new Vector2u(x, y);
                    Color currentColor = map.SamplePosition(currentPosition);

					if (currentColor != color) continue;
                    else selection.Add(currentPosition);
                }
            }

            return selection;
        }        
    }
}
