
function Update () {
	if(Input.GetMouseButton(0)){
		transform.rotation.eulerAngles.y += Input.GetAxis("Mouse X") * Time.deltaTime * -100.0;
	}
}