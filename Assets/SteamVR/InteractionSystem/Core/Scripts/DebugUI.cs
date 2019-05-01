//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Debug UI shown for the player
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	public class DebugUI : MonoBehaviour
	{
		private SteamVR_Player _steamVrPlayer;

		//-------------------------------------------------
		static private DebugUI _instance;
		static public DebugUI instance
		{
			get
			{
				if ( _instance == null )
				{
					_instance = GameObject.FindObjectOfType<DebugUI>();
				}
				return _instance;
			}
		}


		//-------------------------------------------------
		void Start()
		{
			_steamVrPlayer = SteamVR_Player.instance;
		}


		//-------------------------------------------------
		private void OnGUI()
		{
            if (Debug.isDebugBuild)
            {
#if !HIDE_DEBUG_UI
                _steamVrPlayer.Draw2DDebug();
#endif
            }
		}
	}
}
