namespace SCE
{
    using System.Text;

    /// <summary>
    /// A <see cref="TextBoxUI"/> wrapper class representing a console used for in-engine logging.
    /// </summary>
    public class SCEConsole : UIBase
    {
        private const string DEFAULT_NAME = "sce_console";
        private const string VERSION_NAME = "SCEConsole V1.1";

        private const Color DEFAULT_BGCOLOR = Color.Black;
        private const Color DEFAULT_FGCOLOR = Color.White;

        private readonly List<SCELog> logList = new();

        private readonly TextBoxUI ui;

        private int logIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SCEConsole"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the console.</param>
        /// <param name="bgColor">The background color of the console.</param>
        /// <param name="isActive">The initial active state of the console.</param>
        public SCEConsole(string name, Vector2Int dimensions, Color? bgColor = null)
            : base(name)
        {
            ui = new(dimensions) { Text = DefaultText };
            if (bgColor is Color color)
                ui.BgColor = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SCEConsole"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the console.</param>
        /// <param name="isActive">The initial active state of the console.</param>
        public SCEConsole(Vector2Int dimensions, Color? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
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
                FgColor = DEFAULT_FGCOLOR,
                BgColor = Color.Transparent,
                Anchor = Anchor.TopLeft,
            };
        }

        private int MaxLines { get => ui.Height - 1; }

        private string SmartHeader { get => $"- {VERSION_NAME} - Logs: {Logs}"; }

        private string AdjustedHeader { get => StringUtils.PostFitToLength(SmartHeader, ui.Width * Pixel.PIXELWIDTH); }

        /// <summary>
        /// Gets or sets the <see cref="SCELog"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="SCELog"/>.</param>
        /// <returns>The <see cref="SCELog"/> at the specified <paramref name="index"/>.</returns>
        public SCELog this[int index]
        {
            get => logList[index];
            set => logList[index] = value;
        }

        /// <inheritdoc/>
        public override DisplayMap GetMap()
        {
            ui.Text.Data = BuildLogList();
            return ui.GetMap();
        }

        /// <summary>
        /// Removes all logs from this instance.
        /// </summary>
        public void Clear() => logList.Clear();

        /// <inheritdoc cref="List{T}.Add(T)"/>
        public void Add(SCELog log)
        {
            logList.Add(log);
        }

        /// <summary>
        /// Creates and adds a new <see cref="SCELog"/> with a specified message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void AddNew(string message)
        {
            logList.Add(new SCELog(message));
        }

        /// <inheritdoc cref="List{T}.Remove(T)"/>
        public bool Remove(SCELog log)
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

                    SCELog log = this[i];
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
