using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManagerSolo;

public class Instructions_1P : MonoBehaviour
{
    [SerializeField] Localization instruction1P_EN, instruction1P_JP;
    [SerializeField] GameObject instructionCanvas;
    [SerializeField] GameObject rankingCanvas;
    [SerializeField] GameObject pleaseWaitCanvas;
    [SerializeField] GameObject objectionSection, controlsSection, hintsSection;
    [SerializeField] GameObject keyboardSection, joystickSection;
    [SerializeField] GameObject hints1Section, hints2Section, hints3Section, hints4Section, hints5Section;
    [SerializeField] GameObject hint1ImageEN, hint2Image1EN, hint2Image2EN, hint3ImageEN, hint4ImageEN, hint5Image1EN, hint5Image2EN;
    [SerializeField] GameObject hint1ImageJP, hint2Image1JP, hint2Image2JP, hint3ImageJP, hint4ImageJP, hint5Image1JP, hint5Image2JP;
    [SerializeField] GameObject stageEasyImageEN, stageNormalImageEN, stageHardImageEN;
    [SerializeField] GameObject stageEasyImageJP, stageNormalImageJP, stageHardImageJP;
    [SerializeField] GameObject keyboardControlType1ImageEN, keyboardControlType1ImageJP, joystickImageEN, joystickImageJP;
    [SerializeField] GameObject keyboardControlType2mageEN, keyboardControlType2ImageJP;
    [SerializeField] GameObject keyboardControlType1EN, keyboardControlType1JP, keyboardControlType2EN, keyboardControlType2JP;
    [SerializeField] GameObject joystickControlEN, joystickControlJP;
    [SerializeField] string targetEasyMap, targetNormalMap, targetHardMap;
    [SerializeField] string titleMap;
    [SerializeField] Slider loadingSlider;
    [SerializeField] Font instructionENFont, instructionJPFont;
    [SerializeField] Text instructionTitleText, missionButtonText, controlsButtonText, hintsButtonText;
    [SerializeField] Text missionText, keyboardButtonText, joystickButtonText;
    [SerializeField] Text hint1ButtonText, hint2ButtonText, hint3ButtonText, hint4ButtonText, hint5ButtonText;
    [SerializeField] Text hint1Text, hint2Text, hint3Text, hint4Text, hint5Text;
    [SerializeField] Text startButtonText, rankingButtonText, titleButtonText, returnButtonText;
    [SerializeField] Text rankingLabelText, rankingPlayerNameLabelText, rankingPlayerFinalPositionText;
    [SerializeField] Text rankingPlayerRemainingTimeText, rankingPlayerTimeElapsedText, pleaseWaitLabelText;
    [SerializeField] Text[] rankingPlayerPositionLabelTexts;
    [SerializeField] Text[] rankingPlayerNameText;
    [SerializeField] Text[] rankingFinalPositionText;
    [SerializeField] Text[] rankingRemainingTimeText;
    [SerializeField] Text[] rankingTimeElaspedText;
    [SerializeField] TextAsset rankingDataEasyFile, rankingDataNormalFile, rankingDataHardFile;
    [SerializeField] Button btnMission, btnControl, btnHints, btnKeyboard, btnJoystick;
    [SerializeField] Button btnHints1, btnHints2, btnHints3, btnHints4, btnHints5;
    [SerializeField] Color buttonNormalColor, buttonSelectedColor;

    TextAsset rankingDataFile;
    bool view1Flag;

    private void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        if (Language.gameDisplayLanguage == Language.DisplayLanauge.None)
        {
            Language.gameDisplayLanguage = Language.DisplayLanauge.Japanese;
        }
        view1Flag = true;
        instructionCanvas.SetActive(true);
        rankingCanvas.SetActive(false);
        pleaseWaitCanvas.SetActive(false);
        objectionSection.SetActive(true);
        controlsSection.SetActive(false);
        keyboardSection.SetActive(false);
        joystickSection.SetActive(false);
        hints1Section.SetActive(true);
        hints2Section.SetActive(false);
        hints3Section.SetActive(false);
        hints4Section.SetActive(false);
        hints5Section.SetActive(false);
        hintsSection.SetActive(false);
        btnMission.GetComponent<Image>().color = buttonSelectedColor;
        btnControl.GetComponent<Image>().color = buttonNormalColor;
        btnHints.GetComponent<Image>().color = buttonNormalColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
        switch (Difficulty_1P_TempSave.chosenDifficulty)
        {
            case "Easy":
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        stageEasyImageEN.SetActive(true);
                        stageEasyImageJP.SetActive(false);
                        break;
                    case Language.DisplayLanauge.Japanese:
                        stageEasyImageJP.SetActive(true);
                        stageEasyImageEN.SetActive(false);
                        break;
                }
                stageNormalImageEN.SetActive(false);
                stageNormalImageJP.SetActive(false);
                stageHardImageEN.SetActive(false);
                stageHardImageJP.SetActive(false);
                rankingDataFile = rankingDataEasyFile;
                break;
            case "Normal":
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        stageNormalImageEN.SetActive(true);
                        stageNormalImageJP.SetActive(false);
                        break;
                    case Language.DisplayLanauge.Japanese:
                        stageNormalImageJP.SetActive(true);
                        stageNormalImageEN.SetActive(false);
                        break;
                }
                stageEasyImageEN.SetActive(false);
                stageEasyImageJP.SetActive(false);
                stageHardImageEN.SetActive(false);
                stageHardImageJP.SetActive(false);
                rankingDataFile = rankingDataNormalFile;
                break;
            case "Hard":
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        stageHardImageEN.SetActive(true);
                        stageHardImageJP.SetActive(false);
                        break;
                    case Language.DisplayLanauge.Japanese:
                        stageHardImageJP.SetActive(true);
                        stageHardImageEN.SetActive(false);
                        break;
                }
                stageEasyImageEN.SetActive(false);
                stageEasyImageJP.SetActive(false);
                stageNormalImageEN.SetActive(false);
                stageNormalImageJP.SetActive(false);
                rankingDataFile = rankingDataHardFile;
                break;
        }
        ReadRankingData(rankingDataFile);
        switch (Language.gameDisplayLanguage)
        {
            case Language.DisplayLanauge.English:
                instructionTitleText.text = instruction1P_EN.GetLabelContent("InstructionTitleText");
                instructionTitleText.font = instructionENFont;
                missionButtonText.text = instruction1P_EN.GetLabelContent("MissionButtonText");
                missionButtonText.font = instructionENFont;
                controlsButtonText.text = instruction1P_EN.GetLabelContent("ControlsButtonText");
                controlsButtonText.font = instructionENFont;
                hintsButtonText.text = instruction1P_EN.GetLabelContent("HintsButtonText");
                hintsButtonText.font = instructionENFont;
                missionText.text = instruction1P_EN.GetLabelContent("MissionText").Replace("|", Environment.NewLine);
                missionText.font = instructionENFont;
                keyboardButtonText.text = instruction1P_EN.GetLabelContent("KeyboardButtonText");
                keyboardButtonText.font = instructionENFont;
                joystickButtonText.text = instruction1P_EN.GetLabelContent("JoystickButtonText");
                joystickButtonText.font = instructionENFont;
                hint1ButtonText.text = instruction1P_EN.GetLabelContent("Hint1ButtonText");
                hint1ButtonText.font = instructionENFont;
                hint2ButtonText.text = instruction1P_EN.GetLabelContent("Hint2ButtonText");
                hint2ButtonText.font = instructionENFont;
                hint3ButtonText.text = instruction1P_EN.GetLabelContent("Hint3ButtonText");
                hint3ButtonText.font = instructionENFont;
                hint4ButtonText.text = instruction1P_EN.GetLabelContent("Hint4ButtonText");
                hint4ButtonText.font = instructionENFont;
                hint5ButtonText.text = instruction1P_EN.GetLabelContent("Hint5ButtonText");
                hint5ButtonText.font = instructionENFont;
                hint1Text.text = instruction1P_EN.GetLabelContent("Hint1Text").Replace("|", Environment.NewLine);
                hint1Text.font = instructionENFont;
                hint2Text.text = instruction1P_EN.GetLabelContent("Hint2Text").Replace("|", Environment.NewLine);
                hint2Text.font = instructionENFont;
                hint3Text.text = instruction1P_EN.GetLabelContent("Hint3Text").Replace("|", Environment.NewLine);
                hint3Text.font = instructionENFont;
                hint4Text.text = instruction1P_EN.GetLabelContent("Hint4Text").Replace("|", Environment.NewLine);
                hint4Text.font = instructionENFont;
                hint5Text.text = instruction1P_EN.GetLabelContent("Hint5Text").Replace("|", Environment.NewLine);
                hint5Text.font = instructionENFont;
                hint1ImageEN.SetActive(true);
                hint2Image1EN.SetActive(true);
                hint2Image2EN.SetActive(true);
                hint3ImageEN.SetActive(true);
                hint4ImageEN.SetActive(true);
                hint5Image1EN.SetActive(true);
                hint5Image2EN.SetActive(true);
                hint1ImageJP.SetActive(false);
                hint2Image1JP.SetActive(false);
                hint2Image2JP.SetActive(false);
                hint3ImageJP.SetActive(false);
                hint4ImageJP.SetActive(false);
                hint5Image1JP.SetActive(false);
                hint5Image2JP.SetActive(false);
                switch (ControlType.chosenControlType){
                    case "Type1":
                        keyboardControlType1EN.SetActive(true);
                        keyboardControlType1JP.SetActive(false);
                        keyboardControlType2EN.SetActive(false);
                        keyboardControlType2JP.SetActive(false);

                        break;
                    case "Type2":
                        keyboardControlType1EN.SetActive(false);
                        keyboardControlType1JP.SetActive(false);
                        keyboardControlType2EN.SetActive(true);
                        keyboardControlType2JP.SetActive(false);
                        break;
                }
                joystickControlEN.SetActive(true);
                joystickControlJP.SetActive(false);
                startButtonText.text = instruction1P_EN.GetLabelContent("StartButtonText");
                startButtonText.font = instructionENFont;
                rankingButtonText.text = instruction1P_EN.GetLabelContent("RankingButtonText");
                rankingButtonText.font = instructionENFont;
                titleButtonText.text = instruction1P_EN.GetLabelContent("TitleButtonText");
                titleButtonText.font = instructionENFont;
                returnButtonText.text = instruction1P_EN.GetLabelContent("ReturnButtonText");
                returnButtonText.font = instructionENFont;
                rankingLabelText.text = instruction1P_EN.GetLabelContent("RankingLabelText");
                rankingLabelText.font = instructionENFont;
                rankingPlayerNameLabelText.text = instruction1P_EN.GetLabelContent("RankingPlayerNameLabelText");
                rankingPlayerNameLabelText.font = instructionENFont;
                rankingPlayerFinalPositionText.text = instruction1P_EN.GetLabelContent("RankingPlayerFinalPositionText");
                rankingPlayerFinalPositionText.font = instructionENFont;
                rankingPlayerRemainingTimeText.text = instruction1P_EN.GetLabelContent("RankingPlayerRemainingTimeText");
                rankingPlayerRemainingTimeText.font = instructionENFont;
                rankingPlayerTimeElapsedText.text = instruction1P_EN.GetLabelContent("RankingPlayerTimeElapsedText");
                rankingPlayerTimeElapsedText.font = instructionENFont;
                pleaseWaitLabelText.text = instruction1P_EN.GetLabelContent("PleaseWaitText");
                pleaseWaitLabelText.font = instructionENFont;
                for(int i = 0; i < rankingPlayerPositionLabelTexts.Length; i++)
                {
                    rankingPlayerPositionLabelTexts[i].font = instructionENFont;
                }
                break;
            case Language.DisplayLanauge.Japanese:
                instructionTitleText.text = instruction1P_JP.GetLabelContent("InstructionTitleText");
                instructionTitleText.font = instructionJPFont;
                instructionTitleText.fontStyle = FontStyle.Bold;
                missionButtonText.text = instruction1P_JP.GetLabelContent("MissionButtonText");
                missionButtonText.font = instructionJPFont;
                missionButtonText.fontStyle = FontStyle.Bold;
                controlsButtonText.text = instruction1P_JP.GetLabelContent("ControlsButtonText");
                controlsButtonText.font = instructionJPFont;
                controlsButtonText.fontStyle = FontStyle.Bold;
                hintsButtonText.text = instruction1P_JP.GetLabelContent("HintsButtonText");
                hintsButtonText.font = instructionJPFont;
                hintsButtonText.fontStyle = FontStyle.Bold;
                missionText.text = instruction1P_JP.GetLabelContent("MissionText").Replace("|", Environment.NewLine);
                missionText.font = instructionJPFont;
                missionText.fontStyle = FontStyle.Bold;
                keyboardButtonText.text = instruction1P_JP.GetLabelContent("KeyboardButtonText");
                keyboardButtonText.font = instructionJPFont;
                keyboardButtonText.fontStyle = FontStyle.Bold;
                joystickButtonText.text = instruction1P_JP.GetLabelContent("JoystickButtonText");
                joystickButtonText.font = instructionJPFont;
                joystickButtonText.fontStyle = FontStyle.Bold;
                hint1ButtonText.text = instruction1P_JP.GetLabelContent("Hint1ButtonText");
                hint1ButtonText.font = instructionJPFont;
                hint1ButtonText.fontStyle = FontStyle.Bold;
                hint2ButtonText.text = instruction1P_JP.GetLabelContent("Hint2ButtonText");
                hint2ButtonText.font = instructionJPFont;
                hint2ButtonText.fontStyle = FontStyle.Bold;
                hint3ButtonText.text = instruction1P_JP.GetLabelContent("Hint3ButtonText");
                hint3ButtonText.font = instructionJPFont;
                hint3ButtonText.fontStyle = FontStyle.Bold;
                hint4ButtonText.text = instruction1P_JP.GetLabelContent("Hint4ButtonText");
                hint4ButtonText.font = instructionJPFont;
                hint4ButtonText.fontStyle = FontStyle.Bold;
                hint5ButtonText.text = instruction1P_JP.GetLabelContent("Hint5ButtonText");
                hint5ButtonText.font = instructionJPFont;
                hint5ButtonText.fontStyle = FontStyle.Bold;
                hint1Text.text = instruction1P_JP.GetLabelContent("Hint1Text").Replace("|", Environment.NewLine);
                hint1Text.font = instructionJPFont;
                hint1Text.fontStyle = FontStyle.Bold;
                hint2Text.text = instruction1P_JP.GetLabelContent("Hint2Text").Replace("|", Environment.NewLine);
                hint2Text.font = instructionJPFont;
                hint2Text.fontStyle = FontStyle.Bold;
                hint3Text.text = instruction1P_JP.GetLabelContent("Hint3Text").Replace("|", Environment.NewLine);
                hint3Text.font = instructionJPFont;
                hint3Text.fontStyle = FontStyle.Bold;
                hint4Text.text = instruction1P_JP.GetLabelContent("Hint4Text").Replace("|", Environment.NewLine);
                hint4Text.font = instructionJPFont;
                hint4Text.fontStyle = FontStyle.Bold;
                hint5Text.text = instruction1P_JP.GetLabelContent("Hint5Text").Replace("|", Environment.NewLine);
                hint5Text.font = instructionJPFont;
                hint5Text.fontStyle = FontStyle.Bold;
                hint1ImageEN.SetActive(false);
                hint2Image1EN.SetActive(false);
                hint2Image2EN.SetActive(false);
                hint3ImageEN.SetActive(false);
                hint4ImageEN.SetActive(false);
                hint5Image1EN.SetActive(false);
                hint5Image2EN.SetActive(false);
                hint1ImageJP.SetActive(true);
                hint2Image1JP.SetActive(true);
                hint2Image2JP.SetActive(true);
                hint3ImageJP.SetActive(true);
                hint4ImageJP.SetActive(true);
                hint5Image1JP.SetActive(true);
                hint5Image2JP.SetActive(true);
                switch (ControlType.chosenControlType)
                {
                    case "Type1":
                        keyboardControlType1EN.SetActive(false);
                        keyboardControlType1JP.SetActive(true);
                        keyboardControlType2EN.SetActive(false);
                        keyboardControlType2JP.SetActive(false);
                        break;
                    case "Type2":
                        keyboardControlType1EN.SetActive(false);
                        keyboardControlType1JP.SetActive(false);
                        keyboardControlType2EN.SetActive(false);
                        keyboardControlType2JP.SetActive(true);
                        break;
                }
                joystickControlEN.SetActive(false);
                joystickControlJP.SetActive(true);
                startButtonText.text = instruction1P_JP.GetLabelContent("StartButtonText");
                startButtonText.font = instructionJPFont;
                startButtonText.fontStyle = FontStyle.Bold;
                rankingButtonText.text = instruction1P_JP.GetLabelContent("RankingButtonText");
                rankingButtonText.font = instructionJPFont;
                rankingButtonText.fontStyle = FontStyle.Bold;
                titleButtonText.text = instruction1P_JP.GetLabelContent("TitleButtonText");
                titleButtonText.font = instructionJPFont;
                titleButtonText.fontStyle = FontStyle.Bold;
                returnButtonText.text = instruction1P_JP.GetLabelContent("ReturnButtonText");
                returnButtonText.font = instructionJPFont;
                returnButtonText.fontStyle = FontStyle.Bold;
                rankingLabelText.text = instruction1P_JP.GetLabelContent("RankingLabelText");
                rankingLabelText.font = instructionJPFont;
                rankingLabelText.fontStyle = FontStyle.Bold;
                rankingPlayerNameLabelText.text = instruction1P_JP.GetLabelContent("RankingPlayerNameLabelText");
                rankingPlayerNameLabelText.font = instructionJPFont;
                rankingPlayerNameLabelText.fontStyle = FontStyle.Bold;
                rankingPlayerFinalPositionText.text = instruction1P_JP.GetLabelContent("RankingPlayerFinalPositionText");
                rankingPlayerFinalPositionText.font = instructionJPFont;
                rankingPlayerFinalPositionText.fontStyle = FontStyle.Bold;
                rankingPlayerRemainingTimeText.text = instruction1P_JP.GetLabelContent("RankingPlayerRemainingTimeText");
                rankingPlayerRemainingTimeText.font = instructionJPFont;
                rankingPlayerRemainingTimeText.fontStyle = FontStyle.Bold;
                rankingPlayerTimeElapsedText.text = instruction1P_JP.GetLabelContent("RankingPlayerTimeElapsedText");
                rankingPlayerTimeElapsedText.font = instructionJPFont;
                rankingPlayerTimeElapsedText.fontStyle = FontStyle.Bold;
                pleaseWaitLabelText.text = instruction1P_JP.GetLabelContent("PleaseWaitText");
                pleaseWaitLabelText.font = instructionJPFont;
                pleaseWaitLabelText.fontStyle = FontStyle.Bold;
                for (int i = 0; i < rankingPlayerPositionLabelTexts.Length; i++)
                {
                    rankingPlayerPositionLabelTexts[i].font = instructionJPFont;
                    rankingPlayerPositionLabelTexts[i].fontStyle = FontStyle.Bold;
                }
                break;
        }
    }

    private void ReadRankingData(TextAsset rankingDataFile)
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
        foreach (RankingData data in rankingJson.detail)
        {
            if(PlayerPrefs.HasKey(Difficulty_1P_TempSave.chosenDifficulty + "_PlayerName_" + (index + 1)))
            {
                rankingPlayerNameText[index].text = PlayerPrefs.GetString(Difficulty_1P_TempSave.chosenDifficulty + "_PlayerName_" + (index + 1));
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        rankingPlayerNameText[index].font = instructionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        rankingPlayerNameText[index].font = instructionJPFont;
                        rankingPlayerNameText[index].fontStyle = FontStyle.Bold;
                        break;
                }
            }
            else
            {
                rankingPlayerNameText[index].text = data.playerName;
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        rankingPlayerNameText[index].font = instructionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        rankingPlayerNameText[index].font = instructionJPFont;
                        rankingPlayerNameText[index].fontStyle = FontStyle.Bold;
                        break;
                }
                PlayerPrefs.SetString(Difficulty_1P_TempSave.chosenDifficulty + "_PlayerName_" + (index+1), data.playerName);
            }
            if (PlayerPrefs.HasKey(Difficulty_1P_TempSave.chosenDifficulty + "_FinalPosition_" + (index + 1)))
            {
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        rankingFinalPositionText[index].text = PlayerPrefs.GetString(Difficulty_1P_TempSave.chosenDifficulty + "_FinalPosition_" + (index + 1))
                                                                                                 .Replace("Goal", instruction1P_EN.GetLabelContent("GoalText"))
                                                                                                 .Replace("Check Point", instruction1P_EN.GetLabelContent("CheckPointText"))
                                                                                                 .Replace("Start", instruction1P_EN.GetLabelContent("StartText"));
                        rankingFinalPositionText[index].font = instructionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        rankingFinalPositionText[index].text = PlayerPrefs.GetString(Difficulty_1P_TempSave.chosenDifficulty + "_FinalPosition_" + (index + 1))
                                                                                                 .Replace("Goal", instruction1P_JP.GetLabelContent("GoalText"))
                                                                                                 .Replace("Check Point", instruction1P_JP.GetLabelContent("CheckPointText"))
                                                                                                 .Replace("Start", instruction1P_JP.GetLabelContent("StartText"));
                        rankingFinalPositionText[index].font = instructionJPFont;
                        rankingFinalPositionText[index].fontStyle = FontStyle.Bold;
                        break;
                }
            }
            else
            {
                rankingFinalPositionText[index].text = data.finalPosition;
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        rankingFinalPositionText[index].font = instructionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        rankingFinalPositionText[index].font = instructionJPFont;
                        rankingFinalPositionText[index].fontStyle = FontStyle.Bold;
                        break;
                }
                PlayerPrefs.SetString(Difficulty_1P_TempSave.chosenDifficulty + "_FinalPosition_" + (index + 1), data.finalPosition);
            }
            if (PlayerPrefs.HasKey(Difficulty_1P_TempSave.chosenDifficulty + "_RemainingTime_" + (index + 1)))
            {
                rankingRemainingTimeText[index].text = PlayerPrefs.GetInt(Difficulty_1P_TempSave.chosenDifficulty + "_RemainingTime_" + (index + 1)).ToString();
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        rankingRemainingTimeText[index].font = instructionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        rankingRemainingTimeText[index].font = instructionJPFont;
                        rankingRemainingTimeText[index].fontStyle = FontStyle.Bold;
                        break;
                }
            }
            else
            {
                rankingRemainingTimeText[index].text = data.remainingTime.ToString();
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        rankingRemainingTimeText[index].font = instructionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        rankingRemainingTimeText[index].font = instructionJPFont;
                        rankingRemainingTimeText[index].fontStyle = FontStyle.Bold;
                        break;
                }
                PlayerPrefs.SetInt(Difficulty_1P_TempSave.chosenDifficulty + "_RemainingTime_" + (index + 1), data.remainingTime);
            }
            if (PlayerPrefs.HasKey(Difficulty_1P_TempSave.chosenDifficulty + "_TimeElapsed_" + (index + 1)))
            {
                rankingTimeElaspedText[index].text = PlayerPrefs.GetString(Difficulty_1P_TempSave.chosenDifficulty + "_TimeElapsed_" + (index + 1));
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        rankingTimeElaspedText[index].font = instructionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        rankingTimeElaspedText[index].font = instructionJPFont;
                        rankingTimeElaspedText[index].fontStyle = FontStyle.Bold;
                        break;
                }
            }
            else
            {
                rankingTimeElaspedText[index].text = data.timeElapsed;
                switch (Language.gameDisplayLanguage)
                {
                    case Language.DisplayLanauge.English:
                        rankingTimeElaspedText[index].font = instructionENFont;
                        break;
                    case Language.DisplayLanauge.Japanese:
                        rankingTimeElaspedText[index].font = instructionJPFont;
                        rankingTimeElaspedText[index].fontStyle = FontStyle.Bold;
                        break;
                }
                PlayerPrefs.SetString(Difficulty_1P_TempSave.chosenDifficulty + "_TimeElapsed_" + (index + 1), data.timeElapsed);
            }
            index++;
        }
    }

    public void ShowInstruction()
    {
        instructionCanvas.SetActive(true);
        rankingCanvas.SetActive(false);
        pleaseWaitCanvas.SetActive(false);

        btnMission.GetComponent<Image>().color = buttonSelectedColor;
        btnControl.GetComponent<Image>().color = buttonNormalColor;
        btnHints.GetComponent<Image>().color = buttonNormalColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }

    public void ShowRanking()
    {
        rankingCanvas.SetActive(true);
        instructionCanvas.SetActive(false);
        pleaseWaitCanvas.SetActive(false);
    }

    public void ShowObjectiveSection()
    {
        controlsSection.SetActive(false);
        hintsSection.SetActive(false);
        objectionSection.SetActive(true);

        btnMission.GetComponent<Image>().color = buttonSelectedColor;
        btnControl.GetComponent<Image>().color = buttonNormalColor;
        btnHints.GetComponent<Image>().color = buttonNormalColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }

    public void ShowControlsSection()
    {
        objectionSection.SetActive(false);
        hintsSection.SetActive(false);
        keyboardSection.SetActive(true);
        joystickSection.SetActive(false);
        controlsSection.SetActive(true);

        btnMission.GetComponent<Image>().color = buttonNormalColor;
        btnControl.GetComponent<Image>().color = buttonSelectedColor;
        btnHints.GetComponent<Image>().color = buttonNormalColor;
        btnKeyboard.GetComponent<Image>().color = buttonSelectedColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }

    public void ShowKeyboardSection()
    {
        joystickSection.SetActive(false);
        keyboardSection.SetActive(true);

        btnMission.GetComponent<Image>().color = buttonNormalColor;
        btnControl.GetComponent<Image>().color = buttonSelectedColor;
        btnHints.GetComponent<Image>().color = buttonNormalColor;
        btnKeyboard.GetComponent<Image>().color = buttonSelectedColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }

    public void ShowJoystickSection()
    {
        keyboardSection.SetActive(false);
        joystickSection.SetActive(true);

        btnMission.GetComponent<Image>().color = buttonNormalColor;
        btnControl.GetComponent<Image>().color = buttonSelectedColor;
        btnHints.GetComponent<Image>().color = buttonNormalColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonSelectedColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }

    public void ShowHintsSection()
    {
        objectionSection.SetActive(false);
        controlsSection.SetActive(false);
        hints1Section.SetActive(true);
        hints2Section.SetActive(false);
        hints3Section.SetActive(false);
        hints4Section.SetActive(false);
        hints5Section.SetActive(false);
        hintsSection.SetActive(true);

        btnMission.GetComponent<Image>().color = buttonNormalColor;
        btnControl.GetComponent<Image>().color = buttonNormalColor;
        btnHints.GetComponent<Image>().color = buttonSelectedColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonSelectedColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }

    public void ShowHints1Section()
    {
        hints1Section.SetActive(true);
        hints2Section.SetActive(false);
        hints3Section.SetActive(false);
        hints4Section.SetActive(false);
        hints5Section.SetActive(false);

        btnMission.GetComponent<Image>().color = buttonNormalColor;
        btnControl.GetComponent<Image>().color = buttonNormalColor;
        btnHints.GetComponent<Image>().color = buttonSelectedColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonSelectedColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }

    public void ShowHints2Section()
    {
        hints1Section.SetActive(false);
        hints2Section.SetActive(true);
        hints3Section.SetActive(false);
        hints4Section.SetActive(false);
        hints5Section.SetActive(false);

        btnMission.GetComponent<Image>().color = buttonNormalColor;
        btnControl.GetComponent<Image>().color = buttonNormalColor;
        btnHints.GetComponent<Image>().color = buttonSelectedColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonSelectedColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }

    public void ShowHints3Section()
    {
        hints1Section.SetActive(false);
        hints2Section.SetActive(false);
        hints3Section.SetActive(true);
        hints4Section.SetActive(false);
        hints5Section.SetActive(false);

        btnMission.GetComponent<Image>().color = buttonNormalColor;
        btnControl.GetComponent<Image>().color = buttonNormalColor;
        btnHints.GetComponent<Image>().color = buttonSelectedColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonSelectedColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }
    public void ShowHints4Section()
    {
        hints1Section.SetActive(false);
        hints2Section.SetActive(false);
        hints3Section.SetActive(false);
        hints4Section.SetActive(true);
        hints5Section.SetActive(false);

        btnMission.GetComponent<Image>().color = buttonNormalColor;
        btnControl.GetComponent<Image>().color = buttonNormalColor;
        btnHints.GetComponent<Image>().color = buttonSelectedColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonSelectedColor;
        btnHints5.GetComponent<Image>().color = buttonNormalColor;
    }
    public void ShowHints5Section()
    {
        hints1Section.SetActive(false);
        hints2Section.SetActive(false);
        hints3Section.SetActive(false);
        hints4Section.SetActive(false);
        hints5Section.SetActive(true);

        btnMission.GetComponent<Image>().color = buttonNormalColor;
        btnControl.GetComponent<Image>().color = buttonNormalColor;
        btnHints.GetComponent<Image>().color = buttonSelectedColor;
        btnKeyboard.GetComponent<Image>().color = buttonNormalColor;
        btnJoystick.GetComponent<Image>().color = buttonNormalColor;
        btnHints1.GetComponent<Image>().color = buttonNormalColor;
        btnHints2.GetComponent<Image>().color = buttonNormalColor;
        btnHints3.GetComponent<Image>().color = buttonNormalColor;
        btnHints4.GetComponent<Image>().color = buttonNormalColor;
        btnHints5.GetComponent<Image>().color = buttonSelectedColor;
    }

    public void StartGame()
    {
        instructionCanvas.SetActive(false);
        rankingCanvas.SetActive(false);
        pleaseWaitCanvas.SetActive(true);
        switch (Difficulty_1P_TempSave.chosenDifficulty)
        {
            case "Easy":
                StartCoroutine(LoadLevelAsynchronously(targetEasyMap));
                break;
            case "Normal":
                StartCoroutine(LoadLevelAsynchronously(targetNormalMap));
                break;
            case "Hard":
                StartCoroutine(LoadLevelAsynchronously(targetHardMap));
                break;
        }
    }

    IEnumerator LoadLevelAsynchronously(string targetMap)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetMap);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            //float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log("operation progress: "+operation.progress);
            loadingSlider.value = operation.progress;

            yield return 0;
        }
        operation.allowSceneActivation = true;
        loadingSlider.value = 1f;
        yield return operation;
    }

    public void ToTitle()
    {
        if (titleMap.Equals("")) { return; }
        SceneManager.LoadScene(titleMap);
    }
}
