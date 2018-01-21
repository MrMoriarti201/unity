using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public Transform _camera;
	private Transform _myTransform;
	public float PlayerWalkSpeed=8.0f;
	public float PlayerRunSpeed=8.0f;
	public float RotationSpeed=15.0f;

	[Range(0,1)]
	public float airControlPercent;

	float velocityY;
	public float JumpHeight=2.0f;
	public float Gravity = 9.8f;

	private CharacterController _controller;

	float turnSmoothVelocity;
	public float turnSmoothTime=0.12f;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;

	public float FallingDistance = 0.4f;

	private Animator _animator;

	// Use this for initialization
	void Start () {
		_myTransform = transform;
		_controller = GetComponent<CharacterController> ();
		_animator = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Input
		Vector2 input = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		Vector2 inputDir = input.normalized;

		bool running = Input.GetKey (KeyCode.LeftShift);
		bool attack =  Input.GetKeyDown (KeyCode.Mouse0);

		Move (inputDir,running);

		if (Input.GetKeyDown (KeyCode.Space)) {
			Jump ();
		}
			
		//Animator
		float animationSpeedPercent = ((running) ? currentSpeed/PlayerRunSpeed : currentSpeed/PlayerWalkSpeed*0.5f) * inputDir.magnitude;
		_animator.SetFloat ("speedPercent", animationSpeedPercent,speedSmoothTime,Time.deltaTime);
		_animator.SetBool ("falling", !(Physics.Raycast(_myTransform.position,-_myTransform.up,FallingDistance)));
		_animator.SetBool ("attack", attack);
		Debug.Log (!(Physics.Raycast (_myTransform.position, -_myTransform.up,FallingDistance)));
		Debug.DrawRay (_myTransform.position, -_myTransform.up * FallingDistance, Color.green);
	}

	private void Move(Vector2 inputDir,bool running){
		
		if (inputDir != Vector2.zero) {
			_myTransform.rotation = Quaternion.AngleAxis (Mathf.SmoothDampAngle(_myTransform.eulerAngles.y,
				(Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg+_camera.eulerAngles.y),
				ref turnSmoothVelocity,getModifiedSmoothTime(turnSmoothTime)), Vector3.up);
		}

	
		float targetSpeed = ((running) ? PlayerRunSpeed : PlayerWalkSpeed) * inputDir.magnitude;
	    currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, getModifiedSmoothTime(speedSmoothTime));

		velocityY -= Time.deltaTime * Gravity;
		Vector3 velocity = _myTransform.forward * currentSpeed+Vector3.up*velocityY;


		_controller.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (_controller.velocity.x, _controller.velocity.z).magnitude;



		if (_controller.isGrounded) {
			velocityY = 0;

		}


	}
		
	private void Jump(){
		if (_controller.isGrounded) {
			float jumpVelocity = Mathf.Sqrt (2 * Gravity * JumpHeight);
			velocityY = jumpVelocity;

		}
	}

	float getModifiedSmoothTime(float smoothTime){
		if (_controller.isGrounded) {
			return smoothTime;
		}
		if (airControlPercent == 0) {
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}


}
