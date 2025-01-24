namespace SCE
{
    /// <summary>
    /// A wrapper class of <see cref="Pixel"/> <see cref="Grid2D{T}"/> with additional filling and mapping features.
    /// </summary>
    public class DisplayMap : Grid2D<Pixel>, IEquatable<DisplayMap>
    {
        public DisplayMap(int width, int height, Color? bgColor = null)
            : base(width, height)
        {
            if (bgColor is Color color)
                BgColorFill(color);
        }

        public DisplayMap(Vector2Int dimensions, Color? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        public DisplayMap(Grid2D<Pixel> pixelGrid)
            : base(pixelGrid)
        {
        }

        #region Conversion
        public static Grid2D<Pixel> ToPixelGrid(Grid2D<string> elementGrid, Grid2D<Color> fgGrid, Grid2D<Color> bgGrid)
        {
            if (elementGrid.Dimensions != fgGrid.Dimensions || fgGrid.Dimensions != bgGrid.Dimensions)
                throw new ArgumentException("Dimensions do not match");

            Grid2D<Pixel> pixelGrid = new(elementGrid.Dimensions);
            for (int x = 0; x < pixelGrid.Width; x++)
            {
                for (int y = 0; y < pixelGrid.Height; y++)
                    pixelGrid[x, y] = new(elementGrid[x, y], fgGrid[x, y], bgGrid[x, y]);
            }
            return pixelGrid;
        }

        public static DisplayMap ToDisplayMap(Grid2D<string> elementGrid, Grid2D<Color> fgGrid, Grid2D<Color> bgGrid)
        {
            return new(ToPixelGrid(elementGrid, fgGrid, bgGrid));
        }
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
        public void MapString(Vector2Int position, string line, Color fgColor, Color bgColor)
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
        #endregion

        #region ElementFill
        public void ElementFillArea(string element, Area2DInt area, bool ignoreOverflow = false)
        {
            GenericCycleArea((pos) => this[pos] = new(element, this[pos].FgColor, this[pos].BgColor), area, ignoreOverflow);
        }

        public void ElementFill(string element)
        {
            ElementFillArea(element, GridArea);
        }
        #endregion

        #region FgColorFill
        public void FgColorFillArea(Color fgColor, Area2DInt area, bool ignoreOverflow = false)
        {
            GenericCycleArea((pos) => this[pos] = new(this[pos].Element, fgColor, this[pos].BgColor), area, ignoreOverflow);
        }

        public void FgColorFill(Color fgColor)
        {
            FgColorFillArea(fgColor, GridArea);
        }
        #endregion

        #region BgColorFill
        public void BgColorFillArea(Color bgColor, Area2DInt area, bool ignoreOverflow = false)
        {
            GenericCycleArea((pos) => this[pos] = new(this[pos].Element, this[pos].FgColor, bgColor), area, ignoreOverflow);
        }

        public void BgColorFill(Color bgColor)
        {
            BgColorFillArea(bgColor, GridArea);
        }

        public void BgColorFillHorizontalArea(Color bgColor, int y, Vector2Int range)
        {
            BgColorFillArea(bgColor, new Area2DInt(new Vector2Int(range.X, y), new Vector2Int(range.Y, y + 1)));
        }

        public void BgColorFillVerticalArea(Color bgColor, int x, Vector2Int range)
        {
            BgColorFillArea(bgColor, new Area2DInt(new(x, range.X), new(x + 1, range.Y)));
        }

        public void BgColorFillHorizontal(Color bgColor, int y)
        {
            BgColorFillHorizontalArea(bgColor, y, new(0, Width));
        }

        public void BgColorFillVertical(Color bgColor, int x)
        {
            BgColorFillVerticalArea(bgColor, x, new(0, Height));
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
