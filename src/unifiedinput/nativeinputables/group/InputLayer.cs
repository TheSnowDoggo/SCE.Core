namespace SCE
{
    /// <summary>
    /// A class for storing a list of <see cref="IInputReceiver"/>.
    /// </summary>
    public class InputLayer : InputGroup, IComparable<InputLayer>
    {
        public InputLayer(int layer)
            : base()
        {
            Layer = layer;
        }

        public InputLayer(int layer, IEnumerable<IInputReceiver> collection)
            : base(collection)
        {
            Layer = layer;
        }

        public int Layer { get; set; }

        public int CompareTo(InputLayer? other)
        {
            if (other is null)
                throw new NotImplementedException();
            return other.Layer - Layer; // Top down sorting
        }
    }
}
