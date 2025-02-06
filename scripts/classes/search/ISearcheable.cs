namespace SCE
{
    /// <summary>
    /// An interface for types with names to be used in a <see cref="SearchHash{T}"/>
    /// </summary>
    public interface ISearcheable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }
    }
}
