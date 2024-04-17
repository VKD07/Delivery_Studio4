using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Package", menuName = "Create New Package")]
public class Package : ScriptableObject
{
    [Header("=== Package Properties ===")]
    public GameObject packageUIprefab;
    public Sprite packageShape;
    public Color[] packageColor = { Color.red, Color.blue, Color.green };
    public Color[] tagColors = { Color.red, Color.blue, Color.green };


    public int packageIndex { get; private set; }
    public int tagIndex { get; private set; }

    [HideInInspector] public Color chosenPackageColor { get; private set; }
    [HideInInspector] public Color chosenTagColor { get; private set; }
    [HideInInspector] public GameObject spawnedPackageUI { get; private set; }
    public void InitPackage(Transform parent, string name)
    {
        GameObject newPrefabUI = Instantiate(packageUIprefab, parent.position, Quaternion.identity);

        newPrefabUI.name = name;

        packageIndex = Random.Range(0, packageColor.Length);
        chosenPackageColor = packageColor[packageIndex];
        newPrefabUI.transform.Find("PackageImg").GetComponent<Image>().color = chosenPackageColor;
   
        tagIndex = Random.Range(0, tagColors.Length);
        chosenTagColor = tagColors[tagIndex];
        newPrefabUI.transform.Find("TagImg").GetComponent<Image>().color = chosenTagColor;
        newPrefabUI.transform.parent = parent;
        spawnedPackageUI = newPrefabUI;
    }
}
