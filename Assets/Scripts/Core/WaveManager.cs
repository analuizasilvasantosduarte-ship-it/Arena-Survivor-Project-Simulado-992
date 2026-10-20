using System.Collections;
using UnityEngine;
using ArenaSurvivor.Enemies;

namespace ArenaSurvivor.Core
{
    public class WaveManager : MonoBehaviour
    {
        public static readonly int[] WaveSizes = { 5, 8, 12 };

        const float SpawnInterval = 0.7f;
        const float DelayBetweenWaves = 2.5f;
        const float MinSpawnDistanceFromPlayer = 6f;

        Coroutine _routine;
        int _alive;

        public void StartWaves()
        {
            StopWaves();
            _alive = 0;
            _routine = StartCoroutine(RunWaves());
        }

        public void StopWaves()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }
            _alive = 0;
        }

        public void NotifyEnemyDied()
        {
            _alive = Mathf.Max(0, _alive - 1);
        }

        IEnumerator RunWaves()
        {
            var gm = GameManager.Instance;

            for (var i = 0; i < WaveSizes.Length; i++)
            {
                var waveNumber = i + 1;
                gm.SetWave(waveNumber);
                Announce("ONDA " + waveNumber, 1.6f);
                SfxManager.Instance.Play("wave");
                yield return new WaitForSeconds(1.2f);

                for (var e = 0; e < WaveSizes[i]; e++)
                {
                    SpawnEnemy(waveNumber);
                    yield return new WaitForSeconds(SpawnInterval);
                }

                while (_alive > 0)
                    yield return null;

                if (i < WaveSizes.Length - 1)
                {
                    Announce("ONDA " + waveNumber + " ELIMINADA", 1.6f);
                    yield return new WaitForSeconds(DelayBetweenWaves);
                }
            }

            gm.Victory();
        }

        void SpawnEnemy(int waveNumber)
        {
            var position = GetSpawnPosition();
            var enemy = Enemy.Create(position, waveNumber);
            _alive++;
        }

        void Announce(string message, float duration)
        {
            var ui = GameManager.Instance.UI;
            if (ui != null)
                ui.ShowAnnouncement(message, duration);
        }

        Vector2 GetSpawnPosition()
        {
            var bounds = ArenaBuilder.PlayBounds;
            var player = GameManager.Instance.Player;
            Vector2 position;

            for (var attempt = 0; attempt < 20; attempt++)
            {
                var side = Random.Range(0, 4);
                switch (side)
                {
                    case 0: position = new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.min.y + 0.8f); break;
                    case 1: position = new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.max.y - 0.8f); break;
                    case 2: position = new Vector2(bounds.min.x + 0.8f, Random.Range(bounds.min.y, bounds.max.y)); break;
                    default: position = new Vector2(bounds.max.x - 0.8f, Random.Range(bounds.min.y, bounds.max.y)); break;
                }

                if (player == null || Vector2.Distance(position, player.transform.position) >= MinSpawnDistanceFromPlayer)
                    return position;
            }

            return new Vector2(bounds.min.x + 0.8f, bounds.min.y + 0.8f);
        }
    }
}
