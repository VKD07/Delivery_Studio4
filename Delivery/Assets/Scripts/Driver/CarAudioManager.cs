using Driver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioManager : MonoBehaviour
{
    [SerializeField] DriverControls driverControls;
    [SerializeField] AudioSource carEngineSource, carScreechSource, reverseSource;

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
    Rigidbody rb => GetComponent<Rigidbody>();

    private void Update()
    {
        AccelerationSound();
        CarScreechingSound();
        ReverseSFX();
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

        if (rb.velocity.magnitude <= 10)
        {
            if (carEngineSource.pitch > minPitch)
            {
                carEngineSource.pitch -= Time.deltaTime * reduceMultiplier;
            }
        }
    }

    void CarScreechingSound()
    {
        if (rb.velocity.magnitude > 25 && Input.GetKeyDown(driverControls.breakKey))
        {
            if (carScreechSource.isPlaying) return;
            carScreechSource.Play();
        }

        if (rb.velocity.magnitude > 25)
        {
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            reversed = true;
            AudioManager.instance.PlaySound("ReverseTrigger");
        }

        if (Input.GetKeyDown(KeyCode.W) && reversed)
        {
            reversed = false;
            AudioManager.instance.PlaySound("ReverseTrigger");
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (reverseSource.isPlaying) return;
            reverseSource.Play();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            reverseSource.Stop();
        }
    }

    public void TriggerRandomCarHits()
    {
        int randomIndex = Random.Range(0, carHits.Length);
        carEngineSource.PlayOneShot(carHits[randomIndex], carHitVolume);
    }
}
