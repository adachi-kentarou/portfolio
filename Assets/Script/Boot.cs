/**
 * アプリ起動時各初期化 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 起動実行クラス
/// </summary>
public class Boot : MonoBehaviour
{
	// プール名
	private readonly string m_playerPoolName = "Player";
	private readonly string m_playerBulletPoolName = "PlayerBullet";
	private readonly string m_enemyPoolName = "Enemy";

	// Start is called before the first frame update
	void Start()
    {
		
		StartCoroutine("BootCoroutine");
		

    }

    // Update is called once per frame
    void Update()
    {
		if (m_isReady == false) return;

		//イベントドリブン起点
		Engine.GameEngine.GetInstance().Update();
	}

	/// <summary>
	/// 初期化準備完了フラグ
	/// </summary>
	private bool m_isReady = false;


	/// <summary>
	/// 初期化用コルーチン
	/// </summary>
	/// <returns></returns>
	IEnumerator BootCoroutine()
	{

		bool t_isPlayerLoad = false;
		/**
		 * ここでアセットバンドルのロードを行っている
		 * 引数に使用しているパスはエディタ拡張により追加した機能で
		 * ファイルのコンテキストメニューから
		 * AssetBundle→Copy AssetBundles Path
		 * でクリップボードにコードをコピーして貼り付けている
		 */
		AssetBundleManager.GetInstance().LoadAssetBundle<GameObject>("assets/assetbundle/character.assetbundle", "assets/assetbundle/character@bundle/player.prefab",
		(res) => {
			// プレイヤーのプールを作成
			Pool.PoolManager.GetInstance().CreatePool(m_playerPoolName, res, 1, Pool.CanvasType.Top);
			t_isPlayerLoad = true;
		});

		if (t_isPlayerLoad == false) yield return null;


		bool t_isBulletLoad = false;
		AssetBundleManager.GetInstance().LoadAssetBundle<GameObject>("assets/assetbundle/bullet.assetbundle", "assets/assetbundle/bullet@bundle/playerbullet.prefab",
		(res) => {
			// 弾のプールを作成
			Pool.PoolManager.GetInstance().CreatePool(m_playerBulletPoolName, res, 20, Pool.CanvasType.Middle);
			t_isBulletLoad = true;
		});

		if (t_isBulletLoad == false) yield return null;

		bool t_isEnemyLoad = false;
		AssetBundleManager.GetInstance().LoadAssetBundle<GameObject>("assets/assetbundle/character.assetbundle", "assets/assetbundle/character@bundle/enemy.prefab",
		(res) => {
			// 弾のプールを作成
			Pool.PoolManager.GetInstance().CreatePool(m_enemyPoolName, res, 10, Pool.CanvasType.Middle);
			t_isEnemyLoad = true;
		});

		if (t_isEnemyLoad == false) yield return null;

		
		// 初期化準備完了
		m_isReady = true;

		yield return null;
	}
}
