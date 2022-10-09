using System;
using JamMeistro.Debugging;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script
{
    public class MainManager : MonoBehaviour
    {
        [SerializeField] private IngredientTest _ingredientTest;
        [SerializeField] private Button _buttonOptOut;
        [SerializeField] private InputField _inputField;
        [SerializeField] private GameObject _helpPanel;

        private void Awake()
        {
            _inputField.text = PlayerPrefs.GetString("Name", "");
            
            // config music
            if (GameObject.FindGameObjectWithTag("Music") == null)
            {
                GameObject music = new GameObject();
                DontDestroyOnLoad(music);
                music.tag = "Music";
                AudioSource audio = music.AddComponent<AudioSource>();
                audio.clip = Resources.Load<AudioClip>("Music");
                audio.loop = true;
                audio.volume = 0.25f;
                audio.Play();
            }
        }

        private void Update()
        {
            _buttonOptOut.interactable = _inputField.text.Length > 0;
        }

        public void OpenHelp()
        {
            _helpPanel.SetActive(true);
        }

        public void CloseHelp()
        {
            _helpPanel.SetActive(false);
        }

        public void Play()
        {
            PlayerPrefs.SetString("Name", _inputField.text);
            SceneManager.LoadScene("Game");
        }

        public void OptOut()
        {
            _inputField.text = "";
        }

        public void RawData()
        {
            _ingredientTest.GenerateIngredientJson();
        }
        
        public void OpenLeaderboards()
        {
            Application.OpenURL("https://jam.sajber.me");
        }

        public void OpenFAQ()
        {
            Application.OpenURL("https://jam.sajber.me/faq");
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}