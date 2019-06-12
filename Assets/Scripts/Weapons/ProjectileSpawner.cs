using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class ProjectileSpawner : MonoBehaviour
    {
        public float Speed;
        public GameObject Prefab;
        public float DelayBeforeFire = 5f;
        private int count = 0;
        
        private IEnumerator Start()
        {
            while (true)
            {
                var Instiated = Instantiate(Prefab);
                Instiated.name = "Arrow " + count;
                count++;
                Instiated.transform.position = transform.position;
                Instiated.transform.rotation = transform.rotation;
                var projectile = Instiated.GetComponent<Projectile>();

                yield return new WaitForSeconds(DelayBeforeFire);
                projectile.Fire(transform.forward * Speed);
                yield return new WaitForSeconds(0.5f);

            }
        }
    }
}
