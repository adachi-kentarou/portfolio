# portfolio
ポートフォリオ用リポジト

# 開発環境
* Unityバージョン 2019.2.6f1
* Editor Visual Studio 2017

# 実行方法
* EditorのBootシーンを開いて実行  
```bash
Assets/Scenes/Boot.unity
```
* 実行ファイル
動作環境 windows10 64bit  
ディレクトリの  
```bash
/Build/portfolio.exe
```
を実行  

操作はカーソルキーでプレイヤーキャラを移動  
Aキーで弾を発射  
敵が一定間隔で出現しランダム方向に移動する  
敵は弾が当たるか画面外に出ると消える  
エスケープキーでゲーム終了  

# 機能紹介
UnityEditor機能拡張でアセットバンドルの名前変更、  
ビルド、StreamingAssetsディレクトリへのコピー、  
ファイルの参照用コードをクリップボードにコピーを実行できるメニューを作成

Eidtorの上部メニューから
AssetBundle/AssetBundles name setting を選択でアセットバンドル対象ファイルのAsset Labelsの名前をファイルのパスに合わせて変更させる  
ビルドされる対象ファイルはファイルもしくはディレクトリ名の末尾に@bundleがあるものを対象とする  
例）  
```bash
Assets/Sample1@bundle
Assets/Sample2/img@bundle.png
```

Eidtorの上部メニューから  
AssetBundle/Bundle を選択でアセットバンドルをビルド  
ビルドされる対象ファイルはファイルもしくはディレクトリ名の末尾に@bundleがあるもの  

ビルドされたファイルは  
```bash
./AssetBundles/
```
に保存される  

Eidtorの上部メニューから  
AssetBundle/Copy AssetBundles To StreamingAssets を選択でビルドされたアセットバンドルを  
```bash
./Assets/StreamingAssets
```
にコピーする  

Projectタブ内のファイルを右クリックで表示されるコンテキストメニューから  
AssetBundle/Copy AssetBundles Path を選択でアセットバンドルをロードする際に使用するパスのソースコードをクリップボードにコピーする  
使用対象ファイルはアセットバンドル化させる前のファイル  
例)  
```bash
Assets/AssetBundle/bullet@bundle/PlayerBullet.prefab
```
のファイルで実行した場合  
```bash
"assets/assetbundle/bullet.assetbundle", "assets/assetbundle/bullet@bundle/playerbullet.prefab",
```
をクリップボードにコピーする  
複数選択された場合は改行でソースコードを連結する  

# 実行の流れ 
Bootクラスで初期化を行いアセットバンドルの読み込みとオブジェクトプールの初期化をする  
アセットバンドルの読み込みに使用するパスは上記のパスコピー機能で取得したソースコードを利用する  
初期化が終わるとBootクラスのUpdate関数を起点にイベントドリブンで  
GameEngineクラスのUpdateでInGameの初期化とゲームの更新をステートマシーンにて実行させる  
オブジェクトプールはマネージャークラスを利用して初期化、オブジェクトのリクエスト、オブジェクトのリムーブを行う  
