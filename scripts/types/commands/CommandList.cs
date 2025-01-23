namespace SCE
{
    using System.Collections.ObjectModel;


    public class CommandList : SearchHash<Command>
    {
        public CommandList(Collection<Command> collection)
            : base(collection)
        {
        }
        
        public CommandList()
            : base()
        {
        }
    }
}
