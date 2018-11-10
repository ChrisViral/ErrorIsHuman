using DG.Tweening;
using UnityEngine;

namespace ErrorIsHuman.Extensions
{
    /// <summary>
    /// DOTween movement extensions
    /// </summary>
    public static class DOTweenExtensions
    {
        #region Extension methods
        /// <summary>
        /// Moves the Rigidbody to the target on the X axis only, at the given speed
        /// </summary>
        /// <param name="rigidbody">Body to move</param>
        /// <param name="target">Target to reach</param>
        /// <param name="speed">Speed at which to travel</param>
        /// <param name="offset">Distance offset on the x axis to the target to reach</param>
        /// <returns>The tweener generated for this movement</returns>
        public static Tween DOMoveToX(this Rigidbody2D rigidbody, Transform target, float speed, float offset = 0f)
        {
            //Get X target
            float x = target.position.x;
            if (offset > 0f)
            {
                //Flip if needed
                x += offset * (rigidbody.position.x > x ? 1f : -1f);
            }
            return rigidbody.DOMoveX(x, Mathf.Abs(rigidbody.position.x - x) / speed)
                            .SetUpdate(UpdateType.Fixed);
        }

        /// <summary>
        /// Moves the Rigidbody to the target on the Y axis only, at the given speed
        /// </summary>
        /// <param name="rigidbody">Body to move</param>
        /// <param name="target">Target to reach</param>
        /// <param name="speed">Speed at which to travel</param>
        /// <param name="offset">Distance offset on the Y axis to the target to reach</param>
        /// <returns>The tweener generated for this movement</returns>
        public static Tween DOMoveToY(this Rigidbody2D rigidbody, Transform target, float speed, float offset = 0f)
        {
            //Get Y target
            float y = target.position.x;
            if (offset > 0f)
            {
                //Flip if needed
                y += offset * (rigidbody.position.y > y ? 1f : -1f);
            }
            return rigidbody.DOMoveX(y, Mathf.Abs(rigidbody.position.y - y) / speed)
                            .SetUpdate(UpdateType.Fixed);
        }

        /// <summary>
        /// Moves the Rigidbody to the target at the given speed
        /// </summary>
        /// <param name="rigidbody">Body to move</param>
        /// <param name="target">Target to reach</param>
        /// <param name="speed">Speed at which to travel</param>
        /// <param name="offset">Distance offset to the target to reach, in the travel direction</param>
        /// <returns>The tweener generated for this movement</returns>
        public static Tween DOMoveTo(this Rigidbody2D rigidbody, Transform target, float speed, float offset = 0f)
        {
            //Get Y target
            Vector2 pos = target.position;
            if (offset > 0f)
            {
                //Flip if needed
                Vector2 offsetDir = (rigidbody.position - (Vector2)target.position).normalized * offset;
                offsetDir.x *= rigidbody.position.x > pos.x ? 1f : -1f;
                offsetDir.y *= rigidbody.position.y > pos.y ? 1f : -1f;
                pos += offsetDir;
            }
            return rigidbody.DOMove(pos, Vector2.Distance(rigidbody.position, pos) / speed)
                            .SetUpdate(UpdateType.Fixed);
        }
        #endregion
    }
}
