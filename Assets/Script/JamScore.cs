using System;
using UnityEngine;
// ReSharper disable ArrangeRedundantParentheses

namespace Script
{
    public class JamScore
    {
        #region Constants
        private const float LiquidRequirement = 0.5f;
        #endregion
        
        public Jam Jam { get; }
        
        /// <summary>
        /// A score representing how tasty this jam is on average.
        /// </summary>
        public float Tastiness { get; }
        
        /// <summary>
        /// A score representing how spicy this jam is on average.
        /// </summary>
        public float Spiciness { get; }
        
        /// <summary>
        /// A score indicating how well this jam is balanced.
        /// </summary>
        public float Combination { get; }
        
        /// <summary>
        /// A jam indicating how much of the jam is liquid.
        /// </summary>
        public float Liquidness { get; }
        
        /// <summary>
        /// Whether or not a liquid penalty has been applied to this score.
        /// </summary>
        public bool LiquidPenalty => Liquidness < LiquidRequirement;
        
        public float Overall { get; }

        public JamScore(Jam jam)
        {
            Jam = jam;
            
            // Each jam consists out of one or more ingredients. These ingredients have different values to compare them with.
            // The values are: weirdness, sweetness, taste, popularity, liquidness, spiciness, good combination and bad combination
            // good and bad combination is an array of other ingredients. If the ingredient is in the good combination array, it will add to the score.
            // If it is in the bad combination array, it will have a huge penalty on the score.
            
            float tastiness = 0;
            float spiciness = 0;
            float combination = 1;
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
                        combination += (ingredient.Taste * goodIngredient.Taste) * 3 + (1-ingredient.Popularity*5);
                }
                
                foreach (Ingredient badIngredient in ingredient.BadCombinations)
                {
                    if (Jam.Ingredients.Contains(badIngredient))
                        combination -= 3 * ingredient.Weirdness * ingredient.Taste;
                }
                
                liquidness += ingredient.Liquidness;
                
                // if this ingredient appears more than 3 times, it will have a penalty on the score
                if (Jam.Ingredients.FindAll(i => i == ingredient).Count > 3)
                {
                    combination -= 1;
                }
            }

            Tastiness = tastiness / Jam.Ingredients.Count;
            Spiciness = spiciness / Jam.Ingredients.Count;
            Combination = 1 + ((combination - 1) / Jam.Ingredients.Count);
            Liquidness = liquidness / Jam.Ingredients.Count;
            Overall = CalculateOverall();
        }
        
        private float CalculateOverall()
        {
            float score = ((Tastiness * 3) + Spiciness / 3) + (Combination * 2);

            // If the jam is too dry, half the score
            if(Liquidness < LiquidRequirement)
                score /= 2;
                
            // If the jam contains 1-2 ingredients, divide by missing ingredients
            if(Jam.Ingredients.Count < 3)
                score /= 4 - Jam.Ingredients.Count;

            return score;
        }
        
        public override string ToString()
        {
            // round all values to 2 decimals
            return $"Tastiness: {Mathf.Round(Tastiness * 100) / 100}\n" +
                   $"Spiciness: {Mathf.Round(Spiciness * 100) / 100}\n" +
                   $"Combination: {Mathf.Round(Combination * 100) / 100}\n" +
                   $"Liquid penalty: {LiquidPenalty}\n" +
                   $"Overall: {Mathf.Round(Overall * 100) / 100}";
        }
    }
}