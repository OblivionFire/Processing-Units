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

		}
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
		private EnergyLine_Script EL;//energy line script of the processor
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

		#endregion

		#endregion



		void Start()
		{
			initializeValues();
		}


		void Update()
		{

		}
	}
}
