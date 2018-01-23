using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
	
	public GameObject prefab;
	public GameObject borderz;
	public int x = 20;
	public int y = 20;
	[Range(0,1)]
	public float factor=0.5f;
	public int WallHeight;

	char[,] map;

	private int amount;

	char border='x';

	char freeSpace='o';

	char _prefab = 'p';



	// Use this for initialization
	void Start () {

		GenerateBorders ();
		//GenerateWays ();
		LoadingPrefabs ();
	

	

	}

	void GenerateBorders(){

		map = new char[x, y];

		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				if (i == 0 || i ==x-1) {
					map [i, j] = border;
				}
				else if (j == 0 || j == y-1) {
					map [i, j] = border;
				}
				else map [i, j] = freeSpace;
			}
		}

	}

	void GenerateWays(){
		
		amount = Mathf.RoundToInt((x-1)* (y-1)*factor);

		int startX = Random.Range (1, x-1);
		int startY = Random.Range (1, y-1);

		map [startX, startY] = _prefab;

		int currentX, currentY;

		currentX = startX;
		currentY = startY;


		int dir;
		int lastdir=0;

		for (int i = 0; i < amount; i++) {

			dir = Random.Range (1, 5);
	

			bool stuck=((map [currentX + 1, currentY]) != freeSpace && (map [currentX - 1,currentY]) != freeSpace
				&& (map [currentX , currentY + 1]) != freeSpace && (map [currentX,currentY - 1]) != freeSpace);

			if(stuck)
			{
				dir=0;
			}


			if (lastdir == dir) {
				while (lastdir != dir) {
					dir = Random.Range (1, 5);
				}
			}

			switch (dir) {
			case 1:
				{
					if (currentX < x-1) {
						if (map [currentX + 1, currentY] == freeSpace) {
							currentX++;
							map [currentX, currentY] = _prefab;
						}
					} else
						return;
					break;
				}
			case 2:
				{
					if (currentX > 1) {
						if (map [currentX - 1, currentY] == freeSpace) {
							currentX--;
							map [currentX, currentY] = _prefab;
						}
					} else
						return;
					break;
				}
			case 3:
				{
					if (currentY < y-1) {
						if (map [currentX, currentY + 1] == freeSpace) {
							currentY++;
							map [currentX, currentY] = _prefab;
						}
					} else
						return;
					break;
				}
			case 4:
				{
					if (currentY > 1) {
						if (map [currentX, currentY - 1] == freeSpace) {
							currentY--;
							map [currentX, currentY] = _prefab;
						}
					} else
						return;
					break;
				}
			default:
				{
					int tempx,tempy;
					do{
						tempx= Random.Range(1,x-1);
						tempy= Random.Range(1,y-1);
						amount++;
					}while(map[tempx,tempy]!=freeSpace);
					currentX=tempx;
					currentY=tempy;
					break;
				}

			}
			//Debug.Log ("CurrentX=" + currentX + " CurrentY=" + currentY);
			lastdir = dir;

		}
	}

	void LoadingPrefabs(){
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				if (map [i, j] == _prefab) {
					borderz.transform.localScale = new Vector3 (borderz.transform.lossyScale.x, WallHeight, borderz.transform.lossyScale.z);
					Instantiate (borderz, new Vector3 (i * borderz.transform.lossyScale.x, 0, j * borderz.transform.lossyScale.z), borderz.transform.rotation);
					Debug.Log ("border");
				} else if (map [i, j] == border) {
					Instantiate (prefab,new Vector3(i * prefab.transform.lossyScale.x, 0, j * prefab.transform.lossyScale.z),prefab.transform.rotation);
					Debug.Log ("floor");
				} else
					continue;
			}
		}
	}
}
