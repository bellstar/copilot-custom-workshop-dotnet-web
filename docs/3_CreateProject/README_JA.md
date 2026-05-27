# プロジェクト作成（Agent モード）

[前へ - はじめる前に](../2_BeforeGettingStarted/README_JA.md) | [次へ - Custom Instructions](../4_CustomInstructions/README_JA.md)

このステップでは、GitHub Copilot の Agent モードを使って ASP.NET Core MVC プロジェクトの作成から SQLite パッケージの導入、ビルド確認までを一気に行います。

---

## Agent モードでプロジェクトをセットアップする

### 前提

- .NET 10 SDK がインストールされていること
- Visual Studio 2026 または VS Code（C# Dev Kit）がインストールされていること
- Copilot Chat のモード切り替えで「Agent」が選択可能であること

### 手順

1. ワークショップ用のフォルダーを作成します（例：`C:\Repos\MeowWorld-Workshop`）

   > **Note:** まだプロジェクトやソリューションは作成しません。空のフォルダーを用意するだけです。Agent がこの中にプロジェクトを自動生成します。

2. IDE を起動し、手順 1 で作成したフォルダーを開きます
   - **Visual Studio 2026:** 「フォルダーを開く」から選択
   - **VS Code:** `File > Open Folder` から選択

3. Copilot Chat を開き、モード切り替えドロップダウンから **「Agent」** を選択します

4. 以下のプロンプトを入力して送信します：

```
.NET 10 の ASP.NET Core MVC プロジェクト "MeowWorld" を新規作成してください。
作成先は ./app ディレクトリです。
SQLite 関連の NuGet パッケージ（Microsoft.EntityFrameworkCore.Sqlite、
Microsoft.EntityFrameworkCore.Design、Microsoft.EntityFrameworkCore.Tools）を追加し、
ビルドが通ることを確認してください。
```

> **Note (.NET 8):** VS2022 の方は「.NET 10」を「.NET 8」に読み替え、EF Core パッケージもバージョン `8.0.x` を指定してください。

5. Agent がターミナルコマンドの実行許可を求めてきたら、**内容を確認して**「Continue」を押します

6. Agent が以下を順に実行します：
   - `dotnet new mvc` でプロジェクト作成
   - `dotnet add package` で NuGet パッケージ追加
   - `dotnet build` でビルド確認

> **Tip:** Agent の実行中にエラーが発生した場合、Agent が自動的にエラーを解析して修正を試みます。これが Agent モードの大きな利点です。

### 確認

ビルドが成功したら、以下のコマンド（またはデバッグ実行）でアプリケーションが起動することを確認します：

```bash
cd app/MeowWorld
dotnet run
```

ブラウザが起動し、ASP.NET Core の初期画面が表示されれば成功です。

---

## 学びのポイント：Agent モードの特徴

このステップで体験した Agent モードの特徴を振り返りましょう：

| 特徴 | このステップでの体験 |
|------|---------------------|
| **自律的なタスク実行** | 1つのプロンプトでプロジェクト作成〜ビルド確認まで完了 |
| **ターミナル操作** | `dotnet` コマンドを Agent が直接実行 |
| **エラーの自動修復** | ビルドエラーが出た場合に自動で原因分析・修正 |
| **確認ベースの進行** | 各コマンド実行前にユーザーの承認を求める |

> **Ask モードとの違い:** Ask モードでは「コマンドの手順」を教えてくれるだけで、実行は自分で行う必要があります。Agent モードでは Copilot が実行まで担当します。

---

## 補足：手動でセットアップする場合

Agent モードを使わず手動で進める場合は、以下のコマンドを順に実行します：

<details>
<summary>手動セットアップ手順（クリックで展開）</summary>

```bash
# 1. プロジェクト作成
mkdir app
cd app
dotnet new mvc -n MeowWorld
cd MeowWorld

# 2. NuGet パッケージ追加
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools

# 3. ビルド確認
dotnet build
```

> **Note (.NET 8):** `dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.*` のようにバージョンを指定してください。

</details>

---

[前へ - はじめる前に](../2_BeforeGettingStarted/README_JA.md) | [次へ - Custom Instructions](../4_CustomInstructions/README_JA.md)
