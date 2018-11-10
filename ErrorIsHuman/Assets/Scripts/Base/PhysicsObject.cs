using UnityEngine;

namespace ErrorIsHuman.Base
{
    /// <summary>
    /// Physical object abstract class
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PhysicsObject : PausableObject
    {
        #region Properties
        /// <summary>
        /// This object's Rigidbody component
        /// </summary>
        public Rigidbody2D Rigidbody { get; private set; }
        #endregion

        #region Virtual methods
        /// <summary>
        /// This is called from within Awake, you should override this instead of writing an Awake() method
        /// </summary>
        protected virtual void OnAwake() { }
        #endregion

        #region Functions
        private void Awake()
        {
            this.Rigidbody = GetComponent<Rigidbody2D>();
            OnAwake();
        }
        #endregion
    }
}
