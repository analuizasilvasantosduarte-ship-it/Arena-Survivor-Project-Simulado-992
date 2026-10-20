using UnityEngine;
using UnityEngine.UI;
using ArenaSurvivor.Core;

namespace ArenaSurvivor.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Paineis")]
        public GameObject menuPanel;
        public GameObject hudPanel;
        public GameObject victoryPanel;
        public GameObject gameOverPanel;

        [Header("HUD")]
        public Text healthText;
        public Image healthFill;
        public Text scoreText;
        public Text waveText;
        public Text announcementText;

        [Header("Telas de fim")]
        public Text victoryScoreText;
        public Text gameOverScoreText;

        [Header("Botoes - Menu")]
        public Button playButton;
        public Button quitButton;

        [Header("Botoes - Vitoria")]
        public Button replayVictoryButton;
        public Button menuVictoryButton;

        [Header("Botoes - Game Over")]
        public Button replayGameOverButton;
        public Button menuGameOverButton;

        float _announcementEndTime;

        public void Bind(GameManager gm)
        {
            BindButton(playButton, gm.StartGame);
            BindButton(quitButton, gm.QuitGame);
            BindButton(replayVictoryButton, gm.StartGame);
            BindButton(menuVictoryButton, gm.GoToMenu);
            BindButton(replayGameOverButton, gm.StartGame);
            BindButton(menuGameOverButton, gm.GoToMenu);
            ShowMenu();
        }

        static void BindButton(Button button, UnityEngine.Events.UnityAction action)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        void Update()
        {
            if (announcementText != null && announcementText.gameObject.activeSelf &&
                Time.unscaledTime >= _announcementEndTime)
            {
                announcementText.gameObject.SetActive(false);
            }
        }

        public void ShowMenu()
        {
            SetActive(menuPanel, true);
            SetActive(hudPanel, false);
            SetActive(victoryPanel, false);
            SetActive(gameOverPanel, false);
        }

        public void ShowHUD()
        {
            SetActive(menuPanel, false);
            SetActive(hudPanel, true);
            SetActive(victoryPanel, false);
            SetActive(gameOverPanel, false);
            HideAnnouncement();
        }

        public void ShowVictory(int score)
        {
            SetActive(menuPanel, false);
            SetActive(hudPanel, false);
            SetActive(victoryPanel, true);
            SetActive(gameOverPanel, false);
            HideAnnouncement();
            if (victoryScoreText != null)
                victoryScoreText.text = "Pontuação final: " + score;
        }

        public void ShowGameOver(int score)
        {
            SetActive(menuPanel, false);
            SetActive(hudPanel, false);
            SetActive(victoryPanel, false);
            SetActive(gameOverPanel, true);
            HideAnnouncement();
            if (gameOverScoreText != null)
                gameOverScoreText.text = "Pontuação final: " + score;
        }

        public void SetHealth(int current, int max)
        {
            if (healthFill != null)
                healthFill.fillAmount = max > 0 ? (float)current / max : 0f;
            if (healthText != null)
                healthText.text = "Vida: " + current + " / " + max;
        }

        public void SetScore(int score)
        {
            if (scoreText != null)
                scoreText.text = "Pontos: " + score;
        }

        public void SetWave(int wave)
        {
            if (waveText != null)
                waveText.text = wave <= 0 ? "Onda: - / 3" : "Onda: " + wave + " / 3";
        }

        public void ShowAnnouncement(string message, float duration)
        {
            if (announcementText == null) return;
            announcementText.text = message;
            announcementText.gameObject.SetActive(true);
            _announcementEndTime = Time.unscaledTime + duration;
        }

        void HideAnnouncement()
        {
            if (announcementText != null)
                announcementText.gameObject.SetActive(false);
        }

        static void SetActive(GameObject go, bool active)
        {
            if (go != null)
                go.SetActive(active);
        }
    }
}
