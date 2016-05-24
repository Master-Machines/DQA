using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

class SaveMaster {
    private static string PrimarySavePath = "saveData/";
    private static DeckMaster currentDeckMaster;

    static SaveMaster() {
        currentDeckMaster = new DeckMaster();
    }

    public static void Save() {
        BinaryFormatter formatter = new BinaryFormatter();
        currentDeckMaster.SaveAllCards();
        currentDeckMaster.SaveAllDecks();
    }

    public static bool WriteFile(Byte[] data, string directory, string fileName) {
        string truePath = Application.persistentDataPath + "/" + PrimarySavePath + directory;
        try {
            System.IO.Directory.CreateDirectory(truePath);
            truePath += fileName + ".dat";
            // Open file for reading
            Debug.Log("Writing to path: " + truePath);
            System.IO.FileStream _FileStream = new System.IO.FileStream(truePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from
            // a byte array.
            
            _FileStream.Write(data, 0, data.Length);
            
            

            // close file stream
            _FileStream.Close();

            return true;
        } catch (Exception _Exception) {
            // Error
            Debug.LogError("Exception caught while trying to write to file to path " + truePath + ": " + _Exception.ToString());
            return false;
        }
    }

    public static FileInfo[] getFiles(string directory) {
        string path = Application.persistentDataPath + "/" + PrimarySavePath + directory;
        if(!Directory.Exists(path)) {
            return new FileInfo[] { };
        }
        DirectoryInfo d = new DirectoryInfo(path);
        return d.GetFiles("*.dat");
    }

    public static DeckMaster CurrentDeckMaster() {
        return currentDeckMaster;
    }

    public static void ResetDeckMaster() {
        currentDeckMaster = new DeckMaster();
    }
    
}
