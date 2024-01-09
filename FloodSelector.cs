using SFML.System;
using SFML.Graphics;

using static MapTool.SFMLExtensions.Vector2fExtensions;

namespace MapTool
{
    /// <summary>
    /// A tool to select contiguous regions of a map based on an origin point and its color
    /// </summary>
    public class FloodSelector(Map map)
    {
        private readonly Map map = map;

        private Color originColor;

        private Queue<Vector2u> frontier = new Queue<Vector2u>();
        private HashSet<Vector2u> visited = new HashSet<Vector2u>();

        public HashSet<Vector2u> SelectFill(in Vector2u origin)
        {
            originColor = map.SamplePosition(origin);

            if (originColor == Color.Black) throw new InvalidSelectionException("This region cannot be selected!");

            frontier.Clear();
            visited.Clear();

            frontier.Enqueue(origin);

            var selection = new HashSet<Vector2u>();

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                EnqueuNeighbours(current, fill: selection);
            }

            return selection;
        }

        public HashSet<Vector2u> SelectEdge(in Vector2u origin)
        {
            originColor = map.SamplePosition(origin);

            if (originColor == Color.Black) throw new InvalidSelectionException("This region cannot be selected!");

            frontier.Clear();
            visited.Clear();

            frontier.Enqueue(origin);

            var selection = new HashSet<Vector2u>();

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                EnqueuNeighbours(current, edge: selection);
            }

            return selection;
        }

        public (HashSet<Vector2u> fill, HashSet<Vector2u> edge) Select(in Vector2u origin)
        {
            originColor = map.SamplePosition(origin);

            if (originColor == Color.Black) throw new InvalidSelectionException("This region cannot be selected!");

            frontier.Clear();
            visited.Clear();

            frontier.Enqueue(origin);

            var fill = new HashSet<Vector2u>();
            var edge = new HashSet<Vector2u>();

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                EnqueuNeighbours(current, fill, edge);
            }

            return (fill, edge);
        }

        private void EnqueuNeighbours(in Vector2u origin, HashSet<Vector2u>? fill = null, HashSet<Vector2u>? edge = null)
        {
            Border scenario = (origin.X == 0 ? Border.Western : origin.X == map.Size.X ? Border.Eastern : Border.None) | (origin.Y == 0 ? Border.Northern : origin.Y == map.Size.Y ? Border.Southern : Border.None);

            (Vector2i min, Vector2i max) = scenario switch
            {
                Border.None => (new Vector2i(-1, -1), new Vector2i(1, 1)),

                Border.Northern => (new Vector2i(-1, 0), new Vector2i(1, 1)),
                Border.Southern => (new Vector2i(-1, -1), new Vector2i(1, 0)),
                Border.Western => (new Vector2i(0, -1), new Vector2i(1, 1)),
                Border.Eastern => (new Vector2i(-1, -1), new Vector2i(0, 1)),
                Border.Northeastern => (new Vector2i(-1, 0), new Vector2i(0, 1)),
                Border.Northwestern => (new Vector2i(0, 0), new Vector2i(1, 1)),
                Border.Southeastern => (new Vector2i(-1, -1), new Vector2i(0, 0)),
                Border.Southwestern => (new Vector2i(0, -1), new Vector2i(1, 0)),

                _ => throw new Exception("An invalid border was calculated!"),
            };

            for (int y_offset = min.Y; y_offset <= max.Y; y_offset++)
            {
                for (int x_offset = min.Y; x_offset <= max.X; x_offset++)
                {
                    Vector2u offsetPosition = new Vector2u((uint)(origin.X + x_offset), (uint)(origin.Y + y_offset));

                    if (visited.Contains(offsetPosition)) continue;

                    visited.Add(offsetPosition);

                    if (origin == offsetPosition)
                    {
                        fill?.Add(offsetPosition);
                        continue;
                    }

                    Color offsetColor = map.SamplePosition(offsetPosition);

                    if (originColor != offsetColor)
                    {
                        edge?.Add(offsetPosition);
                        continue;
                    }

                    fill?.Add(offsetPosition);
                    frontier.Enqueue(offsetPosition);
                }
            }
        }
    }
}
