using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SaveManager
{
    private const string _saveName = "world.bin";

    private readonly string _filePath = Path.Combine(Application.persistentDataPath, _saveName);

    public void SaveToFile(IEnumerable<byte> data)
    {
        try
        {
            using (FileStream stream = new FileStream(_filePath, FileMode.OpenOrCreate))
            {
                BinaryWriter writer = new(stream);
                writer.Write(data.Count());
                writer.Write(data.ToArray());
            }
        }
        catch
        {
            Debug.Log("Save data failed");
            throw;
        }
    }

    public byte[] LoadFromFile()
    {
        try
        {
            using (FileStream stream = new FileStream(_filePath, FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(stream);
                int length = reader.ReadInt32();
                byte[] data = reader.ReadBytes(length);
                return data;
            }
        }
        catch
        {
            Debug.Log("Load from fail failed");
            return null;
        }
    }
}