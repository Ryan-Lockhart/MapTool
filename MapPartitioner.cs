﻿using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapTool
{
	public static class MapPartitioner
	{
		public static Dictionary<Color, Province> Partition(Map map, Vector2u textureSize)
		{
			Dictionary<Color, HashSet<Vector2u>> provinceFills = new Dictionary<Color, HashSet<Vector2u>>();

			map.SampleRegion(new Vector2u(0, 0), map.Size, out Color[,] colors);

			foreach (var color in colors)
				if (!provinceFills.ContainsKey(color))
					provinceFills.Add(color, new HashSet<Vector2u>());

			for (uint j = 0; j < map.Size.Y; j++)
				for (uint i = 0; i < map.Size.X; i++)
					provinceFills[colors[i, j]].Add(new Vector2u(i, j));

			var provinces = new Dictionary<Color, Province>();

			foreach (var fill in provinceFills)
                provinces.Add(fill.Key, new Province(fill.Key, fill.Value));

            /*Parallel.ForEach(provinceFills, fill =>
			{
				provinces.Add(fill.Key, new Province(fill.Key, fill.Value));
			});*/

            return provinces;
		}
	}
}
