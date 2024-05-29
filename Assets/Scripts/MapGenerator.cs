using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //Prefabs
    [SerializeField] GameObject GameTile;
    [SerializeField] Material[] Materials = new Material[9]; //0 = Bomb

    public tile[] GenerateNewMap(int width, int height, int numOfBombs)
    {
        tile[] tiles = new tile[width*height];

        for (int i = 0; i < numOfBombs; i++)//Generates and places Bombs at random on the tile Array
        {
            int newBombIndex = 0;
            bool validBombIndex = false;
            while (!validBombIndex)
            {
                newBombIndex = UnityEngine.Random.Range(0, tiles.Length);
                if (tiles[newBombIndex] == null )
                {
                    validBombIndex = true;
                }
            }
            tiles[newBombIndex] = new tile() { numOfBombsTouching = -1, type = tile.tileType.Bomb };
        }

        //Checks all other tiles and assigns them a number
        for(int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] == null)
            {
                tiles[i] = new tile() { numOfBombsTouching = 0, type = tile.tileType.Number};

                if (i - width - 1 >= 0 && i % width != 0 && tiles[i - width - 1] != null)//checks top left tile
                {
                    if (tiles[i - width - 1].type == tile.tileType.Bomb)
                    {
                        tiles[i].numOfBombsTouching++;
                    }
                }
                if (i - width >= 0 && tiles[i - width] != null)//checks top middle tile
                {
                        if (tiles[i - width].type == tile.tileType.Bomb)
                        {
                            tiles[i].numOfBombsTouching++;
                        }
                }
                if (i - width + 1 >= 0 && (i+1)%width != 0 && tiles[i - width + 1] != null)//checks top right tile
                {
                    if (tiles[i - width + 1].type == tile.tileType.Bomb)
                    {
                        tiles[i].numOfBombsTouching++;
                    }
                }
                if (i - 1 >= 0 && tiles[i - 1] != null && i % width != 0)//checks middle left tile
                {
                    if (tiles[i - 1].type == tile.tileType.Bomb)
                    {
                        tiles[i].numOfBombsTouching++;
                    }
                }
                if (i + 1 < tiles.Length && (i + 1) % width != 0 && tiles[i + 1] != null)//checks middle right tile
                {
                    if (tiles[i + 1].type == tile.tileType.Bomb)
                    {
                        tiles[i].numOfBombsTouching++;
                    }
                }
                if (i + width - 1 < tiles.Length && i % width != 0 && tiles[i + width - 1] != null)//checks bottom left tile
                {
                    if (tiles[i + width - 1].type == tile.tileType.Bomb)
                    {
                        tiles[i].numOfBombsTouching++;
                    }
                }
                if (i + width < tiles.Length && tiles[i + width] != null)//checks bottom middle tile
                {
                    if (tiles[i + width].type == tile.tileType.Bomb)
                    {
                        tiles[i].numOfBombsTouching++;
                    }
                }
                if (i + width + 1 < tiles.Length && (i + 1) % width != 0 && tiles[i + width + 1] != null)//checks bottom right tile
                {
                    if (tiles[i + width + 1].type == tile.tileType.Bomb)
                    {
                        tiles[i].numOfBombsTouching++;
                    }
                } 
            }
        }
        //DrawMapToLog(width, tiles);
        return tiles;
    }

    public void DrawMapToLog(int width, tile[] tiles)
    {
        string map = "";

        for(int i = 0; i < tiles.Length; i++)
        {
            if((i+1)%width != 0)
            {
                if(tiles[i].numOfBombsTouching != -1)
                {
                    map += " " + tiles[i].numOfBombsTouching + ", ";
                }
                else
                {
                    map += tiles[i].numOfBombsTouching + ", "; 
                }
            }
            else
            {
                if (tiles[i].numOfBombsTouching != -1)
                {
                    map += " " + tiles[i].numOfBombsTouching + ", \n";
                }
                else
                {
                    map += tiles[i].numOfBombsTouching + ", \n";
                }
            }
        }
        Debug.Log(map);
    }

    public GameObject[] GeneratePhysicalMap(int width, tile[] tiles, GameManager gameManager, FlagCounter flagCounter)
    {
        GameObject[] GameTiles = new GameObject[tiles.Length];

        for(int i = 0, currentXPos = 0, currentZPos = 0; i < tiles.Length; i++)
        {
            GameTiles[i] = Instantiate(GameTile, new Vector3(currentXPos, 0.01f, currentZPos), Quaternion.identity, transform);
            GameTiles[i].GetComponent<TileInterations>().gameManager = gameManager;
            GameTiles[i].GetComponent<TileInterations>().flagCounter = flagCounter;
            GameTiles[i].GetComponent<TileInterations>().tile = tiles[i];
            switch (tiles[i].numOfBombsTouching)
            {
                case -1:
                    GameTiles[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Materials[0];
                    break;
                case 1:
                    GameTiles[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Materials[1];
                    break;
                case 2:
                    GameTiles[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Materials[2];
                    break;
                case 3:
                    GameTiles[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Materials[3];
                    break;
                case 4:
                    GameTiles[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Materials[4];
                    break;
                case 5:
                    GameTiles[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Materials[5];
                    break;
                case 6:
                    GameTiles[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Materials[6];
                    break;
                case 7:
                    GameTiles[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Materials[7];
                    break;
                case 8:
                    GameTiles[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Materials[8];
                    break;
            }
            currentXPos -= 2;
            if ((i+1)%width == 0)
            {
                currentXPos = 0;
                currentZPos += 2;
            }
        }

        return GameTiles;

    }

    public class tile
    {
        public enum tileType { Bomb, Number };
        public tileType type;
        public int numOfBombsTouching;
    }
}