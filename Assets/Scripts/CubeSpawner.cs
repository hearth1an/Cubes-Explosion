using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private List<Cube> _cubes;
    [SerializeField] private Exploder _exploder;
    [SerializeField] private int _minCubes = 2;
    [SerializeField] private int _maxCubes = 6;

    private List<Cube> _activeCubes = new();

    private void Start()
    {
        foreach (Cube cube in _cubes)
        {
            RegisterCube(cube);
        }
    }

    private void OnDestroy()
    {
        foreach (Cube cube in _activeCubes)
        {
            UnregisterCube(cube);
        }
    }    

    public void SpawnCubes(Cube sourceCube)
    {
        int childCount = UnityEngine.Random.Range(_minCubes, _maxCubes + 1);
        List<Cube> newCubes = new();

        for (int i = 0; i < childCount; i++)
        {
            Cube newCube = CreateCube(sourceCube);
            newCubes.Add(newCube);
        }

        _exploder.TriggerExplosion(sourceCube.Position, sourceCube.BaseExplosionForce, sourceCube.BaseExplosionRadius, newCubes);

        UnregisterCube(sourceCube);
        Destroy(sourceCube.gameObject);
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

    private Cube CreateCube(Cube sourceCube)
    {
        int scaleReduceValue = 2;
        Vector3 newScale = sourceCube.transform.localScale / scaleReduceValue;

        Cube newCube = Instantiate(_cubePrefab, sourceCube.Position, UnityEngine.Random.rotation);

        newCube.Init(newScale, sourceCube);       

        RegisterCube(newCube);
        return newCube;
    }
}