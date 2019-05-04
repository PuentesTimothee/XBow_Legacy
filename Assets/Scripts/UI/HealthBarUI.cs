using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBarUI : MonoBehaviour
    {
        public HealthBar HealthBar;
        public Image     Image;
        
        private void Update()
        {
            Image.fillAmount = HealthBar.HealthPercentage;
        }
    }
}
