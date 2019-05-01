using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScoringSystem
{
    public class ScoreController : MonoBehaviour
    {
        #region Variables
        public static ScoreController Instance;

        public PopUpTextController ScoreDisplay;
        public float ComboDuration = 2.0f;

        private int _score;
        private int _levelIndex = -1;
    
        private bool _isComboing;
        private float _comboTimer;
        private int _comboCount;

        private Dictionary<int, HighScores> _scores = new Dictionary<int, HighScores>();
    
        /// <summary>
        /// Get the highScore for a particular level
        /// </summary>
        /// <param name="key">Level index of the level to fetch data from. It's the index specified in the sceneManager</param>
        public HighScores this[int key]
        {
            get
            {
                if (!_scores.ContainsKey(key))
                    LoadHighScore(key);
                return (_scores[key]);
            }
        }
    
        public class HighScores
        {
            public int[] entries;

            public HighScores()
            {
                entries = new int[0];
            }

            public void SortDescending()
            {
                var tmp = entries.ToList();
                tmp.Sort(delegate(int x, int y) { return (x - y); });
                entries = tmp.ToArray();
            }

            public void SortAscending()
            {
                var tmp = entries.ToList();
                tmp.Sort(delegate(int x, int y) { return (y - x); });
                entries = tmp.ToArray();
            }
        }
    
        private readonly string _tableName = "Level_";
        private string TableName(int levelIndex) { return (_tableName + levelIndex); }
        #endregion
    
        #region Methods
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (_isComboing)
            {
                if (_comboTimer < 0)
                {
                    _comboCount = 0;
                    _isComboing = false;
                }
                else
                    _comboTimer -= Time.deltaTime;
            }
        }
    
        #region StartLevel
        public static void StartForLevel(int levelIndex)
        {
            Instance._StartForLevel(levelIndex);
        }
    
        private void _StartForLevel(int levelIndex)
        {
            _levelIndex = levelIndex;
            LoadHighScore(_levelIndex);
            _score = 0;
            _comboCount = 0;
            _isComboing = false;
        }

        private void LoadHighScore(int levelIndex)
        {
            if (!_scores.ContainsKey(levelIndex))
            {
                if (PlayerPrefs.HasKey(TableName(levelIndex)))
                {
                    string jsonString = PlayerPrefs.GetString(TableName(levelIndex));
                    _scores.Add(levelIndex, JsonUtility.FromJson<HighScores>(jsonString));
                    _scores[levelIndex].SortAscending();
                }
                else
                {
                    HighScores highscores = new HighScores();
                    string json = JsonUtility.ToJson(highscores);
                    PlayerPrefs.SetString(TableName(levelIndex), json);
                    PlayerPrefs.Save();
                    _scores.Add(levelIndex, highscores);
                }
            }
        }
        #endregion
    
        #region ExitLevel
        public static void ExitLevel()
        {
            Instance._ExitLevel();
        }

        private void _ExitLevel()
        {
            if (_levelIndex == -1)
                return;
            SaveHighScore(_levelIndex, _score);
            _score = 0;
            _levelIndex = -1;
        }

        private void SaveHighScore(int levelIndex, int score)
        {
            var highScore = _scores[levelIndex];
            var tmp = highScore.entries.ToList();
            tmp.Add(score);
            highScore.entries = tmp.ToArray();
            _scores[levelIndex] = highScore;
            
            _scores[levelIndex].SortAscending();

            string json = JsonUtility.ToJson(_scores[levelIndex]);
            PlayerPrefs.SetString(TableName(levelIndex), json);
            PlayerPrefs.Save();
        }
        #endregion

        public void AddScore(int adding, Transform location)
        {
            _isComboing = true;
            _comboTimer = ComboDuration;
            _comboCount++;
            adding += _comboCount * 100;
            _comboCount++;
            _score += adding;
            //ScoreDisplay.CreatePopupText(adding.ToString(), location);
        }

        #endregion

    }
}
