using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private bool gameEnded = false;

    public static GameController instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameController in scene!");
            return;
        }
        instance = this;
    }

    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    public NodeUI nodeUI;

    public bool CanBuild { get { return turretToBuild != null; } }

    public void SelectedNode(Node node) {
        if (selectedNode == node) {
            DeselectNode();
            return;
        }

        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode() {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SelectTurretToBuild(TurretBlueprint turret) {
        turretToBuild = turret;
        DeselectNode();
    }

    void Update () {
        if (gameEnded)
            return;
        if (PlayerStats.lives <= 0)
        {
            EndGame();
        }	
	}

    void EndGame()
    {
        gameEnded = true;
        IGMenu.instance.loseMenu.SetActive(true);
        IGMenu.instance.inGameUI.SetActive(false);
        Time.timeScale = 0;
    }
    public TurretBlueprint GetTurretToBuild() {
        return turretToBuild;
    }
}
