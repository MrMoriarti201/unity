using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

	const int MAX_LEAF_SIZE=20;
	public GameObject leaf;
	public float scale=10.0f;

	public int mapWidth = 50;
	public int mapHeight = 50;

	void Start(){
		
		List<Leaf> _leafs = new List<Leaf> ();
		//Leaf l;
		Leaf root = new Leaf (0,0, mapWidth, mapHeight);
		_leafs.Add (root);

		bool did_split = true;

		while (did_split) {
			did_split = false;
			foreach(Leaf l in _leafs.ToArray()){
				if (l.leftChild == null  && l.rightChild==null){
					if(l.width > MAX_LEAF_SIZE || l.height> MAX_LEAF_SIZE || Random.value > 0.25){
						if(l.split()){
							_leafs.Add(l.leftChild);
							_leafs.Add(l.rightChild);
							did_split=true;
						}
					}
				}
			}

		}
		root.createRooms ();

		foreach(Leaf l in _leafs.ToArray())
		{
			
			Debug.Log ("X:" + l.x + " Y:" + l.y + " width:" + l.width + " height:" + l.height); 
		}


			
	}
		

}
