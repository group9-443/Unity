using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour {
	public float sensitivity = 1;
	public float moveSpeed = 4;

	//x and y euler angles
	private float ex, ey;

	public Text selectionText;

	public List<AgentScript> agents;
	private bool[] selectedAgents;
	private ObstacleController obstacle;
	
	void Start()
	{
		int size = agents.Count;
		selectedAgents = new bool[size];
	}

	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Return))
		{
			ToggleAll();
		}

		//Enables toggling of agents with number row keys
		for(int i=0;i<agents.Count;i++)
		{
			if (i < 9 && Input.GetKeyDown("" + (i+1)))
			{
				SelectAgent(i);
			}
		}

		if(Input.GetMouseButtonDown(0))
		{
			LeftMouseAction();
		}

		//Move the camera with the directional keys if right mouse is held, move the selected obstacle otherwise
		if(Input.GetMouseButton(1))
		{
			ex -= Input.GetAxis("Mouse Y") * sensitivity;
			ey += Input.GetAxis("Mouse X") * sensitivity;
			transform.rotation = Quaternion.Euler(ex, ey, 0);

			transform.position +=  (transform.forward * Input.GetAxis("Vertical") +
									transform.right * Input.GetAxis("Horizontal") +
									transform.up * Input.GetAxis("3D Vertical")) * 
									moveSpeed * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 4 : 1);
		}
		else if(obstacle)
		{
			float x = Vector3.Dot(transform.forward, Vector3.right);
			float z = Vector3.Dot(transform.forward, Vector3.forward);
			Vector3 fwd = new Vector3(x, 0, z);
			//Vector3 planeInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			//float ang = Vector3.Angle(planeInput, fwd);
			//Debug.Log(ang);
			obstacle.Move(fwd * Input.GetAxisRaw("Vertical"));
		}
	}

	//Update the UI text and the list of selectedAgent bools
	void SelectAgent(int index)
	{
		selectedAgents[index] = !selectedAgents[index];
		string newText = "Selected Agents:\n";
		for(int i=0;i<selectedAgents.Length;i++)
		{
			if(selectedAgents[i])
				newText += "[" + (i + 1) + "]" + agents[i].colorName + "\n";
		}
		selectionText.text = newText;
	}

	private void LeftMouseAction()
	{
		Ray ray;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			bool agentHit = false;

			//Check if the gameobject hit by the raycast is an agent
			int i = 0;
			foreach (AgentScript agent in agents)
			{
				if (agent.gameObject == hit.collider.gameObject)
				{
					agentHit = true;
					SelectAgent(i);
				}
				i++;
			}

			if (hit.collider.tag == "Obstacle")
			{
				ObstacleController newObstacle = hit.collider.GetComponent<ObstacleController>();
				//If newObstacle isn't null, have this script control that one instead of the current one
				if (newObstacle)
				{
					if (obstacle)
					{
						obstacle.Active = false;
					}
					obstacle = newObstacle;
					obstacle.Active = true;
				}
			}
			else if (!agentHit)
			{
				i = 0;
				foreach (bool selection in selectedAgents)
				{
					if (selection)
					{
						agents[i].target.position = hit.point + Vector3.up * (0.25f * i);
					}
					i++;
				}
			}
		}
	}

	//Selects all unselected agents. If all agents are selected, deselect all of them
	void ToggleAll()
	{
		bool setAllOff = true;
		for(int i=0;i<selectedAgents.Length;i++)
		{
			if(!selectedAgents[i])
			{
				SelectAgent(i);
				setAllOff = false;
			}
		}

		if(setAllOff)
		{
			for(int i=0;i<agents.Count;i++)
			{
				SelectAgent(i);
			}
		}
	}
}
