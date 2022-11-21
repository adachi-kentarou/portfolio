/** 
 * インプットマネージャー
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input
{
	public class InputManager
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		private InputManager()
		{
			m_keyList = new List<InputParam>();

		}

		/// <summary>
		/// シングルトン用インスタンス
		/// </summary>
		private static InputManager s_instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		public static InputManager GetInstance()
		{
			if (s_instance == null)
			{
				s_instance = new InputManager();

			}

			return s_instance;
		}

		/// <summary>
		/// 入力データ辞書
		/// </summary>
		private System.Collections.Generic.List<InputParam> m_keyList;

		/// <summary>
		/// 入力初期化
		/// </summary>
		/// <param name="a_param">Inputパラメータ</param>
		public void AddInput(InputParam a_param)
		{
			m_keyList.Add(a_param);
		}


		/// <summary>
		/// 更新
		/// </summary>
		// Update is called once per frame
		public void Update()
		{
			foreach(InputParam t_keyParam in m_keyList)
			{
				t_keyParam.AddPushUpdate();
			}


		}
	}
}

