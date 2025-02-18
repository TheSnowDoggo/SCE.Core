namespace SCE
{
    using System.Diagnostics;

    public static class GameHandler
    {
        public enum PriorityType
        {
            /// <summary>
            /// <see cref="IScene"/> updates first.
            /// </summary>
            PrioritizeScenes,
            /// <summary>
            /// <see cref="IUpdate"/> updates first.
            /// </summary>
            PrioritizeUpdates,
        }

        public const int FRAMECAP_UNCAPPED = -1;

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

        /// <summary>
        /// Gets the average number of frames updated per second. 
        /// </summary>
        /// <remarks>
        /// Update frequency is determined by <see cref="FPSUpdateRate"/>.
        /// </remarks>
        public static int FPS { get; private set; } = -1;

        /// <summary>
        /// Gets or sets the update frequency in seconds of the FPS.
        /// </summary>
        public static double FPSUpdateRate { get; set; } = DEFAULT_FPS_UPDATERATE;

        /// <summary>
        /// Gets the time in seconds since the last update.
        /// </summary>
        public static double DeltaTime { get; private set; }

        /// <summary>
        /// Gets the actual time taken to update the last frame ignoring frame cap waiting time.
        /// </summary>
        public static double RealDeltaTime { get; private set; }

        #endregion

        #region FrameCapVariables

        private static double targetDeltaTime = 0.0;
        private static double frameCap = FRAMECAP_UNCAPPED;     

        /// <summary>
        /// Gets or sets the update rate cap in frames per second. 
        /// </summary>
        /// <remarks>
        /// Set to -1 for uncapped update rate.
        /// </remarks>
        public static double FrameCap
        {
            get => frameCap;
            set => SetFrameCap(value);
        }

        private static void SetFrameCap(double value)
        {
            if (value is <= 0 and not -1)
                throw new ArgumentException("Value invalid.");
            frameCap = value;
            targetDeltaTime = 1 / (double)value;
        }

        /// <summary>
        /// Gets a value indicating whether the update rate has been capped.
        /// </summary>
        public static bool FrameCapped { get => frameCap != -1; }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether gamehandler should update.
        /// </summary>
        public static bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="IScene"/> or <see cref="IUpdate"/> should receive updates first.
        /// </summary>
        public static PriorityType PriorityMode { get; set; } = PriorityType.PrioritizeScenes;

        /// <summary>
        /// Calls start before starting up gamehandler thread.
        /// </summary>
        public static void Start()
        {
            CallStart();
            StartUpdateThread();
        }

        #region Call

        /// <summary>
        /// Calls start.
        /// </summary>
        public static void CallStart()
        {
            _sceneGroup.Start();
        }

        /// <summary>
        /// Calls update.
        /// </summary>
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
