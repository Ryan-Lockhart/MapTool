using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

using static MapTool.SFMLExtensions.Vector2fExtensions;

namespace MapTool
{
    public class Camera
    {
        private RenderWindow window;

        private View view;

        private ScaledLevel zoom;
        private ScaledLevel speed;

        private Vector2f maxSize;

        public Camera(RenderWindow window, in Vector2f maxSize, int startingZoom = 24, int zoomSteps = 32, int zoomMidpoint = 24, int startingSpeed = 24, int speedSteps = 32, int speedMidpoint = 16, float baseSpeed = 1000.0f, in Vector2f? origin = null)
        {
            this.window = window;
            this.maxSize = maxSize;

            zoom = new ScaledLevel(startingZoom, zoomSteps, zoomMidpoint);
            speed = new ScaledLevel(startingSpeed, speedSteps, speedMidpoint);

            speed.Multiplier = baseSpeed;

            zoom.LevelChanged += OnZoomLevelChanged;

            view = new View();
            view.Size = Min((Vector2f)window.Size, maxSize);

            CalculateView();

            SetPosition(origin ?? MinExtent);
        }

        public Camera(RenderWindow window, in Vector2f maxSize, in ScaledLevelArgs zoomArgs, in ScaledLevelArgs speedArgs, float baseSpeed = 1000.0f, in Vector2f? origin = null)
        {
            this.window = window;
            this.maxSize = maxSize;

            zoom = new ScaledLevel(zoomArgs);
            speed = new ScaledLevel(speedArgs);

            speed.Multiplier = baseSpeed;

            zoom.LevelChanged += OnZoomLevelChanged;

            view = new View();
            view.Size = Min((Vector2f)window.Size, maxSize);

            CalculateView();

            SetPosition(origin ?? MinExtent);
        }

        public RenderWindow Window => window;

        public ScaledLevel Zoom => zoom;
        public ScaledLevel Speed => speed;

        public View View => view;

        public Vector2f MaxSize
        {
            get => maxSize;
            set => maxSize = value;
        }

        public Vector2f MinExtent => view.Size / 2;
        public Vector2f MaxExtent => maxSize - view.Size / 2;

        public Vector2f Center
        {
            get => view.Center;
            set => view.Center = Clamp(value, MinExtent, MaxExtent);
        }

        public void Activate() => window.SetView(view);
        public void Deactivate() => window.SetView(window.DefaultView);

        public void Translate(in Vector2f offset, bool raw = false) => Center += offset * (raw ? 1.0f : Speed.Scale) * Zoom.Scale;
        public void Translate(in Vector2f offset, float deltaTime, bool raw = false) => Center += offset * (raw ? 1.0f : Speed.Scale) * Zoom.Scale * deltaTime;

        public void MoveTo(in Vector2f position, bool raw = false) => Center += (position - Center).Normalized() * (raw ? 1.0f : Speed.Scale) * Zoom.Scale;
        public void MoveTo(in Vector2f position, float deltaTime, bool raw = false) => Center += (position - Center).Normalized() * (raw ? 1.0f : Speed.Scale) * Zoom.Scale * deltaTime;

        public void SetPosition(in Vector2f position) => Center = position;

        private void OnZoomLevelChanged(object? sender, ValueChangedEventArgs<int> e)
        {
            CalculateView();
        }

        private void CalculateView()
        {
            view.Size = Min((Vector2f)window.Size * zoom.Scale, maxSize);
            Center = Center;
        }
    }
}
