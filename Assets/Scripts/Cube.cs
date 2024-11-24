using UnityEngine;

public class Cube : MonoBehaviour
{
    private static int _currentSplitChance = 100;
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
            _spawner.SpawnCubes(transform);            
        }
        else
        { 
            _exploder.Explode(gameObject);
        }        
    }    

    private bool CanSplit()
    {
        int randomValue = Random.Range(_minSplitChance, _maxSplitChance);

        if (randomValue <= _currentSplitChance)
        {
            _currentSplitChance /= _splitChanceReduce;
            return true;
        }
        else
        {
            return false;
        }
    }
}
