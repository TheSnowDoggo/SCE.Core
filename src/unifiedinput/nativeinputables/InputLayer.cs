namespace SCE
{
    /// <summary>
    /// A class for storing a list of <see cref="IInputReceiver"/>.
    /// </summary>
    public class InputLayer : InputGroup, IComparable<InputLayer>
    {
        public InputLayer(int layer = 0)
            : base()
        {
            Layer = layer;
        }

        public int Layer { get; set; }

        public int CompareTo(InputLayer? other)
        {
            if (other is null)
            {
                throw new NullReferenceException("Other is null.");
            }
            return other.Layer - Layer; // Lower = Higher priority
        }
    }
}
