/**
 * オブジェクトプールの管理
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
	/// <summary>
	/// プールマネージャー
	/// </summary>
	public class PoolManager : System.IDisposable
	{
		

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private PoolManager()
		{
			m_poolDictionary = new Dictionary<string, PoolObj>();

			// 配置先の数だけ作成する
			m_rootObj = new GameObject[(int)CanvasType.Max];

			for (int t_count = 0; t_count < (int)CanvasType.Max; t_count++)
			{
				m_rootObj[t_count] = GameObject.Find(string.Format("Canvas/{0}",(CanvasType)t_count));

			}
			

		}

		/// <summary>
		/// シングルトン用インスタンス
		/// </summary>
		private static PoolManager s_instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		public static PoolManager GetInstance()
		{
			if (s_instance == null)
			{
				s_instance = new PoolManager();

			}

			return s_instance;
		}
		
		/// <summary>
		/// ルートキャンバス配列
		/// </summary>
		private GameObject[] m_rootObj;

		/// <summary>
		/// オブジェクトプールリスト
		/// </summary>
		private Dictionary<string, PoolObj> m_poolDictionary;

		/// <summary>
		/// プール作成
		/// </summary>
		/// <param name="a_name">作成プール名</param>
		/// <param name="a_obj">プールさせるオリジナルオブジェクト</param>
		/// <param name="a_num">オブジェクトの数</param>
		public void CreatePool(string a_name, GameObject a_obj, int a_num, CanvasType a_type)
		{
			
			GameObject t_originObj = m_rootObj[(int)a_type];

			PoolObj t_pool = new PoolObj();
			t_pool.Init(a_name, a_obj, a_num, t_originObj);


			m_poolDictionary.Add(a_name, t_pool);
		}

		/// <summary>
		/// 未使用オブジェクト取得
		/// </summary>
		/// <param name="a_poolName">取得するプール名</param>
		/// <returns></returns>
		public Character.CharaBase RequestObject(string a_poolName)
		{
			if(m_poolDictionary.ContainsKey(a_poolName) == false)
			{
				Debug.LogWarning("no contains pool!");

				return null;
			}

			var t_obj = m_poolDictionary[a_poolName].RequestObject();

			if (t_obj == null)
			{
				Debug.LogWarning("nothing use obj!");
			}
			return t_obj;
		}

		/// <summary>
		/// 指定プールのオブジェクトリスト取得
		/// </summary>
		/// <param name="a_poolName">プール名</param>
		/// <returns>オブジェクトリスト</returns>
		public List<Character.CharaBase> GetPoolList(string a_poolName)
		{
			if (m_poolDictionary.ContainsKey(a_poolName) == false)
			{
				Debug.LogWarning("no contains pool!");

				return null;
			}

			return m_poolDictionary[a_poolName].GetObjList();
		}

		/// <summary>
		/// 更新
		/// </summary>
		// Update is called once per frame
		public void Update()
		{


		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		public void Dispose()
		{
			foreach(KeyValuePair<string, PoolObj> t_pair in m_poolDictionary)
			{
				t_pair.Value.Dispose();
			}
		}
	}
}
