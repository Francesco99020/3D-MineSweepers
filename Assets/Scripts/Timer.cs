using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    int elapsedTime = 0;

    //Timer Places
    [SerializeField] Image OnesPlace;
    [SerializeField] Image TensPlace;
    [SerializeField] Image HundredsPlace;

    //Sprites
    [SerializeField] Sprite[] digitalNumbers = new Sprite[10];

    Coroutine currentCounter = null;

    public void ResetAndStartTimer()
    {
        StopAllCoroutines();
        elapsedTime = 0;
        currentCounter = StartCoroutine(Counter());
    }

    public int StopTimer()
    {
        StopCoroutine(currentCounter);
        return elapsedTime;
    }

    IEnumerator Counter()
    {
        yield return new WaitForSeconds(1f);
        elapsedTime++;
        if(elapsedTime >= 1000)
        {
            yield break;
        }
        //Set timer
        string s_elapsedTime = elapsedTime.ToString();
        int ones;
        int tens;
        int hundreds;
        ones = int.Parse(s_elapsedTime[s_elapsedTime.Length - 1].ToString());
        try
        {
            tens = int.Parse(s_elapsedTime[s_elapsedTime.Length - 2].ToString());
        }
        catch
        {
            tens = 0;
        }
        try
        {
            hundreds = int.Parse(s_elapsedTime[s_elapsedTime.Length - 3].ToString());
        }catch
        {
            hundreds = 0;
        }
        OnesPlace.sprite = digitalNumbers[ones];
        TensPlace.sprite = digitalNumbers[tens];
        HundredsPlace.sprite = digitalNumbers[hundreds];
        currentCounter = StartCoroutine(Counter());
    }
}
