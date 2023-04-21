using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Paper Text", menuName = "Paper Text")]
public class PaperText : ScriptableObject
{
    [TextArea(15, 20)]
    public string text;
}
