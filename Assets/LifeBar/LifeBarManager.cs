using UnityEngine;
using System.Collections;

public class LifeBarManager : MonoBehaviour {
	
	public GameObject greenBar;
	private float initialGreenBarWidthScale;
	private float initialGreenBarPositionX;

	public GameObject redBar;

	public float movementCorrection;
	public int lifePoints;
	public int damage;
	private int maxLifePoints;

	private bool dead;

	// Use this for initialization
	void Start () {
		initialGreenBarWidthScale = greenBar.transform.localScale.z;
		initialGreenBarPositionX = greenBar.transform.localPosition.z;
		maxLifePoints = lifePoints;
		dead = false;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			TakeDamage (damage);
			SetGreenBarWidth();
		}
	}


	public void setLifePoints (int points) {
		lifePoints = points;
	}

	public void TakeDamage (int damage) {
		lifePoints -= damage;
		if (lifePoints < 0)  {
			dead = true;
			lifePoints = 0;
		}
	}

	public bool IsDead () {
		return dead;
	}

	public void Revive () {
		dead = false;
		lifePoints = maxLifePoints;
	}

	public void SetGreenBarWidth () {

		if (!IsDead()) {

			float lifeReduction = (float) lifePoints / (float) maxLifePoints;

			float newWidth = initialGreenBarWidthScale * lifeReduction;
			print (newWidth);

			Transform greenBarTrans = greenBar.transform;

			greenBarTrans.localScale = new Vector3 (greenBarTrans.localScale.x, greenBarTrans.localScale.y,newWidth);

			float reductionPercentage = newWidth / initialGreenBarWidthScale;

			greenBarTrans.localPosition -= 
				new Vector3 (0, 0, reductionPercentage * initialGreenBarWidthScale );
		}
	}
}
