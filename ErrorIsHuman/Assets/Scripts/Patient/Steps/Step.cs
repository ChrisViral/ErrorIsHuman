using System;
using UnityEngine;
using UnityEngine.Events;

namespace ErrorIsHuman.Patient.Steps
{
    public abstract class Step : MonoBehaviour
    {
        #region Events
        public UnityEvent OnFail = new UnityEvent();
        public UnityEvent OnComplete = new UnityEvent();
        #endregion

        #region Abstract methods
        public abstract void Activate();

        public abstract void Fail();

        public abstract void Complete();

        public abstract void OnClick(Vector2 positionr);

        public abstract void OnHold(Vector2 position);

        public abstract void OnRelease(Vector2 position);
        #endregion
    }
}