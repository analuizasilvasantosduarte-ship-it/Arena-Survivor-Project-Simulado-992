using UnityEngine;

namespace ArenaSurvivor.Core
{
    public static class SpriteFactory
    {
        const int Size = 64;

        static Sprite _square;
        static Sprite _circle;

        public static Sprite Square
        {
            get
            {
                if (_square == null) _square = Create(false);
                return _square;
            }
        }

        public static Sprite Circle
        {
            get
            {
                if (_circle == null) _circle = Create(true);
                return _circle;
            }
        }

        static Sprite Create(bool circle)
        {
            var tex = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            var half = Size * 0.5f;
            var radius = half - 1f;

            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    var alpha = 255;
                    if (circle)
                    {
                        var dx = x - half + 0.5f;
                        var dy = y - half + 0.5f;
                        var dist = Mathf.Sqrt(dx * dx + dy * dy);
                        alpha = dist <= radius ? 255 : 0;
                    }
                    tex.SetPixel(x, y, new Color(1f, 1f, 1f, alpha / 255f));
                }
            }

            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, Size, Size), new Vector2(0.5f, 0.5f), Size);
        }

        public static SpriteRenderer CreateSprite(string name, Sprite sprite, Vector3 position, Vector2 scale, Color color, int sortingOrder = 0)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            go.transform.localScale = new Vector3(scale.x, scale.y, 1f);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = color;
            sr.sortingOrder = sortingOrder;
            return sr;
        }
    }
}
