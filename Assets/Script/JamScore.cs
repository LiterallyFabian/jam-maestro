using System;
using UnityEngine;

namespace Script
{
    public class JamScore
    {
        public Jam Jam { get; }
        public float Tastiness { get; }
        public float Spiciness { get; }
        public float Combination { get; }
        public float Liquidness { get; }
        
        private const float LiquidRequirement = 0.5f;
        
        public float Overall {
            get
            {
                float score = (Tastiness * 3 + Spiciness) * Combination / 3;
                
                // If the jam is too dry, half the score
                if(Liquidness < LiquidRequirement)
                    score /= 2;
                
                return score;
            }
        }

        public JamScore(Jam jam)
        {
            Jam = jam;
            
            // Each jam consists out of one or more ingredients. These ingredients have different values to compare them with.
            // The values are: weirdness, sweetness, taste, popularity, liquidness, spiciness, good combination and bad combination
            // good and bad combination is an array of other ingredients. If the ingredient is in the good combination array, it will add to the score.
            // If it is in the bad combination array, it will have a huge penalty on the score.
            
            float tastiness = 0;
            float spiciness = 0;
            float combination = 0;
            float liquidness = 0;

            float highestSweetness = jam.Ingredients[0].Sweetness;
            float lowestSweetness = jam.Ingredients[0].Sweetness;
            
            foreach (Ingredient ingredient in Jam.Ingredients)
            {
                // Tastiness
                tastiness += ingredient.Popularity * ingredient.Taste;

                if (Mathf.Abs(ingredient.Sweetness - highestSweetness) > 0.5f)
                {
                    tastiness -= 0.5f;
                }
                highestSweetness = Mathf.Max(highestSweetness, ingredient.Sweetness);
                lowestSweetness = Mathf.Min(lowestSweetness, ingredient.Sweetness);

                // Spiciness
                spiciness += ingredient.Spiciness;
                
                // Combination
                foreach (Ingredient goodIngredient in ingredient.GoodCombinations)
                {
                    if (Jam.Ingredients.Contains(goodIngredient))
                        combination += ingredient.Taste * goodIngredient.Taste + 1;
                }
                
                foreach (Ingredient badIngredient in ingredient.BadCombinations)
                {
                    if (Jam.Ingredients.Contains(badIngredient))
                        combination -= 3 * ingredient.Weirdness * ingredient.Taste;
                }
                
                liquidness += ingredient.Liquidness;
            }
            
            Tastiness = tastiness;
            Spiciness = spiciness / Jam.Ingredients.Count;
            Combination = combination;
            Liquidness = liquidness / Jam.Ingredients.Count;
        }
        
        public override string ToString()
        {
            // round all values to 2 decimals
            return $"Tastiness: {Mathf.Round(Tastiness * 100) / 100}\n" +
                   $"Spiciness: {Mathf.Round(Spiciness * 100) / 100}\n" +
                   $"Combination: {Mathf.Round(Combination * 100) / 100}\n" +
                   $"Liquid penalty: {Liquidness < LiquidRequirement}\n" +
                   $"Overall: {Mathf.Round(Overall * 100) / 100}";
        }
    }
}