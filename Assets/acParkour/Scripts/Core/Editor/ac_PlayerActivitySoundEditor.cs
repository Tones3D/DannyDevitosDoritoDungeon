/////////////////////////////////////////////////////////////////////////////////
// 
//	ac_PlayerActivitySoundEditor.cs
//	© GamersFrontier. All Rights Reserved.
//	https://twitter.com/thndstrm
//	http://www.gamersfrontier.my
//
//	description:	Custom editor for ac_PlayerActivitySound
//
//					Code re-used and modified with express permission
/////////////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ac_PlayerActivitySound))]

public class ac_PlayerActivitySoundEditor : Editor 
{
	// target component
	public ac_PlayerActivitySound m_Component;

	// foldouts
	public static bool m_MotorJumpFoldout;
	public static bool m_MotorDashFoldout;
	public static bool m_MotorDoubleJumpFoldout;
	public static bool m_MotorWallJumpFoldout;
	public static bool m_MotorWallRunFoldout;
	public static bool m_MotorWallHangFoldout;
	public static bool m_MotorGroundSlideFoldout;
	public static bool m_MotorLedgeGrabFoldout;

	private static vp_ComponentPersister m_Persister = null;

	public virtual void OnEnable()
	{
		m_Component = (ac_PlayerActivitySound)target;

		if (m_Persister == null)
			m_Persister = new vp_ComponentPersister();
		m_Persister.Component = m_Component;
		m_Persister.IsActive = true;

	}

	/// <summary>
	/// disables the persister and removes its reference
	/// </summary>
	public virtual void OnDestroy()
	{
		
		m_Persister.IsActive = false;
		
	}

	public override void OnInspectorGUI()
	{
		GUI.color = Color.white;

		DoMotorJumpFoldout();
		DoMotorDashFoldout();
		DoMotorDoubleJumpFoldout();
		DoMotorGroundSlideFoldout();
		DoMotorLedgeGrabFoldout();
		DoMotorWallJumpFoldout();
		DoMotorWallHangFoldout();
		DoMotorWallRunFoldout();


		// update
		if (GUI.changed)
		{
			
			EditorUtility.SetDirty(target);
			
			// update the default state in order not to loose inspector tweaks
			// due to state switches during runtime
			if(Application.isPlaying)
				m_Component.RefreshDefaultState();
			
			if (m_Component.Persist)
				m_Persister.Persist();
			
		}

	}

	/// <summary>
	/// Dos the motor jump foldout.
	/// </summary>
	public virtual void DoMotorJumpFoldout()
	{
		
		m_MotorJumpFoldout = EditorGUILayout.Foldout(m_MotorJumpFoldout, "Jump Sounds");
		
		if (m_MotorJumpFoldout)
		{	
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.JumpPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.JumpPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.JumpSounds);
		}
	}

	/// <summary>
	/// Dos the motor dash foldout.
	/// </summary>
	public virtual void DoMotorDashFoldout()
	{
		
		m_MotorDashFoldout = EditorGUILayout.Foldout(m_MotorDashFoldout, "Dash Sounds");
		
		if (m_MotorDashFoldout)
		{	
			GUILayout.BeginHorizontal();
			GUILayout.Label("Dash Start Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.DashStartPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.DashStartPitch);
			GUILayout.EndHorizontal();

			EditorAudioClips(m_Component.DashStartSounds);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Dash Stop Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.DashStopPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.DashStopPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.DashStopSounds);
		}
	}

	/// <summary>
	/// Dos the motor double jump foldout.
	/// </summary>
	public virtual void DoMotorDoubleJumpFoldout()
	{
		
		m_MotorDoubleJumpFoldout = EditorGUILayout.Foldout(m_MotorDoubleJumpFoldout, "Double Jump Sounds");
		
		if (m_MotorDoubleJumpFoldout)
		{	
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.DoubleJumpPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.DoubleJumpPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.DoubleJumpSounds);
		}
	}

	/// <summary>
	/// Dos the motor groundslide foldout.
	/// </summary>
	public virtual void DoMotorGroundSlideFoldout()
	{
		
		m_MotorGroundSlideFoldout = EditorGUILayout.Foldout(m_MotorGroundSlideFoldout, "GroundSlide Sounds");
		
		if (m_MotorGroundSlideFoldout)
		{	
			GUILayout.BeginHorizontal();
			GUILayout.Label("GroundSlide Start Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.GroundSlideStartPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.GroundSlideStartPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.GroundSlideStartSounds);
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("GroundSlide Stop Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.GroundSlideStopPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.GroundSlideStopPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.GroundSlideStopSounds);
		}
	}

	/// <summary>
	/// Dos the motor ledgegrab foldout.
	/// </summary>
	public virtual void DoMotorLedgeGrabFoldout()
	{
		
		m_MotorLedgeGrabFoldout = EditorGUILayout.Foldout(m_MotorLedgeGrabFoldout, "LedgeGrab Sounds");
		
		if (m_MotorLedgeGrabFoldout)
		{	
			GUILayout.BeginHorizontal();
			GUILayout.Label("LedgeGrab Start Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.LedgeGrabStartPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.LedgeGrabStartPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.LedgeGrabStartSounds);

			GUILayout.BeginHorizontal();
			GUILayout.Label("LedgeGrab Up Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.LedgeGrabUpPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.LedgeGrabUpPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.LedgeGrabUpSounds);

			GUILayout.BeginHorizontal();
			GUILayout.Label("LedgeGrab Stop Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.LedgeGrabStopPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.LedgeGrabStopPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.LedgeGrabStopSounds);
		}
	}

	/// <summary>
	/// Dos the motor wallrun foldout.
	/// </summary>
	public virtual void DoMotorWallRunFoldout()
	{
		
		m_MotorWallRunFoldout = EditorGUILayout.Foldout(m_MotorWallRunFoldout, "WallRun Sounds");
		
		if (m_MotorWallRunFoldout)
		{	
			GUILayout.BeginHorizontal();
			GUILayout.Label("WallRun Start Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.WallRunStartPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.WallRunStartPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.WallRunStartSounds);
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("WallRun Stop Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.WallRunStopPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.WallRunStopPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.WallRunStopSounds);
		}
	}

	/// <summary>
	/// Dos the motor wall jump foldout.
	/// </summary>
	public virtual void DoMotorWallJumpFoldout()
	{
		
		m_MotorWallJumpFoldout = EditorGUILayout.Foldout(m_MotorWallJumpFoldout, "Wall Jump Sounds");
		
		if (m_MotorWallJumpFoldout)
		{	
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.WallJumpPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.WallJumpPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.WallJumpSounds);
		}
	}

	/// <summary>
	/// Dos the motor wall hang foldout.
	/// </summary>
	public virtual void DoMotorWallHangFoldout()
	{
		
		m_MotorWallHangFoldout = EditorGUILayout.Foldout(m_MotorWallHangFoldout, "Wall Hang Sounds");
		
		if (m_MotorWallHangFoldout)
		{	
			GUILayout.BeginHorizontal();
			GUILayout.Label("Wall Hang Start Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.WallHangStartPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.WallHangStartPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.WallHangStartSounds);
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Wall Hang Stop Sounds.", vp_EditorGUIUtility.NoteStyle);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(35);
			m_Component.WallHangStopPitch = EditorGUILayout.Vector2Field("Random Pitch", m_Component.WallHangStopPitch);
			GUILayout.EndHorizontal();
			
			EditorAudioClips(m_Component.WallHangStopSounds);
		}
	}

	/// <summary>
	/// Tests and show the component's audio clips.
	/// </summary>
	void EditorAudioClips(List<AudioClip> audioclips)
	{
		if(audioclips != null)
		{
			if(audioclips.Count > 0)
			{
				for (int x = 0; x < audioclips.Count; ++x)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Space(50);
					audioclips[x] = (AudioClip)EditorGUILayout.ObjectField("", audioclips[x], typeof(AudioClip), false);
					if (audioclips[x] == null)
						GUI.enabled = false;
					if (GUILayout.Button(">", vp_EditorGUIUtility.SmallButtonStyle, GUILayout.MinWidth(15), GUILayout.MaxWidth(15), GUILayout.MinHeight(15)))
					{
						AudioSource audio = m_Component.transform.root.GetComponentInChildren<AudioSource>();
						if (audio != null)
							audio.PlayOneShot(audioclips[x]);
					}
					GUI.enabled = true;
					if (GUILayout.Button("X", vp_EditorGUIUtility.SmallButtonStyle, GUILayout.MinWidth(15), GUILayout.MaxWidth(15), GUILayout.MinHeight(15)))
						//									if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(15)))
					{
						audioclips.RemoveAt(x);
						--x;
					}
					GUI.backgroundColor = Color.white;
					GUILayout.Space(20);
					
					GUILayout.EndHorizontal();
				}
			}
		}
		if(audioclips.Count == 0)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(50);
			EditorGUILayout.HelpBox("There are no sounds. Click the \"Add Sound\" button to add a sound.", MessageType.Info);
			GUILayout.Space(20);
			GUILayout.EndHorizontal();
		}
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Add Sound", GUILayout.MinWidth(90), GUILayout.MaxWidth(90)))
		{
			AudioClip clip = new AudioClip();
			audioclips.Add(clip);
		}
		GUI.backgroundColor = Color.white;
		GUILayout.EndHorizontal();
		vp_EditorGUIUtility.Separator();
	}


}
