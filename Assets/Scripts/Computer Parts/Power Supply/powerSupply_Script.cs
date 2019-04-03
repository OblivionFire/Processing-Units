using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessingUnits
{
    public class powerSupply_Script : MonoBehaviour
    {

        [Header("Unity Presets: Game Objects")]
        public GameObject powerCablePrefab;

        [Header("Misc (Temp)")]
        private int maxPower;
        private GameObject[] powerCables;
        private Renderer rend;
        public Color hoverColor;
        private Color startColor;


        [Header("Scripts")]
        gameMasterV2_Script gamemaster;


        public void removeCable(int i)
        {
            powerCables[i] = null;
        }

        void initializeValues()
        {
            gamemaster = gameMasterV2_Script.instance;
            maxPower = 2;
            powerCables = new GameObject[maxPower];
            rend = this.GetComponent<Renderer>();
            startColor = rend.material.color;
        }


        // Use this for initialization
        void Start()
        {
            initializeValues();
            Debug.Log("Start, gamemaster: " + gamemaster);
            Debug.Log("Max Power: " + maxPower);
            Debug.Log("Power Cables: " + powerCables.Length);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void powerLink(GameObject toPower)
        {
            Debug.Log("pre for loop powerLink");
            Debug.Log("toPower: " + toPower);
            Debug.Log("gamemaster: " + gamemaster);
            Debug.Log("Power Cables length: " + powerCables.Length);
            for (int i = 0; i < powerCables.Length; i++)
            {
                Debug.Log("Mid For loop, i: " + i);
                Debug.Log("i~toPower: " + i + " ~ " + toPower);
                Debug.Log("i~gameMaster: " + i +" ~ " + gamemaster);
                if (powerCables[i] == null)
                {
                    Debug.Log("Inside if loop");
                    powerCables[i] = toPower;
                    toPower.GetComponent<processorV2_Script>().powered(true);
                    GameObject powerCable = Instantiate(powerCablePrefab);
                    powerCable.GetComponent<powerCable_Script>().drawPowerCable(this.gameObject.transform, toPower.transform, toPower.GetComponent<processorV2_Script>(), this, i);
                    gamemaster.setToPower(null);

                    return;
                }
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Get Mouse Down On Power Supply");
                Debug.Log("Game Master: " + gamemaster);
                Debug.Log("Power Cables: " + powerCables.Length);
            }
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
