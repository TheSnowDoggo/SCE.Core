namespace SCE
{
    using System;

    /// <summary>
    /// A wrapper class of a 2D-array with additional features.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the grid.</typeparam>
    public class Grid2D<T> : IEquatable<Grid2D<T>>, ICloneable
    {
        private const bool DEFAULT_TRIM = false;

        private T[,] data;

        #region Constructors
        public Grid2D(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new InvalidDimensionsException();
            data = new T[width, height];
            Data = new(this);
            UpdateDimensions();
        }

        public Grid2D(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        private Grid2D(T[,] data)
        {
            this.data = data;
            Data = new(this);
            UpdateDimensions();
        }

        public Grid2D(Grid2D<T> grid)
            : this(grid.data)
        {
        }
        #endregion

        #region VGrid
        public VirtualGrid2D<T> Data { get; }
        #endregion

        #region Actions
        public Action? OnResize;

        public Action? OnClear;
        #endregion

        #region Properties
        public int Width { get => data.GetLength(0); }

        public int Height { get => data.GetLength(1); }

        public Vector2Int Dimensions { get; private set; }

        public Area2DInt GridArea { get; private set; }

        public int Size { get => Width * Height; }
        #endregion

        #region Indexers
        public T this[int x, int y]
        {
            get => Get(x, y);
            set => Set(x, y, value);
        }

        public T this[Vector2Int pos]
        {
            get => Get(pos);
            set => Set(pos, value);
        }
        #endregion

        #region Clone
        public virtual Grid2D<T> Clone()
        {
            return new((T[,])data.Clone());
        }
        
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Equal
        public bool Equals(Grid2D<T>? other)
        {
            return other is Grid2D<T> grid && data.Equals(grid.data);
        }

        public override bool Equals(object? obj)
        {
            return Equals((Grid2D<T>?)obj);
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region String
        public override string ToString()
        {
            return Dimensions.ToString();
        }

        public static Grid2D<string> ToStringGrid<U>(Grid2D<U> grid)
            where U : IConvertible
        {
            var stringGrid = new Grid2D<string>(grid.Dimensions);

            grid.GenericCycle((pos) => stringGrid[pos] = grid[pos].ToString() ?? string.Empty);

            return stringGrid;
        }
        #endregion

        #region GetSet
        public T Get(int x, int y)
        {
            if (!PositionValid(x, y))
                throw new PositionOutOfBoundsException();
            return data[x, y];
        }

        public T Get(Vector2Int pos)
        {
            return Get(pos.X, pos.Y);
        }

        public T Set(int x, int y, T value)
        {
            if (!PositionValid(x, y))
                throw new PositionOutOfBoundsException();
            return data[x, y] = value;
        }

        public T Set(Vector2Int pos, T value)
        {
            return Set(pos.X, pos.Y, value);
        }
        #endregion

        #region PositionValid
        public bool PositionValid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public bool PositionValid(Vector2Int position)
        {
            return PositionValid(position.X, position.Y);
        }
        #endregion

        #region Cycle
        public void GenericCycleArea(Func<Vector2Int, bool> func, Area2DInt area, bool tryTrimOnOverflow = DEFAULT_TRIM)
        {
            if (!Area2DInt.Overlaps(GridArea, area))
                throw new InvalidAreaException();
            if (!tryTrimOnOverflow && !GridArea.Contains(area))
                throw new AreaOutOfBoundsException();

            var newArea = GridArea.TrimArea(area);

            newArea.Expose(out Vector2Int start, out Vector2Int end);

            for (int x = start.X; x < end.X; ++x)
            {
                for (int y = start.Y; y < end.Y; ++y)
                {
                    if (!func.Invoke(new(x, y)))
                        return;
                }
            }
        }

        public void GenericCycleArea(Action<Vector2Int> action, Area2DInt area, bool tryTrimOnOverflow = DEFAULT_TRIM)
        {
            GenericCycleArea(ToCycleFunc(action), area, tryTrimOnOverflow);
        }

        public void GenericCycle(Func<Vector2Int, bool> func)
        {
            GenericCycleArea(func, GridArea);
        }

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

        #region Search
        public int IndexOf(T item)
        {
            return Array.IndexOf(data, item);
        }

        public bool Contains(T item, out int index)
        {
            index = IndexOf(item);
            return index != -1;
        }

        public bool Contains(T item)
        {
            return Contains(item, out _);
        }
        #endregion

        #region Mapping
        public void CustomMapToArea(Action<Vector2Int> action, Grid2D<T> dataGrid, Area2DInt dataGridArea, Vector2Int positionOffset, bool tryTrimOnOverflow = DEFAULT_TRIM)
        {
            if (!Area2DInt.Overlaps(dataGrid.GridArea, dataGridArea))
                throw new InvalidAreaException("Given get area doesn't overlap the get grid.");

            if (!tryTrimOnOverflow && !dataGrid.GridArea.Contains(dataGridArea))
                throw new AreaOutOfBoundsException("Given get area is outside of the bounds of the get grid.");

            var offsetArea = dataGridArea + positionOffset;

            if (!Area2DInt.Overlaps(GridArea, offsetArea))
                throw new AreaOutOfBoundsException("Offset area doesn't overlap this grid.");

            dataGridArea = GridArea.TrimArea(offsetArea, out bool hasFixed) - positionOffset;

            dataGrid.GenericCycleArea(action, dataGridArea, tryTrimOnOverflow);
        }

        public virtual void MapToArea(Grid2D<T> dataGrid, Area2DInt dataGridArea, Vector2Int? positionOffset = null, bool tryTrimOnOverflow = DEFAULT_TRIM)
        {
            var validThisOffset = positionOffset ?? Vector2Int.Zero;

            CustomMapToArea((Vector2Int pos) => this[pos + validThisOffset] = dataGrid[pos], dataGrid, dataGridArea, validThisOffset, tryTrimOnOverflow);
        }

        public virtual void MapTo(Grid2D<T> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnOverflow = DEFAULT_TRIM)
        {
            MapToArea(dataGrid, dataGrid.GridArea, positionOffset, tryTrimOnOverflow);
        }

        public void CustomMapAreaFrom(Action<Vector2Int> action, Grid2D<T> dataGrid, Area2DInt thisArea, Vector2Int positionOffset, bool tryTrimOnOverflow = DEFAULT_TRIM)
        {
            if (!Area2DInt.Overlaps(GridArea, thisArea))
                throw new InvalidAreaException("Given set area doesn't overlap with this grid.");
            if (!tryTrimOnOverflow && !GridArea.Contains(thisArea))
                throw new AreaOutOfBoundsException("Given set area is outside of the bounds of this grid.");

            var alignedGetArea = thisArea + positionOffset;

            if (!Area2DInt.Overlaps(dataGrid.GridArea, alignedGetArea))
                throw new InvalidAreaException("Offset area doesn't overlap with the get grid.");

            thisArea = dataGrid.GridArea.TrimArea(alignedGetArea, out bool hasFixed) - positionOffset;

            GenericCycleArea(action, thisArea, tryTrimOnOverflow);
        }

        public virtual void MapAreaFrom(Grid2D<T> dataGrid, Area2DInt thisArea, Vector2Int? positionOffset = null, bool tryTrimOnOverflow = DEFAULT_TRIM)
        {
            var validGetOffset = positionOffset ?? Vector2Int.Zero;

            CustomMapAreaFrom((Vector2Int pos) => this[pos] = dataGrid[pos + validGetOffset], dataGrid, thisArea, validGetOffset, tryTrimOnOverflow);
        }

        public virtual void MapFrom(Grid2D<T> dataGrid, Vector2Int? positionOffset = null, bool tryTrimOnOverflow = DEFAULT_TRIM)
        {
            MapAreaFrom(dataGrid, GridArea, positionOffset, tryTrimOnOverflow);
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

            Vector2 rotationAxis = Dimensions.ToVector2().Midpoint;

            void CycleAction(Vector2Int position)
            {
                T value = this[position];

                Vector2 rotatedOffsetPosition = RotationUtils.GetRotatedOffsetPosition(position, direction, rotationAxis);

                Vector2Int newPixelPos = (rotatedOffsetPosition + rotationAxis.Inverse).ToVector2Int();

                newData[newPixelPos.X, newPixelPos.Y] = value;
            }

            GenericCycle(CycleAction);

            data = newData;
        }
        #endregion

        #region Resizing
        public void CleanResize(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new InvalidDimensionsException();
            data = new T[width, height];
            UpdateDimensions();
            OnResize?.Invoke();
        }

        public void CleanResize(Vector2Int dimensions)
        {
            CleanResize(dimensions.X, dimensions.Y);
        }

        public void MapResize(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new InvalidDimensionsException();

            var transferGrid = new Grid2D<T>(width, height);

            var mapArea = transferGrid.GridArea > GridArea ? GridArea : transferGrid.GridArea;

            transferGrid.MapAreaFrom(this, mapArea);

            data = transferGrid.data;
            UpdateDimensions();
            OnResize?.Invoke();
        }

        public void MapResize(Vector2Int dimensions)
        {
            MapResize(dimensions.X, dimensions.Y);
        }

        private void UpdateDimensions()
        {
            Dimensions = new(Width, Height);
            GridArea = new(Vector2Int.Zero, Dimensions);
        }
        #endregion

        #region Clear
        public void Clear()
        {
            Array.Clear(data);
            OnClear?.Invoke();
        }
        #endregion
    }
}
