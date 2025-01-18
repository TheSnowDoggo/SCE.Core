namespace SCE
{
    using System.Collections.ObjectModel;


    public class CommandList : SearchList<Command>
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
