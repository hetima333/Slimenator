using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MyAllPostprocessor : AssetPostprocessor {
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {

		return;

		// 指定したフォルダにプレハブが含まれていなければ無視
		if (!importedAssets.Any (c => c.Contains ("Assets/Prefabs"))) return;

		// 含まれている場合は
		// まず ScriptableObjectを読み込んでおく
		var path = "Assets/ScriptableObject/Holder/Target/Targettable.asset";
		var targetable = AssetDatabase.LoadAssetAtPath<GameObjectList> (path);

		//var hoge = importedAssets
		///	.Where (c => c.Contains ("「プレハブのパス」"))
		//	.Select (c => AssetDatabase.LoadAssetAtPath<GameObject> (c))
		//	.ToList ();

		// 特定のフォルダ内のプレハブをすべて取得する
		var fuga = Directory
			.GetFiles ("Assets/Prefabs", "*.prefab", SearchOption.AllDirectories)
			.Select (c => AssetDatabase.LoadAssetAtPath<GameObject> (c))
			.Where (c => c != null)
			.ToList ();

		// リストに設定して
		targetable._List = fuga;

		// Unity に変更しましたよと通知する
		EditorUtility.SetDirty (targetable);
	}
}