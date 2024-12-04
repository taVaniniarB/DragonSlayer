using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FadeManager : MonoBehaviour
{
    public float curTime = 0f;
    public GameObject fadePanelPrefab;
    public GameObject panel;


    private static FadeManager _instance;
    public static FadeManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
        StopAllCoroutines();
    }

    public void FadeOut(float fadeTime)
    {
        UIManager.Instance.ShowFadePanel();
        StartCoroutine(CoFadeOut(fadeTime));
    }
    public void FadeIn(float fadeTime)
    {
        UIManager.Instance.ShowFadePanel();
        StartCoroutine(CoFadeIn(fadeTime));
    }

    IEnumerator CoFadeOut(float fadeTime)
    {
        while (curTime < fadeTime)
        {
            curTime += Time.deltaTime;
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Clamp01(curTime / fadeTime));

            yield return null;
        }

        curTime = 0f;
        yield break;
    }

    IEnumerator CoFadeIn(float fadeTime)
    {
        while (curTime < fadeTime)
        {
            curTime += Time.deltaTime;
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Clamp01(1 - (curTime / fadeTime)));

            yield return null;
        }

        curTime = 0f;
 
        UIManager.Instance.HideFadePanel();
        yield break;
    }

}
