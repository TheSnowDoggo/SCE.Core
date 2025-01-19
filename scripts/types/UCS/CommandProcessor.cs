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

            matches = FindMatches(input, CaseSensitive);
        }

        public void ClearInput()
        {
            ReceiveInput("");
        }

        public void TryRunCommand()
        {
            if (matches.Length > 0)
                matches[0].Run(args);
        }

        public string[] GetDisplay()
        {
            string[] matchesStr = BuildMatches(args.Length, PreviewMode);
            string[] displayStr = new string[matchesStr.Length + 1];

            displayStr[0] = search;

            for (int i = 0; i < matchesStr.Length; ++i)
                displayStr[i + 1] = matchesStr[i];

            return displayStr;
        }

        private Command[] FindMatches(string entry, bool caseSensitive)
        {
            if (!caseSensitive)
                entry = entry.ToLower();

            List<Command> matchList = new(CommandList.Count);

            foreach (var command in CommandList)
            {
                string commandName = caseSensitive ? command.Name : command.Name.ToLower();

                if (StringUtils.DoesMatchTo(commandName, entry, CutEmpty))
                    matchList.Add(command);
            }

            return matchList.ToArray();
        }

        private string[] BuildMatches(int argsLen, ArgumentPreviewMode previewMode)
        {
            string[] build = new string[matches.Length];
            for (int i = 0; i < build.Length; ++i)
            {
                Command command = matches[i];

                int previewLength = GetPreviewLength(argsLen, previewMode);

                build[i] = command.Name + command.BuildPreview(previewLength);
            }
            return build;
        }

        private int GetPreviewLength(int argsLen, ArgumentPreviewMode previewMode)
        {
            return previewMode switch
            {
                ArgumentPreviewMode.ShowNext => argsLen,
                ArgumentPreviewMode.ShowAlways => -1,
                ArgumentPreviewMode.ShowOnCommand => matches.Length == 1 ? -1 : 0,
                _ => throw new NotImplementedException()
            };
        }
    }
}
