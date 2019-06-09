using System.Collections;
using System.Collections.Generic;
using ScoringSystem;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(AppearScript))]
public class EnemyMove : MonoBehaviour {

	public float maxDistanceSight;
	public List<Attack> attacks;

	public float basicSpeed = 1;
	public float chaseSpeed = 2;
	public float hp = 100;
	private float lastAttack;
	public GameObject player; // Reference to the player's.
	private UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.
	[SerializeField] private Animator _animator;
	private float minRange = -1;
	private bool _dead = false;
	public float FrontAngle = 90f;

	public SoundPlayOneshot _deathSound;
	public ParticleSystem _deathParticles;
	public SoundPlayOneshot _hurtSound;
	private AppearScript _appear;

    public GameObject orbs;

	void Awake()
	{
		// Set up the references.
		// _deathSound = GetComponent<SoundPlayOneshot>();
		_appear = GetComponent<AppearScript>();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

    private void Start()
    {
	    _dead = false;
	    if (SteamVR_Player.instance)
		    player = SteamVR_Player.instance.headCollider.gameObject;

		lastAttack = 0;
		foreach (Attack attack in attacks)
		{
			if (minRange == -1 || attack.minRange < minRange)
				minRange = attack.minRange;
		}
    }

	void Update()
	{
		if (_dead || player == null)
			return;

		if (lastAttack > 0)
			lastAttack -= Time.deltaTime;

		//if an enemy as further than maxDistance from you, it cannot see you
		var maxDistanceSquared = maxDistanceSight * maxDistanceSight;
		Vector3 rayDirection = player.transform.position - transform.position;
		Vector3 enemyDirection = transform.forward;
		var angle = Vector3.Angle(rayDirection, enemyDirection);
		var playerInFrontOfEnemy = Mathf.Abs(angle) < FrontAngle;
		var playerCloseToEnemy = rayDirection.sqrMagnitude < maxDistanceSquared;

		Vector3 Direction = new Vector3(rayDirection.x, 0, rayDirection.z);
		Vector3 Pos = new Vector3(transform.position.x, 0, transform.position.z);

		Vector3 destination = Pos + (Direction.magnitude - (minRange + 0.5f)) * Direction.normalized;
		destination.y = player.transform.position.y;
		//the enemy always go to the current location of the player but to attack him it needs to see him and be on range
		//But we reduce the magnitude so the enemy stops a little before the player
		nav.SetDestination(destination);

		//m_Animator.SetFloat("zSpeed", 0.2f);
		float angleRot = Vector3.Angle(transform.forward, rayDirection);
		Vector3 godirection = Quaternion.AngleAxis(angleRot, Vector3.up) * (-rayDirection);
		_animator.SetFloat("zSpeed", godirection.normalized.z * basicSpeed);
		_animator.SetFloat("xSpeed", godirection.normalized.x * basicSpeed);
		if (playerInFrontOfEnemy)
		{
			if (playerCloseToEnemy)
			{
				//by using a Raycast you make sure an enemy does not see you
				//if there is a building separating you from his view, for example
				//the enemy only sees you if it has you in open view
				RaycastHit hit;
				Vector3 upPos;
				upPos.x = transform.position.x;
				upPos.y = transform.position.y + 0.5f;
				upPos.z = transform.position.z;
				if (Physics.Raycast(upPos, rayDirection, out hit, Mathf.Infinity) &&
				    hit.collider.gameObject.tag == player.tag) //player object here will be your Player GameObject
				{
					_animator.SetFloat("zSpeed", godirection.normalized.z * chaseSpeed);
					_animator.SetFloat("xSpeed", godirection.normalized.x * chaseSpeed);

					foreach (Attack attack in attacks)
					{
						if ((transform.position - hit.point).magnitude <= attack.maxRange &&
						    (transform.position - hit.point).magnitude >= attack.minRange)
						{
							//attack
							if (lastAttack <= 0 && Random.Range(0, 100) <= attack.probability)
							{
								_animator.SetTrigger(attack.animName);
								lastAttack = attack.cooldown;
								//StartCoroutine(AttackPlayer());
							}
						}
					}
				}
				/*else if (hit.collider != null)
				{
					Debug.Log("Name " + name + "/" + hit.collider.name);
				}*/
			}
		}
	}

	IEnumerator AttackPlayer()
	{
		yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //make damage the player after this
	}

	public void GetHit(ColliderPart partType, int points, float damage, Vector3 position)
	{
		if (_dead)
			return;
		hp -= damage;
		if (ScoreController.Instance != null)
			ScoreController.Instance.AddScore(points, transform);
		if (hp <= 0)
		{
			Vector3 direction = position - transform.position;
			float angle = Vector3.Angle(transform.forward, direction);
			direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
			_deathSound.Play();
			_deathParticles.Play();
			_animator.SetFloat("xDeath", direction.x);
			_animator.SetFloat("zDeath", direction.z);
			_animator.SetTrigger("Death");
			StartCoroutine(Die(position));
			return;
		}
		else if (partType == ColliderPart.Leg)
		{
			basicSpeed /= 2;
			chaseSpeed /= 2;
			_hurtSound.Play();
			//add anim
		} else {
			_animator.SetTrigger("Impact");
			_hurtSound.Play();
		}
	}

	public IEnumerator Die(Vector3 position)
	{
		nav.enabled = false;
		_dead = true;
		yield return new WaitForSeconds(0.3f);
		_animator.enabled = false;
		foreach (Collider col in GetComponentsInChildren<Collider>())
			col.enabled = false;
		_appear.SetPoint(position);
		yield return _appear.Disapear();
        //		yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        int randomOrb = Random.Range(0, 100);
        if (randomOrb  < orbs.GetComponent<Orb>().chance)
        {
            Vector3 orbPos = this.transform.position;
            orbPos.y += 1;
            Instantiate(orbs, orbPos, this.transform.rotation);
        }
        Destroy(this.gameObject);
	}
}
