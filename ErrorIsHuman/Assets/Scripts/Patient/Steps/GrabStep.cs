using System.Collections.Generic;
using ErrorIsHuman.Utils;
using UnityEngine;

namespace ErrorIsHuman.Patient.Steps
{
    public class GrabStep : Step
    {
        [SerializeField]
        private List<Collider2D> objects = new List<Collider2D>();

        private Vector2 startPos;
        private Collider2D dragging;

        public override void OnClick(Vector2 position, Player player)
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(player.GetCursorRay, 20f, LayerUtils.GetLayer(Layers.GRAB).Mask);
            if (hit && this.objects.Contains(hit.collider))
            {
                this.dragging = hit.collider;
                this.startPos = this.dragging.transform.position;
            }
        }

        public override void OnHold(Vector2 position, Player player)
        {
            if (this.dragging)
            {
                this.dragging.transform.position = position;
            }
        }

        public override void OnRelease(Vector2 position, Player player)
        {
            if (this.tool != player.CurrentTool.Type) { return; }

            this.Log("release");
            if (this.dragging)
            {
                if (ViewManager.Instance.table.bounds.Contains(position))
                {
                    this.objects.Remove(this.dragging);
                    Destroy(this.dragging.gameObject);
                    if (this.objects.Count == 0)
                    {
                        Complete();
                    }
                }
                else
                {
                    this.dragging.transform.position = this.startPos;
                    this.dragging = null;
                    Fail();
                }
            }
        }
    }
}
