using UnityEngine;
using TMPro;


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

		[Header("Private Veriables: Scripts")]
		private processorUI_Script processorUIScript; //ATTACH CANVAS AND TEXT TO PROCESSOR PRESET
		gameMaster_Script gameMaster;
		processorUI_Script processorEnergyUIScript;

		[Header("Private Veribales: Boolean Values")]
		private bool underAttack;//set if the processor is under attack. GameMaster will set this if a processor sets this as a deffender
        private bool attacking; //weather or not this processor is attacking

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

		public void setTarget(GameObject targetX)
		{
			if (targetX != null)
			{
				targetCurrent = targetX;
			}
		}

        public GameObject getTarget()
        {
            return targetCurrent;
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
		#region attacker/attacking setter

		public void setAttacker(GameObject attackerX)
		{
			if(attackerX != null)
			{
				attacker = attackerX;
				updateEnergyLine();
			}
		}

		public GameObject getAttacker()
		{
			return attacker;
		}

        public void setAttacking(bool attackingX)
        {
                attacking = attackingX;
        }

        #endregion


        void initializeValues()
		{
			energyTransferRate = 1f;//processors have a base eTR speed on one pulse per second
			energyCreationRate = 2f;//processor have a base eCR of one energy every 2 seconds
			timeUntilNextPulse = 0f;//start this at 0 so processor can imediatly lunch a pulse
			timeUntilNextEnergyCreation = 1f;//start this at one so there is a slight delay on the fist energy added
			targetCurrent = null;//can't be too carful
			targetLast = null;//can't be too carful
			energyLineOne = null;
			attacker = null;
			gameMaster = gameMaster_Script.instance;//singleton patter, making sure all objects referance the same instance gameMaster script
			underAttack = false;
			rend = gameObject.GetComponent<Renderer>();//render attached to the processor used to set colour and material.
			startColor = rend.material.color;//Setting the starting color to what ever the color the object starts 
			Instantiate(processorUICanvasPrefab, transform);//The UI element that displays the processor Energy
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
            if((energyLineOne == null) && (targetCurrent != null))
            {
                drawEnergyLine(gameObject.transform, targetCurrent.transform);
            }

            if((energyLineOne != null) && (targetCurrent != null))
            {
                energyTransfer();
            }

			if(targetCurrent != targetLast)
			{
				updateEnergyLine();
			}
        }

		#region checks
		void targetChecks()
		{
            if(targetCurrent != null)
            {
               if(targetCurrent.GetComponent<processor_Script>().getTarget() == gameObject)
                {
                    underAttack = true;
                }

               else
                {
                    attacking = true;
                }  
            }

            else
            {
                underAttack = false;
                attacking = false;
            }
		}

		void attackerChecks()
		{
			if(attacker != null)
			{
				if (attacker.GetComponent<processor_Script>().getTarget() == this.gameObject)
				{

				}

				else
				{
					attacker = null;
					underAttack = false;
				}

			}
		}
		#endregion

		#region energy

		#region Pulse
		void energyTransfer()
		{
			if ((timeUntilNextPulse <= 0) && (attacking = true) && (energy > 0))
			{
				createPulse();
				energy--;
				timeUntilNextPulse = energyTransferRate;
			}
			timeUntilNextPulse -= Time.deltaTime;
		}

		#region create pulse
		void createPulse()
		{
			if (unitOwner == 1)
			{
				GameObject energyPulseGO = Instantiate(allyEnergyPulsePrefab, gameObject.transform);
				energyPulse_Script energyPulseGoScript = energyPulseGO.GetComponent<energyPulse_Script>();
				energyPulseGoScript.setTarget(targetCurrent);
				energyPulseGoScript.setOwner(unitOwner);
			}

			if (unitOwner == -1)
			{
				GameObject energyPulseGO = Instantiate(enemyEnergyPulsePrefab, gameObject.transform);
				energyPulse_Script energyPulseGoScript = energyPulseGO.GetComponent<energyPulse_Script>();
				energyPulseGoScript.setTarget(targetCurrent);
				energyPulseGoScript.setOwner(unitOwner);
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

		void drawEnergyLine(Transform p1, Transform p2)
		{
            Debug.Log("running draw line");
            if (this.getTarget() == this.getAttacker())
			{
				Debug.Log("running draw line underattack");
				if (energyLineOne != null)
				{
					Destroy(energyLineOne);
				}

				Vector3 midPosision = Vector3.Lerp(p1.position, p2.position, .5f);
				Vector3 posision = Vector3.Lerp(p1.position, midPosision, .5f);
				if (unitOwner == 1)
				{
					energyLineOne = Instantiate(allyEnergyLinePrefab);
				}

				if (unitOwner == -1)
				{
					energyLineOne = Instantiate(enemyEnergyLinePrefab);
				}

				Vector3 newScale = energyLineOne.transform.localScale;
				newScale.z = Vector3.Distance(p1.position, midPosision);
				energyLineOne.transform.localScale = newScale;
				energyLineOne.transform.Translate(posision, Space.World);
				energyLineOne.transform.LookAt(midPosision);
				underAttack = false;
			}


            else
            {
                Debug.Log("running draw line not underattack");
                if (energyLineOne != null)
                {
                    Destroy(energyLineOne);
                }

                Vector3 posision = Vector3.Lerp(p1.position, p2.position, .5f);
                if (unitOwner == 1)
                {
                    energyLineOne = Instantiate(allyEnergyLinePrefab);
                }

                if (unitOwner == -1)
                {
                    energyLineOne = Instantiate(enemyEnergyLinePrefab);
                }

                Vector3 newScale = energyLineOne.transform.localScale;
                newScale.z = Vector3.Distance(p1.position, p2.position);
                energyLineOne.transform.localScale = newScale;
                energyLineOne.transform.Translate(posision, Space.World);
                energyLineOne.transform.LookAt(p2.transform);
            }
		}

		public void updateEnergyLine()
		{
			if (targetCurrent != null)
			{
				drawEnergyLine(gameObject.transform, targetCurrent.transform);
			}
		}
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
				Debug.Log("Attacker and deffender still set to game objects");
			}
		}

		void OnMouseOver()
		{
            if (Input.GetMouseButtonDown(1))
            {
				Debug.Log("2");
				if (energyLineOne != null)
                {
                    Destroy(energyLineOne);
                }

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

