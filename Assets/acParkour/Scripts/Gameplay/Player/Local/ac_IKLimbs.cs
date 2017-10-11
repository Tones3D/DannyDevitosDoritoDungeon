using UnityEngine;
using System.Collections;

public class ac_IKLimbs : MonoBehaviour 
{
    public float RayHeightOffset = 2.2f;
    public float RayDepthOffset = 2.2f;

    public float RayThickness = 0.1f;
    public float RayLength = 0.5f;

    public float OffsetMultiply = 0.3f;
	public Vector3 ClimbPositionOffset = Vector3.zero;
	public float IKTransitionDuration = 0.2f;

    protected float m_IKWeight = 0f;

    protected Vector3 LeftHandPos = Vector3.zero;
    protected Vector3 RightHandPos = Vector3.zero;

    protected bool m_ClimbingUp = false;
    protected bool m_TransitionToShimmy = false;

    protected Animator Animator = null;
	protected ac_FPParkour Parkour = null;
	protected vp_PlayerEventHandler m_Player = null;
	protected vp_PlayerEventHandler Player
	{
		get
		{
			if (m_Player == null)
				m_Player = (vp_PlayerEventHandler)transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
			return m_Player;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnEnable()
	{
		
		if (Player != null)
			Player.Register(this);
		
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnDisable()
	{
		
		if (Player != null)
			Player.Unregister(this);
		
	}


	void Awake()
	{
		Animator = GetComponent<Animator>();
		Parkour = transform.root.GetComponent<ac_FPParkour> ();
	}
    
    void LateUpdate()
    {
        UpdateHandToLedgeIK();
    }

    void UpdateHandToLedgeIK()
    {
        if (!Parkour.IsLedgeGrabbing || !Parkour.IsHanging)
            return;

        RaycastHit centerHit;
        RaycastHit leftHit;
        RaycastHit rightHit;

        Vector3 centeroffset = Parkour.ClimbObjectPosition + new Vector3(0, RayHeightOffset, 0) + -(Parkour.WallNormal * RayDepthOffset);

        if (Physics.SphereCast(centeroffset, RayThickness, -transform.root.up, out centerHit, RayLength, vp_Layer.Mask.ExternalBlockers))
        {
            Debug.DrawLine(centeroffset, centerHit.point, Color.cyan);
        }
        
        Vector3 leftoffset = Parkour.ClimbObjectPosition + new Vector3(0, RayHeightOffset, 0) + -(Parkour.WallNormal * RayDepthOffset) + (Quaternion.AngleAxis(90, Vector3.up) * Parkour.WallNormal * OffsetMultiply);
        Vector3 rightoffset = Parkour.ClimbObjectPosition + new Vector3(0, RayHeightOffset, 0) + -(Parkour.WallNormal * RayDepthOffset) + (Quaternion.AngleAxis(-90, Vector3.up) * Parkour.WallNormal * OffsetMultiply);

        Vector3 leftdirection = centerHit.point - leftoffset;
        Vector3 rightdirection = centerHit.point - rightoffset;


        if (Physics.SphereCast(leftoffset, RayThickness, leftdirection, out leftHit, RayLength, vp_Layer.Mask.ExternalBlockers))
        {
            Debug.DrawLine(leftoffset, leftHit.point, Color.cyan);
            LeftHandPos = leftHit.point;
        }

        if (Physics.SphereCast(rightoffset, RayThickness, rightdirection, out rightHit, RayLength, vp_Layer.Mask.ExternalBlockers))
        {
            Debug.DrawLine(rightoffset, rightHit.point, Color.cyan);
            RightHandPos = rightHit.point;
        }


    }
    
	//a callback for calculating IK
	void OnAnimatorIK()
	{
		// no animator
		if (!Animator) 
			return;

        if (!Parkour.IsLedgeGrabbing)
		{
			Animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
			Animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 

			Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
			Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,0); 
			return;
		}
              
        if (Parkour.IsShimmyLedging && !m_TransitionToShimmy)
        {
            m_TransitionToShimmy = true;
            StartCoroutine(TurnOffIK(0f));
            vp_Timer.In(0.3f, delegate () { StartCoroutine(TurnOnIK(0.1f)); });
            vp_Timer.In(0.3f, delegate () { m_TransitionToShimmy = false; });

        }

        //Vector3 rightheightoffset = new Vector3(0, RightHandPos.y+0.1f, 0) - new Vector3(0, Parkour.ClimbObjectPosition.y, 0);
        //Vector3 leftheightoffset =  new Vector3(0, LeftHandPos.y+0.1f, 0) - new Vector3(0, Parkour.ClimbObjectPosition.y, 0);

        //Vector3 rightoffset = transform.TransformDirection(ClimbPositionOffset) + Parkour.ClimbObjectPosition + rightheightoffset;
        //Vector3 leftoffset = transform.TransformDirection(ClimbPositionOffset) + Parkour.ClimbObjectPosition + leftheightoffset;

        Vector3 rightoffset = transform.TransformDirection(ClimbPositionOffset) + RightHandPos;
        Vector3 leftoffset = transform.TransformDirection(ClimbPositionOffset) + LeftHandPos;

        Animator.SetIKPositionWeight(AvatarIKGoal.RightHand,m_IKWeight);
		Animator.SetIKRotationWeight(AvatarIKGoal.RightHand,m_IKWeight);
		Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,m_IKWeight);
		Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,m_IKWeight);  
		Animator.SetIKPosition(AvatarIKGoal.RightHand, rightoffset);
		Animator.SetIKPosition(AvatarIKGoal.LeftHand, leftoffset);

	}  
    
	void OnStart_LedgeGrab()
	{
		StartCoroutine (TurnOnIK(IKTransitionDuration));
	}

    void OnStop_LedgeGrab()
    {
        m_ClimbingUp = false;

        m_IKWeight = 0;

    }

    /// <summary>
    /// This lerps the camera towards the direction of the wallrunning
    ///
    protected virtual IEnumerator TurnOnIK (float duration)
	{
        float t = 0;
		
		while(t < 1)
		{	
			t += Time.deltaTime/ duration;
			m_IKWeight = Mathf.Lerp (0, 1, t);
			yield return new WaitForEndOfFrame();

		}
		

	}

    /// <summary>
    /// This lerps the camera towards the direction of the wallrunning
    ///
    protected virtual IEnumerator TurnOffIK(float duration)
    {

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            m_IKWeight = Mathf.Lerp(1, 0, t);
            yield return new WaitForEndOfFrame();

        }


    }

}
