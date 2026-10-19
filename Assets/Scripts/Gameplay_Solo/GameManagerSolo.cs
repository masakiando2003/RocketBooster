using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerSolo : MonoBehaviour
{
    public static GameManagerSolo Instance
    {
        get; private set;
    }

    [SerializeField] Localization gameplaySolo_EN, gameplaySolo_JP;
    [SerializeField] Font gameplaySoloENFont, gameplaySoloJPFont;
    [SerializeField] GameObject playerRocket;
    [SerializeField] GameObject[] fuelObjects;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform[] checkPointPositions;
    [SerializeField] Waypoint[] wayPoints;
    [SerializeField] Transform goalPosition;
    [SerializeField] Transform lastCheckPointPosition;
    [SerializeField] Text playerNameLabelText, playerLifeLabelText, playerSpeedLabelText, playerBoostLabelText;
    [SerializeField] Text remainingTimeLabelText, timeElapsedLabelText, finalPositionLabelText;
    [SerializeField] Text rankingPlayerNameLabelText, rankingFinalPositionLabelText, rankingRemainingTimeLabelText, rankingTimeElapsedLabelText;
    [SerializeField] Text rankingLabelText, pauseLabelText, gameOverLabelText, clearLabelText, sliderStartText, sliderGoalText;
    [SerializeField] Text clearNotInRankingLabelText, pauseResumeLabelText, pauseRestartLabelText, pauseTitleLabelText, clearRankingReturnLabelText;
    [SerializeField] Text clearThankToyForPlayingLabelText, gameOverThankYouForPlayingLabelText;
    [SerializeField] Text gameOverNotInRankingLabelText, gameOverRankingButtonText, gameOverRankedLabelText;
    [SerializeField] Text gameOverRankingPositionText, gameOverRestartButtonText, gameOverTitleButtonText;
    [SerializeField] Text clearRankedLabelText, clearRankingButtonText, clearRestartButtonText, clearTitleButtonText;
    [SerializeField] Text playerNameText;
    [SerializeField] Text playerLifeText;
    [SerializeField] Text playerSpeedText;
    [SerializeField] Text playerBoostText; // For Debug
    [SerializeField] Text remainingTimeText;
    [SerializeField] Text timeElapsedText;
    [SerializeField] Text finalPositionText;
    [SerializeField] Text countDownTimeText;
    [SerializeField] Text clearRankedLabel, clearRankedText, clearOutOfRankText;
    [SerializeField] Text gameOverRankedLabel, gameOverOutOfRankText;
    [SerializeField] float startPointSpawnPositionOffsetY = 10f;
    [SerializeField] float checkPointSpawnPositionOffsetX = 5f;
    [SerializeField] float checkPointSpawnPositionOffsetY = 10f;
    [SerializeField] float maxRemainingTime = 300f;
    [SerializeField] float showGameOverPanelTime = 2f;
    [SerializeField] float startCountDownTime = 3.9f;
    [SerializeField] float hideCountDownTime = 1f;
    [SerializeField] TextAsset rankingDataFile;
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] Canvas rankingCanvas;
    [SerializeField] GameObject clearPanel, clearPanel2;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Canvas clearCanvas;
    [SerializeField] Slider stageMiniSlider;
    [SerializeField] Text[] rankingPlayerNameText;
    [SerializeField] Text[] rankingFinalPositionText;
    [SerializeField] Text[] rankingRemainingTimeText;
    [SerializeField] Text[] rankingTimeElaspedText;

    int currentCheckPointIndex;
    float remainingTime, countDownTimer, timeElapsed, startTime, totalDistance;
    string fromPanel, finalElapsedTime, finalPosition;

    public enum GameState
    {
        CountDown,
        GameStart,
        Pause,
        Win,
        GameOver
    }
    private static GameState currentGameState;

    [Serializable]
    public class RankingJson
    {
        public RankingData[] detail;
    }

    [Serializable]
    public class RankingData
    {
        public string playerName;
        public string finalPosition;
        public int checkPointIndex;
        public int remainingTime;
        public string timeElapsed;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        if (playerRocket != null)
        {
            playerRocket.transform.position = startPosition.transform.position + new Vector3(checkPointSpawnPositionOffsetX, startPointSpawnPositionOffsetY, 0f); ;
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.enabled = false;
        }
        if (clearCanvas != null)
        {
            clearCanvas.enabled = false;
        }
        gameOverPanel.SetActive(false);
        pauseCanvas.enabled = false;
        rankingCanvas.enabled = false;
        currentGameState = GameState.CountDown;
        fromPanel = "";
        finalElapsedTime = "";
        Time.timeScale = 1f;
        remainingTime = maxRemainingTime;
        countDownTimer = startCountDownTime;
        playerNameText.text = PlayerNameTempSaveSolo.playerName;
        playerRocket.GetComponent<MovementSolo>().DisablePlayerControl();
        UpdatePlayerBoostText(); // For Debug
        if (Language.gameDisplayLanguage == Language.DisplayLanauge.None)
        {
            Language.gameDisplayLanguage = Language.DisplayLanauge.English;
        }
        switch (Language.gameDisplayLanguage)
        {
            case Language.DisplayLanauge.English:
                finalPosition = "Start";
                finalPositionText.text = finalPosition;
                break;
            case Language.DisplayLanauge.Japanese:
                finalPosition = "�X�^�[�g";
                finalPositionText.text = finalPosition;
                finalPositionText.fontStyle = FontStyle.Bold;
                break;
        }
        totalDistance = 0;
        // checkPointIndex: 0 = Start, 99 = Goal, 1 = Check1Point 1, 2 Check Point 2, etc.
        currentCheckPointIndex = 0;
        CalculatTotalDistance();
        ShowFinalPosition();
        CheckRemainingTime();
        InitializeRemainingTime();
        ReadRankingData();
        switch (Language.gameDisplayLanguage)
        {
            case Language.DisplayLanauge.English:
                playerNameLabelText.text = gameplaySolo_EN.GetLabelContent("PlayerNameLabelText");
                playerNameLabelText.font = gameplaySoloENFont;
                playerLifeLabelText.text = gameplaySolo_EN.GetLabelContent("PlayerLifeLabelText");
                playerLifeLabelText.font = gameplaySoloENFont;
                playerSpeedLabelText.text = gameplaySolo_EN.GetLabelContent("SpeedLabelText");
                playerSpeedLabelText.font = gameplaySoloENFont;
                remainingTimeLabelText.text = gameplaySolo_EN.GetLabelContent("RemainingTimeLabelText");
                remainingTimeLabelText.font = gameplaySoloENFont;
                timeElapsedLabelText.text = gameplaySolo_EN.GetLabelContent("TimeElapsedLabelText");
                timeElapsedLabelText.font = gameplaySoloENFont;
                finalPositionLabelText.text = gameplaySolo_EN.GetLabelContent("FinalPositionLabelText");
                finalPositionLabelText.font = gameplaySoloENFont;
                rankingLabelText.text = gameplaySolo_EN.GetLabelContent("RankingLabelText");
                rankingLabelText.font = gameplaySoloENFont;
                rankingPlayerNameLabelText.text = gameplaySolo_EN.GetLabelContent("RankingPlayerNameLabelText");
                rankingPlayerNameLabelText.font = gameplaySoloENFont;
                rankingFinalPositionLabelText.text = gameplaySolo_EN.GetLabelContent("RankingFinalPositionLabelText");
                rankingFinalPositionLabelText.font = gameplaySoloENFont;
                rankingRemainingTimeLabelText.text = gameplaySolo_EN.GetLabelContent("RankingRemainingTimeLabelText");
                rankingRemainingTimeLabelText.font = gameplaySoloENFont;
                rankingTimeElapsedLabelText.text = gameplaySolo_EN.GetLabelContent("RankingTimeElapsedLabelText");
                rankingTimeElapsedLabelText.font = gameplaySoloENFont;
                pauseLabelText.text = gameplaySolo_EN.GetLabelContent("PausedLabelText");
                pauseLabelText.font = gameplaySoloENFont;
                clearNotInRankingLabelText.text = gameplaySolo_EN.GetLabelContent("ClearNotInRankingText").Replace("|", Environment.NewLine);
                clearNotInRankingLabelText.font = gameplaySoloENFont;
                gameOverLabelText.text = gameplaySolo_EN.GetLabelContent("GameOverLabelText");
                gameOverLabelText.font = gameplaySoloENFont;
                sliderStartText.text = gameplaySolo_EN.GetLabelContent("SliderStartText");
                sliderStartText.font = gameplaySoloENFont;
                sliderGoalText.text = gameplaySolo_EN.GetLabelContent("SliderGoalText");
                sliderGoalText.font = gameplaySoloENFont;
                clearLabelText.text = gameplaySolo_EN.GetLabelContent("ClearLabelText");
                clearLabelText.font = gameplaySoloENFont;
                clearThankToyForPlayingLabelText.text = gameplaySolo_EN.GetLabelContent("ThankYouForPlayingText");
                clearThankToyForPlayingLabelText.font = gameplaySoloENFont;
                gameOverThankYouForPlayingLabelText.text = gameplaySolo_EN.GetLabelContent("ThankYouForPlayingText");
                gameOverThankYouForPlayingLabelText.font = gameplaySoloENFont;
                pauseResumeLabelText.text = gameplaySolo_EN.GetLabelContent("PauseResumeLabelText");
                pauseResumeLabelText.font = gameplaySoloENFont;
                pauseRestartLabelText.text = gameplaySolo_EN.GetLabelContent("PauseRestartLabelText");
                pauseRestartLabelText.font = gameplaySoloENFont;
                pauseTitleLabelText.text = gameplaySolo_EN.GetLabelContent("PauseTitleLabelText");
                pauseTitleLabelText.font = gameplaySoloENFont;
                gameOverNotInRankingLabelText.text = gameplaySolo_EN.GetLabelContent("GameOverNotInRankingLabelText").Replace("|", Environment.NewLine);
                gameOverNotInRankingLabelText.font = gameplaySoloENFont;
                gameOverRankedLabelText.text = gameplaySolo_EN.GetLabelContent("GameOverRankedLabelText");
                gameOverRankedLabelText.font = gameplaySoloENFont;
                gameOverRankingButtonText.text = gameplaySolo_EN.GetLabelContent("GameOverRankingButtonText");
                gameOverRankingButtonText.font = gameplaySoloENFont;
                gameOverRestartButtonText.text = gameplaySolo_EN.GetLabelContent("GameOverRestartButtonText");
                gameOverRestartButtonText.font = gameplaySoloENFont;
                gameOverTitleButtonText.text = gameplaySolo_EN.GetLabelContent("GameOverTitleButtonText");
                gameOverTitleButtonText.font = gameplaySoloENFont;
                clearRankedLabelText.text = gameplaySolo_EN.GetLabelContent("ClearRankedLabelText");
                clearRankedLabelText.font = gameplaySoloENFont;
                clearRankingButtonText.text = gameplaySolo_EN.GetLabelContent("ClearRankingButtonText");
                clearRankingButtonText.font = gameplaySoloENFont;
                clearRestartButtonText.text = gameplaySolo_EN.GetLabelContent("ClearRestartButtonText");
                clearRestartButtonText.font = gameplaySoloENFont;
                clearTitleButtonText.text = gameplaySolo_EN.GetLabelContent("ClearTitleButtonText");
                clearTitleButtonText.font = gameplaySoloENFont;
                clearRankingReturnLabelText.text = gameplaySolo_EN.GetLabelContent("ClearRankingReturnLabelText");
                clearRankingReturnLabelText.font = gameplaySoloENFont;
                break;
            case Language.DisplayLanauge.Japanese:
                playerNameLabelText.text = gameplaySolo_JP.GetLabelContent("PlayerNameLabelText");
                playerNameLabelText.font = gameplaySoloJPFont;
                playerNameLabelText.fontStyle = FontStyle.Bold;
                playerLifeLabelText.text = gameplaySolo_JP.GetLabelContent("PlayerLifeLabelText");
                playerLifeLabelText.font = gameplaySoloJPFont;
                playerLifeLabelText.fontStyle = FontStyle.Bold;
                playerSpeedLabelText.text = gameplaySolo_JP.GetLabelContent("SpeedLabelText");
                playerSpeedLabelText.font = gameplaySoloJPFont;
                playerSpeedLabelText.fontStyle = FontStyle.Bold;
                remainingTimeLabelText.text = gameplaySolo_JP.GetLabelContent("RemainingTimeLabelText");
                remainingTimeLabelText.font = gameplaySoloJPFont;
                remainingTimeLabelText.fontStyle = FontStyle.Bold;
                timeElapsedLabelText.text = gameplaySolo_JP.GetLabelContent("TimeElapsedLabelText");
                timeElapsedLabelText.font = gameplaySoloJPFont;
                timeElapsedLabelText.fontStyle = FontStyle.Bold;
                finalPositionLabelText.text = gameplaySolo_JP.GetLabelContent("FinalPositionLabelText");
                finalPositionLabelText.font = gameplaySoloJPFont;
                finalPositionLabelText.fontStyle = FontStyle.Bold;
                rankingLabelText.text = gameplaySolo_JP.GetLabelContent("RankingLabelText");
                rankingLabelText.font = gameplaySoloJPFont;
                rankingLabelText.fontStyle = FontStyle.Bold;
                rankingPlayerNameLabelText.text = gameplaySolo_JP.GetLabelContent("RankingPlayerNameLabelText");
                rankingPlayerNameLabelText.font = gameplaySoloJPFont;
                rankingPlayerNameLabelText.fontStyle = FontStyle.Bold;
                rankingFinalPositionLabelText.text = gameplaySolo_JP.GetLabelContent("RankingFinalPositionLabelText");
                rankingFinalPositionLabelText.font = gameplaySoloJPFont;
                rankingFinalPositionLabelText.fontStyle = FontStyle.Bold;
                rankingRemainingTimeLabelText.text = gameplaySolo_JP.GetLabelContent("RankingRemainingTimeLabelText");
                rankingRemainingTimeLabelText.font = gameplaySoloJPFont;
                rankingRemainingTimeLabelText.fontStyle = FontStyle.Bold;
                rankingTimeElapsedLabelText.text = gameplaySolo_JP.GetLabelContent("RankingTimeElapsedLabelText");
                rankingTimeElapsedLabelText.font = gameplaySoloJPFont;
                rankingTimeElapsedLabelText.fontStyle = FontStyle.Bold;
                pauseLabelText.text = gameplaySolo_JP.GetLabelContent("PausedLabelText");
                pauseLabelText.font = gameplaySoloJPFont;
                pauseLabelText.fontStyle = FontStyle.Bold;
                clearNotInRankingLabelText.text = gameplaySolo_JP.GetLabelContent("ClearNotInRankingText").Replace("|", Environment.NewLine);
                clearNotInRankingLabelText.font = gameplaySoloJPFont;
                clearNotInRankingLabelText.fontStyle = FontStyle.Bold;
                gameOverLabelText.text = gameplaySolo_JP.GetLabelContent("GameOverLabelText");
                gameOverLabelText.font = gameplaySoloJPFont;
                gameOverLabelText.fontStyle = FontStyle.Bold;
                sliderStartText.text = gameplaySolo_JP.GetLabelContent("SliderStartText");
                sliderStartText.font = gameplaySoloJPFont;
                sliderStartText.fontStyle = FontStyle.Bold;
                sliderGoalText.text = gameplaySolo_JP.GetLabelContent("SliderGoalText");
                sliderGoalText.font = gameplaySoloJPFont;
                sliderGoalText.fontStyle = FontStyle.Bold;
                clearLabelText.text = gameplaySolo_JP.GetLabelContent("ClearLabelText");
                clearLabelText.font = gameplaySoloJPFont;
                clearLabelText.fontStyle = FontStyle.Bold;
                clearThankToyForPlayingLabelText.text = gameplaySolo_JP.GetLabelContent("ThankYouForPlayingText");
                clearThankToyForPlayingLabelText.font = gameplaySoloJPFont;
                clearThankToyForPlayingLabelText.fontStyle = FontStyle.Bold;
                gameOverThankYouForPlayingLabelText.text = gameplaySolo_JP.GetLabelContent("ThankYouForPlayingText");
                gameOverThankYouForPlayingLabelText.font = gameplaySoloJPFont;
                gameOverThankYouForPlayingLabelText.fontStyle = FontStyle.Bold;
                pauseResumeLabelText.text = gameplaySolo_JP.GetLabelContent("PauseResumeLabelText");
                pauseResumeLabelText.font = gameplaySoloJPFont;
                pauseResumeLabelText.fontStyle = FontStyle.Bold;
                pauseRestartLabelText.text = gameplaySolo_JP.GetLabelContent("PauseRestartLabelText");
                pauseRestartLabelText.font = gameplaySoloJPFont;
                pauseRestartLabelText.fontStyle = FontStyle.Bold;
                pauseTitleLabelText.text = gameplaySolo_JP.GetLabelContent("PauseTitleLabelText");
                pauseTitleLabelText.font = gameplaySoloJPFont;
                pauseTitleLabelText.fontStyle = FontStyle.Bold;
                gameOverNotInRankingLabelText.text = gameplaySolo_JP.GetLabelContent("GameOverNotInRankingLabelText").Replace("|", Environment.NewLine);
                gameOverNotInRankingLabelText.font = gameplaySoloJPFont;
                gameOverNotInRankingLabelText.fontStyle = FontStyle.Bold;
                gameOverRankedLabelText.text = gameplaySolo_JP.GetLabelContent("GameOverRankedLabelText");
                gameOverRankedLabelText.font = gameplaySoloJPFont;
                gameOverRankedLabelText.fontStyle = FontStyle.Bold;
                gameOverRankingButtonText.text = gameplaySolo_JP.GetLabelContent("GameOverRankingButtonText");
                gameOverRankingButtonText.font = gameplaySoloJPFont;
                gameOverRankingButtonText.fontStyle = FontStyle.Bold;
                gameOverRestartButtonText.text = gameplaySolo_JP.GetLabelContent("GameOverRestartButtonText");
                gameOverRestartButtonText.font = gameplaySoloJPFont;
                gameOverRestartButtonText.fontStyle = FontStyle.Bold;
                gameOverTitleButtonText.text = gameplaySolo_JP.GetLabelContent("GameOverTitleButtonText");
                gameOverTitleButtonText.font = gameplaySoloJPFont;
                gameOverTitleButtonText.fontStyle = FontStyle.Bold;
                clearRankedLabelText.text = gameplaySolo_JP.GetLabelContent("ClearRankedLabelText");
                clearRankedLabelText.font = gameplaySoloJPFont;
                clearRankedLabelText.fontStyle = FontStyle.Bold;
                clearRankingButtonText.text = gameplaySolo_JP.GetLabelContent("ClearRankingButtonText");
                clearRankingButtonText.font = gameplaySoloJPFont;
                clearRankingButtonText.fontStyle = FontStyle.Bold;
                clearRestartButtonText.text = gameplaySolo_JP.GetLabelContent("ClearRestartButtonText");
                clearRestartButtonText.font = gameplaySoloJPFont;
                clearRestartButtonText.fontStyle = FontStyle.Bold;
                clearTitleButtonText.text = gameplaySolo_JP.GetLabelContent("ClearTitleButtonText");
                clearTitleButtonText.font = gameplaySoloJPFont;
                clearTitleButtonText.fontStyle = FontStyle.Bold;
                clearRankingReturnLabelText.text = gameplaySolo_JP.GetLabelContent("ClearRankingReturnLabelText");
                clearRankingReturnLabelText.font = gameplaySoloJPFont;
                clearRankingReturnLabelText.fontStyle = FontStyle.Bold;
                break;
        }
    }

    private void Update()
    {
        UpdatePlayerLifeText();
        switch (currentGameState)
        {
            case GameState.CountDown:
                countDownTimeText.enabled = true;
                countDownTimer -= Time.deltaTime;
                CountDown(countDownTimer);
                break;
            case GameState.GameStart:
                UpdatePlayerBoostText(); // For Debug
                UpdatePlayerSpeedText();
                CalculateRemainingDistance();
                ShowFinalPosition();
                CheckRemainingTime();
                UpdateRemainingTimeAndTimeElapsed();
                RespondToPauseGame();
                break;
            case GameState.Win:
            case GameState.GameOver:
                ShowFinalPosition();
                UpdatePlayerSpeedText();
                break;
            case GameState.Pause:
                RespondToPauseGame();
                break;
            default:
                break;
        }
    }

    private void ChangeGameState(GameState targetGameState)
    {
        currentGameState = targetGameState;
    }

    private void CountDown(float countDownTimer)
    {
        if (countDownTimeText == null)
        {
            Instance.ChangeGameState(GameState.GameStart);
            return;
        }

        ShowCountTimeText(countDownTimer);
        if (countDownTimer <= 0.0f)
        {
            PlayTimeTempSaveSolo.soloStageStartTime = Time.time;
            Invoke("HideCountDownText", hideCountDownTime);
            Instance.ChangeGameState(GameState.GameStart);
            playerRocket.GetComponent<MovementSolo>().EnablePlayerControl();
        }
    }

    private void ShowCountTimeText(float countDownTimer)
    {
        int remainingSeconds = Mathf.FloorToInt(countDownTimer);
        string startGameLabelText;
        switch (Language.gameDisplayLanguage)
        {
            case Language.DisplayLanauge.English:
                startGameLabelText = gameplaySolo_EN.GetLabelContent("StartGameLabelText");
                countDownTimeText.text = (remainingSeconds > 0) ? remainingSeconds.ToString() : startGameLabelText;
                break;
            case Language.DisplayLanauge.Japanese:
                startGameLabelText = gameplaySolo_JP.GetLabelContent("StartGameLabelText");
                countDownTimeText.text = (remainingSeconds > 0) ? remainingSeconds.ToString() : startGameLabelText;
                break;
        }
    }

    private void HideCountDownText()
    {
        countDownTimeText.enabled = false;
        startTime = Time.time - countDownTimer;
    }

    private void InitializeRemainingTime()
    {
        remainingTimeText.text = Mathf.FloorToInt(remainingTime).ToString();
    }

    private void ReadRankingData()
    {
        if (rankingDataFile == null) { return; }
        int index = 0;
        RankingJson rankingJson = JsonUtility.FromJson<RankingJson>(rankingDataFile.text);
        foreach(RankingData data in rankingJson.detail)
        {
            rankingPlayerNameText[index].text = data.playerName;
            switch (Language.gameDisplayLanguage)
            {
                case Language.DisplayLanauge.English:
                    rankingFinalPositionText[index].text = data.finalPosition.ToString();
                    rankingFinalPositionText[index].font = gameplaySoloENFont;
                    break;
                case Language.DisplayLanauge.Japanese:
                    rankingFinalPositionText[index].text = data.finalPosition.ToString()
                                                                .Replace("Start", gameplaySolo_JP.GetLabelContent("StartPositionText"))
                                                                .Replace("Check Point", gameplaySolo_JP.GetLabelContent("CheckPointPositionText"))
                                                                .Replace("Goal", gameplaySolo_JP.GetLabelContent("GoalPositionText"));
                    rankingFinalPositionText[index].font = gameplaySoloJPFont;
                    rankingFinalPositionText[index].fontStyle = FontStyle.Bold;
                    break;
            }
            rankingRemainingTimeText[index].text = data.remainingTime.ToString();
            rankingTimeElaspedText[index].text = data.timeElapsed;
            index++;
        }
    }

    private void UpdateRemainingTimeAndTimeElapsed()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTimeText == null) { return; }
        if (remainingTime >= 0.0f)
        {
            remainingTimeText.text = Mathf.FloorToInt(remainingTime).ToString();
        }

        float t = Time.time - PlayTimeTempSaveSolo.soloStageStartTime;
        string minutes = ((int)t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        string miliseconds = (t * 60 % 60).ToString("00");
        finalElapsedTime = minutes + ":" + seconds + ":" + miliseconds;

        if (timeElapsedText == null) { return; }
        timeElapsedText.text = finalElapsedTime;
    }

    private void CheckRemainingTime()
    {
        if (remainingTime <= 0.0f)
        {
            DisablePlayerMovement();
            DisablePlayerCollision();
            GameOver();
        }
    }

    public void RecoverRemainingTime(float recoverRemainingTime)
    {
        remainingTime += recoverRemainingTime;
        if(recoverRemainingTime >= maxRemainingTime)
        {
            recoverRemainingTime = maxRemainingTime;
        }
    }

    public void UpdateFinalPosition(string positionName, int checkPointIndex)
    {
       finalPosition = positionName;
       currentCheckPointIndex = checkPointIndex;
    }

    private void CalculatTotalDistance()
    {
        if (wayPoints.Length <= 0)
        {
            totalDistance = Vector3.Distance(startPosition.position, goalPosition.position);
        }
        else if (wayPoints.Length == 1)
        {
            totalDistance += Vector3.Distance(startPosition.position, wayPoints[0].transform.position);
            totalDistance += Vector3.Distance(goalPosition.position, wayPoints[0].transform.position);
        }
        else
        {
            for(var i = 0; i < wayPoints.Length - 1; i++)
            {
                totalDistance += Vector3.Distance(wayPoints[i].transform.position, wayPoints[i+1].transform.position);
            }
        }
        //Debug.Log("Calculated Total Distance: "+totalDistance);
        stageMiniSlider.minValue = 0;
        stageMiniSlider.maxValue = totalDistance;
    }

    private void CalculateRemainingDistance()
    {
        Dictionary<int, float> ToPlayerDistances = new Dictionary<int, float>();
        for (var i = 0; i < wayPoints.Length; i++)
        {
            //Debug.Log("i: "+i+", Distance: "+ Vector3.Distance(wayPoints[i].transform.position, playerRocket.transform.position)+", Direction: "+ (wayPoints[i].transform.position - playerRocket.transform.position).normalized);
            ToPlayerDistances.Add(i, Vector3.Distance(wayPoints[i].transform.position, playerRocket.transform.position));
        }
        int positionIndex = 0;
        float minDistance = Mathf.Infinity;
        foreach(KeyValuePair<int, float> distanceInfo in ToPlayerDistances)
        {
            if(distanceInfo.Value < minDistance)
            {
                positionIndex = distanceInfo.Key;
                minDistance = distanceInfo.Value;
            }
        }
        //Debug.Log("positionIndex: " + positionIndex);

        float totalMovedDistance = 0f;
        if (positionIndex == 0)
        {
            totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
        }
        else if (positionIndex == wayPoints.Length - 1)
        {
            for (var i = 0; i < positionIndex; i++)
            {
                totalMovedDistance += Vector3.Distance(wayPoints[i].transform.position, wayPoints[i+1].transform.position);
            }
            totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
        }
        else
        {
            for (var i = 0; i <= positionIndex - 1; i++)
            {
                totalMovedDistance += Vector3.Distance(wayPoints[i].transform.position, wayPoints[i + 1].transform.position);
            }

            string previousDirection = wayPoints[positionIndex].GetFromPreviousWaypointDirection();
            if (previousDirection == "Left")
            {
                if (playerRocket.transform.position.x <= wayPoints[positionIndex].transform.position.x)
                {
                    totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
                }
                else
                {
                    totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
                }
            }
            else if (previousDirection == "Right")
            {
                if (playerRocket.transform.position.x >= wayPoints[positionIndex].transform.position.x)
                {
                    totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
                }
                else
                {
                    totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
                }
            }
            else if (previousDirection == "Up")
            {
                if (playerRocket.transform.position.y <= wayPoints[positionIndex].transform.position.y)
                {
                    totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
                }
                else
                {
                    totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
                }
            }
            else if (previousDirection == "Down")
            {
                if (playerRocket.transform.position.y >= wayPoints[positionIndex].transform.position.y)
                {
                    totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
                }
                else
                {
                    totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRocket.transform.position);
                }
            }
        }
        //Debug.Log("totalMovedDistance: "+ totalMovedDistance);
        stageMiniSlider.value = totalMovedDistance;
    }

    private void ShowFinalPosition()
    {
        switch (Language.gameDisplayLanguage)
        {
            case Language.DisplayLanauge.English:
                finalPositionText.text = finalPosition;
                finalPositionText.font = gameplaySoloENFont;
                break;
            case Language.DisplayLanauge.Japanese:
                finalPositionText.text = finalPosition.Replace("Start", gameplaySolo_JP.GetLabelContent("StartPositionText"))
                                                      .Replace("Check Point", gameplaySolo_JP.GetLabelContent("CheckPointPositionText"))
                                                      .Replace("Goal", gameplaySolo_JP.GetLabelContent("GoalPositionText"));
                finalPositionText.font = gameplaySoloJPFont;
                finalPositionText.fontStyle = FontStyle.Bold;
                break;
        }
    }

    internal void SaveLatestCheckPoint(Transform checkPointPos)
    {
        lastCheckPointPosition = checkPointPos;
    }

    private void DisablePlayerMovement()
    {
        playerRocket.GetComponent<MovementSolo>().enabled = false;
    }
    private void DisablePlayerCollision()
    {
        playerRocket.GetComponent<CollisionHandlerSolo>().enabled = false;
    }

    private void UpdatePlayerLifeText()
    {
        if(playerLifeText == null) { return; }
        playerLifeText.text = playerRocket.GetComponent<PlayerStatusSolo>().GetCurrentLife().ToString();
    }

    private void UpdatePlayerSpeedText()
    {
        if(playerSpeedText == null) { return; }
        float currentSpeed;
        currentSpeed = Mathf.FloorToInt(playerRocket.GetComponent<Rigidbody>().linearVelocity.magnitude);
        /*
        if (playerRocket.GetComponent<Rigidbody>().velocity.magnitude > playerRocket.GetComponent<Movement>().GetLimitedMaxVelocity())
        {
            currentSpeed = playerRocket.GetComponent<Movement>().GetLimitedMaxVelocity();
        }
        else
        {
            currentSpeed = Mathf.FloorToInt(playerRocket.GetComponent<Rigidbody>().velocity.magnitude);
        }
        */
        playerSpeedText.text = currentSpeed.ToString() + "Km / s";
    }

    private void UpdatePlayerBoostText()
    {
        if (playerBoostText == null) { return; }
        int playerBoost = Mathf.FloorToInt(playerRocket.GetComponent<PlayerStatusSolo>().GetCurrentBoost());
        if(playerBoost < 0)
        {
            playerBoost = 0;
        }
        playerBoostText.text = playerBoost.ToString();
    }
    private void RespondToPauseGame()
    {

        if(currentGameState == GameState.GameStart)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("1PPause"))
            {
                //Debug.Log("Pause Game Test");
                PauseGame();
            }
        }
        else if (currentGameState == GameState.Pause)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("1PPause"))
            {
                //Debug.Log("Resume Game Test");
                ResumeGame();
            }
        }
    }


    public void ResetPlayerToStartPosition(GameObject player)
    {
        if(lastCheckPointPosition != null)
        {
            player.transform.position = lastCheckPointPosition.transform.position + new Vector3(checkPointSpawnPositionOffsetX, checkPointSpawnPositionOffsetY, 0f);
        }
        else if (startPosition != null)
        {
            player.transform.position = startPosition.transform.position + new Vector3(0f, startPointSpawnPositionOffsetY, 0f);
        }
        ResetPlayerRotation(player);
    }

    public void ResetPlayerRotation(GameObject player)
    {
        player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        player.GetComponent<MovementSolo>().StopMovement();
    }
    public void ActiviateAllFuelObjects()
    {
        foreach(GameObject fuel in fuelObjects)
        {
            if(fuel.GetComponent<Fuel>() != null)
            {
                fuel.GetComponent<Fuel>().ActivateFuelObject();
            }
        }
    }

    public void Clear()
    {
        UpdateFinalPosition("Goal", 99);
        UpdateRanking();
        if (clearCanvas == null) { return; }
        clearCanvas.enabled = true;
        StartCoroutine(ShowClearPanel());
        Instance.ChangeGameState(GameState.Win);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Instance.ChangeGameState(GameState.Pause);
        pauseCanvas.enabled = true;
        playerRocket.GetComponent<MovementSolo>().DisablePlayerControl();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Instance.ChangeGameState(GameState.GameStart);
        pauseCanvas.enabled = false;
        playerRocket.GetComponent<MovementSolo>().EnablePlayerControl();
    }
    public void RestartGame()
    {
        PlayTimeTempSaveSolo.totalTimeElapsed = Time.time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        UpdateRanking();
        if (gameOverCanvas == null) { return; }
        gameOverCanvas.enabled = true;
        StartCoroutine(ShowGameOverPanel());
        Instance.ChangeGameState(GameState.GameOver);
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    private IEnumerator ShowClearPanel()
    {
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
        }
        yield return new WaitForSeconds(showGameOverPanelTime);

        if (clearPanel2 != null)
        {
            clearPanel2.SetActive(true);
        }
    }
    private IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(showGameOverPanelTime);
        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void ShowRankingPanelFromClear()
    {
        fromPanel = "Clear";
        if(clearPanel == null || rankingCanvas == null) { return; }
        clearPanel.SetActive(false);
        clearLabelText.enabled = false;
        clearThankToyForPlayingLabelText.enabled = false;
        rankingCanvas.enabled = true;
    }

    public void ShowRankingPanelFromGameOver()
    {
        fromPanel = "GameOver";
        if (gameOverPanel == null || rankingCanvas == null) { return; }
        gameOverLabelText.enabled = false;
        gameOverThankYouForPlayingLabelText.enabled = false;
        gameOverPanel.SetActive(false);
        rankingCanvas.enabled = true;
    }

    private void UpdateRanking()
    {
        RankingData rankingData = new RankingData();
        rankingData.playerName = PlayerNameTempSaveSolo.playerName;
        rankingData.finalPosition = finalPosition;
        rankingData.checkPointIndex = currentCheckPointIndex;
        rankingData.remainingTime = Mathf.FloorToInt(remainingTime);
        rankingData.timeElapsed = timeElapsedText.text;
        if (rankingDataFile == null) { return; }
        int index = 0;
        RankingJson rankingJson = JsonUtility.FromJson<RankingJson>(rankingDataFile.text);
        foreach (RankingData data in rankingJson.detail)
        {
            //Debug.Log("Index: " + index);
            string playerCheckPointIndexKey = Difficulty_1P_TempSave.chosenDifficulty + "_CheckPointIndex_" + (index + 1).ToString();
            int dataCheckPointIndex = PlayerPrefs.HasKey(playerCheckPointIndexKey) ? PlayerPrefs.GetInt(playerCheckPointIndexKey) : data.checkPointIndex;
            //Debug.Log(rankingData.checkPointIndex+" --- "+ dataCheckPointIndex);
            if(rankingData.checkPointIndex >= dataCheckPointIndex)
            {
                if (rankingData.checkPointIndex == dataCheckPointIndex)
                {
                    string playerRemainingTimeKey = Difficulty_1P_TempSave.chosenDifficulty + "_RemainingTime_" + (index + 1).ToString();
                    int dataRemainingTime = PlayerPrefs.HasKey(playerRemainingTimeKey) ? PlayerPrefs.GetInt(playerRemainingTimeKey) : data.remainingTime;
                    // 
                    if (rankingData.remainingTime >= dataRemainingTime)
                    {
                        for (int j = rankingJson.detail.Length - 1; j > index; j--)
                        {
                            rankingJson.detail[j].playerName = rankingJson.detail[j - 1].playerName;
                            rankingJson.detail[j].finalPosition = rankingJson.detail[j - 1].finalPosition;
                            rankingJson.detail[j].remainingTime = rankingJson.detail[j - 1].remainingTime;
                            rankingJson.detail[j].timeElapsed = rankingJson.detail[j - 1].timeElapsed;
                        }
                        rankingJson.detail[index].playerName = rankingData.playerName;
                        rankingJson.detail[index].finalPosition = rankingData.finalPosition;
                        rankingJson.detail[index].remainingTime = rankingData.remainingTime;
                        rankingJson.detail[index].timeElapsed = rankingData.timeElapsed;
                        rankingPlayerNameText[index].text = rankingData.playerName;
                        switch (Language.gameDisplayLanguage)
                        {
                            case Language.DisplayLanauge.English:
                                rankingFinalPositionText[index].text = rankingData.finalPosition.ToString();
                                rankingFinalPositionText[index].font = gameplaySoloENFont;
                                break;
                            case Language.DisplayLanauge.Japanese:
                                rankingFinalPositionText[index].text = rankingData.finalPosition.ToString()
                                                                        .Replace("Start", gameplaySolo_JP.GetLabelContent("StartPositionText"))
                                                                        .Replace("Check Point", gameplaySolo_JP.GetLabelContent("CheckPointPositionText"))
                                                                        .Replace("Goal", gameplaySolo_JP.GetLabelContent("GoalPositionText"));
                                rankingFinalPositionText[index].font = gameplaySoloJPFont;
                                rankingFinalPositionText[index].fontStyle = FontStyle.Bold;
                                break;
                        }
                        rankingRemainingTimeText[index].text = rankingData.remainingTime.ToString();
                        rankingTimeElaspedText[index].text = rankingData.timeElapsed;
                        break;
                    }
                }
                // 
                else
                {
                    for (int j = rankingJson.detail.Length - 1; j > index; j--)
                    {
                        rankingJson.detail[j].playerName = rankingJson.detail[j - 1].playerName;
                        rankingJson.detail[j].finalPosition = rankingJson.detail[j - 1].finalPosition;
                        rankingJson.detail[j].remainingTime = rankingJson.detail[j - 1].remainingTime;
                        rankingJson.detail[j].timeElapsed = rankingJson.detail[j - 1].timeElapsed;
                    }
                    rankingJson.detail[index].playerName = rankingData.playerName;
                    rankingJson.detail[index].finalPosition = rankingData.finalPosition;
                    rankingJson.detail[index].remainingTime = rankingData.remainingTime;
                    rankingJson.detail[index].timeElapsed = rankingData.timeElapsed;
                    rankingPlayerNameText[index].text = rankingData.playerName;
                    switch (Language.gameDisplayLanguage)
                    {
                        case Language.DisplayLanauge.English:
                            rankingFinalPositionText[index].text = rankingData.finalPosition.ToString();
                            rankingFinalPositionText[index].font = gameplaySoloENFont;
                            break;
                        case Language.DisplayLanauge.Japanese:
                            rankingFinalPositionText[index].text = rankingData.finalPosition.ToString()
                                                                        .Replace("Start", gameplaySolo_JP.GetLabelContent("StartPositionText"))
                                                                        .Replace("Check Point", gameplaySolo_JP.GetLabelContent("CheckPointPositionText"))
                                                                        .Replace("Goal", gameplaySolo_JP.GetLabelContent("GoalPositionText"));
                            rankingFinalPositionText[index].font = gameplaySoloJPFont;
                            rankingFinalPositionText[index].fontStyle = FontStyle.Bold;
                            break;
                    }
                    rankingRemainingTimeText[index].text = rankingData.remainingTime.ToString();
                    rankingTimeElaspedText[index].text = rankingData.timeElapsed;
                    break;
                }
            }
            index++;
        }


        if (index >= rankingJson.detail.Length)
        {
            clearOutOfRankText.enabled = true;
            clearRankedLabel.enabled = false;
            clearRankedText.enabled = false;

            gameOverOutOfRankText.enabled = true;
            gameOverRankedLabel.enabled = false;
            gameOverRankingPositionText.enabled = false;
        }
        else
        {
            clearOutOfRankText.enabled = false;
            clearRankedLabel.enabled = true;
            clearRankedText.enabled = true;

            gameOverOutOfRankText.enabled = false;
            gameOverRankedLabel.enabled = true;
            gameOverRankingPositionText.enabled = true;
            switch (index)
            {
                case 0:
                    switch (Language.gameDisplayLanguage)
                    {
                        case Language.DisplayLanauge.English:
                            clearRankedText.text = "1st";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "1st";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                        case Language.DisplayLanauge.Japanese:
                            clearRankedText.text = "1��";
                            gameOverRankingPositionText.text = "1��";
                            break;
                        default:
                            clearRankedText.text = "1st";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "1st";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                    }
                    break;
                case 1:
                    switch (Language.gameDisplayLanguage)
                    {
                        case Language.DisplayLanauge.English:
                            clearRankedText.text = "2nd";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "2nd";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                        case Language.DisplayLanauge.Japanese:
                            clearRankedText.text = "2��";
                            clearRankedText.font = gameplaySoloJPFont;
                            gameOverRankingPositionText.text = "2��";
                            break;
                        default:
                            clearRankedText.text = "2nd";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "2nd";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                    }
                    break;
                case 2:
                    switch (Language.gameDisplayLanguage)
                    {
                        case Language.DisplayLanauge.English:
                            clearRankedText.text = "3rd";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "3rd";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                        case Language.DisplayLanauge.Japanese:
                            clearRankedText.text = "3��";
                            clearRankedText.font = gameplaySoloJPFont;
                            gameOverRankingPositionText.text = "3��";
                            gameOverRankingPositionText.font = gameplaySoloJPFont;
                            break;
                        default:
                            clearRankedText.text = "3rd";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "3rd";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                    }
                    break;
                case 3:
                    switch (Language.gameDisplayLanguage)
                    {
                        case Language.DisplayLanauge.English:
                            clearRankedText.text = "4th";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "4th";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                        case Language.DisplayLanauge.Japanese:
                            clearRankedText.text = "4��";
                            clearRankedText.font = gameplaySoloJPFont;
                            gameOverRankingPositionText.text = "4��";
                            gameOverRankingPositionText.font = gameplaySoloJPFont;
                            break;
                        default:
                            clearRankedText.text = "4th";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "4th";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                    }
                    break;
                case 4:
                    switch (Language.gameDisplayLanguage)
                    {
                        case Language.DisplayLanauge.English:
                            clearRankedText.text = "5th";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "5th";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                        case Language.DisplayLanauge.Japanese:
                            clearRankedText.text = "5��";
                            clearRankedText.font = gameplaySoloJPFont;
                            gameOverRankingPositionText.text = "5��";
                            gameOverRankingPositionText.font = gameplaySoloJPFont;
                            break;
                        default:
                            clearRankedText.text = "5th";
                            clearRankedText.font = gameplaySoloENFont;
                            gameOverRankingPositionText.text = "5th";
                            gameOverRankingPositionText.font = gameplaySoloENFont;
                            break;
                    }
                    break;
            }
        }
        /*
        string saveRankingData = JsonUtility.ToJson(rankingJson, true);
        Debug.Log(saveRankingData);
        File.WriteAllText(Application.dataPath + "/Resources/ranking_1P_"+Difficulty_1P_TempSave.chosenDifficulty+".json", saveRankingData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        */
        UpdateRankingByPlayerPref(rankingJson);
    }

    private void UpdateRankingByPlayerPref(RankingJson rankingJson)
    {
        //Debug.Log("rankingJson.detail.Length: "+ rankingJson.detail.Length);
        for (int i = 1; i <= rankingJson.detail.Length; i++)
        {
            string playerNameKey = Difficulty_1P_TempSave.chosenDifficulty + "_PlayerName_" + i.ToString();
            PlayerPrefs.SetString(playerNameKey, rankingJson.detail[i-1].playerName);

            string playerFinalPositionKey = Difficulty_1P_TempSave.chosenDifficulty + "_FinalPosition_" + i.ToString();
            PlayerPrefs.SetString(playerFinalPositionKey, rankingJson.detail[i-1].finalPosition);

            string playerCheckPointIndexKey = Difficulty_1P_TempSave.chosenDifficulty + "_CheckPointIndex_" + i.ToString();
            PlayerPrefs.SetInt(playerCheckPointIndexKey, rankingJson.detail[i-1].checkPointIndex);

            string playerRemainingTimeKey = Difficulty_1P_TempSave.chosenDifficulty + "_RemainingTime_" + i.ToString();
            PlayerPrefs.SetInt(playerRemainingTimeKey, rankingJson.detail[i-1].remainingTime);

            string playerTimeElapsedKey = Difficulty_1P_TempSave.chosenDifficulty + "_TimeElapsed_" + i.ToString();
            PlayerPrefs.SetString(playerTimeElapsedKey, rankingJson.detail[i-1].timeElapsed);
        }
    }

    public void ReturnFromRanking()
    {
        if (clearPanel == null || gameOverPanel == null || rankingCanvas == null) { return; }
        rankingCanvas.enabled = false;
        switch (fromPanel)
        {
            case "Clear":
                clearLabelText.enabled = true;
                clearThankToyForPlayingLabelText.enabled = true;
                clearPanel.SetActive(true);
                break;
            case "GameOver":
                gameOverLabelText.enabled = true;
                gameOverThankYouForPlayingLabelText.enabled = true;
                gameOverPanel.SetActive(true);
                break;
            default:
                gameOverPanel.SetActive(true);
                break;
        }
    }

    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }
}
