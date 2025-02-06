namespace SCE
{
    internal class RenderInfoMap
    {
        private readonly HashSet<ColorSet> _colors = new(256);

        public ColorSet[] Build()
        {
            var renderInfo = new ColorSet[_colors.Count];
            int i = 0;
            foreach (var colorSet in _colors)
            {
                renderInfo[i] = colorSet;
                ++i;
            }
            return renderInfo;
        }

        public void Load(ColorSet colorSet)
        {
            if (!_colors.Contains(colorSet))
                _colors.Add(colorSet);
        }
    }
}
