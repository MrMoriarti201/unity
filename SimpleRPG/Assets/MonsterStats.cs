using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStats : MonoBehaviour {

	public float health=200.0f;
	private float currentHealth;
	private bool MonsterDied;
	Animator anim;
	//public Slider healthBar;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<MobTrigger> ();
		currentHealth = health;
		MonsterDied = false;
		anim = GetComponent<Animator> ();
		//healthBar.value = healthPercent();
	}

	// Update is called once per frame
	void Update () {
		bool play;
		int damaged = GameObject.FindGameObjectWithTag ("Player").GetComponent<Movement> ().isAttacked ();
		if (damaged == 0)
			play = false;
		else
			play = true;
		if (gameObject.GetComponent<MobTrigger>().getStayTrigger()){
			if (damaged!=0) {
				TakeDamage (damaged);
			}
		}
		anim.SetBool ("Damaged", play);
		//healthBar.value = healthPercent();
	}

	float healthPercent (){
		return currentHealth / health;
	}

	public bool isDied(){
		return MonsterDied;
	}

	void TakeDamage(int damaged){
		float damage=0;
		if (damaged == 1) {
			damage = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ().getPlayerAttack ();
		} else if (damaged == 2) {
			damage=GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats>().getSP_PlayerAttack();
		}
		currentHealth -= damage;
		if (currentHealth <= 0) {
			MonsterDied = true;
			Debug.Log ("You killed an monster!");
		} else {
			Debug.Log ("Monster has a " + currentHealth + " HP");

		}
	}
}
