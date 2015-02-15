using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance { get; private set; }

    public Player Player { get; private set;}
    public CameraController Camera { get; private set;}
    public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }

    public int CurrentTimebonus
    {
        get
        {
            var secondDifference = (int)(BonusCutoffSeconds - RunningTime.TotalSeconds);
            return Mathf.Max(0, secondDifference) * BonusSecondMultiplier; 
        }
    }

    private List<CheckPoint> _checkPoints;
    private int _currentCheckPointIndex;
    private DateTime _started;
    private int _savedPoints;


    public CheckPoint DebugSpawn;
    public int BonusCutoffSeconds;
    public int BonusSecondMultiplier;



    public void Awake ()
    {
        Instance = this;

    }

	public void Start () {

        _checkPoints = FindObjectsOfType<CheckPoint>().OrderBy(e => e.transform.position.x ).ToList();
        _currentCheckPointIndex = _checkPoints.Count > 0 ? 0 : -1;

        Player = FindObjectOfType<Player>();
        Camera = FindObjectOfType<CameraController>();

        _started = DateTime.UtcNow;

        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
        foreach (var listener in listeners)
        {
            for (var i = _checkPoints.Count - 1; i >= 0; i--)
            {
                var distance = ((MonoBehaviour)listener).transform.position.x - _checkPoints[i].transform.position.x;
                if (distance < 0)
                    continue;

                _checkPoints[i].AssingObjectToCheckPoint(listener);
                break;
            }
        }

#if UNITY_EDITOR
        if (DebugSpawn != null)
            DebugSpawn.SpawnPlayer(Player);
        else if (_currentCheckPointIndex != -1)
            _checkPoints[_currentCheckPointIndex].SpawnPlayer(Player);
#else 
        if (_currentCheckPointIndex != -1)
            _checkPoints[_currentCheckPointIndex].SpawnPlayer(Player);
#endif

	}
	

	public void Update () {

        var isAtLastCheckPoint = _currentCheckPointIndex + 1 >= +_checkPoints.Count;
        if (isAtLastCheckPoint)
            return;

        var distanceToNextCheckpoint = _checkPoints[_currentCheckPointIndex + 1].transform.position.x - Player.transform.position.x;
        if (distanceToNextCheckpoint >= 0)
            return;

        _checkPoints[_currentCheckPointIndex].PlayerLeftCheckPoint();
        _currentCheckPointIndex++;
        _checkPoints[_currentCheckPointIndex].PlayerHitCheckPoint();

        GameManager.Instance.AddPoints(CurrentTimebonus);
        _savedPoints = GameManager.Instance.Points;
        _started = DateTime.UtcNow;

        

	}

    public void KillPlayer ()
    {
        StartCoroutine(KillPlayerCo());
    }

    public IEnumerator KillPlayerCo()
    {
        Player.Kill();
        Camera.IsFollowing = false;
        yield return new WaitForSeconds(2f);

        Camera.IsFollowing = true;

        if (_currentCheckPointIndex != -1)
            _checkPoints[_currentCheckPointIndex].SpawnPlayer(Player);

        _started = DateTime.UtcNow;
        GameManager.Instance.ResetPoints(_savedPoints);

    }

}
