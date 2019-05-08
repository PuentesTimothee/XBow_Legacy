using System.Collections;
using UnityEngine;

namespace VisualEffect
{
    public class DestroyEffectAfter : MonoBehaviour
    {
        public float EffectLength = 1f;
        public bool DestroyWhenEnd = true;

        private void OnEnable()
        {
            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            yield return new WaitForSeconds(EffectLength);
            if (DestroyWhenEnd)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            StopCoroutine(StartCountdown());
        }
    }
}
