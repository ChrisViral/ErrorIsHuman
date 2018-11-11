using System;
using System.Globalization;
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
            GAUZE   = 0,
            FORCEPS = 1,
            NEEDLE  = 2,
            SYRINGE = 3,
            HAND    = 4,
            DRAIN   = 5,
            PATCH   = 6
        }

        [Serializable]
        public class Tool
        {
            #region Properties
            [SerializeField]
            private ToolType type;
            public ToolType Type => this.type;

            [SerializeField]
            private Sprite sprite;
            public Sprite Sprite => this.sprite;

            [SerializeField]
            private Sprite usedSprite;
            public Sprite UsedSprite => this.usedSprite;
            #endregion
        }

        #region Fields
        [SerializeField]
        private Tool[] tools = new Tool[0];
        [SerializeField, Range(0f, 100f)]
        private float stress = 10f;

        private new SpriteRenderer renderer;
        private Perlin xPerlin, yPerlin;
        private Vector2 offset;
        #endregion

        #region Properties
        public float Stress
        {
            get => this.stress;
            set => this.stress = value;
        }
        
        public Vector2 ClickPoint => (Vector2)Input.mousePosition + this.offset;

        private Tool currentTool;
        public Tool CurrentTool
        {
            get => this.currentTool;
            set
            {
                this.currentTool = value;
                this.renderer.sprite = this.currentTool.Sprite;
            }
        }
        #endregion

        #region Methods
        public void GetHand() => this.CurrentTool = this.tools[(int)ToolType.HAND];
        #endregion
        
        #region Functions
        private void Awake()
        {
            this.renderer = GetComponent<SpriteRenderer>();
            this.xPerlin = new Perlin();
            this.yPerlin = new Perlin();
        }

        private void Start() => GetHand();

        private void Update()
        {
            this.offset = new Vector2(this.xPerlin.Noise(Time.timeSinceLevelLoad * (this.Stress / 10f)), this.yPerlin.Noise(Time.timeSinceLevelLoad * (this.Stress / 10f))) * this.Stress;
            this.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(this.ClickPoint);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(this.ClickPoint);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 20f, LayerUtils.GetLayer(Layers.DEFAULT).Mask);
#if UNITY_EDITOR
                this.Log($"Origin: {ray.origin.ToString("0.000")}, Direction: {ray.direction.ToString("0.000")}");
                if (hit)
                {
                    this.Log($"Object: {hit.collider.name}, Tag: {hit.collider.tag}, Hit position: {hit.point.ToString("0.000")}, Mouse position: {((Vector2)Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition)).ToString("0.000")}");
                }
                DebugExtension.DebugCircle(hit.point, Vector3.forward, Color.red, 0.25f, 3f);
#endif
                if (hit)
                {
                    GameObject go = hit.collider.gameObject;
                    if (go.TryGetComponent(out Step step))
                    {

                    }
                    
                    else if (go.TryGetComponent(out Area area))
                    {
                        area.OnClick();
                        Debug.Log(area.name + " collider hit");
                    }

                    else if (go.tag.StartsWith("Tool", true, CultureInfo.InvariantCulture))
                    {
                        string tool = go.tag.Replace("Tool", string.Empty).ToUpperInvariant();
                        try
                        {
                            ToolType type = EnumUtils.GetValue<ToolType>(tool);
                            this.CurrentTool = this.tools[(int)type];
                        }
                        catch (Exception e)
                        {
                            this.Log("Invalid tool detected");
                            this.LogException(e);
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                GetHand();
            }
        }
        #endregion
    }
}
