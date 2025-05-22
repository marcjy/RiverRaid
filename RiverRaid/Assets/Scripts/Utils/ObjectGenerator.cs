using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectGenerator<T> : MonoBehaviour where T : MonoBehaviour, IGenerable
{
    public event EventHandler<T> OnObjectGet;
    public event EventHandler<T> OnObjectReleased;

    public T[] ObjectPrefabs;
    public float SecondsPerObject;

    private Coroutine _generatorCoroutine;
    private Dictionary<Type, ObjectPool<T>> _objectPools;
    private Dictionary<int, Tuple<Type, T>> _activeObjectsInPools;

    private IGameLifeCycle _gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        _generatorCoroutine = null;

        _activeObjectsInPools = new Dictionary<int, Tuple<Type, T>>();
        _objectPools = new Dictionary<Type, ObjectPool<T>>();

        foreach (T @object in ObjectPrefabs)
        {
            _objectPools.Add(@object.GetType(), new ObjectPool<T>(
                createFunc: () => CreateObstalceInPool(@object),
                actionOnGet: @object => GetObjectFromPool(@object),
                actionOnRelease: @object => ReleaseObjectFromPool(@object),
                actionOnDestroy: @object => DestroyObjectFromPool(@object),
                false, 2, 4));
        }
    }

    public void Init(IGameLifeCycle GameManager)
    {
        _gameManager = GameManager;

        _gameManager.OnStartNewGame += HandleStartNewGame;
        _gameManager.OnLevelEnds += HandleLevelEnds;
        _gameManager.OnEndGame += HandleGameEnded;
    }

    private void TurnOn() => _generatorCoroutine = StartCoroutine(GeneratorLoop());
    private void TurnOff()
    {
        StopCoroutine(_generatorCoroutine);
        _generatorCoroutine = null;

        StopAllObjects();
    }

    private void StopAllObjects()
    {
        foreach (Tuple<Type, T> tuple in _activeObjectsInPools.Values)
            tuple.Item2.enabled = false;
    }

    private void HandleStartNewGame(object sender, EventArgs e) => TurnOn();
    private void HandleLevelEnds(object sender, EventArgs e) => ReleaseAllObjects();
    private void HandleGameEnded(object sender, EventArgs e) => TurnOff();

    private void HandleObjectShouldBeReleased(object sender, System.EventArgs e)
    {
        Type objectType = sender.GetType();
        _objectPools[objectType].Release((T)sender);
    }

    #region ObjectPool
    private T CreateObstalceInPool(T @object)
    {
        T newObject = Instantiate(@object, transform);
        newObject.OnShouldBeReleased += HandleObjectShouldBeReleased;
        return newObject;
    }
    private void GetObjectFromPool(T @object)
    {
        _activeObjectsInPools.Add(@object.GetInstanceID(), new Tuple<Type, T>(@object.GetType(), @object));
        @object.gameObject.SetActive(true);
        @object.enabled = true;

        @object.Init();

        OnObjectGet?.Invoke(this, @object);
    }
    private void ReleaseObjectFromPool(T @object)
    {
        _activeObjectsInPools.Remove(@object.GetInstanceID());
        @object.gameObject.SetActive(false);

        OnObjectReleased?.Invoke(this, @object);
    }
    private void DestroyObjectFromPool(T @object)
    {
        _activeObjectsInPools.Remove(@object.GetInstanceID());
        @object.OnShouldBeReleased -= HandleObjectShouldBeReleased;
    }
    private void ReleaseAllObjects()
    {
        List<Tuple<Type, T>> objectsToRelease = new List<Tuple<Type, T>>();

        foreach (Tuple<Type, T> tuple in _activeObjectsInPools.Values)
            objectsToRelease.Add(tuple);

        foreach (Tuple<Type, T> tuple in objectsToRelease)
            _objectPools[tuple.Item1].Release(tuple.Item2);
    }
    #endregion


    private IEnumerator GeneratorLoop()
    {
        while (true)
        {
            Type objectType = GetRandomObjectFromList();
            _objectPools[objectType].Get();

            yield return new WaitForSeconds(SecondsPerObject);
        }
    }

    private Type GetRandomObjectFromList() => ObjectPrefabs[UnityEngine.Random.Range(0, ObjectPrefabs.Length)].GetType();

    private void OnDestroy()
    {
        _gameManager.OnStartNewGame -= HandleStartNewGame;
        _gameManager.OnLevelEnds -= HandleLevelEnds;
        _gameManager.OnEndGame -= HandleGameEnded;
    }
}
