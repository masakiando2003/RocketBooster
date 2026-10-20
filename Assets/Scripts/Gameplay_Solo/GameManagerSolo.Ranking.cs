using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GameManagerSolo の partial 分割: ランキング関連
public partial class GameManagerSolo : MonoBehaviour
{
    private void ReadRankingData()
    {
        if (rankingDataFile == null) { return; }
        int index = 0;
        RankingJson rankingJson;
        try { rankingJson = JsonUtility.FromJson<RankingJson>(rankingDataFile.text); }
        catch (System.Exception e)
        {
#if UNITY_EDITOR
            Debug.LogWarning("[Ranking] JSON parse failed in " + rankingDataFile.name + ": " + e.Message);
#endif
            return;
        }
        if (rankingJson == null || rankingJson.detail == null) { return; }
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
        RankingJson rankingJson;
        try { rankingJson = JsonUtility.FromJson<RankingJson>(rankingDataFile.text); }
        catch (System.Exception e)
        {
#if UNITY_EDITOR
            Debug.LogWarning("[Ranking] JSON parse failed in " + rankingDataFile.name + ": " + e.Message);
#endif
            return;
        }
        if (rankingJson == null || rankingJson.detail == null) { return; }
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
}
