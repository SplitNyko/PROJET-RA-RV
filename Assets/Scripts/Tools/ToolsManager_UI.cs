﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(ToolsManager))]
public class ToolsManager_UI : MonoBehaviour {

	public Canvas UICanvas;
	public GameObject TouchpadDirection;
	public GameObject ToolPanelPrefab;
	[Range(0,100)]public float SpaceRatio;
	private ToolsManager toolsManager;
	private Color color_selected;
	private bool isActive = false;

	void Awake () {
		toolsManager = GetComponent<ToolsManager>();

		color_selected = new Color(0.349f, 0.537f, 0.811f);

		float rawItemAngle = 360 / toolsManager.ToolsList.Count;
		float itemAngle = rawItemAngle * ((100 - SpaceRatio) / 100);
		
		for(int i = 0; i < toolsManager.ToolsList.Count; i++)
		{
			var newPanel = Instantiate(ToolPanelPrefab, UICanvas.transform);
			newPanel.name = "Panel_" + i;
			newPanel.transform.localRotation = Quaternion.Euler(0, 0, -(rawItemAngle * i) + itemAngle/2 );
			var newPanelImg = newPanel.GetComponent<Image>();
			newPanelImg.fillAmount = itemAngle/360;
			var toolIconPivot = newPanel.transform.GetChild(0);
			toolIconPivot.transform.localRotation = Quaternion.Euler(0, 0, -(itemAngle/2));
			var imageComps = newPanel.GetComponentsInChildren<Image>();
			foreach(Image img in imageComps)
			{
				if(img.gameObject != newPanel)
				{
					img.transform.localRotation = Quaternion.Euler(0, 0, (rawItemAngle*i));
					img.sprite = toolsManager.ToolsList[i].ToolIcon;
				}
			}
		}
	}

	private void Update()
	{
		if(SteamVR_Input.Swift.inActions.Touchpad.GetStateDown(toolsManager.inputSource))
		{
			isActive = true;
		}
		if(SteamVR_Input.Swift.inActions.Touchpad.GetStateUp(toolsManager.inputSource))
		{
			isActive = false;
		}
		if(isActive)
		{
			Vector2 touchpadPos = SteamVR_Input.Swift.inActions.TouchPos.GetAxis(toolsManager.inputSource);
			float touchpadAngle = ((Mathf.Atan2(touchpadPos.x, touchpadPos.y) / Mathf.PI) * 180f);
			if(touchpadAngle < 0) touchpadAngle += 360f;
			// Haptics pour chaque outil
			Debug.Log(touchpadAngle);
			TouchpadDirection.transform.rotation = Quaternion.Euler(0, 0, -touchpadAngle);
		}
		UICanvas.enabled = isActive;
	}

}
