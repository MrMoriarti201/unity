using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

	public int thickness=4;

	public float x1,y1,x2,y2;

	public Room(float a1,float b1,float a2,float b2){
		x1 = a1;
		y1 = b1;
		x2 = a2;
		y2 = b2;
	}
}
