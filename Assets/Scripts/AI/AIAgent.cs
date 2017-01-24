using UnityEngine;
using System.Collections;

public class AIAgent : MonoBehaviour {

	public int lifePoints = 100;
	public bool dead = false;
    public bool isEnemyAI = false;
	private bool canBeHitten = true;
	public GameObject agentEngine = null;
	public RAIN.Entities.EntityRig agentEntityRig = null;
	public float timeToNextAttack = 0.0f;
    public GvrAudioSource damagedAudio;
	private string fatherGameObjectName;
	public HealthManager playerHealthManager;

    public MonoBehaviour soundManager;

    private AudioClip damaged;

	// Use this for initialization
	void Start () {
        if (tag == "Enemy") {
            GameManager.AddEnemy();
        }
        else if (tag == "Ally") {
            GameManager.AddAlly();
        }
        damaged  = damagedAudio.GetComponent<AudioClip>();
		this.lifePoints = 100;
		dead = false;
		canBeHitten = true;
		timeToNextAttack = 0.0f;
		agentEntityRig = GetComponentInChildren<RAIN.Entities.EntityRig>();
		fatherGameObjectName = transform.root.gameObject.name;
        lastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToNextAttack > 0) {
			timeToNextAttack -= Time.deltaTime;
		} else {
			canBeHitten = true;
		}
    }

    private Vector3 curPos;
    private Vector3 lastPos;

	public int getLifePoints() {
		return lifePoints;
	}

	public void setLifePoints(int lifePoints) {
		this.lifePoints = lifePoints;
	}

	private void checkDead() {
		if (gameObject.tag != "Player") {
			if (this.lifePoints < 1 && !isDead ()) {
				// Points manager
				if (isEnemyAI) {
					GameManager.KilledEnemy (50);
                    GameManager.WinPoints(50);
				} else {
					GameManager.KilledAlly (25);
				}
				dead = true;
				agentEntityRig.Entity.IsActive = false;
				agentEngine.SetActive (false);
				if (GameManager.allEnemiesDead ()) {
					GetComponent<KeyInvoker> ().instantiateKey ();
				}
                PlayDeadSound();
				GetComponent<Animation> ().Play ("die1", PlayMode.StopAll);
			}
		}
	}

	public void getDamage() {
		if (canBeHitten && tag != "Player") {
			timeToNextAttack = 2.0f;
			this.lifePoints -= 20;
			checkDead ();
			canBeHitten = false;
		} else if(canBeHitten && gameObject.tag == "Player") {
            timeToNextAttack = 2.0f;
            checkDead();
            canBeHitten = false;
			HealthManager.Damage (10);
		}
	}

	public void getDamage(int damage) {
		if (canBeHitten) {
			timeToNextAttack = 2.0f;
			this.lifePoints -= damage;
            if (!isDead())
                PlayDamageSound();
            checkDead ();
			canBeHitten = false;

        }
	}
		
	public bool isDead() {
		return dead;
	}

    public void PlayDamageSound ()
    {
        soundManager.Invoke("Damage", 0.0f);
    }

    public void PlayDeadSound ()
    {
        soundManager.Invoke("Dead", 0.0f);
    }

    public void PlayWalkSound ()
    {
        soundManager.Invoke("Walk", 0.0f);
    }

    public void StopWalkSound ()
    {
        soundManager.Invoke("StopWalking", 0.0f);
    }
}
