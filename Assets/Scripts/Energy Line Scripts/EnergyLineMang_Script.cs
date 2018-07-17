using UnityEngine;
using TMPro;

public class EnergyLineMang_Script : MonoBehaviour {

	//[Header("Object Stats")]

	//[Header("Unity Presets")]

	[Header("Private Veriables")]
	private int i;
	private ProcessingUnits.processor_Script home;


	public void setI(int iX)
	{
		i = iX;
	}

	public void setHome(ProcessingUnits.processor_Script homeX)
	{
		if(homeX != null)
		{
			home = homeX;
		}
	}

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

	private void OnMouseOver()
	{
		Debug.Log("Nouse over");
		if(Input.GetMouseButtonDown(1))
		{
			
			Destroy(this.gameObject);
		}
	}
}
