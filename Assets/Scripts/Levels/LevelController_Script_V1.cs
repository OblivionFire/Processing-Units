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
		public GameObject powerSupplyPrefab; //power suppyl prefab used to spawn in levels
		private bool[] complete;
		private GameObject[] processors;
		public double dataOverlayTimer;
        public double powerOverlayTimer;
		public bool dataVisState;
        public bool powerVisState;

		//[Header("Private Veriable: GameObjects")]
		//Private Veriables

		void initializeValues()
		{
			complete = new bool[8];
			processors = new GameObject[8];
			processors[0] = createProcessor(processorAllyPrefab, 9, 1, -4, 0, 0, 1);
			processors[1] = createProcessor(processorAllyPrefab, 9, 1, -4, 0, 3, 1);
			processors[2] = createProcessor(processorAllyPrefab, 9, 1, -4, 0, -3, 1);
			processors[3] = createProcessor(processorEnemyPrefab, 5, -1, 6, 0, 0, 1);
			processors[4] = createProcessor(processorEnemyPrefab, 5, -1, 6, 0, 3, 1);
			processors[5] = createProcessor(processorEnemyPrefab, 5, -1, 6, 0, -3, 1);
			processors[6] = createProcessor(processorNuetralPrefab, 0, 0, 1, 0, 2, 1);
			processors[7] = createProcessor(processorNuetralPrefab, 0, 0, 1, 0, -2, 1);
			gameMasterV2_Script.instance.setPowerSupplyAlly(createProcessor(powerSupplyPrefab, 0, 1, -7, 0, 0, 0));
			gameMasterV2_Script.instance.setPowerSupplyEnemy(createProcessor(powerSupplyPrefab, 0, -1, 9, 0, 0, 0));
			dataOverlayTimer = 1.0;
            powerOverlayTimer = 1.0;
			dataVisState = true;
            powerVisState = true;

		}

		GameObject createProcessor(GameObject prefab, int energy, int owner, int x, int y, int z, int component)
		{

			if (component == 0)
			{
				GameObject temp = Instantiate(prefab);
				temp.transform.position = new Vector3(x, y, z);
				temp.GetComponent<powerSupply_Script>().setOwner(owner);
				return temp;
			}

			else if (component == 1)
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

			else
			{
				return null;
			}
		}

		void Start()
		{
			initializeValues();
		}


		void Update()
		{
			checkLevel(processors);
			levelComplete(complete);
			dataOverlayTimer -= Time.deltaTime;
            powerOverlayTimer -= Time.deltaTime;

			if ((Input.GetKey("o")) && (dataOverlayTimer <= 0.0))
			{
				dataVisState = !dataVisState;
				powerOverlayToggle(dataVisState);
				dataOverlayTimer = 1.0;
			}

            if((Input.GetKey("p")) && (powerOverlayTimer <= 0.0))
            {
                powerLightCall();
                powerOverlayTimer = 1.0;
            }
		}

		public void powerOverlayToggle(bool state)
		{
			for (int i = 0; i < processors.Length; i++)
			{
				processors[i].GetComponent<processorV2_Script>().setDataLineVis(state);
			}
		}

        public void powerLightCall()
        {
            powerVisState = !powerVisState;
            gameMasterV2_Script.instance.togglePowerlights(processors, !powerVisState);
        }


		void levelComplete(bool[] checks)
		{
			bool nextLevel = true;
			foreach (bool check in checks)
			{
				if (check == false)
				{
					nextLevel = false;
				}
			}

			if (nextLevel == true)
			{

			}
		}

		void checkLevel(GameObject[] processors)
		{
			for (int i = 0; i < processors.Length; i++)
			{
				if ((processors[i].GetComponent<processorV2_Script>().getOwner() == 1) || (processors[i].GetComponent<processorV2_Script>().getOwner() == 0))
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