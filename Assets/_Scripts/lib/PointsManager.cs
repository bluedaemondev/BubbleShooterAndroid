using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager instance { get; private set; }

    int total = 0;
    
    public float comboMultiplier = 1;
    public float timeToResetComboMultiplier = 4.5f; // cuanto tiempo pasa hasta que puede hacer otro golpe y sumar al combo
    public float currentTimeInCombo = 0; // timer actual

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public int GetTotalPoints()
    {
        return this.total;
    }

    public void AddToTotal(int pts)
    {
        this.total += pts;

        HudController.current.UpdatePointsUI(total);
        Debug.Log("total actual = " + GetTotalPoints());
    }

    public float SumToComboMultiplier()
    {
        return this.comboMultiplier++;
    }

    public void ResetComboMultiplier()
    {
        this.comboMultiplier = 1;
    }
}
