using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Exploder _exploder;

    private int _maxSplitChance = 100;
    private int _minSplitChance = 0;
    private int _splitChanceReduce = 2;
    
    private Renderer _renderer;

    public event Action<Cube> SplitRequested;

    public Rigidbody Rigidbody { get; private set; }    

    public float BaseExplosionForce { get; private set; } = 10f;
    public float BaseExplosionRadius { get; private set; } = 40f;
    public int CurrentSplitChance { get; private set; } = 100;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();        
    }

    private void OnMouseDown()
    {
        if (CanSplit())
        {
            SplitRequested?.Invoke(this);            
        }
        else
        {
            List<Cube> cubes = _exploder.FindCubesInRadius(transform.position, BaseExplosionRadius);
            _exploder.TriggerExplosion(transform.position, BaseExplosionForce, BaseExplosionRadius, cubes);
        }

        Destroy(gameObject);
    }

    public void Init(Vector3 scale, Cube sourceCube)
    {
        ChangeScale(scale);
        ChangeColor();
        UpdateSplitChance(sourceCube.CurrentSplitChance);
        Rigidbody.useGravity = true;
    }

    public void AddExplosion(float baseForce, Vector3 center, float baseRadius)
    {
        float upwardsModifier = 0.1f;

        int forceMultiplier = 3;

        float sizeFactor = 1f / transform.localScale.magnitude;
        float adjustedForce = baseForce * sizeFactor * forceMultiplier;
        float adjustedRadius = baseRadius * sizeFactor * forceMultiplier;

        Rigidbody.AddExplosionForce(adjustedForce, center, adjustedRadius, upwardsModifier, ForceMode.Impulse);
    }

    private void UpdateSplitChance(int chance)
    {    
        chance /= _splitChanceReduce;

        CurrentSplitChance = chance;
    }

    private void ChangeScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    private void ChangeColor()
    {
        _renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }    

    private bool CanSplit()
    {
        int randomValue = UnityEngine.Random.Range(_minSplitChance, _maxSplitChance);
        
        return randomValue <= CurrentSplitChance;
    }    
}