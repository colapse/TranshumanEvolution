using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Transhuman : SerializedMonoBehaviour
{
    [SerializeField]
    private Dictionary<TechUpgradeCategory, List<GameObject>> upgradePartAttachPoints;
    [SerializeField]
    private Dictionary<TechUpgradeCategory, UpgradePart> activeUpgradeParts;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateActiveUpgradeParts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateActiveUpgradeParts()
    {
        if (upgradePartAttachPoints != null)
        {
            foreach (var attachPointsKVP in upgradePartAttachPoints)
            {
                if (activeUpgradeParts != null &&
                    activeUpgradeParts.ContainsKey(attachPointsKVP.Key) &&
                    activeUpgradeParts[attachPointsKVP.Key] != null &&
                    attachPointsKVP.Value != null)
                {
                    foreach (var attachPoint in attachPointsKVP.Value)
                    {
                        if (attachPoint == null)
                            continue;
                        
                        DeleteChildren(attachPoint);
                        
                        var upgradePart = activeUpgradeParts[attachPointsKVP.Key];
                        if(upgradePart?.prefab == null)
                            continue;
                        
                        GameObject upgradePartGO = Instantiate(upgradePart.prefab, attachPoint.transform);
                        Transform upgradePartParentAttachPoint = upgradePartGO.transform.Find("parentAttachPoint"); // TODO HACKY
                        upgradePartGO.transform.localPosition = upgradePartParentAttachPoint == null ? Vector3.zero : upgradePartParentAttachPoint.localPosition*-1;
                    }
                }
            }
        }
    }

    // Swaps the active upgrade part and returns the old one.
    public UpgradePart SwapActiveUpgradePart(UpgradePart newUpgradePart)
    {
        UpgradePart oldPart = null;
        if (newUpgradePart != null &&
            newUpgradePart.upgradeCategory != null &&
            activeUpgradeParts.ContainsKey(newUpgradePart.upgradeCategory))
        {
            oldPart = activeUpgradeParts[newUpgradePart.upgradeCategory];
        }

        if (activeUpgradeParts.ContainsKey(newUpgradePart.upgradeCategory))
            activeUpgradeParts[newUpgradePart.upgradeCategory] = newUpgradePart;
        else
            activeUpgradeParts.Add(newUpgradePart.upgradeCategory, newUpgradePart);
        
        UpdateActiveUpgradeParts();
        
        return oldPart;
    }

    private void DeleteChildren(GameObject parent)
    {
        if (parent.transform.childCount > 0)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Destroy(parent.transform.GetChild(i).gameObject);
            }
        }
    }
}
