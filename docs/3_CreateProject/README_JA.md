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

1. リポジトリルートで `app` フォルダーを作成し、IDE で **`app/` フォルダーをワークスペースとして開きます**。
   - **VS Code:** `File > Open Folder` → `app` フォルダーを選択
   - **Visual Studio 2026:** 「フォルダーを開く」→ `app` フォルダーを選択

   ```bash
   mkdir app
   ```

   > **Why `app/` をワークスペースにするのか？**
   > - 実際の開発では**プロジェクトルート = ワークスペースルート**が標準です
   > - Copilot のコンテキストにハンズオン資料（`docs/`）が混入しないため、補完精度が向上します
   > - `.github/copilot-instructions.md` がワークスペースルート直下に配置される自然な構成になります
   > - 以降のステップで作成するすべてのファイルパスが、実務と同じ形式になります

2. Copilot Chat を開き、モード切り替えドロップダウンから **「Agent」** を選択します

3. 以下のプロンプトを入力して送信します：

> 🕵️ **Agent モード**

```
.NET 10 の ASP.NET Core MVC プロジェクト "MeowWorld" を新規作成してください。
SQLite 関連の NuGet パッケージ（Microsoft.EntityFrameworkCore.Sqlite、
Microsoft.EntityFrameworkCore.Design、Microsoft.EntityFrameworkCore.Tools）を追加し、
ビルドが通ることを確認してください。
```

4. Agent がターミナルコマンドの実行許可を求めてきたら、**内容を確認して**「Continue」を押します

5. Agent が以下を順に実行します：
   - `dotnet new mvc` でプロジェクト作成
   - `dotnet add package` で NuGet パッケージ追加
   - `dotnet build` でビルド確認

> **Tip:** Agent の実行中にエラーが発生した場合、Agent が自動的にエラーを解析して修正を試みます。これが Agent モードの大きな利点です。

### 確認

ビルドが成功したら、以下のコマンド（またはデバッグ実行）でアプリケーションが起動することを確認します：

```bash
cd MeowWorld
dotnet run
```

ブラウザが起動し、ASP.NET Core の初期画面が表示されれば成功です。

![ASP.NET Core初期画面](./images/init-webapp.png)

起動確認が終わったらプロセスを終了します。

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
# app/ フォルダーをワークスペースとして開いた状態で実行

# 1. プロジェクト作成
dotnet new mvc -n MeowWorld

# 2. NuGet パッケージ追加
cd MeowWorld
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools

# 3. ビルド確認
dotnet build
```

</details>

---

[前へ - はじめる前に](../2_BeforeGettingStarted/README_JA.md) | [次へ - Custom Instructions](../4_CustomInstructions/README_JA.md)
