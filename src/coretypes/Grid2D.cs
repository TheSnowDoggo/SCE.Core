using CSUtils;
using System.Collections;
namespace SCE
{
    /// <summary>
    /// A wrapper class of a 2D-array representing a grid with useful functions.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the grid.</typeparam>
    public class Grid2D<T> : IEnumerable<Vector2Int>, ICloneable
    {
        private T[,] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        public Grid2D(int width, int height)
        {
            data = new T[width, height];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the grid.</param>
        public Grid2D(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class.
        /// </summary>
        /// <param name="data">The default data for the grid.</param>
        private Grid2D(T[,] data)
        {
            this.data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class.
        /// </summary>
        /// <param name="grid">The default data for the grid.</param>
        public Grid2D(Grid2D<T> grid)
            : this(grid.data)
        {
        }

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int Width { get => data.GetLength(0); }

        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int Height { get => data.GetLength(1); }

        /// <summary>
        /// Gets the dimensions of the grid as a <see cref="Vector2Int"/>.
        /// </summary>
        /// <remarks>
        /// Dimensions stored as (x:Width, y:Height)
        /// </remarks>
        public Vector2Int Dimensions { get => new(Width, Height); }

        /// <summary>
        /// Invoked when the size of the grid is changed.
        /// </summary>
        public Action? OnResize;

        /// <summary>
        /// Invoked when the grid is cleared.
        /// </summary>
        public Action? OnClear;

        /// <summary>
        /// Gets or sets the element at the specified zero-based coordinates.
        /// </summary>
        /// <param name="x">The zero-based x coordinate.</param>
        /// <param name="y">The zero-based y coordinate.</param>
        /// <returns>The element at the specified zero-based coordinates.</returns>
        public T this[int x, int y]
        {
            get => data[x, y];
            set => data[x, y] = value;
        }

        /// <summary>
        /// Gets or sets the element at the specified zero-based coordinates.
        /// </summary>
        /// <param name="pos">The zero-based <see cref="Vector2Int"/> coordinates.</param>
        /// <returns>The element at the specified zero-based coordinates.</returns>
        public T this[Vector2Int pos]
        {
            get => this[pos.X, pos.Y];
            set => this[pos.X, pos.Y] = value;
        }

        /// <summary>
        /// Gets the zero-based area of the grid as a <see cref="Rect2D"/>.
        /// </summary>
        public Rect2D GridArea()
        {
            return new(Width, Height);
        }

        /// <summary>
        /// Return the size of the grid (width * height).
        /// </summary>
        /// <returns>The size of the grid.</returns>
        public int Size()
        {
            return Width * Height;
        }

        #region Clone

        /// <summary>
        /// Creates a shallow copy of the <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <returns>A shallow copy of the <see cref="Grid2D{T}"/>.</returns>
        public virtual Grid2D<T> Clone()
        {
            return new((T[,])data.Clone());
        }
        
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Contains

        /// <summary>
        /// Determines whether the specified zero-based coordinates lie within this grid.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><see langword="true"/> if the specified coordinates lie within this grid; otherwise, <see langword="false"/>.</returns>
        public bool Contains(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        /// <summary>
        /// Determines whether the specified zero-based coordinates lie within this grid.
        /// </summary>
        /// <param name="pos">The <see cref="Vector2Int"/> coordinate.</param>
        /// <returns><see langword="true"/> if the specified coordinates lie within this grid; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Vector2Int pos)
        {
            return Contains(pos.X, pos.Y);
        }

        #endregion

        #region Enumerate

        public IEnumerable<Vector2Int> EnumerateGrid(Rect2D area, bool rowMajor = true)
        {
            var thisArea = GridArea();
            if (!Rect2D.Overlaps(thisArea, area))
            {
                yield break;
            }

            area = thisArea.TrimArea(area);

            int s1 = rowMajor ? area.Bottom : area.Left;
            int s2 = rowMajor ? area.Left : area.Bottom;
            int e1 = rowMajor ? area.Top : area.Right;
            int e2 = rowMajor ? area.Right : area.Top;
            for (int i = s1; i < e1; ++i)
            {
                for (int j = s2; j < e2; ++j)
                {
                    yield return rowMajor ? new(j, i) : new(i, j);
                }
            }
        }

        public IEnumerable<Vector2Int> EnumerateGrid(bool rowMajor = true)
        {
            int e1 = rowMajor ? Height : Width;
            int e2 = rowMajor ? Width : Height;
            for (int i = 0; i < e1; ++i)
            {
                for (int j = 0; j < e2; ++j)
                {
                    yield return rowMajor ? new(j, i) : new(i, j);
                }
            }
        }

        public IEnumerator<Vector2Int> GetEnumerator()
        {
            return (IEnumerator<Vector2Int>)EnumerateGrid();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Fill

        /// <summary>
        /// Fills the grid over a specified area <paramref name="area"/>.
        /// </summary>
        /// <param name="fill">The function to fill with.</param>
        /// <param name="area">The area to fill over.</param>
        public void Fill(Func<Vector2Int, T> fill, Rect2D area)
        {
            foreach (var pos in EnumerateGrid(area))
            {
                this[pos] = fill.Invoke(pos);
            }
        }

        /// <summary>
        /// Fills the grid over a specified area <paramref name="area"/>.
        /// </summary>
        /// <param name="value">The item to fill with.</param>
        /// <param name="area">The area to fill over.</param>
        public void Fill(T value, Rect2D area)
        {
            Fill(pos => value, area);
        }

        /// <summary>
        /// Fills the grid.
        /// </summary>
        /// <param name="fill">The function to fill with.</param>
        public void Fill(Func<Vector2Int, T> fill)
        {
            foreach (var pos in this)
            {
                this[pos] = fill.Invoke(pos);
            }
        }

        /// <summary>
        /// Fills the grid.
        /// </summary>
        /// <param name="value">The item to fill with.</param>
        public void Fill(T value)
        {
            Fill(pos => value);
        }

        #endregion

        #region Mapping

        /// <summary>
        /// Enumerates the mapped area <paramref name="dataGridArea"/> of the specified <paramref name="dataGrid"/> onto this grid with a specified offset <paramref name="thisOffset"/>
        /// </summary>
        /// <remarks>
        /// Often used for mapping a small grid (<paramref name="dataGrid"/>) onto a large grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="dataGridArea">The area on the <paramref name="dataGrid"/> to get elements from.</param>
        /// <param name="thisOffset">The offset position on this grid.</param>
        public IEnumerable<Vector2Int> EnumerateMapTo(Grid2D<T> dataGrid, Rect2D dataGridArea, Vector2Int thisOffset)
        {
            var dataArea = dataGrid.GridArea();
            if (!Rect2D.Overlaps(dataArea, dataGridArea))
            {
                yield break;
            }

            var thisArea = GridArea();

            var offsetArea = dataGridArea + thisOffset;
            if (!Rect2D.Overlaps(thisArea, offsetArea))
            {
                yield break;
            }

            dataGridArea = thisArea.TrimArea(offsetArea) - thisOffset;

            foreach (var pos in dataGrid.EnumerateGrid(dataGridArea))
            {
                yield return pos;
            }
        }

        /// <summary>
        /// Maps the area <paramref name="dataGridArea"/> of the specified <paramref name="dataGrid"/> onto this grid.
        /// </summary>
        /// <remarks>
        /// Often used for mapping a small grid (<paramref name="dataGrid"/>) onto a large grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="dataGridArea">The area on the <paramref name="dataGrid"/> to get elements from.</param>
        /// <param name="thisOffset">The offset position on this grid.</param>
        public void MapTo(Grid2D<T> dataGrid, Rect2D dataGridArea, Vector2Int thisOffset)
        {
            foreach (var pos in EnumerateMapTo(dataGrid, dataGridArea, thisOffset))
            {
                this[pos + thisOffset] = dataGrid[pos];
            }
        }

        /// <inheritdoc cref="MapTo(Grid2D{T}, Rect2D, Vector2Int)"/>
        public void MapTo(Grid2D<T> dataGrid, Rect2D dataGridArea)
        {
            MapTo(dataGrid, dataGridArea, Vector2Int.Zero);
        }

        /// <summary>
        /// Maps the specified <paramref name="dataGrid"/> onto this grid with a specified offset <paramref name="thisOffset"/>.
        /// </summary>
        /// <remarks>
        /// Often used for mapping a small grid (<paramref name="dataGrid"/>) onto a large grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="thisOffset">The offset position on this grid.</param>
        public void MapTo(Grid2D<T> dataGrid, Vector2Int thisOffset)
        {
            MapTo(dataGrid, dataGrid.GridArea(), thisOffset);
        }

        /// <inheritdoc cref="MapTo(Grid2D{T}, Vector2Int)"/>
        public void MapTo(Grid2D<T> dataGrid)
        {
            MapTo(dataGrid, dataGrid.GridArea(), Vector2Int.Zero);
        }

        /// <summary>
        /// Enumerates the mapped area maps by enumerating the specified area <paramref name="mapArea"/> on this grid from the specified <paramref name="dataGrid"/> with offset <paramref name="dataOffset"/>
        /// </summary>
        /// <remarks>
        /// Often used for mapping a large grid (<paramref name="dataGrid"/>) onto a small grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="mapArea">The area on this grid to populate.</param>
        /// <param name="dataOffset">The offset position on the <paramref name="dataGrid"/>.</param>
        public IEnumerable<Vector2Int> EnumerateMapFrom(Grid2D<T> dataGrid, Rect2D mapArea, Vector2Int dataOffset)
        {
            var thisArea = GridArea();
            if (!Rect2D.Overlaps(thisArea, mapArea))
            {
                yield break;
            }

            var dataArea = dataGrid.GridArea();

            var alignedGetArea = mapArea + dataOffset;
            if (!Rect2D.Overlaps(dataArea, alignedGetArea))
            {
                yield break;
            }

            mapArea = dataArea.TrimArea(alignedGetArea) - dataOffset;

            foreach (var pos in EnumerateGrid(mapArea))
            {
                yield return pos;
            }
        }

        /// <summary>
        /// Custom maps by populating the specified area <paramref name="thisArea"/> on this grid from the specified <paramref name="dataGrid"/>.
        /// </summary>
        /// <remarks>
        /// Often used for mapping a large grid (<paramref name="dataGrid"/>) onto a small grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="thisArea">The area on this grid to populate.</param>
        /// <param name="dataOffset">The offset position on the <paramref name="dataGrid"/>.</param>
        public void MapFrom(Grid2D<T> dataGrid, Rect2D thisArea, Vector2Int dataOffset)
        {
            foreach (var pos in EnumerateMapFrom(dataGrid, thisArea, dataOffset))
            {
                this[pos] = dataGrid[pos + dataOffset];
            }
        }

        /// <inheritdoc cref="MapFrom(Grid2D{T}, Rect2D, Vector2Int)"/>
        public void MapFrom(Grid2D<T> dataGrid, Rect2D thisArea)
        {
            MapFrom(dataGrid, thisArea, Vector2Int.Zero);
        }

        /// <summary>
        /// Custom maps by populating this grid from the specified <paramref name="dataGrid"/>.
        /// </summary>
        /// <remarks>
        /// Often used for mapping a large grid (<paramref name="dataGrid"/>) onto a small grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="dataOffset">The offset position on the <paramref name="dataGrid"/>.</param>
        public void MapFrom(Grid2D<T> dataGrid, Vector2Int dataOffset)
        {
            MapFrom(dataGrid, GridArea(), dataOffset);
        }

        /// <inheritdoc cref="MapFrom(Grid2D{T}, Vector2Int)"/>
        public void MapFrom(Grid2D<T> dataGrid)
        {
            MapFrom(dataGrid, GridArea(), Vector2Int.Zero);
        }

        #endregion

        #region Rotation

        public virtual void RotateData90Clockwise()
        {
            RotateData90(1);
        }

        public virtual void RotateData90Anticlockwise()
        {
            RotateData90(-1);
        }

        public virtual void RotateData90(int direction)
        {
            if (Math.Abs(direction) != 1)
            {
                throw new ArgumentException("Rotation factor must be either -1 or 1");
            }

            var newData = new T[Width, Height];

            Vector2 rotationAxis = Dimensions.ToVector2().Midpoint();

            foreach (var pos in this)
            {
                T value = this[pos];

                Vector2 rotatedOffsetPosition = RotationUtils.GetRotatedOffsetPosition(pos, direction, rotationAxis);

                Vector2Int newPixelPos = (rotatedOffsetPosition + rotationAxis.Inverse()).ToVector2Int();

                newData[newPixelPos.X, newPixelPos.Y] = value;
            }

            data = newData;
        }

        #endregion

        #region Scaling

        public Grid2D<T> Scale(int scaleFactor)
        {
            var startGrid = Clone();
            Grid2D<T> newGrid = new(startGrid.Dimensions * scaleFactor);

            foreach (var pos in startGrid)
            {
                Vector2Int start = pos * scaleFactor;

                Rect2D area = new(start, start + scaleFactor);

                T value = startGrid[pos];

                newGrid.Fill(value, area);
            }

            return newGrid;
        }

        #endregion

        #region Resizing

        /// <summary>
        /// Resizes this grid clearing all previous data.
        /// </summary>
        /// <param name="width">The new width of the grid.</param>
        /// <param name="height">The new height of the grid.</param>
        public void CleanResize(int width, int height)
        {
            data = new T[width, height];
            OnResize?.Invoke();
        }

        /// <summary>
        /// Resizes this grid clearing all previous data.
        /// </summary>
        /// <param name="dimensions">The new dimensions of the grid.</param>
        public void CleanResize(Vector2Int dimensions)
        {
            CleanResize(dimensions.X, dimensions.Y);
        }

        /// <summary>
        /// Resizes this grid transfering all possible data over.
        /// </summary>
        /// <param name="width">The new width of the grid.</param>
        /// <param name="height">The new height of the grid.</param>
        public void MapResize(int width, int height)
        {
            var transferGrid = new Grid2D<T>(width, height);

            var thisArea = GridArea();
            var newArea = transferGrid.GridArea();

            var mapArea = newArea > thisArea ? thisArea : newArea;

            transferGrid.MapFrom(this, mapArea);

            data = transferGrid.data;
            OnResize?.Invoke();
        }

        /// <summary>
        /// Resizes this grid transfering all possible data over.
        /// </summary>
        /// <param name="dimensions">The new dimensions of the grid.</param>
        public void MapResize(Vector2Int dimensions)
        {
            MapResize(dimensions.X, dimensions.Y);
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clears the contents of the grid.
        /// </summary>
        public void Clear()
        {
            Array.Clear(data);
            OnClear?.Invoke();
        }

        #endregion

        #region Build

        public string BuildFlat()
        {
            return Utils.BuildGridFlat(data, false);
        }

        public string Build2D()
        {
            return Utils.BuildGrid2D(data, false);
        }

        public override string ToString()
        {
            return BuildFlat();
        }

        #endregion
    }
}
