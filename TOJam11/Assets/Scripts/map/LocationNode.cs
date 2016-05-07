﻿using UnityEngine;
using System.Collections.Generic;

public class LocationNode : MonoBehaviour {
    public string locationName;
    public string description;
    public bool isStart = false;
    public float encounterChance = 0f;      // Percenatage chance of an encounter taking place while approaching this node.
    [HideInInspector]
    public  List<LocationConnection> Connections = new List<LocationConnection>();
    [HideInInspector]
    public PlayerNode playerNode = null;
    [HideInInspector]
    public MapController map = null;
    private Renderer rend = null;
    private Color activeColor = Color.green;
    private Color hoverColor = Color.cyan;
    private Color selectableColor = Color.white;
    private Color defaultColor = Color.grey;
    private bool isSelectable = false;       // Set to true if the player can move to this location.
    private bool isActive = false;           // Set to true if the player is at this location.
    private bool isScaledUp = false;
    private bool showingTooltip = false;

    void Start() {
        rend = GetComponent<Renderer>();        
    }

    void OnMouseOver()
    {
        if (!map.isPaused)
        {
            HandleClick();
            HandleHoverOver();
        }
             
    }

    void OnMouseExit()
    {
        HandleHoverExit();
    }

    void HandleClick() {
        if (Input.GetMouseButtonDown(0) && playerNode != null && isSelectable)
        {
            playerNode.SetTargetLocation(this);
            transform.localScale = new Vector3(1f, 1f, 1f);
            isScaledUp = false;
            SetColor(activeColor);
        }
    }

    void HandleHoverOver()
    {
        if (!isScaledUp && isSelectable)
        {
            isScaledUp = true;
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            SetColor(hoverColor);
        }

        if (!showingTooltip)
        {            
            ShowTooltip();
        }
    }

    void HandleHoverExit()
    {
        if (isScaledUp)
        {
            isScaledUp = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
            SetColor(selectableColor);
        }

        if (showingTooltip)
        {
            HideTooltip();
        }
    }

    public void Activate() {
        isActive = true;
        SetColor(activeColor);
        for(var i = 0; i < Connections.Count; i++)
        {
            Connections[i].GetOther(this).SetSelectability(true);
        }
    }

    public void Reset() {
        isActive = false;
        SetSelectability(false);
    }

    public void SetSelectability(bool val)
    {
        isSelectable = val;
        if (val)
        {
            SetColor(Color.white);
        } else
        {
            SetColor(defaultColor);
        }
    }

    void SetColor(Color color)
    {
        rend.material.color = color;
    }

    public void RandomEncounter() {
        if(Random.value * 100f < encounterChance)
        {
            Debug.Log("There's going to be a fight!");
            map.SetPause(true);
            // need to handle changing between encounter and map.
            
        }
    }

    void ShowTooltip() {
        showingTooltip = true;
        map.SetTooltip(locationName, description);
        map.EnableTooltip();
    }

    void HideTooltip()
    {
        showingTooltip = false;
        map.DisableTooltip();
    }
}
