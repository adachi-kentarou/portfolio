using System;
using UnityEngine;

namespace Character
{
	public class Player : CharaBase
	{

		/// <summary>
		/// 弾プール名
		/// </summary>
		private readonly string m_playerBulletPoolName = "PlayerBullet";

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
		/// 更新処理
		/// </summary>
		public void Update()
		{

		}

		/// <summary>
		/// 移動距離
		/// </summary>
		[SerializeField]
		private float m_moveValue = 200f;

		/// <summary>
		/// 右カーソル入力コールバック
		/// </summary>
		public void InputRight()
		{
			var t_pos = this.gameObject.transform.localPosition;
			t_pos.x += m_moveValue * UnityEngine.Time.deltaTime;
			if (CollisionArea(t_pos) == true) this.gameObject.transform.localPosition = t_pos;
		}

		/// <summary>
		/// 左カーソル入力コールバック
		/// </summary>
		public void InputLeft()
		{
			var t_pos = this.gameObject.transform.localPosition;
			t_pos.x -= m_moveValue * UnityEngine.Time.deltaTime;
			if (CollisionArea(t_pos) == true) this.gameObject.transform.localPosition = t_pos;
		}

		/// <summary>
		/// 上カーソル入力コールバック
		/// </summary>
		public void InputUp()
		{
			var t_pos = this.gameObject.transform.localPosition;
			t_pos.y += m_moveValue * UnityEngine.Time.deltaTime;
			if (CollisionArea(t_pos) == true) this.gameObject.transform.localPosition = t_pos;
		}

		/// <summary>
		/// 下カーソル入力コールバック
		/// </summary>
		public void InputDown()
		{
			var t_pos = this.gameObject.transform.localPosition;
			t_pos.y -= m_moveValue * UnityEngine.Time.deltaTime;
			if (CollisionArea(t_pos) == true) this.gameObject.transform.localPosition = t_pos;
		}

		/// <summary>
		/// A入力コールバック
		/// </summary>
		public void InputA()
		{
			// 弾インスタンス初期化
			var t_bullet = (Character.PlayerBullet)Pool.PoolManager.GetInstance().RequestObject(m_playerBulletPoolName);
			t_bullet.Init(this.transform.localPosition, Vector3.one * 0.5f, Quaternion.identity);

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
			return (m_vec.x > a_pos.x || m_vec.y > a_pos.y || m_vec.z < a_pos.x || m_vec.w < a_pos.y)  == false;

		}
	}
}
