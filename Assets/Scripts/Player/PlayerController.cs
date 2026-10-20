using UnityEngine;
using ArenaSurvivor.Combat;
using ArenaSurvivor.Core;

namespace ArenaSurvivor.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float MoveSpeed = 8f;
        public float FireCooldown = 0.28f;

        Rigidbody2D _rb;
        PlayerHealth _health;
        Camera _camera;
        Vector2 _facing = Vector2.right;
        float _nextFireTime;

        public void Setup(PlayerHealth health)
        {
            _health = health;
            _rb = GetComponent<Rigidbody2D>();
            _camera = Camera.main;
        }

        void Update()
        {
            if (_health == null || !_health.IsAlive) return;

            UpdateFacing();

            var wantsShoot = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);
            if (wantsShoot && Time.time >= _nextFireTime)
            {
                _nextFireTime = Time.time + FireCooldown;
                Shoot();
            }
        }

        void FixedUpdate()
        {
            if (_health == null || !_health.IsAlive)
            {
                _rb.velocity = Vector2.zero;
                return;
            }

            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.sqrMagnitude > 1f)
                input.Normalize();

            _rb.velocity = input * MoveSpeed;
            transform.position = ArenaBuilder.ClampToArena(transform.position);
        }

        void UpdateFacing()
        {
            if (Input.GetMouseButton(0))
            {
                var mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
                var dir = (Vector2)mouseWorld - (Vector2)transform.position;
                if (dir.sqrMagnitude > 0.01f)
                    _facing = dir.normalized;
            }
            else
            {
                var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (input.sqrMagnitude > 0.01f)
                    _facing = input.normalized;
            }
        }

        void Shoot()
        {
            var direction = _facing;
            if (Input.GetMouseButton(0))
            {
                var mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
                var dir = (Vector2)mouseWorld - (Vector2)transform.position;
                if (dir.sqrMagnitude > 0.01f)
                    direction = dir.normalized;
            }

            Projectile.Create((Vector2)transform.position + direction * 0.55f, direction);
            SfxManager.Instance.Play("shoot");
        }
    }
}
