using UnityEngine;
using ErrorIsHuman.Utils;

namespace ErrorIsHuman.Patient.Steps
{
    [RequireComponent(typeof(Collider2D))]
    public class HoldStep : Step
    {
        #region Fields
        [SerializeField]
        private float holdDuration;

        private bool active;
        private new Collider2D collider;
        private readonly Timer timer = new Timer();
        #endregion

        #region Method
        public override void OnClick(Vector2 position, Player player)
        {
            if (player.CurrentTool.Type != this.tool)
            {
                this.Log("wrong tool");
                return; 

            }
            this.Log("Start hold");
            this.timer.Restart();
        }

        public override void OnHold(Vector2 position, Player player)
        {
            if (player.CurrentTool.Type != this.tool) { return; }
            if (!this.collider.bounds.Contains(position))
            {
                Fail();
            }
            else if (this.timer.IsRunning && this.timer.ElapsedSeconds > this.holdDuration)
            {
                player.SetUsed();
                this.timer.Stop();
                Complete();
            }
        }

        public override void OnRelease(Vector2 position, Player player)
        {
            if (player.CurrentTool.Type != this.tool) { return; }
            if (this.timer.ElapsedSeconds < this.holdDuration)
            {
                Fail();
            }
        }
        #endregion

        #region Functions
        private void Awake()
        {
            this.collider = GetComponent<Collider2D>();
        }
        #endregion
    }
}
