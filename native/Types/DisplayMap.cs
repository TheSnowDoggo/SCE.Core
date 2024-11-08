using System.Collections;

namespace SCECore.Types
{
    /// <summary>
    /// A wrapper class of <see cref="Pixel"/> <see cref="Grid2D{T}"/> with additional filling and mapping features.
    /// </summary>
    public class DisplayMap : Grid2D<Pixel>, IEquatable<DisplayMap>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMap"/> class given its dimensions.
        /// </summary>
        /// <param name="dimensions">The dimensions of the new instance.</param>
        public DisplayMap(Vector2Int dimensions)
            : base(dimensions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMap"/> class given its dimensions and initial background color.
        /// </summary>
        /// <param name="dimensions">The dimensions of the new instance.</param>
        /// <param name="bgColor">The starting background color to fill the new instance with.</param>
        public DisplayMap(Vector2Int dimensions, Color bgColor)
            : base(dimensions)
        {
            if (bgColor != Color.Black)
            {
                BgColorFill(bgColor);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMap"/> class given a base <see cref="Pixel"/> <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="pixelGrid">The base <see cref="Grid2D{T}"/> to set the new instance to.</param>
        public DisplayMap(Grid2D<Pixel> pixelGrid)
            : base(pixelGrid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMap"/> class given a base set of <see cref="Grid2D{T}"/> of each element to be converted to a <see cref="Pixel"/> <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="elementGrid">The grid containing every element of the overal <see cref="Pixel"/> <see cref="Grid2D{T}"/>.</param>
        /// <param name="fgGrid">The grid containing every foreground color of the overal <see cref="Pixel"/> <see cref="Grid2D{T}"/>.</param>
        /// <param name="bgGrid">The grid containing every background color of the overal <see cref="Pixel"/> <see cref="Grid2D{T}"/>.</param>
        public DisplayMap(Grid2D<string> elementGrid, Grid2D<Color> fgGrid, Grid2D<Color> bgGrid)
            : base(ToPixelGrid(elementGrid, fgGrid, bgGrid))
        {
        }

        /// <summary>
        /// A delegate called to map a single <see cref="string"/> line to this instance.
        /// </summary>
        /// <param name="position">The starting position of the line to map in this instance.</param>
        /// <param name="line">The <see cref="string"/> line to be mapped.</param>
        /// <param name="fgColor">The foreground color to fill every affected <see cref="Pixel"/> with.</param>
        /// <param name="bgColor">The background color to fill every affected <see cref="Pixel"/> with.</param>
        public delegate void CustomMapLine(Vector2Int position, string line, Color fgColor = Color.White, Color bgColor = Color.Transparent);

        /// <summary>
        /// Returns a <see cref="Pixel"/> <see cref="Grid2D{T}"/> combined from a given element <see cref="string"/> <see cref="Grid2D{T}"/>, a foreground color  <see cref="byte"/>  <see cref="Grid2D{T}"/> and a background color  <see cref="byte"/> <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="elementGrid">The <see cref="string"/> <see cref="Grid2D{T}"/> containing the <see cref="Pixel.Element"/> of every <see cref="Pixel"/>.</param>
        /// <param name="fgGrid">The <see cref="Color"/> <see cref="Grid2D{T}"/> containing the <see cref="Pixel.FgColor"/> of every <see cref="Pixel"/>.</param>
        /// <param name="bgGrid">The <see cref="Color"/> <see cref="Grid2D{T}"/> containing the <see cref="Pixel.BgColor"/> of every <see cref="Pixel"/>.</param>
        /// <returns>A <see cref="Pixel"/> <see cref="Grid2D{T}"/> combined of each given <see cref="Grid2D{T}"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the given <paramref name="elementGrid"/>, <paramref name="fgGrid"/> and <paramref name="bgGrid"/> don't all have the same dimensions.</exception>
        public static Grid2D<Pixel> ToPixelGrid(Grid2D<string> elementGrid, Grid2D<Color> fgGrid, Grid2D<Color> bgGrid)
        {
            if (elementGrid.Dimensions != fgGrid.Dimensions || fgGrid.Dimensions != bgGrid.Dimensions)
            {
                throw new ArgumentException("Dimensions do not match");
            }

            Grid2D<Pixel> pixelGrid = new(elementGrid.Dimensions);

            for (int x = 0; x < pixelGrid.Width; x++)
            {
                for (int y = 0; y < pixelGrid.Height; y++)
                {
                    pixelGrid[x, y] = new(elementGrid[x, y], fgGrid[x, y], bgGrid[x, y]);
                }
            }

            return pixelGrid;
        }

        /// <inheritdoc/>
        public override DisplayMap Clone()
        {
            return new(base.Clone());
        }

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

        /// <summary>
        /// Maps a single <see cref="string"/> line to a specified position in this instance.
        /// </summary>
        /// <param name="position">The starting position of the line to map in this instance.</param>
        /// <param name="line">The <see cref="string"/> line to be mapped.</param>
        /// <param name="fgColor">The foreground color to fill every affected <see cref="Pixel"/> with.</param>
        /// <param name="bgColor">The background color to fill every affected <see cref="Pixel"/> with.</param>
        /// <exception cref="ArgumentException">Throws exception when the starting position is invalid or the line will overflow this instance.</exception>
        public void MapLine(Vector2Int position, string line, Color fgColor = Color.White, Color bgColor = Color.Transparent)
        {
            if (!PositionValid(position))
            {
                throw new ArgumentException("Starting position is not valid");
            }

            line = SCEString.PadAfterToEven(line);

            int pixelLength = line.Length / Pixel.PIXELWIDTH, endX = position.X + pixelLength;
            if (endX > Dimensions.X)
            {
                throw new ArgumentException($"Line (Length:{pixelLength}px,posX:{position.X}) will overflow current width ({Dimensions.X}px) by {endX - Dimensions.X}");
            }

            for (int x = 0; x < pixelLength; x++)
            {
                string build = string.Empty;

                for (int i = 0; i < Pixel.PIXELWIDTH; i++)
                {
                    build += line[(x * Pixel.PIXELWIDTH) + i];
                }

                Vector2Int mappedPos = position + new Vector2Int(x, 0);

                Pixel oldPixel = this[mappedPos];

                Color newBgColor = SCEColor.GetStackColor(bgColor, oldPixel.BgColor);

                Color newFgColor = SCEColor.GetStackColor(fgColor, oldPixel.FgColor);

                string newElement = newBgColor != Color.Transparent ? build : SCEString.MergeString(build, oldPixel.Element);

                this[mappedPos] = new(newElement, newFgColor, newBgColor);
            }
        }

        /// <summary>
        /// Maps a <see cref="Text"/> object to this instance.
        /// </summary>
        /// <param name="text">The <see cref="Text"/> to map.</param>
        /// <param name="customMapLine">The <see cref="CustomMapLine"/> delegate to use to map each individual <see cref="string"/> line.</param>
        /// <exception cref="ArgumentException">Throws exception if the <paramref name="text"/> <see cref="Text.Data"/> is empty.</exception>
        public void MapText(Text text, CustomMapLine? customMapLine = null)
        {
            if (text.Data == string.Empty)
            {
                throw new ArgumentException("Text data is empty");
            }

            customMapLine ??= MapLine;

            string[] lineArray = GetSplitLineArray(text);

            int i = 0, topY = Height - 1, startY = text.GetStartMapY(lineArray.Length, Height);
            do
            {
                string line = text.GetFormattedBody(lineArray[i]);

                Vector2Int position = new(text.GetStartMapX(line.Length, Width), topY - (startY + i));

                customMapLine(position, line, text.FgColor, text.BgColor);

                i++;
            }
            while (i < lineArray.Length && i < Height);
        }

        /// <summary>
        /// Returns a <see cref="string"/> array of every line the <see cref="Text.Data"/> of the given <paramref name="text"/> can be split into.
        /// </summary>
        /// <param name="text">The <see cref="Text"/> to split up its <see cref="Text.Data"/>.</param>
        /// <returns>A <see cref="string"/> array of every line the <see cref="Text.Data"/> of the given <paramref name="text"/> can be split into.</returns>
        public string[] GetSplitLineArray(Text text)
        {
            if (text.NewLineOverflow)
            {
                return SCEString.SmartSplitLineArray(text.Data, Width * Pixel.PIXELWIDTH, Height);
            }
            else
            {
                return SCEString.BasicSplitLineArray(text.Data, Height);
            }
        }

        /// <summary>
        /// Replaces every element in the defined <see cref="Area2D"/> in the <see cref="DisplayMap"/> with the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The <see cref="string"/> value to fill with.</param>
        /// <param name="area">The area in this instance to set every <see cref="Pixel.Element"/>.</param>
        /// <param name="ignoreOverflow">Whether overflow errors should be silently dealt with.</param>
        public void ElementFillArea(string element, Area2DInt area, bool ignoreOverflow = false)
        {
            void CycleAction(Vector2Int pos)
            {
                Pixel oldPixel = this[pos];

                this[pos] = new(element, oldPixel.FgColor, oldPixel.BgColor);
            }

            GenericCycleArea(CycleAction, area, ignoreOverflow);
        }

        /// <summary>
        /// Replaces every element in the <see cref="DisplayMap"/> with the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The <see cref="string"/> value to fill with.</param>
        public void ElementFill(string element)
        {
            ElementFillArea(element, GridArea);
        }

        /// <summary>
        /// Replaces every element in the defined <see cref="Area2D"/> in the <see cref="DisplayMap"/> with the given <paramref name="fgColor"/>.
        /// </summary>
        /// <param name="fgColor">The foreground <see cref="byte"/> color to fill with.</param>
        /// <param name="area">The area in this instance to set every <see cref="Pixel.FgColor"/>.</param>
        /// <param name="ignoreOverflow">Whether overflow errors should be silently dealt with.</param>
        public void FgColorFillArea(Color fgColor, Area2DInt area, bool ignoreOverflow = false)
        {
            void CycleAction(Vector2Int pos)
            {
                Pixel oldPixel = this[pos];

                this[pos] = new(oldPixel.Element, fgColor, oldPixel.BgColor);
            }

            GenericCycleArea(CycleAction, area, ignoreOverflow);
        }

        /// <summary>
        /// Replaces every element in the <see cref="DisplayMap"/> with the given <paramref name="fgColor"/>.
        /// </summary>
        /// <param name="fgColor">The foreground <see cref="byte"/> color to fill with.</param>
        public void FgColorFill(Color fgColor)
        {
            FgColorFillArea(fgColor, GridArea);
        }

        /// <summary>
        /// Replaces every element in a defined <see cref="Area2D"/> in the <see cref="DisplayMap"/> with the given <paramref name="bgColor"/>.
        /// </summary>
        /// <param name="bgColor">The background <see cref="byte"/> color to fill with.</param>
        /// <param name="area">The area in this instance to set every <see cref="Pixel.BgColor"/>.</param>
        /// <param name="ignoreOverflow">Overflow errors are be silently dealt if true; otherwise, throws errors.</param>
        public void BgColorFillArea(Color bgColor, Area2DInt area, bool ignoreOverflow = false)
        {
            void CycleAction(Vector2Int pos)
            {
                Pixel oldPixel = this[pos];

                this[pos] = new(oldPixel.Element, oldPixel.FgColor, bgColor);
            }

            GenericCycleArea(CycleAction, area, ignoreOverflow);
        }

        /// <summary>
        /// Replaces every element in the <see cref="DisplayMap"/> with the given <paramref name="bgColor"/>.
        /// </summary>
        /// <param name="bgColor">The background <see cref="byte"/> color to fill with.</param>
        public void BgColorFill(Color bgColor)
        {
            BgColorFillArea(bgColor, GridArea);
        }

        /// <summary>
        /// Replaces every element in a given horizontal line <paramref name="y"/> and range in the <see cref="DisplayMap"/> with the given <paramref name="bgColor"/>.
        /// </summary>
        /// <param name="bgColor">The background <see cref="byte"/> color to fill with.</param>
        /// <param name="y">The constant y-coordinate of the line.</param>
        /// <param name="range">The x-range of the line to fill.</param>
        public void BgColorFillHorizontalArea(Color bgColor, int y, Vector2Int range)
        {
            BgColorFillArea(bgColor, new Area2DInt(new(range.X, y), new(range.Y, y + 1)));
        }

        /// <summary>
        /// Replaces every element in a given vertical line <paramref name="x"/> and range in the <see cref="DisplayMap"/> with the given <paramref name="bgColor"/>.
        /// </summary>
        /// <param name="bgColor">The background <see cref="byte"/> color to fill with.</param>
        /// <param name="x">The constant x-coordinate of the line.</param>
        /// <param name="range">The y-range of the line to fill.</param>
        public void BgColorFillVerticalArea(Color bgColor, int x, Vector2Int range)
        {
            BgColorFillArea(bgColor, new Area2DInt(new(x, range.X), new(x + 1, range.Y)));
        }

        /// <summary>
        /// Replaces every element in a given horizontal line <paramref name="y"/> in the <see cref="DisplayMap"/>  with the given <paramref name="bgColor"/>.
        /// </summary>
        /// <param name="bgColor">The background <see cref="byte"/> color to fill with.</param>
        /// <param name="y">The constant y-coordinate of the line.</param>
        public void BgColorFillHorizontal(Color bgColor, int y)
        {
            BgColorFillHorizontalArea(bgColor, y, new(0, Width));
        }

        /// <summary>
        /// Replaces every element in a given vertical line <paramref name="x"/> in the <see cref="DisplayMap"/> with the given <paramref name="bgColor"/>.
        /// </summary>
        /// <param name="bgColor">The background <see cref="byte"/> color to fill with.</param>
        /// <param name="x">The constant x-coordinate of the line.</param>
        public void BgColorFillVertical(Color bgColor, int x)
        {
            BgColorFillVerticalArea(bgColor, x, new(0, Height));
        }

        /// <inheritdoc/>
        public override void MapTo(Grid2D<Pixel> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            MapToArea(dataGrid, dataGrid.GridArea, positionOffset, tryTrimOnResize);
        }

        /// <inheritdoc/>
        public override void MapToArea(Grid2D<Pixel> dataGrid, Area2DInt dataGridArea, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            Vector2Int validSetOffset = positionOffset ?? Vector2Int.Zero;

            void CycleAction(Vector2Int position)
            {
                Vector2Int mappedPos = position + validSetOffset;

                if (!tryTrimOnResize || PositionValid(mappedPos))
                {
                    Pixel pixel = dataGrid[position], oldPixel = this[mappedPos];

                    this[mappedPos] = new(pixel.Element, SCEColor.GetStackColor(pixel.FgColor, oldPixel.FgColor), SCEColor.GetStackColor(pixel.BgColor, oldPixel.BgColor));
                }
            }

            CustomMapToArea(CycleAction, dataGrid, dataGridArea, validSetOffset, tryTrimOnResize);
        }

        /// <inheritdoc/>
        public override void MapFrom(Grid2D<Pixel> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            MapAreaFrom(dataGrid, GridArea, positionOffset, tryTrimOnResize);
        }

        /// <inheritdoc/>
        public override void MapAreaFrom(Grid2D<Pixel> dataGrid, Area2DInt thisArea, Vector2Int? positionOffset = null, bool tryTrimOnResize = false)
        {
            Vector2Int validGetOffset = positionOffset ?? Vector2Int.Zero;

            void CycleAction(Vector2Int pos)
            {
                Vector2Int mappedPos = pos + validGetOffset;

                if (!tryTrimOnResize || dataGrid.PositionValid(mappedPos))
                {
                    Pixel pixel = dataGrid[mappedPos], oldPixel = this[pos];

                    this[pos] = new(pixel.Element, SCEColor.GetStackColor(pixel.FgColor, oldPixel.FgColor), SCEColor.GetStackColor(pixel.BgColor, oldPixel.BgColor));
                }
            }

            CustomMapAreaFrom(CycleAction, dataGrid, thisArea, validGetOffset, tryTrimOnResize);
        }
    }
}
