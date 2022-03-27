using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HeroModel
{

    public void Save(string gameName, HeroData heroData)
    {
        var binaryFormater = new BinaryFormatter();
        //Debug.Log(Application.persistentDataPath + "/HeroData_" + gameName + ".data");
        var file = File.Create(Application.persistentDataPath + "/HeroData_" + gameName + ".data");
        binaryFormater.Serialize(file, heroData);
        file.Close();


    }

    public HeroData Load(string gameName)
    {
        var heroData = new HeroData();
        var binaryFormater = new BinaryFormatter();

        var path = Application.persistentDataPath + "/HeroData_" + gameName + ".data";

        if (File.Exists(path))
        {
            var file = File.OpenRead(path);
            heroData = binaryFormater.Deserialize(file) as HeroData;
            file.Close();

        }
        else
        {
            heroData = null;
        }

        return heroData;
    }


}

