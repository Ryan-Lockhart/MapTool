namespace MapTool
{
    public struct ScaledLevelArgs(int startingLevel, int steps, int midpoint)
    {
        public readonly int startingLevel = startingLevel;

        public readonly int steps = steps;
        public readonly int midpoint = midpoint;
    }

    public class ScaledLevel
    {
        private int level;

        private int steps;
        private int midpoint;

        private float multiplier;

        public ScaledLevel(int startingLevel, int steps, int midpoint)
        {
            if (midpoint > steps) throw new ArgumentException("Midpoint cannot be greater than the number of steps!");

            this.steps = steps;
            this.midpoint = midpoint;

            multiplier = 1;

            level = int.Clamp(startingLevel, 1, Steps);
        }

        public ScaledLevel(in ScaledLevelArgs args)
        {
            if (midpoint > steps) throw new ArgumentException("Midpoint cannot be greater than the number of steps!");

            steps = args.steps;
            midpoint = args.midpoint;

            multiplier = 1;

            level = int.Clamp(args.startingLevel, 1, Steps);
        }

        public int Level
        {
            get => level;
            set
            {
                value = int.Clamp(value, 1, Steps);

                if (level == value) return;

                (var oldLevel, level) = (level, value);

                LevelChanged?.Invoke(this, new ValueChangedEventArgs<int>(level, value));
            }
        }

        public event EventHandler<ValueChangedEventArgs<int>> LevelChanged = delegate { };

        public int Steps
        {
            get => steps;
            set => steps = value;
        }
        public int Midpoint
        {
            get => midpoint;
            set => midpoint = value;
        }

        public float Scale => ((float)level / Midpoint) * multiplier;

        public float Multiplier { get => multiplier; set => multiplier = value; }
    }
}
