using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/*
 * Allows holding some persistent data that is not used in GameState de facto, but used for some meta operations like loading or syncing.
 */
public interface IBuffer
{
    void AddToBuffer(string data);
    string GetBuffer();
    void SetBuffer(string data);
}

public class SimpleStringBuffer : IBuffer
{
    private string buffer;
    public void SetBuffer(string buffer)
    {
        this.buffer = buffer;
    }

    public void AddToBuffer(string data)
    {
        buffer += data;
    }

    public string GetBuffer()
    {
        return buffer;
    }
}

public class FileReaderBuffer : IBuffer
{

    private string buffer;

    public void SetBuffer(string buffer)
    {
        this.buffer = buffer;
    }

    /// <summary>
    /// Adds to buffer all file data of given path.
    /// </summary>
    /// <param name="path">Path of the file</param>
    public void AddToBuffer(string relativePath)
    {
        relativePath = Path.Combine(Application.persistentDataPath, relativePath);
        if (File.Exists(relativePath))
            buffer += File.ReadAllText(relativePath);
        else
            Debug.LogError("Can't add to FileReaderBuffer: no file found!");
    }

    public void SaveBuffer(string relativePath)
    {
        relativePath = Path.Combine(Application.persistentDataPath, relativePath);
        Debug.Log(relativePath);
        File.WriteAllText(relativePath, buffer);
    }

    public string GetBuffer()
    {
        return buffer;
    }
}
   
