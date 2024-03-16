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

    [Space(1)]
    [SerializeField] UnityEvent onCarBroken;
    [SerializeField] UnityEvent onFixCar;

    #region private vars
    CarTroubleShooting chosenMalfunction;
    int crashCount;
    float currentTime;
    #endregion

    private void Update()
    {
        FixMalfunction();
    }

    private void FixMalfunction()
    {
        if (chosenMalfunction == null) { return; }

        if (chosenMalfunction.CheckifCarIsFixed() && currentTime < chosenMalfunction.timeToHold)
        {
            currentTime += Time.deltaTime;
        }
        else if (chosenMalfunction.CheckifCarIsFixed() && currentTime >= chosenMalfunction.timeToHold)
        {
            chosenMalfunction = null;
            malfunctionSymbol.enabled = false;
            smokeParticle.Stop();

            onFixCar.Invoke();
        }
        else
        {
            currentTime = 0;
        }
    }

    public void TriggerCarMalfunction()
    {
        if (crashCount < numberOfCrashesToTrigger - 1)
        {
            crashCount++;
        }
        else
        {
            crashCount = 0;
            onCarBroken.Invoke();
            ChooseRandomMalfunction();
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
        smokeParticleMain.startColor = chosenMalfunction.GetSmokeColor();
    }

    void EnableSymbol()
    {
        malfunctionSymbol.enabled = true;
        malfunctionSymbol.sprite = chosenMalfunction.getSpriteSymbol();
    }
}
