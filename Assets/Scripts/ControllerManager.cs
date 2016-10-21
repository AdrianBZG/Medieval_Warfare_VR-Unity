using UnityEngine;
using System.Collections;

public class ControllerManager : MonoBehaviour {

    public float walkSpeed;

    public void Rotate(Vector3 eulerAngles)
    {

    }

    public void Move (Vector3 movement)
    {
        GetComponent<CharacterController>().Move(movement);
    }
}
