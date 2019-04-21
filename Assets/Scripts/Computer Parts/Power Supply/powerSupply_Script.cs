using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessingUnits
{
    public class powerSupply_Script : MonoBehaviour
    {

        [Header("Unity Presets: Game Objects")]
        public GameObject powerCablePrefab; //power cable prefab

		[Header("Scripts")]
		gameMasterV2_Script gamemaster; //game master singleton instance

		[Header("Colors")]
        public Color hoverColor; //color of component when mouse is held over
        private Color startColor; //color of component when no mouse is present

        [Header("Power Lines")]
        public int maxPowerOut; //max power output capable, will be changed to watts at some point
        public int currentPowerOut; //current output
        private int maxPower; //max number of power cables allowed, will be broken down into actual pins at some point
        private GameObject[] powerCables; //array of power cables

        [Header("Misc. Private Veriables")]
		private int owner; //Unity onwer (1 ally, 0 neutral, -1 enemy)
		private Renderer rend; //render for this GameObject
		private bool visState;

		#region Getters/Setters

		public void setOwner(int ownerX)
		{
			owner = ownerX;
		}

		public int getOwner()
		{
			return owner;
		}

		#endregion

		void initializeValues()
        {
            gamemaster = gameMasterV2_Script.instance;
            maxPower = 3;
            maxPowerOut = 6;
            powerCables = new GameObject[maxPower];
            rend = this.GetComponent<Renderer>();
            startColor = rend.material.color;
			visState = true;
        }


        // Use this for initialization
        void Start()
        {
            initializeValues();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void powerLink(GameObject toPower, int currentProcessorPower)
        {
            if (currentProcessorPower == 0)
            {
                for (int i = 0; i < powerCables.Length; i++)
                {
                    if (currentPowerOut + 1 <= maxPowerOut)
                    {
                        if (powerCables[i] == null)
                        {
                            toPower.GetComponent<processorV2_Script>().powered(true);
                            GameObject powerCable = Instantiate(powerCablePrefab);
                            powerCables[i] = powerCable;
                            powerCable.GetComponent<powerCable_Script>().drawPowerCable(this.gameObject.transform, toPower.transform, toPower.GetComponent<processorV2_Script>(), this, i);
                            gamemaster.setToPower(null, 0);
                            currentPowerOut += 1;


                            if (visState == false)
                            {
                                powerCable.GetComponentInChildren<MeshRenderer>().enabled = false;
                            }

                            return;
                        }
                    }
                }
            }

            else
            {
                if(currentProcessorPower <= 2)
                {
                    toPower.GetComponent<processorV2_Script>().setPower(currentPowerOut + 1);
                }

                else if(currentProcessorPower == 3)
                {

                }
            }
        }

		public void setPowerLineVis(bool state)
		{
			visState = state;

			for(int i = 0; i < powerCables.Length; i++)
			{
				if(powerCables[i] != null)
				{
					powerCables[i].GetComponentInChildren<MeshRenderer>().enabled = state;
				}

				else
				{

				}
			}
		}

		public void removeCable(int i)
		{
			powerCables[i] = null;
		}

        void OnMouseEnter()
        {
            rend.material.color = hoverColor;
        }

        void OnMouseExit()
        {
            rend.material.color = startColor;
        }
    }
}
