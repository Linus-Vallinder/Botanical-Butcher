using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    public int nrOfLines;
    public float delay;
    private List<string> content = new();
    private string lineBeingWritten;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < nrOfLines; i++){
            content.Add(" ");
        }

        text.text = "";
        
        //StartCoroutine(DebugText());
    }

    // Update is called once per frame
    void Update()
    {
        buildConsole();
    }

    public void AddLine(string l) {
        content.RemoveAt(0);
        if(lineBeingWritten is not null) {
            StopCoroutine("WriteLine");
            content.RemoveAt(content.Count - 1);
            content.Add(lineBeingWritten);
        }

        StartCoroutine("WriteLine", l);
    }

    IEnumerator WriteLine(string l) {
        lineBeingWritten = l;

        content.Add(" ");
        for(int i = 1; i <= l.Length; i++) {
            content.RemoveAt(content.Count - 1);
            content.Add(l.Substring(0,i));
            yield return new WaitForSeconds(delay);
        }

        lineBeingWritten = null;
    }

    void buildConsole() {
        var builtConsole = "";
        foreach(string s in content) {
            builtConsole = $"{builtConsole}\n{s}";
        }
        text.text = builtConsole;
    }

    IEnumerator DebugText() {
        yield return new WaitForSeconds(1.0f);
        AddLine("Player starts walking...");
        yield return new WaitForSeconds(1.0f);
        AddLine("A sapling appears!");
        yield return new WaitForSeconds(5.0f);
        AddLine("Player hit sappling!");
        yield return new WaitForSeconds(1.0f);
        AddLine("12 roots collected!");
        yield return new WaitForSeconds(1.0f);
        AddLine("Player continues walking...");
    }
}
