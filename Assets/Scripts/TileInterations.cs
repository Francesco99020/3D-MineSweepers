using UnityEngine;

public class TileInterations : MonoBehaviour
{
    public GameManager gameManager;
    public FlagCounter flagCounter;
    public MapGenerator.tile tile;
    public bool isFlagged = false;
    public bool hasBeenRevealed = false;

    private void OnMouseOver()
    {
        if(gameManager.totalNumOfNumberTiles != 0 && !gameManager.isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))//Expose Tile
            {
                if (!isFlagged)
                {
                    RevealTile();
                    gameManager.PlayTileRevealAudio();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))//Place Flag
            {
                if(!isFlagged && !hasBeenRevealed && flagCounter.RemoveFlag())
                {
                    isFlagged = true;
                    transform.GetChild(6).gameObject.SetActive(true);
                    //Remove one from flag count
                    gameManager.PlayFlaggedAudio();
                }
                else if(isFlagged)
                {
                    isFlagged = false;
                    transform.GetChild(6).gameObject.SetActive(false);
                    flagCounter.ReturnFlag();
                    gameManager.PlayFlaggedAudio();
                }
                else
                {
                    isFlagged = false;
                    transform.GetChild(6).gameObject.SetActive(false);
                }
            }
        }
    }
    
    public void RevealTile()
    {
        if (!isFlagged && !hasBeenRevealed)
        {
            for (int i = 1; i < 7; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            hasBeenRevealed = true;
            if (tile.numOfBombsTouching == 0)
            {
                //Reveal all surrounding tiles
                gameManager.RevealSurroundingTiles(tile);
            }
            if(tile.type == MapGenerator.tile.tileType.Bomb)
            {
                gameManager.GameOver();
            }
            if (tile.numOfBombsTouching >= 0)
            {
                gameManager.FoundNumberTile();
            }
        }
    }

}
