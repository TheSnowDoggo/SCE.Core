namespace SCE
{
    /// <summary>
    /// A struct representing a log containing a message and the log time.
    /// </summary>
    public readonly struct Log : IEquatable<Log>
    {
        private readonly string message;

        private readonly DateTime logTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> struct.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="logTime">The log time.</param>
        public Log(string message, DateTime logTime)
        {
            this.message = message;
            this.logTime = logTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> struct with a log time of the current time.
        /// </summary>
        /// <param name="message">The log message.</param>
        public Log(string message)
            : this(message, DateTime.Now)
        {
        }

        /// <summary>
        /// Gets the log message.
        /// </summary>
        public string Message { get => message; }

        /// <summary>
        /// Gets the log time.
        /// </summary>
        public DateTime LogTime { get => logTime; }

        /// <summary>
        /// Gets the formatted full log message.
        /// </summary>
        public string FullMessage { get => $">{LongTime}: {Message}"; }

        /// <summary>
        /// Gets the formatted string log time.
        /// </summary>
        public string LongTime { get => LogTime.ToString("T"); }

        /// <summary>
        /// Gets the number of new line characters in the log message.
        /// </summary>
        public int Lines { get => StringUtils.CountOf(Message, '\n'); }

        // Equals operators
        public static bool operator ==(Log left, Log right) => left.Equals(right);

        public static bool operator !=(Log left, Log right) => !(left == right);

        /// <inheritdoc/>
        public bool Equals(Log log)
        {
            return log.Message == Message && log.LogTime == LogTime;
        }

        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            return other is Log log && Equals(log);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return FullMessage;
        }

        /// <summary>
        /// Returns the string representation of this instance formatted to a given pixel width.
        /// </summary>
        /// <param name="pixelWidth">The pixel width to format to.</param>
        /// <returns>The string representation of this instance formatted to a given pixel width.</returns>
        public string ToString(int pixelWidth)
        {
            return StringUtils.FitToLength(ToString(), pixelWidth * Pixel.PIXELWIDTH);
        }
    }
}
