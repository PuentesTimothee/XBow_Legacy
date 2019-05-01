using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public enum ColliderPart
{
	Body,
	Leg,
	Head
}

public class EnemyColliderPart : Hittable
{
	public int PointOnHit;
	
	public float DamageMultiplier = 1;
	
	public ColliderPart PartType;
	public EnemyMove MainBody;

	/*
	void OnCollisionEnter(Collision other)
	{
		if (WeaponLayer == (WeaponLayer | (1 << other.gameObject.layer)))
			GetHit(10f, other.transform.position);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (WeaponLayer == (WeaponLayer | (1 << other.gameObject.layer)))
			GetHit(10f, other.transform.position);
	}
	*/

	public override void Hit(float damage, Vector3 position)
	{
		MainBody.GetHit(PartType, PointOnHit, damage * DamageMultiplier, position);
	}
}
