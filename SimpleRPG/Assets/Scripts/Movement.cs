using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float PlayerWalkSpeed=8.0f;
	public float PlayerRunSpeed=8.0f;
	public float RotationSpeed=15.0f;

	float turnSmoothVelocity;
	public float turnSmoothTime=0.12f;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;

	bool flashlight;

	[Range(0,1)]
	public float airControlPercent;

	float velocityY;
	public float JumpHeight=2.0f;
	public float Gravity = 9.8f;
	Vector3 startPosition;
	private Animator _animator;
	private CharacterController _controller;
	private PlayerStats _stats;
	public Transform _camera;
	private Transform _myTransform;
	public Transform GroundCheck;

	// Use this for initialization
	void Start () {
		flashlight = false;
		_myTransform = transform;
		startPosition = _myTransform.position;
		_controller = GetComponent<CharacterController> ();
		_animator = GetComponentInChildren<Animator> ();
		_stats = GetComponent<PlayerStats> ();
		_myTransform.position = GameObject.FindObjectOfType<Dungeon>().StartPosition ();
	}
	
	// Update is called once per frame
	void Update () {

		//Input
		Vector2 input = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		Vector2 inputDir = input.normalized;	
		//Bools
		bool running = (Input.GetKey (KeyCode.LeftShift ) && _stats.Sprint()>0);
		bool fall = !(_controller.isGrounded);
		bool attack =  (Input.GetKeyDown (KeyCode.Mouse0) && _stats.Attack() && !fall);
		bool specialAttack = (Input.GetKeyDown (KeyCode.Z) && !fall);
		bool slide = (Input.GetKey (KeyCode.LeftControl) && !fall);
		//Move
		Move (inputDir,running);
		bool jump=false;
		if (Input.GetKeyDown (KeyCode.Space)) {
			jump = true;
			Jump ();
		}
		//Restart Position
		if (Input.GetKey (KeyCode.T)) {
			_myTransform.position = startPosition;
		}
		//Flashlight
		if (Input.GetKey (KeyCode.F)) {
			flashlight = !flashlight;
			Light fl = gameObject.GetComponentInChildren<Light> ();
			fl.enabled = flashlight;

		}
			
			
		//Animator
		float animationSpeedPercent = ((running) ? currentSpeed/PlayerRunSpeed : currentSpeed/PlayerWalkSpeed*0.5f) * inputDir.magnitude;
		_animator.SetFloat ("speedPercent", animationSpeedPercent,speedSmoothTime,Time.deltaTime);
		//_animator.SetBool ("falling", !groundCheck());
		_animator.SetBool ("Attack", attack);
		_animator.SetBool ("SpecialAttack", specialAttack);
		_animator.SetBool ("Jump", jump);
		_animator.SetBool ("Slide", slide);
		jump = false;




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
