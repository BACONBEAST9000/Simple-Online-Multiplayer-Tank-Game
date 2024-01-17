using Fusion;
using System;
using UnityEngine;

public class NetworkTimer : NetworkBehaviour {

    public event Action OnTimerEnd;

    [SerializeField] private bool _startOnSpawned = false;
    [SerializeField] private bool _repeatOnTimerEnd = false;
    [SerializeField] private float _timerSeconds = 0f;

    [Networked]
    private TickTimer Timer { get; set; }

    public override void Spawned() {
        if (_startOnSpawned) {
            StartTimer();
        }
    }

    public override void FixedUpdateNetwork() {
        if (Timer.Expired(Runner)) {
            Timer = TickTimer.None;
            OnTimerEnd?.Invoke();

            if (_repeatOnTimerEnd) {
                StartTimer();
            }
        }
    }

    public void StartTimer() => StartTimer(_timerSeconds);

    public void StartTimer(float seconds) {
        Timer = TickTimer.CreateFromSeconds(Runner, seconds);
    }

    public void StopTimer() => Timer = TickTimer.None;

    public float GetTimeLeft() {
        if (Timer.ExpiredOrNotRunning(Runner)) {
            return 0;
        }
        return (float)Timer.RemainingTime(Runner);
    }
}
