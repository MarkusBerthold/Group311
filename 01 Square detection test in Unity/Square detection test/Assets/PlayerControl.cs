using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float speed;
	public float horizontalSpeed;
	public float verticalSpeed;
	
	// Use this for initialization
	void Start () {

		Cursor.visible = false;

	}

	// Update is called once per frame
	void Update () {

		GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis ("Horizontal")*speed, Input.GetAxis("Jump"), Input.GetAxis ("Vertical")*speed);

		float h = horizontalSpeed * Input.GetAxis("Mouse X");
		float v = verticalSpeed * Input.GetAxis("Mouse Y");
		transform.Rotate(v, h, 0);
	}
}
