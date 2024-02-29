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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Client.instance.SendPacket(new DriverArrivedPacket(true));
            WinManager.instance.DeclareWinner(true);
        }
    }


}
