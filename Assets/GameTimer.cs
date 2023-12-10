using Fusion;
using System;
using TMPro;
using UnityEngine;

public class GameTimer : NetworkBehaviour {

    public event Action OnTimerEnd;

    [SerializeField] private bool _startOnSpawned = false;
    [SerializeField] private bool _repeatOnTimerEnd = false;
    [SerializeField] private float _timerSeconds = 0f;

    [SerializeField] private TMP_Text _timerText;

    [Networked]
    private TickTimer Timer { get; set; }

    private bool _timesUp;

    public override void Spawned() {
        if (_startOnSpawned) {
            StartTimer();
        }
    }

    public override void FixedUpdateNetwork() {
        if (Timer.Expired(Runner)) {
            print("TIMES UP!");
            _timesUp = true;
            Timer = TickTimer.None;
            OnTimerEnd?.Invoke();

            if (_repeatOnTimerEnd) {
                StartTimer();
                _timesUp = false;
            }
        }

        if (Timer.ExpiredOrNotRunning(Runner)) {
            return;
        }

        int minutes = Mathf.FloorToInt((float)(Timer.RemainingTime(Runner) / 60F));
        int seconds = Mathf.FloorToInt((float)(Timer.RemainingTime(Runner) - minutes * 60));

        string timeLeftString = string.Format("{00:00}:{1:00}", minutes, seconds);

        _timerText.text = timeLeftString;
    }

    public void StartTimer() => StartTimer(_timerSeconds);

    public void StartTimer(float seconds) {
        Timer = TickTimer.CreateFromSeconds(Runner, seconds);
    }

    public void StopTimer() => Timer = TickTimer.None;
}
