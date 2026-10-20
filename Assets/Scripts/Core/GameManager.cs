using UnityEngine;
using ArenaSurvivor.Combat;
using ArenaSurvivor.Enemies;
using ArenaSurvivor.Player;
using ArenaSurvivor.UI;

namespace ArenaSurvivor.Core
{
    public enum GameState
    {
        Menu,
        Playing,
        Victory,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState State { get; private set; } = GameState.Menu;
        public int Score { get; private set; }
        public int CurrentWave { get; private set; }
        public PlayerHealth Player { get; private set; }
        public WaveManager Waves { get; private set; }
        public UIManager UI { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            ArenaBuilder.Build();
            SetupCamera();
            SfxManager.Create(transform);
            Waves = gameObject.AddComponent<WaveManager>();
            UI = UIManager.Create(this);
            Player = CreatePlayer();
            UI.ShowMenu();
        }

        void SetupCamera()
        {
            var cam = Camera.main;
            if (cam == null)
            {
                var camGo = new GameObject("Main Camera") { tag = "MainCamera" };
                cam = camGo.AddComponent<Camera>();
                camGo.AddComponent<AudioListener>();
            }

            cam.orthographic = true;
            cam.orthographicSize = ArenaBuilder.Height * 0.5f + 1.5f;
            cam.transform.position = new Vector3(0f, 0f, -10f);
            cam.backgroundColor = new Color(0.05f, 0.07f, 0.09f, 1f);
            cam.clearFlags = CameraClearFlags.SolidColor;
        }

        PlayerHealth CreatePlayer()
        {
            var go = new GameObject("Player");
            go.transform.position = Vector3.zero;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = SpriteFactory.Circle;
            sr.color = new Color(0.25f, 0.55f, 1f, 1f);
            sr.sortingOrder = 10;

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.45f;

            var health = go.AddComponent<PlayerHealth>();
            var controller = go.AddComponent<PlayerController>();
            controller.Setup(health);
            go.SetActive(false);
            return health;
        }

        public void StartGame()
        {
            SfxManager.Instance.Play("click");
            ClearLevel();
            Score = 0;
            CurrentWave = 0;
            UI.SetScore(0);
            UI.SetWave(0);
            Player.ResetPlayer();
            State = GameState.Playing;
            UI.ShowHUD();
            Waves.StartWaves();
        }

        public void Victory()
        {
            if (State != GameState.Playing) return;
            State = GameState.Victory;
            ClearLevel();
            SfxManager.Instance.Play("victory");
            UI.ShowVictory(Score);
        }

        public void GameOver()
        {
            if (State != GameState.Playing) return;
            State = GameState.GameOver;
            ClearLevel();
            SfxManager.Instance.Play("gameOver");
            UI.ShowGameOver(Score);
        }

        public void GoToMenu()
        {
            SfxManager.Instance.Play("click");
            State = GameState.Menu;
            ClearLevel();
            UI.ShowMenu();
        }

        public void QuitGame()
        {
            SfxManager.Instance.Play("click");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void AddScore(int amount)
        {
            Score += amount;
            UI.SetScore(Score);
        }

        public void SetWave(int wave)
        {
            CurrentWave = wave;
            UI.SetWave(wave);
        }

        void ClearLevel()
        {
            Waves.StopWaves();

            foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
                Destroy(enemy.gameObject);

            foreach (var projectile in FindObjectsByType<Projectile>(FindObjectsSortMode.None))
                Destroy(projectile.gameObject);

            Player.gameObject.SetActive(false);
        }
    }
}
