using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energyline_Script : MonoBehaviour {

    [Header("Object Targets")]
    public GameObject lineTarget; //Stores the target of the processor to attack, is null if not attacking

    [Header("Unity Presets: Assign Values")]
    public GameObject owner; //The processor that this script belongs to
    public GameObject energyPulsePrefab; //The prefab for the owner energyPulse
    public GameObject energyLinePrefab; //The prefab for the owner energyLine



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void sendEnergy()
    {
        //have this code just release a endergy pulse at the target
        //also have it set the engery pulses target and owner
    }

    void drawEnergyLine()
    {
        //Draw an energy line between owner and the topic
        //much of the code is working in original scripts
        //if(target.target == self)
        //{
            //half distance 
        //}
    }

    void deletEnergyLine()
    {
        //figure out the controls
    }
}
