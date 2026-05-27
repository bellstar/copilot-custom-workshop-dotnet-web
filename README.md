# カスタム GitHub Copilot ワークショップ（ASP.NET Core & SQLite スタック）

**GitHub エキスパートサービスチーム**がご用意したカスタム Copilot ワークショップへようこそ！

このワークショップでは、GitHub Copilot の **Agent モード**・**Custom Instructions**・**Custom Agent** を中心に、ASP.NET Core MVC + SQLite の Web アプリケーション「MeowWorld」を構築しながら、Copilot の最新機能を実践的に学びます。

> **Note:** Visual Studio 2026 + .NET 10（LTS）をメイン環境としています。VS Code（C# Dev Kit）でも実施可能です。Visual Studio 2022 をご利用の場合は .NET 8（LTS）での実施も可能です。詳細は [Step 2](docs/2_BeforeGettingStarted/README_JA.md) をご覧ください。

## 目次

| Step | タイトル | 主な学習内容 |
|------|---------|-------------|
| 1 | [Mona の夢を叶えるストーリー](docs/1_Story/README_JA.md) | ワークショップの背景ストーリー |
| 2 | [はじめにお読みください](docs/2_BeforeGettingStarted/README_JA.md) | 環境構築・モード解説・前提条件 |
| 3 | [プロジェクト作成（Agent モード）](docs/3_CreateProject/README_JA.md) | Agent モードで一気にプロジェクト構築 |
| 4 | [Custom Instructions](docs/4_CustomInstructions/README_JA.md) | 3 層の指示設定（リポジトリ / ファイルスコープ / .prompt.md） |
| 5 | [Token 節約とコンテキスト管理](docs/5_TokenManagement/README_JA.md) | #file・#codebase・#terminalLastCommand の使い分け |
| 6 | [DB レイヤー実装](docs/6_ImplementDBLayer/README_JA.md) | EF Core + SQLite を Agent が自律的に実装 |
| 7 | [MVC 実装 + Vision](docs/7_ImplementMVC/README_JA.md) | .prompt.md 活用 + 画像から UI 生成 |
| 8 | [ユニットテスト](docs/8_UnitTesting/README_JA.md) | /tests コマンド + Agent によるテスト自動生成 |
| 9 | [Custom Agent](docs/9_CustomAgent/README_JA.md) | .agent.md で専門エージェント構築・Before/After 比較 |
| 10 | [まとめとふりかえり](docs/10_LessonsLearned/README_JA.md) | ベストプラクティス・次のステップ |

**付録:** [トラブルシューティングガイド](docs/TroubleshootingGuide/README_JA.md)
