using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MyTankManager 
{
    public Color PlayerColor;
    public Transform BirthPoint;

    [HideInInspector]public int PlayerNumber;
    [HideInInspector]public GameObject instance;
    [HideInInspector]public string ColoredPlayerText;
    [HideInInspector]public int wins;

    private MyTankMovement tankMovement;
    private MyTankShooting tankShooting;
    private GameObject canvasGameObject;

    public void Setup()
    {
        tankMovement = instance.GetComponent<MyTankMovement>();
        tankShooting = instance.GetComponent<MyTankShooting>();
        canvasGameObject = instance.GetComponentInChildren<Canvas>().gameObject;

        tankMovement.PlayerNumber = PlayerNumber;
        tankShooting.PlayerNumber = PlayerNumber;
        ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerColor) + ">PLAYER " + PlayerNumber + "</color>";

        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();
        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = PlayerColor;
        }
    }

    public void DisableControl()
    {
        tankMovement.enabled = false;
        tankShooting.enabled = false;
        canvasGameObject.SetActive(false);
    }

    public void EnableControl()
    {
        tankMovement.enabled = true;
        tankShooting.enabled = true;
        canvasGameObject.SetActive(true);
    }

    public void Reset()
    {
        instance.transform.position = BirthPoint.position;
        instance.transform.rotation = BirthPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
