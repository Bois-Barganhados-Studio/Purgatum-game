using System;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;


public class DataSaver
{
    public static string DataPath = Application.persistentDataPath;
    private static string pattern = @"^(/[\w-]+)?/[\w-]+\.boi$"; // Padrão regex para verificar se o caminho é válido
    public static void printaAi()
    {
        Debug.Log(Application.persistentDataPath);
    }

    public static void SaveData(string fileName, object data, string filePath = "/")
    {


        if (!Regex.IsMatch(filePath + fileName, pattern))
        {
            throw new ArgumentException("O caminho não é válido:" + (filePath + fileName));
        }

        if (Directory.Exists(DataPath + filePath) == false)
        {
            Directory.CreateDirectory(DataPath + filePath);
        }

        filePath =  DataPath + filePath + fileName;

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(fileStream, data);
            Debug.Log("Salvo em: " + filePath);
        }
    }

    public static T LoadData<T>(string fileName, string filePath = "/", bool createIfNotExists = false)
    {
        if (!Regex.IsMatch(filePath + fileName, pattern))
        {
            throw new ArgumentException("O caminho não é válido: " + (filePath + fileName));
        }
        
        string completePath =  DataPath + filePath + fileName;

        if (File.Exists(completePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = new FileStream(completePath, FileMode.Open))
            {
                T loadedObj = (T)formatter.Deserialize(fileStream);
                Debug.Log("Object loaded successfully.");
                return loadedObj;
            }
        }
        else
        {
            Debug.Log("File does not exist: " + completePath);
            if(createIfNotExists)
            {
                T objectT = (T)Activator.CreateInstance(typeof(T));
                Debug.Log("Creating file...");
                SaveData(fileName, objectT, filePath);
                return LoadData<T>(fileName, filePath);
            }
            return default(T);
        }
    }


}
