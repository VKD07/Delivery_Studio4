using Driver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioManager : MonoBehaviour
{
    [SerializeField] DriverControls driverControls;
    [SerializeField] AudioSource carEngineSource, carScreechSource, reverseSource, carCranking;

    [Header("=== CAR ACCEL SETTINGS ===")]
    [SerializeField] float acceleratorMultiplier = .1f;
    [SerializeField] float reduceMultiplier = 1f;
    [SerializeField] float maxPitch;
    [SerializeField] float minPitch = 1f;
    [SerializeField] Transform wheelHolder;

    [Header("=== CAR HITS SETTINGS ===")]
    [SerializeField] AudioClip[] carHits;
    [SerializeField] float carHitVolume = .3f;

    bool reversed;
    bool isTryingToStart;
    float carEngineInitVolume;
    bool carIsFixed;
    Rigidbody rb => GetComponent<Rigidbody>();
    CarMalfunction carMalfunction => GetComponent<CarMalfunction>();
    CarEngineShake engineShake => GetComponent<CarEngineShake>();

    private void Start()
    {
        carEngineInitVolume = carEngineSource.volume;
    }

    private void Update()
    {
        AccelerationSound();
        CarScreechingSound();
        ReverseSFX();
        CarCrankingSFX();
    }

    private void AccelerationSound()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (carEngineSource.pitch < maxPitch)
            {
                carEngineSource.pitch += Time.deltaTime * acceleratorMultiplier;
            }
            else if (carEngineSource.pitch >= maxPitch)
            {
                carEngineSource.pitch = maxPitch;
            }
        }
        else
        {
            if (carEngineSource.pitch > minPitch || Input.GetKey(driverControls.breakKey) && carEngineSource.pitch > minPitch)
            {
                carEngineSource.pitch -= Time.deltaTime * reduceMultiplier;
            }
        }

        if (rb.velocity.magnitude <= 8)
        {
            if (carEngineSource.pitch > minPitch)
            {
                carEngineSource.pitch -= Time.deltaTime * reduceMultiplier;
            }
        }
    }

    void CarScreechingSound()
    {
        if (rb.velocity.magnitude > 25)
        {
            if (Input.GetKey(driverControls.breakKey))
            {
                if (carScreechSource.isPlaying) return;
                carScreechSource.Play();
            }
            else if (Input.GetKeyUp(driverControls.breakKey))
            {
                carScreechSource.Stop();
            }

            if (wheelHolder.localEulerAngles.y > 20 && wheelHolder.localEulerAngles.y < 300 ||
                wheelHolder.localEulerAngles.y >= 330 && wheelHolder.localEulerAngles.y < 340)
            {
                if (carScreechSource.isPlaying) return;
                carScreechSource.Play();
            }
        }

        if (rb.velocity.magnitude >= 10 && Input.GetKeyDown(driverControls.breakKey))
        {
            AudioManager.instance.PlaySound("BreakSound");
        }

        if (rb.velocity.magnitude <= 1)
        {
            carScreechSource.Stop();
        }
    }

    private void ReverseSFX()
    {
        if (Input.GetKeyDown(KeyCode.S) && !reversed && !carMalfunction.isBroken)
        {
            reversed = true;
            AudioManager.instance.PlaySound("ReverseTrigger");
        }

        if (Input.GetKeyDown(KeyCode.W) && reversed)
        {
            reversed = false;
            AudioManager.instance.PlaySound("ReverseTrigger");
        }

        if (Input.GetKey(KeyCode.S) && !carMalfunction.isBroken)
        {
            if (reverseSource.isPlaying) return;
            reverseSource.Play();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            reverseSource.Stop();
        }
    }

    void CarCrankingSFX()
    {
        if (carMalfunction.isBroken)
        {
            carIsFixed = false;
            if (carMalfunction.errorRemovalTime > 0)
            {
                if (!isTryingToStart)
                {
                    isTryingToStart = true;
                    AudioManager.instance?.PlaySound("EngineTryingToStart");
                    StartCoroutine(StartCrankingLoop());
                }
                CinemachineShake.instance.ShakeCamera(.05f, .1f);
            }
            else
            {
                isTryingToStart = false;
                carCranking.Stop();
            }
        }
        else
        {
            isTryingToStart = false;
            carCranking.Stop();

            if (!carIsFixed)
            {
                AudioManager.instance?.PlaySound("CarEngineStart");
                carIsFixed = true;
                carEngineSource.Play();
                carEngineSource.volume = .5f;
            }
        }
    }

    public void TriggerRandomCarHits()
    {
        int randomIndex = Random.Range(0, carHits.Length);
        carEngineSource.PlayOneShot(carHits[randomIndex], carHitVolume);
    }

    public void EnableBrokenCar()
    {
        carEngineSource.Stop();
        AudioManager.instance?.PlaySound("EngineStop");
        CinemachineShake.instance.loop = false;
        StartCoroutine(ReduceEngineVolume());
    }

    IEnumerator ReduceEngineVolume()
    {
        while (carEngineSource.volume > 0)
        {
            yield return null;
            carEngineSource.volume -= Time.deltaTime;
        }
    }

    IEnumerator StartCrankingLoop()
    {
        yield return new WaitForSeconds(.3f);
        if (!carCranking.isPlaying)
        {
            carCranking.Play();
        }
    }
}
