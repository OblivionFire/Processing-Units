using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPulse_Script : MonoBehaviour {



    [Header("Object Target")]
    public GameObject pulseTarget;

    [Header("Unity Presets: Assign Values")]
    public GameObject owner;
    public GameObject energyPulsePrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
