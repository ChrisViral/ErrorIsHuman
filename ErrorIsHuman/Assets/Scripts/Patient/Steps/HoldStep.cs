using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ErrorIsHuman.Utils;
namespace ErrorIsHuman.Patient.Steps {
    [RequireComponent(typeof(CircleCollider2D))]
    public class HoldStep : Step{

        #region Fields
        [SerializeField]
        private float holdDuration, rangeCollider;
        private bool active;
        private Timer timer = new Timer();
        #endregion

        #region Method
        public override void OnClick(Vector2 position)
        {
            Activate();
            timer.Start();
        }
        public override void OnHold(Vector2 position)
        {
            if(Vector2.Distance(position, transform.position) > rangeCollider)
            {
                Fail();
            }
        }
        public override void OnRelease(Vector2 position)
        {
            if(timer.ElapsedSeconds < holdDuration)
            {
                Fail();
            }
        }
        public override void Activate()
        {
            active = true;
            rangeCollider = this.GetComponent<CircleCollider2D>().radius;
        }
        public override void Complete()
        {
            Debug.Log(this.name + " Complete hold step");
            OnComplete?.Invoke();
        }
        public override void Fail()
        {
            Debug.Log(this.name + " failed hold step");
            OnFail?.Invoke();
        }
        #endregion
        // Update is called once per frame
        void Update() {
            if (active && (timer.ElapsedSeconds > holdDuration) )
            {
                Complete();
            }
        }
    }
}
