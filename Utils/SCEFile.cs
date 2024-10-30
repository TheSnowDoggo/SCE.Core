namespace SCECore.Utils
{
    public static class SCEFile
    {
        public static string FindFilePathInDirectory(string rootPath, string nameWithoutExtension)
        {
            string[] fileList = Directory.GetFiles(rootPath);

            foreach (string file in fileList)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);

                if (fileName == nameWithoutExtension)
                {
                    return file;
                }
            }

            throw new ArgumentException($"File with name {nameWithoutExtension} not found");
        }
    }
}
