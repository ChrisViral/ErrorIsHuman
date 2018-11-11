using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorIsHuman {
    public class ViewManager : MonoBehaviour {

        #region Fields
        [SerializeField, Header("Views")]
        private GameObject mainView;
        [SerializeField]
        private GameObject areaView;
        #endregion

        #region Methods
        ///<summary>
        /// go to area view
        /// </summary>
        public void ToAreaView(Sprite areaToRender)
        {
            mainView.SetActive(false);
            areaView.SetActive(true);
            if(areaView.GetComponent<SpriteRenderer>() != null)
            {
                areaView.GetComponent<SpriteRenderer>().sprite = areaToRender;
            }
        }
        ///<summary>
        /// go back to mainView
        /// </summary>
        public void ToMain()
        {
            mainView.SetActive(true);
            areaView.SetActive(false);
        }

        #endregion
    }
}
