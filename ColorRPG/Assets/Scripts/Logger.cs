using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

//Classes regarding logging to external files. In general I am using collections and writing everything at the end to avoid opening and closing a file constantly

//Parent class for all log files    
public abstract class LogFiles
{
    protected string fileName;
    public LogFiles(string fileName)
    {
        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        System.IO.Directory.CreateDirectory("LogFiles/" + timeStamp);
        this.fileName = "LogFiles/" + timeStamp + "/" + fileName;
    }

    public abstract void Write();
}

// TO-DO: actually make csv a <T> and have the float-based one add float-related methods
public class GenericCsv : LogFiles
{
    Dictionary<string, List<string>> rows;
    List<string> labels;

    public GenericCsv(string fileName)
    : base(fileName)
    {
        this.fileName += ".csv";
        rows = new Dictionary<string, List<string>>();
        labels = new List<string>();
    }

    public Dictionary<string, List<string>> Rows
    {
        get { return rows; }
    }

    public void AddRow(string rowName, List<string> row = null)
    {
        rows[rowName] = row ?? new List<string>();
    }

    public override void Write()
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            for (int i = 0; i < labels.Count; i++)
            {
                writer.Write(labels[i] + ',');
            }
            if (labels.Count > 0)
            {
                writer.Write("\n");
            }
            foreach (string name in rows.Keys)
            {
                writer.Write(name);
                writer.Write(',');
                for (int i = 0; i < rows[name].Count; i++)
                {
                    writer.Write(rows[name][i]);
                    writer.Write(",");
                }
                writer.Write("\n");
            }
        }
    }

    public void OverwriteEntry(string rowName, int entryIndex, string newEntry)
    {
        rows[rowName][entryIndex] = newEntry;
    }


    public void OverwriteEntry(string rowName, string label, string newEntry)
    {
        rows[rowName][labels.IndexOf(label) - 1] = newEntry;
    }

    public void AddLabels(List<string> l)
    {
        labels.AddRange(l);
    }

    public void AddLabel(string l)
    {
        labels.Add(l);
    }
}


//class used to generate Csv files
public class Csv : LogFiles
{
    Dictionary<string, List<float>> rows;
    List<string> labels;

    public Csv(string fileName)
    : base(fileName)
    {
        this.fileName += ".csv";
        rows = new Dictionary<string, List<float>>();
        labels = new List<string>();
    }

    public Dictionary<string, List<float>> Rows
    {
        get { return rows; }
    }

    public void AddRow(string rowName)
    {
        rows[rowName] = new List<float>();
    }

    public void IncrementValue(string rowName, int entryNum, float increment)
    {
        rows[rowName][entryNum] += increment;
    }
    public void IncrementValue(string rowName, string label, float increment)
    {
        rows[rowName][labels.IndexOf(label) - 1] += increment;
    }

    public override void Write()
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            for (int i = 0; i < labels.Count; i++)
            {
                writer.Write(labels[i] + ',');
            }
            if (labels.Count > 0)
            {
                writer.Write("\n");
            }
            foreach (string name in rows.Keys)
            {
                writer.Write(name);
                writer.Write(',');
                for (int i = 0; i < rows[name].Count; i++)
                {
                    writer.Write(rows[name][i]);
                    writer.Write(",");
                }
                writer.Write("\n");
            }
        }
    }

    public void LogVector3(string rowName, Vector3 vector)
    {
        rows[rowName].Add(vector.x);
        rows[rowName].Add(vector.y);
        rows[rowName].Add(vector.z);
    }

    public void OverwriteEntry(string rowName, int entryIndex, float newEntry)
    {
        rows[rowName][entryIndex] = newEntry;
    }


    public void OverwriteEntry(string rowName, string label, float newEntry)
    {
        rows[rowName][labels.IndexOf(label) - 1] = newEntry;
    }

    public void OverwriteVector3(string rowName, int startIndex, Vector3 vector)
    {
        rows[rowName][startIndex] = vector.x;
        rows[rowName][startIndex + 1] = vector.y;
        rows[rowName][startIndex + 2] = vector.z;
    }

    public void OverwriteVector3(string rowName, string label, Vector3 vector)
    {
        int startIndex = labels.IndexOf(label) - 1;
        rows[rowName][startIndex] = vector.x;
        rows[rowName][startIndex + 1] = vector.y;
        rows[rowName][startIndex + 2] = vector.z;
    }

    public void AddLabels(List<string> l)
    {
        labels.AddRange(l);
    }

    public void AddLabel(string l)
    {
        labels.Add(l);
    }
}


//class used to write to normal text files
public class TextFile : LogFiles
{
    public List<string> lines;
    public TextFile(string fileName)
    : base(fileName)
    {
        this.fileName += ".txt";
        lines = new List<string>();
    }

    public override void Write()
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            for (int i = 0; i < lines.Count; i++)
            {
                writer.WriteLine(lines[i]);
            }
        }
    }
}

//Singleton used to log information
//Using a singleton so that multiple classes and objects can write to the same file.
public class Logger
{
    public static Logger instance;
    public Dictionary<string, LogFiles> logFiles;
    private Logger()
    {
        logFiles = new Dictionary<string, LogFiles>();
        //Creates LogFiles directory if it doesn't exist
        Directory.CreateDirectory("LogFiles");
    }
    public static Logger Log
    {
        get
        {

#if UNITY_SWITCH
            return null;
#endif

            if (instance == null)
            {
                instance = new Logger();
            }

            return instance;

        }
    }

    public Csv GetCsv(string name)
    {
        return logFiles[name] as Csv;
    }

    public TextFile GetTextFile(string name)
    {
        return logFiles[name] as TextFile;
    }

    public GenericCsv GetGenericCsv(string name)
    {
        return logFiles[name] as GenericCsv;
    }

    //creates and stores a reference to a csv, then returns a reference
    public Csv CreateCsv(string fileName, string logName)
    {

        Csv file = new Csv(fileName);
        logFiles[logName] = file;
        return file;
    }

    public TextFile CreateTextFile(string fileName, string logName)
    {
        TextFile file = new TextFile(fileName);
        logFiles[logName] = file;
        return file;
    }

    public GenericCsv CreateGenericCsv(string fileName, string logName)
    {
        GenericCsv file = new GenericCsv(fileName);
        logFiles[logName] = file;
        return file;
    }

    public void WriteAllLogFiles()
    {
        foreach (string k in logFiles.Keys)
        {
            logFiles[k].Write();
        }
    }
}
