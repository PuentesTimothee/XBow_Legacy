using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Weapons
{
	public class WeaponSlot : MonoBehaviour
	{
		public Hand MainHand;
		public SelectionWheel MainHand_Selection;
		
		public Hand SubHand;
		public SelectionWheel SubHand_Selection;

		public List<Weapon> Weapons;
		
		private List<Weapon> _weaponsInStash;
		private Weapon _equippedWeapon;

		private SelectionWheel _weaponSelectionWheel;
		
		private void Awake()
		{
			_weaponSelectionWheel = SubHand_Selection;
			_weaponSelectionWheel.OnValidateOption += EquipWeapon;
			_weaponsInStash = new List<Weapon>();
		}
	
		private void Start()
		{
			foreach (var weapon in Weapons)
				AddWeaponInStash(weapon);
		
			UpdateSelectionWeapon();
		}

		private void OnEnable()
		{
			_weaponSelectionWheel.gameObject.SetActive(true);
		}
		
		private void OnDisable()
		{
			_weaponSelectionWheel.gameObject.SetActive(false);
			if (_equippedWeapon != null)
			{
				UnequipedOneHand(MainHand, _equippedWeapon.MainHandItemPrefab);
				UnequipedOneHand(SubHand, _equippedWeapon.SubHandItemPrefab);
			}
		}

		#region Stashed weapon
		private void AddWeaponInStash(Weapon newWeapon)
		{
			_weaponsInStash.Add(newWeapon);
		}

		private void UpdateSelectionWeapon()
		{
			var list = new List<string>(_weaponsInStash.Count);

			for (int x = 0; x < _weaponsInStash.Count; x++)
				list.Add(_weaponsInStash[x].Name);

			_weaponSelectionWheel.UpdateNumberOfSlot(list);
		}
		#endregion

		#region Equip
		private void EquipWeapon(int index)
		{
			EquipSomething(_weaponsInStash[index]);
		}

		private void EquipSomething(Weapon weapon)
		{
			if (weapon == _equippedWeapon)
				return;
			
			UnequipEverything();

			EquipOneHand(MainHand, weapon.MainHandItemPrefab);
			EquipOneHand(SubHand, weapon.SubHandItemPrefab);

			_equippedWeapon = weapon;

			foreach (var script in _equippedWeapon.Appears)
				script.Start();
		}

		private void EquipOneHand(Hand hand, GameObject obj)
		{
			obj.SetActive(true);
			hand.AttachObject(obj, GrabTypes.Scripted);
		}

		private void UnequipEverything()
		{
			if (_equippedWeapon != null)
			{
				UnequipedOneHand(MainHand, _equippedWeapon.MainHandItemPrefab);
				UnequipedOneHand(SubHand, _equippedWeapon.SubHandItemPrefab);

				_equippedWeapon = null;
			}
		}
		
		private void UnequipedOneHand(Hand hand, GameObject obj)
		{
			hand.DetachObject(obj, true);
			obj.SetActive(false);
		}
		#endregion
	}
}
