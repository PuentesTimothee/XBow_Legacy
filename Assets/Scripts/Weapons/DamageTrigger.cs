using Enemy;
using UnityEngine;

namespace Weapons
{
    public class DamageTrigger : MonoBehaviour
    {
        public float Damage = 10f;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Hit " + other.name);
            var hittable = other.GetComponent<Hittable>();
            if (hittable != null)
                hittable.Hit(Damage, transform.position);
        }
    }
}
