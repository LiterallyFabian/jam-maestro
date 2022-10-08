using JamMeistro.Jams;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JamMeistro.Game
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void ShowResult(Jam j)
        {
            gameObject.SetActive(true);
            _scoreText.text = j.Score.ToResultText();
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