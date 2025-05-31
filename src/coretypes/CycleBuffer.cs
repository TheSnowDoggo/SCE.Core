using CSUtils;
using System.Collections;

namespace SCE
{
    public class CycleBuffer<T> : IEnumerable<T>
    {
        private T[] data;

        private int start;

        public CycleBuffer(int length)
        {
            data = new T[length];
        }

        public int Length { get => data.Length; }

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

        public void Shift(int shift)
        {
            start = Utils.Mod(start + shift, Length);
        }

        public void Fill(T value, int index, int count)
        {
            if (index + count > Length)
            {
                throw new IndexOutOfRangeException("Fill will overflow bounds.");
            }
            for (int i = 0; i < count; ++i)
            {
                this[index + i] = value;
            }
        }

        public void Clear()
        {
            Array.Clear(data);
            start = 0;
        }

        public void Resize(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length cannot be negative.");
            }
            var newData = new T[length];
            int end = Math.Min(length, Length);
            for (int i = 0; i < end; ++i)
            {
                newData[i] = this[i];
            }
            data = newData;
            start = 0;
        }

        public void CleanResize(int length)
        {
            data = new T[length];
            start = 0;
        }

        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Length; ++i)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
