using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
        public static string   FileLocation = "/DevOps.do";
        public static SaveData Data = new SaveData();

        public void Save()
        {
                FileStream      file = File.Create(Application.persistentDataPath + FileLocation);
                BinaryFormatter bf   = new BinaryFormatter();
                
                bf.Serialize(file, Data);
                
                file.Close();
        }

        public void Load()
        {
                if (File.Exists(FileLocation))
                {
                        FileStream      file = File.Open(Application.persistentDataPath + FileLocation, FileMode.Open);
                        BinaryFormatter bf   = new BinaryFormatter();
                        Data = bf.Deserialize(file) as SaveData;
                        file.Close();
                }
                else
                {
                }
        }
        
        
}
