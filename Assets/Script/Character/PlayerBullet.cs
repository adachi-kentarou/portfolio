using System;
using UnityEngine;

namespace Character
{
	public class PlayerBullet : CharaBase
	{

		/// <summary>
		/// デストラクタ
		/// </summary>
		public override void Dispose()
		{

		}

		/// <summary>
		/// オブジェクト無効化時処理
		/// </summary>
		public void OnDisable()
		{

		}

		/// <summary>
		/// 移動距離
		/// </summary>
		[SerializeField]
		private float m_moveVal = 800f;

		/// <summary>
		/// 更新処理
		/// </summary>
		public void Update()
		{
			var t_pos = this.gameObject.transform.localPosition;
			t_pos.y += m_moveVal * UnityEngine.Time.deltaTime;
			
			if (CollisionArea(t_pos) == true)
			{
				this.gameObject.transform.localPosition = t_pos;
			}
			else
			{
				// 範囲外に出たらプールに戻す
				this.Remove();
			}

		}

		/// <summary>
		/// エリア範囲矩形
		/// </summary>
		private UnityEngine.Vector4 m_vec = new UnityEngine.Vector4(-300f, -200f, 300f, 200f);

		/// <summary>
		/// エリア範囲座標判定
		/// </summary>
		/// <param name="a_pos">現在座標</param>
		/// <returns></returns>
		private bool CollisionArea(Vector3 a_pos)
		{
			return (m_vec.x > a_pos.x || m_vec.y > a_pos.y || m_vec.z < a_pos.x || m_vec.w < a_pos.y) == false;

		}
	}
}
