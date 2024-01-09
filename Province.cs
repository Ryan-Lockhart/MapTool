using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

using static MapTool.SFMLExtensions.Vector2fExtensions;
using static MapTool.SFMLExtensions.Vector2uExtensions;

namespace MapTool
{
    public struct Line
    {
        private Vector2u start;
        private Vector2u end;

        public Line(in Vector2u start, in Vector2u end)
        {
            this.start = start;
            this.end = end;
        }

        public Line(uint x1, uint y1, uint x2, uint y2)
        {
            start = new Vector2u(x1, y1);
            end = new Vector2u(x2, y2);
        }

        public Vector2u Start { get => start; set => start = value; }
        public Vector2u End { get => end; set => end = value; }

        private static bool OnSegment(in Vector2u p, in Vector2u q, in Vector2u r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }

        private static int Orientation(in Vector2u p, in Vector2u q, in Vector2u r)
        {
            int val = (int)((q.Y - p.X) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y));

            if (val == 0) return 0;

            return (val > 0) ? 1 : 2;
        }

        public static bool Intersect(in Line a, in Line b)
        {
            int o1 = Orientation(a.Start, a.End, b.Start);
            int o2 = Orientation(a.Start, a.End, b.End);
            int o3 = Orientation(b.Start, b.End, a.Start);
            int o4 = Orientation(b.Start, b.End, a.End);

            if (o1 != o2 && o3 != o4) return true;

            if (o1 == 0 && OnSegment(a.Start, b.Start, a.End)) return true;
            if (o2 == 0 && OnSegment(a.Start, b.End, a.End)) return true;
            if (o3 == 0 && OnSegment(b.Start, a.Start, b.End)) return true;
            if (o4 == 0 && OnSegment(b.Start, a.End, b.End)) return true;

            return false;
        }
    }

    public class Province : Drawable
    {
        private static readonly Vector2f nudge = new Vector2f(0.5f, 0.5f);

        private readonly Vertex[] vertices;

        private List<Vector2u> GenerateConcaveHull(HashSet<Vector2u> vertices, int neighbours)
        {
            neighbours = Math.Max(neighbours, 3);

            List<Vector2u> dataset = [.. vertices];

            if (dataset.Count < 3) throw new ArgumentException($"[{DateTime.Now}] ERROR: Insufficient vertices to construct polygon!");

            if (dataset.Count == 3) return dataset;

            neighbours = Math.Min(neighbours, dataset.Count);

            float minY = dataset.Min(vertex => vertex.Y);
            List<Vector2u> lowestValues = dataset.Where(vertex => vertex.Y == minY).ToList();

            float min = lowestValues.Min(vertex => MathF.Abs(vertex.X * vertex.X + vertex.Y * vertex.Y));
            var first = lowestValues.Where(vertex => MathF.Abs(vertex.X * vertex.X + vertex.Y * vertex.Y) == min).First();

            var hull = new List<Vector2u> { first };

            var current = first;
            dataset.Remove(first);

            while (dataset.Count > 0)
	        {
                Vector2u[] nearest = dataset
                    .OrderBy(v => (v - current).SqrMagnitude())
                    .Take(neighbours)
                    .OrderBy(v => (v - current).Angle())
                    .ToArray();

                if (hull.Count == 1)
                {
                    current = nearest.First();

                    goto pass;
                }

                for (int i = 0; i < nearest.Length; i++)
                {
                    bool intersection = false;

                    for (int j = hull.Count - 1; j > 0; j--)
                    {
                        intersection = Line.Intersect(new Line(current, nearest[i]), new Line(hull[j], hull[j - 1]));

                        if (intersection)
                            break;
                    }

                    if (intersection)
                        continue;

                    current = nearest[i];

                    goto pass;
                }

                return GenerateConcaveHull(vertices, ++neighbours);

                pass:

                dataset.Remove(current);
                hull.Add(current);

                continue;
            }

            if (dataset.Count > 0) return GenerateConcaveHull(vertices, ++neighbours);

            return hull;
        }


        public Province(HashSet<Vector2u> edge)
        {
            var center = Average(edge);

            var hull = GenerateConcaveHull([.. edge], 2048);

            vertices = new Vertex[hull.Count + 1];
            vertices[0] = new Vertex(center);

            for (int i = 0; i < hull.Count; i++)
                vertices[i + 1] = new Vertex((Vector2f)hull[i] + nudge);
        }

        public Vertex Center => vertices[0];

        public Vertex[] Edge => vertices.Take(new Range(1, vertices.Length)).ToArray();

        public void Draw(RenderTarget target, RenderStates states)
        {
            //target.Draw(vertices, 1, (uint)(vertices.Length - 1), PrimitiveType.LineStrip, states);
            target.Draw(vertices, PrimitiveType.LineStrip, states);
            //target.Draw(vertices, PrimitiveType.TriangleFan, states);
        }
    }
}
