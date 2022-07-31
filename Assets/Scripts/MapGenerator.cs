using UnityEngine;
using System.Collections;
using System;

public class MapGenerator: MonoBehaviour{
    [Header("Sprites")]
	public Sprite floor;
    public Sprite left;
    public Sprite leftDownCorner;
    public Sprite leftUpCorner;
    public Sprite up;
    public Sprite down;
    public Sprite right;
    public Sprite rightDownCorner;
    public Sprite rightUpCorner;
	public Sprite upLeftCornerConnector;
	public Sprite downLeftCornerConnector;
	public Sprite upRightCornerConnector;
	public Sprite downRightCornerConnector;
    public Sprite water;
	public Sprite bush;
	public Sprite berry;
	public Sprite rock;
	public Sprite iron;
	public Sprite lowerTree;
	public Sprite upperTree;
	
	[Header(" ")]
	public int width;
	public int height;
	public string seed;
	public bool useRandomSeed;

	[Range(0,60)]
	public int randomFillPercent; //recommended 45 

	int[,] map;
	int[,] bushMap;
	int[,] treeMap;
	int[,] rockMap;
	int[,] ironMap;
	int[,] berryMap;

	void Start() {
		Debug.Log("among us");
		GenerateMap();
		ObjectsPlacer(GenerateNoiseTexture(0.2f,0.6f),bushMap,bush,"bush",true);
		ObjectsPlacer(GenerateNoiseTexture(0.7f,0.8f),rockMap,rock,"rock",false);
		ObjectsPlacer(GenerateNoiseTexture(0.7f,0.7f),berryMap,berry,"berry",true);
		ObjectsPlacer(GenerateNoiseTexture(0.95f,0.9f),ironMap,iron,"iron",false);
		ObjectsPlacer2(GenerateNoiseTexture(0.8f,0.8f),treeMap,lowerTree,upperTree,"tree");
	
    }

	// generates the 2d arrays of all the types of objects and fills the 2d array of the map
	void GenerateMap() {
		map = new int[width,height];
		bushMap = new int[width,height];
		treeMap=new int[width,height];
		rockMap=new int[width,height];
		ironMap=new int[width,height];
		berryMap=new int[width,height];
		RandomFillMap();

		for (int i = 0; i < 5; i ++) {
			SmoothMap();
		}
		IslandGenerator();
	}
	//fills the 2d array of the map randomly
	void RandomFillMap() {
		if (useRandomSeed) {
			seed = UnityEngine.Random.Range(1f,10f).ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (x == 0 || x == width-1 || y == 0 || y == height -1) {
					map[x,y] = 1;
				}
				else {
					map[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1: 0;
				}
			}
		}
	}
	//makes the values of the 2d array of the map in the shape of a smooth island
	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					map[x,y] = 1;
				else if (neighbourWallTiles < 4)
					map[x,y] = 0;

			}
		}
	}
	//gets the surrounding wall count
	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}

		return wallCount;
	}
	//takes the values of the 2d array of the map and generates an island 
	void IslandGenerator(){
        GameObject island=new GameObject();                                                       //generate base of the world
        island.name="island";
        for(int y=0;y<height;y++){
            for(int x=0;x<width;x++){
                 GameObject currentTile;
                 currentTile=new GameObject();
                currentTile.transform.parent = island.transform; 
                 if (map[x,y]==1){
                    currentTile.AddComponent<SpriteRenderer>().sprite=water;
                    currentTile.name="water";
					 currentTile.AddComponent<BoxCollider2D>();
					 currentTile.GetComponent<BoxCollider2D>().size=new Vector2(1.4f,1.4f);
                 }
                 else{
                    if(map[x+1,y]==1 && map[x,y+1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=rightUpCorner;
                    	currentTile.name="upRightCornerFloor";
					}
					else if(map[x+1,y]==1 && map[x,y-1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=rightDownCorner;
                    	currentTile.name="downRightCornerFloor";
					}
					else if(map[x+1,y]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=right;
                    	currentTile.name="rightFloor";
					}
					else if(map[x-1,y]==1 && map[x,y+1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=leftUpCorner;
                    	currentTile.name="upLeftCornerFloor";
					}
					else if(map[x-1,y]==1 && map[x,y-1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=leftDownCorner;
                    	currentTile.name="downLeftCornerFloor";
					}
					else if(map[x-1,y]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=left;
                    	currentTile.name="leftFloor";
					}
					else if(map[x,y+1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=up;
                    	currentTile.name="upFloor";
					}
					else if(map[x,y-1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=down;
                    	currentTile.name="downFloor";
					}
					else if(map[x+1,y+1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=upRightCornerConnector;
                    	currentTile.name="upRightCornerConnector";
					}
					else if(map[x+1,y-1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=downRightCornerConnector;
                    	currentTile.name="downRightCornerConnector";
					}
					else if(map[x-1,y+1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=upLeftCornerConnector;
                    	currentTile.name="upLeftCornerConnector";
					}
					else if(map[x-1,y-1]==1){
						currentTile.AddComponent<SpriteRenderer>().sprite=downLeftCornerConnector;
                    	currentTile.name="downLeftCornerConnector";
					}
					else{
                    currentTile.AddComponent<SpriteRenderer>().sprite=floor;
                    currentTile.name="floor";
					}
                 }
                 
                 currentTile.transform.position=new Vector3(x,y,0);
                
            }
           
        }
    }
	//method that creates a noise texture
    public Texture2D GenerateNoiseTexture(float frequency, float limit)
    {                                                        

        Texture2D noiseTexture = new Texture2D(100, 100);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float v = Mathf.PerlinNoise((x + float.Parse(seed)) * frequency, (y +  float.Parse(seed)) * frequency);
                if (v > limit)
                    noiseTexture.SetPixel(x, y, Color.white);
                else
                    noiseTexture.SetPixel(x, y, Color.black);
            }
        }
        noiseTexture.Apply();
        return noiseTexture;
    }
	//method that places objects with 1 level (bushes,berries,rocks...) around the map and gives it a collider
	void ObjectsPlacer(Texture2D noise,int[,] arr,Sprite s,string name,Boolean isTrigger){
		GameObject objects=new GameObject();
		objects.name=name+"objects";
		for(int y=0;y<height;y++){
			for(int x=0;x<width;x++){
				if(map[x,y]==0 && noise.GetPixel(x,y).r>0.5f && map[x+1,y]==0 && map[x,y+1]==0 && map[x,y-1]==0 && map[x-1,y]==0 && map[x+1,y+1]==0 && map[x+1,y-1]==0 
				&& map[x-1,y-1]==0 && map[x-1,y+1]==0 && bushMap[x,y]==0 && rockMap[x,y]==0 && ironMap[x,y]==0 && berryMap[x,y]==0 ){
					GameObject b=new GameObject();
					b.transform.SetParent(objects.GetComponent<Transform>());
					b.AddComponent<SpriteRenderer>().sprite=s;
					b.AddComponent<BoxCollider2D>().isTrigger=isTrigger;
					b.GetComponent<BoxCollider2D>().size=new Vector2(0.5f,0.5f);
					b.transform.position=new Vector3(x,y,0);
					b.GetComponent<SpriteRenderer>().sortingOrder=2;
					arr[x,y]=1;
					b.name=name;
				}
			}
		}
	}
	//method that places objects with 2 levels (trees,big rocks...) around the map and give the lower level a collider
	void ObjectsPlacer2(Texture2D noise,int[,] arr,Sprite lower,Sprite upper,string name){
		GameObject objects2=new GameObject();
		
		objects2.name=name+"objects2";
		ObjectsPlacer(noise,arr,lower,"lower"+name,false);
		for(int y=1;y<height;y++){
			for(int x=0;x<width;x++){
				if(arr[x,y-1]==1 ){
					GameObject b=new GameObject();
					b.transform.SetParent(objects2.GetComponent<Transform>());
					
					b.AddComponent<SpriteRenderer>().sprite=upper;
					b.AddComponent<BoxCollider2D>().isTrigger=true;
					b.transform.position=new Vector3(x,y,0);
					b.GetComponent<SpriteRenderer>().sortingOrder=3;
					arr[x,y]=2;
					b.name="upper"+name;
					
				}
			}
		}

	}
	
	
}