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

		[Header("Misc. Public Veriables")]

		[Header("Misc. Private Veriables")]
		private int owner; //Unity onwer (1 ally, 0 neutral, -1 enemy)
		private int maxPower; //max number of power cables allowed, will be broken down into actual pins at some point
		private GameObject[] powerCables; //array of power cables
		private Renderer rend; //render for this GameObject

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
            maxPower = 4;
            powerCables = new GameObject[maxPower];
            rend = this.GetComponent<Renderer>();
            startColor = rend.material.color;
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

        public void powerLink(GameObject toPower)
        {
            for (int i = 0; i < powerCables.Length; i++)
            {
                if (powerCables[i] == null)
                {
                    powerCables[i] = toPower;
                    toPower.GetComponent<processorV2_Script>().powered(true);
                    GameObject powerCable = Instantiate(powerCablePrefab);
                    powerCable.GetComponent<powerCable_Script>().drawPowerCable(this.gameObject.transform, toPower.transform, toPower.GetComponent<processorV2_Script>(), this, i);
                    gamemaster.setToPower(null);

                    return;
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
