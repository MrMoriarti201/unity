using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour {


	public GameObject Floor;
	public GameObject Wall;

	int X = 100;
	int Y = 100;
	char [,] grid;
	int NumberOfRooms=13;
	int RealNumberOfRooms;

	int Level;

	Leaf [] leafs;

	int RBH,RBW,RH,RW,_difference,NX,NY;

	char freeSpace='.';
	char border='#';
	char coridor='o';



	// Use this for initialization
	void Start () {
		RandomDifficulty (ref X,ref Y,ref NumberOfRooms,ref Level);
		Debug.Log ("Difficulty=" + Level);
		grid=new char[Y,X];
		GenerateMap();
		GenerateLeafs();
		Generate3D();
	}

	public bool Check(){
		if (X * Y / NumberOfRooms > 20)
			return true;
		else return false;
	}

	public void RandomDifficulty( ref int A, ref int B,ref int C,ref int L){
		L = Random.Range (1, 5);
		switch (Level) {
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
				if (grid [i, j] == freeSpace) {
					
					Instantiate (Wall, new Vector3 (j * Wall.transform.lossyScale.z, 0+Wall.transform.lossyScale.y/2, i * Wall.transform.lossyScale.x), Quaternion.identity,Map.transform);

				} else if (grid [i, j] == border || grid [i, j] == coridor) {
					
					Instantiate (Floor, new Vector3 (j * Floor.transform.lossyScale.z,0.5f, i * Floor.transform.lossyScale.x), Quaternion.identity,Map.transform);
					Floor.name = "Row" + j + " Column" + i;


				}
			}
			
		}
	}
	
	public void GenerateMap(){
		
	for (int i = 0; i < Y; i++) {
		for (int j = 0; j < X; j++) {
			grid [i,j] = freeSpace;
			}
		}

	}

	public int GetAmountOfRooms(){
		RBH=(int)Mathf.Sqrt(NumberOfRooms);
		RBW=(int)NumberOfRooms/RBH;

		_difference=NumberOfRooms-RBH*RBW;

		if(_difference==0)
		{
			RealNumberOfRooms=NumberOfRooms;
		}
		else
		{
			if(RBH>RBW)
			{
				RBW++;
			}
			else if(RBW>RBH)
			{
				RBH++;
			}
			else
			{
				if(Random.Range(0.0f,1.0f)>0.5f)
				{
					RBW++;
				}
				else RBH++;
			}

		}
		RealNumberOfRooms=RBH*RBW;

		int temp;

		if(X>Y)
		{
			if(RBW<RBH)
			{
				temp=RBH;
				RBH=RBW;
				RBW=temp;
				temp=0;
			}
		}
		else if( X < Y )
		{
			if(RBW > RBH)
			{
				temp=RBW;
				RBW=RBH;
				RBH=temp;
				temp=0;
			}
		}
		else
		{
			if(Random.Range(0.0f,1.0f)>0.5f)
			{
				temp=RBW;
				RBW=RBH;
				RBH=temp;
				temp=0;
			}
			else
			{
				temp=RBH;
				RBH=RBW;
				RBW=temp;
				temp=0;
			}
		}

		RH=Y/RBH;
		RW=X/RBW;

		return RealNumberOfRooms;
	}

	public void GenerateLeafs(){
		
		int amount=GetAmountOfRooms();

		leafs = new Leaf[amount];

		for (int i = 0; i < amount; i++) {
			leafs[i] = new Leaf();
		}

		int MinX=(int)RW/2;
		int MinY=(int)RH/2;

		for(int i=0;i<amount;i++)
		{
			leafs[i].merged =false;
			leafs[i].connected=false;
			leafs[i].ID=-1;
		}

		for(int A=0;A<amount;A++)
		{
			NX=(int)A%RBW;
			NY=(int)A/RBW;

			//Локальные координаты
			leafs[A].x=Random.Range(0,(RW-MinX-1));
			leafs[A].y=Random.Range(0,(RH-MinY-1));
			// Генерируем высоту и ширину в зависимости от локальных координат
			leafs[A].w=Random.Range(MinX,(RW-leafs[A].x-1));
			leafs[A].h=Random.Range(MinY,(RH-leafs[A].y-1));
			// Переводим локальные координаты в глобальные
			leafs[A].x += (NX*RW+1);
			leafs[A].y += (NY*RH+1);



			for(int i=leafs[A].y;i<leafs[A].y+leafs[A].h;i++)
			{
				for( int j=leafs[A].x;j<leafs[A].x+leafs[A].w;j++)
				{
					//	Debug.Log ("i:" + i + " j:" + j + "Number of Room:" + A);
					grid[i,j]=border;
				}
			}
		}
		// Начинаем растягивание комнат
		int idCounter=0;
		int diff=0;
		int breakCounter;
		do{
			for(int i=0;i<amount;i++)
			{

				if(Random.value>0.85f)
				{


					breakCounter=0;
					int direction=Random.Range(1,5);
					do{



						if( leafs[i].ID==-1 && (diff!=amount-NumberOfRooms))
						{

							int tx1,tx2,ty1,ty2;
							switch(direction)
							{
							case 1:    // вверх
								{
									if((int)i/RBW>0 && leafs[i-RBW].ID==-1)
									{
										ty1=leafs[i-RBW].y;
										ty2=leafs[i].y+leafs[i].h;

										if(leafs[i-RBW].x<leafs[i].x)
										{
											tx1=leafs[i-RBW].x;
											if(leafs[i-RBW].x+leafs[i-RBW].w < leafs[i].x+leafs[i].w )
											{
												tx2=leafs[i].x+leafs[i].w;
											}
											else tx2=leafs[i-RBW].x+leafs[i-RBW].w;




										}
										else if(leafs[i-RBW].x>leafs[i].x)
										{
											tx1=leafs[i].x;
											if(leafs[i-RBW].x+leafs[i-RBW].w < leafs[i].x+leafs[i].w )
											{
												tx2=leafs[i].x+leafs[i].w;
											}
											else tx2=leafs[i-RBW].x+leafs[i-RBW].w;


										}
										else
										{
											tx1=leafs[i].x;
											if(leafs[i].w < leafs[i-RBW].w)
											{
												tx2=tx1+leafs[i-RBW].w;
											}
											else if(leafs[i].w > leafs[i-RBW].w)
											{
												tx2=tx1+leafs[i].w;
											}
											else tx2=tx1+leafs[i].w;
										}

										for(int q=ty1;q<ty2;q++)
										{
											for(int j=tx1;j<tx2;j++)
											{
												grid[q,j]=border;
												//Draw();
											}
										}
										leafs[i].x=tx1;
										leafs[i-RBW].x=tx1;
										leafs[i].w=tx2-tx1;;
										leafs[i-RBW].w=tx2-tx1;;

										leafs[i].y=ty1;
										leafs[i-RBW].y=ty1;
										leafs[i].h=ty2-ty1;
										leafs[i-RBW].h=ty2-ty1;

										leafs[i].ID=idCounter;
										leafs[i-RBW].ID=idCounter;
										diff++;
									}
									break;
								}
							case 2:   // влево
								{
									if((int)i%RBW>0 && leafs[i-1].ID==-1)
									{
										tx1=leafs[i-1].x;
										tx2=leafs[i].x+leafs[i].w;

										if(leafs[i-1].y < leafs[i].y)
										{
											ty1=leafs[i-1].y;
											if(leafs[i-1].y+leafs[i-1].h < leafs[i].y+leafs[i].h)
											{
												ty2=leafs[i].y+leafs[i].h;
											}
											else ty2=leafs[i-1].y+leafs[i-1].h;
										}
										else if(leafs[i-1].y > leafs[i].y)
										{
											ty1=leafs[i].y;
											if(leafs[i-1].y+leafs[i-1].h < leafs[i].y+leafs[i].h)
											{
												ty2=leafs[i].y+leafs[i].h;
											}
											else ty2=leafs[i-1].y+leafs[i-1].h;

										}
										else
										{
											ty1=leafs[i].y;
											if(leafs[i].h > leafs[i-1].h)
											{
												ty2=ty1+leafs[i].h;
											}
											else if(leafs[i].h < leafs[i-1].h)
											{
												ty2=ty1+leafs[i-1].h;
											}
											else ty2=ty1+leafs[i].h;
										}

										for(int q=ty1;q<ty2;q++)
										{
											for(int j=tx1;j<tx2;j++)
											{
												grid[q,j]=border;
												//Draw();
											}
										}

										leafs[i].x=tx1;
										leafs[i-1].x=tx1;
										leafs[i].w=tx2-tx1;;
										leafs[i-1].w=tx2-tx1;;

										leafs[i].y=ty1;
										leafs[i-1].y=ty1;
										leafs[i].h=ty2-ty1;
										leafs[i-1].h=ty2-ty1;

										leafs[i].ID=idCounter;
										leafs[i-1].ID=idCounter;
										diff++;
									}
									break;
								}
							case 3: // вниз
								{
									if((int)i/RBW<RBH-1 && leafs[i+RBW].ID==-1)
									{
										ty1=leafs[i].y;
										ty2=leafs[i+RBW].y+leafs[i+RBW].h;

										if(leafs[i+RBW].x<leafs[i].x)
										{
											tx1=leafs[i+RBW].x;
											if(leafs[i+RBW].x+leafs[i+RBW].w < leafs[i].x+leafs[i].w )
											{
												tx2=leafs[i].x+leafs[i].w;
											}
											else tx2=leafs[i+RBW].x+leafs[i+RBW].w;

										}
										else if(leafs[i+RBW].x>leafs[i].x)
										{
											tx1=leafs[i].x;
											if(leafs[i+RBW].x+leafs[i+RBW].w < leafs[i].x+leafs[i].w )
											{
												tx2=leafs[i].x+leafs[i].w;
											}
											else tx2=leafs[i+RBW].x+leafs[i+RBW].w;

										}
										else
										{
											tx1=leafs[i].x;
											if(leafs[i].w < leafs[i+RBW].w)
											{
												tx2=tx1+leafs[i+RBW].w;
											}
											else if(leafs[i].w >leafs[i+RBW].w)
											{
												tx2=tx1+leafs[i].w;
											}
											else tx2=tx1+leafs[i].w;
										}

										for(int q=ty1;q<ty2;q++)
										{
											for(int j=tx1;j<tx2;j++)
											{
												grid[q,j]=border;
												//Draw();
											}
										}

										leafs[i].x=tx1;
										leafs[i+RBW].x=tx1;
										leafs[i].w=tx2-tx1;;
										leafs[i+RBW].w=tx2-tx1;;

										leafs[i].y=ty1;
										leafs[i+RBW].y=ty1;
										leafs[i].h=ty2-ty1;
										leafs[i+RBW].h=ty2-ty1;

										leafs[i].ID=idCounter;
										leafs[i+RBW].ID=idCounter;
										diff++;
									}
									break;
								}
							case 4:    // вправо
								{
									if((int)i%RBW<RBW-1 && leafs[i+1].ID==-1)
									{
										tx1=leafs[i].x;
										tx2=leafs[i+1].x+leafs[i+1].w;


										if(leafs[i+1].y < leafs[i].y)
										{
											ty1=leafs[i+1].y;
											if(leafs[i+1].y+leafs[i+1].h < leafs[i].y+leafs[i].h)
											{
												ty2=leafs[i].y+leafs[i].h;
											}
											else ty2=leafs[i+1].y+leafs[i+1].h;

										}
										else if(leafs[i+1].y > leafs[i].y)
										{
											ty1=leafs[i].y;
											if(leafs[i+1].y+leafs[i+1].h < leafs[i].y+leafs[i].h)
											{
												ty2=leafs[i].y+leafs[i].h;
											}
											else ty2=leafs[i+1].y+leafs[i+1].h;

										}
										else
										{
											ty1=leafs[i].y;
											if(leafs[i].h >leafs[i+1].h)
											{
												ty2=ty1+leafs[i].h;
											}
											else if(leafs[i].h < leafs[i+1].h)
											{
												ty2=ty1+leafs[i+1].h;
											}
											else ty2=ty1+leafs[i].h;
										}

										for(int q=ty1;q<ty2;q++)
										{
											for(int j=tx1;j<tx2;j++)
											{
												grid[q,j]=border;
												//Draw();
											}
										}

										leafs[i].x=tx1;
										leafs[i+1].x=tx1;
										leafs[i].w=tx2-tx1;
										leafs[i+1].w=tx2-tx1;

										leafs[i].y=ty1;
										leafs[i+1].y=ty1;
										leafs[i].h=ty2-ty1;
										leafs[i+1].h=ty2-ty1;

										leafs[i].ID=idCounter;
										leafs[i+1].ID=idCounter;
										diff++;
									}
									break;
								}
							}
							idCounter++;

							if(direction==5)
								direction=1;
							else direction++;

							if(breakCounter==4)
								break;
							else breakCounter++;

						}else if(diff==amount-NumberOfRooms)
						{
							break;
						}
					}while(leafs[i].ID==-1);
				}
			}
		}while(diff!=amount-NumberOfRooms);

		// Коридоры

		for(int t=0;t<amount;t++)
		{
			int cx1,cx2,cy1,cy2;
			int direction=Random.Range(1,3);
			int counter=0;
			do
			{
				if(!leafs[t].connected){
					switch(direction)
					{
					case 1:
						{
							if((int)t/RBW<RBH-1 && (leafs[t+RBW].ID==-1 || leafs[t+RBW].ID!=leafs[t].ID) && !leafs[t+RBW].connected)
							{
								cx1=MinX+(int)t%RBW*RW;
								cx2=cx1;

								if(leafs[t+RBW].x <= cx1-1 && leafs[t].x <= cx1-1)
								{
									cx1--;

								}

								if(((leafs[t+RBW].x+leafs[t+RBW].w) >= cx2+1) && ((leafs[t].x+leafs[t].w) >=cx2+1))
								{
									cx2++;

								}

								if(Random.value >0.5f)
								{
									if(leafs[t+RBW].x <= cx1-1 && leafs[t].x <= cx1-1)
									{
										cx1--;

									}

									else if(((leafs[t+RBW].x+leafs[t+RBW].w) >= cx2+1) && ((leafs[t].x+leafs[t].w) >=cx2+1))
									{
										cx2++;

									}
								}

								cy1=leafs[t].y+leafs[t].h;
								cy2=leafs[t+RBW].y;
								for(int i=cy1;i<cy2;i++)
								{
									for(int j=cx1;j<cx2;j++)
									{

										grid[i,j]=coridor;
									}
								}
								leafs[t].connected=true;
							}
							break;
						}
					case 2:
						{
							if((int)t%RBW<RBW-1 && (leafs[t+1].ID==-1 || leafs[t+1].ID!=leafs[t].ID) && !leafs[t+1].connected)
							{
								cy1=MinY+(int)t/RBW*RH;
								cy2=cy1;

								if(leafs[t+1].y <= cy1-1 && leafs[t].y <= cy1-1)
								{
									cy1--;
								}

								if(leafs[t+1].y+leafs[t+1].h >= cy2+1 && leafs[t].y+leafs[t].h >= cy2+1 )
								{
									cy2++;
								}

								if(Random.value > 0.5f)
								{
									if(leafs[t+1].y <= cy1-1 && leafs[t].y <= cy1-1)
									{
										cy1--;
									}
									else if(leafs[t+1].y+leafs[t+1].h >= cy2+1 && leafs[t].y+leafs[t].h >= cy2+1 )
									{
										cy2++;
									}
								}

								cx1=leafs[t].x+leafs[t].w;
								cx2=leafs[t+1].x;
								for(int i=cy1;i<cy2;i++)
								{
									for(int j=cx1;j<cx2;j++)
									{

										grid[i,j]=coridor;
									}
								}
								leafs[t].connected=true;
							}
							break;
						}
					}
				}

				if(direction>=3)
					direction=1;
				else direction++;

				if(counter==2)
					break;
				else counter++;

			}while(!leafs[t].connected);
		}
	}
		
}
