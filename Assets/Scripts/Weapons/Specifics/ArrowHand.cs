//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: The object attached to the player's hand that spawns and fires the
//			arrow
//
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Weapons;
using Weapons.Specifics;
using Weapons.Specifics.Arrows;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent(typeof(Interactable))]
	public class ArrowHand : MonoBehaviour
	{
		public Interactable Interactable;
		private Hand _hand;
		private Longbow _bow;

		private SelectionWheel _selection;
		[SerializeField] private List<GameObject> _arrowsPrefabs;

		private GameObject SelectArrow
		{
			get { return (_arrowsPrefabs[_currentIndex]); }
		}

		private int _currentIndex;
		private GameObject _currentArrow;

		private GameObject ArrowPrefab
		{
			get { return (SelectArrow.gameObject); }
		}

		public Transform ArrowNockTransform;

		public float nockDistance = 0.1f;
		public float lerpCompleteDistance = 0.08f;
		public float rotationLerpThreshold = 0.15f;
		public float positionLerpThreshold = 0.15f;

		#region Private
		private bool allowArrowSpawn = true;
		private bool nocked;
        private GrabTypes nockedWithType = GrabTypes.None;
		private bool inNockRange = false;
		private bool arrowLerpComplete = false;
		#endregion
		
		public SoundPlayOneshot arrowSpawnSound;

		//-------------------------------------------------
		void Awake()
		{
			Interactable.onAttachedToHand += AttachedToHand;
			Interactable.onDetachedFromHand += DetachedFromHand;
			_currentIndex = 0;
		}

		//-------------------------------------------------
		private void AttachedToHand( Hand attachedHand )
		{
			_hand = attachedHand;
			_selection = _hand.GetComponentInChildren<SelectionWheel>();
			SetSelection();
			FindBow();
		}

		//-------------------------------------------------
		private void DetachedFromHand( Hand hand )
		{
			ClearSelection();
		}
		
		private void SetSelection()
		{
			var list = new List<string>(_arrowsPrefabs.Count);
			for (int x = 0; x < _arrowsPrefabs.Count; x++)
				list.Add(_arrowsPrefabs[x].name);
			_selection.UpdateNumberOfSlot(list);

			_selection.OnValidateOption += OnSelectNewArrow;
		}

		private void ClearSelection()
		{
			if (_selection == null)
				return;
			_selection.OnValidateOption -= OnSelectNewArrow;
			_selection.RemoveAllSlot();
		}
		
		private void OnSelectNewArrow(int index)
		{
			if (index != _currentIndex)
			{
				_currentIndex = index;
				if (_currentArrow)
				{
					Destroy(_currentArrow);
					_currentArrow = InstantiateArrow();
				}
			}
		}
		
		//-------------------------------------------------
		private GameObject InstantiateArrow()
		{
			GameObject arrow = Instantiate( ArrowPrefab, ArrowNockTransform.position, ArrowNockTransform.rotation );
			arrow.name = "Bow " + ArrowPrefab.name;
			arrow.transform.parent = ArrowNockTransform;
			Util.ResetTransform( arrow.transform );
			return arrow;
		}


		//-------------------------------------------------
		private void HandAttachedUpdate( Hand hand )
		{
			if ( _bow == null )
			{
				FindBow();
			}

			if ( _bow == null )
			{
				return;
			}

			if ( allowArrowSpawn && ( _currentArrow == null ) ) // If we're allowed to have an active arrow in hand but don't yet, spawn one
			{
				_currentArrow = InstantiateArrow();
				arrowSpawnSound.Play();
			}

			float distanceToNockPosition = Vector3.Distance( transform.parent.position, _bow.nockTransform.position );

			// If there's an arrow spawned in the hand and it's not nocked yet
			if ( !nocked )
			{
				// If we're close enough to nock position that we want to start arrow rotation lerp, do so
				if ( distanceToNockPosition < rotationLerpThreshold )
				{
					float lerp = Util.RemapNumber( distanceToNockPosition, rotationLerpThreshold, lerpCompleteDistance, 0, 1 );

					ArrowNockTransform.rotation = Quaternion.Lerp( ArrowNockTransform.parent.rotation, _bow.nockRestTransform.rotation, lerp );
				}
				else // Not close enough for rotation lerp, reset rotation
				{
					ArrowNockTransform.localRotation = Quaternion.identity;
				}

				// If we're close enough to the nock position that we want to start arrow position lerp, do so
				if ( distanceToNockPosition < positionLerpThreshold )
				{
					float posLerp = Util.RemapNumber( distanceToNockPosition, positionLerpThreshold, lerpCompleteDistance, 0, 1 );

					posLerp = Mathf.Clamp( posLerp, 0f, 1f );

					ArrowNockTransform.position = Vector3.Lerp( ArrowNockTransform.parent.position, _bow.nockRestTransform.position, posLerp );
				}
				else // Not close enough for position lerp, reset position
				{
					ArrowNockTransform.position = ArrowNockTransform.parent.position;
				}


				// Give a haptic tick when lerp is visually complete
				if ( distanceToNockPosition < lerpCompleteDistance )
				{
					if ( !arrowLerpComplete )
					{
						arrowLerpComplete = true;
						hand.TriggerHapticPulse( 500 );
					}
				}
				else
				{
					if ( arrowLerpComplete )
					{
						arrowLerpComplete = false;
					}
				}

				// Allow nocking the arrow when controller is close enough
				if ( distanceToNockPosition < nockDistance )
				{
					if ( !inNockRange )
					{
						inNockRange = true;
						_bow.ArrowInPosition();
					}
				}
				else
				{
					if ( inNockRange )
					{
						inNockRange = false;
					}
				}

                GrabTypes bestGrab = hand.GetBestGrabbingType(GrabTypes.Pinch, true);

                // If arrow is close enough to the nock position and we're pressing the trigger, and we're not nocked yet, Nock
                if ( ( distanceToNockPosition < nockDistance ) && bestGrab != GrabTypes.None && !nocked )
				{
					if ( _currentArrow == null )
					{
						_currentArrow = InstantiateArrow();
					}

					nocked = true;
                    nockedWithType = bestGrab;
					_bow.StartNock( this );
					hand.HoverLock( GetComponent<Interactable>() );
					_currentArrow.transform.parent = _bow.nockTransform;
					Util.ResetTransform( _currentArrow.transform );
					Util.ResetTransform( ArrowNockTransform );
				}
			}


			// If arrow is nocked, and we release the trigger
			if ( nocked && hand.IsGrabbingWithType(nockedWithType) == false )
			{
				if ( _bow.pulled ) // If bow is pulled back far enough, fire arrow, otherwise reset arrow in arrowhand
				{
					FireArrow();
				}
				else
				{
					ArrowNockTransform.rotation = _currentArrow.transform.rotation;
					_currentArrow.transform.parent = ArrowNockTransform;
					Util.ResetTransform( _currentArrow.transform );
					nocked = false;
                    nockedWithType = GrabTypes.None;
					_bow.ReleaseNock();
					hand.HoverUnlock( GetComponent<Interactable>() );
				}

				_bow.StartRotationLerp(); // Arrow is releasing from the bow, tell the bow to lerp back to controller rotation
			}
		}

		//-------------------------------------------------
		private void FireArrow()
		{
			_currentArrow.transform.parent = null;

			Arrow arrow = _currentArrow.GetComponent<Arrow>();
			arrow.Fire(_currentArrow.transform.forward * _bow.GetArrowVelocity());

			nocked = false;
            nockedWithType = GrabTypes.None;

			_bow.ArrowReleased();

			allowArrowSpawn = false;
			Invoke( "EnableArrowSpawn", 0.5f );
			StartCoroutine( ArrowReleaseHaptics() );

			_currentArrow = null;
		}


		//-------------------------------------------------
		private void EnableArrowSpawn()
		{
			allowArrowSpawn = true;
		}


		//-------------------------------------------------
		private IEnumerator ArrowReleaseHaptics()
		{
			yield return new WaitForSeconds( 0.05f );

			_hand.otherHand.TriggerHapticPulse( 1500 );
			yield return new WaitForSeconds( 0.05f );

			_hand.otherHand.TriggerHapticPulse( 800 );
			yield return new WaitForSeconds( 0.05f );

			_hand.otherHand.TriggerHapticPulse( 500 );
			yield return new WaitForSeconds( 0.05f );

			_hand.otherHand.TriggerHapticPulse( 300 );
		}


		//-------------------------------------------------
		private void OnHandFocusLost( Hand hand )
		{
			gameObject.SetActive( false );
		}


		//-------------------------------------------------
		private void OnHandFocusAcquired( Hand hand )
		{
			gameObject.SetActive( true );
		}


		//-------------------------------------------------
		private void FindBow()
		{
			_bow = _hand.otherHand.GetComponentInChildren<Longbow>();
		}
	}
}
