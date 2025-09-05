using System;
using System.Globalization;
using UnityEngine;

public static class DateUtils
{
    public static string GetDaysAgoStringFromString(string dateString, bool useZero = false)
    {
        DateTime parsedDate;
        if (DateTime.TryParseExact(
                dateString,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDate))
        {
            return GetDaysAgoString(parsedDate);
        }
        else
        {
            Debug.LogError("Could not parse the date string.");
            return string.Empty;
        }
    }
    
    public static string GetDaysAgoString(DateTime dateTime, bool useZero = false)
    {
        TimeSpan difference = DateTime.Now - dateTime;
        int daysAgo = Math.Abs((int)Math.Floor(difference.TotalDays));
        if(daysAgo == 0 && !useZero)
            return "Today";
        
        return $"{daysAgo} days ago";
    }
}