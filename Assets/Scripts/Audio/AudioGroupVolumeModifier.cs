using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

namespace Audio
{
    public class AudioGroupVolumeModifier : MonoBehaviour
    {
        public AudioGroup    Group;
        public LinearMapping Slider;
        public LinearDrive   Drive;
        public Text          Text;

        private AudioManager _audioManager;
        private float _value;
        
        private void Start()
        {
            _audioManager = AudioManager.Instance;
            UpdateValue(_audioManager.GetGroupVolume(Group));
            Slider.value = _audioManager.GetGroupVolume(Group);
            Drive.RepositionSelf();
        }

        private void Update()
        {
            if (Slider.value != _value)
                UpdateValue(Slider.value);
        }
            
        private void UpdateValue(float value)
        {
            _value = value;
            AudioManager.Instance.SetGroupVolume(Group, _value);
            Text.text = (_value*100f).ToString("N");
        }
    }
}
