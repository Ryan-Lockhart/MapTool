using SFML.System;
using SFML.Graphics;

using static MapTool.SFMLExtensions.Vector2fExtensions;
using static MapTool.SFMLExtensions.Vector2uExtensions;

namespace MapTool
{
    public enum Cardinal
    {
		/// <summary>Northwest</summary>
        NW,
        /// <summary>North</summary>
        N,
        /// <summary>Northeast</summary>
        NE,
        /// <summary>West</summary>
        W,
        /// <summary>Central</summary>
        C,
        /// <summary>East</summary>
        E,
        /// <summary>Southwest</summary>
        SW,
        /// <summary>South</summary>
        S,
        /// <summary>Southeast</summary>
        SE
    }

    public class Province : Drawable
    {
		private readonly VertexBuffer hull;
        private readonly VertexBuffer border;

        private readonly Vector2f center;

		private readonly (Vector2u min, Vector2u max) extents;
		private readonly Vector2u size;

        private Vector2f DirectionToVector(Cardinal direction) => direction switch
        {
            Cardinal.NW => new Vector2f(0.0f, 0.0f),
            Cardinal.N => new Vector2f(0.5f, 0.0f),
            Cardinal.NE => new Vector2f(1.0f, 0.0f),
            Cardinal.W => new Vector2f(0.0f, 0.5f),
            Cardinal.C => new Vector2f(0.5f, 0.5f),
            Cardinal.E => new Vector2f(1.0f, 0.5f),
            Cardinal.SW => new Vector2f(0.0f, 1.0f),
            Cardinal.S => new Vector2f(0.5f, 1.0f),
            Cardinal.SE => new Vector2f(1.0f, 1.0f),
            _ => throw new ArgumentException("Invalid direction!")
        };

        private enum TraceType
        {
            Center,
            Outer,
            Inner,
        }

        private void GenerateLineSegmentQuad(Vector2f start, Vector2f end, List<Vector2f> vertices, float thickness, TraceType traceType)
        {
            Vector2f direction = (end - start).Normalized();
            Vector2f perpendicular = new Vector2f(-direction.Y, direction.X);
            Vector2f offset = perpendicular * (thickness / 2.0f);

            start -= direction * thickness / MathF.PI;
            end += direction * thickness / MathF.PI;

            vertices.Add(start + offset);
            vertices.Add(end + offset);
            vertices.Add(end - offset);
            vertices.Add(start - offset);
        }

        private void Trace(Vector2f origin, byte config, List<Vector2f> vertices, float thickness = 0.50f, TraceType traceType = TraceType.Center)
        {
            switch (config)
            {
                case 0:
                case 15:
                    return;
                case 1:
                case 14:
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.W), origin + DirectionToVector(Cardinal.S), vertices, thickness, traceType);
                    return;
                case 2:
                case 13:
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.S), origin + DirectionToVector(Cardinal.E), vertices, thickness, traceType);
                    return;
                case 3:
                case 12:
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.W), origin + DirectionToVector(Cardinal.E), vertices, thickness, traceType);
                    return;
                case 4:
                case 11:
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.N), origin + DirectionToVector(Cardinal.E), vertices, thickness, traceType);
                    return;
                case 5:
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.W), origin + DirectionToVector(Cardinal.N), vertices, thickness, traceType);
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.E), origin + DirectionToVector(Cardinal.S), vertices, thickness, traceType);
                    return;
                case 6:
                case 9:
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.N), origin + DirectionToVector(Cardinal.S), vertices, thickness, traceType);
                    return;
                case 7:
                case 8:
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.N), origin + DirectionToVector(Cardinal.W), vertices, thickness, traceType);
                    return;
                case 10:
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.N), origin + DirectionToVector(Cardinal.E), vertices, thickness, traceType);
                    GenerateLineSegmentQuad(origin + DirectionToVector(Cardinal.W), origin + DirectionToVector(Cardinal.S), vertices, thickness, traceType);
                    return;
            }
        }

        private void Fill(Vector2f origin, byte config, List<Vector2f> vertices)
		{
			switch (config)
			{
				case 0: return;
				case 1:
					vertices.Add(origin + DirectionToVector(Cardinal.W));
					vertices.Add(origin + DirectionToVector(Cardinal.SW));
					vertices.Add(origin + DirectionToVector(Cardinal.S));
					return;
				case 2:
					vertices.Add(origin + DirectionToVector(Cardinal.S));
					vertices.Add(origin + DirectionToVector(Cardinal.SE));
					vertices.Add(origin + DirectionToVector(Cardinal.E));
					return;
				case 3:
					vertices.Add(origin + DirectionToVector(Cardinal.W));
					vertices.Add(origin + DirectionToVector(Cardinal.SW));
					vertices.Add(origin + DirectionToVector(Cardinal.SE));

                    vertices.Add(origin + DirectionToVector(Cardinal.W));
                    vertices.Add(origin + DirectionToVector(Cardinal.SE));
                    vertices.Add(origin + DirectionToVector(Cardinal.E));
					return;
				case 4:
					vertices.Add(origin + DirectionToVector(Cardinal.N));
					vertices.Add(origin + DirectionToVector(Cardinal.E));
					vertices.Add(origin + DirectionToVector(Cardinal.NE));
					return;
				case 5:
                    vertices.Add(origin + DirectionToVector(Cardinal.W));
                    vertices.Add(origin + DirectionToVector(Cardinal.SW));
                    vertices.Add(origin + DirectionToVector(Cardinal.S));

					vertices.Add(origin + DirectionToVector(Cardinal.W));
					vertices.Add(origin + DirectionToVector(Cardinal.S));
                    vertices.Add(origin + DirectionToVector(Cardinal.N));

					vertices.Add(origin + DirectionToVector(Cardinal.N));
					vertices.Add(origin + DirectionToVector(Cardinal.S));
					vertices.Add(origin + DirectionToVector(Cardinal.E));

                    vertices.Add(origin + DirectionToVector(Cardinal.N));
                    vertices.Add(origin + DirectionToVector(Cardinal.E));
                    vertices.Add(origin + DirectionToVector(Cardinal.NE));
                    return;
				case 6:
					vertices.Add(origin + DirectionToVector(Cardinal.N));
					vertices.Add(origin + DirectionToVector(Cardinal.S));
                    vertices.Add(origin + DirectionToVector(Cardinal.SE));

                    vertices.Add(origin + DirectionToVector(Cardinal.N));
                    vertices.Add(origin + DirectionToVector(Cardinal.SE));
                    vertices.Add(origin + DirectionToVector(Cardinal.NE));
                    return;
				case 7:
                    vertices.Add(origin + DirectionToVector(Cardinal.W));
                    vertices.Add(origin + DirectionToVector(Cardinal.SW));
                    vertices.Add(origin + DirectionToVector(Cardinal.SE));

                    vertices.Add(origin + DirectionToVector(Cardinal.N));
                    vertices.Add(origin + DirectionToVector(Cardinal.W));
                    vertices.Add(origin + DirectionToVector(Cardinal.SE));

					vertices.Add(origin + DirectionToVector(Cardinal.N));
                    vertices.Add(origin + DirectionToVector(Cardinal.NE));
					vertices.Add(origin + DirectionToVector(Cardinal.SE));
					return;
				case 8:
					vertices.Add(origin + DirectionToVector(Cardinal.NW));
					vertices.Add(origin + DirectionToVector(Cardinal.W));
					vertices.Add(origin + DirectionToVector(Cardinal.N));
					return;
				case 9:
					vertices.Add(origin + DirectionToVector(Cardinal.NW));
					vertices.Add(origin + DirectionToVector(Cardinal.SW));
					vertices.Add(origin + DirectionToVector(Cardinal.S));

                    vertices.Add(origin + DirectionToVector(Cardinal.NW));
                    vertices.Add(origin + DirectionToVector(Cardinal.S));
                    vertices.Add(origin + DirectionToVector(Cardinal.N));
                    return;
				case 10:
                    vertices.Add(origin + DirectionToVector(Cardinal.NW));
                    vertices.Add(origin + DirectionToVector(Cardinal.W));
                    vertices.Add(origin + DirectionToVector(Cardinal.N));

					vertices.Add(origin + DirectionToVector(Cardinal.W));
					vertices.Add(origin + DirectionToVector(Cardinal.S));
                    vertices.Add(origin + DirectionToVector(Cardinal.N));

					vertices.Add(origin + DirectionToVector(Cardinal.N));
					vertices.Add(origin + DirectionToVector(Cardinal.S));
					vertices.Add(origin + DirectionToVector(Cardinal.E));

                    vertices.Add(origin + DirectionToVector(Cardinal.S));
                    vertices.Add(origin + DirectionToVector(Cardinal.SE));
                    vertices.Add(origin + DirectionToVector(Cardinal.E));
                    return;
				case 11:
					vertices.Add(origin + DirectionToVector(Cardinal.NW));
					vertices.Add(origin + DirectionToVector(Cardinal.SW));
					vertices.Add(origin + DirectionToVector(Cardinal.N));

					vertices.Add(origin + DirectionToVector(Cardinal.N));
					vertices.Add(origin + DirectionToVector(Cardinal.SW));
					vertices.Add(origin + DirectionToVector(Cardinal.E));

                    vertices.Add(origin + DirectionToVector(Cardinal.E));
                    vertices.Add(origin + DirectionToVector(Cardinal.SW));
                    vertices.Add(origin + DirectionToVector(Cardinal.SE));
                    return;
				case 12:
                    vertices.Add(origin + DirectionToVector(Cardinal.NW));
                    vertices.Add(origin + DirectionToVector(Cardinal.W));
                    vertices.Add(origin + DirectionToVector(Cardinal.E));

                    vertices.Add(origin + DirectionToVector(Cardinal.NW));
					vertices.Add(origin + DirectionToVector(Cardinal.E));
					vertices.Add(origin + DirectionToVector(Cardinal.NE));
                    return;
				case 13:
                    vertices.Add(origin + DirectionToVector(Cardinal.NW));
                    vertices.Add(origin + DirectionToVector(Cardinal.SW));
                    vertices.Add(origin + DirectionToVector(Cardinal.S));

                    vertices.Add(origin + DirectionToVector(Cardinal.NW));
                    vertices.Add(origin + DirectionToVector(Cardinal.S));
                    vertices.Add(origin + DirectionToVector(Cardinal.E));

                    vertices.Add(origin + DirectionToVector(Cardinal.NW));
					vertices.Add(origin + DirectionToVector(Cardinal.E));
					vertices.Add(origin + DirectionToVector(Cardinal.NE));
					return;
				case 14:
					vertices.Add(origin + DirectionToVector(Cardinal.NW));
					vertices.Add(origin + DirectionToVector(Cardinal.W));
					vertices.Add(origin + DirectionToVector(Cardinal.NE));

					vertices.Add(origin + DirectionToVector(Cardinal.W));
					vertices.Add(origin + DirectionToVector(Cardinal.S));
					vertices.Add(origin + DirectionToVector(Cardinal.NE));

                    vertices.Add(origin + DirectionToVector(Cardinal.S));
                    vertices.Add(origin + DirectionToVector(Cardinal.SE));
                    vertices.Add(origin + DirectionToVector(Cardinal.NE));
                    return;
				case 15:
                    vertices.Add(origin + DirectionToVector(Cardinal.NW));
                    vertices.Add(origin + DirectionToVector(Cardinal.SW));
                    vertices.Add(origin + DirectionToVector(Cardinal.SE));

                    vertices.Add(origin + DirectionToVector(Cardinal.NW));
					vertices.Add(origin + DirectionToVector(Cardinal.SE));
					vertices.Add(origin + DirectionToVector(Cardinal.NE));
					return;
			}
		}

		private (Vector2u max_extent, Vector2u min_extent) FindExtents(IEnumerable<Vector2u> vertices)
			=> (new Vector2u(vertices.Min(x => x.X), vertices.Min(x => x.Y)), new Vector2u(vertices.Max(x => x.X), vertices.Max(x => x.Y)));

        public Province(in Color color, HashSet<Vector2u> points)
        {
            center = Average(points);

            extents = FindExtents(points);

			size = extents.max - extents.min + new Vector2u(1, 1);

            var hull = new List<Vector2f>();
            var border = new List<Vector2f>();

            var grid = new bool[Size.X + 2, Size.Y + 2];

            for (uint j = 1; j <= Size.Y; ++j)
                for (uint i = 1; i <= Size.X; ++i)
                    grid[i, j] = points.Contains(Extents.Min + new Vector2u(i - 1, j - 1));

            for (uint j = 0; j < Size.Y + 1; ++j)
            {
                for (uint i = 0; i < Size.X + 1; ++i)
                {
                    byte config = 0;

                    if (grid[i, j]) config += 8;
                    if (grid[i + 1, j]) config += 4;
                    if (grid[i + 1, j + 1]) config += 2;
                    if (grid[i, j + 1]) config += 1;

                    var position = new Vector2f(i + Extents.Min.X - 0.5f, j + Extents.Min.Y - 0.5f);

                    Fill(position, config, hull);
                    Trace(position, config, border);
                }
            }

            var hull_buffer = new Vertex[hull.Count];

			for (int i = 0; i < hull.Count; ++i)
                hull_buffer[i] = new Vertex(hull[i], color);

            this.hull = new VertexBuffer((uint)hull.Count, PrimitiveType.Triangles, VertexBuffer.UsageSpecifier.Static);
            this.hull.Update(hull_buffer);

            var border_buffer = new Vertex[border.Count];

            for (int i = 0; i < border.Count; ++i)
                border_buffer[i] = new Vertex(border[i], Color.Black);

            this.border = new VertexBuffer((uint)border.Count, PrimitiveType.Quads, VertexBuffer.UsageSpecifier.Static);
            this.border.Update(border_buffer);
        }

        public Vector2f Center => center;
		public (Vector2u Min, Vector2u Max) Extents => extents;
		public Vector2u Size => size;

        public void Draw(RenderTarget target, RenderStates states)
		{
            hull.Draw(target, states);
            border.Draw(target, states);
        }

        public void DrawDebug(RenderTarget target) => target.Draw(new RectangleShape((Vector2f)Size) { Position = (Vector2f)Extents.Min, OutlineThickness = 1.5f, FillColor = Color.Transparent, OutlineColor = Color.Black });
    }
}
