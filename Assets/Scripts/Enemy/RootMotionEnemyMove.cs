using UnityEngine;
using UnityEngine.AI;

public class RootMotionEnemyMove : MonoBehaviour
{
	public NavMeshAgent Nav;
	public Animator Animator;
	
	void OnAnimatorMove()
	{
		Nav.speed = (Animator.deltaPosition / Time.deltaTime).magnitude;
	}
}
