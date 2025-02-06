namespace SCE
{
    /// <summary>
    /// A class for storing a list of <see cref="IInputReceiver"/>.
    /// </summary>
    public class InputLayer : InputGroup, IComparable<InputLayer>
    {
        #region Constructors
        public InputLayer(int layer, IEnumerable<IInputReceiver> collection)
            : base(collection)
        {
            Layer = layer;
        }

        public InputLayer(string name, int layer, IEnumerable<IInputReceiver> collection)
            : base(name, collection)
        {
            Layer = layer;
        }

        public InputLayer(string name, int layer)
            : base(name)
        {
            Layer = layer;
        }

        public InputLayer(int layer)
            : base()
        {
            Layer = layer;
        }
        #endregion

        public int Layer { get; set; }

        public int CompareTo(InputLayer? other)
        {
            if (other is null)
                throw new NotImplementedException();
            return other.Layer - Layer; // Top down sorting
        }
    }
}
