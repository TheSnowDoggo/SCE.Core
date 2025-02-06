namespace SCE
{
    /// <summary>
    /// A extension class of <see cref="Pixel"/> <see cref="Grid2D{T}"/>.
    /// </summary>
    public class DisplayMap : Grid2D<Pixel>
    {
        #region VGrid2DActions

        private static Func<Pixel, char, Pixel> ElementFFunc { get; } = (old, val) => new(val, old.FgColor, old.BgColor);

        private static Func<Pixel, SCEColor, Pixel> FgColorFFunc { get; } = (old, val) => new(old.Element, val, old.BgColor);

        private static Func<Pixel, SCEColor, Pixel> BgColorFFunc { get; } = (old, val) => new(old.Element, old.FgColor, val);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMap"/> class.
        /// </summary>
        /// <param name="width">The width of the display map.</param>
        /// <param name="height">The height of the display map.</param>
        /// <param name="bgColor">The default background color to fill with.</param>
        public DisplayMap(int width, int height, SCEColor? bgColor = null)
            : base(width, height)
        {
            Elements = new(this, ElementFFunc);
            FgColors = new(this, FgColorFFunc);
            BgColors = new(this, BgColorFFunc);

            if (bgColor is SCEColor color)
                Data.Fill(new Pixel(color));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMap"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the display map.</param>
        /// <param name="bgColor">The default background color to fill with.</param>
        public DisplayMap(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMap"/> class.
        /// </summary>
        /// <param name="pixelGrid">The default data.</param>
        public DisplayMap(Grid2D<Pixel> pixelGrid)
            : base(pixelGrid)
        {
            Elements = new(this, ElementFFunc);
            FgColors = new(this, FgColorFFunc);
            BgColors = new(this, BgColorFFunc);
        }

        #endregion

        #region VGrid2D

        /// <summary>
        /// Gets the elements vgrid.
        /// </summary>
        public VirtualGrid2D<Pixel, char> Elements { get; }

        /// <summary>
        /// Gets the foreground color vgrid.
        /// </summary>
        public VirtualGrid2D<Pixel, SCEColor> FgColors { get; }

        /// <summary>
        /// Gets the background color vgrid.
        /// </summary>
        public VirtualGrid2D<Pixel, SCEColor> BgColors { get; }

        #endregion

        #region Clone

        /// <summary>
        /// Creates a shallow copy of the <see cref="DisplayMap"/>.
        /// </summary>
        /// <returns>A shallow copy of the <see cref="DisplayMap"/>.</returns>
        public override DisplayMap Clone()
        {
            return new(base.Clone());
        }

        #endregion

        #region LineMapping

        /// <summary>
        /// Maps a string line at a specified start position.
        /// </summary>
        /// <param name="line">The line to map.</param>
        /// <param name="pos">The starting zero-based position of the line.</param>
        /// <param name="fgColor">The foreground color to map with.</param>
        /// <param name="bgColor">The background color to map with (by default transparent).</param>
        public void MapLine(string line, Vector2Int pos, SCEColor fgColor, SCEColor? bgColor = null)
        {
            if (!InRange(pos))
                throw new ArgumentException($"Position {pos} is not valid.");

            if (pos.X + line.Length > Dimensions.X)
                throw new LineOverflowException();

            for (int i = 0; i < line.Length; ++i)
            {
                int mappedX = i + pos.X;
                this[mappedX, pos.Y] = Pixel.Merge(new Pixel(line[i], fgColor, bgColor ?? SCEColor.Transparent), this[mappedX, pos.Y]);
            }
        }

        /// <summary>
        /// Maps a string line at a specified start position.
        /// </summary>
        /// <param name="line">The line to map.</param>
        /// <param name="pos">The starting zero-based position of the line.</param>
        /// <param name="colors">The colors to map with.</param>
        public void MapString(string line, Vector2Int pos, ColorSet colors)
        {
            MapLine(line, pos, colors.FgColor, colors.BgColor);
        }

        /// <summary>
        /// Maps a string line at a specified y position. 
        /// </summary>
        /// <param name="line">The line to map.</param>
        /// <param name="y">The starting zero-based y position of the line.</param>
        /// <param name="fgColor">The foreground color to map with.</param>
        /// <param name="bgColor">The background color to map with (by default transparent).</param>
        public void MapString(string line, int y, SCEColor fgColor, SCEColor? bgColor = null)
        {
            MapLine(line, new Vector2Int(0, y), fgColor, bgColor);
        }

        /// <summary>
        /// Maps a string line at a specified y position. 
        /// </summary>
        /// <param name="line">The line to map.</param>
        /// <param name="y">The starting zero-based y position of the line.</param>
        /// <param name="colors">The colors to map with.</param>
        public void MapString(string line, int y, ColorSet colors)
        {
            MapString(line, new Vector2Int(0, y), colors);
        }

        #endregion

        #region Mapping

        /// <inheritdoc/>
        public override void MapToArea(Grid2D<Pixel> dataGrid, Rect2D dataGridArea, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            var validSetOffset = positionOffset ?? Vector2Int.Zero;

            void CycleAction(Vector2Int position)
            {
                var mappedPos = position + validSetOffset;
                if (!tryTrimOnResize || InRange(mappedPos))
                    this[mappedPos] = Pixel.Merge(dataGrid[position], this[mappedPos]);
            }

            CustomMapToArea(CycleAction, dataGrid, dataGridArea, validSetOffset, tryTrimOnResize);
        }

        /// <inheritdoc/>
        public override void MapTo(Grid2D<Pixel> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            MapToArea(dataGrid, dataGrid.GridArea, positionOffset, tryTrimOnResize);
        }

        /// <inheritdoc/>
        public override void MapAreaFrom(Grid2D<Pixel> dataGrid, Rect2D thisArea, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            var validGetOffset = positionOffset ?? Vector2Int.Zero;

            void CycleAction(Vector2Int pos)
            {
                var mappedPos = pos + validGetOffset;
                if (!tryTrimOnResize || dataGrid.InRange(mappedPos))
                    this[pos] = Pixel.Merge(dataGrid[mappedPos], this[pos]);
            }

            CustomMapAreaFrom(CycleAction, dataGrid, thisArea, validGetOffset, tryTrimOnResize);
        }

        /// <inheritdoc/>
        public override void MapFrom(Grid2D<Pixel> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            MapAreaFrom(dataGrid, GridArea, positionOffset, tryTrimOnResize);
        }

        #endregion
    }
}