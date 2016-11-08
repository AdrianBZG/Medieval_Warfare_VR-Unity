var mipmap : float = -0.5;
function Start () {
	GetComponent.<Renderer>().material.mainTexture.mipMapBias = mipmap;
}