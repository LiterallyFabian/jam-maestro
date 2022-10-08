using JamMeistro.Jams;
using TMPro;
using UnityEngine;

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
    }
}