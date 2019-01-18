﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JsonConfiguration : MonoBehaviour {

	GameObject[] machines;

	string jsonString;
	string dataPath;
	string json;
    private string saveDate;
	private Machine loadObject;

	public GameObject SaveButton;
	public GameObject LoadButton;
	public GameObject LoadPanel;



	// Use this for initialization
	void Start () {
		machines = GameObject.FindGameObjectsWithTag("Machine");
		dataPath = Application.dataPath + "/StreamingAssets/SavedLayout/" + "Swift " + GetDate() + ".json";
		SaveConfig();
		LoadConfig();
	}


	public void SaveData()
	{
        StartCoroutine(TakeScreenshot(Screen.width, Screen.height));

		ListOfMachines myList = new ListOfMachines();
		myList.machinesList = new List<Machine>();
		
		foreach (var machine in machines)
		{
			Machine machineData = new Machine();
			machineData.Name = machine.name;
			Debug.Log(machineData.Name);
			machineData.Position = machine.transform.position;
			machineData.Rotation = machine.transform.rotation;
			myList.machinesList.Add(machineData);
		
		}
		
		json = JsonUtility.ToJson(myList);
		Debug.Log(json);
		File.WriteAllText(dataPath, json);

	}

	public void LoadData(string myButton)
	{
		ListOfMachines myListofMachines = new ListOfMachines();
		myListofMachines.machinesList = new List<Machine>();
		string dataAsJSON = File.ReadAllText(myButton);
		myListofMachines = JsonUtility.FromJson<ListOfMachines>(dataAsJSON);
		
		foreach(var element in myListofMachines.machinesList)
		{
			GameObject myTarget = GameObject.Find(element.Name);
			myTarget.transform.position = element.Position;
			myTarget.transform.rotation = element.Rotation;
		}

	}

	public void SaveConfig(){
		GameObject newSaveButton = Instantiate(SaveButton) as GameObject;
		newSaveButton.transform.SetParent(LoadPanel.transform, false);
		newSaveButton.name = "Save";
		newSaveButton.GetComponentInChildren<Text>().text = newSaveButton.name;
		var button = newSaveButton.GetComponent<UnityEngine.UI.Button>();
		button.onClick.AddListener(() => SaveData());
	}

	public void LoadConfig()
	{
		string[] filePaths = Directory.GetFiles(Application.dataPath + "/StreamingAssets/SavedLayout/", "*.json");
		foreach (string filePath in filePaths)
		{
			GameObject newLoadButton = Instantiate(LoadButton) as GameObject;
			newLoadButton.transform.SetParent(LoadPanel.transform, false);
			newLoadButton.name = filePath;
			newLoadButton.GetComponentInChildren<Text>().text = Path.GetFileName(filePath);
			var button = newLoadButton.GetComponent<UnityEngine.UI.Button>();
			button.onClick.AddListener(() => LoadData(newLoadButton.name));
		}
	}


	public string GetDate()
	{
		DateTime localDate = DateTime.Now;
		string format = "yyyy MM dd - HH mm ss";

		String myDate = localDate.ToString(format);
        saveDate = myDate;
		return saveDate;
		
	}


    IEnumerator TakeScreenshot(int width, int height)
    {
        // First, find and enable the camera, then wait until the end of the current frame
        Camera myCamera = GameObject.FindGameObjectWithTag("ScreenshotCamera").GetComponent<Camera>();
        myCamera.enabled = true;
        yield return new WaitForEndOfFrame();

        // Create the render texture and the resulting 2D output
        RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB); // Test this
        myCamera.targetTexture = renderTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, false);

        // Force the camera to render and encode into a png
        myCamera.Render();
        RenderTexture.active = renderTexture;
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);
        byte[] byteArray = renderResult.EncodeToPNG();

        // Create the PNG file on the hard drive
        string CurrentScreenShotName = "Swift " + saveDate + ".png";
        File.WriteAllBytes(Application.dataPath + "/StreamingAssets/Screenshots/" + CurrentScreenShotName, byteArray);

        // Release the temporary render texture, other variables and turn off the camera
        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;
        RenderTexture.active = null;
        myCamera.enabled = false;
    }


}

[Serializable]
public class ListOfMachines
{
	public List<Machine> machinesList;
}

[Serializable]
public class Machine
{
	public string Name;
	public Vector3 Position;
	public Quaternion Rotation;
	
}