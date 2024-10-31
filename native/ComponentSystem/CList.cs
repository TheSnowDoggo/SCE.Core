namespace SCECore.ComponentSystem
{
    using System.Collections;

    /// <summary>
    /// A wrapper class containing a <see cref="IComponent"/> <see cref="List{IComponent}"/>.
    /// </summary>
    public class CList : IEnumerable<IComponent>
    {
        private readonly List<IComponent> componentList;

        /// <summary>
        /// Initializes a new instance of the <see cref="CList"/> class given an initial <see cref="IComponent"/> <see cref="List{T}"/>.
        /// </summary>
        /// <param name="componentList">The initial <see cref="IComponent"/> <see cref="List{T}"/>.</param>
        public CList(List<IComponent> componentList)
        {
            this.componentList = componentList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CList"/> class given an initial <see cref="IComponent"/> to add to this instance.
        /// </summary>
        /// <param name="component">The initial <see cref="IComponent"/> to add to this instance.</param>
        public CList(IComponent component)
            : this(new List<IComponent>() { component })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CList"/> class that is empty.
        /// </summary>
        public CList()
            : this(new List<IComponent>())
        {
        }

        /// <inheritdoc cref="IndexOutOfRangeException"/>
        public static IndexOutOfRangeException RangeException { get => new("Index out of range"); }

        /// <summary>
        /// Gets a value indicating whether this instance has no elements.
        /// </summary>
        public bool IsEmpty { get => componentList.Count == 0; }

        /// <summary>
        /// Gets or sets a <see cref="Action{T}"/> delegate called whenever a new <see cref="IComponent"/> is added to this instance.
        /// </summary>
        public Action<IComponent>? OnAdd { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Action{T}"/> delegate called whenever an <see cref="IComponent"/> is removed from this instance.
        /// </summary>
        public Action<IComponent>? OnRemove { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IComponent"/> at a specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The <see cref="IComponent"/> at the specified index.</returns>
        public IComponent this[int index]
        {
            get => IsIndexValid(index) ? componentList[index] : throw RangeException;
            set => componentList[index] = IsIndexValid(index) ? value : throw RangeException;
        }

        /// <inheritdoc/>
        public IEnumerator<IComponent> GetEnumerator() => componentList.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds a new <see cref="IComponent"/> to this instance if it's valid.
        /// </summary>
        /// <param name="component">Component to add to this instance.</param>
        public void Add(IComponent component)
        {
            OnAdd?.Invoke(component);
            componentList.Add(component);
        }

        /// <summary>
        /// Removes the first occurrence of given <see cref="IComponent"/> from this instance.
        /// </summary>
        /// <param name="component">Component to try find and remove from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="component"/> is successfully removed; otherwise, <see langword="false"/>.</returns>
        public bool Remove(IComponent component)
        {
            OnRemove?.Invoke(component);
            return componentList.Remove(component);
        }

        /// <summary>
        /// Calls <see cref="IComponent.Update"/> for every <see cref="IComponent"/> in this instance.
        /// </summary>
        public void Update()
        {
            foreach (IComponent component in componentList)
            {
                if (component.IsActive)
                {
                    component.Update();
                }
            }
        }

        /// <summary>
        /// Searches for a <see cref="IComponent"/> with matching type <typeparamref name="T"/>, and returns the component if found.
        /// </summary>
        /// <typeparam name="T">The underlying <see cref="IComponent"/> type to search for.</typeparam>
        /// <returns><see cref="IComponent"/> of type <typeparamref name="T"/> if found; otherwise, throws an exception.</returns>
        public T Find<T>()
            where T : IComponent
        {
            bool found = Contains<T>(out int index);
            if (!found)
            {
                throw new ArgumentException($"Failed to resolve component of type {typeof(T)}");
            }
            else
            {
                return (T)this[index];
            }
        }

        /// <summary>
        /// Indicates whether an <see cref="IComponent"/> element with a type of <typeparamref name="T"/> is found in this instance and outputs the index of the found <see cref="IComponent"/>.
        /// </summary>
        /// <typeparam name="T">The underlying <see cref="IComponent"/> type to search for.</typeparam>
        /// <param name="index">Outputs the index of the <see cref="IComponent"/> if found; otherwise, outputs -1.</param>
        /// <returns><see langword="true"/> if a <see cref="IComponent"/> of type <typeparamref name="T"/> is found; otherwise, <see langword="false"/>.</returns>
        public bool Contains<T>(out int index)
            where T : IComponent
        {
            bool found = false;
            int i = 0;
            while (!found && i < componentList.Count)
            {
                IComponent component = componentList[i];
                if (component.GetType() == typeof(T))
                {
                    found = true;
                }
                else
                {
                    i++;
                }
            }

            index = found ? i : -1;
            return found;
        }

        /// <summary>
        /// Indicates whether an <see cref="IComponent"/> element with a type of <typeparamref name="T"/> is found in this instance.
        /// </summary>
        /// <typeparam name="T">The underlying <see cref="IComponent"/> type to search for.</typeparam>
        /// <returns><see langword="true"/> if a <see cref="IComponent"/> of type <typeparamref name="T"/> is found; otherwise, <see langword="false"/>.</returns>
        public bool Contains<T>()
            where T : IComponent
        {
            return Contains<T>(out int _);
        }

        /// <summary>
        /// Determines whether a given index is in range of this instance.
        /// </summary>
        /// <param name="index">The index to check.</param>
        /// <returns><see langword="true"/> if <paramref name="index"/> is in range; otherwise, <see langword="false"/>.</returns>
        public bool IsIndexValid(int index)
        {
            return index >= 0 && index < componentList.Count;
        }

        /// <summary>
        /// Adds every <see cref="IComponent"/> in a given <see cref="CList"/> to the end of this instance.
        /// </summary>
        /// <param name="cList">The <see cref="CList"/> to take every <see cref="IComponent"/> from.</param>
        public void Merge(CList cList)
        {
            foreach (IComponent component in cList)
            {
                Add(component);
            }
        }

        /// <summary>
        /// Returns the <see cref="IComponent"/> at the specified index as its concrete type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The concrete type of the <see cref="IComponent"/> at the specified index.</typeparam>
        /// <param name="index">The zero-based index of the component to find.</param>
        /// <returns>The <see cref="IComponent"/> at the specified index as its underlying type <typeparamref name="T"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the specified <paramref name="index"/> is out of bounds of this instance.</exception>
        /// <exception cref="ArgumentException">Thrown if the found <see cref="IComponent"/> does not match the given generic type <typeparamref name="T"/>.</exception>
        public T Get<T>(int index)
            where T : IComponent
        {
            if (!IsIndexValid(index))
            {
                throw new IndexOutOfRangeException("Index is invalid.");
            }

            IComponent component = this[index];

            if (component is T t)
            {
                return t;
            }

            throw new ArgumentException($"Found component does not match given generic type.");
        }
    }
}
