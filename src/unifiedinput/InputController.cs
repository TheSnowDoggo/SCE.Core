namespace SCE.UIS
{
    public static class InputController
    {
        private static readonly Thread _thread = new(Run);

        private static bool isActive;

        public static Action<UISKeyInfo>? OnKeyEvent { get; set; }

        public static bool Start()
        {
            if (_thread.IsAlive)
            {
                return false;
            }

            _thread.Start();

            return true;
        }

        public static void Stop()
        {
            isActive = false;
        }

        public static void Link(InputHandler inputHandler)
        {
            OnKeyEvent += inputHandler.QueueKey;
        }

        public static void Delink(InputHandler inputHandler)
        {
            OnKeyEvent -= inputHandler.QueueKey;
        }

        private static void Run()
        {
            isActive = true;
            while (isActive)
            {
                var keyInfo = Console.ReadKey(true);
                OnKeyEvent?.Invoke(new UISKeyInfo(keyInfo, InputType.ConsoleStream));
            }
        }
    }
}
