using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	/// <summary>
	/// ゲームエンジンエンジンの
	/// </summary>
	enum GameEngineState
	{
		/// <summary>
		/// 初期化
		/// </summary>
		Init = 0,
		/// <summary>
		/// 更新処理待機
		/// </summary>
		UpdateWait,
		/// <summary>
		/// 終了処理
		/// </summary>
		End
	}
}
