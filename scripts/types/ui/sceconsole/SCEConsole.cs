namespace SCE
{
    using System.Text;

    /// <summary>
    /// A <see cref="TextBoxUI"/> wrapper class representing a console used for in-engine logging.
    /// </summary>
    public class SCEConsole : IRenderable
    {
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
        /// <param name="isActive">The initial active state of the console.</param>
        public SCEConsole(Vector2Int dimensions, Color bgColor = DefaultBgColor)
        {
            ui = new(dimensions)
            {
                Text = DefaultText,
                BgColor = bgColor,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SCEConsole"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the console.</param>
        /// <param name="isActive">The initial active state of the console.</param>
        public SCEConsole(Vector2Int dimensions)
        {
            ui = new(dimensions)
            {
                Text = DefaultText,
            };
        }

        /// <summary>
        /// Gets the number of logs in this instance.
        /// </summary>
        public int Logs { get => logList.Count; }

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

        public string Name { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public Vector2Int Position { get; set; }

        public int Layer { get; set; }

        public Anchor Anchor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        public Color FgColor
        {
            get => ui.Text.FgColor;
            set => ui.Text.FgColor = value;
        }

        private static Text DefaultText 
        {
            get => new()
            {
                FgColor = DefaultFgColor,
                BgColor = Color.Transparent,
                Anchor = Anchor.TopLeft,
            };
        }

        private int MaxLines { get => ui.Height - 1; }

        private string SmartHeader { get => $"- {VersionName} - Logs: {Logs}"; }

        private string AdjustedHeader { get => StringUtils.PostFitToLength(SmartHeader, ui.Width * Pixel.PIXELWIDTH); }

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
        public DisplayMap GetMap()
        {
            ui.Text.Data = BuildLogList();
            return ui.GetMap();
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
                    string[] lineArray = StringUtils.BasicSplitLineArray(log.FullMessage, MaxLines - lines); 

                    foreach (string line in lineArray)
                    {
                        strBuilder.Append(StringUtils.PostFitToLength(line, ui.Width * Pixel.PIXELWIDTH));
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
