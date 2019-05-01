using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class ProjectileSpawner : MonoBehaviour
    {
        public float Speed;
        public GameObject Prefab;
        public float DelayBeforeFire = 5f;

        private IEnumerator Start()
        {
            while (true)
            {
                var Instiated = Instantiate(Prefab);
                Instiated.transform.position = transform.position;
                Instiated.transform.rotation = transform.rotation;
                var projectile = Instiated.GetComponent<Projectile>();

                yield return new WaitForSeconds(DelayBeforeFire);
                projectile.Fire(transform.forward * Speed);
            }
        }
    }
}
