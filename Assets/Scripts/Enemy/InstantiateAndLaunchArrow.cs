using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Specifics.Arrows;


public class InstantiateAndLaunchArrow : MonoBehaviour
{
  public GameObject arrow;
  public GameObject currentArrow;
  public Arrow currentArrowComp;
  public GameObject Hand;
  public GameObject player;
  public GameObject longbow;

  public float force;
  private Vector3 dir;

  private void Awake()
  {
    player = Player.Instance.gameObject;
  }

  private GameObject InstantiateArrow()
  {
    GameObject newArrow = Instantiate( arrow, Hand.transform.position, Hand.transform.rotation );
    newArrow.transform.LookAt(player.transform);
    newArrow.name = "Bow " + arrow.name;
    newArrow.transform.parent = Hand.transform;
    return newArrow;
  }

  private void KnockArrow()
  {
    currentArrow = InstantiateArrow();
    currentArrowComp = currentArrow.GetComponent<Arrow>();
    dir = player.transform.position - currentArrow.transform.position;
    currentArrow.transform.LookAt(dir);
  }

  public void LaunchArrow() {
    dir = player.transform.position - currentArrow.transform.position;
    currentArrow.transform.parent = null;
    currentArrow.transform.LookAt(player.transform.position);
    currentArrowComp.Fire(dir * force);
    currentArrow = null;
    currentArrowComp = null;
  }

  // Start is called before the first frame update
  void Start()
  {
    if(!player)
      player = GameObject.Find("Player");
    if (!longbow)
      longbow = GameObject.Find("LongbowEnnemy");
  }

  // Update is called once per frame
  void Update()
  {

  }


}
