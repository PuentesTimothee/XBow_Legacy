using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Specifics.Arrows;


public class InstantiateAndLaunchArrow : MonoBehaviour
{
  public GameObject arrow;
  public GameObject currentArrow;
  public GameObject Hand;

  public float force;


  public void KnockArrow() {
    currentArrow = Instantiate(arrow, Hand.transform.position, Hand.transform.rotation);
    currentArrow.transform.SetParent(Hand.transform);
  }

  public void LaunchArrow() {
    Arrow ArrowComp = currentArrow.GetComponent<Arrow>();
    GameObject player = GameObject.Find("Player");
    Vector3 dir = player.transform.position - transform.position;
    dir = dir.normalized;
    ArrowComp.Fire(dir * force);
    Debug.Log("LaunchArrow");
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
