using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public float Health=100.0f;

	public float Stamina=100.0f;

	private float AttackCost=50.0f;
	private float SprintCost = 2.0f;
	private float StaminaReg;
	private float HeatlhReg = 0.5f;

	//float currentHealth;
	private float currentStamina;
	private float currentHealth;


	void Start(){
		currentHealth = Health;
		currentStamina = Stamina;
		StaminaReg = Stamina / 75;
	}

	void Update(){

		Regeneration ();


	}

	public float Sprint(){
		currentStamina -= SprintCost;
		currentStamina = Mathf.Clamp (currentStamina, 0.0f, Stamina);
		return currentStamina;
	}

	public bool Attack(){
		if (currentStamina - AttackCost > 0 ) {
			currentStamina -= AttackCost;
			return true;
		}
		else return false;

	}

	private void Regeneration(){
		if (currentStamina < Stamina) {
			currentStamina += StaminaReg;
			currentStamina = Mathf.Clamp (currentStamina, 0.0f, Stamina);
		}
	}

	public float getCurrentStamina(){
		return currentStamina;
	}

	public float getCurrentHealth(){
		return currentHealth;
	}
}
