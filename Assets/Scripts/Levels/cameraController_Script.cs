﻿using UnityEngine;

public class cameraController_Script : MonoBehaviour
{

	private bool doMovement = true;

	public float panSpeed = 10f;
	public float scrollSpeed = 2f;
	public float minY = 2f;
	public float maxY = 20f;

	// Update is called once per frame
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Escape))
			doMovement = !doMovement;

		if (!doMovement)
			return;

		if (Input.GetKey("w"))
		{
			transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("s"))
		{
			transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("d"))
		{
			transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("a"))
		{
			transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
		}
		if(Input.GetKey("r"))
		{
			transform.Translate(Vector3.up * panSpeed * Time.deltaTime, Space.World);
		}
		if(Input.GetKey("f"))
		{
			transform.Translate(Vector3.down * panSpeed * Time.deltaTime, Space.World);
		}

	}
}