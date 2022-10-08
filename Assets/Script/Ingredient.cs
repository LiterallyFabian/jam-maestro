using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Jam/Ingredient")]
[JsonObject(MemberSerialization.OptIn)]
public class Ingredient : ScriptableObject
{
    [Header("Basic Info")]
    [Tooltip("The name of the ingredient.")]
    [JsonProperty("name")]
    public string Name = "Ingredient";
    
    [Tooltip("The sprite of the ingredient.")]
    public Sprite Sprite;
    
    [Header("Ingredient Stats")]
    [Range(0, 1)] [Tooltip("How weird the ingredient is. A weirder ingredient is more risky, but will give a higher score if done right. 0 is normal, 1 is very weird.")] 
    [JsonProperty("weirdness")]
    public float Weirdness = 0f;
    
    [Range(-1, 1)] [Tooltip("How sweet the ingredient is. -1 is sour, 0 is neutral, 1 is very sweet.")]
    [JsonProperty("sweetness")]
    public float Sweetness = 0f;
    
    [Range(0, 1)] [Tooltip("How tasty the ingredient is. 0 is bland, 1 is very tasty.")]
    [JsonProperty("tastiness")]
    public float Taste = 0f;
    
    [Range(0, 1)] [Tooltip("How popular the ingredient is. 0 is unpopular, 1 is very popular.")]
    [JsonProperty("popularity")]
    public float Popularity = 0f;
    
    [Range(0, 1)] [Tooltip("The amount of liquid in this ingredent. 0 requires a lot of water, 1 has no demand for extra liquid.")]
    [JsonProperty("liquidness")]
    public float Liquidness = 0f;
    
    [Range(0, 1)] [Tooltip("How spicy the ingredient is. 0 is not spicy at all, 1 is very spicy.")]
    [JsonProperty("spiciness")]
    public float Spiciness = 0f;

    [Tooltip("The color of the ingredient.")]
    public Color Color = Color.white;

    #region Combinations
    [Tooltip("Ingredients that go well with this ingredient.")]
    public Ingredient[] GoodCombinations;
    
    [Tooltip("Ingredients that don't go well with this ingredient.")]
    public Ingredient[] BadCombinations;
    
    [JsonProperty("goodCombinations")]
    public string[] GoodCombinationsNames => GoodCombinations.Select(x => x.Name).ToArray();
    
    [JsonProperty("badCombinations")]
    public string[] BadCombinationsNames => BadCombinations.Select(x => x.Name).ToArray();
    #endregion
    
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}