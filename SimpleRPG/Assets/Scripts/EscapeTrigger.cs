using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTrigger : MonoBehaviour {

	private bool enterTrigger=false;
	private bool stayTrigger = false;

	public bool getEnterTrigger(){
		return enterTrigger;
	}

	public bool getStayTrigger(){
		return stayTrigger;
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			enterTrigger = true;
			Debug.Log ("You in Escape Zone!");
		} else
			enterTrigger = false;
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player") {
			enterTrigger = false;
		}
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Player") {
			stayTrigger = true;
			Debug.Log ("You staying in Escape Zone :O");
		} else
			stayTrigger = false;
	}
}
