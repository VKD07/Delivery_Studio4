using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerModelHandler : MonoBehaviour
{
    [SerializeField] GameObject[] customerModels;

    private void OnEnable()
    {
        ChooseARandomModel();
    }

    private void ChooseARandomModel()
    {
        DisableAllModels();

        int randomIndex = Random.Range(0, customerModels.Length);
        customerModels[randomIndex].SetActive(true);
    }

    void DisableAllModels()
    {
        for (int i = 0; i < customerModels.Length; i++)
        {
            customerModels[i].SetActive(false);
        }
    }
}
