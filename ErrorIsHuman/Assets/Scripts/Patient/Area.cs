using UnityEngine;

namespace ErrorIsHuman.Patient
{
    public class Area : MonoBehaviour
    {
        [SerializeField]
        private GameObject body;

        [SerializeField]
        private GameObject trauma;

        #region Properties
        public bool IsHealthy{ get; set; }
        public Procedure CurrentProcedure { get; set; }
        #endregion

        #region Methods
        public void SetProcedure(Procedure procedure)
        {
            this.CurrentProcedure = Instantiate(procedure, this.body.transform, false);
            this.CurrentProcedure.AttachArea(this);
        }

        public void Cure() => this.IsHealthy = true;

        /// <summary>
        /// actives area view with this area's sprite
        /// </summary>
        public void OnClick()
        {
            ViewManager.Instance.ToAreaView(this.gameObject);
        }

        private void Update()
        {
            if (this.IsHealthy && this.trauma) { Destroy(this.trauma); }
        }
        #endregion
    }
}
