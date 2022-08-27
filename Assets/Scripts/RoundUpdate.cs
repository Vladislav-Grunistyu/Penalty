using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class RoundUpdate : MonoBehaviour
{
    [SerializeField] private int _availableLive;
    [SerializeField] private int _scorePerGoal;
    [SerializeField] private float _timeUntilNextRound;
    [SerializeField] private float _speedAddToGoalkepper;
    [Space]
    [SerializeField] private GameObject _puckPrefab;
    [SerializeField] private PuckLaunch _puckLaunch;
    [SerializeField] private Goalkepper _goalkepper;
    [SerializeField] private UI _ui;
    [SerializeField] Transform _spawnPosition;
    [Space]
    [SerializeField] AudioClip _goalAudioClip;
    [SerializeField] AudioClip _missAudioClip;

    private int _score;
    private GameObject _puckOnField;

    private void OnEnable()
    {
        Gate.onGoal += Goal;
        Puck.onMissed += Miss;
        Goalkepper.onGoalkepperCaught += Miss;
    }
    private void OnDisable()
    {
        Gate.onGoal -= Goal;
        Puck.onMissed -= Miss;
        Goalkepper.onGoalkepperCaught -= Miss;
    }
    private void Start()
    {
        _ui.UpdateUI(_availableLive);
        StartNewRound();
    }
    private void Goal()
    {
        AddScore(_scorePerGoal);
        gameObject.GetComponent<AudioSource>().clip = _goalAudioClip;
        gameObject.GetComponent<AudioSource>().Play();
        StartNewRound();
    }
    private void Miss()
    {
        gameObject.GetComponent<AudioSource>().clip = _missAudioClip;
        gameObject.GetComponent<AudioSource>().Play();
        if (_availableLive - 1 == 0)
        {
            _ui.GameOverScreen();
            Time.timeScale = 0;
        }
        else
        {
            WastedLife(1);
            StartNewRound();
        }
    }

    private void StartNewRound()
    {
        StartCoroutine(DestroyPuck(_timeUntilNextRound));
        StartCoroutine(SpawnNewPuck(_timeUntilNextRound));
        _goalkepper.AddSpeed(_speedAddToGoalkepper);
        _ui.UpdateUI(_availableLive, _score);
    }

    private IEnumerator SpawnNewPuck(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _puckOnField = Instantiate(_puckPrefab, _spawnPosition.position, Quaternion.identity);
        _puckLaunch.SetNewPuck(_puckOnField);
    }

    private IEnumerator DestroyPuck(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(_puckOnField);
    }

    private void AddScore(int score)
    {
        if (score <= 0)
            throw new InvalidOperationException();
        _score += score;
    }

    private void WastedLife(int live)
    {
        if (live <= 0)
            throw new InvalidOperationException();
        _availableLive -= live;
    }
}
