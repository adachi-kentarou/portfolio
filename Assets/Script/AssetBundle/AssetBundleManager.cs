using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager
{

	/// <summary>
	/// アセットバンドルリスト
	/// </summary>
	private List<string> m_assetBundleList;

	/// <summary>
	/// アセットバンドル読み込み辞書 パス、アセットオブジェクト
	/// </summary>
	private Dictionary<string, AssetBundle> m_assetBundleLoadList;
	
	/// <summary>
	/// コンストラクタ
	/// </summary>
	private AssetBundleManager()
	{

		m_assetBundleList = new List<string>();
		m_assetBundleLoadList = new Dictionary<string, AssetBundle>();

	}

	/// <summary>
	/// シングルトン用インスタンス
	/// </summary>
	private static AssetBundleManager s_instance = null;

	/// <summary>
	/// インスタンス取得
	/// </summary>
	public static AssetBundleManager GetInstance()
	{
		if (s_instance == null)
		{
			s_instance = new AssetBundleManager();
			
		}

		return s_instance;
	}

	/// <summary>
	/// アセットバンドルロード
	/// </summary>
	/// <param name="a_path">パス</param>
	/// <param name="a_name">アセット名</param>
	/// <param name="a_callBack">ロードコールバック</param>
	public void LoadAssetBundle<T>(string a_path, string a_name, System.Action<T> a_callBack) where T : UnityEngine.Object
	{
		string t_assetBundlePath = Application.streamingAssetsPath + "/StandaloneWindows64/" + a_path;

		// アセットバンドルパスチェック
		var t_fileInfo = new System.IO.FileInfo(t_assetBundlePath);
		if (t_fileInfo.Exists == false)
		{
			Debug.LogError("asset path is not found!");
			return;
		}

		// アセットバンドル読み込み確認
		var t_isLoadComp = m_assetBundleLoadList.ContainsKey(a_path) == false;

		// 読み込み中か読み込み済確認
		if (m_assetBundleList.IndexOf(a_path) == -1 && t_isLoadComp)
		{
			// 未ロードの場合はロード処理を開始する
			var t_req = AssetBundle.LoadFromFileAsync(t_assetBundlePath);

			//AssetBundleInfo t_assetBundleInfo = new AssetBundleInfo(a_path);	

			m_assetBundleList.Add(a_path);



			//処理完了コールバック
			t_req.completed += fileOption =>
			{

				//アセットバンドルファイル読み込み完了
				T t_originAsset = t_req.assetBundle.LoadAsset<T>(a_name);
				
				// コールバック処理　ここでインスタンス化したアセットを処理
				T t_asset = GameObject.Instantiate<T>(t_originAsset);
				a_callBack(t_asset);

				// 読み込み済にする
				m_assetBundleLoadList.Add(a_path, t_req.assetBundle);
				m_assetBundleList.Remove(a_path);

			};
		}
		else
		{
			// 読み込み開始しているが未完了の場合
			if (t_isLoadComp)
			{
				Debug.LogWarning(a_path + " is already start load. but this is not completion!");
				return;
			}
			// 読み込み済の場合は辞書からオリジナルのオブジェクトを使ってインスタンス化
			T t_originAsset = (T)m_assetBundleLoadList[a_path].LoadAsset<T>(a_name);

			// コールバック処理　ここでインスタンス化したアセットを処理
			T t_asset = GameObject.Instantiate<T>(t_originAsset);
			a_callBack(t_asset);
		}
		
	}


}
