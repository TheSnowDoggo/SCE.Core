namespace SCE
{
    internal class Test
    {
        internal static void Main()
        {
            string raw = "MMMMYYABBABBSGGSKKMNKMKNTTTKKTTHEEEEGGGBBBYYZZKKMEMMENNEBGAGGGSGGSUMMKKMMMKKKM";

            string compress = StringUtils.RLCompress(raw);

            string decompress = StringUtils.RLDecompress(compress);

            Console.WriteLine(raw);

            Console.WriteLine(compress);

            Console.WriteLine(decompress);

            Console.WriteLine(decompress == raw);

            Console.WriteLine(compress.Length / (double)raw.Length * 100.0);
        }
    }
}
