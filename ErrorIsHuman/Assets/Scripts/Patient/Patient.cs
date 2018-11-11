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
        private Area[] areaPrefabs = new Area[6];
        [SerializeField]
        private Player player;
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
                int numberOfArea = this.areaPrefabs.Length;
                for (int i = 0; i < this.proceduresPrefabs.Length; i++)
                {
                    bool limbSelected = false;
                    while (limbSelected == false)
                    {
                        int index = Random.Range(0, numberOfArea);
                        if (this.areaPrefabs[index].CurrentProcedure == null)
                        {
                            this.areaPrefabs[index].CurrentProcedure = this.proceduresPrefabs[i];
                            this.areaPrefabs[index].ProcedureIndex = 0;
                            this.areaPrefabs[index].IsHealthy = false;
                            limbSelected = true;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
