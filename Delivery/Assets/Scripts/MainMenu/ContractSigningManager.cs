using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MainMenuUIManager))]
public class ContractSigningManager : MonoBehaviour
{
    [Header("=== CLIPBOARD SETTINGS ===")]
    [SerializeField] Material clipBoardMaterial;
    [SerializeField] Texture2D clipBoardDefaultTex, clipBoardContractTex;
    [SerializeField] Animator clipBoardAnimator;
    [SerializeField] float contractTxtDelayTime = 1f;

    [Header("=== USERNAME INPUT SETTINGS ===")]
    [SerializeField] int maxCharacterLimit;

    [Header("=== SIGNATURE DRAW SETTINGS ===")]
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float minDistance = .1f;
    [SerializeField] float lineWidth;
    [SerializeField] float rayCastLength = 100f;
    [SerializeField] LayerMask clipBoardLayer;

    [Header("=== PEN ===")]
    [SerializeField] LayerMask penlayer;
    GameObject pen;

    Vector3 prevMousePos;
    Ray ray;
    RaycastHit lineHit, penHit;
    bool hasStartedWriting;
    bool allowedToWrite;
    bool contractIsActive;

    MainMenuUIManager menuUIManager => GetComponent<MainMenuUIManager>();
    private void Awake()
    {
        clipBoardMaterial.SetTexture("_BaseMap", clipBoardDefaultTex);
        menuUIManager.userNameInput.onValueChanged.AddListener(OnInputValueChanged);
        InitLineRenderer();
    }

    private void InitLineRenderer()
    {
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        InteractWithPen();
        WriteLines();
    }

    #region Signature Writing
    private void InteractWithPen()
    {
        if (!Physics.Raycast(ray, out penHit, rayCastLength, penlayer)) { return; }
        if (Input.GetMouseButton(0))
        {
            pen = penHit.transform.gameObject;
            pen.GetComponent<MeshRenderer>().enabled = false;
            allowedToWrite = true;
            ResetSignature();
        }
    }

    private void WriteLines()
    {
        if (!allowedToWrite) { return; }
        if (!Physics.Raycast(ray, out lineHit, rayCastLength, clipBoardLayer)) { return; }

        if (Input.GetMouseButton(0))
        {
            if (!hasStartedWriting)
            {
                hasStartedWriting = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, lineHit.point);
                lineRenderer.SetPosition(1, lineHit.point);
                prevMousePos = lineHit.point;
            }

            Vector3 currentPos = lineHit.point;

            if (Vector3.Distance(currentPos, prevMousePos) > minDistance)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPos);
                prevMousePos = currentPos;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (pen != null)
            {
                pen.GetComponent<MeshRenderer>().enabled = true;

                if (menuUIManager.userNameInput.text.Length <= 0)
                {
                    menuUIManager.placeHolderTxt.text = "Name is required here!";
                   menuUIManager.placeHolderTxt.color = Color.red;
                }
                else
                {
                    menuUIManager.SetActiveUserNameConfirmationPanel(true);
                }
                allowedToWrite = false;
                pen = null;
            }
        }
    }

    void ResetSignature()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = 2;
        hasStartedWriting = false;
    }

    public void SetPlayerUserName()
    {
        SetActiveContract(false);
        lineRenderer.gameObject.SetActive(false);
    }
    #endregion

    #region UI
    private void OnInputValueChanged(string newText)
    {
        if (newText.Length > maxCharacterLimit)
        {
            menuUIManager.userNameInput.text = newText.Substring(0, maxCharacterLimit);
        }
    }

    public void SetActiveContract(bool val)
    {
        if (val)
        {
            contractIsActive = true;
            clipBoardAnimator.SetBool("EnableContract", true);
            clipBoardMaterial.SetTexture("_BaseMap", clipBoardContractTex);
            StartCoroutine(EnableContractTxtDelay(contractTxtDelayTime));
        }
        else
        {
            contractIsActive = false;
            clipBoardAnimator.SetBool("EnableContract", false);
            clipBoardMaterial.SetTexture("_BaseMap", clipBoardDefaultTex);
        }
    }

    IEnumerator EnableContractTxtDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        menuUIManager.SetActiveContractPanel();
    }
    #endregion
}
