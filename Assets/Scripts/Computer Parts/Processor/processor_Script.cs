using UnityEngine;
using TMPro;
using System;

namespace ProcessingUnits
{
	public class processor_Script : MonoBehaviour
	{

		[Header("Object Stats: Energy")]
		public int energy;//posstive energy is owned by player, 0 it nuetral, |negative| is owned by enemy
		public int unitOwner;//0 is nuteral, 1 is ally, -1 in enemy
		public float energyTransferRate;//speed that the processing unit(PU) can send out energy pulses
		public float energyCreationRate;//speed at which the unit can produce new energy

		[Header("Object Stats: Count Down Veriables")]
		public float timeUntilNextPulse;//count down veriable for energyTransferRate(eTR)
		public float timeUntilNextEnergyCreation;//count down veriable for energyCreationRate(eCR)
		public float[] timeUntilPulse; //Array of count down veriables
		public int totalEnergyLines; //The total ammount of current energy Lines
		public int maxEnergyLine; //The max amount of energy lines



		[Header("Unity Presets: Materials/Colours")]
		public Color hoverColor;//Colour of the processor when mouse hovers over it
		public Material allyProcessor_Material;//ally Processor material. Will be set when energy is > 0
		public Material nuetralProcessor_Material;//nuetral processor material. Will be set when energy = 1
		public Material enemyProcessor_Material;// enemy processor material. Will be set when energy < 0

		[Header("Unity Presets: GameObjects")]
		public GameObject allyEnergyPulsePrefab;//ally energy pulse prefab, used to display energy traveling from ally processors
		public GameObject allyEnergyLinePrefab;//ally energy line, drawn to connect attack/deffending processors to visualise lines of transfer
		public GameObject enemyEnergyPulsePrefab;//enemy energy pulse prefab, used to display energy traveling from enemy processors
		public GameObject enemyEnergyLinePrefab;//enemy energy line, drawn to connect attack/deffending processors to visualise lines of transfer

		[Header("Unity Presets: UI")]
		public Canvas processorUICanvasPrefab;//processor UI canvas that parents TMP text that displays |energy|



		[Header("Private Veriables: GameObjects")]
		private GameObject targetCurrent;//current target. If set to null there is not target. DO NOT ADD NULL CHECKS
		private GameObject targetLast;//used to determin if the target has changed, if lastTarget != target you need to update things like energy line
		private GameObject energyLineOne;//energy line
		private GameObject attacker;//the attacking gameobject, should be made into an array at some point I think. This might all become a null issue when i start ussing thread strength
		private GameObject[] energyLines; //array of energy Lines
		private GameObject[] targetsCurrent;
		private GameObject[] targetsLast;
		private GameObject[] attackers;


		[Header("Private Veriables: Scripts")]
		private processorUI_Script processorUIScript; //ATTACH CANVAS AND TEXT TO PROCESSOR PRESET
		private EnergyLine_Script EL;//energy line script of the processor
		gameMaster_Script gameMaster;
		processorUI_Script processorEnergyUIScript;

		[Header("Private Veribales: Boolean Values")]
		private bool underAttack;//set if the processor is under attack. GameMaster will set this if a processor sets this as a deffender
		private bool attacking; //weather or not this processor is attacking
		private bool[] underAttacks;
		private bool[] attackings;

		[Header("Private Veriables: Render")]
		private Renderer rend;//render attached to this script, use to set colour and material

		[Header("Private Veriable: Matrerial/Colour")]
		private Color startColor;//Colour of the processor at start of match. Does not need to be pre-assigned, will be set in initializeValues(iN)


		#region Energy Getter and Setter

		public void setEnergy(int energyToSetTo)
		{
			//sets energy to a value
			energy = energyToSetTo;
		}

		public void setEnergyPlusX(int energyToAdd)
		{
			//adds a value to energy
			energy += energyToAdd;
		}

		public void setEnergyMinusX(int energyToSubtract)
		{
			//subtracts a value from energy
			energy -= energyToSubtract;
		}

		public int getEnergy()
		{
			return energy;
		}

		#endregion
		#region target Getter and Setter

		public void setTarget(GameObject targetX, int i)
		{
			targetsCurrent[i] = targetX;
		}
		public void setTarget(GameObject targetsX)
		{
			if (targetsX != null)
			{
				for (int i = 0; i <= maxEnergyLine - 1; i++)
				{
					if (targetsX != null)
					{
						if (targetsCurrent[i] == null)
						{
							targetsCurrent[i] = targetsX;
							targetsX = null;
						}
					}
				}

				if (targetsX != null)
				{
					targetsCurrent[1] = targetsX;
				}
			}
		}

		public GameObject getTarget(int i)
		{
			return targetsCurrent[i];
		}

		public GameObject[] getTargets()
		{
			return targetsCurrent;
		}

		#endregion
		#region Unit Owner Getter/Setter

		public void setUnitOwner(int unitOwnerX)
		{
			unitOwner = unitOwnerX;
		}

		public int getUnitOwner()
		{
			return unitOwner;
		}

		#endregion
		#region attacker/attackers setter

		public void setAttackers(GameObject attackerX)
		{
			if (attackerX != null)
			{
				for (int i = 0; i <= maxEnergyLine - 1; i++)
				{
					if (attackerX != null)
					{
						if (attackers[i] == null)
						{
							attackers[i] = attackerX;
							attackerX = null;
						}
					}
				}

				if (attackerX != null)
				{
					attackers[1] = attackerX;
				}
			}
		}

		//public GameObject getAttacker()
		//{
		//	return attacker;
		//}

		//public void setAttacking(bool attackingX)
		//{
		//	attacking = attackingX;
		//}

		#endregion
		#region colour/material setters/getters

		public void setStartClolor(Color startColorX)
		{
			startColor = startColorX;
		}

		public Color getStartColor()
		{
			return startColor;
		}

		public void setHoverColor(Color hoverColorX)
		{
			hoverColor = hoverColorX;
		}

		public Color getHoverColor()
		{
			return hoverColor;
		}


		#endregion


		void initializeValues()
		{
			maxEnergyLine = 4; //The max number of energy line an object can handle
			energyTransferRate = .75f;//processors have a base eTR speed on one pulse per second
			energyCreationRate = 2f;//processor have a base eCR of one energy every 2 seconds
			timeUntilNextPulse = 0f;//start this at 0 so processor can imediatly lunch a pulse
			timeUntilPulse = new float[maxEnergyLine];
			for (int i = 0; i < timeUntilPulse.Length; i++)
			{
				timeUntilPulse[i] = 0;
			}
			timeUntilNextEnergyCreation = 1f;//start this at one so there is a slight delay on the fist energy added
			targetCurrent = null;//can't be too carful
			targetLast = null;//can't be too carful
			energyLineOne = null;
			attacker = null;
			gameMaster = gameMaster_Script.instance;//singleton patter, making sure all objects referance the same instance gameMaster script
			underAttack = false;
			rend = gameObject.GetComponent<Renderer>();//render attached to the processor used to set colour and material.
			EL = this.GetComponent<EnergyLine_Script>();//get the EnergyLine_Script
			startColor = rend.material.color;//Setting the starting color to what ever the color the object starts 
			Instantiate(processorUICanvasPrefab, transform);//The UI element that displays the processor Energy
			energyLines = new GameObject[maxEnergyLine];
			targetsCurrent = new GameObject[maxEnergyLine];
			targetsLast = new GameObject[maxEnergyLine];
			attackers = new GameObject[30]; //30 is temp, should be an unbound queue but I'm lazy ATM
			underAttacks = new bool[maxEnergyLine];
			attackings = new bool[maxEnergyLine];
		}


		void Start()
		{
			initializeValues();
			setMaterial();
		}


		void Update()
		{
			targetChecks();
			attackerChecks();
			energyCreation();
			run();
		}


		void run()
		{

			for (int i = 0; i <= maxEnergyLine - 1; i++)
			{
				//Debug.Log("i = " + i);
				//Debug.Log("Energy Line " + i + " = ");
				//Debug.Log(energyLines[i]);
				//Debug.Log("Target " + i + " = ");
				//Debug.Log(targetsCurrent[i]);
				if ((energyLines[i] == null) && (targetsCurrent[i] != null) && (targetsCurrent[i] != targetsLast[i]))
				{
					energyLines[i] = EL.drawEnergyLine(energyLines[i], underAttacks[i], unitOwner, this.transform, targetsCurrent[i].transform, i);
				}



				if (targetsCurrent[i] != targetsLast[i])
				{
					if (targetsCurrent[i] != null)
					{
						energyLines[i] = EL.updateEnergyLine(energyLines[i], underAttacks[i], unitOwner, this.transform, targetsCurrent[i].transform, i);
					}
					targetsLast[i] = targetsCurrent[i];
				}
			}

			energyTransfer();
		}

		#region checks
		void targetChecks()
		{
			for (int i = 0; i <= targetsCurrent.Length - 1; i++)
			{
				if (targetsCurrent[i] != null)
				{
					GameObject[] targetsTargets = targetsCurrent[i].GetComponent<processor_Script>().getTargets();
					for (int j = 0; j <= targetsTargets.Length - 1; j++)
					{
						if (targetsTargets[j] == this.gameObject)
						{
							if (targetsCurrent[j].tag == this.gameObject.tag)
							{
								underAttack = false;
							}
							else
							{
								underAttacks[i] = true;
							}

						}

						else
						{
							attackings[i] = true;
						}
					}
				}

				else
				{
					attackings[i] = false;
					underAttacks[i] = false;
				}
			}
		}

		void attackerChecks()
		{
			for (int i = 0; i <= targetsCurrent.Length - 1; i++)
			{
				if (attackers[i] != null)
				{
					GameObject[] attackerTargets = attackers[i].GetComponent<processor_Script>().getTargets();
					for (int j = 0; j <= attackerTargets.Length - 1; j++)
					{
						if (attackerTargets[j] == this.gameObject)
						{
							underAttacks[i] = true;
						}
					}
				}

				else
				{
					attackers[i] = null;
					underAttacks[i] = false;
				}

			}
		}
		#endregion

		#region energy

		#region Pulse
		void energyTransfer()
		{
			for (int i = 0; i < energyLines.Length; i++)
			{

				if ((energyLines[i] != null) && (targetsCurrent[i] != null))
				{
					if ((attackings[i] == true) && (timeUntilPulse[i] <= 0) && (energy > 0))
					{
						createPulse(i);
						energy--;
						timeUntilPulse[i] = energyTransferRate;
					}
					timeUntilPulse[i] -= Time.deltaTime;
				}
			}
		}

		#region create pulse
		void createPulse(int targetID)
		{
			Debug.Log("Create Pulse");
			Debug.Log("target ID is: " + targetID);
			if (unitOwner == 1)
			{
				GameObject energyPulseGO = Instantiate(allyEnergyPulsePrefab, gameObject.transform);
				energyPulse_Script energyPulseGoScript = energyPulseGO.GetComponent<energyPulse_Script>();
				energyPulseGoScript.setTarget(targetsCurrent[targetID]);
				energyPulseGoScript.setOwner(unitOwner);
				energyPulseGoScript.setOwnerHoverColor(hoverColor);
				energyPulseGoScript.setOwnerStartColor(startColor);
			}

			if (unitOwner == -1)
			{
				GameObject energyPulseGO = Instantiate(enemyEnergyPulsePrefab, gameObject.transform);
				energyPulse_Script energyPulseGoScript = energyPulseGO.GetComponent<energyPulse_Script>();
				energyPulseGoScript.setTarget(targetsCurrent[targetID]);
				energyPulseGoScript.setOwner(unitOwner);
				energyPulseGoScript.setOwnerHoverColor(hoverColor);
				energyPulseGoScript.setOwnerStartColor(startColor);

			}
		}
		#endregion
		#region reate energy
		void energyCreation()
		{
			if ((unitOwner != 0) && (timeUntilNextEnergyCreation <= 0))
			{
				energy++;
				timeUntilNextEnergyCreation = energyCreationRate * 1;
			}

			timeUntilNextEnergyCreation -= Time.deltaTime;
		}
		#endregion
		#region Remove Energy

		public void removeEnergyLine()
		{
			if (energyLineOne != null)
			{
				Destroy(energyLineOne);
			}
		}

		#endregion

		#endregion

		#region Line
		#endregion

		#endregion

		public void setMaterial()
		{
			if (unitOwner == 1)
			{
				rend.material = allyProcessor_Material;
				return;
			}

			if (unitOwner == -1)
			{
				rend.material = enemyProcessor_Material;
				return;
			}

			else
			{
				rend.material = nuetralProcessor_Material;
			}
		}

		#region Mouse

		void OnMouseEnter()
		{

			rend.material.color = hoverColor;
		}

		void OnMouseExit()
		{
			rend.material.color = startColor;
		}

		void OnMouseDown()
		{
			if (gameMaster.getAttacker() == null)
			{
				gameMaster.setAttacker(gameObject);
				return;
			}

			if ((gameMaster.getAttacker() != null) && (gameMaster.getDeffender() == null))
			{
				gameMaster.setDeffender(gameObject);
				gameMaster.initializeAttack();
				return;
			}

			else
			{
			}
		}

		void OnMouseOver()
		{
			if (Input.GetMouseButtonDown(1))
			{
				EL.deletEnergyLine(energyLineOne);

				attacking = false;
				targetCurrent = null;
			}
		}

		#endregion

		void OnTriggerEnter(Collider other)
		{
			if ((other.gameObject.tag == "EnemyEnergyPulse") && (other.gameObject.tag == "AllyEnergyPulse"))
			{
				energyPulse_Script energyPulse = other.gameObject.GetComponent<energyPulse_Script>();
				energyPulse.setDontCollid(true);
			}
		}

		void OnTriggerExit(Collider other)
		{
			if ((other.gameObject.tag == "EnemyEnergyPulse") && (other.gameObject.tag == "AllyEnergyPulse"))
			{
				energyPulse_Script energyPulse = other.gameObject.GetComponent<energyPulse_Script>();
				energyPulse.setDontCollid(false);
			}
		}
	}
}

