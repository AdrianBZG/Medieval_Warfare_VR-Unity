//walk 1.8
// run 3.5
//sprint 7.0
//strafe 2.3
//crouch walk 1.9
//crouch run 3.8
//crouch strafe 0.9

var speed : float = 1.0;
var direction : Vector3;
function Update () {
		var zLocalRotation : float = transform.localRotation.eulerAngles.z;
		transform.localRotation.eulerAngles.z = 0.0;
		transform.position += transform.TransformDirection(direction) * speed * Time.deltaTime;
		transform.localRotation.eulerAngles.z = zLocalRotation;		
}