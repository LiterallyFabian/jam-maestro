using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Script
{
    [JsonObject(MemberSerialization.OptIn)]
    [CreateAssetMenu(fileName = "New Jam", menuName = "Jam/Jam")]
    public class Jam : ScriptableObject
    {
        public List<Ingredient> Ingredients = new List<Ingredient>();
        
        [JsonProperty("ingredients")]
        public Dictionary<string, int> IngredientCount => Ingredients.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.Count());
        
        [JsonProperty("sugar")]
        public float SugarAmount;
        
        [JsonProperty("score")]
        public JamScore Score => new JamScore(this);

        /// <summary>
        /// Generates a string with all the ingredients in the jam.
        /// </summary>
        public string GetIngredients()
        {
            IEnumerable<IGrouping<string, Ingredient>> groups = Ingredients.GroupBy(f => f.Name);
            return string.Join(", ", groups.Select(f => f.Count() > 1 ? $"{f.Key} x{f.Count()}" : f.Key));
        }
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}