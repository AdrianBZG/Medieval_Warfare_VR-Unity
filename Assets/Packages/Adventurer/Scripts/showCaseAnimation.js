var animations : String[];
private var currentAnimationID : int;
private var currentAnimationName : String;
private var previousAnimationName : String;
private var showSword : boolean = true;
private var showShield : boolean = true;
private var armorPlate : boolean = false;
var adventurer : Transform;
var adventurerArmor : Transform;
var sword : Transform;
var shield : Transform;
//Sounds.
//footsteps.
var footStepAudio : Transform[];
var footStepAudioRun : Transform[];
private var lastFootStepTime : float;
private var footStepToogle : boolean = true;
//swoosh.
var swooshAudio : Transform;
private var lastSwooshTime : float;
private var swooshToogle : boolean = true;
//noise.
var noiseAudio : Transform;
private var lastNoiseTime : float;
private var noiseToogle : boolean = true;
var treasureChest : Transform;
var lever : Transform;

function Start(){
	currentAnimationID = 0;
	currentAnimationName = GetAnimationName(animations[currentAnimationID]);
	previousAnimationName = currentAnimationName;
	adventurer.GetComponent.<Animation>().Blend(currentAnimationName,1.0);
}

function Update(){
	GetComponent.<GUIText>().text = "Press left and right arrow keys to switch animations. Currently playing: ";
	GetComponent.<GUIText>().text += currentAnimationName;
	//Switch animations.
	var newAnimation : boolean = false;
	if(Input.GetKeyDown(KeyCode.RightArrow)){
		newAnimation = true;
		currentAnimationID ++;
		if(currentAnimationID >= animations.Length){
			currentAnimationID = 0;
		}
	}
	if(Input.GetKeyDown(KeyCode.LeftArrow)){
		newAnimation = true;
		currentAnimationID --;
		if(currentAnimationID < 0){
			currentAnimationID = animations.Length-1;
		}

	}	
	//Sword switch.
	if(Input.GetKeyDown(KeyCode.Alpha1)){
		if(showSword){
			showSword = false;
			newAnimation = true;
			sword.GetComponent.<Renderer>().enabled = false;
		}
		else{
			showSword = true;
			newAnimation = true;
			sword.GetComponent.<Renderer>().enabled = true;
		}
	}
	//Shield.
	if(Input.GetKeyDown(KeyCode.Alpha2)){
		if(showShield){
			showShield = false;
			shield.GetComponent.<Renderer>().enabled = false;
		}
		else{
			showShield = true;
			shield.GetComponent.<Renderer>().enabled = true;				
		}
	}
	//Outfit switch.
	var switchedOutfit : boolean = false;
	if(Input.GetKeyDown(KeyCode.Alpha3)){
		if (!armorPlate){
			adventurerArmor.Find("adventureArmor").GetComponent.<Renderer>().enabled = true;
			adventurer.Find("adventurer").GetComponent.<Renderer>().enabled = false;
			armorPlate = true;
		}
		else{
			adventurerArmor.Find("adventureArmor").GetComponent.<Renderer>().enabled = false;
			adventurer.Find("adventurer").GetComponent.<Renderer>().enabled = true;
			armorPlate = false;
		}
		newAnimation = true;
		switchedOutfit = true;
	}
	//Play animations.
	if(newAnimation){
		currentAnimationName = GetAnimationName(animations[currentAnimationID]);
		var normalizedTime : float;
		if(GetRewind(currentAnimationName)){
			normalizedTime = 0.0;
		}
		else{
			normalizedTime = adventurer.GetComponent.<Animation>()[previousAnimationName].normalizedTime;
		}
		//Adventurer.
		adventurer.GetComponent.<Animation>().Blend(previousAnimationName,0.0);
		adventurer.GetComponent.<Animation>().Blend(currentAnimationName, 1.0);
		adventurer.GetComponent.<Animation>()[currentAnimationName].normalizedTime = normalizedTime;
		//Armor plate.
		adventurerArmor.GetComponent.<Animation>().Blend(previousAnimationName,0.0);
		adventurerArmor.GetComponent.<Animation>().Blend(currentAnimationName, 1.0);
		adventurerArmor.GetComponent.<Animation>()[currentAnimationName].normalizedTime = normalizedTime;
		//Treasure chest.
		if(currentAnimationName == "openChest"){
			treasureChest.GetComponent.<Renderer>().enabled = true;
			treasureChest.GetChild(0).GetComponent.<Renderer>().enabled = true;					
		}
		else{
			treasureChest.GetComponent.<Renderer>().enabled = false;
			treasureChest.GetChild(0).GetComponent.<Renderer>().enabled = false;				
		}
		//Lever.
		if(currentAnimationName == "pullLever" || currentAnimationName == "pullLeverBack" ){
			lever.GetChild(0).GetComponent.<Renderer>().enabled = true;
			lever.GetChild(1).GetComponent.<Renderer>().enabled = true;					
		}
		else{
			lever.GetChild(0).GetComponent.<Renderer>().enabled = false;
			lever.GetChild(1).GetComponent.<Renderer>().enabled = false;			
		}
	}
	//Animation & Sound. 
	if(Time.time > lastFootStepTime + 0.4){
		footStepToogle = true;
	}
	if(Time.time > lastSwooshTime + 0.4){
		swooshToogle = true;
	}
	if(Time.time > lastNoiseTime + 0.4){
		noiseToogle = true;
	}
	//shield.GetComponent("heavyLimb").heavyness = 0.5;	
	switch(currentAnimationName){
		case "walk":
		case "walkSword":
		case "run":
		case "runSword":
		case "strafeR":
		case "strafeRSword":
		case "strafeL":
		case "strafeLSword":
		case "crouchWalk":
		case "crouchWalkSword":
		case "crouchStrafeR":
		case "crouchStrafeRSword":
		case "crouchStrafeL":
		case "crouchStrafeLSword":
			FootStepSound(0.95);
			FootStepSound(0.45);
			if(armorPlate){
				NoiseSound(0.95);
				NoiseSound(0.45);				
			}				
			break;
		case "sprint":
		case "sprintSword":
		case "crouchRun":
		case "crouchRunSword":
			FootStepRunSound(0.95);
			FootStepRunSound(0.45);
			if(armorPlate){
				NoiseSound(0.95);
				NoiseSound(0.45);				
			}							
			break;
		case "jump":
			FootStepSound(0.55);
			if(armorPlate){
				NoiseSound(0.55);
			}			
			break;
		case "swordStrike1":
			if(currentAnimationName != previousAnimationName){
				if(!showSword){
					showSword = true;
					sword.GetComponent.<Renderer>().enabled = true;
				}
			}
			SwooshSound(0.3);
			break;
		case "swordStrike1Stuck":
			if(currentAnimationName != previousAnimationName){
				if(!showSword){
					showSword = true;
					sword.GetComponent.<Renderer>().enabled = true;
				}
			}
			SwooshSound(0.3);
			FootStepRunSound(0.8);
			FootStepSound(0.95);
			if(armorPlate){
				NoiseSound(0.8);
			}			
			break;
		case "swordStrike2":
			if(currentAnimationName != previousAnimationName){
				if(!showSword){
					showSword = true;
					sword.GetComponent.<Renderer>().enabled = true;
				}
			}
			SwooshSound(0.4);
			break;
		case "swordStrike3":
			if(currentAnimationName != previousAnimationName){
				if(!showSword){
					showSword = true;
					sword.GetComponent.<Renderer>().enabled = true;
				}
			}
			FootStepRunSound(0.1);
			FootStepRunSound(0.28);
			FootStepRunSound(0.4);
			FootStepSound(0.8);
			FootStepSound(0.95);
			SwooshSound(0.5);
			if(armorPlate){
				NoiseSound(0.28);
				NoiseSound(0.8);				
			}			
			break;
		case "die1":
			if(currentAnimationName != previousAnimationName){
				if(showShield){
					showShield = false;
					shield.GetComponent.<Renderer>().enabled = false;	
				}
				if(showSword){
					showSword = false;
					sword.GetComponent.<Renderer>().enabled = false;
				}
			}
			FootStepRunSound(0.48);
			FootStepSound(0.8);
			if(armorPlate){
				NoiseSound(0.48);
				NoiseSound(0.8);				
			}			
			break;
		case "die2":
			if(currentAnimationName != previousAnimationName){
				if(showShield){
					showShield = false;
					shield.GetComponent.<Renderer>().enabled = false;	
				}
				if(showSword){
					showSword = false;
					sword.GetComponent.<Renderer>().enabled = false;
				}
			}
			FootStepRunSound(0.43);
			FootStepSound(0.8);
			if(armorPlate){
				NoiseSound(0.43);
				NoiseSound(0.8);				
			}			
			break;
		case "shieldBlock":
			//shield.GetComponent("heavyLimb").heavyness = 0.1;	
			if(currentAnimationName != previousAnimationName){
				if(!showShield){
					showShield = true;
					shield.GetComponent.<Renderer>().enabled = true;					
				}
				if(!showSword){
					showSword = true;
					sword.GetComponent.<Renderer>().enabled = true;					
				}
			}
			break;
		case "openChest":
			if(currentAnimationName != previousAnimationName){
				if(showShield){
					showShield = false;
					shield.GetComponent.<Renderer>().enabled = false;	
				}
				if(showSword){
					showSword = false;
					sword.GetComponent.<Renderer>().enabled = false;
				}
			}
			if(!treasureChest.GetComponent.<Animation>().IsPlaying("open")){
				treasureChest.GetComponent.<Animation>().Play("open");
			}
			treasureChest.GetComponent.<Animation>()["open"].normalizedTime = adventurer.GetComponent.<Animation>()["openChest"].normalizedTime;
			break;
		case "swordStance":
			if(currentAnimationName != previousAnimationName){
				if(!showSword){
					showSword = true;
					sword.GetComponent.<Renderer>().enabled = true;
				}
			}
			break;
		case "pullLever":
		case "pullLeverBack":
			if(currentAnimationName != previousAnimationName){
				if(showShield){
					showShield = false;
					shield.GetComponent.<Renderer>().enabled = false;	
				}
				if(showSword){
					showSword = false;
					sword.GetComponent.<Renderer>().enabled = false;
				}
			}
			if(currentAnimationName == "pullLever"){
				if(!lever.GetComponent.<Animation>().IsPlaying("leverPull")){
					lever.GetComponent.<Animation>().Play("leverPull");
				}
				lever.GetComponent.<Animation>()["leverPull"].normalizedTime = adventurer.GetComponent.<Animation>()["pullLever"].normalizedTime;			
			}
			else{
				if(!lever.GetComponent.<Animation>().IsPlaying("leverPullBack")){
					lever.GetComponent.<Animation>().Play("leverPullBack");
				}
				lever.GetComponent.<Animation>()["leverPullBack"].normalizedTime = adventurer.GetComponent.<Animation>()["pullLeverBack"].normalizedTime;				
			}
			break;
	}
	previousAnimationName = currentAnimationName;
}

function GetAnimationName(animationName : String):String{
	if(showSword){
		switch(animationName){
			case "idle":
			case "walk":
			case "run":
			case "sprint":
			case "strafeR":
			case "strafeL":
			case "crouch":
			case "crouchWalk":
			case "crouchRun":
			case "crouchStrafeR":
			case "crouchStrafeL":
			animationName += "Sword";
		}
	}
	return animationName;
}

function GetRewind(animationName : String):boolean{
	switch(animationName){
		case "jump":
		case "swordStance":
		case "swordStrike1":
		case "swordStrike1Stuck":
		case "swordStrike2":
		case "swordStrike3":
		case "die1":
		case "die2":
		case "openChest":
		case "leverPull":
		case "leverPullBack":
		return true;
	}
	return false;
}

function FootStepSound(normalizedTime : float){
	var currentNormalizedTime : float = adventurer.GetComponent.<Animation>()[currentAnimationName].normalizedTime - Mathf.Floor(adventurer.GetComponent.<Animation>()[currentAnimationName].normalizedTime);
	if(Mathf.Abs(currentNormalizedTime - normalizedTime) < 0.1 && footStepToogle){
		var randomID = Mathf.Floor(Random.value * footStepAudio.Length);
		footStepAudio[randomID].GetComponent.<AudioSource>().Play();
		footStepAudio[randomID].GetComponent.<AudioSource>().pitch = Random.Range(0.65,0.75);
		footStepAudio[randomID].GetComponent.<AudioSource>().volume = Random.Range(0.45,0.55);
		lastFootStepTime = Time.time;
		footStepToogle = false;
	}
}

function FootStepRunSound(normalizedTime : float){
	var currentNormalizedTime : float = adventurer.GetComponent.<Animation>()[currentAnimationName].normalizedTime - Mathf.Floor(adventurer.GetComponent.<Animation>()[currentAnimationName].normalizedTime);
	if(Mathf.Abs(currentNormalizedTime - normalizedTime) < 0.1 && footStepToogle){
		var randomID = Mathf.Floor(Random.value * footStepAudio.Length);
		footStepAudioRun[randomID].GetComponent.<AudioSource>().Play();
		footStepAudioRun[randomID].GetComponent.<AudioSource>().pitch = Random.Range(0.75,0.85);
		footStepAudioRun[randomID].GetComponent.<AudioSource>().volume = Random.Range(0.65,0.75);
		lastFootStepTime = Time.time;
		footStepToogle = false;
	}
}

function SwooshSound(normalizedTime : float){
	var currentNormalizedTime : float = adventurer.GetComponent.<Animation>()[currentAnimationName].normalizedTime - Mathf.Floor(adventurer.GetComponent.<Animation>()[currentAnimationName].normalizedTime);	
	if(Mathf.Abs(currentNormalizedTime - normalizedTime) < 0.1 && swooshToogle){
		swooshAudio.GetComponent.<AudioSource>().Play();
		swooshAudio.GetComponent.<AudioSource>().pitch = Random.Range(0.9,1.2);
		swooshAudio.GetComponent.<AudioSource>().volume = Random.Range(0.65,0.75);
		lastSwooshTime = Time.time;
		swooshToogle = false;
	}
}

function NoiseSound(normalizedTime : float){
	var currentNormalizedTime : float = adventurer.GetComponent.<Animation>()[currentAnimationName].normalizedTime - Mathf.Floor(adventurer.GetComponent.<Animation>()[currentAnimationName].normalizedTime);	
	if(Mathf.Abs(currentNormalizedTime - normalizedTime) < 0.1 && noiseToogle){
		noiseAudio.GetComponent.<AudioSource>().Play();
		noiseAudio.GetComponent.<AudioSource>().pitch = Random.Range(1.0,1.3);
		noiseAudio.GetComponent.<AudioSource>().volume = Random.Range(0.65,0.75);
		lastNoiseTime = Time.time;
		noiseToogle = false;
	}
}