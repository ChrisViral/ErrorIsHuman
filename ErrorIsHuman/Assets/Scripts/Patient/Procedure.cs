using ErrorIsHuman.Patient.Steps;
using UnityEngine;

namespace ErrorIsHuman.Patient
{
    public class Procedure : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private Step[] steps = new Step[0];
        [SerializeField]

        private int currentIndex;
        #endregion

        #region Properties
        public Patient Patient { get; set; }

        private Step CurrentStep => this.steps[this.currentIndex];
        #endregion

        #region Methods
        private void SetupStep()
        {
            this.CurrentStep.OnComplete += NextStep;
            this.CurrentStep.OnFail += HurtPatient;
            this.CurrentStep.Activate();
        }

        public void HurtPatient()
        {

        }

        public void NextStep()
        {
            this.CurrentStep.OnComplete -= NextStep;
            this.CurrentStep.OnFail -= HurtPatient;
            this.currentIndex++;
            SetupStep();
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