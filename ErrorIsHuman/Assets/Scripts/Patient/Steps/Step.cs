using System;
using UnityEngine;
using UnityEngine.Events;
using ToolType = ErrorIsHuman.Player.ToolType;

namespace ErrorIsHuman.Patient.Steps
{
    [Serializable]
    public class CompleteEvent : UnityEvent<bool> { }

    public abstract class Step : MonoBehaviour
    {
        #region Events
        public UnityEvent OnFail = new UnityEvent();
        public CompleteEvent OnComplete = new CompleteEvent();
        #endregion

        #region Fields
        [SerializeField]
        private bool changeSprite = true;
        [SerializeField]
        protected ToolType tool = ToolType.GAUZE;
        #endregion

        #region Abstract methods
        public virtual void Fail() => this.OnFail?.Invoke();

        public virtual void Complete() => this.OnComplete?.Invoke(this.changeSprite);

        public abstract void OnClick(Vector2 position, Player player);

        public abstract void OnHold(Vector2 position, Player player);

        public abstract void OnRelease(Vector2 position, Player player);
        #endregion
    }
}