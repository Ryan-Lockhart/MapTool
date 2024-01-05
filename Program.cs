using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using System.ComponentModel.DataAnnotations;

namespace MapTool
{
    internal class Program
    {
#pragma warning disable CS8618

        private static readonly Vector2u windowSize = new Vector2u(1280, 720);
        private static readonly Vector2u windowMidpoint = new Vector2u(windowSize.X / 2, windowSize.Y / 2);

        private static readonly Vector2u mapSize = new Vector2u(5632, 2048);
        private static readonly Vector2u mapMidpoint = new Vector2u(mapSize.X / 2, mapSize.Y / 2);

        private static RenderWindow window;
        private static Map provinces;

        private static Texture cursorTexture;
        private static Sprite cursor;

        private static View camera;

        private static readonly Vector2f cameraOrigin = (Vector2f)(mapMidpoint - windowSize);

        private static readonly Vector2f cameraMinExent = new Vector2f(0, 0);
        private static readonly Vector2f cameraMaxExtent = (Vector2f)(mapSize - windowSize);

        private static Vector2f cameraPosition = cameraOrigin;

        private static float cameraSpeed = 1000.0f;

        private static int zoomMin = -2;
        private static int zoomLevel = 0;
        private static int zoomMax = 2;

        private static float GetZoomSpeed()
        {
            return zoomLevel switch
            {
                -2 => 0.25f,
                -1 => 0.50f,
                0 => 1.00f,
                1 => 2.00f,
                2 => 4.00f,
                _ => 1.0f,
            };
        }

        private static Vector2f lastCursorPosition;

        private static Vector2f CameraPosition
        {
            get => cameraPosition;
            set
            {
                cameraPosition.Clamp(value, cameraMinExent, cameraMaxExtent);

                camera.Center = cameraPosition - (Vector2f)windowMidpoint;
            }
        }

#pragma warning restore

        private static void Main(string[] args)
        {
            Load();

            Clock deltaClock = new Clock();

            while (window.IsOpen)
            {
                float deltaTime = deltaClock.Restart().AsMilliseconds() / 1000.0f;

                if (window.HasFocus())
                {
                    Input(deltaTime);
                    Update(deltaTime);
                    Render();
                }
            }
        }

        private static void Load()
        {
            window = window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), "Map Tool", Styles.None);

            window.SetVerticalSyncEnabled(true);
            window.SetFramerateLimit(60);

            window.SetMouseCursorVisible(false);

            window.MouseWheelScrolled += Window_MouseWheelScrolled;

            provinces = new Map("Assets\\Maps\\provinces_map.png", MapType.Provinces);

            cursorTexture = new Texture("Assets\\cursor.png");
            cursor = new Sprite(cursorTexture);

            camera = new View(new FloatRect(cameraOrigin, (Vector2f)windowSize));

            window.SetView(camera);
        }

        private static void Window_MouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            if (e.Delta > 0.9f && zoomLevel < zoomMax) camera.Zoom(0.9f);
            else if (e.Delta < -0.9f && zoomLevel > zoomMin) camera.Zoom(1.1f);
        }

        private static void Input(float deltaTime)
        {
            window.DispatchEvents();

            Vector2f cameraInput = new Vector2f(0.0f, 0.0f);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Keyboard.IsKeyPressed(Keyboard.Key.W)) cameraInput.Y--;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Keyboard.IsKeyPressed(Keyboard.Key.S)) cameraInput.Y++;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D)) cameraInput.X++;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A)) cameraInput.X--;

            if (cameraInput != new Vector2f(0.0f, 0.0f)) camera.Move(cameraInput * cameraSpeed * deltaTime * GetZoomSpeed());

            Vector2f cursorPosition = window.MapPixelToCoords(Mouse.GetPosition(window));
            Vector2i mousePosition = (Vector2i)window.MapPixelToCoords(Mouse.GetPosition(window), camera);

            if (Mouse.IsButtonPressed(Mouse.Button.Middle) && lastCursorPosition != cursor.Position)
                camera.Move(-(cursorPosition - lastCursorPosition) * GetZoomSpeed());

            lastCursorPosition = cursor.Position;

            cursor.Position = cursorPosition;
            cursor.Color = provinces.SamplePosition(mousePosition);
        }

        private static void Update(float deltaTime)
        {

        }

        private static void Render()
        {
            window.Clear();

            window.SetView(camera);

            window.Draw(provinces);

            window.SetView(window.DefaultView);

            window.Draw(cursor);

            window.Display();
        }
    }
}
