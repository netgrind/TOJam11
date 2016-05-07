﻿using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

    private GridTile _tile;
    public GridTile tile
    {
        get { return _tile; }
        set
        {
            if (_tile != value)
            {
                MoveTile(value);
            }
        }
    }
    private Color _color;
    public Color color
    {
        get { return _color; }
        set {
            if (value != _color)
            {
                _color = value;
                foreach (Renderer r in renderers)
                    r.material.color = value;
            }
        }
    }
    Renderer[] renderers;

    public string name;
    public bool isPlayer;
    public float AP = 0;
    public CarAction[] actions;

    public LensedValue<float> health;
    public LensedValue<float> maxHealth;
    public LensedValue<float> maxAP;
    public LensedValue<float> turnAP;
    public LensedValue<float> defence;

    void Awake()
    {
        health = new LensedValue<float>(0);
        maxHealth = new LensedValue<float>(0);
        health.AddLens(new Lens<float>(int.MinValue, (x) => maxHealth.GetValue()));
        health.AddLens(new Lens<float>(int.MaxValue, (x) => Mathf.Max(x, maxHealth.GetValue())));
        maxAP = new LensedValue<float>(0);
        turnAP = new LensedValue<float>(0);
        defence = new LensedValue<float>(0);
    }

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    void MoveTile(GridTile t)
    {
        if(_tile!=null)
            _tile.car = null;
        transform.parent = t.transform;
        t.car = this;
        _tile = t;
	}
	
	// Update is called once per frame
	void Update () {

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, .1f); ;
	}

    public bool HasEnoughAP()
    {
        actions = GetComponentsInChildren<CarAction>();
        foreach (CarAction c in actions)
            if (c.ap <= AP)
                return true;
        return false;
    }

    void BeginTurn(Car c)
    {
        AP += turnAP.GetValue();
        AP = Mathf.Max(AP, maxAP.GetValue());
    }

    public void Damage(float damage)
    {
        defence.initialValue = damage;
        var d = defence.GetValue();
        health.AddLens(new Lens<float>(0, x => x - d));
        CheckHealth();
    }

    public void CheckHealth()
    {
        float hp = health.GetValue();
        if (hp <= 0)
        {
            _tile.car = null;
            BattleManager.DestroyCar(this);
        }
    }
}
