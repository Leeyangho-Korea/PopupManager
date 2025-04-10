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

            if (GUILayout.Button("스크립트 자동 생성 및 할당"))
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
                    Debug.Log($"{className}.cs 생성 완료");

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
                                Debug.Log($"{className} 스크립트 템플릿 생성 및 자동 부착 완료");
                            }
                            else
                            {
                                Debug.LogWarning("스크립트가 아직 컴파일되지 않았습니다. Unity 에디터를 새로 고침하세요.");
                            }
                        }
                    };
                }
                else
                {
                    Debug.LogWarning($"{className}.cs 는 이미 존재합니다.");
                }
            }

            if (GUILayout.Button("HandleAction switch문 복사"))
            {
                List<string> keys = TryExtractActionKeys(go);
                GUIUtility.systemCopyBuffer = GenerateSwitchBlock(keys);
                Debug.Log("switch문 클립보드에 복사됨. 스크립트에 붙여넣으세요.");
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
                sb.AppendLine($"            Debug.Log(\"{key} 클릭됨\");");
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
    // esc로 창 닫기 여부
    public override bool IsEscapeClosable => true;
    {switchBlock.Replace("\n", "\n    ")}
}}";
        }
    }
}
#endif
