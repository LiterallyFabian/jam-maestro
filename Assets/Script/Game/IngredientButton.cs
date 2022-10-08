using System;
using JamMeistro.Effects;
using JamMeistro.Jams;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JamMeistro.Game
{
    [RequireComponent(typeof(Button))]
    public class IngredientButton : GameButton, IPointerEnterHandler, IPointerExitHandler
    {
        private CircleOutline _circleOutline;
        
        [Tooltip("The text should be hidden unless the user hovers this button")]
        private Text _text;
        
        [SerializeField] private Ingredient _ingredient;
        
        private bool _isHovering = false;
        
        private void Start()
        {
            _circleOutline = GetComponent<CircleOutline>();
            _text = GetComponentInChildren<Text>();

            _text.text = _ingredient.Name;
            _text.enabled = false;
            
            if (_ingredient == null)
                Debug.LogError($"Ingredient is missing on {name}");
            
            Button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            GameManager.Instance.AddIngredient(_ingredient);
        }

        private void Update()
        {
            _text.enabled = _isHovering && Button.interactable;
            _circleOutline.enabled = _isHovering && Button.interactable;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovering = true;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovering = false;
        }

        private void OnDrawGizmos()
        {
            if (_ingredient == null)
                return;
            
            name = $"IngredientButton {_ingredient.Name}";
        }
    }
}