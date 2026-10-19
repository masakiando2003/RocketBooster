using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "Localization",
        menuName = "Localization"
    )
]
public class Localization : ScriptableObject
{
    public List<string> labelName, labelText;

    // ラベル名→テキストの辞書(初回アクセス時に構築する実行時キャッシュ)
    private Dictionary<string, string> lookup;

    private void OnEnable()
    {
        // 再生/ドメインリロード時にキャッシュを作り直す
        lookup = null;
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<string, string>();
        if (labelName == null || labelText == null) { return; }
        int count = Mathf.Min(labelName.Count, labelText.Count);
        for (int i = 0; i < count; i++)
        {
            // 先勝ち(従来の FindIndex と同じ挙動)。重複キーは無視。
            if (labelName[i] != null && !lookup.ContainsKey(labelName[i]))
            {
                lookup[labelName[i]] = labelText[i];
            }
        }
    }

    public string GetLabelContent(string specifiedLabel)
    {
        if (lookup == null) { BuildLookup(); }
        if (lookup.TryGetValue(specifiedLabel, out string content))
        {
            return content;
        }
#if UNITY_EDITOR
        // 未定義ラベルは例外で落とさず、キー名をそのまま返す(表示が崩れても停止しない)
        Debug.LogWarning($"[Localization] Label '{specifiedLabel}' not found in {name}; returning the key as fallback.");
#endif
        return specifiedLabel;
    }
}
