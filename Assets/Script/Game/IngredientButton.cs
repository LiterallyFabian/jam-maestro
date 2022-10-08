using JamMeistro.Jams;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JamMeistro.Game
{
    [RequireComponent(typeof(Button))]
    public class IngredientButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button _button;
        
        [Tooltip("The text should be hidden unless the user hovers this button")]
        private Text _text;
        [SerializeField] private Ingredient _ingredient;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _text = GetComponentInChildren<Text>();

            _text.text = _ingredient.Name;
            _text.enabled = false;
            
            if (_ingredient == null)
                Debug.LogError($"Ingredient is missing on {name}");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _text.enabled = true;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _text.enabled = false;
        }
    }
}