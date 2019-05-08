	using UnityEngine;
	using UnityEngine.Events;

namespace Weapons.Specifics.Arrows
{
	//-------------------------------------------------------------------------
	public class Arrow : Projectile
	{
		[Header("Arrow variables")]
		[SerializeField] private GameObject _trail;
		[SerializeField] private GameObject _releaseEffect;
		[SerializeField] private GameObject _chargingEffect;
		
		[Header("Events")]
		public UnityEvent OnCollision;

		private bool _charging = false;

		public bool Charging
		{
			set
			{
				if (_charging != value && Status == State.Waiting)
				{
					_charging = value;
					_chargingEffect.SetActive(_charging);
				}
			}
		}

		protected override void StickInTarget(Collision collision, Vector3 actualVelocity)
		{
			base.StickInTarget(collision, actualVelocity);
			OnCollision.Invoke();
		}
		
		public override void Fire(Vector3 speedAndDir)
		{
			base.Fire(speedAndDir);
			_chargingEffect.SetActive(false);
			_trail.SetActive(true);
			_releaseEffect.SetActive(true);
			_releaseEffect.transform.parent = null;
		}

		protected override void Fire_SetVelocity(Vector3 speedAndDir)
		{
			base.Fire_SetVelocity(speedAndDir);
			MainRigidbody.AddTorque(transform.forward * 10);
		}
	}
}
