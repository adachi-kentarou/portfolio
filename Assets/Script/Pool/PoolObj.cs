/**
 * プールオブジェクト
 * ここで個別のオブジェクトの対応をする
 * 
 */

using System;
using System.Collections.Generic;
using UnityEngine;
namespace Pool
{
	/// <summary>
	/// プールオブジェクト
	/// </summary>
	public class PoolObj : System.IDisposable
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public PoolObj()
		{
			m_gameObjectList = new List<Character.CharaBase>();
		}

		/// <summary>
		/// オブジェクトプールの初期化
		/// </summary>
		/// <param name="a_name">プール名</param>
		/// <param name="a_obj">オリジナルのオブジェクト</param>
		/// <param name="a_num">オブジェクト数</param>
		/// <param name="a_addObj">配置先オブジェクト</param>
		public void Init(string a_name, GameObject a_obj, int a_num, GameObject a_addObj)
		{
			m_originObject = a_obj;

			for (int t_count = 0; t_count < a_num; t_count++)
			{
				var t_obj = GameObject.Instantiate<GameObject>(m_originObject);

				t_obj.transform.SetParent(a_addObj.transform);

				var t_charBase = t_obj.GetComponent<Character.CharaBase>();

				m_gameObjectList.Add(t_charBase);
			}
		}

		/// <summary>
		/// オブジェクトリスト
		/// </summary>
		private List<Character.CharaBase> m_gameObjectList;

		private string m_name;
		private GameObject m_originObject;

		/// <summary>
		/// 未使用のオブジェクトを取得
		/// </summary>
		/// <returns>未使用オブジェクト</returns>
		public Character.CharaBase RequestObject()
		{
			Character.CharaBase t_obj = m_gameObjectList.Find((a_obj)=> {
				return a_obj.GetActive() == false;
			});

			return t_obj;
		}

		/// <summary>
		/// オブジェクトリスト取得
		/// </summary>
		/// <returns></returns>
		public List<Character.CharaBase> GetObjList()
		{
			return m_gameObjectList;
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		public void Dispose()
		{
			//オブジェクトを削除
			GameObject.Destroy(m_originObject);
			foreach(Character.CharaBase t_obj in m_gameObjectList)
			{
				GameObject.Destroy(t_obj.gameObject);
			}
		}
	}

}
