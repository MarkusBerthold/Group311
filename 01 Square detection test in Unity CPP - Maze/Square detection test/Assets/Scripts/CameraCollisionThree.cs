using UnityEngine;
using System.Collections;

public class CameraCollisionThree : MonoBehaviour
{
	Transform playerTransform;
	Quaternion targetLook;
	Vector3 targetMove;
	public float rayHitMoveInFront = 0.1f;
	Vector3 targetMoveUse;
	public float smoothLook = 0.5f;
	public float smoothMove = 0.5f;
	Vector3 smoothMoveV;
	public float distFromPlayer = 5.0f;
	public float heightFromPlayer = 2.0f;
	
	// Use this for initialization
	void Start ()
	{
		playerTransform = GameObject.FindWithTag("Player").transform;
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		targetMove = playerTransform.position + (playerTransform.rotation * new Vector3(0, heightFromPlayer, -distFromPlayer));
		
		RaycastHit hit;
		if (Physics.Raycast(playerTransform.position, targetMove - playerTransform.position, out hit, Vector3.Distance(playerTransform.position, targetMove)))
		{
			targetMoveUse = Vector3.Lerp (hit.point, playerTransform.position, rayHitMoveInFront);
		}
		else
		{
			targetMoveUse = targetMove;
		}
		
		transform.position = Vector3.SmoothDamp(transform.position, targetMoveUse, ref smoothMoveV, smoothMove);
		
	}
}