using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rounds : MonoBehaviour {

    public Text roundsText;

        void Update()
    {
        if (PlayerStats.rounds <= 3)
        {
            roundsText.text = "wave: " + PlayerStats.rounds;
        }
        else
        {
            roundsText.text = "final wave!";
        }
    }
}
