﻿using UnityEngine;
using TMPro;

namespace ProcessingUnits
{
	public class LevelController_Script_V1 : MonoBehaviour
	{

		public GameObject processorAllyPrefab;//Ally processor prefab used to spawn in levels
		public GameObject processorEnemyPrefab;//Enemy processor prefab used to spawn in levels
		public GameObject processorNuetralPrefab;//Nuetral processor prefab used to spawn in levels

		//[Header("Private Veriable: GameObjects")]
		//Private Veriables

		void initializeValues()
		{
			{
				GameObject Ally_1 = createProcessor(processorAllyPrefab, 9, 1, -4, 0, 0);
				GameObject Ally_2 = createProcessor(processorAllyPrefab, 9, 1, -4, 0, 3);
				GameObject Ally_3 = createProcessor(processorAllyPrefab, 9, 1, -4, 0, -3);
				GameObject Enemy_1 = createProcessor(processorEnemyPrefab, 5, -1, 6, 0, 0);
				GameObject Enemy_2 = createProcessor(processorEnemyPrefab, 5, -1, 6, 0, 3);
				GameObject Enemy_3 = createProcessor(processorEnemyPrefab, 5, -1, 6, 0, -3);
				GameObject Nuetral_1 = createProcessor(processorNuetralPrefab, 0, 0, 1, 0, 2);
				GameObject Nuetral_2 = createProcessor(processorNuetralPrefab, 0, 0, 1, 0, -2);


			}
		}

		GameObject createProcessor(GameObject prefab, int energy, int owner, int x, int y, int z)
		{
			GameObject temp = Instantiate(prefab);
			temp.transform.position = new Vector3(x, y, z);
			processor_Script script = temp.GetComponent<processor_Script>();
			script.setEnergy(energy);
			script.setUnitOwner(owner);

			if(owner == 1)
			{
				script.hoverColor = processorAllyPrefab.GetComponent<processor_Script>().hoverColor;
			}

			else if (owner == - 1)
			{
				script.hoverColor = processorEnemyPrefab.GetComponent<processor_Script>().hoverColor;
			}

			else if (owner == 0)
			{
				script.hoverColor = processorNuetralPrefab.GetComponent<processor_Script>().hoverColor;
			}
			return temp;
		}

		void Start()
		{
			initializeValues();
		}


		void Update()
		{

		}
	}
}