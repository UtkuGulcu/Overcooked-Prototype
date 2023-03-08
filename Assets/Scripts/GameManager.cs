using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 10f;

    private void Awake()
    {
        state = State.WaitingToStart;
        Instance = this;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                HandleWaitingToStartState();
                break;
            case State.CountdownToStart:
                HandleCountdownToStartState();
                break;
            case State.GamePlaying:
                HandleGamePlayingState();
                break;
            case State.GameOver:
                break;
        }

        Debug.Log(state);
    }

    private void HandleWaitingToStartState()
    {
        waitingToStartTimer -= Time.deltaTime;

        if (waitingToStartTimer < 0f)
        {
            state = State.CountdownToStart;
            OnStateChanged.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleCountdownToStartState()
    {
        countdownToStartTimer -= Time.deltaTime;

        if (countdownToStartTimer < 0f)
        {
            state = State.GamePlaying;
            gamePlayingTimer = gamePlayingTimerMax;
            OnStateChanged.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleGamePlayingState()
    {
        gamePlayingTimer -= Time.deltaTime;

        if (gamePlayingTimer < 0f)
        {
            state = State.GameOver;
            OnStateChanged.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
}
