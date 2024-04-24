using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetLocation : MonoBehaviour
{
    public bool hideGlow;

    [Header("=== CUSTOMER PLACEMENT ===")]
    [SerializeField] Transform customerPlacement;
    [SerializeField] GameObject customerPrefab; 

    public MeshRenderer render => GetComponent<MeshRenderer>();
    private void Start()
    {
        EnableDisableGlowRing();
    }

    private void EnableDisableGlowRing()
    {
        if (hideGlow)
        {
            render.enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// If Driver Has arrived then Trigger win
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ShowCustomer();
            other.gameObject.transform.parent.gameObject.SetActive(false);

            StartCoroutine(SendArrivePackets());
            DriverSpawnerManager.instance.DisableSpawnedDriver();
            CustomerDialougeManager.instance.EnableCustomer();
            CarScreenManager.instance?.DisableCarScreen();

            ////NetworkSender.instance?.SendNetworkDriverArrived();
            //SendPackets.SendDriverArrived(ClientManager.instance.playerData.teamNumber);
            ////Client.instance.SendPacket(new DriverArrivedPacket(true).Serialize());
            //WinManager.instance?.DeclareWinner(true);
        }
    }

    IEnumerator SendArrivePackets()
    {
        while (true)
        {
            yield return null;
            SendPackets.SendDriverArrived(ClientManager.instance.playerData.teamNumber);
        }
    }

    void ShowCustomer()
    {
        GameObject customer = Instantiate(customerPrefab, customerPlacement.position, Quaternion.identity);
    }
}