using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempProfessionsGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var professionsTextFile = Resources.Load("JSON/professions");
        Professions professions = Professions.CreateFromJSON(professionsTextFile.ToString());
        Debug.Log(professions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
