using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatPopupCanvasUIController : PopupCanvasUIController
{
    // Start is called before the first frame update
    public override void SubscribeToCorrespondingEvent()
    {
        base.SubscribeToCorrespondingEvent();
        GameManagerActions.instance.defeatEvent.AddListener(ActivatePopup);
    }

}
