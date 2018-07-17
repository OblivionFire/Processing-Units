﻿using UnityEngine;
using TMPro;

public class EnergyLine_Script : MonoBehaviour
{

	[Header("Object Targets")]
	public GameObject lineTarget; //Stores the target of the processor to attack, is null if not attacking

	[Header("Unity Presets: Assign Values")]
	public GameObject owner; //The processor that this script belongs to
	public GameObject enemyEnergyPulsePrefab; //The prefab for the owner energyPulse
	public GameObject allyEnergyPulsePrefab; //The prefab for the owner energyPulse
	public GameObject enemyEnergyLinePrefab; //The prefab for the owner energyLine
	public GameObject allyEnergyLinePrefab; //The prefab for the owner energyLine



	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public GameObject drawEnergyLine(GameObject energyLine, bool underAttack, int owner, Transform p1, Transform p2)
	{
		if (underAttack)
		{
			deletEnergyLine(energyLine);
			Vector3 midPosision = Vector3.Lerp(p1.position, p2.position, .5f);
			Vector3 posision = Vector3.Lerp(p1.position, midPosision, .5f);
			if (owner == 1)
			{
				energyLine = Instantiate(allyEnergyLinePrefab);
			}

			if (owner == -1)
			{
				energyLine = Instantiate(enemyEnergyLinePrefab);
			}

			Vector3 newScale = energyLine.transform.localScale;
			newScale.z = Vector3.Distance(p1.position, midPosision);
			energyLine.transform.localScale = newScale;
			energyLine.transform.Translate(posision, Space.World);
			energyLine.transform.LookAt(midPosision);
		}


		else
		{
			deletEnergyLine(energyLine);
			Vector3 posision = Vector3.Lerp(p1.position, p2.position, .5f);
			if (owner == 1)
			{
				energyLine = Instantiate(allyEnergyLinePrefab);
			}

			if (owner == -1)
			{
				energyLine = Instantiate(enemyEnergyLinePrefab);
			}

			Vector3 newScale = energyLine.transform.localScale;
			newScale.z = Vector3.Distance(p1.position, p2.position);
			energyLine.transform.localScale = newScale;
			energyLine.transform.Translate(posision, Space.World);
			energyLine.transform.LookAt(p2.transform);
		}

		return energyLine;
	}

	public GameObject updateEnergyLine(GameObject energyLine, bool underAttack, int owner, Transform p1, Transform p2)
	{
		if (energyLine != null)
		{
			energyLine.transform.position = new Vector3(0, 0, 0);
			energyLine.transform.rotation = new Quaternion(0, 0, 0, 0);
			if (underAttack)
			{
				Vector3 midPosision = Vector3.Lerp(p1.position, p2.position, .5f);
				Vector3 posision = Vector3.Lerp(p1.position, midPosision, .5f);
				Vector3 newScale = energyLine.transform.localScale;
				newScale.z = Vector3.Distance(p1.position, midPosision);
				energyLine.transform.localScale = newScale;
				energyLine.transform.Translate(posision, Space.World);
				energyLine.transform.LookAt(midPosision);
			}


			else
			{
				Vector3 posision = Vector3.Lerp(p1.position, p2.position, .5f);
				Vector3 newScale = energyLine.transform.localScale;
				newScale.z = Vector3.Distance(p1.position, p2.position);
				energyLine.transform.localScale = newScale;
				energyLine.transform.Translate(posision, Space.World);
				energyLine.transform.LookAt(p2.transform);
			}

			return energyLine;
		}
		return drawEnergyLine(energyLine, underAttack, owner, p2, p2);
	}

	public void deletEnergyLine(GameObject energyLine)
	{
		if (energyLine != null)
		{
			Destroy(energyLine);
		}
	}
}
