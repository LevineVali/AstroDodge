using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayerStats(PlayerStats _stats)
    {
        // create the formatter
        BinaryFormatter formatter = new BinaryFormatter();

        // create the path
        string path = Application.persistentDataPath + "/Stats.save";

        // create the filestream
        FileStream stream = new FileStream(path, FileMode.Create);

        // create new stats-file
        PlayerStats stats = new PlayerStats(_stats)
        {
            saved = true
        };

        // save the file
        formatter.Serialize(stream, stats);

        // close the stream
        stream.Close();
    }

    public static PlayerStats LoadPlayerStats()
    {
        // create the path
        string path = Application.persistentDataPath + "/Stats.save";

        if (File.Exists(path))
        {
            // create the formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // create the filestream
            FileStream stream = new FileStream(path, FileMode.Open);

            // open the file and save this in a variable
            PlayerStats stats = formatter.Deserialize(stream) as PlayerStats;

            // close the stream
            stream.Close();

            // return the stats
            return stats;
        }
        else
        {
            Debug.Log("File not found");

            return null;
        }
    }

    public static void SaveOptions(Options _options)
    {
        // create the formatter
        BinaryFormatter formatter = new BinaryFormatter();

        // create the path
        string path = Application.persistentDataPath + "/Options.save";

        // create the filestream
        FileStream stream = new FileStream(path, FileMode.Create);

        // create ne options-file
        Options options = new Options(_options);

        // save the file
        formatter.Serialize(stream, options);

        // close the stream
        stream.Close();
    }

    public static Options LoadOptions()
    {
        // create the path
        string path = Application.persistentDataPath + "/Options.save";

        if (File.Exists(path))
        {
            // create the formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // create the filestream
            FileStream stream = new FileStream(path, FileMode.Open);

            // open the file and save this in a variable
            Options options = formatter.Deserialize(stream) as Options;

            // close the stream
            stream.Close();

            // return the stats
            return options;
        }
        else
        {
            Debug.LogWarning("File not found! Default values used.");

            Options options = new Options();

            return options;
        }
    }
}
