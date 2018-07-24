using UnityEngine;
using TMPro;

namespace ProcessingUnits
{

	public class ground_Script : MonoBehaviour {

		//[Header("Object Stats")]

		//[Header("Unity Presets")]

		[Header("Private Veriables")]
		gameMaster_Script gameMaster;

		void initializeValues()
		{
			gameMaster = gameMaster_Script.instance;
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
			gameMaster.setAttacker(null);
			gameMaster.setDeffender(null);
		}
	}
}
