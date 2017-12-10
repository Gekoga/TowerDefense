using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

    public Color hoverColor;
    private Color startColor;

    public Vector3 positionOffset;

    [HideInInspector]
    public GameObject turret;
   // [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;

    private Renderer rend;

    GameController gameController; 

    void Start() {
        gameController = GameController.instance;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    public Vector3 GetBuildPosition() {
        return transform.position + positionOffset;
    }

    //when you click
    void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null) {
            print("Can't build there! - TODO: Display on screen");
            gameController.SelectedNode(this);
            return;
        }

        if (!gameController.CanBuild)
            return;

        BuildTurret(gameController.GetTurretToBuild());
        gameController.SelectTurretToBuild(null);
        
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        //Check if you have enough money
        if (PlayerStats.money < blueprint.cost) {
            print("Not enough money to build that!");
            return;
        }

        //Take costs from your money
        PlayerStats.money -= blueprint.cost;

        //Build the turret
        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        turretBlueprint = blueprint;

        print("Turret build! Money left:" + PlayerStats.money);
    }

    public void UpgradeTurret() {
        //Check if you have enough money
        if (PlayerStats.money < turretBlueprint.upgradeCost) {
            print("Not enough money to upgrade that!");
            return;
        }

        if (!isUpgraded) {
            //Take costs from your money
            PlayerStats.money -= turretBlueprint.upgradeCost;

            //Get rid of the old turret
            Destroy(turret);

            //Build the new and beter turret
            GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
            turret = _turret;

            isUpgraded = true;

            print("Turret Upgraded! Money left:" + PlayerStats.money);
        }
    }

    public void SellTurret() {
        if (!isUpgraded) {
            PlayerStats.money += turretBlueprint.nuSellProfit;
        }
        else if (isUpgraded) {
            PlayerStats.money += turretBlueprint.uSellProfit;
        }
        Destroy(turret);
        print("Turret Sold! Money left:" + PlayerStats.money);
        isUpgraded = false;


    }

    //if you hover over a node
    void OnMouseEnter() {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!gameController.CanBuild)
            return;
        rend.material.color = hoverColor;
    }

    void OnMouseExit() {
        rend.material.color = startColor;
    }
}
