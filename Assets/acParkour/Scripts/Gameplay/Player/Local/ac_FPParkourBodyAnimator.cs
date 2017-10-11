/////////////////////////////////////////////////////////////////////////////////
//
//	vp_FPBodyAnimator.cs
//	© GamersFrontier. All Rights Reserved.
//	https://twitter.com/thndstrm
//	http://www.gamersfrontier.my
//
//	description:	A modified version of vp_FPBodyAnimator to work with acParkour.
//
//					Code re-used and modified with express permission
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ac_FPParkourBodyAnimator : vp_FPBodyAnimator 
{
    protected SkinnedMeshRenderer[] m_Renderers = null;
	public SkinnedMeshRenderer[] Renderers
	{
		get
		{
			if (m_Renderers == null)
				m_Renderers = transform.root.GetComponentsInChildren<SkinnedMeshRenderer> ();
			return m_Renderers;
		}
	}

	public class ac_SkinMeshMaterials
	{
		public Material[] FirstPersonMats;
		public Material[] FirstPersonWithArmsMats;
		public Material[] ThirdPersonMats;
		public Material[] InvisiblePersonMats;

		public ac_SkinMeshMaterials(Material[] fpM, Material[] fpwaM, Material[] tpM, Material[] inviM)
		{
			FirstPersonMats = fpM;
			FirstPersonWithArmsMats = fpwaM;
			ThirdPersonMats = tpM;
			InvisiblePersonMats = inviM;
		}
	}

	public List<ac_SkinMeshMaterials> MultiMeshRenderers = new List<ac_SkinMeshMaterials> ();
	protected List<Material[]> m_MultiMeshMaterials = new List<Material[]> ();

	public Vector3 LedgeGrabOffsetPosition ;					// tweak this to get for more accurate placement during ledge grab

	protected CharacterController charactercontroller = null;
	protected vp_FPController m_Controller = null;

	protected ac_FPParkour m_Parkour = null;
	protected ac_FPParkour Parkour	{ get{return m_Parkour;	}}

	protected float m_climbLedgeFeetAdjustAngle = 95;
	protected float m_originalFeetAdjustAngle ;

    protected override void Awake()
	{
		base.Awake();
		m_Parkour = (ac_FPParkour)transform.root.GetComponentInChildren(typeof(ac_FPParkour));

		charactercontroller = (CharacterController)transform.root.GetComponentInChildren(typeof(CharacterController));
		m_Controller = (vp_FPController)transform.root.GetComponentInChildren(typeof(vp_FPController));

	}

	protected virtual void Start ()
	{
		m_originalFeetAdjustAngle = FeetAdjustAngle;

	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
			
		UpdateLedgeGrabPosition();

	}
	
	protected override void UpdateAnimator()
	{

		base.UpdateAnimator();

		// setup the animator for acParkour's animation
        Animator.SetBool("StartLedgeGrab", m_Parkour.IsLedgeGrabbing );
        Animator.SetBool("IsLedgeGrabbing", m_Parkour.IsLedgeGrabbing );
        Animator.SetBool("IsLedgeHanging", m_Parkour.IsHanging );
        Animator.SetBool("Hanging", m_Parkour.IsHanging);
        Animator.SetFloat("ParkourHeight", m_Parkour.DistancePlayerToTop ) ;
        Animator.SetBool("WallRunning", m_Parkour.IsWallRunning ) ;
        Animator.SetBool("Hanging", m_Parkour.IsHanging );
        Animator.SetBool("StartDash", m_Parkour.Dashing );
        Animator.SetBool("IsDashing", m_Parkour.Dashing );
        Animator.SetBool("StartGroundSlide", m_Parkour.GroundSliding );
        Animator.SetBool("IsGroundSliding", m_Parkour.GroundSliding );
        Animator.SetBool("StartWallHang", m_Parkour.IsWallHanging );
        Animator.SetBool("IsWallHanging", m_Parkour.IsWallHanging );
        Animator.SetFloat("WallAngle", m_Parkour.WallAngle);
        Animator.SetBool ("IsWallJumping", m_Parkour.IsWallJumping);
        Animator.SetBool ("IsDoubleJumping", m_Parkour.IsDoubleJumping);
		Animator.SetFloat("VelocityX", m_LocalVelocity.x);
		Animator.SetFloat("VelocityZ", m_LocalVelocity.z);
        Animator.SetBool("IsShimmyLedgeing", m_Parkour.IsShimmyLedging);
        Animator.SetInteger("ShimmyLedgeType", m_Parkour.ShimmyLedgeType);



    }

    GameObject[] InitialSetup(string filterName)
    {
        List<GameObject> temp = new List<GameObject>();
        SkinnedMeshRenderer[] children = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer child in children)
            if (child.name.Contains(filterName))
                temp.Add(child.gameObject);

        return temp.ToArray();

    }

    protected override void InitMaterials()
	{

        if (InvisibleMaterial == null)
		{
			Debug.LogWarning("Warning (" + ") No invisible material has been set.");
			return;
		}

		// we'll loop through each mesh's renderer

		for (int i = 0; i < Renderers.Length; i++) 
		{
			m_FirstPersonMaterials = new Material[Renderers[i].materials.Length];
			m_FirstPersonWithArmsMaterials = new Material[Renderers[i].materials.Length];
			m_ThirdPersonMaterials = new Material[Renderers[i].materials.Length];
			m_InvisiblePersonMaterials = new Material[Renderers[i].materials.Length];

			for (int v = 0; v < Renderers[i].materials.Length; v++)
			{
			
				// create 4 material arrays from the provided one ...
				
				// ... one with visible materials on all body parts (for 3rd person)
				m_ThirdPersonMaterials[v] = Renderers[i].materials[v];
				
				// ... one with invisible head and arm materials (for classic 1st person)
				if (Renderers[i].materials[v].name.ToLower().Contains("head") ||
				    Renderers[i].materials[v].name.ToLower().Contains("arm"))
					m_FirstPersonMaterials[v] = InvisibleMaterial;
				else
					m_FirstPersonMaterials[v] = Renderers[i].materials[v];
				
				// ... one with an invisible head but visible arms (for unarmed 1st person and VR mods)
				if (Renderers[i].materials[v].name.ToLower().Contains("head"))
					m_FirstPersonWithArmsMaterials[v] = InvisibleMaterial;
				else
					m_FirstPersonWithArmsMaterials[v] = Renderers[i].materials[v];
				
				// ... and one array with all-invisible materials (for ragdolled 1st person)
				m_InvisiblePersonMaterials[v] = InvisibleMaterial;

				// add this to our SkinRenderers List
				MultiMeshRenderers.Add (new ac_SkinMeshMaterials(m_FirstPersonMaterials,m_FirstPersonWithArmsMaterials,m_ThirdPersonMaterials,m_InvisiblePersonMaterials));
			}

        }



        RefreshMaterials();

	}

	/// <summary>
	/// swaps out the body model's material array depending on current
	/// gameplay situation
	/// </summary>
    protected new IEnumerator RefreshMaterialsOnEndOfFrame()
    {

        yield return new WaitForEndOfFrame();
		
		if (InvisibleMaterial == null)
		{
			Debug.LogWarning("Warning (" + this + ") No invisible material has been set. Head and arms will look buggy in first person.");
			goto fail;
		}
		
		if (!Player.IsFirstPerson.Get())
		{
			for (int i = 0; i < Renderers.Length; i++) 
			{
				for (int v = 0; v < Renderers[i].materials.Length; v++)
				{
					if (m_ThirdPersonMaterials != null)
						Renderers[i].materials = MultiMeshRenderers[i].ThirdPersonMats;	// all body parts visible
				}
			}
		}
		else
		{
			
			if (!Player.Dead.Active)	// player is alive ...
			{
				// if we can show unarmed arms, and player is unarmed and not climbing
				if (ShowUnarmedArms && ((Player.CurrentWeaponIndex.Get() < 1) && !Player.Climb.Active))
				{

					for (int i = 0; i < Renderers.Length; i++) 
					{
						for (int v = 0; v < Renderers[i].materials.Length; v++)
						{
							if (m_FirstPersonWithArmsMaterials != null)
								Renderers[i].materials = MultiMeshRenderers[i].FirstPersonWithArmsMats;	// only head is invisible
						}
					}
				}
				// player is armed, climbing, or prohibited to show naked arms :)
				else
				{
					for (int i = 0; i < Renderers.Length; i++) 
					{
						for (int v = 0; v < Renderers[i].materials.Length; v++)
						{
							if (m_FirstPersonMaterials != null && Renderers[i].gameObject.layer != vp_Layer.Weapon )
								Renderers[i].materials = MultiMeshRenderers[i].FirstPersonMats;	// head & arms are invisible
						}
					}
				}
			}
			else						// player is dead ...
			{
				if (m_InvisiblePersonMaterials != null)
				{
					for (int i = 0; i < Renderers.Length; i++) 
					{
						for (int v = 0; v < Renderers[i].materials.Length; v++)
						{
							if (m_FirstPersonMaterials != null && Renderers[i].gameObject.layer != vp_Layer.Weapon )
								Renderers[i].materials = MultiMeshRenderers[i].FirstPersonMats;	// head & arms are invisible
						}
					}
				}
			}
		}
		
	fail:
		{}
		
	}

	protected virtual void UpdateLedgeGrabPosition()
	{
		if (Parkour.IsLedgeGrabbing)
		{
			transform.position += LedgeGrabOffsetPosition;
			Vector3 m_HangRotation = Quaternion.AngleAxis(-180,Vector3.up) * Parkour.WallNormal;
			transform.rotation = Quaternion.LookRotation(m_HangRotation,Vector3.up);

		}
	}

    protected override void UpdatePosition()
    {
        Transform.position = FPController.SmoothPosition + (FPController.SkinWidth * Vector3.down);

        if (Player.IsFirstPerson.Get() && Parkour.IsHanging || Parkour.IsLedgeGrabbing)
        {
            Transform.position = Transform.position;	

            return;
        }
        else
            base.UpdatePosition();
    }

    protected override void UpdateBody()
	{
		if(Parkour.IsHanging || Parkour.IsLedgeGrabbing )
			return;
		else
			base.UpdateBody();
	}

	protected override void UpdateSpine()
	{
		if(Parkour.IsHanging || Parkour.IsLedgeGrabbing )
			return;
		else
			base.UpdateSpine();
	}

	protected virtual void OnStart_LedgeGrab()
	{
		m_originalFeetAdjustAngle = FeetAdjustAngle;
		FeetAdjustAngle = m_climbLedgeFeetAdjustAngle;
	}

	protected virtual void OnStop_LedgeGrab()
	{
		FeetAdjustAngle = m_originalFeetAdjustAngle;
	}

}
