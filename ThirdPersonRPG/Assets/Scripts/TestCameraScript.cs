using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraScript : MonoBehaviour {

	public Transform target;

	private float xRot;
	private float yRot;

	public float ms = 2.0f;

	public Vector2 distance = new Vector2(3,1);
	public Vector2 angle = new Vector2(-15,45);

	// Use this for initialization
	void Start () {
		transform.position = target.position - transform.forward * distance.x + transform.up * distance.y;
	}

	// Update is called once per frame
	void Update () {

		yRot += Input.GetAxis ("Mouse X") * ms;
		xRot -= Input.GetAxis ("Mouse Y") * ms;

		xRot = Mathf.Clamp (xRot, angle.x, angle.y);



	}

	void LateUpdate(){
		transform.rotation = Quaternion.Euler (xRot, yRot, 0.0f);

		transform.position = target.position - transform.forward * distance.x + transform.up * distance.y;
	}
}
