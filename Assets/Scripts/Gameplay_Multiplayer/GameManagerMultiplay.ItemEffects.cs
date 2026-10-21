using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GameManagerMultiplay の partial 分割: アイテム効果・標的探索
public partial class GameManagerMultiplay : MonoBehaviour
{
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
}
