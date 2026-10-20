using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ArenaSurvivor.Core;

namespace ArenaSurvivor.UI
{
    public class UIManager : MonoBehaviour
    {
        GameManager _gm;

        Canvas _canvas;
        Font _font;

        GameObject _menuPanel;
        GameObject _hudPanel;
        GameObject _victoryPanel;
        GameObject _gameOverPanel;

        Text _healthText;
        Text _scoreText;
        Text _waveText;
        Text _announcementText;
        Text _victoryScoreText;
        Text _gameOverScoreText;
        Image _healthFill;

        float _announcementEndTime;

        public static UIManager Create(GameManager gm)
        {
            var go = new GameObject("UIManager");
            var ui = go.AddComponent<UIManager>();
            ui.Build(gm);
            return ui;
        }

        public void Build(GameManager gm)
        {
            _gm = gm;
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            var canvasGo = new GameObject("UICanvas");
            canvasGo.transform.SetParent(transform, false);
            _canvas = canvasGo.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasGo.AddComponent<GraphicRaycaster>();

            BuildMenu();
            BuildHud();
            BuildEndPanel(true);
            BuildEndPanel(false);
            BuildAnnouncement();
        }

        void Update()
        {
            if (_announcementText != null && _announcementText.gameObject.activeSelf && Time.unscaledTime >= _announcementEndTime)
                _announcementText.gameObject.SetActive(false);
        }

        public void ShowMenu()
        {
            _menuPanel.SetActive(true);
            _hudPanel.SetActive(false);
            _victoryPanel.SetActive(false);
            _gameOverPanel.SetActive(false);
        }

        public void ShowHUD()
        {
            _menuPanel.SetActive(false);
            _hudPanel.SetActive(true);
            _victoryPanel.SetActive(false);
            _gameOverPanel.SetActive(false);
        }

        public void ShowVictory(int score)
        {
            _menuPanel.SetActive(false);
            _hudPanel.SetActive(false);
            _victoryPanel.SetActive(true);
            _gameOverPanel.SetActive(false);
            _victoryScoreText.text = "Pontuação final: " + score;
        }

        public void ShowGameOver(int score)
        {
            _menuPanel.SetActive(false);
            _hudPanel.SetActive(false);
            _victoryPanel.SetActive(false);
            _gameOverPanel.SetActive(true);
            _gameOverScoreText.text = "Pontuação final: " + score;
        }

        public void SetHealth(int current, int max)
        {
            if (_healthFill != null)
                _healthFill.fillAmount = max > 0 ? (float)current / max : 0f;
            if (_healthText != null)
                _healthText.text = "Vida: " + current + " / " + max;
        }

        public void SetScore(int score)
        {
            if (_scoreText != null)
                _scoreText.text = "Pontos: " + score;
        }

        public void SetWave(int wave)
        {
            if (_waveText != null)
                _waveText.text = wave <= 0 ? "Onda: - / 3" : "Onda: " + wave + " / 3";
        }

        public void ShowAnnouncement(string message, float duration)
        {
            _announcementText.text = message;
            _announcementText.gameObject.SetActive(true);
            _announcementEndTime = Time.unscaledTime + duration;
        }

        void BuildMenu()
        {
            _menuPanel = CreateFullScreenPanel("MenuPanel", new Color(0.03f, 0.05f, 0.08f, 0.95f));

            CreateText(_menuPanel.transform, "ARENA SURVIVOR", 96, new Color(1f, 0.85f, 0.2f),
                new Vector2(0f, 220f), new Vector2(1600f, 120f));
            CreateText(_menuPanel.transform, "Sobreviva às 3 ondas de inimigos", 36, new Color(0.8f, 0.85f, 0.9f),
                new Vector2(0f, 130f), new Vector2(1400f, 60f));

            CreateButton(_menuPanel.transform, "JOGAR", new Vector2(0f, 0f), new Vector2(360f, 90f), _gm.StartGame);
            CreateButton(_menuPanel.transform, "SAIR", new Vector2(0f, -130f), new Vector2(360f, 90f), _gm.QuitGame);

            CreateText(_menuPanel.transform, "Ana Luiza S. S. Duarte  |  Anna Sophia F. Santos  |  Pâmela C. Machado",
                28, new Color(0.55f, 0.6f, 0.65f), new Vector2(0f, -420f), new Vector2(1700f, 50f));
        }

        void BuildHud()
        {
            _hudPanel = CreateFullScreenPanel("HudPanel", new Color(0f, 0f, 0f, 0f));
            _hudPanel.GetComponent<Image>().raycastTarget = false;

            var barBg = CreateImage(_hudPanel.transform, "HealthBarBackground", new Color(0.15f, 0.1f, 0.1f, 0.85f),
                new Vector2(0f, 0f), new Vector2(360f, 40f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            AnchorTopLeft(barBg.rectTransform, 30f, 30f);

            _healthFill = CreateImage(barBg.transform, "HealthFill", new Color(0.25f, 0.85f, 0.3f, 1f),
                Vector2.zero, new Vector2(352f, 32f), Vector2.zero, Vector2.zero);
            var fillRect = _healthFill.rectTransform;
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = new Vector2(4f, 4f);
            fillRect.offsetMax = new Vector2(-4f, -4f);
            _healthFill.type = Image.Type.Filled;
            _healthFill.fillMethod = Image.FillMethod.Horizontal;
            _healthFill.fillAmount = 1f;

            _healthText = CreateText(barBg.transform, "Vida: 100 / 100", 26, Color.white,
                Vector2.zero, new Vector2(352f, 40f));
            var ht = _healthText.rectTransform;
            ht.anchorMin = Vector2.zero;
            ht.anchorMax = Vector2.one;
            ht.offsetMin = Vector2.zero;
            ht.offsetMax = Vector2.zero;

            _waveText = CreateText(_hudPanel.transform, "Onda: - / 3", 40, Color.white,
                new Vector2(0f, 0f), new Vector2(400f, 60f));
            AnchorTopCenter(_waveText.rectTransform, 30f);

            _scoreText = CreateText(_hudPanel.transform, "Pontos: 0", 40, new Color(1f, 0.9f, 0.3f),
                new Vector2(0f, 0f), new Vector2(400f, 60f));
            AnchorTopRight(_scoreText.rectTransform, 30f);
        }

        void BuildEndPanel(bool victory)
        {
            var panel = CreateFullScreenPanel(victory ? "VictoryPanel" : "GameOverPanel",
                victory ? new Color(0.02f, 0.15f, 0.05f, 0.93f) : new Color(0.2f, 0.03f, 0.03f, 0.93f));

            CreateText(panel.transform, victory ? "VITÓRIA!" : "DERROTA", 100,
                victory ? new Color(0.4f, 1f, 0.45f) : new Color(1f, 0.35f, 0.3f),
                new Vector2(0f, 230f), new Vector2(1400f, 130f));

            var scoreText = CreateText(panel.transform, "Pontuação final: 0", 44, Color.white,
                new Vector2(0f, 100f), new Vector2(1200f, 70f));

            CreateButton(panel.transform, "JOGAR NOVAMENTE", new Vector2(0f, -40f), new Vector2(480f, 90f), _gm.StartGame);
            CreateButton(panel.transform, "MENU", new Vector2(0f, -170f), new Vector2(480f, 90f), _gm.GoToMenu);

            if (victory)
                _victoryScoreText = scoreText;
            else
                _gameOverScoreText = scoreText;

            panel.SetActive(false);
        }

        void BuildAnnouncement()
        {
            _announcementText = CreateText(_canvas.transform, "", 72, new Color(1f, 0.9f, 0.3f),
                new Vector2(0f, 60f), new Vector2(1400f, 100f));
            _announcementText.gameObject.SetActive(false);
        }

        GameObject CreateFullScreenPanel(string name, Color color)
        {
            var panel = CreateImage(_canvas.transform, name, color, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
            var rect = panel.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return panel.gameObject;
        }

        Image CreateImage(Transform parent, string name, Color color, Vector2 anchoredPos, Vector2 size,
            Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = color;
            var rect = img.rectTransform;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = size;
            return img;
        }

        Text CreateText(Transform parent, string content, int fontSize, Color color, Vector2 anchoredPos, Vector2 size)
        {
            var go = new GameObject("Text");
            go.transform.SetParent(parent, false);
            var text = go.AddComponent<Text>();
            text.font = _font;
            text.text = content;
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = TextAnchor.MiddleCenter;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            var rect = text.rectTransform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = size;
            return text;
        }

        Button CreateButton(Transform parent, string label, Vector2 anchoredPos, Vector2 size, UnityAction onClick)
        {
            var go = new GameObject(label + "Button");
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = new Color(0.18f, 0.35f, 0.65f, 1f);
            var button = go.AddComponent<Button>();
            var colors = button.colors;
            colors.highlightedColor = new Color(0.3f, 0.5f, 0.85f);
            colors.pressedColor = new Color(0.1f, 0.22f, 0.45f);
            button.colors = colors;
            button.onClick.AddListener(onClick);

            var rect = img.rectTransform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = size;

            var text = CreateText(go.transform, label, 36, Color.white, Vector2.zero, size);
            var textRect = text.rectTransform;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            return button;
        }

        static void AnchorTopLeft(RectTransform rect, float x, float y)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(x, -y);
        }

        static void AnchorTopCenter(RectTransform rect, float y)
        {
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -y);
        }

        static void AnchorTopRight(RectTransform rect, float x, float y)
        {
            rect.anchorMin = new Vector2(1f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(-x, -y);
        }
    }
}
