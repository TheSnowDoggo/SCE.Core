namespace SCECore.Components
{
    using System.Collections;

    /// <summary>
    /// A class containing a CList and the ICContainerHolder.
    /// Used for storing components by a holder.
    /// </summary>
    public class CContainer : IEnumerable<IComponent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CContainer"/> class.
        /// Takes the CContainerHolder and a default CList.
        /// </summary>
        /// <param name="holder">The reference to the CContainerHolder storing this instance.</param>
        /// <param name="cList">The default CList for this instance.</param>
        public CContainer(ICContainerHolder holder, CList cList)
        {
            CContainerHolder = holder;
            CList = cList;

            CList.OnAdd += CContainer_OnAdd;
            CList.OnRemove += CContainer_OnRemove;
            SetAllCContainer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CContainer"/> class with an empty CList.
        /// Takes the CContainerHolder.
        /// </summary>
        /// <param name="holder">The reference to the CContainerHolder storing this instance.</param>
        public CContainer(ICContainerHolder holder)
            : this(holder, new CList())
        {
        }

        /// <inheritdoc cref="SCEComponents.ICContainerHolder"/>
        public ICContainerHolder CContainerHolder { get; }

        /// <inheritdoc cref="SCEComponents.CList"/>
        public CList CList { get; }

        /// <summary>
        /// Gets a value indicating whether this instance's CList has no elements.
        /// </summary>
        public bool IsEmpty => CList.IsEmpty;

        /// <summary>
        /// Gets or sets the <see cref="IComponent"/> at a specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The <see cref="IComponent"/> at the specified index.</returns>
        public IComponent this[int index]
        {
            get => CList[index];
            set => CList[index] = value;
        }

        /// <inheritdoc/>
        public IEnumerator<IComponent> GetEnumerator() => CList.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="CList.Update"/>
        public void Update()
        {
            CList.Update();
        }

        /// <inheritdoc cref="CList.Add(IComponent)"/>
        public void Add(IComponent component)
        {
            CList.Add(component);
        }

        /// <inheritdoc cref="CList.Remove(IComponent)"/>
        public bool Remove(IComponent component)
        {
            return CList.Remove(component);
        }

        /// <inheritdoc cref="CList.Find{T}"/>
        public T Find<T>()
            where T : IComponent
        {
            return CList.Find<T>();
        }

        /// <inheritdoc cref="CList.Contains{T}(out int)"/>
        public bool Contains<T>(out int index)
            where T : IComponent
        {
            return CList.Contains<T>(out index);
        }

        /// <inheritdoc cref="CList.Contains{TComponent}"/>
        public bool Contains<T>()
            where T : IComponent
        {
            return Contains<T>(out _);
        }

        /// <inheritdoc cref="CList.Get{T}(int)"/>
        public T Get<T>(int index)
            where T : IComponent
        {
            return CList.Get<T>(index);
        }

        /// <summary>
        /// Sets the <see cref="CContainerHolder"/> of every <see cref="IComponent"/> in <see cref="CList"/>.
        /// </summary>
        private void SetAllCContainer()
        {
            foreach (IComponent component in CList)
            {
                component.SetCContainer(this, CContainerHolder);
            }
        }

        private void CContainer_OnAdd(IComponent component)
        {
            component.SetCContainer(this, CContainerHolder);
        }

        private void CContainer_OnRemove(IComponent component)
        {
            component.SetCContainer(null, CContainerHolder);
        }
    }
}
