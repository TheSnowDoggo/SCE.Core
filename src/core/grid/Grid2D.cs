namespace SCE
{
    using CSUtils;
    using System;

    /// <summary>
    /// A wrapper class of a 2D-array representing a grid with useful functions.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the grid.</typeparam>
    public class Grid2D<T> : ICloneable
    {
        private const bool DEFAULT_TRIM = false;

        private T[,] data;

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
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        public Grid2D(int width, int height)
        {
            data = new T[width, height];
            Data = new(this, (pos, val) => val);
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
            Data = new(this, (pos, val) => val);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class.
        /// </summary>
        /// <param name="grid">The default data for the grid.</param>
        public Grid2D(Grid2D<T> grid)
            : this(grid.data)
        {
        }

        public VirtualGrid2D<T, T> Data { get; }

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

        #region PositionValid

        /// <summary>
        /// Determines whether the specified zero-based coordinates lie within this grid.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><see langword="true"/> if the specified coordinates lie within this grid; otherwise, <see langword="false"/>.</returns>
        public bool InRange(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        /// <summary>
        /// Determines whether the specified zero-based coordinates lie within this grid.
        /// </summary>
        /// <param name="pos">The <see cref="Vector2Int"/> coordinate.</param>
        /// <returns><see langword="true"/> if the specified coordinates lie within this grid; otherwise, <see langword="false"/>.</returns>
        public bool InRange(Vector2Int pos)
        {
            return InRange(pos.X, pos.Y);
        }

        #endregion

        #region Cycle

        /// <summary>
        /// Cycles through each position which overlaps the specified <paramref name="area"/> and invokes the specified <paramref name="func"/> in this grid.
        /// </summary>
        /// <remarks>
        /// If the function call returns <see langword="false"/>, cycling will terminate.
        /// </remarks>
        /// <param name="func">The func to invoke at every position.</param>
        /// <param name="area">The area to cycle over.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the area to fit within the bounds of this grid.</param>
        public void GenericCycleArea(Func<Vector2Int, bool> func, Rect2D area, bool trimOnOverflow = DEFAULT_TRIM)
        {
            var thisArea = GridArea(); 
            if (!Rect2D.Overlaps(thisArea, area))
                throw new InvalidAreaException();
            if (!trimOnOverflow && !thisArea.Contains(area))
                throw new AreaOutOfBoundsException();

            var newArea = thisArea.TrimArea(area);
            for (int y = newArea.Bottom; y < newArea.Top; ++y)
            {
                for (int x = newArea.Left; x < newArea.Right; ++x)
                {
                    if (!func.Invoke(new Vector2Int(x, y)))
                        return;
                }
            }
        }

        /// <summary>
        /// Cycles through each position which overlaps the specified <paramref name="area"/> and invokes the specified <paramref name="action"/> in this grid.
        /// </summary>
        /// <param name="action">The action to invoke at every position.</param>
        /// <param name="area">The area to cycle over.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the area to fit within the bounds of this grid.</param>
        public void GenericCycleArea(Action<Vector2Int> action, Rect2D area, bool trimOnOverflow = DEFAULT_TRIM)
        {
            GenericCycleArea(ToCycleFunc(action), area, trimOnOverflow);
        }

        /// <summary>
        /// Cycles through each position and invokes the specified <paramref name="func"/> in this grid.
        /// </summary>
        /// <remarks>
        /// If the function call returns <see langword="false"/>, cycling will terminate.
        /// </remarks>
        /// <param name="func">The func to invoke at every position.</param>
        public void GenericCycle(Func<Vector2Int, bool> func)
        {
            GenericCycleArea(func, GridArea());
        }

        /// <summary>
        /// Cycles through each position and invokes the specified <paramref name="action"/> in this grid.
        /// </summary>
        /// <param name="action">The action to invoke at every position.</param>
        public void GenericCycle(Action<Vector2Int> action)
        {
            GenericCycle(ToCycleFunc(action));
        }

        private static Func<Vector2Int, bool> ToCycleFunc(Action<Vector2Int> action)
        {
            return (pos) =>
            {
                action.Invoke(pos);
                return true;
            };
        }

        #endregion

        #region Mapping

        /// <summary>
        /// Custom maps the area <paramref name="dataGridArea"/> of the specified <paramref name="dataGrid"/> onto this grid with a specified offset <paramref name="thisOffset"/>
        /// </summary>
        /// <remarks>
        /// Often used for mapping a small grid (<paramref name="dataGrid"/>) onto a large grid (this grid).
        /// </remarks>
        /// <param name="action">The map action to invoke.</param>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="dataGridArea">The area on the <paramref name="dataGrid"/> to get elements from.</param>
        /// <param name="thisOffset">The offset position on this grid.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the area to fit within the bounds of this grid.</param>
        public void CustomMapToArea(Action<Vector2Int> action, Grid2D<T> dataGrid, Rect2D dataGridArea, Vector2Int thisOffset, bool trimOnOverflow = DEFAULT_TRIM)
        {
            var dataArea = dataGrid.GridArea();
            if (!Rect2D.Overlaps(dataArea, dataGridArea))
                throw new InvalidAreaException("Given get area doesn't overlap the get grid.");
            if (!trimOnOverflow && !dataArea.Contains(dataGridArea))
                throw new AreaOutOfBoundsException("Given get area is outside of the bounds of the get grid.");

            var thisArea = GridArea();

            var offsetArea = dataGridArea + thisOffset;
            if (!Rect2D.Overlaps(thisArea, offsetArea))
                throw new AreaOutOfBoundsException("Offset area doesn't overlap this grid.");

            dataGridArea = thisArea.TrimArea(offsetArea) - thisOffset;

            dataGrid.GenericCycleArea(action, dataGridArea, trimOnOverflow);
        }

        /// <summary>
        /// Maps the area <paramref name="dataGridArea"/> of the specified <paramref name="dataGrid"/> onto this grid with a specified offset <paramref name="thisOffset"/>
        /// </summary>
        /// <remarks>
        /// Often used for mapping a small grid (<paramref name="dataGrid"/>) onto a large grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="dataGridArea">The area on the <paramref name="dataGrid"/> to get elements from.</param>
        /// <param name="thisOffset">The offset position on this grid.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the area to fit within the bounds of this grid.</param>
        public void MapToArea(Grid2D<T> dataGrid, Rect2D dataGridArea, Vector2Int? thisOffset = null, bool trimOnOverflow = DEFAULT_TRIM)
        {
            var validThisOffset = thisOffset ?? Vector2Int.Zero;
            CustomMapToArea((Vector2Int pos) => this[pos + validThisOffset] = dataGrid[pos], dataGrid, dataGridArea, validThisOffset, trimOnOverflow);
        }

        /// <summary>
        /// Maps the specified <paramref name="dataGrid"/> onto this grid with a specified offset <paramref name="thisOffset"/>
        /// </summary>
        /// <remarks>
        /// Often used for mapping a small grid (<paramref name="dataGrid"/>) onto a large grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="thisOffset">The offset position on this grid.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the area to fit within the bounds of this grid.</param>
        public void MapTo(Grid2D<T> dataGrid, Vector2Int? thisOffset = null, bool trimOnOverflow = DEFAULT_TRIM)
        {
            MapToArea(dataGrid, dataGrid.GridArea(), thisOffset, trimOnOverflow);
        }

        /// <summary>
        /// Custom maps by populating the specified area <paramref name="mapArea"/> on this grid from the specified <paramref name="dataGrid"/> with offset <paramref name="dataOffset"/>
        /// </summary>
        /// <remarks>
        /// Often used for mapping a large grid (<paramref name="dataGrid"/>) onto a small grid (this grid).
        /// </remarks>
        /// <param name="action">The map action to invoke.</param>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="mapArea">The area on this grid to populate.</param>
        /// <param name="dataOffset">The offset position on the <paramref name="dataGrid"/>.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the area to fit within the bounds of this grid.</param>
        public void CustomMapAreaFrom(Action<Vector2Int> action, Grid2D<T> dataGrid, Rect2D mapArea, Vector2Int dataOffset, bool trimOnOverflow = DEFAULT_TRIM)
        {
            var thisArea = GridArea();
            if (!Rect2D.Overlaps(thisArea, mapArea))
                throw new InvalidAreaException("Given set area doesn't overlap with this grid.");
            if (!trimOnOverflow && !thisArea.Contains(mapArea))
                throw new AreaOutOfBoundsException("Given set area is outside of the bounds of this grid.");

            var dataArea = dataGrid.GridArea();

            var alignedGetArea = mapArea + dataOffset;
            if (!Rect2D.Overlaps(dataArea, alignedGetArea))
                throw new InvalidAreaException("Offset area doesn't overlap with the get grid.");

            mapArea = dataArea.TrimArea(alignedGetArea) - dataOffset;

            GenericCycleArea(action, mapArea, trimOnOverflow);
        }

        /// <summary>
        /// Custom maps by populating the specified area <paramref name="thisArea"/> on this grid from the specified <paramref name="dataGrid"/> with offset <paramref name="dataOffset"/>
        /// </summary>
        /// <remarks>
        /// Often used for mapping a large grid (<paramref name="dataGrid"/>) onto a small grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="thisArea">The area on this grid to populate.</param>
        /// <param name="dataOffset">The offset position on the <paramref name="dataGrid"/>.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the area to fit within the bounds of this grid.</param>
        public void MapAreaFrom(Grid2D<T> dataGrid, Rect2D thisArea, Vector2Int? dataOffset = null, bool trimOnOverflow = DEFAULT_TRIM)
        {
            var validGetOffset = dataOffset ?? Vector2Int.Zero;
            CustomMapAreaFrom((Vector2Int pos) => this[pos] = dataGrid[pos + validGetOffset], dataGrid, thisArea, validGetOffset, trimOnOverflow);
        }

        /// <summary>
        /// Custom maps by populating this grid from the specified <paramref name="dataGrid"/> with offset <paramref name="dataOffset"/>
        /// </summary>
        /// <remarks>
        /// Often used for mapping a large grid (<paramref name="dataGrid"/>) onto a small grid (this grid).
        /// </remarks>
        /// <param name="dataGrid">The grid to get elements from.</param>
        /// <param name="dataOffset">The offset position on the <paramref name="dataGrid"/>.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the area to fit within the bounds of this grid.</param>
        public void MapFrom(Grid2D<T> dataGrid, Vector2Int? dataOffset = null, bool trimOnOverflow = DEFAULT_TRIM)
        {
            MapAreaFrom(dataGrid, GridArea(), dataOffset, trimOnOverflow);
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
                throw new ArgumentException("Rotation factor must be either -1 or 1");

            var newData = new T[Width, Height];

            Vector2 rotationAxis = Dimensions.ToVector2().Midpoint();

            void CycleAction(Vector2Int position)
            {
                T value = this[position];

                Vector2 rotatedOffsetPosition = RotationUtils.GetRotatedOffsetPosition(position, direction, rotationAxis);

                Vector2Int newPixelPos = (rotatedOffsetPosition + rotationAxis.Inverse()).ToVector2Int();

                newData[newPixelPos.X, newPixelPos.Y] = value;
            }

            GenericCycle(CycleAction);

            data = newData;
        }

        #endregion

        #region Scaling

        public Grid2D<T> Scale(int scaleFactor)
        {
            var startGrid = Clone();
            Grid2D<T> newGrid = new(startGrid.Dimensions * scaleFactor);

            startGrid.GenericCycle(pos =>
            {
                Vector2Int start = pos * scaleFactor;

                Rect2D area = new(start, start + scaleFactor);

                T value = startGrid[pos];

                newGrid.Data.FillArea(value, area);
            });

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

            transferGrid.MapAreaFrom(this, mapArea);

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
