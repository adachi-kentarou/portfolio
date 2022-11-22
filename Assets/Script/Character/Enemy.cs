using System;
using UnityEngine;

namespace Character
{
	public class Enemy : CharaBase
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
		/// 弾のオブジェクトプール名
		/// </summary>
		private readonly string m_playerBulletPoolName = "PlayerBullet";

		/// <summary>
		/// 移動速度
		/// </summary>
		[SerializeField]
		private float m_moveVal = 0.5f;

		/// <summary>
		/// 弾の当たり半径^2
		/// </summary>
		[SerializeField]
		private float m_hitBulletRadius = 100f;

		/// <summary>
		/// 更新処理
		/// </summary>
		public void Update()
		{
			var t_pos = this.gameObject.transform.localPosition;
			t_pos += (m_moveVal * UnityEngine.Time.deltaTime) * m_direction;

			if (CollisionArea(t_pos) == true)
			{
				this.gameObject.transform.localPosition = t_pos;
			}
			else
			{
				// 範囲外に出たらプールに戻す
				this.Remove();
			}

			// 弾の当たり判定
			var t_list = Pool.PoolManager.GetInstance().GetPoolList(m_playerBulletPoolName);

			foreach(var t_bullet in t_list)
			{
				var diffPos = t_bullet.transform.localPosition - this.transform.localPosition;

				if (diffPos.sqrMagnitude < m_hitBulletRadius)
				{
					t_bullet.Remove();
					this.Remove();
					return;
				}

			}
		}

		private Vector3 m_direction;

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="a_pos">座標</param>
		/// <param name="a_size">サイズ</param>
		/// <param name="a_rot">回転</param>
		public override void Init(Vector3 a_pos, Vector3 a_size, Quaternion a_rot)
		{
			// 移動方向設定
			var t_dir = UnityEngine.Random.Range(0f,360f);
			m_direction = Quaternion.Euler(0f, 0f, t_dir + 90f) * Vector3.right;
			var t_rot = Quaternion.Euler(0f, 0f, t_dir);

			base.Init(a_pos, a_size, t_rot);


			
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
