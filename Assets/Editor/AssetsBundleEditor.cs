/**
 * アセットバンドル作成エディタ拡張処理
 */

using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Editor拡張　AssetBundleメニュー追加
/// </summary>
public class AssetBundleEditor
{
	/// <summary>
	/// アセットバンドルビルド対象ルートディレクトリ
	/// </summary>
	private static readonly string rootPath = "AssetBundles";
    private static readonly string variant = "assetbundle";
	
	private static readonly string assetBundleRootDir = @"Assets\AssetBundle";
	/// <summary>
	/// アセットバンドルファイル命名規則正規表現
	/// </summary>
	private static readonly string assetBundleFileMatchReg = @"[^\./]+(@bundle\.)[^\./(meta)]+$";

	/// <summary>
	/// メタファイル命名規則正規表現
	/// </summary>
	private static readonly string metaFileMatchReg = @".meta$";

	/// <summary>
	/// アセットバンドルディレクトリ命名規則正規表現
	/// </summary>
	private static readonly string assetBundleDirMatchReg = @"[^\./]+(@bundle)$";

	/// <summary>
	/// アセットバンドルファイル名作成用置き換え正規表現
	/// </summary>
	private static readonly string assetBundleFileChageReg = @"(@bundle\.)[^\./]+$";

	/// <summary>
	/// アセットバンドルディレクトリ名作成用置き換え正規表現
	/// </summary>
	private static readonly string assetBundleDirChageReg = @"(@bundle)$";

	/// <summary>
	/// アセットバンドルパス名置き換え正規表現
	/// </summary>
	private static readonly string assetBundlePathChageReg = @"(@bundle).+$";

	/// <summary>
	/// アセットバンドルパス名置き換え名前
	/// </summary>
	private static readonly string assetBundlePathChageName = @".assetbundle";
	
	/// <summary>
	/// コピー元アセットバンドルパス名
	/// </summary>
	private static readonly string originAssetBundleDirPath = @"./AssetBundles";

	/// <summary>
	/// コピー元アセットバンドルパス名
	/// </summary>
	private static readonly string copyAssetBundleDirPath = @"./Assets/StreamingAssets";

	/// <summary>
	/// アセットバンドルビルド
	/// </summary>
	[MenuItem("AssetBundle / Build", false)]
	static private void BuildAssetBundles()
	{
		// ビルド前にディレクトリ内を削除
		var t_desDir = new DirectoryInfo(originAssetBundleDirPath);
		foreach (DirectoryInfo t_dir in t_desDir.GetDirectories())
		{
			Debug.Log(t_dir.Name);
			t_dir.Delete(true);
		}

		UnityEditor.BuildTarget t_targetPlatform = UnityEditor.BuildTarget.StandaloneWindows64;

		// ビルド出力パス
		var t_outputPath = System.IO.Path.Combine(rootPath, t_targetPlatform.ToString());

		// ディレクトリが存在しない場合は新規に作成する
		if (System.IO.Directory.Exists(t_outputPath) == false)
		{
			System.IO.Directory.CreateDirectory(t_outputPath);
		}
		
		var t_assetBundleBuildList = new List<UnityEditor.AssetBundleBuild>();
		
		// アセットバンドルビルド対象収集
		foreach (string t_assetBundleName in UnityEditor.AssetDatabase.GetAllAssetBundleNames())
		{
			var t_builder = new AssetBundleBuild();
			t_builder.assetBundleName = t_assetBundleName;
			
			t_builder.assetNames = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(t_builder.assetBundleName);
			t_builder.assetBundleVariant = variant;
			t_assetBundleBuildList.Add(t_builder);
		}
		
		// ビルド対象がある場合はビルドする
		if (t_assetBundleBuildList.Count > 0)
		{
			UnityEditor.BuildPipeline.BuildAssetBundles(
				t_outputPath,
				t_assetBundleBuildList.ToArray(),
				UnityEditor.BuildAssetBundleOptions.ChunkBasedCompression,
				t_targetPlatform
			);
		}

		// 終了ダイアログ
		EditorUtility.DisplayDialog("ビルド終了","アセットバンドルビルド終了","OK");
	}

	/// <summary>
	/// アセットバンドル名前設定
	/// </summary>
	[MenuItem("AssetBundle / AssetBundles name setting", false)]
	static private void SettingAssetBundleNames()
	{
		UnityEditor.AssetDatabase.RemoveUnusedAssetBundleNames();
		ChangeAssetName(@"", true);

		// 終了ダイアログ
		EditorUtility.DisplayDialog("修正終了", "アセットバンドル名修正終了", "OK");
	}

	/// <summary>
	/// アセットバンドル名変更
	/// </summary>
	/// <param name="a_sourceDir">アッセとバンドルルートディレクトリパス</param>
	/// <param name="a_recursive">ディレクトリ階層再帰処理フラグ</param>
	private static void ChangeAssetName(string a_sourceDir, bool a_recursive)
	{
		// Assetsディレクトリ内を対象とする
		var t_path = Path.Combine(assetBundleRootDir, a_sourceDir);
		
		// ディレクトリ情報取得
		var dir = new DirectoryInfo(t_path);
		
		// アセットバンドル名変更
		foreach (FileInfo t_file in dir.GetFiles())
		{
			// metaファイルは除外
			var t_isMetaFileMatch = Regex.IsMatch(t_file.Name, metaFileMatchReg);
			if (t_isMetaFileMatch == true)
			{
				continue;
			}

			string t_targetFilePath = Path.Combine(t_path, t_file.Name);
			
			AssetImporter t_assetImp = AssetImporter.GetAtPath(t_targetFilePath);

			// ファイル名の末尾に@bundleがついているものをアセットバンドル化対象とする
			var t_isMatch = Regex.IsMatch(t_file.Name, assetBundleFileMatchReg);

			if (t_isMatch == false)
			{
				// アセットバンドル名前を削除
				if (t_assetImp != null)
				{
					t_assetImp.assetBundleName = string.Empty;
					t_assetImp.SaveAndReimport();
				}
				// 一致しない場合は飛ばす
				continue;
			}
			
			Regex t_reg = new System.Text.RegularExpressions.Regex(assetBundleFileChageReg);
			string t_newName = t_reg.Replace(t_targetFilePath, string.Empty);

			// アセットバンドル名前を変更する
			t_assetImp.assetBundleName = Path.Combine(a_sourceDir, t_newName);
			t_assetImp.SaveAndReimport();
			
		}

		// 子のディレクトリ再帰処理
		if (a_recursive)
		{
			// 内部のディレクトリ取得
			DirectoryInfo[] t_dirs = dir.GetDirectories();

			foreach (DirectoryInfo t_subDir in t_dirs)
			{
				string dirName = t_subDir.Name;

				// ディレクトリ名の末尾に@bundleがついているものは飛ばす
				string t_assetDirName = Path.Combine(t_path, dirName);
				var t_isMatch = Regex.IsMatch(dirName, assetBundleDirMatchReg);
				AssetImporter t_assetImp = AssetImporter.GetAtPath(t_assetDirName);

				if (t_isMatch == true)
				{
					Regex t_reg = new System.Text.RegularExpressions.Regex(assetBundleDirChageReg);
					string t_newName = t_reg.Replace(t_assetDirName, string.Empty);

					// 一致したのでアセットバンドル名を変更して飛ばす
					t_assetImp.assetBundleName = Path.Combine(a_sourceDir, t_newName);
					t_assetImp.SaveAndReimport();
					
					continue;
				}
				else
				{
					// 一致しなかった場合は名前を削除
					t_assetImp.assetBundleName = string.Empty;
					t_assetImp.SaveAndReimport();
				}

				string t_newDestinationDir = Path.Combine(a_sourceDir, dirName);
				ChangeAssetName(t_newDestinationDir, true);
			}
		}
	}

	/// <summary>
	/// アセットバンドルディレクトリーコピー
	/// </summary>
	[MenuItem("AssetBundle / Copy AssetBundles to StreamingAssets", false)]
	static private void CopyAssetBundles()
	{
		// コピー前にコピー先ディレクトリ内を削除
		var t_desDir = new DirectoryInfo(copyAssetBundleDirPath);
		foreach (FileInfo t_file in t_desDir.GetFiles())
		{
			t_file.Delete();
		}



		CopyDirectory(originAssetBundleDirPath, copyAssetBundleDirPath, true);
		
		// 終了ダイアログ
		EditorUtility.DisplayDialog("複製終了", "アセットバンドル複製終了", "OK");
	}

	/// <summary>
	/// ディレクトリコピー
	/// </summary>
	/// <param name="a_sourceDir">コピー元ディレクトリパス</param>
	/// <param name="a_destinationDir">コピー先ディレクトリパス</param>
	/// <param name="a_recursive">ディレクトリ階層再帰処理フラグ</param>
	private static void CopyDirectory(string a_sourceDir, string a_destinationDir, bool a_recursive)
	{
		// ディレクトリ情報取得
		var t_dir = new DirectoryInfo(a_sourceDir);

		var t_desDir = new DirectoryInfo(a_destinationDir);

		// コード節 コピー元ディレクトリ存在確認
		if (!t_dir.Exists)
		{
			Debug.Log("Copy directory not found");
			return;
		}
		
		// コピー先ディレクトリ作成
		Directory.CreateDirectory(a_destinationDir);

		// コピー先ファイル削除
		foreach (FileInfo t_file in t_desDir.GetFiles())
		{
			t_file.Delete();
		}

		// ファイルコピー
		foreach (FileInfo t_file in t_dir.GetFiles())
		{
			string t_targetFilePath = Path.Combine(a_destinationDir, t_file.Name);
			t_file.CopyTo(t_targetFilePath);
		}

		// 子のディレクトリ再帰処理
		if (a_recursive)
		{
			// 内部のディレクトリ取得
			DirectoryInfo[] t_dirs = t_dir.GetDirectories();

			foreach (DirectoryInfo t_subDir in t_dirs)
			{
				string t_newDestinationDir = Path.Combine(a_destinationDir, t_subDir.Name);
				CopyDirectory(t_subDir.FullName, t_newDestinationDir, true);
			}
		}
	}

	/// <summary>
	/// 選択ファイルのアセットバンドルパスソースコードをコピー
	/// </summary>
	/// <remarks>
	/// Assets/xxxx/yyyy@bundle/fine.png の場合
	/// 
	/// "assets/xxxx/yyyy.assetbundle","assets/xxxx/yyyy@bundle/fine.png",\n
	/// 
	/// </remarks>
	[MenuItem("Assets/AssetBundle/Copy AssetBundles Path")]
	static private void CopyAssetBundlePath()
	{
		
		UnityEngine.Object[] t_fileList =  Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Unfiltered);

		string t_assetBundlePaths = string.Empty;

		foreach (var t_obj in t_fileList)
		{
			string t_path = AssetDatabase.GetAssetPath(t_obj.GetInstanceID()).ToLower();
			Regex t_reg = new System.Text.RegularExpressions.Regex(assetBundlePathChageReg);
			string t_newName = t_reg.Replace(t_path, assetBundlePathChageName);

			t_assetBundlePaths += string.Format("\"{0}\",\"{1}\",\n", t_newName,t_path);
			
		}
		
		if (t_assetBundlePaths != string.Empty)
		{
			// クリップボードにソースコードをコピーする
			GUIUtility.systemCopyBuffer = t_assetBundlePaths;
			
			// 終了ダイアログ
			EditorUtility.DisplayDialog("ソースコードコピー", string.Format("コピー完了\n{0}",t_assetBundlePaths), "OK");
		}
		else
		{

		}
	}
}