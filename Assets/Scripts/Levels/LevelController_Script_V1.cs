using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace ProcessingUnits
{
	public class LevelController_Script_V1 : MonoBehaviour
	{

		public GameObject processorAllyPrefab;//Ally processor prefab used to spawn in levels
		public GameObject processorEnemyPrefab;//Enemy processor prefab used to spawn in levels
		public GameObject processorNuetralPrefab;//Nuetral processor prefab used to spawn in levels
		private bool[] complete;
		private GameObject[] processors;

		//[Header("Private Veriable: GameObjects")]
		//Private Veriables

		void initializeValues()
		{
			complete = new bool[8];
			processors = new GameObject[8];
			processors[0] = createProcessor(processorAllyPrefab, 9, 1, -4, 0, 0);
			processors[1] = createProcessor(processorAllyPrefab, 9, 1, -4, 0, 3);
			processors[2] = createProcessor(processorAllyPrefab, 9, 1, -4, 0, -3);
			processors[3] = createProcessor(processorEnemyPrefab, 5, -1, 6, 0, 0);
			processors[4] = createProcessor(processorEnemyPrefab, 5, -1, 6, 0, 3);
			processors[5] = createProcessor(processorEnemyPrefab, 5, -1, 6, 0, -3);
			processors[6] = createProcessor(processorNuetralPrefab, 0, 0, 1, 0, 2);
			processors[7] = createProcessor(processorNuetralPrefab, 0, 0, 1, 0, -2);

		}

		GameObject createProcessor(GameObject prefab, int energy, int owner, int x, int y, int z)
		{
			GameObject temp = Instantiate(prefab);
			temp.transform.position = new Vector3(x, y, z);
			processorV2_Script script = temp.GetComponent<processorV2_Script>();
			script.setData(energy);
			script.setOwner(owner);
			if (owner == 1)
			{
				script.hoverColor = processorAllyPrefab.GetComponent<processorV2_Script>().hoverColor;
			}

			else if (owner == -1)
			{
				script.hoverColor = processorEnemyPrefab.GetComponent<processorV2_Script>().hoverColor;
			}

			else if (owner == 0)
			{
				script.hoverColor = processorNuetralPrefab.GetComponent<processorV2_Script>().hoverColor;
			}
			return temp;
		}

		void Start()
		{
			initializeValues();
		}


		void Update()
		{
			checkLevel(processors);
			levelComplete(complete);
		}

		void levelComplete(bool[] checks)
		{
			bool nextLevel = true;
			foreach(bool check in checks)
			{
				if(check == false)
				{
					nextLevel = false;
				}
			}

			if(nextLevel == true)
			{
				
			}
		}

		void checkLevel(GameObject[] processors)
		{
			for (int i = 0; i < processors.Length; i++)
			{
				if((processors[i].GetComponent<processorV2_Script>().getOwner() == 1) || (processors[i].GetComponent<processorV2_Script>().getOwner() == 0))
				{
					complete[i] = true;
				}

				else
				{
					complete[i] = false;
				}
			}
		}
	}
}