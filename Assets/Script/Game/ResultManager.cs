using JamMeistro.Jams;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JamMeistro.Game
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private GameObject _buttons;
        [SerializeField] private Button _leaderboardButton;

        private void Start()
        {
            if (PlayerPrefs.GetString("Name", "").Length == 0)
            {
                _leaderboardButton.GetComponentInChildren<Text>().text = "Join leaderboard";
                _leaderboardButton.onClick = new Button.ButtonClickedEvent();
                _leaderboardButton.onClick.AddListener(() => SceneManager.LoadScene("Main"));
            }
        }

        public void ShowResult(Jam j, string pos)
        {
            gameObject.SetActive(true);
            _scoreText.text = j.Score.ToResultText(pos);
            _buttons.SetActive(true);
        }
        
        public void ClearResult()
        {
            _scoreText.text = "<size=55>Tasting...</size>";
            _buttons.SetActive(false);
        }

        public void NewGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void OpenLeaderboard()
        {
            Application.OpenURL("https://jam.sajber.me");
        }
    }
}