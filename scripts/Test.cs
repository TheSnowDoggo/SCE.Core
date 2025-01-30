namespace SCE
{
    internal class Test
    {
        internal static void Main()
        {
            string raw = "hello world uwu asdjaskld thisi s a test        asdjljansjdnsasssssssssss";

            string compress = StringUtils.RLCompress(raw);

            string decompress = StringUtils.RLDecompress(compress);

            double savings = (1 - (double)compress.Length / raw.Length) * 100.0;

            Console.WriteLine(raw);

            Console.WriteLine(compress);

            Console.WriteLine(decompress);

            Console.WriteLine(decompress == raw);

            Console.WriteLine($"Savings: {savings:0.00}%");
        }
    }
}
