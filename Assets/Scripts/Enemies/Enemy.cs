using System.Collections;
using UnityEngine;
using ArenaSurvivor.Core;
using ArenaSurvivor.Player;

namespace ArenaSurvivor.Enemies
{
    public class Enemy : MonoBehaviour
    {
        public const int ScoreReward = 100;

        public int MaxHealth { get; private set; } = 100;
        public int ContactDamage { get; private set; } = 10;

        Rigidbody2D _rb;
        SpriteRenderer _sr;
        Transform _target;
        int _health;
        float _speed;
        float _lastAttackTime = -99f;
        Color _baseColor;

        public static Enemy Create(Vector2 position, int waveNumber)
        {
            var go = new GameObject("Enemy");
            go.transform.position = position;
            var scale = Random.Range(0.85f, 1.1f);
            go.transform.localScale = new Vector3(scale, scale, 1f);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = SpriteFactory.Circle;
            sr.color = new Color(0.9f, 0.25f, 0.2f, 1f);
            sr.sortingOrder = 5;

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.45f;

            var enemy = go.AddComponent<Enemy>();
            enemy._rb = rb;
            enemy._sr = sr;
            enemy._baseColor = sr.color;
            enemy._health = enemy.MaxHealth;
            enemy._speed = 2.6f + waveNumber * 0.4f;
            enemy._target = GameManager.Instance.Player.transform;
            return enemy;
        }

        void FixedUpdate()
        {
            if (_target == null || !_target.gameObject.activeInHierarchy)
            {
                _rb.velocity = Vector2.zero;
                return;
            }

            var direction = ((Vector2)_target.position - (Vector2)transform.position).normalized;
            _rb.velocity = direction * _speed;
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            var playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth == null) return;

            if (Time.time - _lastAttackTime < 0.7f) return;
            _lastAttackTime = Time.time;
            playerHealth.TakeDamage(ContactDamage);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            SfxManager.Instance.Play("hit");
            StartCoroutine(FlashRoutine());

            if (_health <= 0)
                Die();
        }

        void Die()
        {
            GameManager.Instance.AddScore(ScoreReward);
            GameManager.Instance.Waves.NotifyEnemyDied();
            SfxManager.Instance.Play("enemyDie");
            Destroy(gameObject);
        }

        IEnumerator FlashRoutine()
        {
            _sr.color = Color.white;
            yield return new WaitForSeconds(0.08f);
            if (_sr != null)
                _sr.color = _baseColor;
        }
    }
}
