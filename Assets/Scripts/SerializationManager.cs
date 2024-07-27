using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class SerializationManager
{
    public static bool Save(int saveNumber, PlayerData playerData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        string path = Application.persistentDataPath + "/saves/save" + saveNumber.ToString() + ".save";

        FileStream file = File.Create(path);

        formatter.Serialize(file, playerData);
        file.Close();

        return true;
    }

    public static PlayerData Load(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            PlayerData save = (PlayerData)formatter.Deserialize(file);
            file.Close();
            Debug.Log("Loaded File at " + path);
            return save;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            file.Close();
            return null;
        }


    }

    private static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        // SurrogateSelector selector = new SurrogateSelector();

        // InventorySerializationSurrogate inventorySerializationSurrogate = new InventorySerializationSurrogate();

        // selector.AddSurrogate(typeof(InventoryContainer[]), new StreamingContext(StreamingContextStates.All), inventorySerializationSurrogate);

        // formatter.SurrogateSelector = selector;

        return formatter;
    }
}
