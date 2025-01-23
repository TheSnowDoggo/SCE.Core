using System.Text;

namespace SCE
{
    public class Command : ISearcheable
    {
        public Command(string name, Action<string[]>? action = null, string[]? argumentPreview = null)
        {
            Name = name;
            Action = action;
            ArgumentPreview = argumentPreview;
        }

        public string Name { get; set; }

        public Action<string[]>? Action { get; set; }

        public string[]? ArgumentPreview { get; set; }

        public int MinArgs { get; set; } = 0;

        public int MaxArgs { get; set; } = -1;

        public void Run(string[] args)
        {
            Action?.Invoke(args);
        }

        public string BuildPreview(int argsLen)
        {
            if (ArgumentPreview is null)
                return string.Empty;

            int length = argsLen != -1 ? Math.Min(ArgumentPreview.Length, argsLen) : ArgumentPreview.Length;
            StringBuilder strBuilder = new();
            for (int i = 0; i < length; ++i)
                strBuilder.Append(' ' + ArgumentPreview[i]);

            return strBuilder.ToString();
        }

        public override string ToString()
        {
            return Name + BuildPreview(-1);
        }
    }
}
