using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private int _maxSplitChance = 100;
    private int _minSplitChance = 0;
    private int _splitChanceReduce = 2;
    private bool _isSplitted = false;

    private Rigidbody _rigidbody;
    private Renderer _renderer;

    public event Action<Cube> SplitRequested;

    public float BaseExplosionForce { get; private set; } = 10f;
    public float BaseExplosionRadius { get; private set; } = 40f;
    public int CurrentSplitChance { get; private set; } = 100;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnMouseDown()
    {
        if (_isSplitted)
            return;

        if (TryReduceSplitChance())
        {
            SplitRequested?.Invoke(this);
            _isSplitted = true;
        }
        else
        {
            var cubesInRadius = FindCubesInRadius(BaseExplosionRadius);

            Exploder.TriggerExplosion(transform.position, BaseExplosionForce, BaseExplosionRadius, cubesInRadius);
           
        }

        Destroy(gameObject);
    }

    public void UpdateSplitChance(int chance)
    {
        CurrentSplitChance = chance;
    }

    public void ChangeScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void ChangeColor()
    {
        _renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void AddExplosion(float force, Vector3 center, float radius)
    {
        float upwardsModifier = 0.1f;

        GetComponent<Rigidbody>().AddExplosionForce(force, center, radius, upwardsModifier, ForceMode.Impulse);
    }

    private bool TryReduceSplitChance()
    {
        int randomValue = UnityEngine.Random.Range(_minSplitChance, _maxSplitChance);

        if (randomValue <= CurrentSplitChance)
        {
            CurrentSplitChance /= _splitChanceReduce;

            return true;
        }

        return false;
    }

    private List<Cube> FindCubesInRadius(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        List<Cube> cubes = new();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Cube>(out Cube cube) && cube != this)
            {
                cubes.Add(cube);
            }
        }

        return cubes;
    }
}