using System.Text.RegularExpressions;
using System;

namespace SerialNumberCheckup
{
    internal record StickerContent(ProductNumber ProductNumber, SerialNumber SerialNumber)
    {
        private static Regex reg = new Regex(@"(^(1[Ss])(\w){10}(\w){8}$)");

        public static StickerContent FromString(string input)
        {
            if (!(input.Length == 20 && reg.IsMatch(input)))
            {
                throw new ArgumentException(nameof(input));
            }

            SplitInput(input, out var pn, out var sn);

            var p = new ProductNumber(pn);
            var n = new SerialNumber(sn);

            return new StickerContent(p, n);
        }

        public static bool TryCreateInstanceFromString(string input, out StickerContent? stickerContent)
        {
            stickerContent = null;
            
            if (!(input.Length == 20 && reg.IsMatch(input)))
            {
                return false;
            }

            SplitInput(input, out var productNumberString, out var serialNumberStrings);

            var productNumber = new ProductNumber(productNumberString);
            var serialNumber = new SerialNumber(serialNumberStrings);

            stickerContent = new StickerContent(productNumber, serialNumber);

            return true;
        }

        public static void SplitInput(string input, out string productNumberString, out string serialNumberString)
        {
            productNumberString = input.Substring(2, 10);
            serialNumberString = input.Substring(12, 8);
        }

        public static string GetStickerContent()
        {
            Console.WriteLine("WORKING ONLY WITH LENOVO STICKERS!");
            Console.Write("Please scan product sticker followed by ENTER key:");
            var input = Console.ReadLine();
            return input;
        }
    }
}
