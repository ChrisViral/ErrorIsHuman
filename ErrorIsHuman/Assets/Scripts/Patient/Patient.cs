using UnityEngine;

namespace ErrorIsHuman.Patient
{
    public class Patient : MonoBehaviour
    {
        [SerializeField]
        private float startHeartbeat = 70f;
        [SerializeField]
        private float startBloodQuantity = 5000f, maxBloodQuantity = 5000f;
        [SerializeField]
        private Procedure[] proceduresPrefabs = new Procedure[0];

    }
}
