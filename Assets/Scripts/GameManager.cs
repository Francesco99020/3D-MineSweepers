using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    List<HighScoreEntry> entries = new List<HighScoreEntry>();
    const string filename = "HighScores.json";

    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] Timer timer;
    [SerializeField] FlagCounter flagCounter;
    [SerializeField] Image resetBtnImage;
    [SerializeField] PlayerController playerController;
    [SerializeField] CameraController cameraController;
    [SerializeField] Canvas HomeScreenCanvas;
    [SerializeField] Canvas PlayerUICanvas;
    [SerializeField] GameObject HowToPlayWindow;
    [SerializeField] GameObject HighScoreWindow;

    [SerializeField] Sprite[] resetBtnImages = new Sprite[2];//0 = happy, 1 = sad
    [SerializeField] Material bombMaterial;

    MapGenerator.tile[] tiles;
    GameObject[] GameTiles;

    string difficulty = "Easy";
    int mapWidth = 8;
    int mapHeight = 8;
    int numOfBombs = 10;

    public int totalNumOfNumberTiles = 0;
    public bool isGameOver = false;

    public void RevealSurroundingTiles(MapGenerator.tile tile)
    {
        List<GameObject> revealTiles = new List<GameObject>();
        int tileIndex = 0;

        for(int i = 0; i < tiles.Length;i++)
        {
            if (tiles[i] == tile) 
            { 
                tileIndex = i;
                break;
            }
        }

        if (tileIndex - mapWidth - 1 >= 0 && tileIndex % mapWidth != 0)//checks top left tile
        {
            revealTiles.Add(GameTiles[tileIndex - mapWidth - 1]);
        }
        if (tileIndex - mapWidth >= 0)//checks top middle tile
        {
            revealTiles.Add(GameTiles[tileIndex - mapWidth]);
        }
        if (tileIndex - mapWidth + 1 >= 0 && (tileIndex + 1) % mapWidth != 0)//checks top right tile
        {
            revealTiles.Add(GameTiles[tileIndex - mapWidth + 1]);
        }
        if (tileIndex - 1 >= 0 && tileIndex % mapWidth != 0)//checks middle left tile
        {
            revealTiles.Add(GameTiles[tileIndex - 1]);
        }
        if (tileIndex + 1 < tiles.Length && (tileIndex + 1) % mapWidth != 0)//checks middle right tile
        {
            revealTiles.Add(GameTiles[tileIndex + 1]);
        }
        if (tileIndex + mapWidth - 1 < tiles.Length && tileIndex % mapWidth != 0)//checks bottom left tile
        {
            revealTiles.Add(GameTiles[tileIndex + mapWidth - 1]);
        }
        if (tileIndex + mapWidth < tiles.Length)//checks bottom middle tile
        {
            revealTiles.Add(GameTiles[tileIndex + mapWidth]);
        }
        if (tileIndex + mapWidth + 1 < tiles.Length && (tileIndex + 1) % mapWidth != 0)//checks bottom right tile
        {
            revealTiles.Add(GameTiles[tileIndex + mapWidth + 1]);
        }

        foreach(GameObject item in revealTiles)
        {
            item.GetComponent<TileInterations>().RevealTile();
        }
    }

    private void Start()
    {
        entries = FileHandler.ReadListFromJSON<HighScoreEntry>(filename);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !HomeScreenCanvas.gameObject.activeSelf)
        {
            NewGame();
        }
        if(Input.GetKeyDown(KeyCode.Escape) && !HomeScreenCanvas.gameObject.activeSelf)
        {
            HomeScreenCanvas.gameObject.SetActive(true);
            PlayerUICanvas.gameObject.SetActive(false);
            playerController.enabled= false;
            cameraController.enabled= false;
            Cursor.visible=true;
            Cursor.lockState= CursorLockMode.None;
        }
    }

    public void NewGame()
    {
        isGameOver = false;
        if (HomeScreenCanvas.gameObject.activeSelf) 
        { 
            HomeScreenCanvas.gameObject.SetActive(false);
            PlayerUICanvas.gameObject.SetActive(true);
        }
        if(GameTiles != null)
        {
            for(int i = 0; i < GameTiles.Length; i++)
            {
                Destroy(GameTiles[i]);
            }
        }
        playerController.enabled = true;
        cameraController.enabled = true;
        playerController.gameObject.transform.position = new Vector3(0, 1, 0);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        resetBtnImage.sprite = resetBtnImages[0];
        tiles = mapGenerator.GenerateNewMap(mapWidth, mapHeight, numOfBombs);
        totalNumOfNumberTiles = 0;
        for(int i = 0;i < tiles.Length;i++)
        {
            if (tiles[i].numOfBombsTouching >= 0)
            {
                totalNumOfNumberTiles++;
            }
        }
        GameTiles = mapGenerator.GeneratePhysicalMap(mapWidth, tiles, gameObject.GetComponent<GameManager>(), flagCounter);
        flagCounter.SetNumOfFlags(numOfBombs);
        timer.ResetAndStartTimer();

        //Set borders so player can't leave tile map
        playerController.topBoarder = GameTiles[0].transform.position.x + 0.5f;
        playerController.rightBoarder = GameTiles[0].transform.position.z - 0.5f;
        playerController.bottomBoarder = GameTiles[GameTiles.Length - 1].transform.position.x - 0.5f;
        playerController.leftBoarder = GameTiles[GameTiles.Length - 1].transform.position.z + 0.5f;
    }

    public void SetDifficulty(int value)
    {
        if(value == 0)
        {
            difficulty = "Easy";
            mapWidth = 8;
            mapHeight = 8;
            numOfBombs = 10;
        }
        else if(value == 1)
        {
            difficulty = "Intermediate";
            mapWidth = 16;
            mapHeight = 16;
            numOfBombs = 40;
        }
        else if(value == 2)
        {
            difficulty = "Expert";
            mapWidth = 30;
            mapHeight = 16;
            numOfBombs = 99;
        }
    }

    public void OpenHowToPlayWindow()
    {
        HowToPlayWindow.SetActive(true);
    }

    public void CloseHowToPlayWindow()
    {
        HowToPlayWindow.SetActive(false);
    }

    public void OpenHighScoreWindow()
    {
        HighScoreWindow.SetActive(true);
        HighScoreWindow.GetComponent<HighScoreManager>().UpdateHighScoreStats();
    }

    public void CloseHighScoreWindow()
    {
        HighScoreWindow.SetActive(false);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void FoundNumberTile()
    {
        totalNumOfNumberTiles--;
        if(totalNumOfNumberTiles == 0)//all number tiles found, Game Won
        {
            GameWon();
        }
    }

    public void GameOver()
    {
        playerController.PlayExplosionAudio();
        isGameOver = true;
        resetBtnImage.sprite = resetBtnImages[1];
        timer.StopTimer();
        //Reveal all bombs
        for(int i = 0; i < GameTiles.Length; i++)
        {
            if (GameTiles[i].GetComponent<TileInterations>().tile.numOfBombsTouching == -1 && !GameTiles[i].GetComponent<TileInterations>().hasBeenRevealed)
            {
                GameTiles[i].transform.GetChild(5).gameObject.GetComponent<MeshRenderer>().material = bombMaterial;//TODO: Fix this (Material Not Found)
            }
        }
    }

    public void GameWon()
    {
        playerController.PlayWinAudio();
        //Save new time
        AddUserToList(difficulty, timer.StopTimer());
        FileHandler.SaveToJSON<HighScoreEntry>(entries, filename);
    }

    public void PlayTileRevealAudio()
    {
        playerController.PlayTileRevealAudio();
    }

    public void PlayFlaggedAudio()
    {
        playerController.PlayFlagPlacementAudio();
    }

    public void AddUserToList(string difficulty, int playerTime)
    {
        entries.Add(new HighScoreEntry(difficulty, playerTime));
    }
}
