using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    const string filename = "HighScores.json";

    List<HighScoreEntry> HighScoreList = new List<HighScoreEntry>();

    List<HighScoreEntry> EasyScoreList = new List<HighScoreEntry>();
    List<HighScoreEntry> IntermediateScoreList = new List<HighScoreEntry>();
    List<HighScoreEntry> ExpertScoreList = new List<HighScoreEntry>();

    int averageTimeToCompleteAnEasy = 0;
    int averageTimeToCompleteAnIntermediate = 0;
    int averageTimeToCompleteAnExpert = 0;

    [SerializeField] TextMeshProUGUI EasyAverage;
    [SerializeField] TextMeshProUGUI IntermediateAverage;
    [SerializeField] TextMeshProUGUI ExpertAverage;


    [SerializeField] TextMeshProUGUI[] EasyScores = new TextMeshProUGUI[3];

    [SerializeField] TextMeshProUGUI[] IntermediateScores = new TextMeshProUGUI[3];

    [SerializeField] TextMeshProUGUI[] ExpertScores = new TextMeshProUGUI[3];

    public void UpdateHighScoreStats()
    {
        HighScoreList.Clear();
        EasyScoreList.Clear();
        IntermediateScoreList.Clear();
        ExpertScoreList.Clear();


        //get data from JSON file
        HighScoreList = FileHandler.ReadListFromJSON<HighScoreEntry>(filename);

        //Seperate scores based on Easy, Intermediate or Expert difficulty
        foreach (HighScoreEntry entry in HighScoreList)
        {
            if (entry.difficulty.Equals("Easy")) { EasyScoreList.Add(entry); }
            else if (entry.difficulty.Equals("Intermediate")) { IntermediateScoreList.Add(entry); }
            else { ExpertScoreList.Add(entry); }
        }
        //If not empty order from lowest time to highest
        if (EasyScoreList != null) { EasyScoreList = EasyScoreList.OrderBy(o => o.playerTime).ToList(); }
        if (IntermediateScoreList != null) { IntermediateScoreList = IntermediateScoreList.OrderBy(o => o.playerTime).ToList(); }
        if (ExpertScoreList != null) { ExpertScoreList = ExpertScoreList.OrderBy(o => o.playerTime).ToList(); }

        for (int i = 0; i < EasyScoreList.Count; i++)
        {
            if (i == 3) { break; }
            else
            {
                ConvertAndSetTime(EasyScoreList[i].playerTime, EasyScores[i]);
            }
        }
        for (int i = 0; i < IntermediateScoreList.Count; i++)
        {
            if (i == 3) { break; }
            else
            {
                ConvertAndSetTime(IntermediateScoreList[i].playerTime, IntermediateScores[i]);
            }
        }
        for (int i = 0; i < ExpertScoreList.Count; i++)
        {
            if (i == 3) { break; }
            else
            {
                ConvertAndSetTime(ExpertScoreList[i].playerTime, ExpertScores[i]);
            }
        }

        //For getting average time to complete
        averageTimeToCompleteAnEasy = GetAverage(EasyScoreList);
        averageTimeToCompleteAnIntermediate = GetAverage(IntermediateScoreList);
        averageTimeToCompleteAnExpert = GetAverage(ExpertScoreList);

        if(averageTimeToCompleteAnEasy != -1) { ConvertAndSetTime(averageTimeToCompleteAnEasy, EasyAverage); }
        if(averageTimeToCompleteAnIntermediate != -1) { ConvertAndSetTime(averageTimeToCompleteAnIntermediate, IntermediateAverage); }
        if(averageTimeToCompleteAnExpert != -1) { ConvertAndSetTime(averageTimeToCompleteAnExpert, ExpertAverage); }
    }

    void ConvertAndSetTime(int timeToConvert, TextMeshProUGUI textToSet)
    {
        if (timeToConvert >= 60)
        {
            int mins = 0;
            int playerTime = timeToConvert;
            while (playerTime >= 60)
            {
                mins++;
                playerTime -= 60;
            }
            int secs = playerTime;
            string s_mins = mins.ToString();

            if (secs == 0) { textToSet.text = s_mins + ":00"; }
            else if (secs < 10)
            {
                string s_secs = "0" + secs.ToString();
                textToSet.text = s_mins + ":" + s_secs;
            }
            else
            {
                string s_secs = secs.ToString();
                textToSet.text = s_mins + ":" + s_secs;
            }
        }
        else
        {
            textToSet.text = timeToConvert.ToString() + "s";
        }
    }

    /// <summary>
    /// Gets average, return -1 if empty list.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    int GetAverage(List<HighScoreEntry> list)
    {
        if(list.Count == 0) { return -1; }
        if(list.Count == 1) { return list[0].playerTime; }
        int ans = 0;
        for (int i = 0; i < list.Count; i++)
        {
            ans += list[i].playerTime;
        }
        return ans/list.Count;
    }

    void Start()
    {
        UpdateHighScoreStats();
    }
}
