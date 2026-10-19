using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerMultiplay : MonoBehaviour
{

    public static GameManagerMultiplay Instance
    {
        get; private set;
    }

    [SerializeField] Localization gameManagerMultiplay_EN, gameManagerMultiplay_JP;
    [SerializeField] Font gameManagerMultiplayENFont, gameManagerMultiplayJPFont;
    [SerializeField] GameObject rocketMesh;
    [SerializeField] GameObject[] playerRockets;
    [SerializeField] GameObject[] fuelObjects;
    [SerializeField] Camera[] followPlayerCameras;
    [SerializeField] Camera[] playerStageViewCameras;
    [SerializeField] Transform[] startPositions;
    [SerializeField] Transform[] checkPointPositions;
    [SerializeField] Transform goalPosition;
    [SerializeField] Transform[] lastCheckPointPositions;
    [SerializeField] Waypoint[] wayPoints;
    [SerializeField] Text[] playerNameTexts;
    [SerializeField] Text[] playerSpeedTexts;
    [SerializeField] Text[] playerCheckPointTexts;
    [SerializeField] Text[] playerTimeElapsedTexts;
    [SerializeField] Text[] playerTeamLabels;
    [SerializeField] Text[] playerTeamTexts;
    [SerializeField] Text[] playerPositionTexts;
    [SerializeField] Text[] playerNameLabelTexts;
    [SerializeField] Text[] playerSpeedLabelTexts;
    [SerializeField] Text[] playerCheckPointLabelTexts;
    [SerializeField] Text[] playerTimeElapsedLabelTexts;
    [SerializeField] Text[] playerPositionLabelTexts;
    [SerializeField] Text[] playerItemLabelTexts;
    [SerializeField] Text[] targetPlayerLabelTexts;
    [SerializeField] Text finishText, pauseText, thankYouForPlayingText;
    [SerializeField] Text winnerPlayerNameText;
    [SerializeField] Text countDownTimeText;
    [SerializeField] Text clearRestartButtonText, clearTitleButtonText;
    [SerializeField] Text pauseResumeButtonText, pauseRestartButtonText, pauseTitleButtonText;
    [SerializeField] Text winnerText, congradulationText;
    [SerializeField] Text[] sliderStartTexts, sliderGoalTexts;
    [SerializeField] Color teamAColor, teamBColor;
    [SerializeField] float startPointSpawnPositionOffsetY = 10f;
    [SerializeField] float checkPointSpawnPositionOffsetX = 5f;
    [SerializeField] float checkPointSpawnPositionOffsetY = 10f;
    [SerializeField] float stopOtherPlayersMovementTime = 3f;
    [SerializeField] float stopMissilesMovementTime = 3f;
    [SerializeField] float showFinishPanelTime = 2f;
    [SerializeField] float startCountDownTime = 3.9f;
    [SerializeField] float hideCountDownTime = 1f;
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] float reduceOtherPlayerSpeedFactor = 20f;
    [SerializeField] float reduceOtherPlayerSpeedTime = 3f;
    [SerializeField] float reduceOtherPlayerBoostFactor = 0.8f;
    [SerializeField] string startGameText = "GO!";
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] GameObject finishPanel;
    [SerializeField] Canvas finishCanvas;
    [SerializeField] Slider[] stageMiniSliders;
    [SerializeField] RandomItemSettings[] playerRandomItemSettings;

    Dictionary<int, float> playerRemainingDistances;
    Dictionary<KeyCode, bool> keys = new Dictionary<KeyCode, bool>();
    float countDownTimer, startTime;
    float[] playerTotalDistance;
    string finalElapsedTime;
    string winnerTeam;
    bool isPaused;

    public enum GameState
    {
        CountDown,
        GameStart,
        Pause,
        Finish
    }
    private static GameState currentGameState;

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
        playerTotalDistance = new float[playerRockets.Length];
        playerRemainingDistances = new Dictionary<int, float>();
        List<int> teamAIDs = MultiplayPlayerMode.TeamAPlayerIDs;
        List<int> teamBIDs = MultiplayPlayerMode.TeamBPlayerIDs;
        for (int i = 0; i < playerRockets.Length; i++)
        {
            if (playerRockets[i] != null)
            {
                playerRockets[i].transform.position = startPositions[i].transform.position + new Vector3(checkPointSpawnPositionOffsetX, startPointSpawnPositionOffsetY, 0f); ;
                playerNameTexts[i].text = PlayerNameTempSaveMultiplay.playerName[i];
                playerRockets[i].GetComponent<MovementMultiplay>().DisablePlayerControl();
                lastCheckPointPositions[i] = startPositions[i];
                playerCheckPointTexts[i].text = "Start";
                playerSpeedTexts[i].text = "0Km / s";
            }
            if(followPlayerCameras[i] != null)
            {
                followPlayerCameras[i].GetComponent<PostProcessLayer>().enabled = false;
            }
            if (playerStageViewCameras[i] != null)
            {
                playerStageViewCameras[i].GetComponent<PostProcessLayer>().enabled = false;
            }
            if(MultiplayPlayerMode.gameMode.Equals("BattleRoyale"))
            {
                if(playerTeamLabels[i] != null)
                {
                    playerTeamLabels[i].enabled = false;
                }
                if(playerTeamLabels[i] != null)
                {
                    playerTeamTexts[i].enabled = false;
                }
            }
            else
            {
                int playerID = i + 1;
                if(teamAIDs.Contains(playerID))
                {
                    playerTeamTexts[i].text = "A";
                    playerTeamLabels[i].color = teamAColor;
                    playerTeamTexts[i].color = teamAColor;
                }
                else if(teamBIDs.Contains(playerID))
                {
                    playerTeamTexts[i].text = "B";
                    playerTeamLabels[i].color = teamBColor;
                    playerTeamTexts[i].color = teamBColor;
                }
            }
            CalculatTotalDistance(i);
        }
        if (finishCanvas != null)
        {
            finishCanvas.enabled = false;
        }
        pauseCanvas.enabled = false;
        currentGameState = GameState.CountDown;
        finalElapsedTime = "";
        Time.timeScale = 1f;
        countDownTimer = startCountDownTime;
        winnerTeam = "";
        isPaused = false;
        if (Language.gameDisplayLanguage == Language.DisplayLanauge.None)
        {
            Language.gameDisplayLanguage = Language.DisplayLanauge.English;
        }
        switch (Language.gameDisplayLanguage)
        {
            case Language.DisplayLanauge.English:
                for(int i = 0; i < playerNameLabelTexts.Length; i++)
                {
                    playerNameLabelTexts[i].text = gameManagerMultiplay_EN.GetLabelContent("PlayerNameLabelText");
                    playerNameLabelTexts[i].font = gameManagerMultiplayENFont;
                }
                for (int i = 0; i < playerSpeedLabelTexts.Length; i++)
                {
                    playerSpeedLabelTexts[i].text = gameManagerMultiplay_EN.GetLabelContent("PlayerSpeedLabelText");
                    playerSpeedLabelTexts[i].font = gameManagerMultiplayENFont;
                }
                for (int i = 0; i < playerCheckPointLabelTexts.Length; i++)
                {
                    playerCheckPointLabelTexts[i].text = gameManagerMultiplay_EN.GetLabelContent("PlayerCheckPointLabelText");
                    playerCheckPointLabelTexts[i].font = gameManagerMultiplayENFont;
                }
                for (int i = 0; i < playerTimeElapsedLabelTexts.Length; i++)
                {
                    playerTimeElapsedLabelTexts[i].text = gameManagerMultiplay_EN.GetLabelContent("PlayerTimeElapsedLabelText");
                    playerTimeElapsedLabelTexts[i].font = gameManagerMultiplayENFont;
                }
                for (int i = 0; i < playerPositionLabelTexts.Length; i++)
                {
                    playerPositionLabelTexts[i].text = gameManagerMultiplay_EN.GetLabelContent("PlayerPositionLabelText");
                    playerPositionLabelTexts[i].font = gameManagerMultiplayENFont;
                }
                for (int i = 0; i < playerItemLabelTexts.Length; i++)
                {
                    playerItemLabelTexts[i].text = gameManagerMultiplay_EN.GetLabelContent("PlayerItemLabelText");
                    playerItemLabelTexts[i].font = gameManagerMultiplayENFont;
                }
                for (int i = 0; i < targetPlayerLabelTexts.Length; i++)
                {
                    targetPlayerLabelTexts[i].text = gameManagerMultiplay_EN.GetLabelContent("TargetPlayerLabelText");
                    targetPlayerLabelTexts[i].font = gameManagerMultiplayENFont;
                }
                finishText.text = gameManagerMultiplay_EN.GetLabelContent("FinishText");
                finishText.font = gameManagerMultiplayENFont;
                pauseText.text = gameManagerMultiplay_EN.GetLabelContent("PauseText");
                pauseText.font = gameManagerMultiplayENFont;
                thankYouForPlayingText.text = gameManagerMultiplay_EN.GetLabelContent("ThankYouForPlayingText");
                thankYouForPlayingText.font = gameManagerMultiplayENFont;
                clearRestartButtonText.text = gameManagerMultiplay_EN.GetLabelContent("ClearRestartButtonText");
                clearRestartButtonText.font = gameManagerMultiplayENFont;
                clearTitleButtonText.text = gameManagerMultiplay_EN.GetLabelContent("ClearTitleButtonText");
                clearTitleButtonText.font = gameManagerMultiplayENFont;
                pauseResumeButtonText.text = gameManagerMultiplay_EN.GetLabelContent("PauseResumeButtonText");
                pauseResumeButtonText.font = gameManagerMultiplayENFont;
                pauseRestartButtonText.text = gameManagerMultiplay_EN.GetLabelContent("PauseRestartButtonText");
                pauseRestartButtonText.font = gameManagerMultiplayENFont;
                pauseTitleButtonText.text = gameManagerMultiplay_EN.GetLabelContent("PauseTitleButtonText");
                pauseTitleButtonText.font = gameManagerMultiplayENFont;
                for (int i = 0; i < sliderStartTexts.Length; i++)
                {
                    sliderStartTexts[i].text = gameManagerMultiplay_EN.GetLabelContent("SliderStartText");
                    sliderStartTexts[i].font = gameManagerMultiplayENFont;
                }
                for (int i = 0; i < sliderGoalTexts.Length; i++)
                {
                    sliderGoalTexts[i].text = gameManagerMultiplay_EN.GetLabelContent("SliderGoalText");
                    sliderGoalTexts[i].font = gameManagerMultiplayENFont;
                }
                winnerText.text = gameManagerMultiplay_EN.GetLabelContent("WinnerText");
                winnerText.font = gameManagerMultiplayENFont;
                congradulationText.text = gameManagerMultiplay_EN.GetLabelContent("CongradulationText");
                congradulationText.font = gameManagerMultiplayENFont;
                break;
            case Language.DisplayLanauge.Japanese:
                for (int i = 0; i < playerNameLabelTexts.Length; i++)
                {
                    playerNameLabelTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("PlayerNameLabelText");
                    playerNameLabelTexts[i].font = gameManagerMultiplayJPFont;
                    playerNameLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                for (int i = 0; i < playerSpeedLabelTexts.Length; i++)
                {
                    playerSpeedLabelTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("PlayerSpeedLabelText");
                    playerSpeedLabelTexts[i].font = gameManagerMultiplayJPFont;
                    playerSpeedLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                for (int i = 0; i < playerCheckPointLabelTexts.Length; i++)
                {
                    playerCheckPointLabelTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("PlayerCheckPointLabelText");
                    playerCheckPointLabelTexts[i].font = gameManagerMultiplayJPFont;
                    playerCheckPointLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                for (int i = 0; i < playerTimeElapsedLabelTexts.Length; i++)
                {
                    playerTimeElapsedLabelTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("PlayerTimeElapsedLabelText");
                    playerTimeElapsedLabelTexts[i].font = gameManagerMultiplayJPFont;
                    playerTimeElapsedLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                for (int i = 0; i < playerPositionLabelTexts.Length; i++)
                {
                    playerPositionLabelTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("PlayerPositionLabelText");
                    playerPositionLabelTexts[i].font = gameManagerMultiplayJPFont;
                    playerPositionLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                for (int i = 0; i < playerItemLabelTexts.Length; i++)
                {
                    playerItemLabelTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("PlayerItemLabelText");
                    playerItemLabelTexts[i].font = gameManagerMultiplayJPFont;
                    playerItemLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                for (int i = 0; i < targetPlayerLabelTexts.Length; i++)
                {
                    targetPlayerLabelTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("TargetPlayerLabelText");
                    targetPlayerLabelTexts[i].font = gameManagerMultiplayJPFont;
                    targetPlayerLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                finishText.text = gameManagerMultiplay_JP.GetLabelContent("FinishText");
                finishText.font = gameManagerMultiplayJPFont;
                finishText.fontStyle = FontStyle.Bold;
                pauseText.text = gameManagerMultiplay_JP.GetLabelContent("PauseText");
                pauseText.font = gameManagerMultiplayJPFont;
                pauseText.fontStyle = FontStyle.Bold;
                thankYouForPlayingText.text = gameManagerMultiplay_JP.GetLabelContent("ThankYouForPlayingText");
                thankYouForPlayingText.font = gameManagerMultiplayJPFont;
                thankYouForPlayingText.fontStyle = FontStyle.Bold;
                clearRestartButtonText.text = gameManagerMultiplay_JP.GetLabelContent("ClearRestartButtonText");
                clearRestartButtonText.font = gameManagerMultiplayJPFont;
                clearRestartButtonText.fontStyle = FontStyle.Bold;
                clearTitleButtonText.text = gameManagerMultiplay_JP.GetLabelContent("ClearTitleButtonText");
                clearTitleButtonText.font = gameManagerMultiplayJPFont;
                clearTitleButtonText.fontStyle = FontStyle.Bold;
                pauseResumeButtonText.text = gameManagerMultiplay_JP.GetLabelContent("PauseResumeButtonText");
                pauseResumeButtonText.font = gameManagerMultiplayJPFont;
                pauseResumeButtonText.fontStyle = FontStyle.Bold;
                pauseRestartButtonText.text = gameManagerMultiplay_JP.GetLabelContent("PauseRestartButtonText");
                pauseRestartButtonText.font = gameManagerMultiplayJPFont;
                pauseRestartButtonText.fontStyle = FontStyle.Bold;
                pauseTitleButtonText.text = gameManagerMultiplay_JP.GetLabelContent("PauseTitleButtonText");
                pauseTitleButtonText.font = gameManagerMultiplayJPFont;
                pauseTitleButtonText.fontStyle = FontStyle.Bold;
                for (int i = 0; i < sliderStartTexts.Length; i++)
                {
                    sliderStartTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("SliderStartText");
                    sliderStartTexts[i].font = gameManagerMultiplayJPFont;
                    sliderStartTexts[i].fontStyle = FontStyle.Bold;
                }
                for (int i = 0; i < sliderGoalTexts.Length; i++)
                {
                    sliderGoalTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("SliderGoalText");
                    sliderGoalTexts[i].font = gameManagerMultiplayJPFont;
                    sliderGoalTexts[i].fontStyle = FontStyle.Bold;
                }
                winnerText.text = gameManagerMultiplay_JP.GetLabelContent("WinnerText");
                winnerText.font = gameManagerMultiplayJPFont;
                winnerText.fontStyle = FontStyle.Bold;
                congradulationText.text = gameManagerMultiplay_JP.GetLabelContent("CongradulationText");
                congradulationText.font = gameManagerMultiplayJPFont;
                congradulationText.fontStyle = FontStyle.Bold;
                for (int i = 0; i < playerPositionTexts.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            playerPositionTexts[i].text = "1��";
                            playerPositionTexts[i].font = gameManagerMultiplayJPFont;
                            playerPositionTexts[i].fontStyle = FontStyle.Bold;
                            break;
                        case 1:
                            playerPositionTexts[i].text = "2��";
                            playerPositionTexts[i].font = gameManagerMultiplayJPFont;
                            playerPositionTexts[i].fontStyle = FontStyle.Bold;
                            break;
                        case 2:
                            playerPositionTexts[i].text = "3��";
                            playerPositionTexts[i].font = gameManagerMultiplayJPFont;
                            playerPositionTexts[i].fontStyle = FontStyle.Bold;
                            break;
                        case 3:
                            playerPositionTexts[i].text = "4��";
                            playerPositionTexts[i].font = gameManagerMultiplayJPFont;
                            playerPositionTexts[i].fontStyle = FontStyle.Bold;
                            break;
                    }
                }
                for(int i = 0; i < playerCheckPointTexts.Length; i++)
                {
                    playerCheckPointTexts[i].text = gameManagerMultiplay_JP.GetLabelContent("StartText");
                    playerCheckPointTexts[i].font = gameManagerMultiplayJPFont;
                    playerCheckPointTexts[i].fontStyle = FontStyle.Bold;
                }
                break;
        }
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.CountDown:
                countDownTimeText.enabled = true;
                countDownTimer -= Time.deltaTime;
                CountDown(countDownTimer);
                break;
            case GameState.GameStart:
                SortPositions();
                for (int i = 0; i < playerRockets.Length; i++)
                {
                    if (playerRockets[i] != null)
                    {
                        UpdateTimeElapsed(i);
                        UpdatePlayerSpeedText(i);
                        ShowCheckPoint(i);
                        CalculateRemainingDistance(i);
                        DisplayPosition(i);
                    }
                }
                RespondToPauseGame();
                break;
            case GameState.Finish:
                for (int i = 0; i < playerRockets.Length; i++)
                {
                    if (playerRockets[i] != null)
                    {
                        if (playerRockets[i] != null)
                        {
                            ShowCheckPoint(i);
                        }
                    }
                }
                break;
            case GameState.Pause:
                RespondToPauseGame();
                break;
            default:
                break;
        }
        RotateRocketMeshContinously();
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
            Invoke("HideCountDownText", hideCountDownTime);
            Instance.ChangeGameState(GameState.GameStart);
            for(int i = 0; i < playerRockets.Length; i++)
            {
                playerRockets[i].GetComponent<MovementMultiplay>().EnablePlayerControl();
            }
        }
    }

    private void ShowCountTimeText(float countDownTimer)
    {
        int remainingSeconds = Mathf.FloorToInt(countDownTimer);
        switch (Language.gameDisplayLanguage)
        {
            case Language.DisplayLanauge.English:
                countDownTimeText.text = (remainingSeconds > 0) ? remainingSeconds.ToString() : gameManagerMultiplay_EN.GetLabelContent("GoText");
                countDownTimeText.font = gameManagerMultiplayENFont;
                break;
            case Language.DisplayLanauge.Japanese:
                countDownTimeText.text = (remainingSeconds > 0) ? remainingSeconds.ToString() : gameManagerMultiplay_JP.GetLabelContent("GoText");
                countDownTimeText.font = gameManagerMultiplayJPFont;
                countDownTimeText.fontStyle = FontStyle.Bold;
                break;
        }
    }

    private void HideCountDownText()
    {
        countDownTimeText.enabled = false;
        startTime = Time.time - countDownTimer;
    }


    private void UpdateTimeElapsed(int playerIndex)
    {
        float t = Time.time - PlayTimeTempSaveMultiplay.totalTimeElapsed[playerIndex] - startCountDownTime;
        string minutes = ((int)t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        string miliseconds = (t * 60 % 60).ToString("00");
        finalElapsedTime = minutes + ":" + seconds + ":" + miliseconds;

        if (playerTimeElapsedTexts[playerIndex] == null) { return; }
        playerTimeElapsedTexts[playerIndex].text = finalElapsedTime;
    }

    private void ShowCheckPoint(int playerIndex)
    {
        string currentCheckPoint = lastCheckPointPositions[playerIndex].gameObject.name;
        //Debug.Log("currentCheckPoint: " + currentCheckPoint);
        if (currentCheckPoint.Contains("Start"))
        {
            currentCheckPoint = "Start";
        }
        switch (Language.gameDisplayLanguage)
        {
            case Language.DisplayLanauge.English:
                playerCheckPointTexts[playerIndex].text = currentCheckPoint;
                playerCheckPointTexts[playerIndex].font = gameManagerMultiplayENFont;
                break;
            case Language.DisplayLanauge.Japanese:
                playerCheckPointTexts[playerIndex].text = currentCheckPoint.Replace("Start", gameManagerMultiplay_JP.GetLabelContent("StartText"))
                                                                           .Replace("CheckPoint", gameManagerMultiplay_JP.GetLabelContent("CheckpointText"))
                                                                           .Replace("Goal", gameManagerMultiplay_JP.GetLabelContent("GoalText"));
                playerCheckPointTexts[playerIndex].font = gameManagerMultiplayJPFont;
                playerCheckPointTexts[playerIndex].fontStyle = FontStyle.Bold;
                break;
        }
    }

    private void UpdateStageMiniSlider(int playerIndex)
    {
        if (playerRockets[playerIndex - 1].transform.position.x > goalPosition.transform.position.x)
        {
            stageMiniSliders[playerIndex].value = 0;
        }
        else
        {
            stageMiniSliders[playerIndex].value = -Vector3.Distance(goalPosition.position, playerRockets[playerIndex - 1].transform.position);
        }
    }

    internal void SaveLatestCheckPoint(int playerIndex, Transform checkPointPos)
    {
        lastCheckPointPositions[playerIndex - 1] = checkPointPos;
    }

    private void UpdatePlayerSpeedText(int playerIndex)
    {
        if (playerSpeedTexts[playerIndex] == null) { return; }
        float currentSpeed;
        currentSpeed = Mathf.FloorToInt(playerRockets[playerIndex].GetComponent<Rigidbody>().linearVelocity.magnitude);
        playerSpeedTexts[playerIndex].text = currentSpeed.ToString() + "Km / s";
    }

    private void RespondToPauseGame()
    {
        if(currentGameState == GameState.GameStart)
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("1PPause") ||
                Input.GetButtonDown("2PPause") ||
                Input.GetButtonDown("3PPause") ||
                Input.GetButtonDown("4PPause"))
            {
                PauseGame();
            }
        }
        else if (currentGameState == GameState.Pause)
        {

            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("1PPause") ||
                Input.GetButtonDown("2PPause") ||
                Input.GetButtonDown("3PPause") ||
                Input.GetButtonDown("4PPause"))
            {
                ResumeGame();
            }
        }
    }

    public void ResetPlayerToStartPosition(int playerIndex, GameObject player)
    {
        if (lastCheckPointPositions[playerIndex - 1] != null)
        {
            player.transform.position = lastCheckPointPositions[playerIndex - 1].transform.position + new Vector3(checkPointSpawnPositionOffsetX, checkPointSpawnPositionOffsetY, 0f);
        }
        else if (startPositions[playerIndex - 1] != null)
        {
            player.transform.position = startPositions[playerIndex - 1].transform.position + new Vector3(0f, startPointSpawnPositionOffsetY, 0f);
        }
        ResetPlayerRotation(player);
    }

    public void ResetPlayerRotation(GameObject player)
    {
        player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        player.GetComponent<MovementMultiplay>().StopMovement();
    }
    public void ActiviateAllFuelObjects()
    {
        foreach (GameObject fuel in fuelObjects)
        {
            if (fuel.GetComponent<Fuel>() != null)
            {
                fuel.GetComponent<Fuel>().ActivateFuelObject();
            }
        }
    }

    public void FinishBattleRoyale(int playerID)
    {
        if (finishCanvas == null) { return; }
        finishCanvas.enabled = true;
        ChangeGameState(GameState.Finish);
        StartCoroutine(ShowFinishPanelBattleRoyale(playerID));
    }

    public void FinishTeamPlay()
    {
        if (finishCanvas == null) { return; }
        finishCanvas.enabled = true;
        ChangeGameState(GameState.Finish);
        StartCoroutine(ShowFinishPanelTeamPlay());
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Instance.ChangeGameState(GameState.Pause);
        pauseCanvas.enabled = true;
        for(int i = 0; i < playerRockets.Length; i++)
        {
            playerRockets[i].GetComponent<MovementMultiplay>().DisablePlayerControl();
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Instance.ChangeGameState(GameState.GameStart);
        pauseCanvas.enabled = false;
        for (int i = 0; i < playerRockets.Length; i++)
        {
            playerRockets[i].GetComponent<MovementMultiplay>().EnablePlayerControl();
        }
    }
    public void RestartGame()
    {
        PlayTimeTempSaveSolo.totalTimeElapsed = Time.time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    private IEnumerator ShowFinishPanelBattleRoyale(int playerIndex)
    {
        yield return new WaitForSeconds(showFinishPanelTime);

        if (finishPanel != null)
        {
            finishText.enabled = true;
            finishPanel.SetActive(true);
            string winnerPlayerName = PlayerNameTempSaveMultiplay.playerName[playerIndex - 1];
            if(winnerPlayerNameText != null)
            {
                winnerPlayerNameText.text = winnerPlayerName;
            }
        }
    }

    private IEnumerator ShowFinishPanelTeamPlay()
    {
        yield return new WaitForSeconds(showFinishPanelTime);

        if (finishPanel != null)
        {
            finishText.enabled = true;   
            finishPanel.SetActive(true);
            string winnerPlayerName1 = "", winnerPlayerName2 = "";
            if (winnerTeam == "Team A")
            {
                winnerPlayerName1 = PlayerNameTempSaveMultiplay.playerName[MultiplayPlayerMode.TeamAPlayerIDs[0]];
                winnerPlayerName2 = PlayerNameTempSaveMultiplay.playerName[MultiplayPlayerMode.TeamAPlayerIDs[1]];
            }
            else
            {
                winnerPlayerName1 = PlayerNameTempSaveMultiplay.playerName[MultiplayPlayerMode.TeamBPlayerIDs[0]];
                winnerPlayerName2 = PlayerNameTempSaveMultiplay.playerName[MultiplayPlayerMode.TeamBPlayerIDs[1]];
            }
            if (winnerPlayerNameText != null)
            {
                winnerPlayerNameText.text = winnerPlayerName1 + Environment.NewLine + winnerPlayerName2;
            }
        }
    }

    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }

    private void RotateRocketMeshContinously()
    {
        if(rocketMesh == null || currentGameState == GameState.Pause) { return; }
        Time.timeScale = 1f;
        rocketMesh.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    public void StopOtherPlayersMovement(int playerID)
    {
        int playerIndex = playerID - 1;
        if (MultiplayPlayerMode.gameMode.Equals("BattleRoyale"))
        {
            StartCoroutine(StopPlayersMovementAndGrayScreenBattleRoyale(playerIndex));
        }
        else
        {
            string teamBelongsTo;
            //if (Array.IndexOf(MultiplayPlayerMode.TeamAPlayerIDs, playerID) >= 0)
            if(MultiplayPlayerMode.TeamAPlayerIDs.Contains(playerID))
            {
                teamBelongsTo = "Team A";
            }
            else
            {
                teamBelongsTo = "Team B";
            }
            StartCoroutine(StopPlayersMovementAndGrayScreenTeamPlay(playerIndex, teamBelongsTo));
        }
    }

    public void ReduceOtherPlayersSpeed(int playerID)
    {
        int playerIndex = playerID - 1;
        if (MultiplayPlayerMode.gameMode.Equals("BattleRoyale"))
        {
            StartCoroutine(ReducePlayerSpeedForAWhileBattleRoyale(playerIndex));
        }
        else
        {
            string teamBelongsTo;
            //if (Array.IndexOf(MultiplayPlayerMode.TeamAPlayerIDs, playerID) >= 0)
            if (MultiplayPlayerMode.TeamAPlayerIDs.Contains(playerID))
            {
                teamBelongsTo = "Team A";
            }
            else
            {
                teamBelongsTo = "Team B";
            }
            StartCoroutine(ReducePlayerSpeedForAWhileTeamPlay(playerIndex, teamBelongsTo));
        }
    }

    public void ReduceOtherPlayersBoost(int playerID)
    {
        if (MultiplayPlayerMode.gameMode.Equals("BattleRoyale"))
        {
            int playerIndex = playerID - 1;
            for (int i = 0; i < playerRockets.Length; i++)
            {
                if (i != playerIndex)
                {
                    playerRockets[i].GetComponent<PlayerStatusMultiplay>().ReducePlayerBoostOnce(reduceOtherPlayerBoostFactor);
                }
            }
        }
        else
        {
            string teamBelongsTo;
            if (MultiplayPlayerMode.TeamAPlayerIDs.Contains(playerID))
            {
                teamBelongsTo = "Team A";
            }
            else
            {
                teamBelongsTo = "Team B";
            }

            List<int> rivalTeamIDs;

            if (teamBelongsTo == "Team A")
            {
                rivalTeamIDs = MultiplayPlayerMode.TeamBPlayerIDs;
            }
            else
            {
                rivalTeamIDs = MultiplayPlayerMode.TeamAPlayerIDs;
            }

            for (int i = 0; i < rivalTeamIDs.Count; i++)
            {
                int rivalTeamMemberPlayerIndex = rivalTeamIDs[i] - 1;
                playerRockets[rivalTeamMemberPlayerIndex].GetComponent<PlayerStatusMultiplay>().ReducePlayerBoostOnce(reduceOtherPlayerBoostFactor);
            }
        }
    }

    private IEnumerator StopPlayersMovementAndGrayScreenBattleRoyale(int playerIndex)
    {
        for (int i = 0; i < playerRockets.Length; i++)
        {
            if (i != playerIndex)
            {
                if (followPlayerCameras[i] != null)
                {
                    followPlayerCameras[i].GetComponent<PostProcessLayer>().enabled = true;
                }
                if (playerStageViewCameras[i] != null)
                {
                    playerStageViewCameras[i].GetComponent<PostProcessLayer>().enabled = true;
                }
                playerRockets[i].GetComponent<MovementMultiplay>().DisablePlayerControl();
                playerRockets[i].GetComponent<PlayerItem>().DisableUseItem();
                playerRockets[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        yield return new WaitForSeconds(stopOtherPlayersMovementTime);


        for (int i = 0; i < playerRockets.Length; i++)
        {
            if (i != playerIndex)
            {
                if (followPlayerCameras[i] != null)
                {
                    followPlayerCameras[i].GetComponent<PostProcessLayer>().enabled = false;
                }
                if (playerStageViewCameras[i] != null)
                {
                    playerStageViewCameras[i].GetComponent<PostProcessLayer>().enabled = false;
                }
                playerRockets[i].GetComponent<MovementMultiplay>().EnablePlayerControl();
                playerRockets[i].GetComponent<PlayerItem>().EnableUseItem();
                playerRockets[i].GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezePositionZ | 
                    RigidbodyConstraints.FreezeRotationX | 
                    RigidbodyConstraints.FreezeRotationY;
            }
        }
    }

    private IEnumerator StopPlayersMovementAndGrayScreenTeamPlay(int playerIndex, string teamBelongsTo)
    {
        int playerID = playerIndex + 1;
        List<int> rivalTeamIDs;

        if (teamBelongsTo == "Team A")
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamBPlayerIDs;
        }
        else
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamAPlayerIDs;
        }

        for (int i = 0; i < rivalTeamIDs.Count; i++)
        {
            int rivalTeamMemberPlayerIndex = rivalTeamIDs[i] - 1;
            if (followPlayerCameras[rivalTeamMemberPlayerIndex] != null)
            {
                followPlayerCameras[rivalTeamMemberPlayerIndex].GetComponent<PostProcessLayer>().enabled = true;
            }
            if (playerStageViewCameras[rivalTeamMemberPlayerIndex] != null)
            {
                playerStageViewCameras[rivalTeamMemberPlayerIndex].GetComponent<PostProcessLayer>().enabled = true;
            }
            playerRockets[rivalTeamMemberPlayerIndex].GetComponent<MovementMultiplay>().DisablePlayerControl();
            playerRockets[rivalTeamMemberPlayerIndex].GetComponent<PlayerItem>().DisableUseItem();
            playerRockets[rivalTeamMemberPlayerIndex].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        yield return new WaitForSeconds(stopOtherPlayersMovementTime);


        for (int i = 0; i < rivalTeamIDs.Count; i++)
        {
            int rivalTeamMemberPlayerIndex = rivalTeamIDs[i] - 1;
            if (followPlayerCameras[rivalTeamMemberPlayerIndex] != null)
            {
                followPlayerCameras[rivalTeamMemberPlayerIndex].GetComponent<PostProcessLayer>().enabled = false;
            }
            if (playerStageViewCameras[rivalTeamMemberPlayerIndex] != null)
            {
                playerStageViewCameras[rivalTeamMemberPlayerIndex].GetComponent<PostProcessLayer>().enabled = false;
            }
            playerRockets[rivalTeamMemberPlayerIndex].GetComponent<MovementMultiplay>().EnablePlayerControl();
            playerRockets[rivalTeamMemberPlayerIndex].GetComponent<PlayerItem>().EnableUseItem();
            playerRockets[rivalTeamMemberPlayerIndex].GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationY;
        }
    }

    public void StopAllMissilesMovement()
    {
        StartCoroutine(FreezeAllMissiles());
    }

    private IEnumerator FreezeAllMissiles()
    {
        foreach(Missile missile in FindObjectsOfType<Missile>())
        {
            missile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        yield return new WaitForSeconds(stopMissilesMovementTime);

        foreach (Missile missile in FindObjectsOfType<Missile>())
        {
            missile.GetComponent<Rigidbody>().constraints = 
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotationX |
                    RigidbodyConstraints.FreezeRotationY;
        }
    }

    private IEnumerator ReducePlayerSpeedForAWhileBattleRoyale(int playerIndex)
    {
        for (int i = 0; i < playerRockets.Length; i++)
        {
            if (i != playerIndex)
            {
                playerRockets[i].GetComponent<MovementMultiplay>().ReduceMaximumSpeed(reduceOtherPlayerSpeedFactor);
            }
        }
        
        yield return new WaitForSeconds(reduceOtherPlayerSpeedTime);

        for (int i = 0; i < playerRockets.Length; i++)
        {
            if (i != playerIndex)
            {
                playerRockets[i].GetComponent<MovementMultiplay>().RestoreOrginalMaximumSpeed();
            }
        }
    }

    private IEnumerator ReducePlayerSpeedForAWhileTeamPlay(int playerIndex, string teamBelongsTo)
    {
        int playerID = playerIndex + 1;
        List<int> rivalTeamIDs;

        if (teamBelongsTo == "Team A")
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamBPlayerIDs;
        }
        else
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamAPlayerIDs;
        }

        for (int i = 0; i < rivalTeamIDs.Count; i++)
        {
            int rivalTeamMemberPlayerIndex = rivalTeamIDs[i] - 1;
            playerRockets[rivalTeamMemberPlayerIndex].GetComponent<MovementMultiplay>().ReduceMaximumSpeed(reduceOtherPlayerSpeedFactor);
        }
        
        yield return new WaitForSeconds(reduceOtherPlayerSpeedTime);

        for (int i = 0; i < rivalTeamIDs.Count; i++)
        {
            int rivalTeamMemberPlayerIndex = rivalTeamIDs[i] - 1;
            playerRockets[rivalTeamMemberPlayerIndex].GetComponent<MovementMultiplay>().RestoreOrginalMaximumSpeed();
        }
    }

    public GameObject GetClosetPlayerRocketBattleRoyale(int playerID)
    {
        GameObject closetPlayerRocket = null;
        int playerIndex = playerID - 1;
        Vector3 currentPlayerPosition = playerRockets[playerIndex].transform.position;
        float closetDiff = Mathf.Infinity;
        for (int i = 0; i < playerRockets.Length; i++)
        {
            if (i != playerIndex)
            {
                Vector3 otherplayerPosition = playerRockets[i].transform.position;
                float posDiff = Vector3.Distance(currentPlayerPosition, otherplayerPosition);
                if (posDiff < closetDiff)
                {
                    closetPlayerRocket = playerRockets[i];
                    closetDiff = posDiff;
                }
            }
        }
        return closetPlayerRocket;
    }

    public string GetClosetPlayerNameBattleRoyale(int playerID)
    {
        string closePlayerName = "";
        int playerIndex = playerID - 1;
        Vector3 currentPlayerPosition = playerRockets[playerIndex].transform.position;
        float closetDiff = Mathf.Infinity;
        for (int i = 0; i < playerRockets.Length; i++)
        {
            if (i != playerIndex)
            {
                Vector3 otherplayerPosition = playerRockets[i].transform.position;
                float posDiff = Vector3.Distance(currentPlayerPosition, otherplayerPosition);
                if(posDiff < closetDiff)
                {
                    closePlayerName = PlayerNameTempSaveMultiplay.playerName[i];
                    closetDiff = posDiff;
                }
            }
        }
        return closePlayerName;
    }

    public int GetClosetPlayerIDBattleRoyale(int playerID)
    {
        int closetPlayerID = 0;
        int playerIndex = playerID - 1;
        Vector3 currentPlayerPosition = playerRockets[playerIndex].transform.position;
        float closetDiff = Mathf.Infinity;
        for (int i = 0; i < playerRockets.Length; i++)
        {
            if (i != playerIndex)
            {
                Vector3 otherplayerPosition = playerRockets[i].transform.position;
                float posDiff = Vector3.Distance(currentPlayerPosition, otherplayerPosition);
                if (posDiff < closetDiff)
                {
                    closetPlayerID =  i+ 1;
                    closetDiff = posDiff;
                }
            }
        }
        return closetPlayerID;
    }

    public GameObject GetClosetPlayerRocketTeamPlay(int playerID, string teamBelongsTo)
    {
        GameObject closetPlayerRocket = null;
        int playerIndex = playerID - 1;
        Vector3 currentPlayerPosition = playerRockets[playerIndex].transform.position;
        float closetDiff = Mathf.Infinity;
        List<int> rivalTeamIDs;

        if (teamBelongsTo == "Team A")
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamBPlayerIDs;
        }
        else
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamAPlayerIDs;
        }

        for (int i = 0; i < rivalTeamIDs.Count; i++)
        {
            if (rivalTeamIDs[i] != playerIndex)
            {
                Vector3 otherplayerPosition = playerRockets[rivalTeamIDs[i]].transform.position;
                float posDiff = Vector3.Distance(currentPlayerPosition, otherplayerPosition);
                if (posDiff < closetDiff)
                {
                    closetPlayerRocket = playerRockets[rivalTeamIDs[i] - 1];
                    closetDiff = posDiff;
                }
            }
        }
        return closetPlayerRocket;
    }

    public string GetClosetPlayerNameTeamPlay(int playerID, string teamBelongsTo)
    {
        string closePlayerName = "";
        int playerIndex = playerID - 1;
        Vector3 currentPlayerPosition = playerRockets[playerIndex].transform.position;
        float closetDiff = Mathf.Infinity;

        List<int> rivalTeamIDs;
        if (teamBelongsTo == "Team A")
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamBPlayerIDs;
        }
        else
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamAPlayerIDs;
        }

        for (int i = 0; i < rivalTeamIDs.Count; i++)
        {
            if (rivalTeamIDs[i] != playerIndex)
            {
                Vector3 otherplayerPosition = playerRockets[rivalTeamIDs[i]].transform.position;
                float posDiff = Vector3.Distance(currentPlayerPosition, otherplayerPosition);
                if (posDiff < closetDiff)
                {
                    closePlayerName = PlayerNameTempSaveMultiplay.playerName[rivalTeamIDs[i] - 1];
                    closetDiff = posDiff;
                }
            }
        }
        return closePlayerName;
    }

    public int GetClosetPlayerIDTeamPlay(int playerID, string teamBelongsTo)
    {
        int closetPlayerID = 0;
        int playerIndex = playerID - 1;
        Vector3 currentPlayerPosition = playerRockets[playerIndex].transform.position;
        float closetDiff = Mathf.Infinity;

        List<int> rivalTeamIDs;
        if (teamBelongsTo == "Team A")
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamBPlayerIDs;
        }
        else
        {
            rivalTeamIDs = MultiplayPlayerMode.TeamAPlayerIDs;
        }
        for (int i = 0; i < rivalTeamIDs.Count; i++)
        {
            if (rivalTeamIDs[i] != playerIndex)
            {
                Vector3 otherplayerPosition = playerRockets[rivalTeamIDs[i]].transform.position;
                float posDiff = Vector3.Distance(currentPlayerPosition, otherplayerPosition);
                if (posDiff < closetDiff)
                {
                    closetPlayerID = rivalTeamIDs[i];
                    closetDiff = posDiff;
                }
            }
        }
        return closetPlayerID;
    }

    private void CalculatTotalDistance(int playerIndex)
    {
        if (wayPoints.Length <= 0)
        {
            playerTotalDistance[playerIndex] = Vector3.Distance(startPositions[playerIndex].position, goalPosition.position);
        }
        else if (wayPoints.Length == 1)
        {
            playerTotalDistance[playerIndex] += Vector3.Distance(startPositions[playerIndex].position, wayPoints[0].transform.position);
            playerTotalDistance[playerIndex] += Vector3.Distance(goalPosition.position, wayPoints[0].transform.position);
        }
        else
        {
            for (var i = 0; i < wayPoints.Length - 1; i++)
            {
                playerTotalDistance[playerIndex] += Vector3.Distance(wayPoints[i].transform.position, wayPoints[i + 1].transform.position);
            }
        }
        stageMiniSliders[playerIndex].minValue = 0;
        stageMiniSliders[playerIndex].maxValue = playerTotalDistance[playerIndex];
        playerRemainingDistances.Add(playerIndex, playerTotalDistance[playerIndex]);
    }

    private void CalculateRemainingDistance(int playerIndex)
    {
        Dictionary<int, float> ToPlayerDistances = new Dictionary<int, float>();
        for (var i = 0; i < wayPoints.Length; i++)
        {
            /*
            Debug.Log("i: " + i + ", Distance: " + Vector3.Distance(wayPoints[i].transform.position, playerRockets[playerIndex].transform.position) + 
                ", Direction: " + (wayPoints[i].transform.position - playerRockets[playerIndex].transform.position).normalized);
            */
            ToPlayerDistances.Add(i, Vector3.Distance(wayPoints[i].transform.position, playerRockets[playerIndex].transform.position));
        }
        int positionIndex = 0;
        float minDistance = Mathf.Infinity;
        foreach (KeyValuePair<int, float> distanceInfo in ToPlayerDistances)
        {
            if (distanceInfo.Value < minDistance)
            {
                positionIndex = distanceInfo.Key;
                minDistance = distanceInfo.Value;
            }
        }
        //Debug.Log("positionIndex: " + positionIndex);

        float totalMovedDistance = 0f;
        if (positionIndex == 0)
        {
            int direction = (playerRockets[playerIndex].transform.position.x >= wayPoints[positionIndex].transform.position.x) ? 1 : -1;
            totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
        }
        else if (positionIndex == wayPoints.Length - 1)
        {
            for (var i = 0; i < positionIndex; i++)
            {
                totalMovedDistance += Vector3.Distance(wayPoints[i].transform.position, wayPoints[i + 1].transform.position);
            }
            totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
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
                if (playerRockets[playerIndex].transform.position.x <= wayPoints[positionIndex].transform.position.x)
                {
                    totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
                }
                else
                {
                    totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
                }
            }
            else if (previousDirection == "Right")
            {
                if (playerRockets[playerIndex].transform.position.x >= wayPoints[positionIndex].transform.position.x)
                {
                    totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
                }
                else
                {
                    totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
                }
            }
            else if (previousDirection == "Up")
            {
                if (playerRockets[playerIndex].transform.position.y <= wayPoints[positionIndex].transform.position.y)
                {
                    totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
                }
                else
                {
                    totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
                }
            }
            else if (previousDirection == "Down")
            {
                if (playerRockets[playerIndex].transform.position.y >= wayPoints[positionIndex].transform.position.y)
                {
                    totalMovedDistance += Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
                }
                else
                {
                    totalMovedDistance -= Vector3.Distance(wayPoints[positionIndex].transform.position, playerRockets[playerIndex].transform.position);
                }
            }
        }
        stageMiniSliders[playerIndex].value = totalMovedDistance;
        playerRemainingDistances[playerIndex] = playerTotalDistance[playerIndex] - totalMovedDistance;
        //Debug.Log("Player Index: " + playerIndex + ", playerRemainingDistances: " + playerRemainingDistances[playerIndex]);
    }

    private void SortPositions()
    {
        playerRemainingDistances = playerRemainingDistances.OrderBy(key => key.Value).ToDictionary(x => x.Key, x => x.Value);

    }

    private void DisplayPosition(int playerIndex)
    {
        switch (playerRemainingDistances.Keys.ToList().IndexOf(playerIndex))
        {
            case 0:
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        playerPositionTexts[playerIndex].text = "1st";
                        playerPositionTexts[playerIndex].font = gameManagerMultiplayENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        playerPositionTexts[playerIndex].text = "1��";
                        playerPositionTexts[playerIndex].font = gameManagerMultiplayJPFont;
                        playerPositionTexts[playerIndex].fontStyle = FontStyle.Bold;
                        break;
                }
                break;
            case 1:
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        playerPositionTexts[playerIndex].text = "2nd";
                        playerPositionTexts[playerIndex].font = gameManagerMultiplayENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        playerPositionTexts[playerIndex].text = "2��";
                        playerPositionTexts[playerIndex].font = gameManagerMultiplayJPFont;
                        playerPositionTexts[playerIndex].fontStyle = FontStyle.Bold;
                        break;
                }
                break;
            case 2:
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        playerPositionTexts[playerIndex].text = "3rd";
                        playerPositionTexts[playerIndex].font = gameManagerMultiplayENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        playerPositionTexts[playerIndex].text = "3��";
                        playerPositionTexts[playerIndex].font = gameManagerMultiplayJPFont;
                        playerPositionTexts[playerIndex].fontStyle = FontStyle.Bold;
                        break;
                }
                break;
            case 3:
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        playerPositionTexts[playerIndex].text = "4th";
                        playerPositionTexts[playerIndex].font = gameManagerMultiplayENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        playerPositionTexts[playerIndex].text = "4��";
                        playerPositionTexts[playerIndex].font = gameManagerMultiplayJPFont;
                        playerPositionTexts[playerIndex].fontStyle = FontStyle.Bold;
                        break;
                }
                break;
        }
    }

    // playerID(1始まり)に対応する追従カメラを返す。範囲外なら null。
    public Camera GetFollowPlayerCamera(int playerID)
    {
        int index = playerID - 1;
        if (followPlayerCameras == null || index < 0 || index >= followPlayerCameras.Length) { return null; }
        return followPlayerCameras[index];
    }

    public RandomItemSettings GetPlayerRandomItemSettings(int playerID)
    {
        //Debug.Log("position1: " + playerRemainingDistances.Keys.ToList().IndexOf(playerID-1));
        int position = playerRemainingDistances.Keys.ToList().IndexOf(playerID) - 1 >= 0 ? playerRemainingDistances.Keys.ToList().IndexOf(playerID) - 1 : 0;
        //Debug.Log("position2: "+position);
        return playerRandomItemSettings[position];
    }
}
