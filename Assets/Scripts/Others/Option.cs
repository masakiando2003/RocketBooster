using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManagerSolo;

public class Option : MonoBehaviour
{
    [SerializeField] Localization option_EN, option_JP;
    [SerializeField] Font optionENFont, optionJPFont;
    [SerializeField] Text optionTitleText, titleButtonText, backgroundLabelText, backgroundText;
    [SerializeField] Text easyButtonText, normalButtonText, hardButtonText;
    [SerializeField] Text bgmVolumeText, seVolumeText, soloPlayRankingDataText;
    [SerializeField] Text bgmVolumeValueText, seVolumeValueText;
    [SerializeField] Text rankingTitleEasyLabelText, rankingTitleNormalLabelText, rankingTitleHardLabelText;
    [SerializeField] Text rankingEasyReturnButtonText, rankingNormalReturnButtonText, rankingHardReturnButtonText;
    [SerializeField] Text rankingEasyResetDataButtonText, rankingNormalResetDataButtonText, rankingHardResetDataButtonText;
    [SerializeField] Text rankingEasyPlayerNameLabelText, rankingEasyFinalPositionLabelText, rankingEasyRemainingTimeLabelText, rankingvEasyTimeElapsedLabelText;
    [SerializeField] Text rankingNormalPlayerNameLabelText, rankingNormalFinalPositionLabelText, rankingNormalRemainingTimeLabelText, rankingNormalTimeElapsedLabelText;
    [SerializeField] Text rankingHardPlayerNameLabelText, rankingHardFinalPositionLabelText, rankingHardRemainingTimeLabelText, rankingHardTimeElapsedLabelText;
    [SerializeField] Text confirmResetDataLabelText, resetDataYesButtonText, resetDataNoButtonText;
    [SerializeField] Slider bgmVolumeSlider, seVolumeSlider;
    [SerializeField] GameObject mainOptionCanvas, soloEasyRankingCanvas, soloNormalRankingCanvas, soloHardRankingCanvas, resetRankingDataCanvas;
    [SerializeField] TextAsset rankingDataEasyFile, rankingDataNormalFile, rankingDataHardFile;
    [SerializeField] TextAsset rankingDefaultDataEasyFile, rankingDefaultDataNormalFile, rankingDefaultDataHardFile;
    [SerializeField] Text[] rankingEasyPlayerNameText, rankingEasyFinalPositionText, rankingEasyRemainingTimeText, rankingEasyTimeElaspedText, rankingEasyPositionLabelTexts;
    [SerializeField] Text[] rankingNormalPlayerNameText, rankingNormalFinalPositionText, rankingNormalRemainingTimeText, rankingNormalTimeElaspedText, rankingNormalPositionLabelTexts;
    [SerializeField] Text[] rankingHardPlayerNameText, rankingHardFinalPositionText, rankingHardRemainingTimeText, rankingHardTimeElaspedText, rankingHardPositionLabelTexts;
    [SerializeField] string[] skyboxNames;


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


    string selectedDifficulty;
    int numPlayers;

    private void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        numPlayers = 5;
        if (Language.gameDisplayLanguage == Language.DisplayLanauge.None)
        {
            Language.gameDisplayLanguage = Language.DisplayLanauge.English;
        }
        switch (Language.gameDisplayLanguage)
        {
            case Language.DisplayLanauge.English:
                optionTitleText.text = option_EN.GetLabelContent("OptionTitleText");
                optionTitleText.font = optionENFont;
                titleButtonText.text = option_EN.GetLabelContent("TitleButtonText");
                titleButtonText.font = optionENFont;
                easyButtonText.text = option_EN.GetLabelContent("EasyButtonText");
                easyButtonText.font = optionENFont;
                normalButtonText.text = option_EN.GetLabelContent("NormalButtonText");
                normalButtonText.font = optionENFont;
                hardButtonText.text = option_EN.GetLabelContent("HardButtonText");
                hardButtonText.font = optionENFont;
                bgmVolumeText.text = option_EN.GetLabelContent("BGMVolumeText");
                bgmVolumeText.font = optionENFont;
                seVolumeText.text = option_EN.GetLabelContent("SEVolumeText");
                seVolumeText.font = optionENFont;
                soloPlayRankingDataText.text = option_EN.GetLabelContent("SoloPlayRankingDataText").Replace("|", System.Environment.NewLine);
                soloPlayRankingDataText.font = optionENFont;
                rankingTitleEasyLabelText.text = option_EN.GetLabelContent("RankingTitleEasyLabelText");
                rankingTitleEasyLabelText.font = optionENFont;
                rankingTitleNormalLabelText.text = option_EN.GetLabelContent("RankingTitleNormalLabelText");
                rankingTitleNormalLabelText.font = optionENFont;
                rankingTitleHardLabelText.text = option_EN.GetLabelContent("RankingTitleHardLabelText");
                rankingTitleHardLabelText.font = optionENFont;
                rankingEasyReturnButtonText.text = option_EN.GetLabelContent("ReturnButtonText");
                rankingEasyReturnButtonText.font = optionENFont;
                rankingNormalReturnButtonText.text = option_EN.GetLabelContent("ReturnButtonText");
                rankingNormalReturnButtonText.font = optionENFont;
                rankingHardReturnButtonText.text = option_EN.GetLabelContent("ReturnButtonText");
                rankingHardReturnButtonText.font = optionENFont;
                rankingEasyPlayerNameLabelText.text = option_EN.GetLabelContent("PlayerNameText");
                rankingEasyPlayerNameLabelText.font = optionENFont;
                rankingEasyFinalPositionLabelText.text = option_EN.GetLabelContent("FinalPositionText");
                rankingEasyFinalPositionLabelText.font = optionENFont;
                rankingEasyRemainingTimeLabelText.text = option_EN.GetLabelContent("RemainingTimeText");
                rankingEasyRemainingTimeLabelText.font = optionENFont;
                rankingvEasyTimeElapsedLabelText.text = option_EN.GetLabelContent("TimeElapsedText");
                rankingvEasyTimeElapsedLabelText.font = optionENFont;
                rankingNormalPlayerNameLabelText.text = option_EN.GetLabelContent("PlayerNameText");
                rankingNormalPlayerNameLabelText.font = optionENFont;
                rankingNormalFinalPositionLabelText.text = option_EN.GetLabelContent("FinalPositionText");
                rankingNormalFinalPositionLabelText.font = optionENFont;
                rankingNormalRemainingTimeLabelText.text = option_EN.GetLabelContent("RemainingTimeText");
                rankingNormalRemainingTimeLabelText.font = optionENFont;
                rankingNormalTimeElapsedLabelText.text = option_EN.GetLabelContent("TimeElapsedText");
                rankingNormalTimeElapsedLabelText.font = optionENFont;
                rankingHardPlayerNameLabelText.text = option_EN.GetLabelContent("PlayerNameText");
                rankingHardPlayerNameLabelText.font = optionENFont;
                rankingHardFinalPositionLabelText.text = option_EN.GetLabelContent("FinalPositionText");
                rankingHardFinalPositionLabelText.font = optionENFont;
                rankingHardRemainingTimeLabelText.text = option_EN.GetLabelContent("RemainingTimeText");
                rankingHardRemainingTimeLabelText.font = optionENFont;
                rankingHardTimeElapsedLabelText.text = option_EN.GetLabelContent("TimeElapsedText");
                rankingHardTimeElapsedLabelText.font = optionENFont;
                rankingEasyResetDataButtonText.text = option_EN.GetLabelContent("ResetDataButtonText");
                rankingEasyResetDataButtonText.font = optionENFont;
                rankingNormalResetDataButtonText.text = option_EN.GetLabelContent("ResetDataButtonText");
                rankingNormalResetDataButtonText.font = optionENFont;
                rankingHardResetDataButtonText.text = option_EN.GetLabelContent("ResetDataButtonText");
                rankingHardResetDataButtonText.font = optionENFont;
                resetDataYesButtonText.text = option_EN.GetLabelContent("YesButtonText");
                resetDataYesButtonText.font = optionENFont;
                resetDataNoButtonText.text = option_EN.GetLabelContent("NoButtonText");
                resetDataNoButtonText.font = optionENFont;
                backgroundLabelText.text = option_EN.GetLabelContent("BackgroundLabelText");
                backgroundLabelText.font = optionENFont;
                backgroundText.font = optionENFont;
                for(int i = 0; i < rankingEasyPositionLabelTexts.Length; i++)
                {
                    rankingEasyPositionLabelTexts[i].font = optionENFont;
                }
                for (int i = 0; i < rankingNormalPositionLabelTexts.Length; i++)
                {
                    rankingNormalPositionLabelTexts[i].font = optionENFont;
                }
                for (int i = 0; i < rankingHardPositionLabelTexts.Length; i++)
                {
                    rankingHardPositionLabelTexts[i].font = optionENFont;
                }
                bgmVolumeValueText.font = optionENFont;
                seVolumeValueText.font = optionENFont;
                break;
            case Language.DisplayLanauge.Japanese:
                optionTitleText.text = option_JP.GetLabelContent("OptionTitleText");
                optionTitleText.font = optionJPFont;
                optionTitleText.fontStyle = FontStyle.Bold;
                titleButtonText.text = option_JP.GetLabelContent("TitleButtonText");
                titleButtonText.font = optionJPFont;
                titleButtonText.fontStyle = FontStyle.Bold;
                easyButtonText.text = option_JP.GetLabelContent("EasyButtonText");
                easyButtonText.font = optionJPFont;
                easyButtonText.fontStyle = FontStyle.Bold;
                normalButtonText.text = option_JP.GetLabelContent("NormalButtonText");
                normalButtonText.font = optionJPFont;
                normalButtonText.fontStyle = FontStyle.Bold;
                hardButtonText.text = option_JP.GetLabelContent("HardButtonText");
                hardButtonText.font = optionJPFont;
                hardButtonText.fontStyle = FontStyle.Bold;
                bgmVolumeText.text = option_JP.GetLabelContent("BGMVolumeText");
                bgmVolumeText.font = optionJPFont;
                bgmVolumeText.fontStyle = FontStyle.Bold;
                seVolumeText.text = option_JP.GetLabelContent("SEVolumeText");
                seVolumeText.font = optionJPFont;
                seVolumeText.fontStyle = FontStyle.Bold;
                soloPlayRankingDataText.text = option_JP.GetLabelContent("SoloPlayRankingDataText").Replace("|", System.Environment.NewLine);
                soloPlayRankingDataText.font = optionJPFont;
                soloPlayRankingDataText.fontStyle = FontStyle.Bold;
                rankingTitleEasyLabelText.text = option_JP.GetLabelContent("RankingTitleEasyLabelText");
                rankingTitleEasyLabelText.font = optionJPFont;
                rankingTitleEasyLabelText.fontStyle = FontStyle.Bold;
                rankingTitleNormalLabelText.text = option_JP.GetLabelContent("RankingTitleNormalLabelText");
                rankingTitleNormalLabelText.font = optionJPFont;
                rankingTitleNormalLabelText.fontStyle = FontStyle.Bold;
                rankingTitleHardLabelText.text = option_JP.GetLabelContent("RankingTitleHardLabelText");
                rankingTitleHardLabelText.font = optionJPFont;
                rankingTitleHardLabelText.fontStyle = FontStyle.Bold;
                rankingEasyReturnButtonText.text = option_JP.GetLabelContent("ReturnButtonText");
                rankingEasyReturnButtonText.font = optionJPFont;
                rankingEasyReturnButtonText.fontStyle = FontStyle.Bold;
                rankingNormalReturnButtonText.text = option_JP.GetLabelContent("ReturnButtonText");
                rankingNormalReturnButtonText.font = optionJPFont;
                rankingNormalReturnButtonText.fontStyle = FontStyle.Bold;
                rankingHardReturnButtonText.text = option_JP.GetLabelContent("ReturnButtonText");
                rankingHardReturnButtonText.font = optionJPFont;
                rankingHardReturnButtonText.fontStyle = FontStyle.Bold;
                rankingEasyPlayerNameLabelText.text = option_JP.GetLabelContent("PlayerNameText");
                rankingEasyPlayerNameLabelText.font = optionJPFont;
                rankingEasyPlayerNameLabelText.fontStyle = FontStyle.Bold;
                rankingEasyFinalPositionLabelText.text = option_JP.GetLabelContent("FinalPositionText");
                rankingEasyFinalPositionLabelText.font = optionJPFont;
                rankingEasyFinalPositionLabelText.fontStyle = FontStyle.Bold;
                rankingEasyRemainingTimeLabelText.text = option_JP.GetLabelContent("RemainingTimeText");
                rankingEasyRemainingTimeLabelText.font = optionJPFont;
                rankingEasyRemainingTimeLabelText.fontStyle = FontStyle.Bold;
                rankingvEasyTimeElapsedLabelText.text = option_JP.GetLabelContent("TimeElapsedText");
                rankingvEasyTimeElapsedLabelText.font = optionJPFont;
                rankingvEasyTimeElapsedLabelText.fontStyle = FontStyle.Bold;
                rankingNormalPlayerNameLabelText.text = option_JP.GetLabelContent("PlayerNameText");
                rankingNormalPlayerNameLabelText.font = optionJPFont;
                rankingNormalPlayerNameLabelText.fontStyle = FontStyle.Bold;
                rankingNormalFinalPositionLabelText.text = option_JP.GetLabelContent("FinalPositionText");
                rankingNormalFinalPositionLabelText.font = optionJPFont;
                rankingNormalFinalPositionLabelText.fontStyle = FontStyle.Bold;
                rankingNormalRemainingTimeLabelText.text = option_JP.GetLabelContent("RemainingTimeText");
                rankingNormalRemainingTimeLabelText.font = optionJPFont;
                rankingNormalRemainingTimeLabelText.fontStyle = FontStyle.Bold;
                rankingNormalTimeElapsedLabelText.text = option_JP.GetLabelContent("TimeElapsedText");
                rankingNormalTimeElapsedLabelText.font = optionJPFont;
                rankingNormalTimeElapsedLabelText.fontStyle = FontStyle.Bold;
                rankingHardPlayerNameLabelText.text = option_JP.GetLabelContent("PlayerNameText");
                rankingHardPlayerNameLabelText.font = optionJPFont;
                rankingHardPlayerNameLabelText.fontStyle = FontStyle.Bold;
                rankingHardFinalPositionLabelText.text = option_JP.GetLabelContent("FinalPositionText");
                rankingHardFinalPositionLabelText.font = optionJPFont;
                rankingHardFinalPositionLabelText.fontStyle = FontStyle.Bold;
                rankingHardRemainingTimeLabelText.text = option_JP.GetLabelContent("RemainingTimeText");
                rankingHardRemainingTimeLabelText.font = optionJPFont;
                rankingHardRemainingTimeLabelText.fontStyle = FontStyle.Bold;
                rankingHardTimeElapsedLabelText.text = option_JP.GetLabelContent("TimeElapsedText");
                rankingHardTimeElapsedLabelText.font = optionJPFont;
                rankingHardTimeElapsedLabelText.fontStyle = FontStyle.Bold;
                rankingEasyResetDataButtonText.text = option_JP.GetLabelContent("ResetDataButtonText");
                rankingEasyResetDataButtonText.font = optionJPFont;
                rankingEasyResetDataButtonText.fontStyle = FontStyle.Bold;
                rankingNormalResetDataButtonText.text = option_JP.GetLabelContent("ResetDataButtonText");
                rankingNormalResetDataButtonText.font = optionJPFont;
                rankingNormalResetDataButtonText.fontStyle = FontStyle.Bold;
                rankingHardResetDataButtonText.text = option_JP.GetLabelContent("ResetDataButtonText");
                rankingHardResetDataButtonText.font = optionJPFont;
                rankingHardResetDataButtonText.fontStyle = FontStyle.Bold;
                resetDataYesButtonText.text = option_JP.GetLabelContent("YesButtonText");
                resetDataYesButtonText.font = optionJPFont;
                resetDataYesButtonText.fontStyle = FontStyle.Bold;
                resetDataNoButtonText.text = option_JP.GetLabelContent("NoButtonText");
                resetDataNoButtonText.font = optionJPFont;
                resetDataNoButtonText.fontStyle = FontStyle.Bold;
                backgroundLabelText.text = option_JP.GetLabelContent("BackgroundLabelText");
                backgroundLabelText.font = optionJPFont;
                backgroundLabelText.fontStyle = FontStyle.Bold;
                backgroundText.font = optionJPFont;
                backgroundText.fontStyle = FontStyle.Bold;
                for (int i = 0; i < rankingEasyPositionLabelTexts.Length; i++)
                {
                    rankingEasyPositionLabelTexts[i].font = optionJPFont;
                    rankingEasyPositionLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                for (int i = 0; i < rankingNormalPositionLabelTexts.Length; i++)
                {
                    rankingNormalPositionLabelTexts[i].font = optionJPFont;
                    rankingNormalPositionLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                for (int i = 0; i < rankingHardPositionLabelTexts.Length; i++)
                {
                    rankingHardPositionLabelTexts[i].font = optionJPFont;
                    rankingHardPositionLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                bgmVolumeValueText.font = optionJPFont;
                bgmVolumeValueText.fontStyle = FontStyle.Bold;
                seVolumeValueText.font = optionJPFont;
                seVolumeValueText.fontStyle = FontStyle.Bold;
                break;
        }
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            int bgmVolume = PlayerPrefs.GetInt("BGMVolume");
            bgmVolumeSlider.value = bgmVolume;

        }
        if (PlayerPrefs.HasKey("SEVolume"))
        {
            int seVolume = PlayerPrefs.GetInt("SEVolume");
            seVolumeSlider.value = seVolume;

        }
        ReadRankingData("Easy");
        ReadRankingData("Normal");
        ReadRankingData("Hard");
        mainOptionCanvas.SetActive(true);
        soloEasyRankingCanvas.SetActive(false);
        soloNormalRankingCanvas.SetActive(false);
        soloHardRankingCanvas.SetActive(false);
        resetRankingDataCanvas.SetActive(false);
        selectedDifficulty = "Easy";
        int skyboxIndex = 0;
        if (PlayerPrefs.HasKey("BackgroundIndex"))
        {
            skyboxIndex = PlayerPrefs.GetInt("BackgroundIndex");
        }
        if (skyboxIndex >= 0 && skyboxIndex <= skyboxNames.Length - 1)
        {
            backgroundText.text = skyboxNames[skyboxIndex];
        }
    }

    public void ToTitle(string titleMap)
    {
        SceneManager.LoadScene(titleMap);
    }

    public void ChangeBGMVolume()
    {
        PlayerPrefs.SetInt("BGMVolume", (int)bgmVolumeSlider.value);
        bgmVolumeValueText.text = bgmVolumeSlider.value.ToString();
        BGMController.SetBGMVolume((int)bgmVolumeSlider.value);
    }
    public void ChangeSEVolume()
    {
        PlayerPrefs.SetInt("SEVolume", (int)seVolumeSlider.value);
        seVolumeValueText.text = seVolumeSlider.value.ToString();
        BGMController.SetSEVolume((int)seVolumeSlider.value);
    }

    public void Show1PlayerRankingData(string difficulty)
    {
        mainOptionCanvas.SetActive(false);
        switch (difficulty)
        {
            case "Easy":
                soloNormalRankingCanvas.SetActive(false);
                soloHardRankingCanvas.SetActive(false);
                soloEasyRankingCanvas.SetActive(true);
                break;
            case "Normal":
                soloEasyRankingCanvas.SetActive(false);
                soloHardRankingCanvas.SetActive(false);
                soloNormalRankingCanvas.SetActive(true);
                break;
            case "Hard":
                soloEasyRankingCanvas.SetActive(false);
                soloNormalRankingCanvas.SetActive(false);
                soloHardRankingCanvas.SetActive(true);
                break;
        }
    }

    private void ReadRankingData(string difficulty)
    {
        int index = 0;
        TextAsset rankingDataFile = null;
        switch (difficulty)
        {
            case "Easy":
                rankingDataFile = rankingDataEasyFile;
                break;
            case "Normal":
                rankingDataFile = rankingDataNormalFile;
                break;
            case "Hard":
                rankingDataFile = rankingDataHardFile;
                break;
        }
        if (rankingDataFile == null) { return; }
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
            switch (difficulty)
            {
                case "Easy":
                    if (PlayerPrefs.HasKey("Easy_PlayerName_" + (index + 1)))
                    {
                        rankingEasyPlayerNameText[index].text = PlayerPrefs.GetString("Easy_PlayerName_" + (index + 1));
                        rankingEasyPlayerNameText[index].font = optionENFont;
                    }
                    else
                    {
                        rankingEasyPlayerNameText[index].text = data.playerName;
                        PlayerPrefs.SetString("Easy_PlayerName_" + (index + 1), data.playerName);
                    }
                    switch (Language.gameDisplayLanguage)
                    {
                        case Language.DisplayLanauge.English:
                            rankingEasyFinalPositionText[index].text = data.finalPosition.Replace("Goal", option_EN.GetLabelContent("GoalText"))
                                                                                         .Replace("Check Point", option_EN.GetLabelContent("CheckPointText"))
                                                                                         .Replace("Start", option_EN.GetLabelContent("StartText"));
                            rankingEasyFinalPositionText[index].font = optionENFont;
                            rankingEasyRemainingTimeText[index].font = optionENFont;
                            rankingEasyTimeElaspedText[index].font = optionENFont;
                            break;
                        case Language.DisplayLanauge.Japanese:
                            rankingEasyFinalPositionText[index].text = data.finalPosition.Replace("Goal", option_JP.GetLabelContent("GoalText"))
                                                                                         .Replace("Check Point", option_JP.GetLabelContent("CheckPointText"))
                                                                                         .Replace("Start", option_JP.GetLabelContent("StartText"));
                            rankingEasyPlayerNameText[index].font = optionJPFont;
                            rankingEasyPlayerNameText[index].fontStyle = FontStyle.Bold;
                            rankingEasyFinalPositionText[index].font = optionJPFont;
                            rankingEasyFinalPositionText[index].fontStyle = FontStyle.Bold;
                            rankingEasyRemainingTimeText[index].font = optionJPFont;
                            rankingEasyRemainingTimeText[index].fontStyle = FontStyle.Bold;
                            rankingEasyTimeElaspedText[index].font = optionJPFont;
                            rankingEasyTimeElaspedText[index].fontStyle = FontStyle.Bold;
                            break;
                    }
                    rankingEasyRemainingTimeText[index].text = data.remainingTime.ToString();
                    rankingEasyTimeElaspedText[index].text = data.timeElapsed;
                    break;
                case "Normal":
                    if (PlayerPrefs.HasKey("Normal_PlayerName_" + (index + 1)))
                    {
                        rankingNormalPlayerNameText[index].text = PlayerPrefs.GetString("Normal_PlayerName_" + (index + 1));
                        rankingNormalPlayerNameText[index].font = optionENFont;
                    }
                    else
                    {
                        rankingNormalPlayerNameText[index].text = data.playerName;
                        PlayerPrefs.SetString("Normal_PlayerName_" + (index + 1), data.playerName);
                    }
                    switch (Language.gameDisplayLanguage)
                    {
                        case Language.DisplayLanauge.English:
                            rankingNormalFinalPositionText[index].text = data.finalPosition.Replace("Goal", option_EN.GetLabelContent("GoalText"))
                                                                                         .Replace("Check Point", option_EN.GetLabelContent("CheckPointText"))
                                                                                         .Replace("Start", option_EN.GetLabelContent("StartText"));
                            rankingNormalFinalPositionText[index].font = optionENFont;
                            rankingNormalRemainingTimeText[index].font = optionENFont;
                            rankingNormalTimeElaspedText[index].font = optionENFont;
                            break;
                        case Language.DisplayLanauge.Japanese:
                            rankingNormalFinalPositionText[index].text = data.finalPosition.Replace("Goal", option_JP.GetLabelContent("GoalText"))
                                                                                         .Replace("Check Point", option_JP.GetLabelContent("CheckPointText"))
                                                                                         .Replace("Start", option_JP.GetLabelContent("StartText"));
                            rankingNormalPlayerNameText[index].font = optionJPFont;
                            rankingNormalPlayerNameText[index].fontStyle = FontStyle.Bold;
                            rankingNormalFinalPositionText[index].font = optionJPFont;
                            rankingNormalFinalPositionText[index].fontStyle = FontStyle.Bold;
                            rankingNormalRemainingTimeText[index].font = optionJPFont;
                            rankingNormalRemainingTimeText[index].fontStyle = FontStyle.Bold;
                            rankingNormalTimeElaspedText[index].font = optionJPFont;
                            rankingNormalTimeElaspedText[index].fontStyle = FontStyle.Bold;
                            break;
                    }
                    rankingNormalRemainingTimeText[index].text = data.remainingTime.ToString();
                    rankingNormalTimeElaspedText[index].text = data.timeElapsed;
                    break;
                case "Hard":

                    if (PlayerPrefs.HasKey("Hard_PlayerName_" + (index + 1)))
                    {
                        rankingHardPlayerNameText[index].text = PlayerPrefs.GetString("Hard_PlayerName_" + (index + 1));
                        rankingHardPlayerNameText[index].font = optionENFont;
                    }
                    else
                    {
                        rankingHardPlayerNameText[index].text = data.playerName;
                        PlayerPrefs.SetString("Hard_PlayerName_" + (index + 1), data.playerName);
                    }
                    switch (Language.gameDisplayLanguage)
                    {
                        case Language.DisplayLanauge.English:
                            rankingHardFinalPositionText[index].text = data.finalPosition.Replace("Goal", option_EN.GetLabelContent("GoalText"))
                                                                                         .Replace("Check Point", option_EN.GetLabelContent("CheckPointText"))
                                                                                         .Replace("Start", option_EN.GetLabelContent("StartText"));
                            rankingHardFinalPositionText[index].font = optionENFont;
                            rankingHardRemainingTimeText[index].font = optionENFont;
                            rankingHardTimeElaspedText[index].font = optionENFont;
                            break;
                        case Language.DisplayLanauge.Japanese:
                            rankingHardFinalPositionText[index].text = data.finalPosition.Replace("Goal", option_JP.GetLabelContent("GoalText"))
                                                                                         .Replace("Check Point", option_JP.GetLabelContent("CheckPointText"))
                                                                                         .Replace("Start", option_JP.GetLabelContent("StartText"));
                            rankingHardPlayerNameText[index].font = optionJPFont;
                            rankingHardPlayerNameText[index].fontStyle = FontStyle.Bold;
                            rankingHardFinalPositionText[index].font = optionJPFont;
                            rankingHardFinalPositionText[index].fontStyle = FontStyle.Bold;
                            rankingHardRemainingTimeText[index].font = optionJPFont;
                            rankingHardRemainingTimeText[index].fontStyle = FontStyle.Bold;
                            rankingHardTimeElaspedText[index].font = optionJPFont;
                            rankingHardTimeElaspedText[index].fontStyle = FontStyle.Bold;
                            break;
                    }
                    rankingHardRemainingTimeText[index].text = data.remainingTime.ToString();
                    rankingHardTimeElaspedText[index].text = data.timeElapsed;
                    break;
            }
            index++;
        }
    }

    public void ReturnToMainOption()
    {
        resetRankingDataCanvas.SetActive(false);
        soloEasyRankingCanvas.SetActive(false);
        soloNormalRankingCanvas.SetActive(false);
        soloHardRankingCanvas.SetActive(false);
        mainOptionCanvas.SetActive(true);
    }

    public void ConfirmResetData(string difficulty)
    {
        selectedDifficulty = difficulty;
        mainOptionCanvas.SetActive(false);
        soloEasyRankingCanvas.SetActive(false);
        soloNormalRankingCanvas.SetActive(false);
        soloHardRankingCanvas.SetActive(false);
        resetRankingDataCanvas.SetActive(true);
        switch (difficulty)
        {
            case "Easy":
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        confirmResetDataLabelText.text = option_EN.GetLabelContent("ConfirmResetDataText") + "\"" + option_EN.GetLabelContent("EasyButtonText") + "\"?";
                        confirmResetDataLabelText.font = optionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        confirmResetDataLabelText.text = "\"" + option_JP.GetLabelContent("EasyButtonText") + "\"" + option_JP.GetLabelContent("ConfirmResetDataText") +  "?";
                        confirmResetDataLabelText.font = optionJPFont;
                        break;
                }
                break;
            case "Normal":
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        confirmResetDataLabelText.text = option_EN.GetLabelContent("ConfirmResetDataText") + "\"" + option_EN.GetLabelContent("NormalButtonText") + "\"?";
                        confirmResetDataLabelText.font = optionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        confirmResetDataLabelText.text = "\"" + option_JP.GetLabelContent("NormalButtonText") + "\"" + option_JP.GetLabelContent("ConfirmResetDataText") + "?";
                        confirmResetDataLabelText.font = optionJPFont;
                        break;
                }
                break;
            case "Hard":
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        confirmResetDataLabelText.text = option_EN.GetLabelContent("ConfirmResetDataText") + "\"" + option_EN.GetLabelContent("HardButtonText") + "\"?";
                        confirmResetDataLabelText.font = optionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        confirmResetDataLabelText.text = "\"" + option_JP.GetLabelContent("HardButtonText") + "\"" + option_JP.GetLabelContent("ConfirmResetDataText") + "?";
                        confirmResetDataLabelText.font = optionJPFont;
                        break;
                }
                break;
        }

    }
    public void CancelResetData()
    {
        resetRankingDataCanvas.SetActive(false);
        mainOptionCanvas.SetActive(false);
        switch (selectedDifficulty)
        {
            case "Easy":
                soloNormalRankingCanvas.SetActive(false);
                soloHardRankingCanvas.SetActive(false);
                soloEasyRankingCanvas.SetActive(true);
                break;
            case "Normal":
                soloEasyRankingCanvas.SetActive(false);
                soloHardRankingCanvas.SetActive(false);
                soloNormalRankingCanvas.SetActive(true);
                break;
            case "Hard":
                soloEasyRankingCanvas.SetActive(false);
                soloNormalRankingCanvas.SetActive(false);
                soloHardRankingCanvas.SetActive(true);
                break;
        }
    }

    public void ResetData()
    {
        TextAsset rankingDataFile = null;
        switch (selectedDifficulty)
        {
            case "Easy":
                rankingDataFile = rankingDefaultDataEasyFile;
                break;
            case "Normal":
                rankingDataFile = rankingDefaultDataNormalFile;
                break;
            case "Hard":
                rankingDataFile = rankingDefaultDataHardFile;
                break;
        }
        if (rankingDataFile == null) { return; }
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
        string saveRankingData = JsonUtility.ToJson(rankingJson, true);
        //Debug.Log(saveRankingData);
        for (int i = 1; i <= numPlayers; i++)
        {
            string playerNameKey = Difficulty_1P_TempSave.chosenDifficulty + "_PlayerName_" + i.ToString();
            PlayerPrefs.SetString(playerNameKey, rankingJson.detail[i - 1].playerName);
            string playerFinalPositionKey = Difficulty_1P_TempSave.chosenDifficulty + "_FinalPosition_" + i.ToString();
            PlayerPrefs.SetString(playerFinalPositionKey, rankingJson.detail[i - 1].finalPosition);
            string playerCheckPointIndexKey = Difficulty_1P_TempSave.chosenDifficulty + "_CheckPointIndex_" + i.ToString();
            PlayerPrefs.SetInt(playerCheckPointIndexKey, rankingJson.detail[i - 1].checkPointIndex);
            string playerRemainingTimeKey = Difficulty_1P_TempSave.chosenDifficulty + "_RemainingTime_" + i.ToString();
            PlayerPrefs.SetInt(playerRemainingTimeKey, rankingJson.detail[i - 1].remainingTime);
            string playerTimeElapsedKey = Difficulty_1P_TempSave.chosenDifficulty + "_TimeElapsed_" + i.ToString();
            PlayerPrefs.SetString(playerTimeElapsedKey, rankingJson.detail[i - 1].timeElapsed);
        }
        /*
        File.WriteAllText(Application.dataPath + "/Resources/ranking_1P_" + selectedDifficulty + ".json", saveRankingData);
        #if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        #endif
        */
        resetRankingDataCanvas.SetActive(false);
        switch (selectedDifficulty)
        {
            case "Easy":
                ReadRankingData("Easy");
                soloNormalRankingCanvas.SetActive(false);
                soloHardRankingCanvas.SetActive(false);
                soloEasyRankingCanvas.SetActive(true);
                break;
            case "Normal":
                ReadRankingData("Normal");
                soloEasyRankingCanvas.SetActive(false);
                soloHardRankingCanvas.SetActive(false);
                soloNormalRankingCanvas.SetActive(true);
                break;
            case "Hard":
                ReadRankingData("Hard");
                soloEasyRankingCanvas.SetActive(false);
                soloNormalRankingCanvas.SetActive(false);
                soloHardRankingCanvas.SetActive(true);
                break;
        }
    }

    public void ChangeSkybox(string direction)
    {
        int skyboxIndex = 0;
        if (PlayerPrefs.HasKey("BackgroundIndex"))
        {
            skyboxIndex = PlayerPrefs.GetInt("BackgroundIndex");
        }
        if (skyboxIndex < 0 || skyboxIndex > skyboxNames.Length - 1)
        {
            skyboxIndex = 0;
        }
        if(direction.Equals("Left"))
        {
            skyboxIndex = (skyboxIndex - 1) < 0 ? skyboxNames.Length - 1 : skyboxIndex - 1;
        }
        else if (direction.Equals("Right"))
        {
            skyboxIndex = (skyboxIndex + 1) > (skyboxNames.Length - 1) ? 0 : skyboxIndex + 1;
        }
        //Debug.Log("skyboxIndex: "+skyboxIndex);
        PlayerPrefs.SetInt("BackgroundIndex", skyboxIndex);
        if (skyboxIndex >= 0 && skyboxIndex <= skyboxNames.Length - 1)
        {
            backgroundText.text = skyboxNames[skyboxIndex];
        }
    }
}
