using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public enum AudioGroup
    {
        Master = 1,
        SFX = 2,
        Music = 3
    }
    
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
    
        public AudioMixer Mixer;
        
        private readonly Dictionary<AudioGroup, float> _groupsVolume = new Dictionary<AudioGroup, float> { { AudioGroup.Master, 0 }, { AudioGroup.SFX, 0}, { AudioGroup.Music, 0} };
        private readonly string _volumeSuffix = "_Volume";

        private void Awake()
        {
            Instance = this;
            LoadGroupsVolume();
        }

        private static void SetMixerVolume0To1(AudioMixer mixer, string key, float value)
        {
            mixer.SetFloat(key, (value * 80) - 80);
        }
        
        private void LoadGroupVolume(AudioGroup audioGroup)
        {
            string key = audioGroup + _volumeSuffix;
            _groupsVolume[audioGroup] = PlayerPrefs.GetFloat(key);
            SetMixerVolume0To1(Mixer, key, _groupsVolume[audioGroup]);
        }
        
        public void LoadGroupsVolume()
        {
            LoadGroupVolume(AudioGroup.Master);
            LoadGroupVolume(AudioGroup.SFX);
            LoadGroupVolume(AudioGroup.Music);
        }

        public void SetGroupVolume(AudioGroup audioGroup, float value)
        {
            _groupsVolume[audioGroup] = value;
            string key = audioGroup + _volumeSuffix;
            PlayerPrefs.SetFloat(key, value);
            SetMixerVolume0To1(Mixer, key, value);
        }
        
        public float GetGroupVolume(AudioGroup audioGroup)
        {
            return (_groupsVolume[audioGroup]);
        }
    }
}
