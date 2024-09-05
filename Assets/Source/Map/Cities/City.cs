using System;

public class City
{
    private readonly uint _x;
    private readonly uint _y;
    private readonly CitySize _size;
    private readonly uint _id;

    public City(uint x, uint y, CitySize size)
    {
        _x = x >= 0 ? x : throw new ArgumentOutOfRangeException(nameof(x));
        _y = y >= 0 ? y : throw new ArgumentOutOfRangeException(nameof(y));
        _size = size;
        _id = (uint)UnityEngine.Random.Range(0, int.MaxValue);
    }

    public uint Id => _id;

    public uint X => _x;

    public uint Y => _y;

    public CitySize Size => _size;
}
