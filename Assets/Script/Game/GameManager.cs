using System;
using System.Collections;
using JamMeistro.Game;
using JamMeistro.Jams;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject _progressPrefab;
    [SerializeField] private Transform _progressParent;

    [Header("Result sound effects")] [SerializeField]
    private AudioClip _drumRoll;

    [SerializeField] private AudioClip _reactionHeavenly;
    [SerializeField] private AudioClip _reactionGood;
    [SerializeField] private AudioClip _reactionNeutral;
    [SerializeField] private AudioClip _reactionBad;
    [SerializeField] private AudioClip _reactionHorrible;
    [SerializeField] private AudioClip _reactionSpicy;
    [SerializeField] private AudioClip _reactionSour;
    
    
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
        
        GameObject icon = Instantiate(_progressPrefab, _progressParent);
        icon.GetComponent<Image>().sprite = ingredient.Sprite;
        
        // TODO: play animation
        
    }

    public void CookJam()
    {
        Debug.Log("Cooking the jam...");
        AudioManager.PlayAudio(_boilingSound).volume = 0.4f;

        IsCooking?.Invoke();
        StartCoroutine(Cook());
    }

    private IEnumerator Cook()
    {
        _animator.Play("Cook");
        _resultManager.ClearResult();
        yield return new WaitForSeconds(3f);
        AudioManager.PlayAudio(_drumRoll);
        yield return new WaitForSeconds(5f);

        switch (Jam.Score.Reaction)
        {
            case JamReaction.Heavenly:
                _characterAnimator.Play("CharacterHeavenly");
                AudioManager.PlayAudio(_reactionHeavenly);
                break;
            case JamReaction.Good:
                _characterAnimator.Play("CharacterGood");
                AudioManager.PlayAudio(_reactionGood);
                break;
            case JamReaction.Neutral:
                _characterAnimator.Play("CharacterNeutral");
                AudioManager.PlayAudio(_reactionNeutral);
                break;
            case JamReaction.Bad:
                _characterAnimator.Play("CharacterBad");
                AudioManager.PlayAudio(_reactionBad);
                break;
            case JamReaction.Spicy:
                _characterAnimator.Play("CharacterSpicy");
                AudioManager.PlayAudio(_reactionSpicy);
                break;
            case JamReaction.Sour:
                _characterAnimator.Play("CharacterSour");
                AudioManager.PlayAudio(_reactionSour);
                break;
            case JamReaction.Horrible:
                _characterAnimator.Play("CharacterHorrible");
                AudioManager.PlayAudio(_reactionHorrible);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        _resultManager.ShowResult(Jam);
    }
    
    public void GoToMain()
    {
        SceneManager.LoadScene("Main");
    }
}