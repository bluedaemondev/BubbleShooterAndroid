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
        var newSpecial = gameManager.ballManager.GenerateBallAsBullet();
        newSpecial.isLineSpecial = true;
        newSpecial.GetComponent<UnityEngine.UI.Image>().sprite = BubbleResources.GenerateSpecialBubbleType().sprite;

        gameManager.gun.BlockGun();
        gameManager.gun.LoadBullets(newSpecial);

        gameManager.gun.UnBlockGun();
        foreach (var ctr in GameObject.FindObjectsOfType<Counter>())
        {
            ctr.ContinueTimer();
        }

        Debug.Log("loaded " + newSpecial.ToString());
    }
}
