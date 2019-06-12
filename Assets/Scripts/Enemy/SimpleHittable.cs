using UnityEngine;

namespace Enemy
{
    public class SimpleHittable : Hittable
    {
        public ParticleSystem[] HitParticleSystem;
        public AudioSource AudioSource;
        
        public override void Hit(float damage, Vector3 position)
        {
            Debug.Log("Hit");
            if (AudioSource != null)
            {
                AudioSource.transform.position = position;
                AudioSource.Play();
            }
            
            foreach (var part in HitParticleSystem)
            {
                part.transform.position = position;
                part.Play();
            }
        }
    }
}
