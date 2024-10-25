using System;
using System.Collections.Generic;

public static class TempDataStorage
{
    // Dictionary to store data
    private static readonly Dictionary<string, object> _data = new Dictionary<string, object>();

    // Method to set data
    public static void SetData<T>(string key, T value)
    {
        if (_data.ContainsKey(key))
        {
            _data[key] = value; // If the key already exists, update the value
        }
        else
        {
            _data.Add(key, value); // If the key does not exist, add a new entry
        }
    }

    // Method to get data
    public static T GetData<T>(string key)
    {
        if (_data.TryGetValue(key, out object value))
        {
            if (value is T typedValue)
            {
                return typedValue; // Return the value if it exists and is of the correct type
            }
            else
            {
                throw new InvalidCastException($"Stored value is not of type {typeof(T)}.");
            }
        }
        else
        {
            throw new KeyNotFoundException($"Key '{key}' not found.");
        }
    }

    // Method to remove data
    public static void RemoveData(string key)
    {
        if (_data.ContainsKey(key))
        {
            _data.Remove(key); // Remove data by key
        }
    }

    // Method to clear all data
    public static void ClearAllData()
    {
        _data.Clear(); // Clear the entire dictionary
    }
}