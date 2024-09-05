using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CitiesSpawner
{
    private readonly int _distance;
    private readonly int _distanceSpread;
    private readonly List<City> _cities = new List<City>();
    private readonly int _mapWidth;

    public CitiesSpawner(uint mapSize, int distance, int distanceSpread)
    {
        _distance = distance > 0 ? distance : throw new ArgumentOutOfRangeException(nameof(distance));
        _distanceSpread = distanceSpread >= 0 ? distanceSpread : throw new ArgumentOutOfRangeException(nameof(distanceSpread));
        _mapWidth = mapSize > 0 ? (int)mapSize : throw new ArgumentOutOfRangeException(nameof(mapSize));

        _cities.Add(new City(mapSize / 2, mapSize / 2, CitySize.Huge));

        Debug.Log(Time.realtimeSinceStartup);
        SpawnCities();
        Debug.Log(Time.realtimeSinceStartup);
        Debug.Log(_cities.Count);
        /*
        for (int i = 0; i < _cities.Count; i++)
            Debug.Log($"City#{i}: {_cities[i].X}, {_cities[i].Y}");
        */
    }

    private void SpawnCities()
    {
        const int Tries = 5;

        int currentTries = Tries;

        while (currentTries > 0)
        {
            for (int i = 0; i < _cities.Count; i++)
            {
                if (TrySpawnCity(_cities[i]))
                {
                    currentTries = Tries;
                    break;
                }
            }

            currentTries--;
        }
    }

    private bool TrySpawnCity(City point)
    {
        int min = _distance - _distanceSpread;
        int max = _distance + _distanceSpread;
        int squareDistance = min * min;

        int distance = UnityEngine.Random.Range(min, max);
        Vector2 position = new Vector2(point.X, point.Y);
        Vector2 spot = UnityEngine.Random.insideUnitCircle.normalized * distance + position;

        if (spot.x < 0 || spot.x >= _mapWidth || spot.y < 0 || spot.y >= _mapWidth)
            return false;

        bool canSpawn = true;

        for (int i = _cities.Count - 1; i >= 0; i--)
        {
            position = new Vector2(_cities[i].X, _cities[i].Y);

            if (Vector2.SqrMagnitude(spot - position) < squareDistance)
            {
                canSpawn = false;
                break;
            }
        }

        if (canSpawn)
        {
            _cities.Add(new City((uint)spot.x, (uint)spot.y, CitySize.Huge));
            return true;
        }

        return false;
    }
}
