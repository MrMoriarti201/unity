using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour {

	public Transform target;
	private Transform _myTransform;
	public float ms = 2.0f;
	public float offsetZ = 3.0f;
	public float offsetY = 0.0f;
	public float smooth = 2.0f;
	public Vector2 AngleLimit=new Vector2(-40,60);

	private float xRot;
	private float yRot;

	void Awake(){
		_myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		_myTransform.position = target.position - _myTransform.forward * offsetZ+_myTransform.up*offsetY;
	}

	void Update () {

		yRot += Input.GetAxis ("Mouse X") * ms;
		xRot -= Input.GetAxis ("Mouse Y") * ms;

		xRot = Mathf.Clamp (xRot, AngleLimit.x, AngleLimit.y);

	}

	// Update is called once per frame
	void LateUpdate(){
		CameraMove ();
	}

	private void CameraMove(){
		_myTransform.rotation = Quaternion.Euler (xRot, yRot, 0.0f);
		_myTransform.position = target.position - _myTransform.forward * offsetZ+_myTransform.up*offsetY;
	}
}
