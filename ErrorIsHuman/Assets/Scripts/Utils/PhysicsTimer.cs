using UnityEngine;

namespace ErrorIsHuman.Utils
{
    /// <summary>
    /// A generic Stopwatch clone running on Unity's internal physics clock
    /// </summary>
    public class PhysicsTimer : Timer
    {
        #region Properites
        /// <inheritdoc cref="Timer.CurrentTime"/>
        protected override float CurrentTime => Time.fixedTime;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new PhysicsTimer
        /// </summary>
        public PhysicsTimer() { }

        /// <summary>
        /// Creates a new PhysicsTimer starting at a certain amount of time
        /// </summary>
        /// <param name="seconds">Time to start at, in seconds</param>
        public PhysicsTimer(float seconds) : base(seconds) { }
        #endregion

        #region Static methods
        /// <summary>
        /// Creates a new PhysicsTimer, starts it, and returns the current instance
        /// </summary>
        public new static PhysicsTimer StartNew()
        {
            PhysicsTimer timer = new PhysicsTimer();
            timer.Start();
            return timer;
        }

        /// <summary>
        /// Creates a new PhysicsTimer from a certain amount of time, starts it, and returns the current instance
        /// </summary>
        /// <param name="seconds">Time to start the watch at, in seconds</param>
        public new static PhysicsTimer StartNewFromTime(float seconds)
        {
            PhysicsTimer timer = new PhysicsTimer(seconds);
            timer.Start();
            return timer;
        }
        #endregion
    }
}