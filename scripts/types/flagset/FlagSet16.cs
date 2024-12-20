namespace SCE
{
    public struct FlagSet16
    {
        private static readonly ushort[] powerArr = new ushort[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768 };

        private ushort data;

        public FlagSet16(ushort data = 0)
        {
            this.data = data;
        }

        public static explicit operator FlagSet8(FlagSet16 f) => f.ToFlagset8();

        public static explicit operator ushort(FlagSet16 f) => f.data;

        public static FlagSet16 operator ~(FlagSet16 f) => new((ushort)~f.data);

        public static FlagSet16 operator >>(FlagSet16 f, int shift) => new((ushort)(f.data >> shift));

        public static FlagSet16 operator <<(FlagSet16 f, int shift) => new((ushort)(f.data << shift));

        public static FlagSet16 operator ^(FlagSet16 f1, FlagSet16 f2) => new((ushort)(f1.data ^ f2.data));

        public static FlagSet16 operator |(FlagSet16 f1, FlagSet16 f2) => new((ushort)(f1.data | f2.data));

        public static FlagSet16 operator &(FlagSet16 f1, FlagSet16 f2) => new((ushort)(f1.data & f2.data));

        public bool this[int bit]
        {
            get => Read(bit);
            set => Set(bit, value);
        }

        public override string ToString()
        {
            return StringUtils.PreFitToLength(Convert.ToString(data, 2), 16, '0');
        }

        public FlagSet8 ToFlagset8()
        {
            return new((byte)data);
        }

        public bool Read(int bit)
        {
            if (bit < 0 || bit > 15)
                throw new BitOutOfRangeException($"{bit} is invalid.");
            if (bit == 15)
                return data >= powerArr[15];
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
