#pragma strict

// Test Scene script
var waters : GameObject[];
private var currentIndex: int = 0;

function Update () {
	if(Input.anyKeyDown)
	{
		// make sure we are not at end of array of waters
		if(currentIndex < waters.Length-1)
			currentIndex++;
		else
			currentIndex = 0;
		
		// hide all water objects
		for(var g : GameObject in  waters)
		{
			g.SetActiveRecursively(false);	
		}
		
		// show next one in array
		waters[currentIndex].SetActiveRecursively(true);
	}
}


