using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

namespace MapTool
{
    internal class Program
    {
#pragma warning disable CS8618

        private static readonly Vector2u windowSize = new Vector2u(2160, 1440);
        private static readonly Vector2u windowMidpoint = new Vector2u(windowSize.X / 2, windowSize.Y / 2);

        private static readonly Vector2u mapSize = new Vector2u(5632, 2048);
        private static readonly Vector2u mapMidpoint = new Vector2u(mapSize.X / 2, mapSize.Y / 2);

        private static RenderWindow window;
        private static Map provinces;

        private static Camera camera;

        private static Texture cursorTexture;
        private static Sprite cursor;

        private static Vector2f lastCursorPosition;
        private static Color lastCursorColor;

        private static FloodSelector floodSelector;

        private static Province? selection;
        private static bool canReselect;

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
            floodSelector = new FloodSelector(provinces);

            cursorTexture = new Texture("Assets\\cursor.png");
            cursor = new Sprite(cursorTexture);

            camera = new Camera(window, (Vector2f)mapSize);
        }

        private static void Window_MouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            if (e.Wheel == Mouse.Wheel.VerticalWheel)
            {
                if (e.Delta > 0.0f) camera.Zoom.Level--;
                else if (e.Delta < 0.0f) camera.Zoom.Level++;
            }
        }

        private static void Input(float deltaTime)
        {
            window.DispatchEvents();

            Vector2f cursorPosition = window.MapPixelToCoords(Mouse.GetPosition(window));
            Vector2i mousePosition = (Vector2i)window.MapPixelToCoords(Mouse.GetPosition(window), camera.View);

            lastCursorPosition = cursor.Position;
            lastCursorColor = cursor.Color;

            cursor.Position = cursorPosition;
            cursor.Color = provinces.SamplePosition(mousePosition);

            if (lastCursorColor != cursor.Color) canReselect = true;

            if (canReselect && Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                try { selection = new Province(floodSelector.SelectEdge((Vector2u)mousePosition)); }
                catch (InvalidSelectionException err) { Console.WriteLine(err.Message); }
                finally { canReselect = false; }
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Middle) && lastCursorPosition != cursor.Position)
                camera.Translate(-(cursorPosition - lastCursorPosition), true);

            Vector2f cameraInput = new Vector2f(0.0f, 0.0f);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Keyboard.IsKeyPressed(Keyboard.Key.W)) cameraInput.Y--;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Keyboard.IsKeyPressed(Keyboard.Key.S)) cameraInput.Y++;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D)) cameraInput.X++;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A)) cameraInput.X--;

            if (cameraInput != new Vector2f(0.0f, 0.0f)) camera.Translate(cameraInput, deltaTime);
        }

        private static void Update(float deltaTime)
        {

        }

        private static void Render()
        {
            window.Clear();

            camera.Activate();

            window.Draw(provinces);

            if (selection != null) window.Draw(selection);

            camera.Deactivate();

            window.Draw(cursor);

            window.Display();
        }
    }
}
