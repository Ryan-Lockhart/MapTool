﻿using SFML.Graphics;
using SFML.System;

namespace MapTool
{
    public enum MapType
    {
        Areas,
        Climates,
        Continents,
        Cultures,
        CultureGroups,
        Provinces,
        Regions,
        Religions,
        Rivers,
        Superregions,
        Terrain,
        TradeGoods
    }

    public class Map : Drawable
    {
        private readonly MapType type;

        private readonly Texture texture;
        private readonly Image image;

        private Sprite sprite;

        public Map(string path, MapType type)
        {
            this.type = type;
            texture = new Texture(path);
            image = texture.CopyToImage();

            sprite = new Sprite(texture);
        }

        public MapType MapType => type;

        public void Draw(RenderTarget target, RenderStates states) => target.Draw(sprite, states);

        public void Translate(int x, int y) => sprite.Position += new Vector2f(x, y);
        public void Translate(in Vector2i offset) => sprite.Position += (Vector2f)offset;

        public Color SamplePosition(uint x, uint y) => image.GetPixel(uint.Clamp(x, 0, texture.Size.X), uint.Clamp(y, 0, texture.Size.Y));
        public Color SamplePosition(int x, int y) => image.GetPixel(x < 0 ? 0u : uint.Clamp((uint)x, 0, texture.Size.X), y < 0 ? 0u : uint.Clamp((uint)y, 0, texture.Size.Y));
        public Color SamplePosition(in Vector2u position) => image.GetPixel(uint.Clamp(position.X, 0, texture.Size.X), uint.Clamp(position.Y, 0, texture.Size.Y));
        public Color SamplePosition(in Vector2i position) => image.GetPixel(position.X < 0 ? 0u : uint.Clamp((uint)position.X, 0, texture.Size.X), position.Y < 0 ? 0u : uint.Clamp((uint)position.Y, 0, texture.Size.Y));

        public void SampleRegion(uint originX, uint originY, uint width, uint height, out Color[,] sampleRegion)
        {
            if (width == 0 || height == 0) throw new ArgumentException("Width or height of region to sample cannot be zero!");

            sampleRegion = new Color[width, height];

            uint destinationX = originX + width;
            uint destinationY = originY + height;

            for (uint y = originY; y < destinationY; y++)
                for (uint x = originX; x < destinationX; x++)
                    sampleRegion[x, y] = image.GetPixel(x, y);
        }

        public void SampleRegion(in Vector2u origin, uint width, uint height, out Color[,] sampleRegion)
        {
            if (width == 0 || height == 0) throw new ArgumentException("Width or height of region to sample cannot be zero!");

            sampleRegion = new Color[width, height];

            Vector2u destination = origin + new Vector2u(width, height);

            for (uint y = destination.Y; y < destination.Y; y++)
                for (uint x = destination.X; x < destination.X; x++)
                    sampleRegion[x, y] = image.GetPixel(x, y);
        }

        public void SampleRegion(uint originX, uint originY, in Vector2u size, out Color[,] sampleRegion)
        {
            if (size.X == 0 || size.Y == 0) throw new ArgumentException("Width or height of region to sample cannot be zero!");

            sampleRegion = new Color[size.X, size.Y];

            Vector2u destination = new Vector2u(originX, originY) + size;

            for (uint y = destination.Y; y < destination.Y; y++)
                for (uint x = destination.X; x < destination.X; x++)
                    sampleRegion[x, y] = image.GetPixel(x, y);
        }

        public void SampleRegion(in Vector2u origin, in Vector2u size, out Color[,] sampleRegion)
        {
            if (size.X == 0 || size.Y == 0) throw new ArgumentException("Width or height of region to sample cannot be zero!");

            sampleRegion = new Color[size.X, size.Y];

            Vector2u destination = origin + size;

            for (uint y = destination.Y; y < destination.Y; y++)
                for (uint x = destination.X; x < destination.X; x++)
                    sampleRegion[x, y] = image.GetPixel(x, y);
        }
    }
}