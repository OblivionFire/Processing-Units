using UnityEngine;
using TMPro;

namespace ProcessingUnits
{
	public class gameMasterV2_Script : MonoBehaviour
	{

		[Header("Object Stats")]
		public static gameMasterV2_Script instance;//singleton patttern. Basically there can only ever be one gameMaster so it sets itself to instance and everything else referances instance
		public GameObject processorAllyPrefab;//Ally processor prefab used to spawn in levels
		public GameObject processorEnemyPrefab;//Enemy processor prefab used to spwn in levels
        public GameObject powerSupply;//Used to track all power supplies on the board

		[Header("Private Veriable: GameObjects")]
		private GameObject attacker;//the offencive processor(first selected)
		private GameObject deffender;//the deffencive processor(second selected)
        private GameObject toPower;//The next object the player clicks after clicking a power supply

		#region Attacker Setter and Getter

		public void setAttacker(GameObject attackerX)
		{
			attacker = attackerX;
		}

		public GameObject getAttacker()
		{
			return attacker;
		}

		#endregion
		#region Deffender Setter and Getter

		public void setDeffender(GameObject deffenderX)
		{
			deffender = deffenderX;
		}

		public GameObject getDeffender()
		{
			return deffender;
		}

        #endregion
        #region powerSupply Setter
        public void setPowerSupply(GameObject powerSupplyX)
        {
            powerSupply = powerSupplyX;
        }
        #endregion
        #region toPower Setter
        public void setToPower(GameObject prosessorToPower)
        {
            if (prosessorToPower == null)
            {
                toPower = null;
            }

            else if (prosessorToPower.tag == "AllyComputerUnit" || prosessorToPower.tag == "EnemyComputerUnit")
            {
                Debug.Log("Calling power Link");
                powerSupply.GetComponent<powerSupply_Script>().powerLink(prosessorToPower);
            }
            
            else
            {
                Debug.Log("Trying to power an object that is not tagged as a ally or enemy computer unit: " + toPower);
            }
        }
        #endregion

        void Awake()
		{
			if (instance != null)
			{
				Debug.Log("There can only be one...");
			}

			else
			{
				instance = this;
			}
		}


		void initializeValues()
		{

		}


		void Start()
		{
			initializeValues();
		}


		void Update()
		{

		}

		public void initializeAttack()
		{
			processorV2_Script attackerScript = attacker.GetComponent<processorV2_Script>();
			processorV2_Script deffenderScript = deffender.GetComponent<processorV2_Script>();
			#region Attcker/Deffender Null Checks


			//Debug.Log("Attacker script: " + attackerScript);
			//Debug.Log("Deffender script: " + deffenderScript);


			if (attacker == null)
			{
				Debug.Log("Attacker is null");
				return;
			}

			else if (deffender == null)
			{
				Debug.Log("Deffender is null");
				return;
			}

			else if (attackerScript.getOwner() == 0)
			{
				attacker = null;
				deffender = null;
			}

			else
			{
				if (attacker != deffender)
				{
					attackerScript.setTarget(deffender);
					attacker = null;
					deffender = null;
				}

				else
				{
					attacker = null;
					deffender = null;
				}
			}

			#endregion
		}
	}
}