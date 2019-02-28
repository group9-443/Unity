using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AgentScript : MonoBehaviour {

	public LayerMask agentMask;

	//How wide of an angle the agent will detect agents in its way
	public float fov = 60;

	public Color asglh;

	public string colorName;

	public float agentCheckRadius = 1;

	public Transform target;

	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		agent.SetDestination(target.position);
		Collider[] hits = Physics.OverlapSphere(transform.position, agentCheckRadius, agentMask);

		for(int i=0;i < hits.Length; i++)
		{
			if (hits[i].gameObject != gameObject)
			{
				Vector3 agentPos = hits[i].transform.position;
				Vector3 difference = agentPos - transform.position;

				float ang = Mathf.Abs(Vector3.Angle( agent.velocity, difference));
				while (ang >= 360)
					ang -= 360;
				while (ang < 0)
					ang += 360;

				if (agent.velocity.sqrMagnitude != 0 && (ang < fov/2 || ang > 360 - fov/2))
				{
					agent.velocity *= 0;
				}
			}
		}
	}
}
