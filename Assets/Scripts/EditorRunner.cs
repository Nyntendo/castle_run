using UnityEngine;
using System.Collections;

public class EditorRunner : MonoBehaviour
{
    public void Start()
    {
        var matchController = GameObject.FindWithTag("MatchController");
        if (matchController == null)
            Application.LoadLevel(0);
    }
}
