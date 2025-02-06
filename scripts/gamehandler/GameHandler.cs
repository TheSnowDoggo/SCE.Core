namespace SCE
{
    using System.Diagnostics;
    public static class GameHandler
    {
        public enum PriorityType
        {
            PrioritizeScenes,
            PrioritizeUpdates,
        }

        private const double DEFAULT_FPS_UPDATERATE = 0.5;

        #region Thread
        private static readonly Thread _updateThread = new(UpdateLoop);

        private static bool isRunning;
        #endregion

        #region Scenes
        private static readonly SceneGroup _sceneGroup = new();

        private static readonly UpdateGroup _updateGroup = new();

        public static SearchHashTypeExt<IScene> Scenes { get => _sceneGroup; }

        public static SearchHashTypeExt<IUpdate> Updates { get => _updateGroup; }
        #endregion

        #region StatisticVariables
        private static readonly Stopwatch deltaStopwatch = new();
        private static readonly Stopwatch realDeltaStopwatch = new();

        private static double fpsTimer = 0.0;
        private static int frameCount = 0;

        public static int FPS { get; private set; } = -1;

        public static double FPSUpdateRate { get; set; } = DEFAULT_FPS_UPDATERATE;

        public static double DeltaTime { get; private set; }

        public static double RealDeltaTime { get; private set; }
        #endregion

        #region FrameCapVariables
        private static int frameCap = -1;
        private static double targetDeltaTime = 0.0;

        public static int FrameCap
        {
            get => frameCap;
            set
            {
                if (value is <= 0 and not -1)
                    throw new ArgumentException("Value invalid.");

                frameCap = value;
                targetDeltaTime = 1 / (double)value;
            }
        }

        public static bool FrameCapped { get => frameCap != -1; }
        #endregion

        public static bool IsActive { get; set; } = true;

        public static PriorityType PriorityMode { get; set; } = PriorityType.PrioritizeScenes;

        public static void Start()
        {
            CallStart();
            StartUpdateThread();
        }

        #region Call
        public static void CallStart()
        {
            _sceneGroup.Start();
        }

        public static void CallUpdate()
        {
            if (PriorityMode == PriorityType.PrioritizeScenes)
            {
                _sceneGroup.Update();
                _updateGroup.Update();
            }
            else
            {
                _updateGroup.Update();
                _sceneGroup.Update();
            }
        }
        #endregion

        #region Update
        private static void UpdateLoop()
        {
            while (isRunning)
            {
                if (IsActive)
                {
                    UpdateStatistics();

                    CallUpdate();

                    StopRDTime();

                    TryCapFPS();
                }
            }
        }

        public static void StopUpdateThread()
        {
            isRunning = false;
        }

        public static void StartUpdateThread()
        {
            if (_updateThread.IsAlive)
                throw new GameHandlerAlreadyRunningException();
            isRunning = true;
            _updateThread.Start();
        }
        #endregion

        #region FrameCap
        private static void TryCapFPS()
        {
            while (FrameCapped && deltaStopwatch.Elapsed.TotalSeconds < targetDeltaTime)
            {
            }
        }
        #endregion

        #region Statistics
        private static void UpdateDTime()
        {
            DeltaTime = deltaStopwatch.Elapsed.TotalSeconds;
            deltaStopwatch.Restart();
        }

        private static void UpdateFPS()
        {
            fpsTimer += DeltaTime;
            frameCount++;
            if (fpsTimer >= FPSUpdateRate)
            {
                FPS = (int)Math.Round(frameCount / fpsTimer);
                fpsTimer = 0;
                frameCount = 0;
            }
        }

        private static void UpdateRDTime()
        {
            realDeltaStopwatch.Restart();
        }

        private static void StopRDTime()
        {
            realDeltaStopwatch.Stop();
            RealDeltaTime = realDeltaStopwatch.Elapsed.TotalSeconds;
        }

        private static void UpdateStatistics()
        {
            UpdateRDTime();
            UpdateDTime();
            UpdateFPS();
        }
        #endregion
    }
}
