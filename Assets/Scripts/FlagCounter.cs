using UnityEngine;
using UnityEngine.UI;

public class FlagCounter : MonoBehaviour
{
    int numOfRemainingFlags = 0;

    [SerializeField] Image OnesPlace;
    [SerializeField] Image TensPlace;
    [SerializeField] Image HundredsPlace;

    //Sprites
    [SerializeField] Sprite[] digitalNumbers = new Sprite[10];

    public void SetNumOfFlags(int flags) 
    { 
        numOfRemainingFlags = flags;
        SetFlagCounter();
    }

    public int GetNumOfFlags() { return numOfRemainingFlags; }

    public bool RemoveFlag()
    {
        if(numOfRemainingFlags == 0) return false;
        else
        {
            numOfRemainingFlags--;
            SetFlagCounter();
            return true;
        }
    }

    public void ReturnFlag() { 
        numOfRemainingFlags++;
        SetFlagCounter();
    }

    void SetFlagCounter()
    {
        string s_numOfRemainingFlags = numOfRemainingFlags.ToString();
        int ones;
        int tens;
        int hundreds;
        ones = int.Parse(s_numOfRemainingFlags[s_numOfRemainingFlags.Length - 1].ToString());
        try
        {
            tens = int.Parse(s_numOfRemainingFlags[s_numOfRemainingFlags.Length - 2].ToString());
        }
        catch
        {
            tens = 0;
        }
        try
        {
            hundreds = int.Parse(s_numOfRemainingFlags[s_numOfRemainingFlags.Length - 3].ToString());
        }
        catch
        {
            hundreds = 0;
        }
        OnesPlace.sprite = digitalNumbers[ones];
        TensPlace.sprite = digitalNumbers[tens];
        HundredsPlace.sprite = digitalNumbers[hundreds];
    }
}
