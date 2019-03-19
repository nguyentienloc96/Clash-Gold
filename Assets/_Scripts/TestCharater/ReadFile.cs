using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadFile : MonoBehaviour
{
    private void Start()
    {
        WriteFile();
    }
    string data;
    void ReadFileName(string namefile)
    {
        TextAsset spelldata = (TextAsset)Resources.Load(namefile, typeof(TextAsset));
        string datalv = spelldata.text;
        string line;
        StringReader file = new StringReader(datalv);
        while ((line = file.ReadLine()) != null)
        {
            data += ":" + line + ":" + ",\n";
        }

        file.Close();
    }

    void WriteFile()
    {
        ReadFileName("NameIsLand");
        string path = "Assets/Resources/NameIsLand2.txt";
        StreamWriter wfile = new StreamWriter(path);
        wfile.WriteLine(data);
        wfile.Close();
    }
}
