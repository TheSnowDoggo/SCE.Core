namespace SCE
{
    public struct FlagSet8
    {
        private static readonly byte[] powerArr = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };

        private byte data;

        public FlagSet8(byte data = 0)
        {
            this.data = data;
        }

        public static implicit operator FlagSet16(FlagSet8 f) => f.ToFlagset16();
        
        public static explicit operator byte(FlagSet8 f) => f.data;

        public static FlagSet8 operator ~(FlagSet8 f) => new((byte)~f.data);

        public static FlagSet8 operator >>(FlagSet8 f, int shift) => new((byte)(f.data >> shift));

        public static FlagSet8 operator <<(FlagSet8 f, int shift) => new((byte)(f.data << shift));

        public static FlagSet8 operator ^(FlagSet8 f1, FlagSet8 f2) => new((byte)(f1.data ^ f2.data));

        public static FlagSet8 operator |(FlagSet8 f1, FlagSet8 f2) => new((byte)(f1.data | f2.data));

        public static FlagSet8 operator &(FlagSet8 f1, FlagSet8 f2) => new((byte)(f1.data & f2.data));

        public bool this[int bit]
        {
            get => Read(bit);
            set => Set(bit, value);
        }

        public override string ToString()
        {
            return StringUtils.PreFitToLength(Convert.ToString(data, 2), 8, '0');
        }

        public FlagSet16 ToFlagset16()
        {
            return new(data);
        }

        public bool Read(int bit)
        {
            if (bit < 0 || bit > 7)
                throw new BitOutOfRangeException($"{bit} is invalid.");
            if (bit == 7)
                return data >= powerArr[7];
            return data % powerArr[bit + 1] >= powerArr[bit];
        }

        public bool Flip(int bit)
        {
            bool current = Read(bit);
            FSet(bit, !current);
            return !current;
        }

        public void Set(int bit, bool set)
        {
            bool current = Read(bit);
            if (current != set)
                FSet(bit, set);
        }

        public void Clear()
        {
            data = 0;
        }

        private void FSet(int bit, bool set)
        {
            if (set)
                data += powerArr[bit];
            else
                data -= powerArr[bit];
        }
    }
}
