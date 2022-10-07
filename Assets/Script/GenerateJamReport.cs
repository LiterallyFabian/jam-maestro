using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Script
{
    public class GenerateJamReport : MonoBehaviour
    {
        private void Start()
        {
            Jam[] jams = Resources.LoadAll<Jam>("Jams");
            
            Array.Sort(jams, (a, b) => b.Score.Overall.CompareTo(a.Score.Overall));
            
            string text = "";
            for (int i = 0; i < jams.Length; i++)
            {
                Jam jam = jams[i];
                text+="========================================\n";
                text += $"**Jam {i + 1}: {jam.name}** \n";
                text += jam.GetIngredients() + "\n\n";
                text += jam.Score + "\n\nFeedback:\n";
                text += string.Join("\n", jam.Score.Feedback.ToArray());
                text+="\n========================================\n";
            }

            // print text to temp file
            string path = Application.temporaryCachePath + "/jam_report.txt";
            File.WriteAllText(path, text);
            Process.Start("notepad.exe", path);
        }
    }
}