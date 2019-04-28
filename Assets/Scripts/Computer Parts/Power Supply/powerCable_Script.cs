using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessingUnits
{
    public class powerCable_Script : MonoBehaviour
    {

		[Header("public variables")]
		public int slot;//used to store what 'slot' this cable is plugged into on the power supply
		public int power;

		[Header("Scripts")]
        public processorV2_Script target;
		public powerSupply_Script home;



		#region Getters/Setters

		public processorV2_Script getTarget()
		{
			return target;
		}
		
		#endregion
		// Use this for initialization
		void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void drawPowerCable(Transform p1, Transform p2, processorV2_Script targetProsessor, powerSupply_Script homeX, int i)
        {
            Vector3 posision = Vector3.Lerp(p1.position, p2.position, .5f);
            GameObject powerCableX = this.gameObject;
            Vector3 newScale = powerCableX.transform.localScale;
            newScale.z = Vector3.Distance(p1.position, p2.position);
            powerCableX.transform.localScale = newScale;
            powerCableX.transform.Translate(posision, Space.World);
            powerCableX.transform.LookAt(p2.transform);
            target = targetProsessor;
            slot = i;
            home = homeX;
        }

        public void deletePowerCable(GameObject energyLine)
        {
            if (energyLine != null)
            {
                Destroy(energyLine.gameObject);
            }
        }

        public void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                home.removeCable(slot);
                target.powered(false);
                deletePowerCable(this.gameObject);
            }
        }
    }
}