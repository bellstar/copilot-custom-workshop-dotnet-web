<!-- filepath: docs_dotnet/2_BeforeGettingStarted/README_JA.md -->
# はじめる前に

[前へ - モナの夢を実現する物語](../1_Story/README_JA.md) | [次へ - プロジェクト作成とSQLite導入](../3_CreateProjectAndDB/README_JA.md)

## このワークショップで作るものの全体像

![High Level View](./images/ArchitectureStacks.jpg)

1. ASP.NET Core MVC（.NET 8）プロジェクト作成とSQLite導入
2. DTO/DbContext/初期データ投入などDBレイヤーの実装
3. データベースに接続しWeb UIやREST APIを公開するMVCアプリケーションの実装
4. 単体テスト（xUnit）
5. 学びとベストプラクティス

## このワークショップの対象者

以下の条件に当てはまる方に最適です：
- Copilot 101の基礎トレーニングを受講済み、またはGitHub Copilotに慣れている方
- .NET 8 (ASP.NET Core MVC)とファイルベースDB（SQLite）を含むフルスタックWeb開発に興味がある方

## 前提条件

このワークショップを進めるには、以下の環境が整っていることを確認してください。

- Copilotライセンスへのアクセス
- Copilot for Businessライセンス付きのCopilot Chatへのアクセス
- Visual Studio 2022のバージョン17.14.5以降 または .NET 8 SDKとCopilot/Copilot Chatが使えるVisual Studio Code
- dotnet-ef ツールがインストールされていること
- .NET 8 SDKがインストールされていること
- Windowsの場合はGit CLI、Mac/Linuxの場合はターミナルでgitが使えること

## 期待される成果

このワークショップの終了時には、.NET 8 (ASP.NET Core MVC) バックエンドとSQLiteファイルデータベースレイヤーをGitHub Copilotを活用して構築できるようになります。

## 推奨フォルダ構成

以下のように、`app` ディレクトリ中心の構成を推奨します：
- `app` - .NET 8 (ASP.NET Core MVC) アプリケーション（コントローラー、モデル、ビュー、DBファイルを含む）

[前へ - モナの夢を実現する物語](../1_Story/README_JA.md) | [次へ - プロジェクト作成とSQLite導入](../3_CreateProjectAndDB/README_JA.md)
