#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace YH
{
    [CustomEditor(typeof(Popup))]
    public class PopupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Popup popup = (Popup)target;
            GameObject go = popup.gameObject;

            if (GUILayout.Button("��ũ��Ʈ �ڵ� ���� �� �Ҵ�"))
            {
                string className = go.name;
                string path = $"Assets/Scripts/UI/Popup/{className}.cs";

                if (!Directory.Exists("Assets/Scripts/UI/Popup"))
                    Directory.CreateDirectory("Assets/Scripts/UI/Popup");

                if (!File.Exists(path))
                {
                    List<string> keys = TryExtractActionKeys(go);
                    File.WriteAllText(path, GetScriptTemplate(className, keys));
                    AssetDatabase.Refresh();
                    Debug.Log($"{className}.cs ���� �Ϸ�");

                    EditorApplication.delayCall += () =>
                    {
                        var scriptAsset = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                        if (scriptAsset != null)
                        {
                            MonoScript mono = scriptAsset;
                            System.Type type = mono.GetClass();
                            if (type != null)
                            {
                                Undo.AddComponent(go, type);
                                popup.scriptAttached = true;
                                Debug.Log($"{className} ��ũ��Ʈ ���ø� ���� �� �ڵ� ���� �Ϸ�");
                            }
                            else
                            {
                                Debug.LogWarning("��ũ��Ʈ�� ���� �����ϵ��� �ʾҽ��ϴ�. Unity �����͸� ���� ��ħ�ϼ���.");
                            }
                        }
                    };
                }
                else
                {
                    Debug.LogWarning($"{className}.cs �� �̹� �����մϴ�.");
                }
            }

            if (GUILayout.Button("HandleAction switch�� ����"))
            {
                List<string> keys = TryExtractActionKeys(go);
                GUIUtility.systemCopyBuffer = GenerateSwitchBlock(keys);
                Debug.Log("switch�� Ŭ�����忡 �����. ��ũ��Ʈ�� �ٿ���������.");
            }
        }

        private List<string> TryExtractActionKeys(GameObject go)
        {
            var basePopup = go.GetComponent<BasePopup>();
            if (basePopup == null) return new List<string>();

            SerializedObject so = new SerializedObject(basePopup);
            SerializedProperty entries = so.FindProperty("buttonMappings");

            List<string> keys = new();
            for (int i = 0; i < entries.arraySize; i++)
            {
                var prop = entries.GetArrayElementAtIndex(i);
                var keyProp = prop.FindPropertyRelative("actionKey");
                if (!string.IsNullOrEmpty(keyProp.stringValue))
                {
                    keys.Add(keyProp.stringValue);
                }
            }
            return keys.Distinct().ToList();
        }

        private string GenerateSwitchBlock(List<string> keys)
        {
            if (keys == null || keys.Count == 0)
                return "protected override void HandleAction(string key) { }";

            StringBuilder sb = new();
            sb.AppendLine("protected override void HandleAction(string key)");
            sb.AppendLine("{");
            sb.AppendLine("    switch (key)");
            sb.AppendLine("    {");
            foreach (var key in keys)
            {
                sb.AppendLine($"        case \"{key}\":");
                sb.AppendLine($"            Debug.Log(\"{key} Ŭ����\");");
              //  sb.AppendLine("            Close();");
                sb.AppendLine("            break;");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private string GetScriptTemplate(string className, List<string> keys)
        {
            string switchBlock = GenerateSwitchBlock(keys);

            return
$@"using UnityEngine;
using YH;

public class {className} : BasePopup
{{
    // esc�� â �ݱ� ����
    public override bool IsEscapeClosable => true;
    {switchBlock.Replace("\n", "\n    ")}
}}";
        }
    }
}
#endif
