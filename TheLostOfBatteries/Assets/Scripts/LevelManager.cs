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

    private IEnumerable<Player> Players;
    private int CurrentPlayer = 0;

    public CheckPoint DebugSpawn;
    public int BonusCutoffSeconds;
    public int BonusSecondMultiplier;

    public void Awake ()
    {
        Instance = this;
        Players = FindObjectsOfType<Player>().OrderBy(e => e.PosPlayerSelect);
    }

	public void Start () {

        _checkPoints = FindObjectsOfType<CheckPoint>().OrderBy(e => e.transform.position.x ).ToList();
        _currentCheckPointIndex = _checkPoints.Count > 0 ? 0 : -1;
        
        Player = Players.ElementAt(CurrentPlayer);
        Camera = FindObjectOfType<CameraController>();
        Camera.Player = Player.transform;

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

    public void NextPLayer()
    {
        StartCoroutine(NextPlayerCo());
    }
   
    public IEnumerator NextPlayerCo()
    {
        var totalPlayer = Players.Count();
        
        CurrentPlayer = (CurrentPlayer + 1) % totalPlayer;
        Player.Focused = false;
        Player.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Player 2";
        //FindObjectsOfType<HealthBar>().Where(e => e.name == "Health Bar " + Player.name)
        //    .FirstOrDefault().ChangeLayoutOrder("Player 2");
        Player = Players.ElementAt(CurrentPlayer);
        yield return new WaitForSeconds(0.1f);
        //FindObjectsOfType<HealthBar>().Where(e => e.name == "Health Bar " + Player.name)
        //    .FirstOrDefault().ChangeLayoutOrder("Player");
        Player.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Player";
        Camera.Player = Player.transform;
        Player.Focused = true;
    }

    public void KillPlayer(Player playerToKill)
    {
        StartCoroutine(KillPlayerCo(playerToKill));
    }

    public IEnumerator KillPlayerCo(Player playerToKill)
    {
        playerToKill.Kill();
        if (Players.ElementAt(CurrentPlayer) == playerToKill)
        {
            Camera.IsFollowing = false;
            yield return new WaitForSeconds(1.5f);

            Camera.IsFollowing = true;
        }
        else
            yield return new WaitForSeconds(1.5f);

        if (_currentCheckPointIndex != -1)
            _checkPoints[_currentCheckPointIndex].SpawnPlayer(playerToKill);

        _started = DateTime.UtcNow;
        GameManager.Instance.ResetPoints(_savedPoints);

    }


}
