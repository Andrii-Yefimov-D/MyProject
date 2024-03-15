using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MyCustomUIToolkitEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    
    [SerializeField]
    private VisualTreeAsset m_UXMLTree;
    
    private int m_ClickCount = 0;
    private const string m_ButtonPrefix = "button";

    [MenuItem("Window/UI Toolkit/MyCustomUIToolkitEditor")]
    public static void ShowExample()
    {
        MyCustomUIToolkitEditor wnd = GetWindow<MyCustomUIToolkitEditor>();
        wnd.titleContent = new GUIContent("MyCustomUIToolkitEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElements following a tree hierarchy.
        Label label = new Label("These controls were created using C# code.");
        root.Add(label);

        Button button = new Button();
        button.name = "button3";
        button.text = "This is button3.";
        root.Add(button);

        Toggle toggle = new Toggle();
        toggle.name = "toggle3";
        toggle.label = "Number?";
        root.Add(toggle);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/MyCustomUIToolkitEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
        
        root.Add(m_UXMLTree.Instantiate());
        
        //Call the event handler
        SetupButtonHandler();
    }
    
    //Functions as the event handlers for your button click and number counts 
    private void SetupButtonHandler()
    {
        VisualElement root = rootVisualElement;

        var buttons = root.Query<Button>();
        buttons.ForEach(RegisterHandler);
    }

    private void RegisterHandler(Button button)
    {
        button.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        VisualElement root = rootVisualElement;

        ++m_ClickCount;

        //Because of the names we gave the buttons and toggles, we can use the
        //button name to find the toggle name.
        Button button = evt.currentTarget as Button;
        string buttonNumber = button.name.Substring(m_ButtonPrefix.Length);
        string toggleName = "toggle" + buttonNumber;
        Toggle toggle = root.Q<Toggle>(toggleName);

        Debug.Log($"Button <{button.name}> was clicked!" +
            (toggle.value ? " Count: " + m_ClickCount : ""));
    }
}
