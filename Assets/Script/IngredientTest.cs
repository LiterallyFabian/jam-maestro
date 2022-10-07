using System;
using UnityEngine;

namespace Script
{
    public class IngredientTest : MonoBehaviour
    {

        private void Awake()
        {
            // get all ingredients from Resources
            Ingredient[] ingredients = Resources.LoadAll<Ingredient>("Ingredients");
            
            // order ingredients by property
            Array.Sort(ingredients, (x, y) => y.Sweetness.CompareTo(x.Sweetness));
            
            // print ingredients
            string output = "";
            foreach (Ingredient ingredient in ingredients)
            {
                output += ingredient.Sweetness + " " + ingredient.name + "\n";
            }
            
            Debug.Log(output);
        }
    }
}