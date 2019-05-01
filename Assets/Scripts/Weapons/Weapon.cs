using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        public string Name;
        public GameObject MainHandItemPrefab; // object to be spawned on tracked controller
        public GameObject SubHandItemPrefab; // object to be spawned in Other Hand
        public AppearScript[] Appears;
    }
}
