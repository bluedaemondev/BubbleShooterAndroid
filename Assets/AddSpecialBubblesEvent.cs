using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
public class AddSpecialBubblesEvent : MonoBehaviour
{
    [Header("Cantidad de burbujas especiales a entregar")]
    public int qtyGiven = 3;
    Button btnComponent;
    public GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        btnComponent = GetComponent<Button>();

        gameManager = FindObjectOfType<GameManager>();

        btnComponent.onClick.AddListener(AddBubbles);

    }

    void AddBubbles()
    {
        gameManager.gun.BlockGun();
        var counters = GameObject.FindObjectsOfType<Counter>();
        foreach (var ctr in counters)
        {
            ctr.StopTimer();
        }

        var newSpecial = gameManager.ballManager.GenerateBallAsBullet();
        var lastBullet = gameManager.gun.GetLastBulletInChain();

        var lastBulletObjectRef = gameManager.ballManager.GenerateBallAsBullet();

        gameManager.gun.ClearBullets();

        //lastBulletObjectRef = lastBullet;

        newSpecial.isLineSpecial = true;
        newSpecial.GetComponent<UnityEngine.UI.Image>().sprite = BubbleResources.GenerateSpecialBubbleType().sprite;
        newSpecial.GetComponent<UnityEngine.UI.Image>().color = Color.white;

        //for (int i = 0; i < qtyGiven; i++)
        //    gameManager.gun.LoadBullets(newSpecial);


        gameManager.gun.LoadDoneBullets(newSpecial, lastBulletObjectRef);

        gameManager.gun.UnBlockGun();
        
        foreach (var ctr in counters)
        {
            ctr.ContinueTimer();
        }

        Debug.Log("loaded " + newSpecial.ToString());
    }
}
