	using UnityEngine;
	using UnityEngine.Events;

namespace Weapons.Specifics.Arrows
{
	//-------------------------------------------------------------------------
	public class Arrow : Projectile
	{
		[Header("Arrow variables")]
		[SerializeField] private ParticleSystem _trailPs;
		[SerializeField] private GameObject _effect;
		
		[Header("Events")]
		public UnityEvent OnCollision;
		
		protected override void StopStart(bool keepSpeed)
		{
			base.StopStart(keepSpeed);
			var main = _trailPs.main;
			main.simulationSpeed = 0;
		}

		protected override void StopEnd(bool keepSpeed)
		{
			base.StopEnd(keepSpeed);
			var main = _trailPs.main;
			main.simulationSpeed = 1;
		}

		protected override void StickInTarget(Collision collision, Vector3 actualVelocity)
		{
			base.StickInTarget(collision, actualVelocity);
			OnCollision.Invoke();
		}
		
		public override void Fire(Vector3 speedAndDir)
		{
			base.Fire(speedAndDir);
			_trailPs.gameObject.SetActive(true);
			_effect.SetActive(true);
			_effect.transform.parent = null;
		}

		protected override void Fire_SetVelocity(Vector3 speedAndDir)
		{
			base.Fire_SetVelocity(speedAndDir);
			MainRigidbody.AddTorque(transform.forward * 10);
		}
	}
}
