using UnityEngine;
using TMPro;

public class processorV2_Script : MonoBehaviour {

	[Header("Object Stats: Data")]
	public int data;//how much data has been stored in the processor
	public int owner;//0 is neurtral, 1 is ally, -1 enemy
	public float dataTrans;//how fast the processor can send data
	public float dataCreate;//how fast the processor can create data

	[Header("Object Stats: Power")]
	public float power;//0.0<=x<=2.0

	[Header("Object Stats: Heat")]
	public int heat;//0<=x<=100 100 = shut down  65-85 is sweet spot

	[Header("Object Stats: Count Downs")]
	public float[] timeUntilPulse;//time until the next data pulse for each line
	public float dataCycle;//the time until data is created again
	public int maxThreadsLines;//the max number of data lines

	[Header("Unity Presets")]

	//Private Veriables

	void initializeValues()
	{

	}


	void Start () 
	{
		initializeValues();
	}
	

	void Update () 
	{

	}
}
