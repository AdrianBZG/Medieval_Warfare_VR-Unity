using UnityEngine;
using System.Collections;

public class KeyInvoker : MonoBehaviour {


    public GameObject keyObject;

	public void instantiateKey ()
    {
        
        if (keyObject != null)
        {
            print("Instantiating key");
            Invoke("instantiate", 1f); 
        }
    }

    public void instantiate ()
    {
        Instantiate(keyObject, transform.position, Quaternion.identity);
    }


}
