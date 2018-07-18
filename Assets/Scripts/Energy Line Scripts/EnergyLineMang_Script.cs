using UnityEngine;
using TMPro;

public class EnergyLineMang_Script : MonoBehaviour {

	//[Header("Object Stats")]

	//[Header("Unity Presets")]

	[Header("Private Veriables")]
	private int i;
	private ProcessingUnits.processor_Script home;
	public GameObject homeProcessor;
	public GameObject targetProcessor;


	public void setI(int iX)
	{
		i = iX;
	}

	public void setHome(GameObject homeX)
	{
		if(homeX != null)
		{
			home = homeX.GetComponent<ProcessingUnits.processor_Script>();
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
		if(Input.GetMouseButtonDown(1))
		{
			home.setTarget(null, i);
			Destroy(this.gameObject);
		}
	}
}
