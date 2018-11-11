using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorIsHuman.Patient
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Area : MonoBehaviour {

        #region Fields
        [SerializeField]
        private Sprite areaSprite;
        [SerializeField]
        private Sprite overlayWound;
        [SerializeField]
        private AreaViewManager areaViewManager;

        //Sprite renderers to alter
        private SpriteRenderer overlayRenderer;
        
        #endregion

        #region Properties
        public bool IsHealthy{ get; set; }
        public Procedure CurrentProcedure { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Change the background sprite to that of that specific area
        /// </summary>
        public void GoToArea()
        {
            areaViewManager.goToAreaView(areaSprite);
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
            overlayRenderer = this.GetComponent<SpriteRenderer>();
            overlayRenderer.sprite = overlayWound;
            if (this.IsHealthy == true)
            {
                overlayRenderer.enabled = false;
            }
        }
        #endregion



    }
}
