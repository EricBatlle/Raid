using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class TestWriteFile : MonoBehaviour
{
    [SerializeField] List<DateTime> dates = new List<DateTime>();
    [SerializeField] string text = null;
    string formatSpecifier = "d";
    CultureInfo culture = CultureInfo.CreateSpecificCulture("es-ES");

    private void Start()
    {        
        for (int i = 10; i <= 12; i++)
        {
            GetDates(2018,i);
        }

        WriteString(text);
    }
    static void WriteString(string newString)
    {
        string path = "Assets/Resources/test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(newString);
        writer.Close();

        //Re-import the file to update the reference in the editor
        TextAsset asset = (TextAsset)Resources.Load("test");
    }

    public List<DateTime> GetDates(int year, int month)
    {
        // Loop from the first day of the month until we hit the next month, moving forward a day at a time
        for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
        {
            dates.Add(date);
            if(date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                text += date.ToString(formatSpecifier, culture) + "\r\n";
        }

        return dates;
    }
}
