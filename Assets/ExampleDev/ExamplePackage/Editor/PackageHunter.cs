using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace ExampleDev.ExamplePackage
{
    public static class PackageHunter
    {
        public const string VRCFuryPackageName = "com.vrcfury.vrcfury";

        public const string ReadmeGUID = "3bc3db52ac380444f96a82a85041eb0a";

        public const string PoiyomiShaderName = ".poiyomi/Poiyomi Toon";

        static ListRequest Request;
        public static bool IsPackageInstalled(string packageName)
        {
            Request = Client.List();
            while (!Request.IsCompleted) ;
            return Request.Result.Any(p => p.name == packageName);
        }

        public static bool IsFileGuidPresent(string guid)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            return !string.IsNullOrEmpty(assetPath);
        }

        public static T FetchAssetByGuid<T>(string guid) where T : Object
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        public static void PingAssetByGuid(string guid)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
            if (asset != null)
            {
                EditorGUIUtility.PingObject(asset);
            }
            else
            {
                Debug.LogError("Asset not found at path: " + assetPath);
            }
        }

        public static bool IsShaderPresent(string ShaderName)
        {
            Shader shader = Shader.Find(ShaderName);
            return (shader != null);
        }
    }
}