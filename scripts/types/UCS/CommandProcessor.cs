namespace SCE
{
    public class CommandProcessor
    {
        public enum ArgumentPreviewMode
        {
            ShowNext,
            ShowAlways,
            ShowOnCommand,
        }

        private Command[] matches = Array.Empty<Command>();

        private string search = string.Empty;

        private string[] args = Array.Empty<string>();

        public CommandProcessor(CommandList commandList)
        {
            CommandList = commandList;
        }

        public CommandList CommandList { get; set; }

        public ArgumentPreviewMode PreviewMode { get; set; } = ArgumentPreviewMode.ShowNext;

        public bool CaseSensitive { get; set; } = false;

        public bool CutEmpty { get; set; } = true;

        public void ReceiveInput(string input)
        {
            search = input;

            args = search.Split(' ');
            StringUtils.TrimFirst(ref args);

            matches = FindMatches(input);
        }

        public string[] GetDisplay()
        {
            string[] matchArr = BuildMatches(args.Length);
            string[] displayArr = new string[matchArr.Length + 1];

            for (int i = 0; i < matchArr.Length; ++i)
                displayArr[i + 1] = matchArr[i];

            return displayArr;
        }

        private Command[] FindMatches(string entry)
        {
            if (!CaseSensitive)
                entry = entry.ToLower();

            List<Command> matchList = new(CommandList.Count);

            foreach (var command in CommandList)
            {
                string commandName = CaseSensitive ? command.Name : command.Name.ToLower();

                if (StringUtils.DoesMatchTo(commandName, entry, CutEmpty))
                    matchList.Add(command);
            }

            return matchList.ToArray();
        }

        private string[] BuildMatches(int argsLen)
        {
            string[] build = new string[matches.Length];
            for (int i = 0; i < build.Length; ++i)
            {
                Command command = matches[i];

                int previewLength = PreviewMode switch
                {
                    ArgumentPreviewMode.ShowNext => argsLen,
                    ArgumentPreviewMode.ShowAlways => -1,
                    ArgumentPreviewMode.ShowOnCommand => matches.Length == 1 ? -1 : 0,
                    _ => throw new NotImplementedException()
                };

                build[i] = command.Name + command.BuildPreview(previewLength);
            }
            return build;
        }
    }
}
