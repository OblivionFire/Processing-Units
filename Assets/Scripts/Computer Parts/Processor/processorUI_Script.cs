using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProcessingUnits
{
	public class processorUI_Script : MonoBehaviour
	{
		public TextMeshProUGUI energyUI;
		private GameObject processor;
		private processorV2_Script processorScript;
		int processorEnergy;


		void initializeValues()
		{
			processor = gameObject.transform.parent.gameObject;
			processorEnergy = 0;
		}

		void Start()
		{
			initializeValues();
		}

		void Update()
		{
			if ((processorScript == null) && (processor != null))
			{ 
				processorScript = processor.GetComponent<processorV2_Script>();
			}

			if (processorScript != null)
			{
				processorEnergy = processorScript.getData();
				energyUI.text = processorEnergy.ToString();
			}
		}
	}
}