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
        #endregion


        #region Methods
        public void FailedStepLoss()
        {
            startBloodQuantity -= bloodLoss;
        }
        #endregion

        #region Functions
        private void Start()
        {
            if(proceduresPrefabs.Length == 0)
            {
                Debug.Log("no procedures were initilized");
            }

            int numberOfArea = areaPrefabs.Length; 
                for(int i  = 0; i < proceduresPrefabs.Length; i++)
            {
                bool limbSelected = false;
                while (limbSelected == false)
                {
                    int index = Random.Range(0, numberOfArea);
                    if (areaPrefabs[index].CurrentProcedure == null)
                    {
                        areaPrefabs[index].CurrentProcedure = proceduresPrefabs[i];
                        limbSelected = true;
                    }
                }
            }
        }
        #endregion



    }
}
