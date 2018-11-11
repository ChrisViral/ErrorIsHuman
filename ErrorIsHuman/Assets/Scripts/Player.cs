using System;
using ErrorIsHuman.Patient;
using ErrorIsHuman.Patient.Steps;
using ErrorIsHuman.Utils;
using UnityEngine;

namespace ErrorIsHuman
{
    [DisallowMultipleComponent, RequireComponent(typeof(SpriteRenderer))]
    public class Player : MonoBehaviour
    {
        public enum ToolType
        {
            GAUZE,
            FORCEPS,
            NEEDLE,
            SYRINGE,
            HAND,
            DRAIN,
            PATCH
        }

        [Serializable]
        public class ToolData
        {
            [SerializeField]
            private ToolType type;
            [SerializeField]
            private Sprite tool;
            [SerializeField]
            private Sprite usedTool;
        }

        #region Fields
        [SerializeField]
        private Sprite hand;
        [SerializeField]
        private ToolData[] tools = new ToolData[0];
        [SerializeField, Range(0f, 100f)]
        private float stress = 10f;

        private new SpriteRenderer renderer;
        private Perlin xPerlin, yPerlin;
        private Vector2 offset;
        private readonly RaycastHit2D[] hits = new RaycastHit2D[16];
        #endregion

        #region Properties
        public float Stress
        {
            get => this.stress;
            set => this.stress = value;
        }
        
        public Vector2 ClickPoint => (Vector2)Input.mousePosition + this.offset;
        #endregion
        
        #region Functions
        private void Awake()
        {
            this.renderer = GetComponent<SpriteRenderer>();
            this.xPerlin = new Perlin();
            this.yPerlin = new Perlin();
        }

        private void Start() { this.renderer.sprite = this.hand; }

        private void Update()
        {
            this.offset = new Vector2(this.xPerlin.Noise(Time.timeSinceLevelLoad * (this.Stress / 10f)), this.yPerlin.Noise(Time.timeSinceLevelLoad * (this.Stress / 10f))) * this.Stress;
            this.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(this.ClickPoint);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(this.ClickPoint);
                int count = Physics2D.GetRayIntersectionNonAlloc(ray, this.hits, 20f, LayerUtils.GetLayer(Layers.DEFAULT).Mask);

#if UNITY_EDITOR
                this.Log($"Origin: {ray.origin.ToString("0.000")}, Direction: {ray.direction.ToString("0.000")}, Hit count: {count}");
#endif
                for (int i = 0; i < count; i++)
                {
                    RaycastHit2D hit = this.hits[i];

#if UNITY_EDITOR
                    this.Log($"Object: {hit.collider.name}, Hit position: {hit.point.ToString("0.000")}, Mouse position: {((Vector2)Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition)).ToString("0.000")}");
                    DebugExtension.DebugCircle(hit.point, Vector3.forward, Color.red, 0.25f, 3f);
#endif

                    GameObject go = hit.collider.gameObject;
                    if (go.TryGetComponent(out Step step))
                    {

                    }
                    
                    else if (go.TryGetComponent(out Area area))
                    {

                    }

                    else
                    {
                        switch (go.tag)
                        {

                        }
                    }
                }
            }
        }
        #endregion
    }
}
