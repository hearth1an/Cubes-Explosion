using UnityEngine;

public class Cube : MonoBehaviour
{
    private int _currentSplitChance = 10;
    private int _maxSplitChance = 100;
    private int _minSplitChance = 0;
    private int _splitChanceReduce = 2;

    private CubeSpawner _spawner;
    private Exploder _exploder;

    private void Awake()
    {
        _spawner = FindObjectOfType<CubeSpawner>();
        _exploder = FindObjectOfType<Exploder>();
    }

    private void OnMouseDown()
    {
        if (CanSplit())
        {
            _spawner.SpawnCubes(this);
        }
        else
        {
            _exploder.Explode(this);
        }
    }

    private bool CanSplit()
    {
        int randomValue = Random.Range(_minSplitChance, _maxSplitChance);

        if (randomValue <= _currentSplitChance)
        {
            _currentSplitChance /= _splitChanceReduce;

            Debug.Log(_currentSplitChance);
            return true;
        }

        return false;
    }

    public Renderer GetRenderer()
    {
        return gameObject.GetComponent<Renderer>();
    }

    public Transform GetTransform()
    {
       return gameObject.GetComponent<Transform>();
    }

    public void ChangeScale(Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }

    public void ChangeColor()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color (Random.value, Random.value, Random.value);
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
}
