using UnityEngine;
using ArenaSurvivor.Core;
using ArenaSurvivor.Enemies;

namespace ArenaSurvivor.Combat
{
    public class Projectile : MonoBehaviour
    {
        public float Speed = 14f;
        public int Damage = 34;
        public float Lifetime = 2.5f;

        Vector2 _direction;
        float _bornAt;

        public static Projectile Create(Vector2 origin, Vector2 direction)
        {
            var go = new GameObject("Projectile");
            go.transform.position = origin;
            go.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = SpriteFactory.Circle;
            sr.color = new Color(1f, 0.9f, 0.25f, 1f);
            sr.sortingOrder = 8;

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;

            var col = go.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.5f;

            var projectile = go.AddComponent<Projectile>();
            projectile._direction = direction.normalized;
            projectile._bornAt = Time.time;
            rb.velocity = projectile._direction * projectile.Speed;
            return projectile;
        }

        void Update()
        {
            if (Time.time - _bornAt >= Lifetime)
                Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<ArenaWall>() != null)
            {
                Destroy(gameObject);
                return;
            }

            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
}
