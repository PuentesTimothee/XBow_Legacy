using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    static public ScoreController Instance;

    public PopUpTextController ScoreDisplay;
    public float ComboDuration = 2.0f;

    private int _score;
    private int _levelIndex = -1;
    
    private bool _isComboing;
    private float _comboTimer;
    private int _comboCount;

    private Dictionary<int, List<int>> _scores = new Dictionary<int, List<int>>();
    
    public List<int> this[int key]
    {
        get {if (_scores.ContainsKey(key)) return (_scores[key]); return (null); }
    }
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

    public static void StartForLevel(int levelIndex)
    {
        Instance._StartForLevel(levelIndex);
    }
    
    private void _StartForLevel(int levelIndex)
    {
        _levelIndex = levelIndex;
        if (!_scores.ContainsKey(_levelIndex))
            _scores[_levelIndex] = new List<int>();

        _score = 0;
        _comboCount = 0;
        _isComboing = false;
        _score = 0;
    }

    public static void ExitLevel()
    {
        Instance._ExitLevel();
    }

    private void _ExitLevel()
    {
        if (_levelIndex == -1)
            return;
        _scores[_levelIndex].Add(_score);
        _score = 0;
        _levelIndex = -1;
    }
    
    public void AddScore(int adding, Transform location)
    {
        _isComboing = true;
        _comboTimer = ComboDuration;
        _comboCount++;
        adding += _comboCount * 100;
        _comboCount++;
        _score += adding;
        ScoreDisplay.CreatePopupText(adding.ToString(), location);
    }
}
