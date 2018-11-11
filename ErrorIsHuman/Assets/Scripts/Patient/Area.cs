using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorIsHuman.Patient
{
    public class Area : MonoBehaviour {

        #region Fields
        [SerializeField]
        private SpriteRenderer bgSprite;
        #endregion

        #region Properties
        public bool IsHealthy{ get; set; }
        public Procedure CurrentProcedure { get; set; }
        #endregion
    }
}
