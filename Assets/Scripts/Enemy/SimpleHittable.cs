using UnityEngine;

namespace Enemy
{
    public class SimpleHittable : Hittable
    {
        public ParticleSystem HitParticleSystem;
        public AudioSource AudioSource;
        
        public override void Hit(float damage, Vector3 position)
        {
            if (AudioSource != null)
            {
                AudioSource.transform.position = position;
                AudioSource.Play();
            }
            if (HitParticleSystem != null)
            {
                HitParticleSystem.transform.position = position;
                HitParticleSystem.Play();
            }
        }
    }
}
