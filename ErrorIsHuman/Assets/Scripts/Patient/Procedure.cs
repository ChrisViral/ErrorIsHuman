using ErrorIsHuman.Patient.Steps;
using UnityEngine;

namespace ErrorIsHuman.Patient
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Procedure : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private Step[] steps = new Step[0];
        [SerializeField]
        private Sprite[] sprites = new Sprite[0];

        private Area area;
        private Patient patient;
        private new SpriteRenderer renderer;
        #endregion

        #region Properties
        public Patient Patient { get; set; }
        public int CurrentIndex { get; set; }
        public int SpriteIndex { get; set; }
        private Step CurrentStep => this.steps[this.CurrentIndex];
        #endregion

        #region Methods
        public void AttachArea(Area area) => this.area = area;

        private void SetupStep()
        {
            this.CurrentStep.OnComplete.AddListener(NextStep);
            this.CurrentStep.OnFail.AddListener(HurtPatient);
            this.CurrentStep.gameObject.SetActive(true);
        }
        /// <summary>
        /// Reduces patients health
        /// </summary>
        public void HurtPatient()
        {
            ViewManager.Instance.Patient.FailedStepLoss();
        }

        public void NextStep(bool changeSprite)
        {
            if (changeSprite)
            {
                this.renderer.sprite = this.sprites[this.SpriteIndex++];
            }
            this.CurrentStep.OnComplete.RemoveListener(NextStep);
            this.CurrentStep.OnFail.RemoveListener(HurtPatient);
            Destroy(this.CurrentStep.gameObject);
            this.CurrentIndex++;
            if(this.CurrentIndex == this.steps.Length)
            {
                this.area.Cure();
            }
            else
            {
                SetupStep();
            }
        }
        
        #endregion

        #region Functions
        private void Awake()
        {
            this.renderer = GetComponent<SpriteRenderer>();
            this.renderer.sprite = this.sprites[this.SpriteIndex++];
        }

        private void Start()
        {
            if (this.steps.Length > 0)
            {
                foreach (Step step in this.steps)
                {
                    step.gameObject.SetActive(false);
                }
                SetupStep();
            }
        }
        #endregion
    }
}