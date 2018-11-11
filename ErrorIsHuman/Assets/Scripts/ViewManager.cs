using UnityEngine;

namespace ErrorIsHuman
{
    public class ViewManager : MonoBehaviour
    {
        public enum Areas
        {
            TORSO     = 0,
            BELLY     = 1,
            RIGHT_ARM = 2,
            LEFT_ARM  = 3,
            RIGHT_LEG = 4,
            LEFT_LEG  = 5
        }

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
            this.mainView.SetActive(false);
            this.areaView.SetActive(true);
            if(this.areaView.GetComponent<SpriteRenderer>() != null)
            {
                this.areaView.GetComponent<SpriteRenderer>().sprite = areaToRender;
            }
        }
        ///<summary>
        /// go back to mainView
        /// </summary>
        public void ToMain()
        {
            this.mainView.SetActive(true);
            this.areaView.SetActive(false);
        }
        #endregion
    }
}
