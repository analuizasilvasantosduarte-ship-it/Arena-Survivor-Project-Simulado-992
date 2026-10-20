using UnityEngine;

namespace ArenaSurvivor.Core
{
    public static class GameBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Init()
        {
            if (Object.FindFirstObjectByType<GameManager>() != null)
                return;

            var root = new GameObject("GameRoot");
            root.AddComponent<GameManager>();
        }
    }
}
