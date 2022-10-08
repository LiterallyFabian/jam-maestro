using UnityEngine;
using UnityEngine.UI;

namespace JamMeistro.Game
{
    /// <summary>
    /// A game button will disable itself when the cooking phase has started.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class GameButton : MonoBehaviour
    {
        internal Button Button;
        
        private void Awake()
        {
            Button = GetComponent<Button>();
            GameManager.IsCooking += OnCookingStarted;
        }
        
        private void OnCookingStarted()
        {
            Button.interactable = false;
        }
        
        private void OnDestroy()
        {
            GameManager.IsCooking -= OnCookingStarted;
        }
    }
}