using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveScript : MonoBehaviour {

	public Transform cam;

	public float speed=7.5f;


	public float rotSpeed = 25f;

	private CharacterController _controller;

	private Vector3 movement = Vector3.zero;

	// Use this for initialization
	void Start () {
		_controller = GetComponent<CharacterController> ();
	}

	// Update is called once per frame
	void Update () {

		float x = Input.GetAxis ("Horizontal");
		float z = Input.GetAxis ("Vertical");

		if (x != 0 || z != 0) {
			movement.x = x;
			movement.z = z;
		} else
			movement = Vector3.zero;


		if (movement != Vector3.zero)
			RotatePlayer ();

		movement.y=0;

		_controller.Move (movement * Time.deltaTime * speed);

	}

	private void RotatePlayer(){

		movement = cam.TransformDirection (movement.x,0.0f,movement.z);

		Quaternion desiredRotation = Quaternion.LookRotation(movement);

		Quaternion temp = Quaternion.Euler (0.0f, desiredRotation.eulerAngles.y, 0.0f);

		transform.rotation = Quaternion.Slerp (transform.rotation,temp, Time.deltaTime * rotSpeed);

	}
}
