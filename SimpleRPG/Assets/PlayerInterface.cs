using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour {

	private PlayerStats stats;

	public Slider staminaBar;
	public Slider healthBar;


	void Awake(){
		stats = GameObject.Find ("Player").GetComponent<PlayerStats> ();
	}
	// Use this for initialization
	void Start () {
		staminaBar.value = staminaPercent ();
		healthBar.value = healthPercent ();
		
	}

	float staminaPercent(){
		return stats.getCurrentStamina ()/stats.Stamina;
	}

	float healthPercent(){
		return stats.getCurrentHealth ()/stats.Health;
	}
	
	// Update is called once per frame
	void Update () {
		staminaBar.value = staminaPercent ();
		healthBar.value = healthPercent ();
	}
}
