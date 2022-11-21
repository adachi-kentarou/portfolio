/** 
 * ゲームエンジン
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Engine 
{
	public class GameEngine
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		private GameEngine()
		{
			//AssetBundleManager.GetInstance();
			Pool.PoolManager.GetInstance();
		}

		/// <summary>
		/// シングルトン用インスタンス
		/// </summary>
		private static GameEngine s_instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		public static GameEngine GetInstance()
		{
			if (s_instance == null)
			{
				s_instance = new GameEngine();

			}

			return s_instance;
		}

		/// <summary>
		/// 状態
		/// </summary>
		private Engine.GameEngineState m_state = Engine.GameEngineState.Init;

		/// <summary>
		/// プレイヤーインスタンス
		/// </summary>
		private Character.Player m_player;

		/// <summary>
		/// プレイヤーショットキー入力間隔
		/// </summary>
		[SerializeField]
		private float m_playerShotInputInterval = 0.2f;

		/// <summary>
		/// プレイヤー移動キー入力間隔
		/// </summary>
		[SerializeField]
		private float m_playerMoveInputInterval = 0.0f;

		/// <summary>
		/// プレイヤープール名
		/// </summary>
		private readonly string m_playerPoolName = "Player";

		/// <summary>
		/// 敵プール名
		/// </summary>
		private readonly string m_enemyPoolName = "Enemy";

		/// <summary>
		/// 敵出現間隔
		/// </summary>
		[SerializeField]
		private float m_enemySpawnInterval = 2f;

		/// <summary>
		/// 現在出現間隔時間
		/// </summary>
		private float m_nowEnemyInterval;

		/// <summary>
		/// 更新
		/// </summary>
		// Update is called once per frame
		public void Update()
		{
			switch (m_state)
			{
				case GameEngineState.Init:
					// プレイヤーインスタンス初期化
					m_player = (Character.Player)Pool.PoolManager.GetInstance().RequestObject(m_playerPoolName);
					m_player.Init(Vector3.zero, Vector3.one * 0.5f, Quaternion.identity);

					// キー入力設定
					// 上
					Input.InputParam param = new Input.InputParam(
						KeyCode.UpArrow,
						Input.InputType.Interval,
						m_playerMoveInputInterval,
						m_player.InputUp
						);

					Input.InputManager.GetInstance().AddInput(param);

					// 下
					param = new Input.InputParam(
						KeyCode.DownArrow,
						Input.InputType.Interval,
						m_playerMoveInputInterval,
						m_player.InputDown
						);

					Input.InputManager.GetInstance().AddInput(param);

					// 右
					param = new Input.InputParam(
						KeyCode.RightArrow,
						Input.InputType.Interval,
						m_playerMoveInputInterval,
						m_player.InputRight
						);

					Input.InputManager.GetInstance().AddInput(param);

					// 左
					param = new Input.InputParam(
						KeyCode.LeftArrow,
						Input.InputType.Interval,
						m_playerMoveInputInterval,
						m_player.InputLeft
						);

					Input.InputManager.GetInstance().AddInput(param);

					param = new Input.InputParam(
						KeyCode.A,
						Input.InputType.Interval,
						m_playerShotInputInterval,
						m_player.InputA
						);

					Input.InputManager.GetInstance().AddInput(param);

					m_nowEnemyInterval = 0f;

					m_state = GameEngineState.UpdateWait;
					break;
				case GameEngineState.UpdateWait:
					// 敵配置
					m_nowEnemyInterval += UnityEngine.Time.deltaTime;
					if (m_nowEnemyInterval >= m_enemySpawnInterval)
					{
						m_nowEnemyInterval -= m_enemySpawnInterval;

						// 敵インスタンス初期化
						var t_pos = new Vector3(UnityEngine.Random.Range(-150f, 150f), UnityEngine.Random.Range(-150f, 150f), 0f);
						var t_bullet = (Character.Enemy)Pool.PoolManager.GetInstance().RequestObject(m_enemyPoolName);
						t_bullet.Init(t_pos, Vector3.one, Quaternion.identity);

					}

					break;
				case GameEngineState.End:
					break;
			}

			// 入力更新
			Input.InputManager.GetInstance().Update();
			

		}
	}
}

