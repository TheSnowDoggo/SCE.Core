namespace SCE
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A wrapper class of a 2D-array with additional features.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the grid.</typeparam>
    public class Grid2D<T> : IEquatable<Grid2D<T>>, ICloneable
    {
        private const bool DefaultTryTrimOnOverflowState = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class given its initial width and height.
        /// </summary>
        /// <param name="width">The initial width of the new grid.</param>
        /// <param name="height">The initial height of the new grid.</param>
        public Grid2D(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new InvalidDimensionsException("Dimensions are invalid.");
            Data = new T[width, height];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class given its initial dimensions.
        /// </summary>
        /// <param name="dimensions">The initial dimensions of the new grid.</param>
        public Grid2D(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class given its initial data.
        /// </summary>
        /// <param name="data">The initial data of the new grid.</param>
        public Grid2D(T[,] data)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid2D{T}"/> class given its initial data.
        /// </summary>
        /// <param name="grid">The <see cref="Grid2D{T}"/> to get the initial data of the new grid.</param>
        public Grid2D(Grid2D<T> grid)
            : this(grid.Data)
        {
        }

        /// <summary>
        /// A delegate type used for cycling element positions in <see cref="Grid2D{T}"/> while <see langword="true"/>.
        /// </summary>
        /// <param name="position">The current element position.</param>
        /// <returns><see langword="true"/> to keep cycling; otherwise, if <see langword="false"/> cycling will stop.</returns>
        public delegate bool CycleActionWhile(Vector2Int position);

        /// <summary>
        /// A delegate type used for cycling element positions in <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="position">The current element position.</param>
        public delegate void CycleAction(Vector2Int position);

        /// <summary>
        /// Gets or sets the underlying 2D-array of this instance.
        /// </summary>
        protected T[,] Data { get; set; }

        /// <summary>
        /// Gets or sets a delegate called whenever this instance has been resized.
        /// </summary>
        public Action? OnResize { get; set; }

        /// <summary>
        /// Gets or sets a delegate called whenever this instance has been cleared.
        /// </summary>
        public Action? OnClear { get; set; }

        /// <summary>
        /// Gets the current <see cref="int"/> width of the grid.
        /// </summary>
        public int Width { get => Data.GetLength(0); }

        /// <summary>
        /// Gets the current <see cref="int"/> height of the grid.
        /// </summary>
        public int Height { get => Data.GetLength(1); }

        /// <summary>
        /// Gets the current <see cref="Vector2"/> dimensions of the grid.
        /// </summary>
        public Vector2Int Dimensions { get => new(Width, Height); }

        /// <summary>
        /// Gets the current <see cref="Area2D"/> area of the grid.
        /// </summary>
        public Area2DInt GridArea { get => new(Vector2Int.Zero, Dimensions); }

        /// <summary>
        /// Gets the current number of elements in the grid.
        /// </summary>
        public int Elements { get => Width * Height; }

        /// <summary>
        /// Gets or sets the element at a specified position.
        /// </summary>
        /// <param name="x">The zero-based x-coordinate of the element to get or set.</param>
        /// <param name="y">The zero-based y-coordinate of the element to get or set.</param>
        /// <returns>The element at the specified position.</returns>
        public T this[int x, int y]
        {
            get
            {
                if (!PositionValid(x, y))
                    throw new PositionOutOfBoundsException("Position is invalid.");
                return Data[x, y]; 
            }
            set
            {
                if (!PositionValid(x, y))
                    throw new PositionOutOfBoundsException("Position is invalid.");
                Data[x, y] = value;
            }
        }

        /// <summary>
        /// Gets or sets the element at a specified position.
        /// </summary>
        /// <param name="pos">The zero-based coordinate of the element to get or set.</param>
        /// <returns>The element at the specified position.</returns>
        public T this[Vector2Int pos]
        {
            get
            {
                if (!PositionValid(pos))
                    throw new PositionOutOfBoundsException("Position is invalid.");
                return Data[pos.X, pos.Y];
            }
            set
            {
                if (!PositionValid(pos))
                    throw new PositionOutOfBoundsException("Position is invalid.");
                Data[pos.X, pos.Y] = value;
            }
        }

        /// <summary>
        /// Converts all data in the <see cref="Grid2D{T}"/> if it implements <see cref="IConvertible"/>.
        /// </summary>
        /// <typeparam name="T2">The data type of this <see cref="Grid2D{T}"/> which implements <see cref="IConvertible"/>.</typeparam>
        /// <returns>A new <see cref="string"/> <see cref="Grid2D{T}"/> of all the converted data.</returns>
        public static Grid2D<string> ToStringGrid<T2>(Grid2D<T2> grid)
            where T2 : IConvertible
        {
            Grid2D<string> stringGrid = new(grid.Dimensions);

            void CycleAction(Vector2Int pos)
            {
                stringGrid[pos] = grid[pos].ToString() ?? string.Empty;
            }

            grid.GenericCycle(CycleAction);

            return stringGrid;
        }

        /// <summary>
        /// Creates a shallow copy of this instance.
        /// </summary>
        /// <returns>A shallow copy of this instance.</returns>
        public virtual Grid2D<T> Clone()
        {
            return new((T[,])Data.Clone());
        }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <inheritdoc/>
        public bool Equals(Grid2D<T>? other)
        {
            if (other is null)
                return false;
            return other.OnResize == OnResize && ((IStructuralEquatable)Data).Equals(other.Data, EqualityComparer<T>.Default);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Grid2D<T> grid2D && Equals(grid2D);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Dimensions.ToString();
        }

        /// <summary>
        /// Indicates whether the given zero-based position is valid in the <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="x">The x-coordinate of the position.</param>
        /// <param name="y">The y-coordinate of the position.</param>
        /// <returns><see langword="true"/> if the given <paramref name="x"/>, <paramref name="y"/> position is valid in the <see cref="Grid2D{T}"/>; otherwise, <see langword="false"/>.</returns>
        public bool PositionValid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        /// <summary>
        /// Indicates whether the given zero-based <see cref="Vector2"/> position is valid in the <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="position">The coordinates to check.</param>
        /// <returns><see langword="true"/> if the given <paramref name="position"/> is valid in the <see cref="Grid2D{T}"/>; otherwise, <see langword="false"/>.</returns>
        public bool PositionValid(Vector2Int position)
        {
            return PositionValid(position.X, position.Y);
        }

        /// <summary>
        /// Indicates whether the given <see cref="Area2D"/> is valid in the <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="area">The area to check.</param>
        /// <returns><see langword="true"/> if the given <paramref name="area"/> is valid in the <see cref="Grid2D{T}"/>; otherwise, <see langword="false"/>.</returns>
        public bool AreaValid(Area2DInt area)
        {
            return GridArea.Contains(area);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based position of the first occurance within the the <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <remarks>
        /// <i>Note: Searching is done from the bottom-left to the top-right in rows.</i>
        /// </remarks>
        /// <param name="item">The object to locate in this grid.</param>
        /// <returns>The zero-based position of the first occurance of <paramref name="item"/> within the entire <see cref="Grid2D{T}"/>, if found; otherwise, <see cref="Vector2"/>(-1,-1).</returns>
        public Vector2Int PositionOf(T item)
        {
            Contains(item, out Vector2Int position);

            return position;
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="Grid2D{T}"/> and outputs its position if found.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="Grid2D{T}"/>.</param>
        /// <param name="position">The zero-based position of the first occurance of <paramref name="item"/> within the entire <see cref="Grid2D{T}"/>, if found; otherwise, <see cref="Vector2"/>(-1,-1).</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="Grid2D{T}"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item, out Vector2Int position)
        {
            Vector2Int foundPosition = new(-1, -1);
            bool found = false;

            bool CycleActionWhile(Vector2Int elementPos)
            {
                if (Comparer<T>.Default.Compare(this[elementPos], item) == 0)
                {
                    foundPosition = elementPos;
                    found = true;
                }

                return !found;
            }

            GenericCycle(CycleActionWhile);

            position = foundPosition;
            return found;
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="Grid2D{T}"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="Grid2D{T}"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item)
        {
            return Contains(item, out _);
        }

        /// <summary>
        /// Invokes the <see cref="CycleActionWhile"/> delegate at every position in a defined <see cref="Area2D"/> in this <see cref="Grid2D{T}"/> until <paramref name="cycleActionWhile"/> returns <see langword="false"/>.
        /// </summary>
        /// <param name="cycleActionWhile">The <see cref="CycleActionWhile"/> delegate invoked at every element position.</param>
        /// <param name="area">The area in the <see cref="Grid2D{T}"/> to cycle.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the given <paramref name="area"/> is out of bounds but valid, try trim it to fit onto the <see cref="Grid2D{T}"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        /// <exception cref="InvalidAreaException">Thrown when the given <paramref name="area"/> contains non-integer values.</exception>
        /// <exception cref="AreaOutOfBoundsException">Thrown if <paramref name="tryTrimOnOverflow"/> is <see langword="false"/> and an overflow is found.</exception>
        public void GenericCycleArea(CycleActionWhile cycleActionWhile, Area2DInt area, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            if (!Area2DInt.Overlaps(GridArea, area))
            {
                throw new InvalidAreaException("Given area doesn't overlap this grid.");
            }

            if (!tryTrimOnOverflow && !AreaValid(area))
            {
                throw new AreaOutOfBoundsException("Given area is outside of the bounds of the grid.");
            }

            Area2DInt newArea = GridArea.TrimArea(area);

            newArea.Expose(out Vector2Int start, out Vector2Int end);

            for (int x = start.X; x < end.X; x++)
            {
                for (int y = start.Y; y < end.Y; y++)
                {
                    if (!cycleActionWhile.Invoke(new(x, y)))
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Invokes the <see cref="CycleAction"/> delegate at every position in a defined <see cref="Area2D"/> in this <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="cycleAction">The <see cref="CycleAction"/> delegate invoked at every element position.</param>
        /// <param name="area">The area in the <see cref="Grid2D{T}"/> to cycle.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the given <paramref name="area"/> is out of bounds but valid, try trim it to fit onto the <see cref="Grid2D{T}"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        public void GenericCycleArea(CycleAction cycleAction, Area2DInt area, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            bool CycleActionValid(Vector2Int pos)
            {
                cycleAction(pos);
                return true;
            }

            GenericCycleArea(CycleActionValid, area, tryTrimOnOverflow);
        }

        /// <summary>
        /// Invokes the <see cref="CycleActionWhile"/> delegate at every position in a defined <see cref="Area2D"/> in this <see cref="Grid2D{T}"/> until <paramref name="cycleActionWhile"/> returns <see langword="false"/>.
        /// </summary>
        /// <param name="cycleActionWhile">The <see cref="CycleActionWhile"/> delegate invoked at every element position.</param>
        public void GenericCycle(CycleActionWhile cycleActionWhile)
        {
            GenericCycleArea(cycleActionWhile, GridArea);
        }

        /// <summary>
        /// Invokes the <see cref="CycleAction"/> delegate at every position in a defined <see cref="Area2D"/> in this <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <param name="cycleAction">The <see cref="CycleAction"/> delegate invoked at every element position.</param>
        public void GenericCycle(CycleAction cycleAction)
        {
            bool CycleActionValid(Vector2Int position)
            {
                cycleAction(position);
                return true;
            }

            GenericCycle(CycleActionValid);
        }

        /// <summary>
        /// Replaces every element in the <see cref="Grid2D{T}"/> in a given <see cref="Area2D"/> with given item.
        /// </summary>
        /// <param name="item">The object to fill the <see cref="Grid2D{T}"/> with.</param>
        /// <param name="area">The area in the <see cref="Grid2D{T}"/> to fill.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the final offset <see cref="Area2D"/> is out of bounds but valid, try trim it to fit onto the <paramref name="dataGrid"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        public void FillArea(T item, Area2DInt area, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            GenericCycleArea((Vector2Int pos) => this[pos] = item, area, tryTrimOnOverflow);
        }

        /// <summary>
        /// Replaces every element in the <see cref="Grid2D{T}"/> with given item.
        /// </summary>
        /// <param name="item">The object to fill the <see cref="Grid2D{T}"/> with.</param>
        public void Fill(T item) => GenericCycle((Vector2Int pos) => this[pos] = item);

        /// <summary>
        /// Replaces every element in the <see cref="Grid2D{T}"/> in a given horizontal line <paramref name="y"/> and range in this instance with given <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The object to fill the <see cref="Grid2D{T}"/> with.</param>
        /// <param name="y">The constant y-coordinate of the line.</param>
        /// <param name="range">The y-range of the line to fill.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the final offset <see cref="Area2D"/> is out of bounds but valid, try trim it to fit onto the <paramref name="dataGrid"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        public void FillHorizontalArea(T item, int y, Vector2Int range, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            FillArea(item, new Area2DInt(new(range.X, y), new(range.Y, y + 1)), tryTrimOnOverflow);
        }

        /// <summary>
        /// Replaces every element in the <see cref="Grid2D{T}"/> in a given vertical line <paramref name="x"/> and range in this instance with given <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The object to fill the <see cref="Grid2D{T}"/> with.</param>
        /// <param name="x">The constant x-coordinate of the line.</param>
        /// <param name="range">The y-range of the line to fill.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the final offset <see cref="Area2D"/> is out of bounds but valid, try trim it to fit onto the <paramref name="dataGrid"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        public void FillVerticalArea(T item, int x, Vector2Int range, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            FillArea(item, new Area2DInt(new(x, range.X), new(x + 1, range.Y)), tryTrimOnOverflow);
        }

        /// <summary>
        /// Replaces every element in the <see cref="Grid2D{T}"/> in a given horizontal line <paramref name="y"/> with given <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The object to fill the <see cref="Grid2D{T}"/> with.</param>
        /// <param name="y">The constant y-coordinate of the line.</param>
        public void FillHorizontal(T item, int y)
        {
            FillHorizontalArea(item, y, new(0, Width));
        }

        /// <summary>
        /// Replaces every element in the <see cref="DisplayMap"/> in a given vertical line <paramref name="x"/> with given <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The object to fill the <see cref="Grid2D{T}"/> with.</param>
        /// <param name="x">The constant x-coordinate of the line.</param>
        public void FillVertical(T item, int x)
        {
            FillVerticalArea(item, x, new(0, Height));
        }

        /// <summary>
        /// Invokes the <see cref="CycleAction"/> delegate at every position in the defined <paramref name="dataGridArea"/> in the <paramref name="dataGrid"/> which offset by <paramref name="positionOffset"/> will overlap this <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <remarks>
        /// <i>Note: Used for custom MapToArea functions, hence it doesn't map any elements, only calling the <paramref name="cycleAction"/>.</i>
        /// </remarks>
        /// <param name="cycleAction">The <see cref="CycleAction"/> delegate invoked at every position.</param>
        /// <param name="dataGrid">The <see cref="Grid2D{T}"/> who's values are taken and mapped onto this <see cref="Grid2D{T}"/>.</param>
        /// <param name="dataGridArea">The area of the values of <paramref name="dataGrid"/> to take and map.</param>
        /// <param name="positionOffset">The offset of the positions of the values to map onto this <see cref="Grid2D{T}"/>.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the final offset <see cref="Area2D"/> is out of bounds but valid, try trim it to fit onto this <see cref="Grid2D{T}"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        /// <exception cref="InvalidPositionException">Thrown if the given offset position has non-integer values.</exception>
        /// <exception cref="InvalidAreaException">Thrown if an <see cref="Area2D"/> is invalid.</exception>
        /// <exception cref="AreaOutOfBoundsException">Thrown if an <see cref="Area2D"/> is outside of the bounds of its associated <see cref="Grid2D{T}"/>.</exception>
        public void CustomMapToArea(CycleAction cycleAction, Grid2D<T> dataGrid, Area2DInt dataGridArea, Vector2Int positionOffset, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            if (!Area2DInt.Overlaps(dataGrid.GridArea, dataGridArea))
            {
                throw new InvalidAreaException("Given get area doesn't overlap the get grid.");
            }

            if (!tryTrimOnOverflow && !dataGrid.AreaValid(dataGridArea))
            {
                throw new AreaOutOfBoundsException("Given get area is outside of the bounds of the get grid.");
            }

            Area2DInt offsetArea = dataGridArea + positionOffset;

            if (!Area2DInt.Overlaps(GridArea, offsetArea))
            {
                throw new AreaOutOfBoundsException("Offset area doesn't overlap this grid.");
            }

            dataGridArea = GridArea.TrimArea(offsetArea, out bool hasFixed) - positionOffset;

            dataGrid.GenericCycleArea(cycleAction, dataGridArea, tryTrimOnOverflow);
        }

        /// <summary>
        /// Maps every element in the defined <paramref name="dataGridArea"/> in the <paramref name="dataGrid"/> which offset by <paramref name="positionOffset"/> will overlap this <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <remarks>
        /// <i>Note: Data is always mapped from <paramref name="dataGrid"/> into this <see cref="Grid2D{T}"/>.</i>
        /// </remarks>
        /// <param name="dataGrid">The <see cref="Grid2D{T}"/> who's values are taken and mapped onto this <see cref="Grid2D{T}"/>.</param>
        /// <param name="dataGridArea">The area of the values of <paramref name="dataGrid"/> to take and map.</param>
        /// <param name="positionOffset">The offset of the positions of the values to map onto this <see cref="Grid2D{T}"/>.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the final offset <see cref="Area2D"/> is out of bounds but valid, try trim it to fit onto this <see cref="Grid2D{T}"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        /// <exception cref="InvalidPositionException">Thrown if the given offset position has non-integer values.</exception>
        /// <exception cref="InvalidAreaException">Thrown if an <see cref="Area2D"/> is invalid.</exception>
        /// <exception cref="AreaOutOfBoundsException">Thrown if an <see cref="Area2D"/> is outside of the bounds of its associated <see cref="Grid2D{T}"/>.</exception>
        public virtual void MapToArea(Grid2D<T> dataGrid, Area2DInt dataGridArea, Vector2Int? positionOffset = null, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            Vector2Int validThisOffset = positionOffset ?? Vector2Int.Zero;

            CustomMapToArea((Vector2Int pos) => this[pos + validThisOffset] = dataGrid[pos], dataGrid, dataGridArea, validThisOffset, tryTrimOnOverflow);
        }

        /// <summary>
        /// Maps every element in the <paramref name="dataGrid"/> which offset by <paramref name="positionOffset"/> will overlap this <see cref="Grid2D{T}"/>.
        /// </summary>
        /// <remarks>
        /// <i>Note: Data is always mapped from <paramref name="dataGrid"/> into this <see cref="Grid2D{T}"/>.</i>
        /// </remarks>
        /// <param name="dataGrid">The <see cref="Grid2D{T}"/> who's values are taken and mapped onto this <see cref="Grid2D{T}"/>.</param>
        /// <param name="positionOffset">The offset of the positions of the values to map onto this <see cref="Grid2D{T}"/>.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the final offset <see cref="Area2D"/> is out of bounds but valid, try trim it to fit onto this <see cref="Grid2D{T}"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        /// <exception cref="InvalidPositionException">Thrown if the given offset position has non-integer values.</exception>
        /// <exception cref="InvalidAreaException">Thrown if an <see cref="Area2D"/> is invalid.</exception>
        /// <exception cref="AreaOutOfBoundsException">Thrown if an <see cref="Area2D"/> is outside of the bounds of its associated <see cref="Grid2D{T}"/>.</exception>
        public virtual void MapTo(Grid2D<T> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            MapToArea(dataGrid, dataGrid.GridArea, positionOffset, tryTrimOnOverflow);
        }

        /// <summary>
        /// Invokes the <see cref="CycleAction"/> delegate at every position in the defined <paramref name="thisArea"/> in this <see cref="Grid2D{T}"/> which offset by <paramref name="positionOffset"/> will overlap the <paramref name="dataGrid"/>.
        /// </summary>
        /// <remarks>
        /// <i>Note: Used for custom MapAreaFrom functions, hence it doesn't map any elements, only calling the <paramref name="cycleAction"/>.</i>
        /// </remarks>
        /// <param name="cycleAction">The <see cref="CycleAction"/> delegate invoked at every position.</param>
        /// <param name="dataGrid">The <see cref="Grid2D{T}"/> who's values are taken and mapped onto this <see cref="Grid2D{T}"/>.</param>
        /// <param name="thisArea">The area of the values of this <see cref="Grid2D{T}"/> to map onto.</param>
        /// <param name="positionOffset">The offset of the elements positions to map from <paramref name="dataGrid"/>.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the final offset <see cref="Area2D"/> is out of bounds but valid, try trim it to fit onto the <paramref name="dataGrid"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        /// <exception cref="InvalidPositionException">Thrown if the given offset position has non-integer values.</exception>
        /// <exception cref="InvalidAreaException">Thrown if an <see cref="Area2D"/> is invalid.</exception>
        /// <exception cref="AreaOutOfBoundsException">Thrown if an <see cref="Area2D"/> is outside of the bounds of its associated <see cref="Grid2D{T}"/>.</exception>
        public void CustomMapAreaFrom(CycleAction cycleAction, Grid2D<T> dataGrid, Area2DInt thisArea, Vector2Int positionOffset, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            if (!Area2DInt.Overlaps(GridArea, thisArea))
            {
                throw new InvalidAreaException("Given set area doesn't overlap with this grid.");
            }

            if (!tryTrimOnOverflow && !AreaValid(thisArea))
            {
                throw new AreaOutOfBoundsException("Given set area is outside of the bounds of this grid.");
            }

            Area2DInt alignedGetArea = thisArea + positionOffset;

            if (!Area2DInt.Overlaps(dataGrid.GridArea, alignedGetArea))
            {
                throw new InvalidAreaException("Offset area doesn't overlap with the get grid.");
            }

            thisArea = dataGrid.GridArea.TrimArea(alignedGetArea, out bool hasFixed) - positionOffset;

            GenericCycleArea(cycleAction, thisArea, tryTrimOnOverflow);
        }

        /// <summary>
        /// Maps every element in the defined <paramref name="thisArea"/> in this <see cref="Grid2D{T}"/> which offset by <paramref name="positionOffset"/> will overlap the <paramref name="dataGrid"/>.
        /// </summary>
        /// <remarks>
        /// <i>Note: Data is always mapped from <paramref name="dataGrid"/> into this <see cref="Grid2D{T}"/>.</i>
        /// </remarks>
        /// <param name="dataGrid">The <see cref="Grid2D{T}"/> who's values are taken and mapped onto this <see cref="Grid2D{T}"/>.</param>
        /// <param name="thisArea">The area of the values of this <see cref="Grid2D{T}"/> to map onto.</param>
        /// <param name="positionOffset">The offset of the elements positions to map from <paramref name="dataGrid"/>.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the final offset <see cref="Area2D"/> is out of bounds but valid, try trim it to fit onto the <paramref name="dataGrid"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        /// <exception cref="InvalidPositionException">Thrown if the given offset position has non-integer values.</exception>
        /// <exception cref="InvalidAreaException">Thrown if an <see cref="Area2D"/> is invalid.</exception>
        /// <exception cref="AreaOutOfBoundsException">Thrown if an <see cref="Area2D"/> is outside of the bounds of its associated <see cref="Grid2D{T}"/>.</exception>
        public virtual void MapAreaFrom(Grid2D<T> dataGrid, Area2DInt thisArea, Vector2Int? positionOffset = null, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            Vector2Int validGetOffset = positionOffset ?? Vector2Int.Zero;

            CustomMapAreaFrom((Vector2Int pos) => this[pos] = dataGrid[pos + validGetOffset], dataGrid, thisArea, validGetOffset, tryTrimOnOverflow);
        }

        /// <summary>
        /// Maps every element in this <see cref="Grid2D{T}"/> which offset by <paramref name="positionOffset"/> will overlap the <paramref name="dataGrid"/>.
        /// </summary>
        /// <remarks>
        /// <i>Note: Data is always mapped from <paramref name="dataGrid"/> into this <see cref="Grid2D{T}"/>.</i>
        /// </remarks>
        /// <param name="dataGrid">The <see cref="Grid2D{T}"/> who's values are taken and mapped onto this <see cref="Grid2D{T}"/>.</param>
        /// <param name="positionOffset">The offset of the elements positions to map from <paramref name="dataGrid"/>.</param>
        /// <param name="tryTrimOnOverflow">If <see langword="true"/> and the final offset <see cref="Area2D"/> is out of bounds but valid, try trim it to fit onto the <paramref name="dataGrid"/>; otherwise, if <see langword="false"/>, throw an error when an overflow is found.</param>
        /// <exception cref="InvalidPositionException">Thrown if the given offset position has non-integer values.</exception>
        /// <exception cref="InvalidAreaException">Thrown if an <see cref="Area2D"/> is invalid.</exception>
        /// <exception cref="AreaOutOfBoundsException">Thrown if an <see cref="Area2D"/> is outside of the bounds of its associated <see cref="Grid2D{T}"/>.</exception>
        public virtual void MapFrom(Grid2D<T> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnOverflow = DefaultTryTrimOnOverflowState)
        {
            MapAreaFrom(dataGrid, GridArea, positionOffset, tryTrimOnOverflow);
        }

        /// <summary>
        /// Rotates the <see cref="Grid2D{T}"/> clockwise by 90 degrees.
        /// </summary>
        public virtual void RotateData90Clockwise()
        {
            RotateData90(1);
        }

        /// <summary>
        /// Rotates the <see cref="Grid2D{T}"/> anticlockwise by 90 degrees.
        /// </summary>
        public virtual void RotateData90Anticlockwise()
        {
            RotateData90(-1);
        }

        /// <summary>
        /// Rotates the <see cref="Grid2D{T}"/> by 90 degrees given the direction to rotate.
        /// </summary>
        /// <param name="direction">The direction to rotate in. Must be either 1 (clockwise) or -1 (anticlockwise).</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="direction"/> is not either 1 or -1.</exception>
        public virtual void RotateData90(int direction)
        {
            if (Math.Abs(direction) != 1)
            {
                throw new ArgumentException("Rotation factor must be either -1 or 1");
            }

            T[,] newData = new T[Height, Width];

            Vector2 rotationAxis = Dimensions.ToVector2().Midpoint;

            void CycleAction(Vector2Int position)
            {
                T value = this[position];

                Vector2 rotatedOffsetPosition = RotationUtils.GetRotatedOffsetPosition(position, direction, rotationAxis);

                Vector2Int newPixelPos = (rotatedOffsetPosition + rotationAxis.Inverse).ToVector2Int();

                newData[newPixelPos.X, newPixelPos.Y] = value;
            }

            GenericCycle(CycleAction);

            Data = (T[,])newData.Clone();
        }

        /// <summary>
        /// Resizes the <see cref="Grid2D{T}"/> clearing all previous data.
        /// </summary>
        /// <param name="width">The new width to resize the <see cref="Grid2D{T}"/> to.</param>
        /// <param name="height">The new height to resize the <see cref="Grid2D{T}"/> to.</param>
        public void CleanResize(int width, int height)
        {
            Data = new T[width, height];

            OnResize?.Invoke();
        }

        /// <summary>
        /// Resizes the <see cref="Grid2D{T}"/> clearing all previous data.
        /// </summary>
        /// <param name="dimensions">The new dimensions to resize the <see cref="Grid2D{T}"/> to.</param>
        public void CleanResize(Vector2Int dimensions)
        {
            CleanResize(dimensions.X, dimensions.Y);
        }

        /// <summary>
        /// Resizes the <see cref="Grid2D{T}"/> transfering all possible previous data.
        /// </summary>
        /// <remarks>
        /// Note: When downsizing, data that lies outside the new <see cref="Grid2D{T}"/> dimensions will lost.
        /// </remarks>
        /// <param name="width">The new width to resize the <see cref="Grid2D{T}"/> to.</param>
        /// <param name="height">The new height to resize the <see cref="Grid2D{T}"/> to.</param>
        public void MapResize(int width, int height)
        {
            if (width < 0 || height < 0)
            {
                throw new ArgumentException("Given dimensions are invalid.");
            }

            Grid2D<T> transferGrid = new(width, height);

            Area2DInt mapArea = transferGrid.GridArea > GridArea ? GridArea : transferGrid.GridArea;

            // Could very well be swapped out for MapToArea for same function
            transferGrid.MapAreaFrom(this, mapArea);

            Data = transferGrid.Data;

            OnResize?.Invoke();
        }

        /// <summary>
        /// Resizes the <see cref="Grid2D{T}"/> transfering all possible previous data.
        /// </summary>
        /// <remarks>
        /// Note: When downsizing, data that lies outside the new <see cref="Grid2D{T}"/> dimensions will lost.
        /// </remarks>
        /// <param name="dimensions">The new dimensions to resize the <see cref="Grid2D{T}"/> to.</param>
        public void MapResize(Vector2Int dimensions)
        {
            MapResize(dimensions.X, dimensions.Y);
        }

        /// <summary>
        /// Clears all data in the <see cref="Grid2D{T}"/>.
        /// </summary>
        public void Clear()
        {
            Array.Clear(Data);

            OnClear?.Invoke();
        }
    }
}
