using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeCounter
{
    float GetRoundCurrentCount();
    void ResetCurrentCount();
    void CountUp();
    bool CheckTime(float time);
}

[System.Serializable]
public class TimeCounter : ITimeCounter
{
    [SerializeField]
    private int roundPosition = 100;
    [SerializeField]
    private float currentCount = 0.0f;

    public float GetRoundCurrentCount()
    {
        return Mathf.Round(currentCount*roundPosition)/roundPosition;
    }

    public void ResetCurrentCount()
    {
        currentCount = 0f;
    }

    public void CountUp()
    {
        currentCount+=Time.deltaTime;
    }

    //基準値と計測値が一緒だった場合も勝ちにする
    public bool CheckTime(float time)
    {
        return currentCount - time >= 0 ? true : false;
    }
}
