using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            MudSplashManager mudSplash = other.GetComponentInParent<MudSplashManager>();
            if (mudSplash == null) return;
            mudSplash.EnableMudSplash();
        }
    }
}
