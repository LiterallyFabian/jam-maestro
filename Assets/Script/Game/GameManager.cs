using System;
using System.Collections;
using JamMeistro.Game;
using JamMeistro.Jams;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public static event Action IsCooking;

    public Jam Jam { get; private set; }
    
    [SerializeField] private AudioClip[] _addIngredientSounds;
    [SerializeField] private AudioClip _boilingSound;
    [SerializeField] private ResultManager _resultManager;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _characterAnimator;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        Jam = ScriptableObject.CreateInstance<Jam>();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        Debug.Log($"Adding a {ingredient.name} to the jam... ({Jam.Ingredients.Count+1} ingredients)");
        Jam.Ingredients.Add(ingredient);

        if (ingredient.UseSound == null || ingredient.UseSound.Length == 0)
            AudioManager.PlayAudio(_addIngredientSounds);
        else
            AudioManager.PlayAudio(ingredient.UseSound);
        
        // TODO: play animation
        
    }

    public void CookJam()
    {
        Debug.Log("Cooking the jam...");
        IsCooking?.Invoke();
        StartCoroutine(Cook());
    }

    private IEnumerator Cook()
    {
        AudioManager.PlayAudio(_boilingSound);
        _animator.Play("Cook");
        _resultManager.ClearResult();

        yield return new WaitForSeconds(9f);

        switch (Jam.Score.Reaction)
        {
            case JamReaction.Heavenly:
                _characterAnimator.Play("CharacterHeavenly");
                break;
            case JamReaction.Good:
                _characterAnimator.Play("CharacterGood");
                break;
            case JamReaction.Neutral:
                _characterAnimator.Play("CharacterNeutral");
                break;
            case JamReaction.Bad:
                _characterAnimator.Play("CharacterBad");
                break;
            case JamReaction.Spicy:
                _characterAnimator.Play("CharacterSpicy");
                break;
            case JamReaction.Sour:
                _characterAnimator.Play("CharacterSour");
                break;
            case JamReaction.Horrible:
                _characterAnimator.Play("CharacterHorrible");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        _resultManager.ShowResult(Jam);
    }
}