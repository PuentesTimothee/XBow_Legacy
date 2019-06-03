using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class WaveDisplay : MonoBehaviour
    {
        private Text _text;
        
        private void Awake()
        {
            _text = GetComponent<Text>();
        }
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
