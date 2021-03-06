﻿using UnityEngine;
using TMPro;

namespace ProcessingUnits
{
	public class gameMaster_Script : MonoBehaviour
	{

		[Header("Object Stats")]
		public static gameMaster_Script instance;//singleton patttern. Basically there can only ever be one gameMaster so it sets itself to instance and everything else referances instance
		public GameObject processorAllyPrefab;//Ally processor prefab used to spawn in levels
		public GameObject processorEnemyPrefab;//Enemy processor prefab used to spwn in levels

		[Header("Private Veriable: GameObjects")]
		private GameObject attacker;//the offencive processor(first selected)
		private GameObject deffender;//the deffencive processor(second selected)

		#region Attacker Setter and Getter

		public void setAttacker(GameObject attackerX)
		{
			if (attackerX != null)
			{
				attacker = attackerX;
			}

			else
			{
				Debug.Log("attacker does not equal null");
			}
		}

		public GameObject getAttacker()
		{
			return attacker;
		}

		#endregion
		#region Deffender Setter and Getter

		public void setDeffender(GameObject deffenderX)
		{
			if (deffenderX != null)
			{
				deffender = deffenderX;
			}

			else
			{
				Debug.Log("Deffender does not equal null");
			}
		}

		public GameObject getDeffender()
		{
			return deffender;
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
			GameObject ally = Instantiate(processorAllyPrefab);
			GameObject enemy = Instantiate(processorEnemyPrefab);
			processor_Script allyScript = ally.GetComponent<processor_Script>();
			processor_Script enemyScript = enemy.GetComponent<processor_Script>();
			allyScript.setEnergy(5);
			enemyScript.setEnergy(5);
			allyScript.setUnitOwner(1);
			enemyScript.setUnitOwner(-1);
			allyScript.hoverColor = processorAllyPrefab.GetComponent<processor_Script>().hoverColor;
			enemyScript.hoverColor = processorEnemyPrefab.GetComponent<processor_Script>().hoverColor;

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
			processor_Script attackerScript = attacker.GetComponent<processor_Script>();
			processor_Script deffenderScript = deffender.GetComponent<processor_Script>();
			#region Attcker/Deffender Null Checks

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

			else
			{
				attackerScript.setTarget(deffender);
				deffenderScript.setAttacker(attacker);
                attackerScript.removeEnergyLine();
                deffenderScript.removeEnergyLine();
                attackerScript.setAttacking(true);
                attacker = null;
				deffender = null;
			}

			#endregion
		}
	}
}