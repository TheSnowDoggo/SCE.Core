using CSUtils;
using System.Collections;
namespace SCE
{
    public class LoopStack<T> : IEnumerable<T>
    {
        private T[] data;

        private int start;

        private int count;

        public LoopStack(int length)
        {
            data = new T[length];
        }

        public int Length { get => data.Length; }

        public int Count { get => count; }

        public T this[int index]
        {
            get => data[Translate(index)];
            set => data[Translate(index)] = value;
        }

        private int Translate(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new IndexOutOfRangeException();
            }
            return Utils.Mod(start + index, Length);
        }

        private int End()
        {
            return Utils.Mod(start + count, Length);
        }

        public void Increase()
        {
            if (count < Length)
            {
                ++count;
            }
            else
            {
                start = Utils.Mod(start + 1, Length);
            }
        }

        public void Add(T value)
        {
            data[End()] = value;
            Increase();
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public void Remove(int count)
        {
            if (count > this.count)
            {
                throw new ArgumentException("Count exceeds current count.");
            }
            this.count -= count;
        }

        public void VirtualClear()
        {
            count = 0;
            start = 0;
        }

        public void Clear()
        {
            Array.Clear(data);
            VirtualClear();
        }

        public void Resize(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length cannot be negative.");
            }
            var newData = new T[length];
            int end = Math.Min(length, count);
            for (int i = 0; i < end; ++i)
            {
                newData[i] = this[i];
            }
            data = newData;
            start = 0;
            count = end;
        }

        public void CleanResize(int length)
        {
            data = new T[length];
            VirtualClear();
        }

        #region IEnumerable

        public IEnumerable<T> Enumerate()
        {
            for (int i = 0; i < count; ++i)
            {
                yield return this[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
