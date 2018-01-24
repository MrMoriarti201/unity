using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf {

	private const int MIN_LEAF_SIZE=6;

	public float x,y,width,height;

	public Leaf leftChild;
	public Leaf rightChild;
	public Room room;
	public List<Room> halls;

	public Leaf(float X,float  Y,float  Width,float  Height){
		x = X;
		y = Y;
		width = Width;
		height = Height;
	}

	public bool split(){
		if (leftChild != null && rightChild != null)
			return false;

		bool splitH = Random.value > 0.5f;
		if (width > height && width / height > 1.25f)
			splitH = false;
		else if (height > width && height / width > 1.25f)
			splitH = true;

		float max = (splitH ? height : width) - MIN_LEAF_SIZE;

		if (max <= MIN_LEAF_SIZE)
			return false;
		
		float split = Random.Range (MIN_LEAF_SIZE, max + 1);

		if (splitH) {
			leftChild = new Leaf (x, y, width, split);
			rightChild = new Leaf (x, y + split, width, height - split);
		} else {
			leftChild = new Leaf (x, y, split, height);
			rightChild = new Leaf (x + split, y, width - split, height);
		}

		return true;
	}

	public void createRooms(){
		if (leftChild != null || rightChild != null) {
			if (leftChild != null) {
				leftChild.createRooms ();
			}
			if (rightChild != null) {
				rightChild.createRooms ();
			}
		} else {
			Vector2 roomSize;
			Vector2 roomPos;
			roomSize = new Vector2 (Random.Range (3, width - 1), Random.Range (3, height - 1));
			roomPos = new Vector2 (Random.Range (1, width - roomSize.x), Random.Range (1, height - roomSize.y));
			room = new Room (x + roomPos.x, y + roomPos.y, roomSize.x, roomSize.y);
		}
	}
}
