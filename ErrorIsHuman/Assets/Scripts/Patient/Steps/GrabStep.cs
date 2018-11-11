using System.Collections.Generic;
using DG.Tweening;
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
                this.dragging.transform.SetParent(player.transform);
            }
        }

        public override void OnHold(Vector2 position, Player player) { }

        public override void OnRelease(Vector2 position, Player player)
        {
            if (this.tool != player.CurrentTool.Type) { return; }

            this.Log("release");
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
                this.dragging.transform.SetParent(null, true);
                this.dragging.transform.position = this.startPos;
                Fail();
            }
        }
    }
}
