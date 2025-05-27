namespace SCE
{
    /// <summary>
    /// A class for grid-based pathfinding using A* algorithm.
    /// </summary>
    public class PathFinder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathFinder"/> class.
        /// </summary>
        /// <param name="area">The allowed area to path through.</param>
        /// <param name="collisionFunc">The function determining whether a given position is occupied or not.</param>
        public PathFinder(Rect2DInt area, Func<Vector2Int, bool> collisionFunc)
        {
            Area = area;
            CollisionFunc = collisionFunc;
        }

        public Rect2DInt Area { get; set; }

        public Func<Vector2Int, bool> CollisionFunc { get; set; }

        public Func<Vector2Int, Vector2Int, int> DistanceSystem { get; set; } = TaxicabDistance;

        public Func<Vector2Int, Vector2Int, int> HeuristicSystem { get; set; } = TaxicabDistance;

        public static Vector2Int[] Axis8Moves { get; } = new Vector2Int[]
        {
            Vector2Int.Up,
            Vector2Int.Down,
            Vector2Int.Left,
            Vector2Int.Right,
            Vector2Int.UpLeft,
            Vector2Int.UpRight,
            Vector2Int.DownLeft,
            Vector2Int.DownRight,
        };

        public static Vector2Int[] Axis4Moves { get; } = new Vector2Int[]
        {
            Vector2Int.Up,
            Vector2Int.Down,
            Vector2Int.Left,
            Vector2Int.Right,
        };

        public Vector2Int[] Moves { get; set; } = Axis4Moves;

        public Queue<Vector2Int>? PathTo(Vector2Int start, Vector2Int goal)
        {
            if (!Area.Contains(start) || !Area.Contains(goal))
                return null;

            PriorityQueue<Vector2Int, int> frontier = new();
            frontier.Enqueue(start, HeuristicSystem(start, goal));

            Dictionary<Vector2Int, Vector2Int> cameFrom = new();

            Dictionary<Vector2Int, int> gScore = new() { [start] = 0 };

            while (frontier.Count != 0)
            {
                var current = frontier.Dequeue();
                if (current == goal)
                    return ReconstructPath(cameFrom, current);
                foreach (var move in Moves)
                {
                    var neighbor = current + move;
                    if (Area.Contains(neighbor) && !CollisionFunc(neighbor))
                    {
                        int tentativeGScore = gScore[current] + DistanceSystem(current, neighbor);
                        bool contains = gScore.TryGetValue(neighbor, out int neighborGScore);
                        if (!contains || tentativeGScore < neighborGScore)
                        {
                            cameFrom[neighbor] = current;
                            gScore[neighbor] = tentativeGScore;
                            if (!contains)
                                frontier.Enqueue(neighbor, tentativeGScore + HeuristicSystem(neighbor, goal));
                        }
                    }
                }
            }
            return null;
        }

        private static Queue<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            Queue<Vector2Int> totalPath = new();
            do 
                totalPath.Enqueue(current);
            while (cameFrom.TryGetValue(current, out current));
            return totalPath;
        }

        #region Distance

        public static int TaxicabDistance(Vector2Int v1, Vector2Int v2)
        {
            return Math.Abs(v2.X - v1.X) + Math.Abs(v2.Y - v1.Y);
        }

        public static int ChebyshevDistance(Vector2Int v1, Vector2Int v2)
        {
            return Math.Max(Math.Abs(v2.X - v1.X), Math.Abs(v2.Y - v1.Y));
        }

        public static int EuclideanDistance(Vector2Int v1, Vector2Int v2)
        {
            return (int)v1.ToVector2().DistanceFrom(v2);
        }

        #endregion
    }
}
