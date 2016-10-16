using UnityEngine;
using System.Collections;

public class EventsManager : MonoBehaviour {

    public delegate void AreaReachedAction();
    public static event AreaReachedAction onAreaReached;

    public GameObject player;
    public Transform area;
    public float eventDistance;  // At this or least distance the event will onAreaReached will be emited
    bool eventEmited;

	// Use this for initialization
	void Start () {
        eventEmited = false;
	}
	
	// Update is called once per frame
	void Update () {

        print("Distance is: " + Vector3.Distance(player.transform.position, area.position));
        if (Vector3.Distance(player.transform.position, area.position) < eventDistance)
        {
            if (!eventEmited && onAreaReached != null)
            {
                onAreaReached();
                eventEmited = true;
            }
        }
	
	}
}
