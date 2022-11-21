/**
 * アセットバンドル読み込み結果
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アセットバンドル情報クラス
/// </summary>
public class AssetBundleInfo
{
	private System.Type m_type;

	/// <summary>
	/// アセットバンドルパス
	/// </summary>
	private string m_path = string.Empty;

	
	/// <summary>
	/// アセットバンドルリクエスト
	/// </summary>
	private AssetBundleCreateRequest m_assetBundleReq = null;

	/// <summary>
	/// アセットバンドルパス取得
	/// </summary>
	/// <returns>アセットバンドルパス</returns>
	public string GetPath()
	{
		return m_path;
	}

	/// <summary>
	/// ロード完了フラグ
	/// </summary>
	private bool m_isLoad = false;

	/// <summary>
	/// 準備フラグゲッター
	/// </summary>
	/// <returns>true=準備完了 false=準備未完了</returns>
	public bool GetIsReady()
	{
		return m_isLoad;
	}

	/// <summary>
	/// アセットバンドルリクエスト取得
	/// </summary>
	/// <returns>アセットバンドルリクエスト</returns>
	public AssetBundleCreateRequest GetRequest()
	{
		return m_assetBundleReq;
	}

	public void Update()
	{
		
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="a_path">アセットバンドルパス</param>
	/// <param name="a_req">アセットバンドルリクエスト</param>
	/// <param name="a_callBack">ロードコールバック</param>
	public AssetBundleInfo(string a_path)
	{
		
		m_isLoad = false;
		

	}
	
}
