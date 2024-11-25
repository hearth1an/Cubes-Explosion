using UnityEngine;
using System;

public class Cube : MonoBehaviour
{
    private float _baseExplosionForce = 10f;
    private float _baseExplosionRadius = 40f;

    private int _currentSplitChance = 100;
    private int _maxSplitChance = 100;
    private int _minSplitChance = 0;
    private int _splitChanceReduce = 2;
    private bool _isSplitted = false;    

    public float baseExplosionForce { get; private set; }
    public float baseExplosionRadius { get; private set; }

    public event Action<Cube> SplitAllowed;

    private void Awake()
    {
        baseExplosionForce = _baseExplosionForce;
        baseExplosionRadius = _baseExplosionRadius;        
    }

    private void OnMouseDown()
    {
        if (_isSplitted)
            return;

        if (CanSplit())
        {
            SplitAllowed?.Invoke(this);            
            _isSplitted = true;
            TriggerExplosion();

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }    

    private void TriggerExplosion()
    {
        int forceMultiplier = 3;
        float cubeSize = transform.localScale.magnitude;
        float force = _baseExplosionForce / cubeSize * forceMultiplier;
        float radius = _baseExplosionRadius / cubeSize * forceMultiplier;
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

    public void UpdateSplitChance(int chance)
    {
        _currentSplitChance = chance;
    }

    public int GetCurrentSplitChance()
    {
        int chance = _currentSplitChance;

        return chance;
    }

    public void ChangeScale(Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }    

    public void ChangeColor()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
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

    private bool CanSplit()
    {
        int randomValue = UnityEngine.Random.Range(_minSplitChance, _maxSplitChance);

        if (randomValue <= _currentSplitChance)
        {
            _currentSplitChance /= _splitChanceReduce;

            return true;
        }

        return false;
    }
}
