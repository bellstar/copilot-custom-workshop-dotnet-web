# はじめる前に

[前へ - モナの夢を実現する物語](../1_Story/README_JA.md) | [次へ - プロジェクト作成](../3_CreateProject/README_JA.md)

## このワークショップで作るものの全体像

![High Level View](./images/ArchitectureStacks.jpg)

1. ASP.NET Core MVC（.NET 10）プロジェクト作成と SQLite 導入
2. DTO/DbContext/初期データ投入などDBレイヤーの実装
3. データベースに接続し Web UI や REST API を公開する MVC アプリケーションの実装
4. 単体テスト（xUnit）
5. Custom Agent による新機能追加

## このワークショップで学ぶ Copilot の機能

| 機能 | 学ぶステップ | 概要 |
|------|-------------|------|
| **Agent モード** | Step 3〜 | Copilot がターミナル操作・ファイル編集を自律的に実行 |
| **Custom Instructions** | Step 4 | `.github/copilot-instructions.md`、`.instructions.md`（applyTo）、`.prompt.md` |
| **Token 節約・コンテキスト管理** | Step 5 | `#file`、`#codebase` 等の効率的な使い方 |
| **Vision（画像からコード生成）** | Step 7 | ワイヤーフレーム画像を添付して UI 実装 |
| **`/tests` コマンド** | Step 8 | 既存コードからテストを自動生成 |
| **Custom Agent** | Step 9 | `.agent.md` でプロジェクト専用エージェントを作成 |

## GitHub Copilot のモードについて

このワークショップでは主に **Agent モード** を使います。

| モード | 特徴 | 使い分け |
|--------|------|----------|
| **Agent** | ファイル編集・ターミナル実行・エラー修正を自律的に行う | 実装作業全般（本ワークショップのメイン） |
| **Ask** | 質問に回答するのみ。コードの提案はするが直接編集しない | 調査・学習・設計相談 |

> **Tip:** Agent モードでは、Copilot が提案するターミナルコマンドやファイル変更を確認してから承認できます。内容を必ず確認する習慣をつけましょう。

## コンテキスト変数（このワークショップで使うもの）

Copilot Chat ではプロンプト内で以下の変数を使い、必要な情報だけを正確に伝えられます：

| 変数 | 用途 | 例 |
|------|------|-----|
| `#file:パス` | 特定ファイルをコンテキストに追加 | `#file:appsettings.json を参照して接続文字列を確認して` |
| `#codebase` | ワークスペース全体を検索対象に | `#codebase この構造に合うコントローラーを作って` |
| `#selection` | エディタで選択中のコード | 選択範囲について質問したいとき |
| `#terminalLastCommand` | 直前のターミナル出力 | エラーが出た直後に「このエラーを修正して」 |

> **Note:** これらは Step 5「Token 節約とコンテキスト管理」で詳しく学びます。ここでは「こういうものがある」と覚えておいてください。

## 開発環境と .NET バージョンについて

このワークショップは **Visual Studio 2026 + .NET 10（LTS）** をメイン環境としています。

| 環境 | 推奨 .NET バージョン | 備考 |
|------|---------------------|------|
| **Visual Studio 2026** | .NET 10（LTS） | 本ワークショップのメインパス |
| **Visual Studio Code**（C# Dev Kit） | .NET 10（LTS） | VS Code でも実施可能 |
| **Visual Studio 2022**（v17.14.5 以降） | .NET 8（LTS） | VS2022 では .NET 10 が未サポートのため .NET 8 を使用 |

> **VS2022 + .NET 8 をご利用の方へ:** 各ステップのプロンプトやコマンドで `.NET 10` と記載されている箇所は `.NET 8` に読み替えてください。主な技術的差異は以下のとおりです：
>
> | 項目 | .NET 10 | .NET 8 |
> |------|---------|--------|
> | ターゲットフレームワーク (TFM) | `net10.0` | `net8.0` |
> | EF Core パッケージバージョン | 10.x | 8.x |
> | `dotnet-ef` ツールバージョン | 10.x | 8.x |

## このワークショップの対象者

- Copilot の基礎（コード補完・チャット）に触れたことがある方
- Agent モードや Custom Instructions など、最新の Copilot 機能を実践的に学びたい方
- ASP.NET Core MVC + SQLite のフルスタック開発に興味がある方

## 前提条件

- [GitHub Copilot](https://github.com/features/copilot/plans) ライセンス（Business または Enterprise）
- 以下のいずれかの開発環境：
  - **推奨:** [Visual Studio 2026](https://visualstudio.microsoft.com/ja/vs/) + [.NET 10 SDK](https://dotnet.microsoft.com/ja-jp/download/dotnet/10.0)
  - [VS Code](https://code.visualstudio.com/)（[C# Dev Kit 拡張機能](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)）+ [.NET 10 SDK](https://dotnet.microsoft.com/ja-jp/download/dotnet/10.0)
  - **VS2022 利用者向け:** [Visual Studio 2022](https://visualstudio.microsoft.com/ja/vs/) v17.14.5 以降 + [.NET 8 SDK](https://dotnet.microsoft.com/ja-jp/download/dotnet/8.0)
- Copilot Chat で **Agent モード** が利用可能であること
- [dotnet-ef](https://learn.microsoft.com/ja-jp/ef/core/cli/dotnet) ツールがインストールされていること
- [Git CLI](https://git-scm.com/install/windows)

## 期待される成果

このワークショップの終了時には：
- Copilot の Agent モードを使って効率的にアプリケーションを構築できる
- Custom Instructions / Custom Agent でチームの規約を Copilot に反映できる
- Token を無駄にしない効率的なプロンプトの書き方を身につけている
- 画像からの UI 生成など、最新の Copilot 機能を活用できる

## 推奨フォルダ構成

```
app/          ← .NET 10 (ASP.NET Core MVC) アプリケーション
.github/      ← Custom Instructions、Custom Agent 定義
.instructions.md  ← ファイルスコープ指示（複数配置可）
.prompt.md        ← 再利用可能プロンプト（複数配置可）
docs/         ← 設計ガイド等
```

[前へ - モナの夢を実現する物語](../1_Story/README_JA.md) | [次へ - プロジェクト作成](../3_CreateProject/README_JA.md)
