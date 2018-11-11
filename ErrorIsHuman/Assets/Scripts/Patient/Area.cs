using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorIsHuman.Patient
{
    public class Area : MonoBehaviour {

        #region Fields
        [SerializeField]
        private Sprite areaSprite;
        //private Sprite overlayWound;
        [SerializeField]
        private ViewManager viewManager;

        //Sprite renderers to alter
        private SpriteRenderer overlayRenderer;
        
        #endregion

        #region Properties
        public bool IsHealthy{ get; set; }
        public Procedure CurrentProcedure { get; set; }
        public int ProcedureIndex { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// actives area view with this area's sprite
        /// </summary>
        public void OnClick()
        {
            viewManager.ToAreaView(areaSprite);
        }
        /// <summary>
        /// Disable the overlayWound when the procedure is done
        /// </summary>
        public void DisableOverlayWound()
        {
            if (overlayRenderer != null)
            {
                overlayRenderer.enabled = false;
            }
        }
        #endregion

        #region Functions
        private void Start()
        {
            overlayRenderer = this.GetComponentInChildren<SpriteRenderer>();
            // overlayRenderer.sprite = overlayWound;
            if (this.IsHealthy == true)
            {
                overlayRenderer.enabled = false;
            }
        }
        #endregion



    }
}
