﻿using System.Collections;
namespace SCE
{
    public class ArrayUpdateView<T> : IEnumerable<T>
    {
        private readonly T[] _data;

        private readonly Action<int> _onUpdate;

        public ArrayUpdateView(T[] data, Action<int> onUpdate)
        {
            _data = data;
            _onUpdate = onUpdate;
        }

        public int Length { get => _data.Length; }

        public T this[int index]
        {
            get => _data[index];
            set
            {
                if (MiscUtils.QueueSet(ref _data[index], value))
                {
                    _onUpdate.Invoke(index);
                }
            }
        }

        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
