using UnityEngine;

namespace Menu
{
    public class ScoreBoardManager : MonoBehaviour
    {
        public GameObject[] scoreBoardContainers;

        void Awake()
        {
            scoreBoardContainers[0].SetActive(true);

            for(int i = 1; i < scoreBoardContainers.Length; i++)
                scoreBoardContainers[i].SetActive(false);
        }

        public void ShowScoreBoard(int index)
        {
            for (int i = 0; i < scoreBoardContainers.Length; i++)
            {
                if (i == index - 1)
                    scoreBoardContainers[i].SetActive(true);
                else
                    scoreBoardContainers[i].SetActive(false);
            }
        }
    }
}
