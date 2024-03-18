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

    #region private vars
    CarTroubleShooting chosenMalfunction;
    int crashCount;
    float currentTime;
    bool isBroken;
    #endregion

    private void Update()
    {
        FixMalfunction();
    }

    private void FixMalfunction()
    {
        if (chosenMalfunction == null) { return; }

        EnableBlinkingSymbol();
        if (chosenMalfunction.CheckifCarIsFixed() && currentTime < chosenMalfunction.timeToHold)
        {
            currentTime += Time.deltaTime;
        }
        else if (chosenMalfunction.CheckifCarIsFixed() && currentTime >= chosenMalfunction.timeToHold)
        {
            crashCount = 0;
            chosenMalfunction = null;
            malfunctionSymbol.enabled = false;
            smokeParticle.Stop();
            onFixCar.Invoke();
            isBroken = false;
        }
        else
        {
            currentTime = 0;
        }
    }

    public void TriggerCarMalfunction()
    {
        if (crashCount >= numberOfCrashesToTrigger-1)
        {
            if (!isBroken)
            {
                isBroken = true;
                crashCount = 0;
                onCarBroken.Invoke();
                ChooseRandomMalfunction();
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
        malfunctionSymbol.enabled = true;
        malfunctionSymbol.sprite = chosenMalfunction.getSpriteSymbol;
    }

    void EnableBlinkingSymbol()
    {
        float alpha = Mathf.Lerp(0, 1, Mathf.Sin(Time.time * symbolBlinkSpeed));
        malfunctionSymbol.color = new Color(malfunctionSymbol.color.r, malfunctionSymbol.color.g, malfunctionSymbol.color.b, alpha);
    }
}
