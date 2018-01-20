using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour {

	public Transform target;
	public float PlayerSpeed=5.0f;
	public float JumpSpeed=20.0f;
	public float rotSpeed = 15.0f;

	private CharacterController _controller;
	private Transform _myTransform;
	Vector3 moveDir=Vector3.zero;

	void Awake(){
		_controller = GetComponent<CharacterController> ();
	}

	// Use this for initialization
	void Start () {
		_myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log (onGround());
		Debug.DrawRay (_myTransform.position, -_myTransform.up * _controller.height / 2);

			float horInput = Input.GetAxis ("Horizontal");
			float vertInput = Input.GetAxis ("Vertical");

			if (horInput != 0 || vertInput != 0) {
				moveDir.x = horInput;
				moveDir.z = vertInput;
			} else
				moveDir = Vector3.zero;

			if (moveDir.x != 0 || moveDir.z != 0) {
			
				//transform.rotation = Quaternion.Euler (0.0f,target.eulerAngles.y,0.0f); // Быстрый поворот
				Quaternion desiredRotation = Quaternion.Euler (0.0f, target.eulerAngles.y, 0.0f);
				transform.rotation = Quaternion.Slerp (_myTransform.rotation, desiredRotation, Time.deltaTime * rotSpeed);
			}


			moveDir = _myTransform.TransformDirection (moveDir);

			_controller.Move (moveDir * Time.deltaTime * PlayerSpeed);

	}

	private bool onGround(){
		RaycastHit info;

		float rayDistance = _controller.height/1.5f;
	
		if (Physics.Linecast (_myTransform.position, -_myTransform.up * rayDistance, out info)) {
			if (info.collider.tag == "Ground")
				return true;
			else
				return false;
		} else
			return false;
	}
}
