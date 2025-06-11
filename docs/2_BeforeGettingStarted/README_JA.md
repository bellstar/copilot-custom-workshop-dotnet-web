<!-- filepath: docs_dotnet/2_BeforeGettingStarted/README_JA.md -->
# はじめる前に

[前へ - モナの夢を実現する物語](../1_Story/README_JA.md) | [次へ - Dockerコンテナを使ったPostgreSQLデータベースレイヤーの構築](../3_BuildPostgreSQL/README_VS2022_JA.md)

## このワークショップで作るものの全体像

![High Level View](images/ArchitectureStacks.jpg)

1. Dockerコンテナを使ったPostgreSQLデータベースレイヤーの構築
2. PostgreSQLへの接続確認用のシンプルなスクリプト作成
3. データベースに接続しWeb UIやREST APIを公開する .NET 8 (ASP.NET Core MVC) アプリケーションの作成
4. （オプション）Razor Viewsや最小限のReact統合によるフロントエンド強化
5. アプリケーションインフラのデプロイ（発展課題）

## このワークショップの対象者

以下の条件に当てはまる方に最適です：
- Copilot 101の基礎トレーニングを受講済み、またはGitHub Copilotに慣れている方
- .NET 8 (ASP.NET Core MVC)とデータベースレイヤーを含むフルスタックWeb開発に興味がある方

## 前提条件

このワークショップを進めるには、以下の環境が整っていることを確認してください。

- Copilotライセンスへのアクセス
- Copilot for Businessライセンス付きのCopilot Chatへのアクセス
- Visual Studio 2022のバージョン17.14.5以降 または .NET 8 SDKとCopilot/Copilot Chatが使えるVisual Studio Code
- Docker（DockerHubからベースイメージ取得可能なこと）
- .NET 8 SDKがインストールされていること
- Windowsの場合はGit CLI、Mac/Linuxの場合はターミナルでgitが使えること

## 期待される成果

このワークショップの終了時には、.NET 8 (ASP.NET Core MVC) バックエンドとPostgreSQLデータベースレイヤーをGitHub Copilotを活用して構築できるようになります。

## 推奨フォルダ構成

以下のように、各コンポーネントごとに個別のフォルダを作成することを推奨します：
- `app` - .NET 8 (ASP.NET Core MVC) アプリケーション（コントローラー、モデル、ビューを含む）
- `database` - PostgreSQLデータベースレイヤー
- `automation` - アプリケーションインフラ構築用の自動化スクリプト

![Recommended setup](images/0_FolderStructure.jpg)

[前へ - モナの夢を実現する物語](../1_Story/README_JA.md) | [次へ - Dockerコンテナを使ったPostgreSQLデータベースレイヤーの構築](../3_BuildPostgreSQL/README_VS2022_JA.md)
