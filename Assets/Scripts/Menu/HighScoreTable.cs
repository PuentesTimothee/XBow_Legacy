using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class HighScoreTable : MonoBehaviour
{
    public float templateHeight = 30f;
    public int numberOfEntry = 5;

    public int LevelIndex = 1;
    private readonly string _tableName = "Level_";
    private string Tablename
    {
        get { return (_tableName + LevelIndex); }
    }
    
    public GameObject scoreEntryTemplate;
    private Transform scoreContainer;
    private List<HighScoreEntry> highscoreEntryList;
    private List<GameObject> highscoreEntryListTransform;

    [Serializable]
    private class HighScoreEntry : IComparable
    {
        public int score;

        public int CompareTo(object obj)
        {
            HighScoreEntry scoreToCompare = obj as HighScoreEntry;

            if (scoreToCompare.score < score)
                return -1;
            if (scoreToCompare.score > score)
                return 1;
            return 0;
        }
    }
    
    private class HighScores
    {
        public List<HighScoreEntry> highScoreEntriesList;
    }

    private void Start()
    {
        scoreContainer = transform;
        if (scoreEntryTemplate == null)
            scoreEntryTemplate = scoreContainer.Find("scoreEntryTemplate").gameObject;

        scoreEntryTemplate.gameObject.SetActive(false);
        /*      
      
              // load the list of highscore if there is one
              if (PlayerPrefs.HasKey(Tablename))
              {
                  string jsonString = PlayerPrefs.GetString(Tablename);
                  HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);
                  highscoreEntryList = highscores.highScoreEntriesList;
              }
              else
              {
                  HighScores highscores = new HighScores();
                  string json = JsonUtility.ToJson(highscores);
                  PlayerPrefs.SetString(Tablename, json);
                  PlayerPrefs.Save();
                  highscoreEntryList = highscores.highScoreEntriesList; 
              }
      
              // Sort the list of highscores
              highscoreEntryList.Sort();
              // Show the list of highScore
              highscoreEntryListTransform = new List<Transform>();
      
              for (int i = 0; i < highscoreEntryList.Count && i < numberOfEntry; i++)
              {
                  HighScoreEntry entry = highscoreEntryList[i];
                  createNewEntryTransform(entry, scoreContainer, highscoreEntryListTransform);
              }
      */
        var newScore = ScoreController.Instance[LevelIndex];
        if (newScore != null)
        foreach (var score in newScore)
        {
            createNewEntryTransform(new HighScoreEntry {score = score}, scoreContainer, highscoreEntryListTransform);
        }
    }
    
    private void createNewEntryTransform(HighScoreEntry  entry, Transform container, List<GameObject> list)
    {
        GameObject instantiate = Instantiate(scoreEntryTemplate, container);
        RectTransform entryRecTransform = instantiate.GetComponent<RectTransform>();
        entryRecTransform.anchoredPosition = new Vector2(0, -templateHeight * list.Count);
        instantiate.gameObject.SetActive(true);

        /*
        int rank = list.Count + 1;
        string rankString;

        switch (rank)
        {
            case 1:
                rankString = "1st";
                break;
            case 2:
                rankString = "2nd";
                break;
            case 3:
                rankString = "3rd";
                break;
            default:
                rankString = rank + "th";
                break;
        }

        entryTransform.Find("rankText").GetComponent<Text>().text = rankString;
        */
        instantiate.transform.Find("scoreText").GetComponent<Text>().text = entry.score.ToString();
        //entryTransform.Find("nameText").GetComponent<Text>().text = entry.name;

        list.Add(instantiate);
    }
   
    // Add a new highcores to the table 
    public void SaveHighScoreEntry(int score)
    {
        HighScoreEntry entry = new HighScoreEntry { score = score };
        HighScores table = new HighScores();

        // Load the table
        if (PlayerPrefs.HasKey(Tablename))
        {
            string jsonString = PlayerPrefs.GetString(Tablename);
            table = JsonUtility.FromJson<HighScores>(jsonString);

            table.highScoreEntriesList.Sort();

            // if the score is lesser than the lowest score from the table we don't save it
            if (table.highScoreEntriesList[table.highScoreEntriesList.Count-1].score > entry.score)
            {
                return;
            }
        }

        table.highScoreEntriesList.Add(entry);

        // Save the table 
        string json = JsonUtility.ToJson(table);
        PlayerPrefs.SetString(Tablename, json);
        PlayerPrefs.Save();
    }
}
