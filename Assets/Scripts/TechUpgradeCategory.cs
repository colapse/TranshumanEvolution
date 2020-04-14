using UnityEngine;

[CreateAssetMenu(fileName="UpgradeCategory",menuName="TransHuman/New Upgrade Category",order=3)]
public class TechUpgradeCategory : ScriptableObject
{
    public string categoryName;
    public string categoryDescription;
    public Sprite categoryIcon;
    public TechUpgradeCategory topCategory;
}