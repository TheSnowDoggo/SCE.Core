namespace SCECore.Utils
{
    public static class StartupText
    {
        private const string TextHeader = "- LunaSCE -\r\n\r\n";

        /// <summary>
        /// Displays the software license.
        /// </summary>
        /// <remarks>
        /// This may be removed.
        /// Note: The original license file must not be removed.
        /// </remarks>
        public static void DisplayLicense()
        {
            Console.CursorVisible = false;

            Console.WriteLine(
                TextHeader +
                "MIT License\r\n\r\n" +
                "Copyright (c) 2024 Adam Giaziri\r\n\r\n" +
                "Permission is hereby granted, free of charge, to any person obtaining a copy\r\n" +
                "of this software and associated documentation files (the \"Software\"), to deal\r\n" +
                "in the Software without restriction, including without limitation the rights\r\n" +
                "to use, copy, modify, merge, publish, distribute, sublicense, and/or sell\r\n" +
                "copies of the Software, and to permit persons to whom the Software is\r\n" +
                "furnished to do so, subject to the following conditions:\r\n\r\n" +
                "The above copyright notice and this permission notice shall be included in all\r\n" +
                "copies or substantial portions of the Software.\r\n\r\n" +
                "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR\r\n" +
                "IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,\r\n" +
                "FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE\r\n" +
                "AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER\r\n" +
                "LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,\r\n" +
                "OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE\r\n" +
                "SOFTWARE.\r\n");

            PromptContinue(true);
        }

        /// <summary>
        /// Displays the photosensitivity warning.
        /// </summary>
        /// <remarks>
        /// This may be removed, but is not recommended.
        /// </remarks>
        public static void DisplayPhotosensitivityWarning()
        {
            Console.CursorVisible = false;

            Console.WriteLine(
                TextHeader +
                "<!> PHOTOSENSITIVITY WARNING <!>\r\n\r\n" +
                "This application may potentially trigger seizures for people with photosensitive epilepsy due to visual bugs/glitches.\r\n\r\n" +
                "User discretion is advised.\r\n");

            PromptContinue(true);
        }

        /// <summary>
        /// Displays the platform compatibility notice.
        /// </summary>
        /// <remarks>
        /// This may be removed.
        /// </remarks>
        public static void DisplayPlatformCompatibilityNotice()
        {
            Console.CursorVisible = false;

            Console.WriteLine(
                TextHeader +
                "Platform Compatibility Notice:\r\n\r\n" +
                "Luna_SCE is a proprietary game engine developed on and for Windows.\r\n\r\n" +
                "As a result, unforseen bugs/glitches may occur due to platform incompatibilites.\r\n");

            PromptContinue(true);
        }

        private static void PromptContinue(bool clear)
        {
            Console.CursorVisible = true;

            Console.Write("Press any key to continue...");

            Console.ReadKey(true);

            if (clear)
            {
                Console.Clear();
            }
        }
    }
}
