using UnityEngine;
using TMPro;

namespace ProcessingUnits
{
	public class energyPulse_Script : MonoBehaviour
	{

		//[Header("Object Stats")]

		//[Header("Unity Presets")]

		[Header("Private Veriables: Game Objects")]
		private GameObject target;//That targeted PU of this energy pulse

		[Header("Private Veriables: Object Stats")]
		private float speed;//the speed at which this energy pulse will move
		private int energy;//the amount over energy contained in this energy pulse
		private string tag;//the game tag of this object
		private bool dontCollid;
		private int owner; //numaric value of the owner 0 nuetral, 1 ally, -1 enemy
		private Color ownerStartColor;
		private Color ownerHoverColor;

		#region Target Getter/Setter
		public void setTarget(GameObject targetX)
		{
			if (targetX != null)
			{
				target = targetX;
			}
		}

		public GameObject getTarget()
		{
			return target;
		}

		#endregion
		#region dontCollid Setter

		public void setDontCollid(bool dontCollidX)
		{
			dontCollid = dontCollidX;
		}

		#endregion
		#region owner Getter/Setter

		public void setOwner (int ownerX)
		{
			if ((ownerX == 0) || (ownerX == 1) || (ownerX == -1))
			{
				owner = ownerX;
			}
		}

		public int getOwner()
		{
			return owner;
		}

		#endregion
		#region Color Getter/Setter

		public void setOwnerStartColor(Color ownerStartColorX)
		{
			ownerStartColor = ownerStartColorX;
		}

		public void setOwnerHoverColor(Color ownerHoverColorX)
		{
			ownerHoverColor = ownerHoverColorX;
		}

		#endregion

		void initializeValues()
		{
			speed = 5;
			energy = 1;
			tag = gameObject.tag;
		}


		void Start()
		{
			initializeValues();
		}


		void Update()
		{
			move();
		}

		void move()
		{
			Vector3 dir = target.transform.position - transform.position;
			float distanceThisFrame = speed * Time.deltaTime;

			if (dir.magnitude <= distanceThisFrame)
			{
				hitTarget();
				return;
			}

			transform.Translate(dir.normalized * distanceThisFrame, Space.World);

		}

		void hitTarget()
		{
			processor_Script t = target.GetComponent<processor_Script>();

			if (((t.tag == "EnemyComputerUnit") && (tag == "AllyEnergyPulse")) || ((t.tag == "AllyComputerUnit") && (tag == "EnemyEnergyPulse")))
			{
				if(t.getEnergy() <= 0)
				{ 
					t.setUnitOwner(owner);
					t.setMaterial();
					t.setHoverColor(ownerHoverColor);
					t.setStartClolor(ownerStartColor);
					
					if(owner == -1)
					{
						t.tag = "EnemyComputerUnit";
					}

					if(owner == 1)
					{
						t.tag = "AllyComputerUnit";
					}
				}
				else
				{
					t.setEnergyMinusX(energy);
				}
				Destroy(gameObject);
				return;
			}

			if (((t.tag == "EnemyComputerUnit") && (tag == "EnemyEnergyPulse")) || ((t.tag == "AllyComputerUnit") && (tag == "AllyEnergyPulse")))
			{
				t.setEnergyPlusX(energy);
				Destroy(gameObject);
				return;
			}
		}

		void OnTriggerEnter(Collider other)
		{
			if ((other.gameObject.tag == "EnemyEnergyPulse") && (dontCollid != true))
			{
				Destroy(other.gameObject);
				Destroy(gameObject);
				return;
			}
		}
	}
}