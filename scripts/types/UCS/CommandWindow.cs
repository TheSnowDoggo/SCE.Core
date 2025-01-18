namespace SCE
{
    public class CommandWindow : LineRenderer
    {
        public CommandWindow(CommandProcessor commandProcessor, Vector2Int dimensions)
            : base(dimensions)
        {
            CommandProcessor = commandProcessor;
        }

        public CommandProcessor CommandProcessor { get; set; }

        protected override void Render()
        {
            string[] displayArr = CommandProcessor.GetDisplay();

            for (int i = 0; i < displayArr.Length; ++i)
                SetLine(i, new(displayArr[i])); 
        }
    }
}
