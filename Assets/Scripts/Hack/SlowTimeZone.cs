using System.Collections;
using UnityEngine;
using Weapons;

namespace Hack
{
    public class SlowTimeZone : MonoBehaviour
    {
        public Animator Animator;

        public bool AllowToReturnToSpeed = true;
        public float ZoneLingerTime = 5f;
        
        private readonly int _hashOnCollision = Animator.StringToHash("OnCollision");

        private void OnTriggerEnter(Collider other)
        {
            var projectile = other.GetComponent<Projectile>();
            if (projectile != null)
                projectile.Stop(ZoneLingerTime - (Time.time - _time), AllowToReturnToSpeed);
        }

        private float _time;
        public IEnumerator Start()
        {
            _time = Time.time;
            transform.parent = null;
            Animator.SetTrigger(_hashOnCollision);
            yield return new WaitForSeconds(ZoneLingerTime);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
