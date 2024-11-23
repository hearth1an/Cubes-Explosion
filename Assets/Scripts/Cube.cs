using UnityEngine;

public class Cube : MonoBehaviour
{
    private static int _currentSplitChance = 100;
    private int _maxSplitChance = 100;
    private int _minSplitChance = 0;
    private int _splitChanceReduce = 2;

    private CubeSpawner _spawner;

    private void Awake()
    {
        _spawner = FindObjectOfType<CubeSpawner>();
    }

    private void OnMouseDown()
    {
        if (_spawner == null)
        {
            Debug.LogError("CubeSpawner is not assigned!");
            return;
        }

        Debug.Log($"Current split chance: {_currentSplitChance}%");
        
        int randomValue = Random.Range(_minSplitChance, _maxSplitChance);

        Debug.Log($"Random value: {randomValue}");

        if (randomValue <= _currentSplitChance)
        {            
            _spawner.SpawnCubes(transform);
            _currentSplitChance /= _splitChanceReduce;
        }
        else
        {
            Debug.Log("Cube destroyed!");
            Destroy(gameObject);
        }
    }
}
