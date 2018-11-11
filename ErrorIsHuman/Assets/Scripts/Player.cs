using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using ErrorIsHuman.Patient;
using ErrorIsHuman.Patient.Steps;
using ErrorIsHuman.Utils;
using UnityEngine;

namespace ErrorIsHuman
{
    [DisallowMultipleComponent, RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
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
        private Tool[] tools = new Tool[7];
        [SerializeField, Range(0f, 100f)]
        private float stress = 0f;
        [SerializeField, Header("Sounds")]
        private AudioClip breathing;
        [SerializeField]
        private AudioClip breathingFast, breathingNervous, pickupItem; 



        private new SpriteRenderer renderer;
        private AudioSource audioSource;
        private Perlin xPerlin, yPerlin;
        private Vector2 offset;
        private Step currentStep;
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

        public Collider2D HitCollider { get; private set; }
        #endregion

        #region Methods
        public void GetHand() => this.CurrentTool = this.tools[(int)ToolType.HAND];
        public void increaseStress(float stressAmount)
        {
            this.stress = this.stress + stressAmount;
            if (this.stress == 100.0f)
            {
                audioSource.clip = breathingNervous;
                Debug.Log("heavy Stress");
            }
            else if(this.stress == 70.0f)
            {
                audioSource.clip = breathingFast;
                Debug.Log("Medium Stress");
                audioSource.Play();
                audioSource.loop = true;
            }
            else if (this.stress == 20.0f)
            {
                Debug.Log("low Stress");
                audioSource.clip = breathing;
                audioSource.Play();
                audioSource.loop = true;
            }
        }

        public void SetUsed() => this.renderer.sprite = this.CurrentTool.UsedSprite;

        public Vector2 GetWorldPosition => Camera.main.ScreenToWorldPoint(this.ClickPoint);

        public Ray GetCursorRay => Camera.main.ScreenPointToRay(this.ClickPoint);
        #endregion
        
        #region Functions
        private void Awake()
        {
            this.renderer = GetComponent<SpriteRenderer>();
            this.audioSource = GetComponent<AudioSource>();
            this.xPerlin = new Perlin();
            this.yPerlin = new Perlin();
            this.stress = 0;
        }

        private void Start() => GetHand();
        private void Update()
        {
            this.offset = new Vector2(this.xPerlin.Noise(Time.timeSinceLevelLoad * (this.Stress / 10f)), this.yPerlin.Noise(Time.timeSinceLevelLoad * (this.Stress / 10f))) * this.Stress;
            this.transform.position = this.GetWorldPosition;

            if (this.currentStep)
            {
                if (Input.GetMouseButton(0))
                {
                    this.currentStep.OnHold(this.transform.position, this);
                }
                else
                {
                    this.currentStep.OnRelease(this.transform.position, this);
                    this.currentStep = null;
                }

                this.HitCollider = null;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.GetRayIntersection(this.GetCursorRay, 20f, LayerUtils.GetLayer(Layers.DEFAULT).Mask);
                    if (hit)
                    {
                        this.Log(hit.collider.name);
                        this.HitCollider = hit.collider;
                        GameObject go = hit.collider.gameObject;
                        if (go.TryGetComponent(out Step step))
                        {
                            this.currentStep = step;
                            step.OnClick(this.transform.position, this);
                        }

                        else if (go.TryGetComponent(out Area area))
                        {
                            area.OnClick();
                            this.Log(area.name + " collider hit");
                        }

                        else if (go.tag.StartsWith("Tool", true, CultureInfo.InvariantCulture))
                        {
                            string tool = go.tag.Replace("Tool", string.Empty).ToUpperInvariant();
                            try
                            {
                                ToolType type = EnumUtils.GetValue<ToolType>(tool);
                                this.CurrentTool = this.tools[(int)type];
                                audioSource.PlayOneShot(pickupItem);
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
                    this.HitCollider = null;
                    GetHand();
                }
            }
        }
        #endregion
    }
}
