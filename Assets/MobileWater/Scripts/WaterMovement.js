
// speed of water
var speed : float = 0.7;
// transparency of water.  
// 0 is transparent 
// 1 is opaque
var alpha : float = 0.5;

// size of wave texture
var waveScale : float = 3;

function Update () 
{
	var theTime = Time.time;
	var moveWater = Mathf.PingPong(theTime * speed, 100) * 0.15;
	gameObject.GetComponent.<Renderer>().material.mainTextureOffset = Vector2( moveWater, moveWater );	
	gameObject.GetComponent.<Renderer>().material.color.a = alpha;
	gameObject.GetComponent.<Renderer>().material.mainTextureScale = new Vector2(waveScale, waveScale);
}