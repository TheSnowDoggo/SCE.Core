namespace SCECore.Types
{
    using System.Text;

    /// <summary>
    /// A <see cref="TextBoxUI"/> wrapper class representing a console used for in-engine logging.
    /// </summary>
    public class SCEConsole : IRenderable
    {
        private const bool DefaultActiveState = true;

        private const string VersionName = "SCEConsole V1.1";

        private const Color DefaultBgColor = Color.Black;
        private const Color DefaultFgColor = Color.White;

        private readonly List<Log> logList = new();

        private readonly TextBoxUI ui;

        private int logIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SCEConsole"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the console.</param>
        /// <param name="bgColor">The background color of the console.</param>
        /// <param name="fgColor">The foreground color of the console.</param>
        /// <param name="isActive">The initial active state of the console.</param>
        public SCEConsole(Vector2Int dimensions, Color bgColor, Color fgColor, bool isActive = DefaultActiveState)
        {
            ui = new(dimensions, bgColor, DefaultText, isActive);
            FgColor = fgColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SCEConsole"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the console.</param>
        /// <param name="bgColor">The background color of the console.</param>
        /// <param name="isActive">The initial active state of the console.</param>
        public SCEConsole(Vector2Int dimensions, Color bgColor = DefaultBgColor, bool isActive = DefaultActiveState)
        {
            ui = new(dimensions, bgColor, DefaultText, isActive);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SCEConsole"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the console.</param>
        /// <param name="isActive">The initial active state of the console.</param>
        public SCEConsole(Vector2Int dimensions, bool isActive = DefaultActiveState)
        {
            ui = new(dimensions, DefaultText, isActive);
        }

        /// <summary>
        /// Gets the number of logs in this instance.
        /// </summary>
        public int Logs => logList.Count;

        /// <summary>
        /// Gets or sets the index of the selected log.
        /// </summary>
        public int LogIndex
        {
            get => logIndex;
            set
            {
                if (value >= 0)
                {
                    logIndex = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the console is active.
        /// </summary>
        public bool IsActive
        {
            get => ui.IsActive;
            set => ui.IsActive = value;
        }

        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        public Color FgColor
        {
            get => ui.Text.FgColor;
            set => ui.Text.FgColor = value;
        }

        /// <summary>
        /// Gets or sets the position of the console.
        /// </summary>
        public Vector2Int Position
        {
            get => ui.Position;
            set => ui.Position = value;
        }

        /// <summary>
        /// Gets or sets the layer of the console.
        /// </summary>
        public byte Layer
        {
            get => ui.Layer;
            set => ui.Layer = value;
        }

        private static Text DefaultText => new(DefaultFgColor, Color.Transparent, Text.AlignLock.TopLeft);

        private int MaxLines => ui.Height - 1;

        private string SmartHeader => $"- {VersionName} - Logs: {Logs}";

        private string AdjustedHeader => SCEString.FitToLength(SmartHeader, ui.Width * Pixel.PIXELWIDTH);

        /// <summary>
        /// Gets or sets the <see cref="Log"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="Log"/>.</param>
        /// <returns>The <see cref="Log"/> at the specified <paramref name="index"/>.</returns>
        public Log this[int index]
        {
            get => logList[index];
            set => logList[index] = value;
        }

        /// <inheritdoc/>
        public Image GetImage()
        {
            ui.Text.Data = BuildLogList();
            return ui.GetImage();
        }

        /// <summary>
        /// Removes all logs from this instance.
        /// </summary>
        public void Clear() => logList.Clear();

        /// <inheritdoc cref="List{T}.Add(T)"/>
        public void Add(Log log)
        {
            logList.Add(log);
        }

        /// <summary>
        /// Creates and adds a new <see cref="Log"/> with a specified message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void AddNew(string message)
        {
            logList.Add(new Log(message));
        }

        /// <inheritdoc cref="List{T}.Remove(T)"/>
        public bool Remove(Log log)
        {
            return logList.Remove(log);
        }

        /// <inheritdoc cref="List{T}.RemoveAt(int)"/>
        public void RemoveAt(int index)
        {
            logList.RemoveAt(index);
        }

        private string BuildLogList()
        {
            StringBuilder strBuilder = new(AdjustedHeader);

            int lines = 0, i = logIndex;

            if (i < logList.Count)
            {
                do
                {
                    strBuilder.Append('\n');

                    Log log = this[i];
                    string[] lineArray = SCEString.BasicSplitLineArray(log.FullMessage, MaxLines - lines); 

                    foreach (string line in lineArray)
                    {
                        strBuilder.Append(SCEString.FitToLength(line, ui.Width * Pixel.PIXELWIDTH));
                    }

                    lines += lineArray.Length;
                    i++;
                }
                while (lines != MaxLines && i < Logs);
            }

            return strBuilder.ToString();
        }
    }
}
