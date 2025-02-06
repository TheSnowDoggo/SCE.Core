namespace SCE
{
    public abstract class InputMap
    {
        #region UsefulMaps
        public static Dictionary<ConsoleKey, Vector2Int> WASDMap()
        {
            return new() {
                { ConsoleKey.W, Vector2Int.Up },
                { ConsoleKey.A, Vector2Int.Left },
                { ConsoleKey.S, Vector2Int.Down },
                { ConsoleKey.D, Vector2Int.Right },
            };
        }

        public static InputMap<Vector2Int> ArrowMap()
        {
            return new() {
                { ConsoleKey.UpArrow, Vector2Int.Up },
                { ConsoleKey.LeftArrow, Vector2Int.Left },
                { ConsoleKey.DownArrow, Vector2Int.Down },
                { ConsoleKey.RightArrow, Vector2Int.Right },
            };
        }
        #endregion
    }
}
