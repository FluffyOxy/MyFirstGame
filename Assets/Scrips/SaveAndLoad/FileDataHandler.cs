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
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));//����Ҫ�󴴽�Ŀ¼�����Ѿ�������ʲôҲ������

            string dataToStore = JsonUtility.ToJson(_data, true);//��������ת��Ϊstring(��public��[SerializeField]�ɱ����л��������boolָʾ�ļ��Ƿ񱻸�ʽ������߿ɶ���)

            if(isEncryptData)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))//�����ļ�(���ļ��Ѿ��������串��)
            {
                using(StreamWriter writer = new StreamWriter(stream))//����д����
                {
                    writer.Write(dataToStore);//д��
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
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);//������α��ԭֵ
        }

        return modifiedData;
    }
}
