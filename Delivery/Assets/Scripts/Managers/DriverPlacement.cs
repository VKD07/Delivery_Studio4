using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DriverPlacement : MonoBehaviour
{
    [SerializeField] bool hideGlow;

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
            NetworkSender.instance?.SendNetworkDriverArrived();
            //Client.instance.SendPacket(new DriverArrivedPacket(true).Serialize());
            WinManager.instance?.DeclareWinner(true);
        }
    }
}
