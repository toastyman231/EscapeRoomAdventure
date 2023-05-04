using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class TangramSolutionGenerator : EditorWindow
{
    private VisualElement _root;

    [MenuItem("Window/UI Toolkit/TangramSolutionGenerator")]
    public static void ShowExample()
    {
        TangramSolutionGenerator wnd = GetWindow<TangramSolutionGenerator>();
        wnd.titleContent = new GUIContent("TangramSolutionGenerator");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        _root = root;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/UI/TangramSolutionGenerator.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        root.Q<Button>("GenerateButton").clicked += GenerateTangramSolution;
    }

    private void GenerateTangramSolution()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Cannot generate solution: Game is not playing!");
            return;
        }

        string solutionName = _root.Q<TextField>("NameField").value;

        Transform bigTriangle1 = Tangram.bigTriangle1.transform;
        Transform bigTriangle2 = Tangram.bigTriangle2.transform;
        Transform mediumTriangle = Tangram.mediumTriangle.transform;
        Transform smallTriangle1 = Tangram.smallTriangle1.transform;
        Transform smallTriangle2 = Tangram.smallTriangle2.transform;
        Transform square = Tangram.square.transform;
        Transform rhombus = Tangram.rhombus.transform;

        TangramSolution generatedSolution = CreateInstance<TangramSolution>();
        generatedSolution.BigTriangle1Position = bigTriangle1.position;
        generatedSolution.BigTriangle2Position = bigTriangle2.position;
        generatedSolution.MediumTrianglePosition = mediumTriangle.position;
        generatedSolution.SmallTriangle1Position = smallTriangle1.position;
        generatedSolution.SmallTriangle2Position = smallTriangle2.position;
        generatedSolution.SquarePosition = square.position;
        generatedSolution.RhombusPosition = rhombus.position;

        generatedSolution.BigTriangle1Rotation = bigTriangle1.localEulerAngles;
        generatedSolution.BigTriangle2Rotation = bigTriangle2.localEulerAngles;
        generatedSolution.MediumTriangleRotation = mediumTriangle.localEulerAngles;
        generatedSolution.SmallTriangle1Rotation = smallTriangle1.localEulerAngles;
        generatedSolution.SmallTriangle2Rotation = smallTriangle2.localEulerAngles;
        generatedSolution.SquareRotation = square.localEulerAngles;
        generatedSolution.RhombusRotation = rhombus.localEulerAngles;

        AssetDatabase.CreateAsset(generatedSolution, "Assets/Resources/Tangram Solutions/" + solutionName + ".asset");
    }
}