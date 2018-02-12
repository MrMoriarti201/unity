using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour {


	public GameObject Floor;
	public GameObject Wall;
	public GameObject Escape;
	public bool isCeiling;
	private int X = 100;
	private int Y = 100;
	private char [,] grid;
	private int Difficulty;
	private int NumberOfRooms;
	Leaf leaf;

	// Use this for initialization
	void Start () {
		GenerateDungeon ();
	}
		
	void Update(){
		
	}

	void GenerateDungeon(){
		leaf = new Leaf();
		RandomDifficulty (ref X,ref Y,ref NumberOfRooms,ref Difficulty);
		Debug.Log ("Difficulty=" + Difficulty);
		grid=new char[Y,X];
		leaf.GenerateMap(ref grid,X,Y);
		leaf.GenerateLeafs(ref grid,X,Y,NumberOfRooms);
		Generate3D();
		CreateTriggerZone ();
	}

	private void CreateTriggerZone(){
		Vector3 temp = leaf.getTriggerZone();
		float x = temp.x;
		float y = temp.y;
		float z = temp.z;
		Vector3 trigger = new Vector3 (x * Floor.transform.lossyScale.x, Wall.transform.lossyScale.y/2, z * Floor.transform.lossyScale.z);
		GameObject zone = new GameObject ("Level Escape");
		zone.transform.position = trigger;
		zone.AddComponent<BoxCollider> ();
		BoxCollider volume = zone.GetComponent<BoxCollider> ();
		volume.isTrigger = true;
		volume.size = new Vector3(x,Wall.transform.lossyScale.y,z);
		volume.tag = "Exit";
		Instantiate (Escape, new Vector3(trigger.x,1.0f,trigger.z), Quaternion.identity, zone.transform);

	}

	public Vector3 StartPosition(){
		Vector3 temp = leaf.getStartPosition ();
		float x = temp.x;
		float y = temp.y;
		float z = temp.z;
		return new Vector3 (x * Floor.transform.lossyScale.x, y, z * Floor.transform.lossyScale.z);
	}
		
	public bool Check(){
		if (X * Y / NumberOfRooms > 20)
			return true;
		else return false;
	}

	public void RandomDifficulty( ref int A, ref int B,ref int C,ref int L){
		L = Random.Range (1, 5);
		switch (Difficulty) {
		case 1:
			
				A = Random.Range (20, 41);
				B = Random.Range (20, 41);
			do {
				C = Random.Range (3, 6);
			} while(!Check ());
			break;
		case 2:
			
				A = Random.Range (40, 81);
				B = Random.Range (40, 81);
			do {
				C = Random.Range (5, 10);
			} while(!Check ());
			break;
		case 3:
			
				A= Random.Range (80, 121);
				B = Random.Range (80, 121);
			do {
				C = Random.Range (9, 13);
			} while(!Check ());
			break;
		case 4:
			
				A = Random.Range (120, 161);
				B = Random.Range (120, 161);
			do {
				C = Random.Range (12, 17);
			} while(!Check ());
			break;
		}
	}

	public void Generate3D(){
		GameObject Map = new GameObject ("Dungeon");
		Map.transform.position = new Vector3 (0, 0, 0);
		for (int i = 0; i < Y; i++) {
			for (int j = 0; j < X; j++) {
				if (grid [i, j] == leaf.getChar('f')) {
					
					Instantiate (Wall, new Vector3 (j * Wall.transform.lossyScale.x, 0+Wall.transform.lossyScale.y/2, i * Wall.transform.lossyScale.z), Quaternion.identity,Map.transform);

				} else if (grid [i, j] == leaf.getChar('b') || grid [i, j] == leaf.getChar('c')) {
					
					Instantiate (Floor, new Vector3 (j * Floor.transform.lossyScale.x,0.5f, i * Floor.transform.lossyScale.z), Quaternion.identity,Map.transform);
					Floor.name = "Floor";
					if (isCeiling) {
						Instantiate (Floor, new Vector3 (j * Floor.transform.lossyScale.x, Wall.transform.lossyScale.y - 0.5f, i * Floor.transform.lossyScale.z), Quaternion.identity, Map.transform);
						Floor.name = "Ceiling";
					}


				}
			}
			
		}
	}


}
