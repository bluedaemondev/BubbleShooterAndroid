using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
public class SetPointMultiplierOnClick : MonoBehaviour
{
    Button btnComponent;

    // Start is called before the first frame update
    void Start()
    {
        this.btnComponent = GetComponent<Button>();
        this.btnComponent.onClick.AddListener(ChangeMultiplierBasedOnShuffleState);
    }

    void ChangeMultiplierBasedOnShuffleState()
    {
        var targetMult = ShuffleMusicManager.instance.currentIndexSelected;
        PointsManager.instance.SetMultiplier(targetMult+1);
        Debug.Log("target multiplier " + targetMult + " , manager = " + PointsManager.instance.GetComboMultiplier());

    }
}
