using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
using Valve.VR.InteractionSystem;

public class PlayerCollider : MonoBehaviour
{
    public SoundPlayOneshot _impactSound;

    private void Awake()
    {
        if (_impactSound == null)
            _impactSound = GetComponent<SoundPlayOneshot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageTrigger weapon = other.gameObject.GetComponent<DamageTrigger>();
        HealthBar.playerHealthBar.TakeDamage(weapon.Damage);
        _impactSound.Play();
    }
}
