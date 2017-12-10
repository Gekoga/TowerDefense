using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    public TurretBlueprint turrets;
    public TurretBlueprint missleLauncher;
    public TurretBlueprint laserBeamer;

    GameController gameController;


    void Start()
    {
        gameController = GameController.instance;
    }

    public void PurchaseStandardTurret() {
        print("standard turret selected");
        gameController.SelectTurretToBuild(turrets);
    }

    public void PurchaseStandardRocket() {
        print("standard Rocket selected");
        gameController.SelectTurretToBuild(missleLauncher);
    }

    public void PurchaseStandardLaser()
    {
        print("standard laser selected");
        gameController.SelectTurretToBuild(laserBeamer);
    }
}
