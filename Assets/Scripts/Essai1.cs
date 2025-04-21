using UnityEngine;

[CreateAssetMenu(fileName = "Essai1", menuName = "Scriptable Objects/Essai1")]
public class Essai1 : ScriptableObject
{
    public string preFabName;
    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}
