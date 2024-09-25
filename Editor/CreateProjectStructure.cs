using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

namespace com.ajc.projectcreator
{
    public class CreateProjectStructure : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;
        private static string _projectName;
        private Label _errorLogMessage; 


        [MenuItem("Tools/Project Creator")]
        public static void ShowExample()
        {
            CreateProjectStructure wnd = GetWindow<CreateProjectStructure>();
            wnd.titleContent = new GUIContent("Project Creator");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            VisualElement uiBuilderElements = m_VisualTreeAsset.Instantiate();
            var projectNameInputField = uiBuilderElements.Q<TextField>();
            projectNameInputField.RegisterValueChangedCallback(OnNameSelection);
            uiBuilderElements.Q<Button>().clicked += CreateProject;

            _errorLogMessage = new Label();
            root.Add(uiBuilderElements);
            root.Add(_errorLogMessage);

            var splitPanel = new VisualElement();
            var splitPanelLeft = new VisualElement();
            splitPanelLeft.Add(new Label("Left"));

            var splitPanelRight = new VisualElement();
            splitPanelRight.Add(new Label("Right"));

            splitPanel.style.justifyContent = Justify.SpaceEvenly;
            splitPanel.style.flexDirection = FlexDirection.Row;
            splitPanel.Add(splitPanelLeft);

            splitPanel.Add(splitPanelRight);

            root.Add(splitPanel);

        }

        private void CreateProject()
        {
            _errorLogMessage.text = "";
            try
            {

                if (string.IsNullOrEmpty(_projectName))
                {
                    throw new NullReferenceException("Project Name is Empty");
                }
            }
            catch (Exception e) {
            
                _errorLogMessage.text = e.Message;
        
            }
        
        }

        private static void CreateAllFolders()
        {
            List<string> folders = new List<string>
             {
             "Animations",
             "Audio",
             "Editor",
             "Materials",
             "Meshes",
             "Prefabs",
             "Scripts",
             "Scenes",
             "Shaders",
             "Textures",
             "UI"
             };
            foreach (string folder in folders)
            {
                if (!Directory.Exists("Assets/" + folder))
                {
                    Directory.CreateDirectory("Assets/" + _projectName + "/" + folder);
                    File.Create("Assets/" + _projectName + "/" + folder+"/.gitKeep");
                }
            }
            List<string> uiFolders = new List<string>
             {
             "Assets",
             "Fonts",
             "Icon"
             };
            foreach (string subfolder in uiFolders)
            {
                if (!Directory.Exists("Assets/" + _projectName + "/UI/" + subfolder))
                {
                    Directory.CreateDirectory("Assets/" + _projectName + "/UI/" + subfolder);
                    File.Create("Assets/" + _projectName + "/" + subfolder + "/.gitKeep");
                }
            }
            AssetDatabase.Refresh();
        }

        private void OnNameSelection(ChangeEvent<string> evt)
        {
            _projectName = evt.newValue;
        }
    }
}
