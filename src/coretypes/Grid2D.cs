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

        public Grid2D(MapView<T> mapView)
            : this(mapView.Dimensions)
        {
            Fill(pos => mapView[pos]);
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
        /// Gets the zero-based area of the grid as a <see cref="Rect2DInt"/>.
        /// </summary>
        public Rect2DInt GridArea()
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

        public static implicit operator Grid2DView<T>(Grid2D<T> grid) => grid.ToView();

        public virtual Grid2DView<T> ToView()
        {
            return new(this);
        }

        #region Equality

        public static bool ValueEquals<U>(MapView<U> g1, MapView<U> g2)
            where U : IEquatable<U>
        {
            if (g1.Dimensions != g2.Dimensions)
            {
                return false;
            }
            foreach (var pos in g1)
            {
                if (!g1[pos].Equals(g2[pos]))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

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

        public IEnumerator<Vector2Int> GetEnumerator()
        {
            return GridArea().GetEnumerator();
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
        public void Fill(Func<Vector2Int, T> fill, Rect2DInt area)
        {
            foreach (var pos in GridArea().Enumerate(area))
            {
                this[pos] = fill.Invoke(pos);
            }
        }

        /// <summary>
        /// Fills the grid over a specified area <paramref name="area"/>.
        /// </summary>
        /// <param name="value">The item to fill with.</param>
        /// <param name="area">The area to fill over.</param>
        public void Fill(T value, Rect2DInt area)
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
        public IEnumerable<Vector2Int> EnumerateMapTo(MapView<T> dataGrid, Rect2DInt dataGridArea, Vector2Int thisOffset)
        {
            var dataArea = dataGrid.GridArea();
            if (!Rect2DInt.Overlaps(dataArea, dataGridArea))
            {
                yield break;
            }

            var thisArea = GridArea();

            var offsetArea = dataGridArea + thisOffset;
            if (!Rect2DInt.Overlaps(thisArea, offsetArea))
            {
                yield break;
            }

            dataGridArea = thisArea.GetOverlap(offsetArea) - thisOffset;

            foreach (var pos in dataArea.Enumerate(dataGridArea))
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
        public void MapTo(MapView<T> dataGrid, Rect2DInt dataGridArea, Vector2Int thisOffset)
        {
            foreach (var pos in EnumerateMapTo(dataGrid, dataGridArea, thisOffset))
            {
                this[pos + thisOffset] = dataGrid[pos];
            }
        }

        /// <inheritdoc cref="MapTo(MapView{T}, Rect2DInt, Vector2Int)"/>
        public void MapTo(MapView<T> dataGrid, Rect2DInt dataGridArea)
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
        public void MapTo(MapView<T> dataGrid, Vector2Int thisOffset)
        {
            MapTo(dataGrid, dataGrid.GridArea(), thisOffset);
        }

        /// <inheritdoc cref="MapTo(MapView{T}, Vector2Int)"/>
        public void MapTo(MapView<T> dataGrid)
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
        public IEnumerable<Vector2Int> EnumerateMapFrom(MapView<T> dataGrid, Rect2DInt mapArea, Vector2Int dataOffset)
        {
            var thisArea = GridArea();
            if (!Rect2DInt.Overlaps(thisArea, mapArea))
            {
                yield break;
            }

            var dataArea = dataGrid.GridArea();

            var alignedGetArea = mapArea + dataOffset;
            if (!Rect2DInt.Overlaps(dataArea, alignedGetArea))
            {
                yield break;
            }

            mapArea = dataArea.GetOverlap(alignedGetArea) - dataOffset;
            foreach (var pos in thisArea.Enumerate(mapArea))
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
        public void MapFrom(MapView<T> dataGrid, Rect2DInt thisArea, Vector2Int dataOffset)
        {
            foreach (var pos in EnumerateMapFrom(dataGrid, thisArea, dataOffset))
            {
                this[pos] = dataGrid[pos + dataOffset];
            }
        }

        /// <inheritdoc cref="MapFrom(MapView{T}, Rect2DInt, Vector2Int)"/>
        public void MapFrom(MapView<T> dataGrid, Rect2DInt thisArea)
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
        public void MapFrom(MapView<T> dataGrid, Vector2Int dataOffset)
        {
            MapFrom(dataGrid, GridArea(), dataOffset);
        }

        /// <inheritdoc cref="MapFrom(MapView{T}, Vector2Int)"/>
        public void MapFrom(MapView<T> dataGrid)
        {
            MapFrom(dataGrid, GridArea(), Vector2Int.Zero);
        }

        #endregion

        #region Rotation

        public void Rotate90(bool clockwise = true)
        {
            var newData = new T[Height, Width];

            float val = (Math.Max(Width, Height) - 1) / 2.0f;

            Vector2 axis = new(val, val);

            var fix = Vector2.Zero;
            if (Width > Height)
            {
                fix = new(0, (Width - Height) / 2.0f);
            }
            else if (Width < Height)
            {
                fix = new((Height - Width) / 2.0f, 0);
            }

            foreach (var pos in this)
            {
                var rot = clockwise ? RotateUtils.Rotate90CW(pos + fix, axis) :
                    RotateUtils.Rotate90ACW(pos + fix, axis);

                var next = (Vector2Int)(rot - fix.Inverse());

                newData[next.X, next.Y] = this[pos];
            }

            data = newData;
        }

        public void Rotate90CW()
        {
            Rotate90(true);
        }

        public void Rotate90ACW()
        {
            Rotate90(false);
        }

        public void Rotate180()
        {
            var newData = new T[Width, Height];

            var mid = (Dimensions - 1).ToVector2().Midpoint();

            foreach (var pos in this)
            {
                var next = (Vector2Int)RotateUtils.Rotate180(pos, mid);

                newData[next.X, next.Y] = this[pos];
            }

            data = newData;
        }

        #endregion

        #region Flipping

        public void FlipHorizontal()
        {
            for (int x = 0; x < Width / 2; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Utils.Swap(ref data[x, y], ref data[Width - x - 1, y]);
                }
            }
        }

        public void FlipVertical()
        {
            for (int y = 0; y < Height / 2; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Utils.Swap(ref data[x, y], ref data[x, Height - y - 1]);
                }
            }
        }

        #endregion

        #region Scaling

        public void Upscale(Vector2Int scaleFactor)
        {
            if (scaleFactor <= Vector2.Zero)
            {
                throw new ArgumentException("Scale factor must be greater than 0,0.");
            }

            Grid2D<T> newGrid = new(Dimensions * scaleFactor);

            foreach (var pos in this)
            {
                Vector2Int start = pos * scaleFactor;

                Rect2DInt area = new(start, start + scaleFactor);

                newGrid.Fill(this[pos], area);
            }

            data = newGrid.data;
        }

        public void Upscale(int scaleFactor)
        {
            Upscale(new Vector2Int(scaleFactor, scaleFactor));
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
            if (width < 0 || height < 0)
            {
                throw new ArgumentException("Dimensions cannot be less than 0.");
            }

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
            if (width < 0 || height < 0)
            {
                throw new ArgumentException("Dimensions cannot be less than 0.");
            }

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
