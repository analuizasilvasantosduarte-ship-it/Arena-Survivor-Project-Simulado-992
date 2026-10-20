using UnityEngine;

namespace ArenaSurvivor.Core
{
    public static class ArenaBuilder
    {
        public const float Width = 28f;
        public const float Height = 16f;

        public static Bounds PlayBounds => new Bounds(Vector3.zero, new Vector3(Width, Height, 0f));

        public static Vector3 ClampToArena(Vector3 position)
        {
            var bounds = PlayBounds;
            position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
            position.y = Mathf.Clamp(position.y, bounds.min.y, bounds.max.y);
            return position;
        }

        public static void Build()
        {
            var background = new GameObject("ArenaBackground").AddComponent<SpriteRenderer>();
            background.gameObject.transform.position = new Vector3(0f, 0f, 2f);
            background.gameObject.transform.localScale = new Vector3(Width + 2f, Height + 2f, 1f);
            background.sprite = SpriteFactory.Square;
            background.color = new Color(0.13f, 0.22f, 0.14f, 1f);
            background.sortingOrder = -100;

            var border = new Color(0.25f, 0.28f, 0.32f, 1f);
            float t = 1f;

            CreateBorderPart("WallTop", new Vector3(0f, Height * 0.5f + t * 0.5f, 1f), new Vector2(Width + t * 2f, t), border);
            CreateBorderPart("WallBottom", new Vector3(0f, -Height * 0.5f - t * 0.5f, 1f), new Vector2(Width + t * 2f, t), border);
            CreateBorderPart("WallLeft", new Vector3(-Width * 0.5f - t * 0.5f, 0f, 1f), new Vector2(t, Height), border);
            CreateBorderPart("WallRight", new Vector3(Width * 0.5f + t * 0.5f, 0f, 1f), new Vector2(t, Height), border);

            CreateWallCollider("BoundsTop", new Vector2(Width + t * 4f, t), new Vector2(0f, Height * 0.5f + t * 0.5f));
            CreateWallCollider("BoundsBottom", new Vector2(Width + t * 4f, t), new Vector2(0f, -Height * 0.5f - t * 0.5f));
            CreateWallCollider("BoundsLeft", new Vector2(t, Height + t * 4f), new Vector2(-Width * 0.5f - t * 0.5f, 0f));
            CreateWallCollider("BoundsRight", new Vector2(t, Height + t * 4f), new Vector2(Width * 0.5f + t * 0.5f, 0f));
        }

        static void CreateBorderPart(string name, Vector3 position, Vector2 scale, Color color)
        {
            var sr = SpriteFactory.CreateSprite(name, SpriteFactory.Square, position, scale, color, -90);
            sr.gameObject.transform.SetParent(null);
        }

        static void CreateWallCollider(string name, Vector2 size, Vector2 position)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            go.AddComponent<ArenaWall>();
            var box = go.AddComponent<BoxCollider2D>();
            box.size = size;
        }
    }

    public class ArenaWall : MonoBehaviour
    {
    }
}
