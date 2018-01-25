using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

	const int MAX_LEAF_SIZE=20;
	public GameObject leaf;


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
		int i = 0;
		foreach(Leaf l in _leafs.ToArray())
		{
			
			i++;
			Debug.Log ("Number" + i + " X:" + l.x + " Y:" + l.y + " width:" + l.width + " height:" + l.height);
			leaf.transform.localScale=new Vector3(l.width,1.0f,l.height);
			Instantiate (leaf, new Vector3 (l.x, 1.0f, l.y), leaf.transform.rotation);
		}


			
	}
		

}
