using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] unAssignedPlayers;

    public void SetNameToPlayerList(string playerName)
    {
        //TODO : Add new name to the list, check if theres no existing name
        for (int i = 0; i < unAssignedPlayers.Length; i++)
        {
            if (!unAssignedPlayers[i].gameObject.activeSelf)
            {
                unAssignedPlayers[i].gameObject.SetActive(true);
                unAssignedPlayers[i].SetText(playerName);
                break;
            }
        }
    }
}
