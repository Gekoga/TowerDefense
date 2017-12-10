using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

    public GameObject ui;
    private Node target;
    public Text upgradeCost;
    public Text SellProfit;

    public void SetTarget(Node _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();
        if (!target.isUpgraded)
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
        else
            upgradeCost.text = "Done";
        if (!target.isUpgraded)
            SellProfit.text = "$" + target.turretBlueprint.nuSellProfit;
        else
            SellProfit.text = "$" + target.turretBlueprint.uSellProfit;


        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
    }
    public void Upgrade()
    {
        target.UpgradeTurret();
        GameController.instance.DeselectNode();
    }
    public void Sell()
    {
        target.SellTurret();
        GameController.instance.DeselectNode();
    }
}
