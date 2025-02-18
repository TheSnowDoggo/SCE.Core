namespace SCE
{
    public static class StartupText
    {
        private const string TextHeader = "- SCE -\n\n";

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
                "MIT License\n\n" +
                "Copyright (c) 2024 Adam Giaziri\n\n" +
                "Permission is hereby granted, free of charge, to any person obtaining a copy\n" +
                "of this software and associated documentation files (the \"Software\"), to deal\n" +
                "in the Software without restriction, including without limitation the rights\n" +
                "to use, copy, modify, merge, publish, distribute, sublicense, and/or sell\n" +
                "copies of the Software, and to permit persons to whom the Software is\n" +
                "furnished to do so, subject to the following conditions:\n\n" +
                "The above copyright notice and this permission notice shall be included in all\n" +
                "copies or substantial portions of the Software.\n\n" +
                "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR\n" +
                "IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,\n" +
                "FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE\n" +
                "AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER\n" +
                "LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,\n" +
                "OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE\n" +
                "SOFTWARE.\n");

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
                "<!> PHOTOSENSITIVITY WARNING <!>\n\n" +
                "This application may potentially trigger seizures for people with photosensitive epilepsy due to visual bugs/glitches.\n\n" +
                "User discretion is advised.\n");

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
                "Platform Compatibility Notice:\n\n" +
                "SCE is a proprietary game engine developed on and for Windows.\n\n" +
                "As a result, unforseen bugs/glitches may occur due to platform incompatibilites.\n");

            PromptContinue(true);
        }


        public static void PromptContinue(bool clear)
        {
            Console.CursorVisible = true;

            Console.Write("Press any key to continue...");

            Console.ReadKey(true);

            if (clear)
                Console.Clear();
        }
    }
}
