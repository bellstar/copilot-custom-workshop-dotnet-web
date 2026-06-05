# Custom Instructions でプロジェクトルールを定義する

[前へ - プロジェクト作成](../3_CreateProject/README_JA.md) | [次へ - DB レイヤー実装](../5_ImplementDBLayer/README_JA.md)

このステップでは、GitHub Copilot の **Custom Instructions**（カスタム指示）機能を 3 段階で学びます。毎回のプロンプトに書かなくても、Copilot が自動的にプロジェクトのルールに従うようになります。

---

## Custom Instructions の全体像

Copilot のカスタム指示には 3 つのレベルがあります：

| レベル | ファイル | 適用範囲 | 用途 |
|--------|---------|----------|------|
| **リポジトリ全体** | `.github/copilot-instructions.md` | すべてのチャット | プロジェクト共通ルール |
| **ファイルスコープ** | `.instructions.md`（applyTo 付き） | 特定パターンのファイル操作時 | ファイル種類別ルール |
| **再利用可能プロンプト** | `.prompt.md` | 呼び出し時のみ | テンプレート化した指示 |

---

## 1. リポジトリレベルの指示（`.github/copilot-instructions.md`）

### Before: 設定前の出力を確認

まず、Custom Instructions なしの状態を確認します。

> 💬 **Ask モード**

```
ASP.NET Core MVC で猫の情報（名前・年齢・品種）を表す DTO クラスを作成してください。
```

出力を観察します：
- コメントは何語で書かれていますか？
- namespace は `namespace X { }` 形式？ `namespace X;` 形式？
- `required` キーワードや nullable 型は使われていますか？

### 手順：ファイル作成

プロジェクトルートに `.github/copilot-instructions.md` を作成します：

```markdown
# MeowWorld プロジェクト - Copilot カスタム指示

## 全般
- 日本語で回答すること

## コメント規約
- コード内のコメントは日本語で記述すること
- XML ドキュメントコメント（`///`）も日本語で記述すること

## C# コーディング規約
- file-scoped namespace（`namespace X;` 形式）を使用すること
- nullable reference types を考慮し、必要に応じて `string?` や `required` キーワードを使用すること
- プライマリコンストラクタが使える場面では積極的に使うこと

## ASP.NET Core / Entity Framework Core
- 非同期メソッド（async/await）を優先すること
- DbContext はコンストラクタインジェクションで受け取ること
- LINQ メソッド構文を優先すること

## プロジェクト構成
- OS: Windows
- フレームワーク: .NET 10, ASP.NET Core MVC
- データベース: SQLite（Entity Framework Core 経由）
- テスト: xUnit + Moq
```

> **設定の確認:**
> - **VS Code:** `github.copilot.chat.codeGeneration.useInstructionFiles` が有効であることを確認
> - **Visual Studio 2026:** ツール > オプション > GitHub Copilot > Custom Instructions で参照が有効か確認

### After: 設定後の出力を確認

**新しいチャットセッション**を開き、同じプロンプトを再度入力してください：

> 💬 **Ask モード**

```
ASP.NET Core MVC で猫の情報（名前・年齢・品種）を表す DTO クラスを作成してください。
```

### 確認ポイント

| 観点 | Before | After |
|------|--------|-------|
| コメントの言語 | 英語が多い | 日本語 |
| namespace 形式 | ブロック形式 | file-scoped |
| null 安全性 | 考慮なし | `required`/`string?` 使用 |

---

## 2. ファイルスコープ指示（`.instructions.md` + `applyTo`）

リポジトリ全体のルールに加え、**ファイルの種類ごとに異なるルール**を適用できます。

### `.github/` フォルダの全体構成

このハンズオンで作成する Copilot カスタマイズファイルは、すべて `.github/` フォルダに配置します。最終的に以下の構成になります：

```
.github/
├── copilot-instructions.md              ← Step 4-1: リポジトリ全体の指示
├── instructions/
│   ├── views.instructions.md            ← Step 4-2: ファイルスコープ（ビュー用）
│   └── tests.instructions.md            ← Step 4-2: ファイルスコープ（テスト用）
├── prompts/
│   └── create-crud-controller.prompt.md ← Step 4-3: 再利用プロンプト
├── agents/
│   └── *.agent.md                       ← Step 9: カスタムエージェント
└── skills/
    └── */SKILL.md                       ← Step 9: スキル（Agent が参照するナレッジ）
```

> **Why this structure?** GitHub 公式ドキュメントでは、path-specific instructions を `.github/instructions/` ディレクトリに配置するよう定められています。VS Code ではワークスペース内のどこに置いても検出されますが、GitHub.com の Copilot 機能（cloud agent、code review）では `.github/instructions/` 配下が必須です。また `prompts/`、`agents/`、`skills/` と同じ「種類別サブディレクトリ」パターンに揃うため、構成が直感的になります。

### ビュー用の指示を作成

`.github/instructions/views.instructions.md` を作成します：

```markdown
---
applyTo: "**/*.cshtml"
---
- Bootstrap 5 のクラスを使用すること
- レスポンシブデザインを意識すること（`container`、`row`、`col-*` を活用）
- 日本語の UI ラベルを使用すること
- `asp-*` タグヘルパーを積極的に使用すること
```

### テスト用の指示を作成

`.github/instructions/tests.instructions.md` を作成します：

```markdown
---
applyTo: "**/*.Tests/**"
---
- テストメソッド名は日本語で「何をテストしているか」を明記すること（例: `猫一覧が正しく取得できること`）
- AAA パターン（Arrange / Act / Assert）を必ず使用すること
- 各セクションをコメントで区切ること（`// Arrange` など）
- InMemory データベースを使用し、テストごとに一意のDB名を付けること
```

> **Note:** ファイル名は `*.instructions.md` のパターンに一致すれば任意です。ここでは用途が明確になるよう `views`・`tests` というプレフィックスを付けています。

### 効果の確認

以下を試してください（ファイルは変更されません）：

> 💬 **Ask モード**

```
Views/Cats/Index.cshtml に猫の一覧テーブルを表示するビューを作成してください。
```

`views.instructions.md` の `applyTo: "**/*.cshtml"` が自動的に適用され、Bootstrap 5 と日本語ラベルが使われたビューが出力されるはずです。実際のファイル作成は Step 6 で行います。

> **Note:** `applyTo` にはグロブパターンを使います。複数の `.instructions.md` ファイルを配置でき、パターンにマッチするファイルへの操作時に自動適用されます。

---

## 3. 再利用可能プロンプト（`.prompt.md`）

同じような指示を何度も書く代わりに、**テンプレート化したプロンプト**を保存できます。

### CRUD コントローラー生成プロンプトを作成

`.github/prompts/create-crud-controller.prompt.md` を作成します：

```markdown
---
description: "Entity に対する CRUD コントローラーを生成する"
---
# CRUD コントローラー生成

以下の Entity に対して、ASP.NET Core MVC の CRUD コントローラーを作成してください。

## 対象 Entity
{{input}}

## 要件
- AppDbContext を DI で受け取る
- 全アクションを async/await で実装する
- Index（一覧）、Details（詳細）、Create（作成 GET/POST）、Edit（編集 GET/POST）、Delete（削除 GET/POST）を実装する
- POST アクションでは ModelState.IsValid を検証する
- 例外処理は try-catch で囲み、エラー時はログ出力する
```

### 使い方

Copilot Chat で `/` を入力すると、保存したプロンプトが候補として表示されます。`create-crud-controller` を選択し、Entity 名を入力するだけで一貫したコントローラーが生成されます：

> 💬 **Ask モード**（動作確認のみ。実際の生成は Step 6 で行います）

```
/create-crud-controller Cat（Id, Name, Age, Breed, Description, CreatedAt プロパティを持つ）
```

> **Tip:** `.prompt.md` はチーム全体で共有でき、「この機能を追加するときはこのプロンプトを使う」という運用が可能です。次の Step 6 で実際にこのプロンプトを活用します。

---

## まとめ

| レベル | ファイル | いつ適用されるか |
|--------|---------|----------------|
| リポジトリ全体 | `.github/copilot-instructions.md` | 常に自動適用 |
| ファイルスコープ | `.instructions.md`（applyTo） | パターンにマッチするファイル操作時に自動適用 |
| 再利用可能プロンプト | `.prompt.md` | ユーザーが明示的に呼び出したとき |

これらを組み合わせることで：
- **毎回プロンプトにルールを書く手間**がなくなる
- **チーム全員が同じ品質**のコード生成を受けられる
- **Token の節約**にもなる（Step 7 で詳しく学びます）

---

[前へ - プロジェクト作成](../3_CreateProject/README_JA.md) | [次へ - DB レイヤー実装](../5_ImplementDBLayer/README_JA.md)
