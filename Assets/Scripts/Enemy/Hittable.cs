using UnityEngine;

namespace Enemy
{
    public abstract class Hittable : MonoBehaviour
    {
        public abstract void Hit(float damage, Vector3 position);
    }
}
