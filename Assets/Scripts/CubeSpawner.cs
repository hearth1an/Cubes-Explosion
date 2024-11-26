using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private List<Cube> _cubes;
    [SerializeField] private int _minCubes = 2;
    [SerializeField] private int _maxCubes = 6;

    private List<Cube> _activeCubes = new();

    private void Start()
    {        
        foreach (var cube in _cubes)
        {
            RegisterCube(cube);
        }
    }

    private void RegisterCube(Cube cube)
    {
        cube.SplitRequested += SpawnCubes;
        _activeCubes.Add(cube);
    }

    private void UnregisterCube(Cube cube)
    {
        cube.SplitRequested -= SpawnCubes;
        _activeCubes.Remove(cube);
    }

    public void SpawnCubes(Cube sourceCube)
    {
        int childCount = UnityEngine.Random.Range(_minCubes, _maxCubes + 1);
        
        for (int i = 0; i < childCount; i++)
        {
            Cube newCube = CreateCube(sourceCube);
            _activeCubes.Add(newCube);            
        }

        foreach (Cube cube in _activeCubes)
        {            
            cube.AddExplosion(sourceCube.BaseExplosionForce, sourceCube.GetPosition(), sourceCube.BaseExplosionRadius);
        }

    }

    private Cube CreateCube(Cube sourceCube)
    {
        int scaleReduceValue = 2;
        Vector3 newScale = sourceCube.GetScale() / scaleReduceValue;

        Cube newCube = Instantiate(_cubePrefab, sourceCube.GetPosition(), UnityEngine.Random.rotation);       

        newCube.ChangeScale(newScale);
        newCube.ChangeColor();
        newCube.UpdateSplitChance(sourceCube.CurrentSplitChance);
        newCube.GetComponent<Rigidbody>().useGravity = true;

        return newCube;
    }
}
