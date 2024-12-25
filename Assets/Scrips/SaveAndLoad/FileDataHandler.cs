using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool isEncryptData = false;
    private string codeWord;

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _isEncryptData, string _codeWord)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
        this.isEncryptData = _isEncryptData;
        this.codeWord = _codeWord;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));//按照要求创建目录（若已经存在则什么也不做）

            string dataToStore = JsonUtility.ToJson(_data, true);//将数据类转换为string(仅public和[SerializeField]可被序列化，后面的bool指示文件是否被格式化以提高可读性)

            if(isEncryptData)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))//开启文件(若文件已经存在则将其覆盖)
            {
                using(StreamWriter writer = new StreamWriter(stream))//创建写入器
                {
                    writer.Write(dataToStore);//写入
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error on trying to save data to file: " + fullPath + "\n" + e);
        }
    }
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (isEncryptData)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";

        for(int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);//异或两次变回原值
        }

        return modifiedData;
    }
}
