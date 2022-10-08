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
        yield return new WaitForSeconds(5f);
        Debug.Log(Jam.Score);
    }
}