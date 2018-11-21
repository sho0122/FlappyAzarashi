﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    //ゲームステート
    enum State
    {
        Ready,
        Play,
        GameOver
    }

    State state;
    int score;

    public AzarashiController azarashi;
    public GameObject blocks;
    public Text scoreLabel;
    public Text stateLabel;

	// Use this for initialization
	void Start () {

        //開始と同時にReadyステートに移行
        Ready();
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
		//ゲームのステートごとにイベントを監視
        switch(state)
        {
            case State.Ready:
                //タッチしたらゲームスタート
                if (Input.GetButtonDown("Fire1")) GameStart();
                break;
            case State.Play:
                //キャラクターが死亡したらゲームオーバー
                if (azarashi.IsDead()) GameOver();
                break;
            case State.GameOver:
                //タッチしたらシーンをロード
                if (Input.GetButtonDown("Fire1")) Reload();
                break;
        }
	}

    void Ready()
    {
        state = State.Ready;
        //各オブジェクトを無効状態にする
        azarashi.SetSteerActive(false);
        blocks.SetActive(false);

        //ラベルを更新
        scoreLabel.text = "Score : " + 0;

        stateLabel.gameObject.SetActive(true);
        stateLabel.text = "Ready";
    }

    void GameStart()
    {
        state = State.Play;

        //各オブジェクトを有効にする
        azarashi.SetSteerActive(true);
        blocks.SetActive(true);

        //最初の入力だけゲームコントローラーから渡す
        azarashi.Flap();

        //ラベルを更新
        stateLabel.gameObject.SetActive(false);
        stateLabel.text = "";
    }

    void GameOver()
    {
        state = State.GameOver;

        //シーン中の全てのScrollObjectコンポーネントを探し出す
        ScrollObject[] scrollObjects = GameObject.FindObjectsOfType<ScrollObject>();

        //全ScrollObjectのスクロール状態を無効にする
        foreach (ScrollObject so in scrollObjects) so.enabled = false;

        //ラベルを更新
        stateLabel.gameObject.SetActive(true);
        stateLabel.text = "GameOver";
    }

    void Reload()
    {
        //現在読み込んでいるシーンを再読み込み
        Application.LoadLevel(Application.loadedLevel);
    }

    public void IncreaseScore()
    {
        score++;
        scoreLabel.text = "Score : " + score;
    }
}
