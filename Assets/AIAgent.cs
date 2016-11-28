using UnityEngine;
using System.Collections;

public class AIAgent : MonoBehaviour {

	public int lifePoints = 100;
	public bool dead = false;
	private bool canBeHitten = true;
	public GameObject agentEngine = null;
	public RAIN.Entities.EntityRig agentEntityRig = null;
	public float timeToNextAttack = 0.0f;

	// Use this for initialization
	void Start () {
		this.lifePoints = 100;
		dead = false;
		canBeHitten = true;
		timeToNextAttack = 0.0f;
		agentEntityRig = GetComponentInChildren<RAIN.Entities.EntityRig>();
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToNextAttack > 0) {
			timeToNextAttack -= Time.deltaTime;
		} else {
			canBeHitten = true;
		}
	}

	public int getLifePoints() {
		return lifePoints;
	}

	public void setLifePoints(int lifePoints) {
		this.lifePoints = lifePoints;
	}

	private void checkDead() {
		if (this.lifePoints < 1 && !isDead()) {
			dead = true;
			agentEntityRig.Entity.IsActive = false;
			agentEngine.SetActive (false);
			GetComponent<Animation>().Play ("die1", PlayMode.StopAll);
		}
	}

	public void getDamage() {
		if (canBeHitten) {
			timeToNextAttack = 2.0f;
			this.lifePoints -= 20;
			checkDead ();
			canBeHitten = false;
		}
	}

	public void getDamage(int damage) {
		if (canBeHitten) {
			timeToNextAttack = 2.0f;
			this.lifePoints -= damage;
			checkDead ();
			canBeHitten = false;
		}
	}
		
	public bool isDead() {
		return dead;
	}
}
