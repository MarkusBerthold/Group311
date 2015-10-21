using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float speed;
	public float horizontalSpeed;
	public float verticalSpeed;
	private bool jumpable = true;
	
	// Use this for initialization
	void Start () {

		Cursor.visible = false;
		this.tag = "Player";

	}

	// Update is called once per frame
	void Update () {


		transform.Rotate (0,Input.GetAxis("Horizontal") * speed/5,0);

		if (Input.GetKey(KeyCode.W))
		{
			transform.position += transform.forward/3;
		}

		if (Input.GetKey(KeyCode.S))
		{
			transform.position -= transform.forward/3;
		}


		if (Input.GetKey(KeyCode.Space) && jumpable) {
			GetComponent<Rigidbody>().velocity = new Vector3(0, Input.GetAxis("Jump")*3, 0);
			jumpable = false;
		}

		if (transform.position.y <= 1 && transform.position.y >= 0.95) {
			jumpable = true;
		}



		
		
		// float h = horizontalSpeed * Input.GetAxis("Mouse X");
		// float v = verticalSpeed * Input.GetAxis("Mouse Y");
		//transform.Rotate (v, h, 0);

	}


}
