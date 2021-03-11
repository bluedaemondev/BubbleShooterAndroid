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

    // Start is called before the first frame update
    void Start()
    {
        btnComponent = GetComponent<Button>();

        btnComponent.onClick.AddListener(AddBubbles);

    }

   void AddBubbles()
    {
        var shooter = FindObjectOfType<BubbleShooter>();

        var currentToSwitch = shooter.currentThrowables.Dequeue();
        currentToSwitch.type = BubbleResources.GenerateSpecialBubbleType();
        currentToSwitch.GetComponent<SpriteRenderer>().sprite = currentToSwitch.type.sprite;
        
        shooter.currentThrowables.Enqueue(currentToSwitch);
    }
}
