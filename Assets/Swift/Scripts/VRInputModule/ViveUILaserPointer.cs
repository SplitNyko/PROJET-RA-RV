﻿using UnityEngine;
using Valve.VR;


public class ViveUILaserPointer : IUILaserPointer
{
    public SteamVR_Input_Sources inputSource;

    protected override void Initialize()
    {
        base.Initialize();
    }

    public override bool ButtonDown()
    {
        return SteamVR_Input.Swift.inActions.Interact.GetStateDown(inputSource);
    }

    public override bool ButtonUp()
    {
        return SteamVR_Input.Swift.inActions.Interact.GetStateUp(inputSource);
    }

    public override void OnEnterControl(GameObject control)
    {
        if (control.GetComponent<UnityEngine.UI.Selectable>())
        {
            SteamVR_Input.Swift.outActions.Haptics.Execute(0, 0.1f, 50, 0.5f, inputSource);
        }
    }

    public override void OnExitControl(GameObject control)
    {

    }
}
