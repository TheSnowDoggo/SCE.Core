namespace SCE
{
    using System.Diagnostics;

    /// <summary>
    /// A class containing useful time sleeping and conversion functions.
    /// </summary>
    public static class TimeUtils
    {
        /// <summary>
        /// Suspends the current thread for the sepcified number of seconds.
        /// </summary>
        /// <param name="milleiecondsTimeout">Milliseconds to sleep for.</param>
        public static void WaitMS(int milleiecondsTimeout) => Thread.Sleep(milleiecondsTimeout);

        /// <summary>
        /// Suspends the current thread for the sepcified number of seconds.
        /// </summary>
        /// <param name="secondsTimeout">Seconds to sleep for.</param>
        public static void Wait(double secondsTimeout) => WaitMS((int)(secondsTimeout * 1000));

        /// <summary>
        /// Returns the <see cref="Stopwatch"/>.Elapsed.TotalMilliseconds converted into <see cref="double"/> seconds.
        /// </summary>
        /// <param name="stopwatch">The stopwatch to check.</param>
        /// <returns><see cref="Stopwatch"/>.Elapsed.TotalMilliseconds converted to <see cref="double"/> seconds.</returns>
        public static double TotalSeconds(Stopwatch stopwatch) => ToSeconds(TotalMilliseconds(stopwatch));

        /// <summary>
        /// Returns the <see cref="Stopwatch"/>.Elapsed.TotalMilliseconds of the specified stopwatch.
        /// </summary>
        /// <param name="stopwatch">The stopwatch to use.</param>
        /// <returns>The <see cref="Stopwatch"/>.Elapsed.TotalMilliseconds of the specified stopwatch.</returns>
        public static double TotalMilliseconds(Stopwatch stopwatch) => stopwatch.Elapsed.TotalMilliseconds;

        /// <summary>
        /// Converts <see cref="double"/> milliseconds to <see cref="double"/> seconds.
        /// </summary>
        /// <param name="milliseconds">The milliseconds to convert from.</param>
        /// <returns>The <see cref="double"/> <paramref name="milliseconds"/> time in <see cref="double"/> seconds.</returns>
        public static double ToSeconds(double milliseconds) => milliseconds / 1000;

        /// <summary>
        /// Converts <see cref="int"/> milliseconds to <see cref="double"/> seconds.
        /// </summary>
        /// <param name="milliseconds">The milliseconds to convert from.</param>
        /// <returns>The <see cref="int"/> <paramref name="milliseconds"/> time in <see cref="double"/> seconds.</returns>
        public static double ToSeconds(int milliseconds) => ToSeconds((double)milliseconds);

        /// <summary>
        /// Converts <see cref="double"/> seconds to <see cref="double"/> milliseconds.
        /// </summary>
        /// <param name="seconds">The seconds to convert from.</param>
        /// <returns>The <see cref="double"/> <paramref name="seconds"/> time in <see cref="double"/> milliseconds.</returns>
        public static double ToMilliseconds(double seconds) => seconds * 1000;

        /// <summary>
        /// Converts <see cref="double"/> seconds to <see cref="int"/> milliseconds.
        /// </summary>
        /// <param name="seconds">The seconds to convert from.</param>
        /// <returns>The <see cref="double"/> <paramref name="seconds"/> time in <see cref="int"/> milliseconds.</returns>
        public static int ToIntMilliseconds(double seconds) => (int)ToMilliseconds(seconds);
    }
}
