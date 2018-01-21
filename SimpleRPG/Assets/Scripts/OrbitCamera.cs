using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour {

	public Transform Player;

	public float mouseSensivity = 2.0f;

	public Vector2 offset = new Vector2(5.0f,0.0f);

	public float rotationSmoothTime = 0.08f;

	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	public Vector2 angle = new Vector2(-25.0f,40.0f);

	private float xRot;
	private float yRot;

	private Transform _myTransform;



	// Use this for initialization
	void Start () {
		_myTransform = transform;
		_myTransform.position = Player.position - _myTransform.forward * offset.x + _myTransform.up * offset.y;
	}

	void Update(){

		if(Input.GetKeyDown(KeyCode.Escape)){
			Cursor.visible=!Cursor.visible;
		}

		yRot += Input.GetAxis ("Mouse X") * mouseSensivity;
		xRot -= Input.GetAxis ("Mouse Y") * mouseSensivity;

		xRot = Mathf.Clamp (xRot, angle.x, angle.y);

	}

	// Update is called once per frame
	void LateUpdate () {
		currentRotation = Vector3.SmoothDamp (currentRotation, new Vector3 (xRot, yRot, 0.0f), ref rotationSmoothVelocity, rotationSmoothTime);
		_myTransform.eulerAngles = currentRotation;
		_myTransform.position = Player.position - _myTransform.forward * offset.x + _myTransform.up * offset.y;

	}
}
