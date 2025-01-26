namespace SCE
{
    /// <summary>
    /// A wrapper class of <see cref="Pixel"/> <see cref="Grid2D{T}"/> with additional filling and mapping features.
    /// </summary>
    public class DisplayMap : Grid2D<Pixel>, IEquatable<DisplayMap>
    {
        #region VGrid2DActions
        private static Func<Pixel, string, Pixel> ElementFFunc { get; } = (old, val) => new(val, old.FgColor, old.BgColor);
        private static Func<Pixel, SCEColor, Pixel> FgColorFFunc { get; } = (old, val) => new(old.Element, val, old.BgColor);
        private static Func<Pixel, SCEColor, Pixel> BgColorFFunc { get; } = (old, val) => new(old.Element, old.FgColor, val);
        #endregion

        #region Constructors
        public DisplayMap(int width, int height, SCEColor? bgColor = null)
            : base(width, height)
        {
            Elements = new(this, ElementFFunc);
            FgColors = new(this, FgColorFFunc);
            BgColors = new(this, BgColorFFunc);

            if (bgColor is SCEColor color)
                BgColors.Fill(color);
        }

        public DisplayMap(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        public DisplayMap(Grid2D<Pixel> pixelGrid)
            : base(pixelGrid)
        {
            Elements = new(this, ElementFFunc);
            FgColors = new(this, FgColorFFunc);
            BgColors = new(this, BgColorFFunc);
        }
        #endregion

        #region VGrid2D
        public VirtualGrid2D<Pixel, string> Elements { get; }

        public VirtualGrid2D<Pixel, SCEColor> FgColors { get; }

        public VirtualGrid2D<Pixel, SCEColor> BgColors { get; }
        #endregion

        #region Clone
        public override DisplayMap Clone()
        {
            return new(base.Clone());
        }
        #endregion

        #region Equals
        public bool Equals(DisplayMap? other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object? obj)
        {
            return obj is DisplayMap displayMap && Equals(displayMap);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region MapString
        public void MapString(Vector2Int position, string line, SCEColor fgColor, SCEColor bgColor)
        {
            if (!PositionValid(position))
                throw new InvalidPositionException($"Position {position} is not valid.");

            line = StringUtils.PadAfterToEven(line);

            int pixelLength = Pixel.GetPixelLength(line.Length);
            int endX = position.X + pixelLength;

            if (endX > Dimensions.X)
                throw new LineOverflowException();

            for (int x = 0; x < pixelLength; ++x)
            {
                var chrArr = new char[Pixel.PIXELWIDTH];

                for (int i = 0; i < Pixel.PIXELWIDTH; i++)
                    chrArr[i] = line[(x * Pixel.PIXELWIDTH) + i];

                Vector2Int mappedPos = position + new Vector2Int(x, 0);

                var newPixel = new Pixel(new(chrArr), fgColor, bgColor);

                this[mappedPos] = Pixel.MergeLayers(newPixel, this[mappedPos]);
            }
        }

        public void MapString(Vector2Int position, string line, ColorSet colorSet)
        {
            MapString(position, line, colorSet.FgColor, colorSet.BgColor);
        }

        public void MapString(int y, string line, SCEColor fgColor, SCEColor bgColor)
        {
            MapString(new Vector2Int(0, y), line, fgColor, bgColor);
        }

        public void MapString(int y, string line, ColorSet colorSet)
        {
            MapString(new Vector2Int(0, y), line, colorSet);
        }
        #endregion

        #region Mapping
        public override void MapTo(Grid2D<Pixel> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            MapToArea(dataGrid, dataGrid.GridArea, positionOffset, tryTrimOnResize);
        }

        public override void MapToArea(Grid2D<Pixel> dataGrid, Area2DInt dataGridArea, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            Vector2Int validSetOffset = positionOffset ?? Vector2Int.Zero;

            void CycleAction(Vector2Int position)
            {
                Vector2Int mappedPos = position + validSetOffset;
                if (!tryTrimOnResize || PositionValid(mappedPos))
                    this[mappedPos] = Pixel.MergeLayers(dataGrid[position], this[mappedPos]);
            }

            CustomMapToArea(CycleAction, dataGrid, dataGridArea, validSetOffset, tryTrimOnResize);
        }

        public override void MapFrom(Grid2D<Pixel> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            MapAreaFrom(dataGrid, GridArea, positionOffset, tryTrimOnResize);
        }

        public override void MapAreaFrom(Grid2D<Pixel> dataGrid, Area2DInt thisArea, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            Vector2Int validGetOffset = positionOffset ?? Vector2Int.Zero;

            void CycleAction(Vector2Int pos)
            {
                Vector2Int mappedPos = pos + validGetOffset;
                if (!tryTrimOnResize || dataGrid.PositionValid(mappedPos))
                    this[pos] = Pixel.MergeLayers(dataGrid[mappedPos], this[pos]);
            }

            CustomMapAreaFrom(CycleAction, dataGrid, thisArea, validGetOffset, tryTrimOnResize);
        }
        #endregion
    }
}
