using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ProcessingUnits
{
    public class processorV2_Script : MonoBehaviour
    {
        #region Veriables
        [Header("Object Stats: Data")]
        private int data;//how much data has been stored in the 
        private float dataTrans;//Acts as a multiplyer of 1 to change trans speed
        private float dataTransMin;//the slowest you can send data given low enough heat, also allows for 0 to stop trans without disconnecting thread
        private float dataTransMax;//the fastest you can send data given enoug data and low enough heat
        private int owner;
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
        public Color startColor;//colour of the processor when the round is started
        public Material allyProcessor_Material;//ally Processor material.
        public Material nuetralProcessor_Material;//nuetral processor material.
        public Material enemyProcessor_Material;// enemy processor material.
        public Material allyPulse_Material;//used for data trans bar
        public Material enemyPulse_Material;//used for data trans bar 

        [Header("Unity Presets: GameObjects")]
        public GameObject allyDataPulsePrefab;//object used to visualize data transfer
        public GameObject allyThreadPrefab;//the 'thread' data moves over
        public GameObject enemyDataPulsePrefab;//object used to visualize data transfer
        public GameObject enemyThreadPrefab;//the 'thread' data moves over

        [Header("Unity Presets: UI")]
        public Canvas processorUICanvasPrefab;//processor UI canvas that parents TMP text that displays |energy|
        public Image dataTranBar; //bar that displays data trans

        [Header("Private Veriables: GameObjects")]
        private GameObject[] threads;//array of threads
        private GameObject[] targetsLast;//A list of prev. targets, may or may not be used. Hopefully line updates will be done by the GameMaster
		private GameObject[] TargetsCurrent; //all current targets

		[Header("Private Veriables: Scripts")]
        private processorUI_Script processorUIScript; //ATTACH CANVAS AND TEXT TO PROCESSOR PRESET
        private EnergyLine_Script ELS;//energy line script of the processor
        gameMasterV2_Script gameMaster;
        processorUI_Script processorEnergyUIScript;

        [Header("Private Veribales: Boolean Values")]
        private bool[] targetDeffending;//true if the target is deffending. might not be used

        [Header("Private Veriables: Render")]
        private Renderer rend;//render attached to this script, use to set colour and material
        #endregion

        #region Getters/Setters

        #region ObjectStats: Data

        public void setData(int dataX)
        {
            if ((dataX >= 0) && (dataX < 100))
            {
                data = dataX;
            }
        }
        public void setDataPlus(int dataX)
        {
            if (data + dataX < 100)
            {
                data += dataX;
            }
        }
        public int getData()
        {
            return data;
        }

        public void setOwner(int ownerX)
        {
            owner = ownerX;

            if (ownerX == 1)
            {
                dataTranBar.material = allyPulse_Material;
            }

            else if (ownerX == -1)
            {
                dataTranBar.material = enemyPulse_Material;
            }

            else
            {
                dataTranBar.material = null;
            }

        }
        public int getOwner()
        {
            return owner;
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
        public float DataCreate { get; set; }

        #endregion
        #region ObjectStats: Power

        public float Power
        {
            get { return power; }
            set { power = value; }
        }
        
        public void powered(bool powerX)
        {
            if (powerX == true)
            {
                power = 1;
            }
            else
            {
                power = 0;
            }
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
        public void setStartColor(Color startColorX)
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
        #region Threads and Targets

        public GameObject GetTarget(int i)
        {
            return TargetsCurrent[i];
        }

        public void setTarget(GameObject targetX, int i)
        {
            TargetsCurrent[i] = targetX;
        }
        public void setTarget(GameObject targetsX)
        {
            if (targetsX != null)
            {
                for (int i = 0; i <= maxThreads - 1; i++)
                {
                    if (targetsX != null)
                    {
                        if (TargetsCurrent[i] == null)
                        {
                            TargetsCurrent[i] = targetsX;
                            targetsX = null;
                        }
                    }
                }

                if (targetsX != null)
                {
                    TargetsCurrent[1] = targetsX;
                }
            }
        }

        #endregion

        #endregion

        void initializeValues()
        {
            data = 0;
            dataTrans = 0.75f;
            dataTransMax = 2f;
            dataTransMin = 0.0f;
            DataCreate = 2;
            roundRobin = 1;
            power = 0;
            heat = 0;
            maxThreads = 4;
            dataCycle = 0;
            timeUntilPulse = new float[maxThreads];
            for (int i = 0; i < maxThreads; i++)
                timeUntilPulse[i] = 0;
            DataCreate = 1f;
            gameMaster = gameMasterV2_Script.instance;
            rend = gameObject.GetComponent<Renderer>();
            ELS = this.GetComponent<EnergyLine_Script>();
            startColor = rend.material.color;
            Instantiate(processorUICanvasPrefab, transform);//The UI element that displays the processor Energy
            threads = new GameObject[maxThreads];
            TargetsCurrent = new GameObject[maxThreads];
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
                if ((threads[i] == null) && (TargetsCurrent[i] != null) && (TargetsCurrent[i] != targetsLast[i]))
                {
                    threads[i] = ELS.drawEnergyLine(threads[i], this.gameObject, TargetsCurrent[i].gameObject, false, owner, this.transform, TargetsCurrent[i].transform, i);
                }
            }

            if ((owner != 0) && (attacking()))
            {
                energyTransfer();
            }


            if ((owner == 1) || (owner == -1))
            {
                dataTranBar.fillAmount = (Mathf.Ceil(100 * (dataTrans / dataTransMax))) / 100; //Inverts controls, the higher dataTrans the slower it transfers
            }
            else
            {
                dataTranBar.fillAmount = 0.0f;
            }
        }

        #region Data

        #region Pulse
        void energyTransfer()
        {
            for (int i = 0; i < threads.Length; i++)
            {

                if ((threads[i] != null) && (TargetsCurrent[i] != null))
                {
                    if ((TargetsCurrent[i] == true) && (timeUntilPulse[i] <= 0) && (data > maxThreads) && (TargetsCurrent[i].GetComponent<processorV2_Script>().getData() < 99) && (dataTrans != 0))
                    {
                        createPulse(i);
                        data--;
                        timeUntilPulse[i] = (1 / dataTrans);
                    }

                    else if ((TargetsCurrent[i] == true) && (timeUntilPulse[i] <= 0) && (data > 0) && (TargetsCurrent[i].GetComponent<processorV2_Script>().getData() < 99) && (dataTrans != 0))
                    {
                        if (i == roundRobin - 1)
                        {
                            createPulse(i);
                            data--;
                            timeUntilPulse[i] = (1 / dataTrans);

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


            //Used to increment the counter of roundRobin when the array of threads is no full
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
        #endregion
        #region Create Data
        void dataCreation()
        {
            if ((owner != 0) && (dataCycle <= 0) && (data < 99) && (power > 0))
            {
                data++;
                dataCycle = DataCreate * power;
            }

            dataCycle -= Time.deltaTime;
        }


        //Use the scroll wheel to move DataTrans between 0-2
        void dataTransRate()
        {
            float d = Input.GetAxis("Mouse ScrollWheel");

            if ((d > 0f) && (dataTrans + .25f <= dataTransMax))
            {
                dataTrans += .25f;
            }

            if ((d < 0f) && (dataTrans - .25f >= dataTransMin))
            {
                dataTrans -= .25f;
            }
        }
        #endregion
        #region create pulse
        void createPulse(int targetID)
        {
            if (owner == 1)
            {
                GameObject energyPulseGO = Instantiate(allyDataPulsePrefab, gameObject.transform);
                energyPulse_Script energyPulseGoScript = energyPulseGO.GetComponent<energyPulse_Script>();
                energyPulseGoScript.setTarget(TargetsCurrent[targetID]);
                energyPulseGoScript.setHome(this.gameObject);
                energyPulseGoScript.setOwner(owner);
                energyPulseGoScript.setOwnerHoverColor(hoverColor);
                energyPulseGoScript.setOwnerStartColor(startColor);
            }

            if (owner == -1)
            {
                GameObject energyPulseGO = Instantiate(enemyDataPulsePrefab, gameObject.transform);
                energyPulse_Script energyPulseGoScript = energyPulseGO.GetComponent<energyPulse_Script>();
                energyPulseGoScript.setTarget(TargetsCurrent[targetID]);
                energyPulseGoScript.setHome(this.gameObject);
                energyPulseGoScript.setOwner(owner);
                energyPulseGoScript.setOwnerHoverColor(hoverColor);
                energyPulseGoScript.setOwnerStartColor(startColor);

            }
        }

        #endregion
        #endregion



        #region Misc.

        public void setMaterial()
        {
            if (owner == 1)
            {
                rend.material = allyProcessor_Material;

            }

            else if (owner == -1)
            {
                rend.material = enemyProcessor_Material;
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

        void OnMouseOver()
        {
            dataTransRate();

            if (Input.GetMouseButtonDown(1))
            {
                gameMaster.setToPower(this.gameObject);
            }
        }

        void OnMouseExit()
        {
            if (owner == 1)
            {
                rend.material = allyProcessor_Material;
            }

            else if (owner == -1)
            {
                rend.material = enemyProcessor_Material;
            }


            else
            {
                rend.material = nuetralProcessor_Material;
            }
            //rend.material.color = StartColor;
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
