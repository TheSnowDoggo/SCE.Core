namespace SCE
{
    /// <summary>
    /// A extension class of <see cref="Pixel"/> <see cref="Grid2D{T}"/>.
    /// </summary>
    public class DisplayMap : Grid2D<Pixel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMap"/> class.
        /// </summary>
        /// <param name="width">The width of the display map.</param>
        /// <param name="height">The height of the display map.</param>
        /// <param name="bgColor">The default background color to fill with.</param>
        public DisplayMap(int width, int height, SCEColor? bgColor = null)
            : base(width, height)
        {
            if (bgColor is SCEColor color)
            {
                Fill(new Pixel(color));
            }
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
        }

        /// <summary>
        /// Creates a shallow copy of the <see cref="DisplayMap"/>.
        /// </summary>
        /// <returns>A shallow copy of the <see cref="DisplayMap"/>.</returns>
        public override DisplayMap Clone()
        {
            return new(base.Clone());
        }

        public static implicit operator DisplayMapView(DisplayMap dpMap) => dpMap.ToView();

        public DisplayMapView ToView()
        {
            return new(this);
        }

        #region Fill

        public Func<Vector2Int, Pixel> FgFill(SCEColor color)
        {
            return pos => new(this[pos].Element, color, this[pos].BgColor);
        }

        public Func<Vector2Int, Pixel> BgFill(SCEColor color)
        {
            return pos => new(this[pos].Element, this[pos].FgColor, color);
        }

        public Func<Vector2Int, Pixel> ElementFill(char element)
        {
            return pos => new(element, this[pos].FgColor, this[pos].BgColor);
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
        public bool MapString(string line, Vector2Int pos, SCEColor fgColor, SCEColor bgColor = SCEColor.Transparent)
        {
            if (!Contains(pos) || pos.X + line.Length > Dimensions.X)
            {
                return false;
            }

            for (int i = 0; i < line.Length; ++i)
            {
                int mappedX = i + pos.X;
                this[mappedX, pos.Y] = Pixel.Merge(new Pixel(line[i], fgColor, bgColor), this[mappedX, pos.Y]);
            }

            return true;
        }

        /// <summary>
        /// Maps a string line at a specified start position.
        /// </summary>
        /// <param name="line">The line to map.</param>
        /// <param name="pos">The starting zero-based position of the line.</param>
        /// <param name="colorSet">The colors to map with.</param>
        public void MapString(string line, Vector2Int pos, ColorSet colorSet)
        {
            MapString(line, pos, colorSet.FgColor, colorSet.BgColor);
        }

        /// <summary>
        /// Maps a string line at a specified y position. 
        /// </summary>
        /// <param name="line">The line to map.</param>
        /// <param name="y">The starting zero-based y position of the line.</param>
        /// <param name="fgColor">The foreground color to map with.</param>
        /// <param name="bgColor">The background color to map with (by default transparent).</param>
        public void MapString(string line, int y, SCEColor fgColor, SCEColor bgColor = SCEColor.Transparent)
        {
            MapString(line, new Vector2Int(0, y), fgColor, bgColor);
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

        /// <inheritdoc cref="Grid2D{Pixel}.MapTo(Grid2DView{Pixel}, Rect2D, Vector2Int)"/>
        public void PMapTo(Grid2DView<Pixel> dataGrid, Rect2D dataGridArea, Vector2Int positionOffset)
        {
            foreach (var pos in EnumerateMapTo(dataGrid, dataGridArea, positionOffset))
            {
                var mappedPos = pos + positionOffset;
                this[mappedPos] = Pixel.Merge(dataGrid[pos], this[mappedPos]);
            }
        }

        /// <inheritdoc cref="Grid2D{Pixel}.MapTo(Grid2DView{Pixel}, Rect2D)"/>
        public void PMapTo(Grid2DView<Pixel> dataGrid, Rect2D dataGridArea)
        {
            PMapTo(dataGrid, dataGridArea, Vector2Int.Zero);
        }

        /// <inheritdoc cref="Grid2D{Pixel}.MapTo(Grid2DView{Pixel}, Vector2Int)"/>
        public void PMapTo(Grid2DView<Pixel> dataGrid, Vector2Int positionOffset)
        {
            PMapTo(dataGrid, dataGrid.GridArea(), positionOffset);
        }

        /// <inheritdoc cref="Grid2D{Pixel}.MapTo(Grid2DView{Pixel})"/>
        public void PMapTo(Grid2DView<Pixel> dataGrid)
        {
            PMapTo(dataGrid, dataGrid.GridArea(), Vector2Int.Zero);
        }

        /// <inheritdoc cref="Grid2D{Pixel}.MapFrom(Grid2DView{Pixel}, Rect2D, Vector2Int)"/>
        public void PMapFrom(Grid2DView<Pixel> dataGrid, Rect2D thisArea, Vector2Int positionOffset)
        {
            foreach (var pos in EnumerateMapFrom(dataGrid, thisArea, positionOffset))
            {
                var mappedPos = pos + positionOffset;
                this[pos] = Pixel.Merge(dataGrid[mappedPos], this[pos]);
            }
        }

        /// <inheritdoc cref="Grid2D{Pixel}.MapFrom(Grid2DView{Pixel}, Rect2D)"/>
        public void PMapFrom(Grid2DView<Pixel> dataGrid, Rect2D thisArea)
        {
            PMapFrom(dataGrid, thisArea, Vector2Int.Zero);
        }

        /// <inheritdoc cref="Grid2D{Pixel}.MapFrom(Grid2DView{Pixel}, Vector2Int)"/>
        public void PMapFrom(Grid2DView<Pixel> dataGrid, Vector2Int positionOffset)
        {
            PMapFrom(dataGrid, GridArea(), positionOffset);
        }

        /// <inheritdoc cref="Grid2D{Pixel}.MapFrom(Grid2DView{Pixel})"/>
        public void PMapFrom(Grid2DView<Pixel> dataGrid)
        {
            PMapFrom(dataGrid, GridArea(), Vector2Int.Zero);
        }

        #endregion
    }
}