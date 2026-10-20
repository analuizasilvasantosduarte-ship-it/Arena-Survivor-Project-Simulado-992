using System.Collections;
using UnityEngine;
using ArenaSurvivor.Core;

namespace ArenaSurvivor.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public int MaxHealth { get; private set; } = 100;
        public int Current { get; private set; }
        public bool IsAlive => Current > 0 && gameObject.activeInHierarchy;

        const float InvulnerabilityTime = 0.6f;

        float _lastDamageTime = -99f;
        SpriteRenderer _sr;
        Color _baseColor;

        void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            if (_sr != null)
                _baseColor = _sr.color;
            Current = MaxHealth;
        }

        public void ResetPlayer()
        {
            Current = MaxHealth;
            _lastDamageTime = -99f;
            transform.position = Vector3.zero;
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = Vector2.zero;
            gameObject.SetActive(true);
            var ui = GameManager.Instance.UI;
            if (ui != null)
                ui.SetHealth(Current, MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (!IsAlive) return;
            if (Time.time - _lastDamageTime < InvulnerabilityTime) return;

            _lastDamageTime = Time.time;
            Current = Mathf.Max(0, Current - damage);
            var ui = GameManager.Instance.UI;
            if (ui != null)
                ui.SetHealth(Current, MaxHealth);
            SfxManager.Instance.Play("playerHurt");
            StartCoroutine(FlashRoutine());

            if (Current <= 0)
                Die();
        }

        void Die()
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = Vector2.zero;
            GameManager.Instance.GameOver();
        }

        IEnumerator FlashRoutine()
        {
            if (_sr == null) yield break;
            _sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            if (_sr != null)
                _sr.color = _baseColor;
        }
    }
}
