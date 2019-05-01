using UnityEngine;
using Valve.VR.InteractionSystem;
using Weapons;

[RequireComponent(typeof(SteamVR_Player))]
public class Player : MonoBehaviour
{
    public SteamVR_Player    SteamVrPlayer;
    public WeaponSlot        WeaponsSlot;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
