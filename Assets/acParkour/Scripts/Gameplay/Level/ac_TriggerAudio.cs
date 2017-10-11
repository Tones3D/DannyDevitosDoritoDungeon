using UnityEngine;
using System.Collections;

public class ac_TriggerAudio : MonoBehaviour 
{
	public AudioSource m_Audio;
	public Collider m_Collider ;
	public GameObject cake ;
	public bool triggered = false;

	void OnTriggerEnter()
	{
		if(!triggered)
		{
			if(cake != null)
				GameObject.Instantiate(cake,transform.position,Quaternion.identity);
			m_Audio.Play();
			triggered = true;


		}
	}
}
