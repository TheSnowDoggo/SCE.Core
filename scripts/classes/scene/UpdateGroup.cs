namespace SCE
{
    public class UpdateGroup : SearchHashTypeExt<IUpdate>
    {
        public UpdateGroup(IEnumerable<IUpdate> collection)
            : base(collection)
        {
        }

        public UpdateGroup()
            : base()
        {
        }

        public void Update()
        {
            foreach (var update in this)
            {
                if (update.IsActive)
                    update.Update();
            }
        }
    }
}
