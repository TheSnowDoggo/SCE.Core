namespace SCE
{
    /// <summary>
    /// A wrapper class of <see cref="Pixel"/> <see cref="Grid2D{T}"/> with additional filling and mapping features.
    /// </summary>
    public class DisplayMap : Grid2D<Pixel>, IEquatable<DisplayMap>
    {
        #region VGrid2DActions
        private static Func<Pixel, char, Pixel> ElementFFunc { get; } = (old, val) => new(val, old.FgColor, old.BgColor);
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
        public VirtualGrid2D<Pixel, char> Elements { get; }

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
        public void MapString(Vector2Int pos, string line, SCEColor fgColor, SCEColor bgColor)
        {
            if (!PositionValid(pos))
                throw new InvalidPositionException($"Position {pos} is not valid.");

            if (pos.X + line.Length > Dimensions.X)
                throw new LineOverflowException();

            for (int i = 0; i < line.Length; ++i)
            {
                int mappedX = i + pos.X;
                this[mappedX, pos.Y] = Pixel.Merge(new Pixel(line[i], fgColor, bgColor), this[mappedX, pos.Y]);
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
                    this[mappedPos] = Pixel.Merge(dataGrid[position], this[mappedPos]);
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
                    this[pos] = Pixel.Merge(dataGrid[mappedPos], this[pos]);
            }

            CustomMapAreaFrom(CycleAction, dataGrid, thisArea, validGetOffset, tryTrimOnResize);
        }
        #endregion
    }
}
