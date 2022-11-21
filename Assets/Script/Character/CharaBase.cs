/*
 * キャラクターオブジェクトの基底クラス
 * 必ず継承させるのでabscract
 * 初期化、更新、画像の表示など
 */
using System;
using UnityEngine;

namespace Character
{
	/// <summary>
	/// キャラ基底クラス
	/// </summary>
	public abstract class CharaBase : MonoBehaviour, System.IDisposable
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CharaBase ()
		{
			
		}

		/// <summary>
		/// プールからの生存確認用
		/// </summary>
		private bool m_isActive = false;

		/// <summary>
		/// 生存確認フラグ取得
		/// </summary>
		/// <returns>true=使用中 false=未使用</returns>
		public bool GetActive()
		{
			
			return m_isActive;
		}

		/// <summary>
		/// 初期化
		/// </summary>
		public void Start()
		{
			this.gameObject.SetActive(false);
		}

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="a_pos">座標</param>
		/// <param name="a_size">サイズ</param>
		/// <param name="a_rot">回転</param>
		public virtual void Init(Vector3 a_pos, Vector3 a_size,Quaternion a_rot)
		{
			this.transform.localPosition = a_pos;
			this.transform.localScale = a_size;
			this.transform.rotation = a_rot;

			m_isActive = true;
			this.gameObject.SetActive(true);
		}

		/// <summary>
		/// オブジェクトを無効にしプールに戻す
		/// </summary>
		public void Remove()
		{
			this.gameObject.SetActive(false);
			m_isActive = false;
		}


		/// <summary>
		/// デストラクタ
		/// </summary>
		public abstract void Dispose();
	}
}
