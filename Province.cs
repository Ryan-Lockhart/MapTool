using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

using static MapTool.SFMLExtensions.Vector2fExtensions;
using static MapTool.SFMLExtensions.Vector2uExtensions;
using System.Data;

namespace MapTool
{
    public enum Cardinal
    {
        Northeast,
        North,
        Northwest,
        East,
        Central,
        West,
        Southeast,
        South,
        Southwest
    }

	public class Province : Drawable
    {
		private readonly Vertex[] vertices;

		private readonly Vertex center;

		private Vector2f DirectionToVector(Cardinal direction) => direction switch
		{
			Cardinal.Northwest => new Vector2f(0.0f, 0.0f),
			Cardinal.North => new Vector2f(0.5f, 0.0f),
			Cardinal.Northeast => new Vector2f(1.0f, 0.0f),
			Cardinal.West => new Vector2f(0.0f, 0.5f),
            Cardinal.Central => new Vector2f(0.5f, 0.5f),
			Cardinal.East => new Vector2f(1.0f, 0.5f),
			Cardinal.Southwest => new Vector2f(0.0f, 1.0f),
			Cardinal.South => new Vector2f(0.5f, 1.0f),
			Cardinal.Southeast => new Vector2f(1.0f, 1.0f),
			_ => throw new ArgumentException("Invalid direction!")
		};

		private void AddCase(Vector2f origin, byte config, List<Vector2f> vertices)
		{
			switch (config)
			{
				case 0: return;
				case 1:
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.Southwest));
					vertices.Add(origin + DirectionToVector(Cardinal.South));
					return;
				case 2:
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));
					vertices.Add(origin + DirectionToVector(Cardinal.Southeast));
					return;
				case 3:
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 1, vertices);
					AddCase(origin, 2, vertices);
					return;
				case 4:
					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.Northeast));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					return;
				case 5:
					AddCase(origin, 4, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 1, vertices);
					return;
				case 6:
					AddCase(origin, 4, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 2, vertices);
					return;
				case 7:
					AddCase(origin, 4, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 1, vertices);
					AddCase(origin, 2, vertices);
					return;
				case 8:
					vertices.Add(origin + DirectionToVector(Cardinal.Northwest));
					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					return;
				case 9:
					AddCase(origin, 8, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 1, vertices);
					return;
				case 10:
					AddCase(origin, 8, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 2, vertices);
					return;
				case 11:
					AddCase(origin, 8, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 1, vertices);
					AddCase(origin, 2, vertices);
					return;
				case 12:
					AddCase(origin, 8, vertices);
					AddCase(origin, 4, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					return;
				case 13:
					AddCase(origin, 8, vertices);
					AddCase(origin, 4, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 1, vertices);
					return;
				case 14:
					AddCase(origin, 8, vertices);
					AddCase(origin, 4, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 2, vertices);
					return;
				case 15:
					AddCase(origin, 8, vertices);
					AddCase(origin, 4, vertices);

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.West));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					vertices.Add(origin + DirectionToVector(Cardinal.North));
					vertices.Add(origin + DirectionToVector(Cardinal.East));
					vertices.Add(origin + DirectionToVector(Cardinal.South));

					AddCase(origin, 1, vertices);
					AddCase(origin, 2, vertices);
					return;
			}
		}

		private (Vector2u max_extent, Vector2u min_extent) FindExtents(IEnumerable<Vector2u> vertices)
			=> (new Vector2u(vertices.Min(x => x.X), vertices.Min(x => x.Y)), new Vector2u(vertices.Max(x => x.X), vertices.Max(x => x.Y)));

		private List<Vector2f> GenerateConcaveHull(List<Vector2u> dataset)
        {
			(var min_extent, var max_extent) = FindExtents(dataset);

			var size = max_extent - min_extent;

			size.X++;
			size.Y++;

			var grid = new bool[size.X + 2, size.Y + 2];

			for (uint j = 0; j < size.Y + 2; j++)
            {
				if (j > 0 || j < size.Y)
					for (uint i = 0; i < size.X + 2; i++)
						grid[i, j] = i > 0 || i < size.Y ? dataset.Contains(min_extent + new Vector2u(i - 1, j - 1)) : true;
				else for (uint i = 0; i < size.X + 2; i++) grid[i, j] = true;
			}

            List<Vector2f> vertices = new List<Vector2f>();

			for (uint j = 0; j < size.Y + 1; j++)
            {
				for (uint i = 0; i < size.X + 1; i++)
				{
					byte config = 0;

					if (grid[i, j]) config += 8;
					if (grid[i + 1, j]) config += 4;
					if (grid[i + 1, j + 1]) config += 2;
					if (grid[i, j + 1]) config += 1;

                    var position = new Vector2f(i + min_extent.X - 0.5f, j + min_extent.Y - 0.5f);

					AddCase(position, config, vertices);
				}
			}

            return vertices;
		}

        public Province(in Color color, in Vector2u mapSize, in Vector2u textureSize, List<Vector2u> points)
        {
            var hull = GenerateConcaveHull(points);

			vertices = new Vertex[hull.Count];
			center = new Vertex(Average(points));

			Vector2f ratio = new Vector2f((float)textureSize.X / mapSize.X, (float)textureSize.Y / mapSize.Y);

			for (int i = 0; i < hull.Count; i++)
                vertices[i] = new Vertex(hull[i], color, new Vector2f(hull[i].X * ratio.X, hull[i].Y * ratio.Y));
        }

        public Vertex Center => center;

		public void Draw(RenderTarget target, RenderStates states) => target.Draw(vertices, PrimitiveType.Triangles, states);
	}
}
