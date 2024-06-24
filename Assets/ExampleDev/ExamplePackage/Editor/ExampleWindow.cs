#if UNITY_EDITOR
using Mono.Cecil;
using Serilog.Parsing;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace ExampleDev.ExamplePackage
{
    public class ExampleWindow : EditorWindow
    {
        /// <summary>
        /// Package Logo
        /// </summary>
        Texture Logo;

        Texture SmallIcon;

        public bool VRCFuryInstalled = true;

        public bool ReadMeAssetReady = true;

        public bool ShaderReady = true;

        /// <summary>
        /// Window's vertical scroll position
        /// </summary>
        Vector2 windowScrollPosition = Vector2.zero;

        #region Styles
        GUIStyle headStyle;
        GUIStyle instructionsCentreStyle;
        GUIStyle instructionsStyle;
        GUIStyle headingStyle;
        GUIStyle descStyle;
        #endregion
        /// <summary>
        /// Pre-loads graphics
        /// </summary>
        private void OnEnable()
        {
            Logo = Resources.Load<Texture>("Unknown256");
            SmallIcon = Resources.Load<Texture>("Unknown32");
        }

        void OnGUI()
        {
            setupStyles();

            windowScrollPosition = GUILayout.BeginScrollView(windowScrollPosition);
            var defaultColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.clear;
            var headContent = new GUIContent("Example Package", Logo);
            GUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                UIHelpers.BeginCenter();
                    GUILayout.Box(Logo, GUILayout.Width(160), GUILayout.Height(160));
                UIHelpers.EndCenter();
                UIHelpers.BeginCenter();
                    GUILayout.Label(headContent, headStyle);
                UIHelpers.EndCenter();
                UIHelpers.BeginCenter();
                    GUILayout.Label("vs. " + ExamplePackageCore.Version.ToString(), descStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));
                UIHelpers.EndCenter();

                GUI.backgroundColor = defaultColor;
                UIHelpers.DrawUILine(GUI.color);

                GUILayout.FlexibleSpace();

                GUILayout.Label("Welcome message about the package here", instructionsStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));
                GUILayout.Label("Click a button below to get started or you can come back later using the menu bar under:", instructionsStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));
                GUILayout.Label("ExampleDev > Example Package > About", instructionsCentreStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));

                GUILayout.FlexibleSpace();
                UIHelpers.DrawUILine(GUI.color);

                GUILayout.Label("Section 1 Heading", headingStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));

                if (VRCFuryInstalled)
                {
                    UIHelpers.BeginCenter();
                        GUILayout.Label(new GUIContent("VRC Fury is installed and ready to use.", UIHelpers.InfoIcon), instructionsCentreStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));
                    UIHelpers.EndCenter();
                }
                else
                {
                    EditorGUILayout.HelpBox("VRC Fury is not currently installed! You'll need to install it first using the Creator Companion!", MessageType.Warning);
                    UIHelpers.BeginCenter();
                    if (GUILayout.Button(new GUIContent("      Click here and follow the instructions to install VRC Fury", Logo), GUILayout.Height(48), GUILayout.MaxWidth(UIHelpers.AboutWindowWidth)))
                    {
                        Application.OpenURL(UIHelpers.VRCFuryDownloadURL);
                    }
                    UIHelpers.EndCenter();
                }

                GUILayout.Label("Section 2 Heading", headingStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));

                if (ReadMeAssetReady)
                {
                    UIHelpers.BeginCenter();
                        if (GUILayout.Button(new GUIContent("      Open the README file", SmallIcon), GUILayout.Height(32), GUILayout.MaxWidth(UIHelpers.AboutWindowWidth)))
                        {
                            ExamplePackageCore.OpenSupportFile(PackageHunter.ReadmeGUID);
                        }
                    UIHelpers.EndCenter();
                }
                else
                {
                    EditorGUILayout.HelpBox("Oh dear! I can't find the file! You may need to reinstall this package!", MessageType.Error);
                }

                GUILayout.Label("Section 3 Heading", headingStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));

                if (ShaderReady)
                {
                    UIHelpers.BeginCenter();
                        GUILayout.Label(new GUIContent("Poiyomi is installed and ready to use.", UIHelpers.ShaderIcon), instructionsCentreStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));
                    UIHelpers.EndCenter();
                }
                else
                {
                    EditorGUILayout.HelpBox("Poiyomi is not currently installed! You'll need to install it first using the Creator Companion!", MessageType.Warning);
                }

                GUILayout.FlexibleSpace();

                UIHelpers.BeginCenter();
                    if (GUILayout.Button(new GUIContent("       Go to the product website", SmallIcon, UIHelpers.PackageURL), GUILayout.Height(32), GUILayout.MaxWidth(UIHelpers.AboutWindowWidth)))
                    {
                        Application.OpenURL(UIHelpers.PackageURL);
                    }
                UIHelpers.EndCenter();
                UIHelpers.DrawUILine(GUI.color);

                GUILayout.BeginVertical();
                    UIHelpers.BeginCenter();
                        GUILayout.Label("Developed by", descStyle, GUILayout.MaxWidth(UIHelpers.AboutWindowWidth));
                    UIHelpers.EndCenter();
                    UIHelpers.BeginCenter();
                        if (GUILayout.Button(new GUIContent("      Dev Name", SmallIcon, UIHelpers.ProfileURL), GUILayout.Height(48), GUILayout.MaxWidth(UIHelpers.AboutWindowWidth)))
                        {
                            Application.OpenURL(UIHelpers.ProfileURL);
                        }
                    UIHelpers.EndCenter();
                    GUILayout.Space(20f);
                GUILayout.EndVertical();

                //Startup Option
                EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUI.BeginChangeCheck();
                        bool showStartup = ExamplePackageCore.AlwaysShow;
                        float originalValue = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = 240;
                        showStartup = !EditorGUILayout.Toggle("Don't show this window when Unity starts", !showStartup);
                        EditorGUIUtility.labelWidth = originalValue;
                    if (EditorGUI.EndChangeCheck())
                    {
                        ExamplePackageCore.AlwaysShow = showStartup;
                    }
                EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        void setupStyles()
        {
            headStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                fontSize = 24,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 35f,
                imagePosition = ImagePosition.TextOnly
            };
            instructionsCentreStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                padding = new RectOffset(5, 5, 2, 3),
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fixedHeight = 32f,
                imagePosition = ImagePosition.ImageLeft
            };
            instructionsStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                padding = new RectOffset(15, 5, 2, 3),
                alignment = TextAnchor.MiddleLeft,
            };
            headingStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                padding = new RectOffset(10, 5, 2, 3),
                alignment = TextAnchor.LowerCenter,
                fontStyle = FontStyle.Bold,
            };
            descStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                padding = new RectOffset(2, 2, 2, 2),
                alignment = TextAnchor.MiddleCenter,
            };
        }
    }
}
#endif