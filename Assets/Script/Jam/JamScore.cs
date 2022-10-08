using System;
using System.Collections.Generic;
using JamMeistro.Game;
using Newtonsoft.Json;
using UnityEngine;
// ReSharper disable ArrangeRedundantParentheses

namespace JamMeistro.Jams
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JamScore
    {
        #region Constants
        private const float LiquidRequirement = 0.5f;
        #endregion
        
        #region Fields
        public Jam Jam { get; }
        
        /// <summary>
        /// A score representing how tasty this jam is on average.
        /// </summary>
        [JsonProperty("taste")]
        public float Tastiness { get; }
        
        /// <summary>
        /// A score representing how spicy this jam is on average.
        /// </summary>
        [JsonProperty("spiciness")]
        public float Spiciness { get; }
        
        /// <summary>
        /// A score indicating how well this jam is balanced.
        /// </summary>
        [JsonProperty("combination")]
        public float Combination { get; }
        
        /// <summary>
        /// A jam indicating how much of the jam is liquid.
        /// </summary>
        [JsonProperty("liquidness")]
        public float Liquidness { get; }
        
        [JsonProperty("sourness")]
        public float Sourness { get; }

        [JsonProperty("overall")]
        public float Overall { get; }
        
        [JsonProperty("feedback")]
        public List<string> Feedback { get; } = new List<string>();

        [JsonProperty("reaction")]
        public JamReaction Reaction => GetReaction();
        
        #endregion

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
            float sourness = 0;

            float highestSweetness = jam.Ingredients[0].Sweetness;
            float lowestSweetness = jam.Ingredients[0].Sweetness;
            
            foreach (Ingredient ingredient in Jam.Ingredients)
            {
                // Tastiness
                tastiness += ingredient.Popularity * ingredient.Taste;
                liquidness += ingredient.Liquidness;
                sourness += ingredient.Sweetness*-1;

                if (Mathf.Abs(ingredient.Sweetness - highestSweetness) > 0.5f)
                {
                    tastiness -= 0.5f;
                }
                highestSweetness = Mathf.Max(highestSweetness, ingredient.Sweetness);
                lowestSweetness = Mathf.Min(lowestSweetness, ingredient.Sweetness);

                // Spiciness
                spiciness += ingredient.Spiciness;
                
                // Calculate good combination score
                foreach (Ingredient goodIngredient in ingredient.GoodCombinations)
                {
                    if (Jam.Ingredients.Contains(goodIngredient))
                    {
                        combination += (ingredient.Taste * goodIngredient.Taste) * 3 + ((1-ingredient.Popularity)*5);
                        tastiness += 0.5f * ingredient.Taste;
                        AddFeedback($"{ingredient.Name} and {goodIngredient.Name} is a good combination! Yum!");
                        
                        // if a sour ingredient is in a good combination, it will add to the taste
                        if (goodIngredient.Sweetness < 0)
                            tastiness += goodIngredient.Sweetness*-1;
                        
                        // bonus for using weird ingredients well, scaling exponentially
                        tastiness += Mathf.Pow(goodIngredient.Weirdness, 2) * 4;
                    }
                }
                
                // Calculate bad combination score
                foreach (Ingredient badIngredient in ingredient.BadCombinations)
                {
                    if (Jam.Ingredients.Contains(badIngredient))
                    {
                        combination -= 3 * ingredient.Weirdness * ingredient.Taste;
                        tastiness -= 0.5f * ingredient.Taste;
                        AddFeedback($"I didn't like to have {badIngredient.Name} and {ingredient.Name} in the same jar.");
                        
                        // if a weird ingredient is used badly, it will exponentially reduce the taste
                        tastiness -= Mathf.Pow(badIngredient.Weirdness, 2) * 8;
                    }
                }
                
                // if this ingredient appears more than 3 times, it will have a penalty on the score
                int amountsOfThis = Jam.Ingredients.FindAll(i => i == ingredient).Count;
                if (amountsOfThis > 3)
                {
                    combination -= Mathf.Pow(ingredient.Taste, 2) * (amountsOfThis * 2);
                    AddFeedback($"You used too much {ingredient.Name}!");
                }
            }

            Sourness = sourness / Jam.Ingredients.Count;
            Tastiness = tastiness / Jam.Ingredients.Count;
            Spiciness = spiciness / Jam.Ingredients.Count;
            Combination = combination / Jam.Ingredients.Count;
            Liquidness = liquidness / Jam.Ingredients.Count;
            Overall = CalculateOverall();
        }
        
        private float CalculateOverall()
        {
            float score = (Tastiness * 3) + (Combination * 3);
            
            // If the jam is too dry or too liquid, it will have a penalty on the score
            if (Liquidness < 0.5f)
            {
                score /= 2;
                AddFeedback("This jam is too dry!");
            }
            else if (Liquidness > 1.1f)
            {
                score /= 2;
                AddFeedback("This jam is too liquid!");
            }

            // If the jam contains 1-2 ingredients, divide by missing ingredients
            if (Jam.Ingredients.Count < 3)
            {
                score /= 4 - Jam.Ingredients.Count;
                score -= 0.5f;
                AddFeedback("You should add more ingredients to your jam.");
            }
            
            // If the jam is too spicy, divide by 2
            if (Spiciness > 0.5f)
            {
                score /= 2;
                AddFeedback("This jam is too spicy!");
            }
            
            // If the jam is too sour, divide by 2
            if (Sourness > 0.5f)
            {
                score /= 2;
                AddFeedback("This jam is too sour!");
            }
            
            return score;
        }
        
        public override string ToString()
        {
            // round all values to 2 decimals
            return $"Tastiness: {Mathf.Round(Tastiness * 100) / 100}\n" +
                   $"Spiciness: {Mathf.Round(Spiciness * 100) / 100}\n" +
                   $"Sourness: {Mathf.Round(Sourness * 100) / 100}\n" +
                   $"Combination: {Mathf.Round(Combination * 100) / 100}\n" +
                   $"Liquidness: {Mathf.Round(Liquidness * 100) / 100}\n" +
                   $"Reaction: {Reaction}\n" +
                   $"Overall: {Mathf.Round(Overall * 100) / 100}";
        }

        private void AddFeedback(string s)
        {
            if (!Feedback.Contains(s))
            {
                Feedback.Add(s);
            }
        }
        
        private JamReaction GetReaction()
        {
            if (Spiciness > 0.5f)
                return JamReaction.Spicy;
            if (Sourness > 0.9f)
                return JamReaction.Sour;
            if (Overall > 5)
                return JamReaction.Heavenly;
            if (Overall > 2)
                return JamReaction.Good;
            if (Overall > 0)
                return JamReaction.Neutral;
            if (Overall > -4)
                return JamReaction.Bad;
            
            return JamReaction.Horrible;
        }

        public string ToResultText()
        {
            string result = $"<b>{Reaction.ToString().ToUpper()}</b>\n";
            result += Reaction switch
            {
                JamReaction.Heavenly => "Your jam is amazing!",
                JamReaction.Good => "Your jam is good!",
                JamReaction.Neutral => "Your jam is okay.",
                JamReaction.Bad => "Your jam is bad.",
                JamReaction.Horrible => "Your jam is horrible!",
                JamReaction.Spicy => "Your jam is too spicy!",
                JamReaction.Sour => "Your jam is too sour!",
                _ => throw new ArgumentOutOfRangeException(),
            };
            
            result += $"\n<size=40>Tastiness: {Math.Round(Tastiness, 2)}\n" +
                      $"Combination: {Math.Round(Combination, 2)}\n" +
                      $"Overall: {Math.Round(Overall, 2)}</size>";
            
            return result;
        }
    }
}