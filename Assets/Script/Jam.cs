using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script
{
    [CreateAssetMenu(fileName = "New Jam", menuName = "Jam/Jam")]
    public class Jam : ScriptableObject
    {
        public List<Ingredient> Ingredients = new List<Ingredient>();
        
        public float SugarAmount;
        
        public JamScore Score => new JamScore(this);

        public string GetIngredients()
        {
            IEnumerable<IGrouping<string, Ingredient>> groups = Ingredients.GroupBy(f => f.Name);
            return string.Join(", ", groups.Select(f => f.Count() > 1 ? $"{f.Key} x{f.Count()}" : f.Key));
        }
    }
}