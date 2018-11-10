using System;
using UnityEngine;

namespace ErrorIsHuman.Patient.Steps
{
    public abstract class Step : MonoBehaviour
    {
        #region Events
        public event Action OnFail;
        public event Action OnComplete;
        #endregion

        #region Abstract methods
        public abstract void Activate();

        public abstract void Fail();

        public abstract void Complete();
        #endregion
    }
}