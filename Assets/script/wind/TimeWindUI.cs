using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeWindUI : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text windText;


    public void UpdateDisplay(TimeWindController.TimePeriod time, TimeWindController.WindDirection wind)
    {
        timeText.text = "Time: " + time.ToString();
        windText.text = "Wind: " + wind.ToString();
    }

    string GetTimeName(TimeWindController.TimePeriod time)
    {
        switch (time)
        {
            case TimeWindController.TimePeriod.Morning: return "早上";
            case TimeWindController.TimePeriod.Noon: return "中午";
            case TimeWindController.TimePeriod.Evening: return "晚上";
            default: return "";
        }
    }

    string GetWindName(TimeWindController.WindDirection wind)
    {
        switch (wind)
        {
            case TimeWindController.WindDirection.East: return "東風";
            case TimeWindController.WindDirection.West: return "西風";
            case TimeWindController.WindDirection.None: return "無風";
            default: return "";
        }
    }
}
