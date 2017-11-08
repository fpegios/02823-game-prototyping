﻿using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.UI;using UnityEngine.SceneManagement;public class LoadingBarScript : MonoBehaviour {    AsyncOperation ao;    public GameObject loadingScreenBG;    public Slider progBar;    public Text loadingText;    public bool isFakeLoadingBar = false;    public float fakeIncrement = 0f;    public float fakeTiming = 0f;	// Use this for initialization	void Start () {			}		// Update is called once per frame	void Update () {			}    public void LoadLevel01()    {        loadingScreenBG.SetActive(true);        progBar.gameObject.SetActive(true);        loadingText.gameObject.SetActive(true);        loadingText.text = "Loading...";        if (!isFakeLoadingBar)        {            StartCoroutine(LoadLevelWithRealProgress());        }        else        {        }    }    IEnumerator LoadLevelWithRealProgress()    {        yield return new WaitForSeconds(1);        ao = SceneManager.LoadSceneAsync(1);        ao.allowSceneActivation = false;        while (!ao.isDone)        {            if (ao.progress == 0.9f)            {                progBar.value = 1f;                loadingText.text = "Press 'F' to continue";                if(Input.GetKeyDown(KeyCode.F))                {                    ao.allowSceneActivation = true;                }            }            else            {                progBar.value = ao.progress;            }            Debug.Log(ao.progress);            yield return null;        }    }}