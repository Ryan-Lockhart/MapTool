using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using System.Collections;

namespace MapTool
{
    public class Map : Drawable
    {
        private readonly Texture texture;
        private readonly Image image;

        private readonly MapType type;

        private readonly int checksum;

        private Sprite sprite;

        public Map(string path, MapType type)
        {
            texture = new Texture(path);
            image = texture.CopyToImage();

            this.type = type;
            checksum = CalculateChecksum();

            sprite = new Sprite(texture);
        }

        public MapType MapType => type;
        public int Checksum => checksum;

        public Vector2u Size => image.Size;

        private int CalculateChecksum()
        {
            int hash = 0;

            for (uint j = 0; j < image.Size.Y; j++)
				for (uint i = 0; i < image.Size.X; i++)
					hash = HashCode.Combine(hash, image.GetPixel(i, j).GetHashCode());

			return hash;
		}

        public Color SamplePosition(uint x, uint y)
        {
            uint clampedX = uint.Clamp(x, 0, image.Size.X - 1);
            uint clampedY = uint.Clamp(y, 0, image.Size.Y - 1);

            return image.GetPixel(clampedX, clampedY);
        }

        public Color SamplePosition(int x, int y)
        {
            uint clampedX = x < 0 ? 0u : uint.Clamp((uint)x, 0, image.Size.X - 1);
            uint clampedY = x < 0 ? 0u : uint.Clamp((uint)y, 0, image.Size.Y - 1);

            return image.GetPixel(clampedX, clampedY);
        }

        public Color SamplePosition(in Vector2u position)
        {
            uint clampedX = uint.Clamp(position.X, 0, image.Size.X - 1);
            uint clampedY = uint.Clamp(position.Y, 0, image.Size.Y - 1);

            return image.GetPixel(clampedX, clampedY);
        }

        public Color SamplePosition(in Vector2i position)
        {
            uint clampedX = position.X < 0 ? 0u : uint.Clamp((uint)position.X, 0, image.Size.X - 1);
            uint clampedY = position.Y < 0 ? 0u : uint.Clamp((uint)position.Y, 0, image.Size.Y - 1);

            return image.GetPixel(clampedX, clampedY);
        }

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

            for (uint y = origin.Y; y < destination.Y; y++)
                for (uint x = origin.X; x < destination.X; x++)
                    sampleRegion[x, y] = image.GetPixel(x, y);
        }

        public void SampleRegion(uint originX, uint originY, in Vector2u size, out Color[,] sampleRegion)
        {
            if (size.X == 0 || size.Y == 0) throw new ArgumentException("Width or height of region to sample cannot be zero!");

            sampleRegion = new Color[size.X, size.Y];

            Vector2u destination = new Vector2u(originX, originY) + size;

            for (uint y = originY; y < destination.Y; y++)
                for (uint x = originX; x < destination.X; x++)
                    sampleRegion[x, y] = image.GetPixel(x, y);
        }

        public void SampleRegion(in Vector2u origin, in Vector2u size, out Color[,] sampleRegion)
        {
            if (size.X == 0 || size.Y == 0) throw new ArgumentException("Width or height of region to sample cannot be zero!");

            sampleRegion = new Color[size.X, size.Y];

            Vector2u destination = origin + size;

            for (uint y = origin.Y; y < destination.Y; y++)
                for (uint x = origin.X; x < destination.X; x++)
                    sampleRegion[x, y] = image.GetPixel(x, y);
        }

		public void Draw(RenderTarget target, RenderStates states) => target.Draw(sprite, states);
	}
}
