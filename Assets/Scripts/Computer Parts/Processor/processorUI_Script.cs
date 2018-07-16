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
		private processor_Script processorScript;
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
				processorScript = processor.GetComponent<processor_Script>();
			}

			if (processorScript != null)
			{
				processorEnergy = processorScript.getEnergy();
				energyUI.text = processorEnergy.ToString();
			}
		}
	}
}