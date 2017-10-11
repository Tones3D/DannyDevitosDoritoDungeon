/////////////////////////////////////////////////////////////////////////////////
// 
//	ac_FPParkourEventHandler.cs
//	© GamersFrontier. All Rights Reserved.
//	https://twitter.com/thndstrm
//	http://www.gamersfrontier.my
//
//	description:	This class adds the following states to the event system
//					Dash, WallJump, DoubleJump, WallRun, GroundSlide, and LedgeGrab
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class ac_FPParkourEventHandler : vp_FPPlayerEventHandler 
{
	// custom player activities
	public vp_Activity WallJump;
	public vp_Activity WallRun;
	public vp_Activity WallHang;
	public vp_Activity LedgeGrab;
	public vp_Activity GroundSlide;
	public vp_Activity Dash;
	public vp_Activity DoubleJump;
    public vp_Activity GroundSlam;
    public vp_Activity NeuralKinetic;
	public vp_Activity Melee;

    public vp_Message<vp_DamageInfo> HUDShieldFlash;
	public vp_Message ClimbingLedge;
	public vp_Value<Texture> WallTexture;

	// custom exosuit activities

	public vp_Activity ExoDefault;
	public vp_Activity ExoRun;
	public vp_Activity ExoCrouch;
	public vp_Activity JetPack;

	public vp_Activity AttackLeft;
	public vp_Activity AttackRight;
	
	public vp_Activity<int> SetWeaponRight;
	public vp_Activity<int> SetWeaponLeft;
	
	// weapon object events
	public vp_Message<int> WieldRight;
	public vp_Message<int> WieldLeft;
	public vp_Message UnwieldRight;
	public vp_Message UnwieldLeft;
	public vp_Attempt FireRight;
	public vp_Attempt FireLeft;
	public vp_Message DryFireRight;
	public vp_Message DryFireLeft;
	
	// weapon handler events
	public vp_Attempt SetPrevWeaponRight;
	public vp_Attempt SetPrevWeaponLeft;
	public vp_Attempt SetNextWeaponRight;
	public vp_Attempt SetNextWeaponLeft;
	public vp_Attempt<string> SetWeaponRightByName;
	public vp_Attempt<string> SetWeaponLeftByName;
	
	public vp_Value<int> CurrentWeaponRightIndex;
	public vp_Value<int> CurrentWeaponLeftIndex;
	public vp_Value<string> CurrentRightWeaponName;
	public vp_Value<string> CurrentLeftWeaponName;
	public vp_Value<bool> CurrentWeaponRightWielded;
	public vp_Value<bool> CurrentWeaponLeftWielded;
	
	public vp_Value<float> CurrentWeaponRightReloadDuration;
	public vp_Value<float> CurrentWeaponLeftReloadDuration;
	
	// inventory
	public vp_Attempt RefillCurrentWeaponRight;
	public vp_Attempt RefillCurrentWeaponLeft;
	
	public vp_Value<int> CurrentWeaponRightAmmoCount;
	public vp_Value<int> CurrentWeaponLeftAmmoCount;
	
	public vp_Value<int> CurrentWeaponRightMaxAmmoCount;
	public vp_Value<int> CurrentWeaponLeftMaxAmmoCount;
	
	public vp_Value<int> CurrentWeaponRightClipCount;
	public vp_Value<int> CurrentWeaponLeftClipCount;
	
	public vp_Value<int> CurrentWeaponRightType;
	public vp_Value<int> CurrentWeaponLeftType;
	
	public vp_Value<int> CurrentWeaponRightGrip;
	public vp_Value<int> CurrentWeaponLeftGrip;


	protected override void Awake()
	{
		
		base.Awake();

		// custom activity state bindings
		BindStateToActivity(WallJump);		// custom walljump
		BindStateToActivity(WallRun);		// custom wallrun
		BindStateToActivity(WallHang);		// custom wallrun
		BindStateToActivity(LedgeGrab);		// custom parkour climbing 
		BindStateToActivity(GroundSlide);	// custom ground sliding
		BindStateToActivity(Dash);			// custom dashing
		BindStateToActivity(DoubleJump);	// custom double jump
        BindStateToActivity(GroundSlam);    // custom double jump
        BindStateToActivity(NeuralKinetic); // custom neural kinetic
		BindStateToActivity(Melee);    // custom double jump

		BindStateToActivity(ExoDefault);// custom jetpack
		BindStateToActivity(ExoRun);  	// custom jetpack
		BindStateToActivity(ExoCrouch); // custom jetpack
		BindStateToActivity(JetPack);  		// custom jetpack
		BindStateToActivity(AttackLeft);  	// custom jetpack
		BindStateToActivity(AttackRight);  	// custom jetpack

		
		// --- activity AutoDurations ---
		// automatically stops an activity after a set timespan
		Melee.AutoDuration = 0.2f;		// NOTE: altered at runtime by each weapon


	}
}
