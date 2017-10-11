/////////////////////////////////////////////////////////////////////////////////
// 
//	ac_PlayerActivitySound.cs
//	© GamersFrontier. All Rights Reserved.
//	https://twitter.com/thndstrm
//	http://www.gamersfrontier.my
//
//	description:	Helper class that works with ac_FPParkourEventHandler
//					to play the various sounds for parkour events
//
//					Code re-used and modified with express permission
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ac_PlayerActivitySound : vp_Component 
{
	// sounds

	public Vector2 DashStartPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> DashStartSounds = new List<AudioClip>();
	public Vector2 DashStopPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> DashStopSounds = new List<AudioClip>();

	public Vector2 GroundSlideStartPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> GroundSlideStartSounds = new List<AudioClip>();
	public Vector2 GroundSlideStopPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> GroundSlideStopSounds = new List<AudioClip>();

	public Vector2 LedgeGrabStartPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> LedgeGrabStartSounds = new List<AudioClip>();
	public Vector2 LedgeGrabUpPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> LedgeGrabUpSounds = new List<AudioClip>();
	public Vector2 LedgeGrabStopPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> LedgeGrabStopSounds = new List<AudioClip>();

	public Vector2 WallHangStartPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> WallHangStartSounds = new List<AudioClip>();
	public Vector2 WallHangStopPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> WallHangStopSounds = new List<AudioClip>();

	public Vector2 WallRunStartPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> WallRunStartSounds = new List<AudioClip>();
	public Vector2 WallRunStopPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> WallRunStopSounds = new List<AudioClip>();

	public Vector2 JumpPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> JumpSounds = new List<AudioClip>();
	public Vector2 DoubleJumpPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> DoubleJumpSounds = new List<AudioClip>();
	public Vector2 WallJumpPitch = new Vector2(1.0f, 1.5f);
	public List<AudioClip> WallJumpSounds = new List<AudioClip>();
	
	// general
	protected ac_FPParkourEventHandler m_Player = null; 	// Need this to access the above and the player.

	// Use this for initialization
	protected override void  Awake () 
	{
		base.Awake();

		//initial setup
		m_Audio = GetComponent<AudioSource>();
		m_Player  = (ac_FPParkourEventHandler)transform.root.GetComponentInChildren(typeof(ac_FPParkourEventHandler));

	}

	/// <summary>
	/// registers this component with the event handler (if any)
	/// </summary>
	protected override void OnEnable()
	{
		if (m_Player != null)
			m_Player.Register(this);
	}
	
	
	/// <summary>
	/// unregisters this component from the event handler (if any)
	/// </summary>
	protected override void OnDisable()
	{
		if (m_Player != null)
			m_Player.Unregister(this);
	}
	
	protected virtual void OnStart_Dash()
	{
		if(DashStartSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, DashStartSounds, DashStartPitch);
	}

	protected virtual void OnStop_Dash()
	{
		if(DashStopSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, DashStopSounds, DashStopPitch);
	}

	protected virtual void OnStart_Jump()
	{
		if(JumpSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, JumpSounds, JumpPitch);
	}
	
	protected virtual void OnStart_DoubleJump()
	{
		if(DoubleJumpSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, DoubleJumpSounds, DoubleJumpPitch);
	}

	protected virtual void OnStart_GroundSlide()
	{
		if(GroundSlideStartSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, GroundSlideStartSounds, GroundSlideStartPitch);
	}

	protected virtual void OnStop_GroundSlide()
	{
		if(GroundSlideStopSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, GroundSlideStopSounds, GroundSlideStopPitch);
	}
	

	protected virtual void OnStart_WallJump()
	{
		if(WallJumpSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, WallJumpSounds, WallJumpPitch);
	}

	protected virtual void OnStart_WallHang()
	{
		if(WallHangStartSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, WallHangStartSounds, WallHangStartPitch);
	}

	protected virtual void OnStop_WallHang()
	{
		if(WallHangStopSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, WallHangStopSounds, WallHangStopPitch);
	}

	protected virtual void OnStart_LedgeGrab()
	{
		if(LedgeGrabStartSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, LedgeGrabStartSounds, LedgeGrabStartPitch);
	}
	
	protected virtual void OnStop_LedgeGrab()
	{
		if(LedgeGrabStopSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, LedgeGrabStopSounds, LedgeGrabStopPitch);
	}

	protected virtual void OnStart_WallRun()
	{
		if(WallRunStartSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, WallRunStartSounds, WallRunStartPitch);
	}

	protected virtual void OnMessage_ClimbingLedge()
	{
		if(LedgeGrabUpSounds != null)
			vp_AudioUtility.PlayRandomSound(m_Audio, LedgeGrabUpSounds, LedgeGrabUpPitch);	
	}

//	protected virtual void OnStop_WallRun()
//	{
//		if(WallRunStopSounds != null)
//			vp_AudioUtility.PlayRandomSound(m_Audio, WallRunStopSounds, WallRunStopPitch);
//	}
}
