using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(VelocityEstimator))]
    public class Shuriken : Projectile
    {
        [Header("Shuriken variables")]
        public float VelocityMultiplier = 4.5f;

        [HideInInspector] public VelocityEstimator VelocityEstimator;
        [HideInInspector] public SoundPlayOneshot ThrowSound;
        private readonly int _hashRotate = Animator.StringToHash("Rotate");

        protected void Awake()
        {
            ThrowSound = GetComponent<SoundPlayOneshot>();
            VelocityEstimator = GetComponent<VelocityEstimator>();
        }

        protected override void Fire_SetVelocity(Vector3 speedAndDir)
        {
            MainRigidbody.AddForce(transform.forward * speedAndDir.sqrMagnitude * VelocityMultiplier, ForceMode.VelocityChange);
        }

        public override void Fire(Vector3 speedAndDir)
        {
            base.Fire(speedAndDir);
            Animator.SetBool(_hashRotate, true);
            if (ThrowSound != null)
            {
              ThrowSound.Play();
            }
        }

        protected override void StickInTarget(Collision collision, Vector3 actualVelocity)
        {
            base.StickInTarget(collision, actualVelocity);
            Animator.SetBool(_hashRotate, false);
        }
    }
}
