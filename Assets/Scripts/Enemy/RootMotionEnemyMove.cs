using UnityEngine;
using UnityEngine.AI;

public class RootMotionEnemyMove : MonoBehaviour
{
	public NavMeshAgent Nav;
	public Animator Animator;
	
	void OnAnimatorMove()
	{
		if (Time.deltaTime != 0)
			Nav.speed = (Animator.deltaPosition / Time.deltaTime).magnitude;
		else
			Nav.speed = 0;
	}
}
