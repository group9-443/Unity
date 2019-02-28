using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

	private bool active;

	public float moveSpeed = 8;

	private void Start()
	{
		Active = false;
	}

	public bool Active
	{
		get
		{
			return active;
		}
		set
		{
			active = value;
			if(active)
			{
				GetComponent<Renderer>().material.color = Color.green;
			}
			else
			{
				GetComponent<Renderer>().material.color = Color.gray;
			}

		}
	}

	public void Move(Vector3 direction)
	{
		transform.position += direction.normalized * moveSpeed * Time.deltaTime;
	}

}
