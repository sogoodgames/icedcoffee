using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public Text TimeText;

    void Update () {
        TimeText.text = DialogueProcesser.FormatTime(DateTime.Now);
    }
}
