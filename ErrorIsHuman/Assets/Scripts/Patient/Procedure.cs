using ErrorIsHuman.Patient.Steps;
using UnityEngine;

namespace ErrorIsHuman.Patient
{
    public class Procedure : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private Step[] steps = new Step[0];
        #endregion

        #region Properties
        public Patient Patient { get; set; }
        public int CurrentIndex { get; set; }
        private Step CurrentStep => this.steps[this.CurrentIndex];
        #endregion

        #region Methods
        private void SetupStep()
        {
            this.CurrentStep.OnComplete.AddListener(NextStep);
            this.CurrentStep.OnFail.AddListener(HurtPatient);
            this.CurrentStep.Activate();
        }
        /// <summary>
        /// Reduces patients health
        /// </summary>
        public void HurtPatient()
        {
            Patient.FailedStepLoss();
        }

        public void NextStep()
        {
            this.CurrentStep.OnComplete.RemoveListener(NextStep);
            this.CurrentStep.OnFail.RemoveListener(HurtPatient);
            this.CurrentIndex++;
            if(CurrentIndex == steps.Length)
            {
                // Stop the rendering of the wound in room view
                this.GetComponentInParent<Area>().DisableOverlayWound();
            }
            else
            {
                SetupStep();
            }
        }
        
        #endregion

        #region Functions
        private void Start()
        {
            if (this.steps.Length > 0)
            {
                SetupStep();
            }
        }
        #endregion
    }
}