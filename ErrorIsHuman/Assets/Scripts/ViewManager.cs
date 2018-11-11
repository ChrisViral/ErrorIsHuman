using System.Collections.Generic;
using DG.Tweening;
using ErrorIsHuman.Base;
using ErrorIsHuman.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace ErrorIsHuman
{
    public class ViewManager : Singleton<ViewManager>
    {
        public enum Areas
        {
            TORSO     = 0,
            BELLY     = 1,
            RIGHTARM = 2,
            LEFTARM  = 3,
            RIGHTLEG = 4,
            LEFTLEG  = 5
        }

        #region Fields
        [SerializeField, Header("Views")]
        private GameObject mainView;
        [SerializeField]
        private GameObject areaView;
        [SerializeField]
        private Transform body;
        [SerializeField]
        private Image fade;
        [SerializeField]
        private Vector2[] positions = new Vector2[6];
        public Collider2D table;
        #endregion

        #region MyRegion
        
        #endregion

        #region Methods
        ///<summary>
        /// go to area view
        /// </summary>
        public void ToAreaView(GameObject go)
        {
            if (!go.tag.StartsWith("Area")) { return; }
            
            this.body.localPosition = this.positions[(int)EnumUtils.GetValue<Areas>(go.tag.Replace("Area", string.Empty).ToUpperInvariant())];
            StartCoroutine(Fade(false));
        }

        private IEnumerator<YieldInstruction> Fade(bool mainView)
        {
            yield return this.fade.DOFade(1f, 0.5f).WaitForCompletion();
            this.mainView.SetActive(mainView);
            this.areaView.SetActive(!mainView);
            yield return this.fade.DOFade(0f, 0.5f).WaitForCompletion();
        }

        ///<summary>
        /// go back to mainView
        /// </summary>
        public void ToMain() => StartCoroutine(Fade(true));
        #endregion

        #region Functions
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToMain();
            }
        }
        #endregion
    }
}
