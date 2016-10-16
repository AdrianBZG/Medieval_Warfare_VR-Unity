using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator))]
public class RootMovionScript : MonoBehaviour {

    bool canWalk;
    public float runSpeed;

    void OnEnable()
    {
        EventsManager.onAreaReached += CanWalk;
    }


    void OnDisable()
    {
        EventsManager.onAreaReached -= CanWalk;
    }

    void Start ()
    {
        canWalk = false;
    }

    void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();

        if (animator && canWalk)
        {
            print("CAN run!!");
            Vector3 newPosition = transform.position;
            newPosition.x += animator.GetFloat("Runspeed") * runSpeed;
            transform.position = newPosition;
        }
    }

    public void CanWalk ()
    {
        canWalk = true;
    }
}
