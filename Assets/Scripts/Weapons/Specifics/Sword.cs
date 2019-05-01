using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Weapons.Specifics
{
    public class Sword : MonoBehaviour
    {
        public float _frequency;
        public float _amplitude;
        public float _duration;
        public SoundPlayOneshot _impactSound;
        private Hand _attachedHand;
        private Interactable _interactable;


        private void Awake()
        {
            _interactable = GetComponentInParent<Interactable>();
            _impactSound = GetComponent<SoundPlayOneshot>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_interactable)
            {
                _interactable.attachedToHand.TriggerHapticPulse(_duration, _frequency, _amplitude);
                if (other.tag != "Sword")
                    _impactSound.Play();
            }
        }

        private void OnColliderEnter(Collision other)
        {
          _interactable.attachedToHand.TriggerHapticPulse(_duration, _frequency, _amplitude);
          if (other.gameObject.tag != "Sword")
            _impactSound.Play();
        }
    }
}
