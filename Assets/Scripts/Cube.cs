using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private int _currentSplitChance = 100;
    private int _maxSplitChance = 100;
    private int _minSplitChance = 0;
    private int _splitChanceReduce = 2;
    private bool _isSplitted = false;

    public event Action<Cube> SplitRequested;

    public float BaseExplosionForce { get; private set; } = 10f;
    public float BaseExplosionRadius { get; private set; } = 40f;

    public int CurrentSplitChance => _currentSplitChance;
   
    private void OnMouseDown()
    {
        if (_isSplitted)
            return;

        if (TryReduceSplitChance())
        {
            SplitRequested?.Invoke(this);            
            _isSplitted = true;
            TriggerExplosion();            
        }

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }    

    public void UpdateSplitChance(int chance)
    {
        _currentSplitChance = chance;
    }

    public void ChangeScale(Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }    

    public void ChangeColor()
    {
        GetComponent<Renderer>().material.color = new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }

    public Vector3 GetScale()
    {
        Vector3 scale = gameObject.transform.localScale;

        return scale;
    }

    public Vector3 GetPosition()
    {
        Vector3 position = gameObject.transform.position;

        return position;
    }

    public void AddExplosion(float force, Vector3 center, float radius)
    {
        float upwardsModifier = 0.1f;

        gameObject.GetComponent<Rigidbody>().AddExplosionForce(force, center, radius, upwardsModifier, ForceMode.Impulse);
    }

    private bool TryReduceSplitChance()
    {
        int randomValue = UnityEngine.Random.Range(_minSplitChance, _maxSplitChance);

        if (randomValue <= _currentSplitChance)
        {
            _currentSplitChance /= _splitChanceReduce;

            return true;
        }

        return false;
    }

    private void TriggerExplosion()
    {
        int forceMultiplier = 3;
        float cubeSize = transform.localScale.magnitude;
        float force = BaseExplosionForce / cubeSize * forceMultiplier;
        float radius = BaseExplosionRadius / cubeSize * forceMultiplier;
        Vector3 center = transform.position;

        Collider[] colliders = Physics.OverlapSphere(center, radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rigidBody = collider.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                if (collider.TryGetComponent<Cube>(out Cube cube))
                {
                    cube.AddExplosion(force, center, radius);
                }
            }
        }
    }
}
