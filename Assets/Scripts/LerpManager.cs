using System;
using System.Collections;
using UnityEngine;

public static class LerpManager
{
    
    public static event Action OnComplete;
    
    public enum LerpType
    {
        Linear,
        EaseOut,
        EaseIn,
        Exponential,
        SmoothStep,
        SmootherStep
    }

    public static IEnumerator Lerp(RectTransform target, float start, float end, float lerpTime, LerpType lerpType)
    {
        float currentLerpTime = 0f;
        float t;

        while (currentLerpTime < lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            t = currentLerpTime / lerpTime;
            t = LerpStyleFinder(lerpType, t);
            target.sizeDelta = new Vector2(Mathf.Lerp(start, end, t), target.sizeDelta.y);

            yield return new WaitForEndOfFrame();
        }

        target.sizeDelta = new Vector2(end, target.sizeDelta.y);

        OnComplete?.Invoke();
    }

    private static float LerpStyleFinder(LerpType lerpType, float lerpTime)
    {
        return lerpType switch
        {
            LerpType.Linear => lerpTime,
            LerpType.EaseOut => Mathf.Sin(lerpTime * Mathf.PI * 0.5f),
            LerpType.EaseIn => 1f - Mathf.Cos(lerpTime * Mathf.PI * 0.5f),
            LerpType.Exponential => lerpTime * lerpTime,
            LerpType.SmoothStep => lerpTime * lerpTime * (3f - 2f * lerpTime),
            LerpType.SmootherStep => lerpTime * lerpTime * lerpTime * (lerpTime * (6f * lerpTime - 15f) + 10f),
            _ => throw new ArgumentOutOfRangeException(nameof(lerpType), lerpType, null)
        };
    }
}