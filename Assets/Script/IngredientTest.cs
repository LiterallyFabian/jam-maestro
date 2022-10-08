using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Script
{
    public class IngredientTest : MonoBehaviour
    {
        private Ingredient[] _ingredients;
        private void Awake()
        {
             _ingredients = Resources.LoadAll<Ingredient>("Ingredients");
        }

        private void PrintValues()
        {
            // order ingredients by property
            Array.Sort(_ingredients, (x, y) => y.Sweetness.CompareTo(x.Sweetness));
            
            // print ingredients
            string output = "";
            foreach (Ingredient ingredient in _ingredients)
            {
                output += ingredient.Sweetness + " " + ingredient.name + "\n";
            }
            
            Debug.Log(output);
        }

        private void GenerateIngredientJson()
        {
            string output = JsonConvert.SerializeObject(_ingredients, Formatting.Indented);
            
            string path = Application.temporaryCachePath + "/ingredients.json";
            File.WriteAllText(path, output);
            Process.Start("notepad.exe", path);
        }
    }
}