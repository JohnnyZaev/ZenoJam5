using System;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
	public Transform PlayerTransform;
	public Transform CameraTransform;
	public bool PlayerAlive = true;
	[SerializeField] Vector3 offSet;
  
	void LateUpdate()
	{
		if (PlayerAlive == true)
		{
			CameraTransform.position = PlayerTransform.position + offSet;
		}
	}
}
