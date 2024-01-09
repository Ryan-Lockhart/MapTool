namespace MapTool
{
    internal class InvalidSelectionException : Exception
    {
        public InvalidSelectionException() { }

        public InvalidSelectionException(string? message) : base(message) { }

        public InvalidSelectionException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}