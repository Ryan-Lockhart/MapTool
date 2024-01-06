namespace MapTool
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        private readonly T oldValue;
        private readonly T newValue;

        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public T OldValue => oldValue;
        public T NewValue => newValue;
    }
}
