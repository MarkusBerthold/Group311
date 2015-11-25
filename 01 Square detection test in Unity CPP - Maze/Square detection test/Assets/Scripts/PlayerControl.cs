using UnityEngine;
using System.Collections;

public class PlayerControl : Singleton<PlayerControl> {


    public float speed;
    public float horizontalSpeed;
    public float verticalSpeed;
    //private bool jumpable = true;
    public Vector3 startPos;
    public Quaternion startRot;

    CharacterController controller;

    public Animator anim;

    // Use this for initialization
    void Start() {

        //AudioSource audio = GetComponent<AudioSource>();
        //audio.Play();

        //Cursor.visible = false;
        this.tag = "Player";
        startPos = transform.position; // sets the initial position of the character - based on how the cube is placed
        startRot = transform.rotation; // sets the initial rotation of the character - based on how the cube is placed
    }

    public float speed1 = 6F;
    public float jumpSpeed = 1.0F;
    public float gravity = 10.0F;
    private Vector3 moveDirection = Vector3.zero;

    // Update is called once per frame
    void Update() {

        controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed1;
            if (Input.GetButton("Jump"))
            {
				anim.SetBool("isRunning", false);
				anim.SetBool("isJumping", true);

                //jumpable = false;
                moveDirection.y = jumpSpeed/1.1f;
                
                GetComponent<AudioSource>().Play();

            }
            else
            {
                anim.SetBool("isJumping", false);
            }


        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * 0.05f); // previously 0.05f was Time.deltaTime


        transform.Rotate(0, Input.GetAxis("Horizontal") * speed1 / 3.5f, 0);




        /*   if (Input.GetKey(KeyCode.Space))
           {
               anim.SetBool("isJumping", true);
           }
           else if (Input.GetKeyDown(KeyCode.Space))
           {
               anim.SetBool("isJumping", true);
           }
           else if (Input.GetKeyUp(KeyCode.Space))
           {
               //anim.SetBool("isJumping", false);
           } */



           
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && !anim.GetBool("isJumping"))
           {
               anim.SetBool("isRunning", true);

           }
		else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
           {
               anim.SetBool("isRunning", false);
               }
    
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

//       if (transform.position.y <= 1 && transform.position.y >= 0.95) {
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

	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "Battery") {
			Destroy(c.gameObject);
			print ("Battery is obtained");

            Destroy(PreProcessing.Instance.getGoal());

            //Creation of the goal
            GameObject goal2 = (GameObject)Instantiate(Resources.Load("teleportGoalYesBattery"));
            goal2.transform.position = new Vector3(PreProcessing.Instance.getTeleportMeanX(), -1.7f, PreProcessing.Instance.getTeleportMeanZ());
            goal2.transform.localScale = new Vector3(80, 80, 80);
            goal2.name = "teleportGoalYesBattery";
            goal2.gameObject.tag = "Goal";

        }
        if (c.gameObject.tag == "Laser")
        {
            print("Hit by lightnings");
            // STILL NEEDS TO RESET THE MOMENTUM OF THE PLAYER WHEN HIT - possibly CharacterController
            
            transform.position = startPos;
            transform.rotation = startRot;
            
            

        }

        if (c.gameObject.tag == "Goal")
        {
            print("You win! - no cake");

            //NEED ANIMATION, BLINK AND ROTATION
        }
        
    }

    

}
