﻿using ErrorIsHuman.Patient.Steps;
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
        /// <summary>
        /// Reduces patients health
        /// </summary>
        public void HurtPatient()
        {
            Patient.FailedStepLoss();
        }

        public void NextStep()
        {
            this.CurrentStep.OnComplete -= NextStep;
            this.CurrentStep.OnFail -= HurtPatient;
            this.currentIndex++;
            if(currentIndex == steps.Length)
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