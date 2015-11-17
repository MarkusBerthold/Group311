using UnityEngine;
using System.Collections;

public class LaserCollision : MonoBehaviour {

    //public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

       

    }
    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Laser")
        {
            print("Suck my balls");
            PlayerControl.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // resets the velocity speed, so when the character is respawned, he won't bounce on the platform.
            PlayerControl.Instance.GetComponent<Rigidbody>().transform.position = PlayerControl.Instance.startPos;
            PlayerControl.Instance.GetComponent<Rigidbody>().transform.rotation = PlayerControl.Instance.startRot;

            
        }
    }
}
