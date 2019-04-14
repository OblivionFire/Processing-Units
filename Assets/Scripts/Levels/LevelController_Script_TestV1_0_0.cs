using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ProcessingUnits
{
    public class LevelController_Script_TestV1_0_0 : MonoBehaviour
    {

        public GameObject processorAllyPrefab;//Ally processor prefab used to spawn in levels
        public GameObject processorEnemyPrefab;//Enemy processor prefab used to spawn in levels
        public GameObject processorNuetralPrefab;//Nuetral processor prefab used to spawn in levels
        public GameObject powerSupplyPrefab;
        private GameObject[] processors;
        private bool[] complete;
        private float endCount;

        //[Header("Private Veriable: GameObjects")]
        //Private Veriables

        void initializeValues()
        {
            {
                endCount = 2;
                processors = new GameObject[2];
                complete = new bool[2];
                processors[0] = createProcessor(processorAllyPrefab, 9, 1, -1, 0, -2, 1);
                processors[1] = createProcessor(processorEnemyPrefab, 4, -1, 5, 0, -2, 1);
				gameMasterV2_Script.instance.setPowerSupplyAlly(createProcessor(powerSupplyPrefab, 0, 1, 1, 0, 0, 0));
				gameMasterV2_Script.instance.setPowerSupplyEnemy(createProcessor(powerSupplyPrefab, 0, -1, 3, 0, 0, 0));


			}
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
        }

		public void powerOverlayToggle(bool state)
		{
			for (int i = 0; i < processors.Length; i++)
			{
				processors[i].GetComponent<processorV2_Script>().setDataLineVis(state);
			}
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
                if (endCount > 0)
                {
                    endCount -= Time.deltaTime;
                }

                else
                {
                    SceneManager.LoadScene("V1.4.0");
                }
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
