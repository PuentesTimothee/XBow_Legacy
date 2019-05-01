using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScoringSystem.Menu
{
    public class HighscoreTable : MonoBehaviour
    {
        public List<HighscoreEntry>    HighscoreEntries;
        public Text                    Title;
        public string                  TitleFormat = "Level {0}";
        private int                    LevelIndexDisplayed = 0;

        #if UNITY_EDITOR
        private void Reset()
        {
            HighscoreEntries = new List<HighscoreEntry>(GetComponentsInChildren<HighscoreEntry>());
        }     
        #endif
        private void Awake()
        {
            Disable();
        }

        public void Disable()
        {
            Title.text = "";
            for (int x = 0; x < HighscoreEntries.Count; x++)
                HighscoreEntries[x].SetActive(false);
        }

        public void ChangedLevelDisplayed(int levelToDisplay)
        {
            LevelIndexDisplayed = levelToDisplay;
            Title.text = String.Format(TitleFormat, LevelIndexDisplayed);
            var highScores = ScoreController.Instance[LevelIndexDisplayed];

            for (int x = 0; x < HighscoreEntries.Count; x++)
            {
                if (x < highScores.entries.Length)
                    HighscoreEntries[x].SetActive(true, highScores.entries[x]);
                else
                    HighscoreEntries[x].SetActive(false);
            }
        }
    }
}
