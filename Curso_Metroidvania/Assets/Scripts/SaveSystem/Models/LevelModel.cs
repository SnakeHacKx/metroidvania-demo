using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class LevelModel
{


    public void Save(string gameName, LevelData levelData)
    {
        var binaryFormater = new BinaryFormatter();

        var file = File.Create(Application.persistentDataPath + "/LevelData_" + gameName + ".data");
        binaryFormater.Serialize(file, levelData);

        file.Close();
    }

    public LevelData Load(string gameName)
    {
        var levelData = new LevelData();
        var binaryFormater = new BinaryFormatter();
        var path = Application.persistentDataPath + "/LevelData_" + gameName + ".data";
        if (File.Exists(path))
        {
            var file = File.OpenRead(path);
            levelData = binaryFormater.Deserialize(file) as LevelData;
            file.Close();

        }
        else
        {
            levelData = null;
        }

        return levelData;
    }


}
