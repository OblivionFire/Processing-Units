using UnityEngine;
using TMPro;

namespace ProcessingUnits
{

	public class ground_Script : MonoBehaviour {

		//[Header("Object Stats")]

		//[Header("Unity Presets")]

		[Header("Private Veriables")]
		gameMasterV2_Script gameMaster;

		void initializeValues()
		{
			gameMaster = gameMasterV2_Script.instance;
		}


		void Start()
		{
			initializeValues();
		}


		void Update()
		{

		}

		private void OnMouseDown()
		{
			gameMaster.setDeffender(null);
		}
	}
}
