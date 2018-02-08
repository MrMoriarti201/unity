using UnityEngine;
using System.Collections;

public class Leaf 
{
	public int x;
	public int y;
	public int w;
	public int h;
	public bool connected;
	public bool merged;
	public int ID;

	public Leaf(){
		x = 0;
		y = 0;
		w = 0;
		h = 0;
		connected = false;
		merged = false;
		ID = -1;
	}
}

