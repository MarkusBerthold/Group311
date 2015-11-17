using UnityEngine;
using System.Collections;

public class PlayerControl : Singleton<PlayerControl> {


	public float speed;
	public float horizontalSpeed;
	public float verticalSpeed;
	private bool jumpable = true;
	public Vector3 startPos;
    public Quaternion startRot;

	// Use this for initialization
	void Start () {

		//Cursor.visible = false;
		this.tag = "Player";
		startPos = transform.position; // sets the initial position of the character - based on how the cube is placed
        startRot = transform.rotation; // sets the initial rotation of the character - based on how the cube is placed
	}

    public float speed1 = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    // Update is called once per frame
    void Update () {

        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed1;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * 0.05f); // previously 0.05f was Time.deltaTime


        transform.Rotate (0,Input.GetAxis("Horizontal") * speed1/3.5f,0);

//	if (Input.GetKey(KeyCode.W))
//	{
//        //GetComponent<Rigidbody>().AddForce(0,0,100);
//        //transform.position += transform.forward/3;
//        
//	}
//
//	if (Input.GetKey(KeyCode.S))
//	{
//		//transform.position -= transform.forward/3;
//	}
//
//
//	if (Input.GetKey(KeyCode.Space) && jumpable) {
//		//GetComponent<Rigidbody>().velocity = new Vector3(0, Input.GetAxis("Jump")*3, 0);
//		jumpable = false;
//	}
//
//	if (transform.position.y <= 1 && transform.position.y >= 0.95) {
//		jumpable = true;
//	}

	if (transform.position.y <= -50) {

		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // resets the velocity speed, so when the character is respawned, he won't bounce on the platform.
		transform.position = startPos;
        transform.rotation = startRot;
	}
	// float h = horizontalSpeed * Input.GetAxis("Mouse X");
	// float v = verticalSpeed * Input.GetAxis("Mouse Y");
	//transform.Rotate (v, h, 0);

	}

	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Key") {
			Destroy(c.gameObject);
			print ("Key is obtained");
		}
        if (c.gameObject.tag == "Laser")
        {
            print("Hit by lightnings");
            // STILL NEEDS TO RESET THE FORCE OF THE PLAYER WHEN HIT
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // resets the velocity speed, so when the character is respawned, he won't bounce on the platform.
            transform.position = startPos;
            transform.rotation = startRot;
        }
	}

    

}
