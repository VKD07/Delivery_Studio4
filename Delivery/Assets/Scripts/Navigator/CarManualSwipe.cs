using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

[RequireComponent(typeof(CarManualInitializer))]
public class CarManualSwipe : MonoBehaviour
{
    [SerializeField] GameObject closeBtn;

    [Header("=== SWIPE SETTINGS ===")]
    [SerializeField] float distanceToDisableFrontPage = 20f;
    [SerializeField] float paperMoveSpeed = 50f;
    float distance;

    RaycastHit hit;
    Ray ray;

    GameObject manual;
    public float xMouse;
    int currentActiveContent;
    bool triggerPageMovement;
    bool enableSwipe = true;

    Vector3 frontPagePos = Vector3.up;
    Vector3 secondPage = new Vector3(0, .9f, 0f);

    CarManualInitializer carManualInitializer => GetComponent<CarManualInitializer>();

    void Update()
    {
        MouseRayCast();
        MovePaperToTheRight();
    }

    private void MouseRayCast()
    {
        if (Input.GetMouseButton(0) && !triggerPageMovement)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 500f))
            {
                if (hit.transform.tag == "CarManual")
                {
                    manual = hit.transform.gameObject;
                    closeBtn.SetActive(false);
                    distance = Vector3.Distance(frontPagePos, carManualInitializer.pages[currentActiveContent].transform.localPosition);
                    EnableNextPage();
                }
            }
        }

        if (manual != null)
        {
            Swipe();
            if (Input.GetMouseButtonUp(0))
            {
                if (distance >= distanceToDisableFrontPage)
                {
                    triggerPageMovement = true;
                    StartCoroutine(DisableActivePage());
                    enableSwipe = false;
                    if (currentActiveContent >= carManualInitializer.pages.Count - 1)
                    {
                        currentActiveContent = 0;
                        carManualInitializer.pages[currentActiveContent].transform.localPosition = frontPagePos;
                    }
                    else
                    {
                        carManualInitializer.pages[currentActiveContent + 1].transform.localPosition = frontPagePos;
                        currentActiveContent++;
                    }
                }
                else if(!triggerPageMovement)
                {
                    manual.transform.localPosition = frontPagePos;
                    manual = null;
                }
                closeBtn.SetActive(true);
            }
        }
    }
    private void MovePaperToTheRight()
    {
        if (triggerPageMovement && manual != null)
        {
            manual.transform.localPosition += Vector3.right * Time.deltaTime * paperMoveSpeed;
        }
    }

    IEnumerator DisableActivePage()
    {
        yield return new WaitForSeconds(.2f);
        manual.transform.localPosition = secondPage;
        manual.SetActive(false);
        triggerPageMovement = false;
        manual = null;
        enableSwipe = true;
    }

    private void Swipe()
    {
        if (!enableSwipe) return;
        xMouse = Input.GetAxis("Mouse X") * Time.deltaTime * 150f;
        Vector3 newPos = new Vector3(xMouse, 0, 0);
        manual.transform.localPosition += newPos;
    }

    void EnableNextPage()
    {
        if (currentActiveContent < carManualInitializer.pages.Count - 1)
        {
            carManualInitializer.pages[currentActiveContent + 1].SetActive(true);
        }
        else
        {
            carManualInitializer.pages[currentActiveContent - (carManualInitializer.pages.Count - 1)].SetActive(true);
        }
    }
}