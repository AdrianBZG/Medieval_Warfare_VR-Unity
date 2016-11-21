using UnityEngine;
using System.Collections;

public class AIAgent : MonoBehaviour {

	private int lifePoints = 100;
	private bool dead = false;
	public GameObject agentEntity = null;
	public GameObject agentEngine = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int getLifePoints() {
		return lifePoints;
	}

	public void setLifePoints(int lifePoints) {
		this.lifePoints = lifePoints;
	}

	private void checkDead() {
		if (this.lifePoints < 0 && !dead) {
			dead = true;
			agentEntity.SetActive (false);
			agentEngine.SetActive (false);
			GetComponent<Animation>().Play ("die1", PlayMode.StopAll);
		}
	}

	public void getDamage() {
		this.lifePoints -= 20;
		checkDead ();
	}

	public void getDamage(int damage) {
		this.lifePoints -= damage;
		checkDead ();
	}
		
	public bool isDead() {
		return dead;
	}
}
