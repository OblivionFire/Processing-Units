using UnityEngine;
using TMPro;

namespace ProcessingUnits
{
	public class processorV2_Script : MonoBehaviour
	{
		#region Veriables
		[Header("Object Stats: Data")]
		private int data;//how much data has been stored in the processor
		private int owner;//0 is neurtral, 1 is ally, -1 enemy
		private float dataTrans;//how fast the processor can send data
		private float dataCreate;//how fast the processor can create data
		private int roundRobin;//used to roundRobin data

		[Header("Object Stats: Power")]
		private float power;//0.0<=x<=3.0

		[Header("Object Stats: Heat")]
		private int heat;//0<=x<=100 100 = shut down  65-85 is sweet spot


		[Header("Object Stats: Count Downs")]
		private float[] timeUntilPulse;//time until the next data pulse for each line
		private float dataCycle;//the time until data is created again
		private int maxThreads;//the max number of data lines

		[Header("Unity Presets: Materials/Colours")]
		public Color hoverColor;//Colour of the processor when moused over
		private Color startColor;//Colour of the processor at start of match. Does not need to be pre-assigned, will be set in initializeValues(iN)
		public Material allyProcessor_Material;//ally Processor material.
		public Material nuetralProcessor_Material;//nuetral processor material.
		public Material enemyProcessor_Material;// enemy processor material.

		[Header("Unity Presets: GameObjects")]
		public GameObject allyDataPulsePrefab;//object used to visualize data transfer
		public GameObject allyThreadPrefab;//the 'thread' data moves over
		public GameObject enemyDataPulsePrefab;//object used to visualize data transfer
		public GameObject enemyThreadPrefab;//the 'thread' data moves over

		[Header("Unity Presets: UI")]
		public Canvas processorUICanvasPrefab;//processor UI canvas that parents TMP text that displays |energy|

		[Header("Private Veriables: GameObjects")]
		private GameObject[] threads;//array of threads
		private GameObject[] targetsCurrent;//A list of current targets
		private GameObject[] targetsLast;//A list of prev. targets, may or may not be used. Hopefully line updates will be done by the GameMaster

		[Header("Private Veriables: Scripts")]
		private processorUI_Script processorUIScript; //ATTACH CANVAS AND TEXT TO PROCESSOR PRESET
		private EnergyLine_Script ELS;//energy line script of the processor
		gameMaster_Script gameMaster;
		processorUI_Script processorEnergyUIScript;

		[Header("Private Veribales: Boolean Values")]
		private bool[] targetDeffending;//true if the target is deffending. might not be used

		[Header("Private Veriables: Render")]
		private Renderer rend;//render attached to this script, use to set colour and material
		#endregion

		#region Getters/Setters

		#region ObjectStats: Data

		public int Data
		{
			get { return data; }
			set
			{
				if (value >= 0)
				{
					data = value;
				}
			}
		}
		public int Owner
		{
			get { return owner; }
			set { owner = value; }
		}
		public int RoundRobin
		{
			get { return roundRobin; }
			set
			{
				if ((value >= 0) && (value <= maxThreads))
				{
					roundRobin = value;
				}

			}
		}
		public float DataTrans
		{
			get { return dataTrans; }
			set { dataTrans = value; }
		}
		public float DataCreate
		{
			get { return dataCreate; }
			set { dataCreate = value; }
		}

		#endregion
		#region ObjectStats: Power

		public float Power
		{
			get { return power; }
			set { power = value; }
		}

		#endregion
		#region ObjectStats: Heat

		public int Heat
		{
			get { return heat; }
			set
			{
				if ((value >= 0) && (value <= 100))
				{
					heat = value;
				}

			}
		}

		#endregion
		#region ObjectStats: Color
		public Color StartColor
		{
			get { return startColor; }
			set
			{
				startColor = value;
			}
		}
		#endregion
		#region Threads and Targets

		public GameObject GetTarget(int i)
		{
			 return targetsCurrent[i];
		}

		public GameObject[] TargetsCurrent
		{
			get { return targetsCurrent; }
		}

		public void setTarget(GameObject targetX, int i)
		{
			targetsCurrent[i] = targetX;
		}
		public void setTarget(GameObject targetsX)
		{
			if (targetsX != null)
			{
				for (int i = 0; i <= maxThreads - 1; i++)
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

		#endregion

		#endregion

		void initializeValues()
		{
			data = 0;
			owner = 0;
			DataTrans = 0.75f;
			dataCreate = 2;
			roundRobin = 1;
			power = 1;
			heat = 0;
			maxThreads = 4;
			dataCycle = 0;
			timeUntilPulse = new float[maxThreads];
			for (int i = 0; i < maxThreads; i++)
				timeUntilPulse[i] = 0;
			dataCreate = 1f;
			gameMaster = gameMaster_Script.instance;
			rend = gameObject.GetComponent<Renderer>();
			ELS = this.GetComponent<EnergyLine_Script>();
			startColor = rend.material.color;
			threads = new GameObject[maxThreads];
			targetsCurrent = new GameObject[maxThreads];
			targetsLast = new GameObject[maxThreads];


		}

		void Start()
		{
			initializeValues();
			setMaterial();
			
		}


		void Update()
		{
			dataCreation();
			run();
		}

		void run()
		{

			for (int i = 0; i <= maxThreads - 1; i++)
			{
				if ((threads[i] == null) && (targetsCurrent[i] != null) && (targetsCurrent[i] != targetsLast[i]))
				{
					threads[i] = ELS.drawEnergyLine(threads[i], this.gameObject, targetsCurrent[i].gameObject, false, owner, this.transform, targetsCurrent[i].transform, i);
				}
			}

			if ((owner != 0) && (attacking()))
			{
				energyTransfer();
			}
		}

		#region Data

		#region Pulse
		void energyTransfer()
		{
			for (int i = 0; i < threads.Length; i++)
			{

				if ((threads[i] != null) && (targetsCurrent[i] != null))
				{
					if ((targetsCurrent[i] == true) && (timeUntilPulse[i] <= 0) && (data > maxThreads))
					{
						createPulse(i);
						data--;
						timeUntilPulse[i] = dataTrans;
					}

					else if ((targetsCurrent[i] == true) && (timeUntilPulse[i] <= 0) && (data > 0))
					{
						if (i == roundRobin - 1)
						{
							createPulse(i);
							data--;
							timeUntilPulse[i] = dataTrans;

							if (roundRobin < maxThreads - 1)
							{
								roundRobin++;
							}

							else
							{
								roundRobin = 1;
							}
						}
					}
				}
				timeUntilPulse[i] -= Time.deltaTime;
			}

			if (data < maxThreads)
			{
				for (int i = 1; i < threads.Length; i++)
				{
					if ((i == roundRobin) && (threads[i - 1] == null))
					{
						if (roundRobin < maxThreads - 1)
						{
							roundRobin++;
						}

						else
						{
							roundRobin = 1;
						}
					}
				}
			}
		}
		#region Create Data
		void dataCreation()
		{
			if ((owner != 0) && (dataCycle <= 0))
			{
				data++;
				dataCycle = dataCreate * 1;
			}

			dataCycle -= Time.deltaTime;
		}
		#endregion
		#region create pulse
		void createPulse(int targetID)
		{
			if (owner == 1)
			{
				GameObject energyPulseGO = Instantiate(allyDataPulsePrefab, gameObject.transform);
				energyPulse_Script energyPulseGoScript = energyPulseGO.GetComponent<energyPulse_Script>();
				energyPulseGoScript.setTarget(targetsCurrent[targetID]);
				energyPulseGoScript.setOwner(owner);
				energyPulseGoScript.setOwnerHoverColor(hoverColor);
				energyPulseGoScript.setOwnerStartColor(startColor);
			}

			if (owner == -1)
			{
				GameObject energyPulseGO = Instantiate(enemyDataPulsePrefab, gameObject.transform);
				energyPulse_Script energyPulseGoScript = energyPulseGO.GetComponent<energyPulse_Script>();
				energyPulseGoScript.setTarget(targetsCurrent[targetID]);
				energyPulseGoScript.setOwner(owner);
				energyPulseGoScript.setOwnerHoverColor(hoverColor);
				energyPulseGoScript.setOwnerStartColor(startColor);

			}
		}

		#endregion
		#endregion
		#endregion
		#region Misc.

		public void setMaterial()
		{
			if (owner == 1)
			{
				rend.material = allyProcessor_Material;
				return;
			}

			if (owner == -1)
			{
				rend.material = enemyProcessor_Material;
				return;
			}

			else
			{
				rend.material = nuetralProcessor_Material;
			}
		}

		bool attacking()
		{
			//Used to not run energyTrans when uncessisary
			for (int i = 0; i < threads.Length; i++)
			{
				if (threads[i] != null)
				{
					return true;
				}
			}

			return false;
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


		#endregion

		void OnTriggerEnter(Collider other)
		{
			if ((other.gameObject.tag == "EnemyEnergyPulse") || (other.gameObject.tag == "AllyEnergyPulse"))
			{
				energyPulse_Script energyPulse = other.gameObject.GetComponent<energyPulse_Script>();
				energyPulse.setDontCollid(true);
			}
		}

		void OnTriggerExit(Collider other)
		{
			if ((other.gameObject.tag == "EnemyEnergyPulse") || (other.gameObject.tag == "AllyEnergyPulse"))
			{
				energyPulse_Script energyPulse = other.gameObject.GetComponent<energyPulse_Script>();
				energyPulse.setDontCollid(false);
			}
		}

		#endregion
	}
}
