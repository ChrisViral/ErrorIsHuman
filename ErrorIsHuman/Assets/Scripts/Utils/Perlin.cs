using UnityEngine;

namespace ErrorIsHuman.Utils
{
    public class Perlin
    {
        private Vector2 offset, direction;

        public Perlin()
        {
            this.offset = Random.insideUnitCircle * Random.Range(0f, 10f);
            this.direction = Random.insideUnitCircle;
        }

        public float Noise(float time) => (Mathf.PerlinNoise(this.offset.x + (this.direction.x * time), this.offset.y + (this.direction.y * time)) * 2f) - 1f;
    }
}
