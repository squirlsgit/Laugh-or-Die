using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HandleParticleSystemLifetime : MonoBehaviour
{
	public bool onlyDeactivate;
	public float increment = 0.5f;
	public ParticleSystem ps;
	[SerializeField]
	private GameObject _target;

	private GameObject Target => _target ?? gameObject;
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(increment);
			if(!(ps ??= GetComponent<ParticleSystem>()).IsAlive(true))
			{
				if(onlyDeactivate)
				{
					Target.SetActive(false);
				}
				else
					Destroy(Target);
				break;
			}
		}
	}
}