using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;
using UnityEngine;

public interface IDroppable
{
    GameObject GetDropPrefab();  // What object is dropped?
    string GetResourceType();  // What resource type?
    int GetDropAmount();         // How much of it?
}
