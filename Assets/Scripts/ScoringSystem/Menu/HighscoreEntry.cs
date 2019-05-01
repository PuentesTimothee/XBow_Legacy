using UnityEngine;
using UnityEngine.UI;

namespace ScoringSystem.Menu
{
    public class HighscoreEntry : MonoBehaviour
    {
        [SerializeField] private Text Score;

        public void SetActive(bool val, int score = 0)
        {
            gameObject.SetActive(val);
            Score.text = score + " point";
            if (score > 1)
                Score.text += "s";
        }
    }
}
