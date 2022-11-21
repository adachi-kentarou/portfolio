/**
 * InputManager用ボタン初期化パラメータ
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Input
{

	/// <summary>
	/// 入力タイプ
	/// </summary>
	public enum InputType
	{
		/// <summary>
		/// 単押し
		/// </summary>
		Single = 0,
		/// <summary>
		/// 押しっぱなし一定間隔
		/// </summary>
		Interval,
	}

	/// <summary>
	/// InputManager用パラメータ
	/// </summary>
	public class InputParam
	{
		public InputParam (
		UnityEngine.KeyCode a_keyCode,
		InputType a_type,
		float a_interval,
		System.Action a_callBack
		)
		{
			m_keyCode = a_keyCode;
			m_type = a_type;
			m_interval = a_interval;
			m_callBack = a_callBack;
		}

		/// <summary>
		/// 対象キーコード
		/// </summary>
		public UnityEngine.KeyCode m_keyCode;
		
		/// <summary>
		/// 入力タイプ
		/// </summary>
		public InputType m_type;

		/// <summary>
		/// 入力間隔
		/// </summary>
		public float m_interval;

		/// <summary>
		/// 入力コールバック
		/// </summary>
		public System.Action m_callBack;

		/// <summary>
		/// キー入力時間
		/// </summary>
		private float m_nowInterval;

		/// <summary>
		/// キー入力判定
		/// </summary>
		private bool m_isPush;

		/// <summary>
		/// キー入力判定ゲッター
		/// </summary>
		/// <returns></returns>
		public bool GetIsPush()
		{
			return m_isPush;
		}
		
		/// <summary>
		/// キー入力時間更新
		/// </summary>
		public void AddPushUpdate()
		{
			
			if (UnityEngine.Input.GetKey(m_keyCode) == false)
			{
				m_isPush = false;
				m_nowInterval = 0f;
				return;
			}
			switch (m_type)
			{
				case InputType.Single: // 単押し
					{
						if (m_isPush == false)
						{
							m_isPush = true;
							m_callBack();
						}
					}
					break;
				case InputType.Interval: // 押しっぱなし一定間隔
					{
						m_nowInterval += UnityEngine.Time.deltaTime;

						//UnityEngine.Debug.Log(m_nowInterval);
						
						// 指定間隔時間経過した回数だけコールバックを実行
						while (m_nowInterval >= m_interval)
						{
							m_nowInterval -= m_interval;

							m_callBack();
							if (m_interval == 0) break;
						}

					}
					break;
			}
			
		}
	}
}
