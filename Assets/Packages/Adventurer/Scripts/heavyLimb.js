var limbs : Transform[];
private var rotations : Quaternion[];
private var animationRotations : Quaternion[];
var heavyness : float = 1.0;

function Start(){
	rotations = new Quaternion[limbs.Length];
	animationRotations = new Quaternion[limbs.Length];
	for (var i = 0; i < limbs.Length; i++){
		rotations[i] = limbs[i].localRotation;
		animationRotations[i] = limbs[i].localRotation;
	}
}

function Update () {
	if(GetComponent.<Renderer>().enabled){
		heavyness = Mathf.Max(heavyness, 0.01);
		for (var i = 0; i < limbs.Length; i++){
			limbs[i].localRotation = animationRotations[i];
			rotations[i] = Quaternion.Slerp(rotations[i], limbs[i].localRotation, Time.deltaTime / heavyness);
		}
	}
}

function LateUpdate(){
	if(GetComponent.<Renderer>().enabled){
		for(var i = 0; i < limbs.Length; i++){
			animationRotations[i] = limbs[i].localRotation;
			limbs[i].localRotation = rotations[i];
		}
	}
}