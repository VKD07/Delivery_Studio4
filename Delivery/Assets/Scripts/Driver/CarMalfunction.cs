using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarMalfunction : MonoBehaviour
{
    [SerializeField] int numberOfCrashesToTrigger = 4;
    [SerializeField] CarTroubleShooting[] malfunctions;

    [Header("=== SMOKE AND SYMBOLS ===")]
    [SerializeField] ParticleSystem smokeParticle;
    [SerializeField] Image malfunctionSymbol;
    [SerializeField] float symbolBlinkSpeed = 2f;

    [Space(1)]
    [SerializeField] UnityEvent onCarBroken;
    [SerializeField] UnityEvent onFixCar;

    [HideInInspector] public bool isBroken { get; private set; }
    public float errorRemovalTime { get; private set; }



    #region private vars
    CarTroubleShooting chosenMalfunction;
    int crashCount;
    bool errorHasPlayed;
    CarAudioManager carAudioManager => GetComponent<CarAudioManager>();
    #endregion

    private void Start()
    {
        CarScreenManager.instance.carMalfunctionSymbol = malfunctionSymbol;
    }

    private void Update()
    {
        FixMalfunction();
    }

    private void FixMalfunction()
    {
        if (chosenMalfunction == null) { return; }

        EnableBlinkingSymbol();
        if (chosenMalfunction.CheckifCarIsFixed() && errorRemovalTime < chosenMalfunction.timeToHold)
        {
            errorRemovalTime += Time.deltaTime;
        }
        else if (chosenMalfunction.CheckifCarIsFixed() && errorRemovalTime >= chosenMalfunction.timeToHold)
        {
            crashCount = 0;
            chosenMalfunction = null;
            malfunctionSymbol.enabled = false;
            smokeParticle.Stop();
            onFixCar.Invoke();
            isBroken = false;
            CarScreenManager.instance?.CarIsDamaged(false);
            SendPackets.SendCarMalfunction(false);
        }
        else
        {
            errorRemovalTime = 0;
        }
    }

    public void TriggerCarMalfunction()
    {
        if (crashCount >= numberOfCrashesToTrigger-1)
        {
            if (!isBroken)
            {
                CarScreenManager.instance?.CarIsDamaged(true);
                isBroken = true;
                crashCount = 0;
                onCarBroken.Invoke();
                ChooseRandomMalfunction();
                carAudioManager.EnableBrokenCar();
                SendPackets.SendCarMalfunction(true);
            }
        }
        else
        {
            crashCount++;
        }
    }

    void ChooseRandomMalfunction()
    {
        int randomIndex = Random.Range(0, malfunctions.Length);
        chosenMalfunction = malfunctions[randomIndex];

        EnableSmoke();
        EnableSymbol();
    }

    private void EnableSmoke()
    {
        smokeParticle.Play();
        var smokeParticleMain = smokeParticle.main;
        smokeParticleMain.startColor = chosenMalfunction.GetSmokeColor;
    }

    void EnableSymbol()
    {
        //malfunctionSymbol.enabled = true;
        malfunctionSymbol.sprite = chosenMalfunction.getSpriteSymbol;
    }

    void EnableBlinkingSymbol()
    {
        float alpha = Mathf.Lerp(0, 1, Mathf.Sin(Time.time * symbolBlinkSpeed));
        malfunctionSymbol.color = new Color(malfunctionSymbol.color.r, malfunctionSymbol.color.g, malfunctionSymbol.color.b, alpha);

        if (alpha > .9 && !errorHasPlayed)
        {
            AudioManager.instance?.PlaySound("CarError");
            errorHasPlayed = true;
        }
        else if(alpha <= 0)
        {
            errorHasPlayed = false;
        }
    }
}
