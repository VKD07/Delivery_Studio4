using UnityEngine;

[CreateAssetMenu(fileName = "CarColor_", menuName = "SHOP/Add New Car Color")]
public class CarColorItem : ScriptableObject
{
    public Color itemColor;
    public int itemPrice;
    public Texture2D itemTexture;
}
