using UnityEngine;

namespace ErrorIsHuman.Patient
{
    public class Patient : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private float startHeartbeat = 70f;
        [SerializeField]
        private float startBloodQuantity = 5000f, maxBloodQuantity = 5000f, bloodLoss = 500f;
        [SerializeField]
        private Procedure[] proceduresPrefabs = new Procedure[0];
        [SerializeField]
        private Player player;
        [SerializeField]
        private Area[] areas = new Area[6];
        #endregion


        #region Methods
        public void FailedStepLoss()
        {
            this.startBloodQuantity -= this.bloodLoss;
            player.increaseStress(10);
        }
        #endregion

        #region Functions
        private void Start()
        {
            if (this.proceduresPrefabs.Length == 0)
            {
                Debug.Log("no procedures were initialized");
            }
            else
            {
                int numberOfArea = this.areas.Length;
                foreach (Procedure p in this.proceduresPrefabs)
                {
                    bool limbSelected = false;
                    while (limbSelected == false)
                    {
                        int index = Random.Range(0, numberOfArea);
                        if (this.areas[index].CurrentProcedure == null)
                        {
                            this.areas[index].SetProcedure(p);
                            limbSelected = true;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
