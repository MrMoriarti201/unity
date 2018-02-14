using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobTrigger : MonoBehaviour {

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

		} else
			stayTrigger = false;
	}
}
