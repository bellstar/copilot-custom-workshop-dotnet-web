# Custom Agent & Skill

[前へ - ユニットテスト](../8_UnitTesting/README_JA.md) | [次へ - まとめ](../10_LessonsLearned/README_JA.md)

このステップでは、**Custom Agent（`.agent.md`）** と **Skill（`SKILL.md`）** を作成し、プロジェクト専用の AI アシスタントを構築します。

このハンズオンの比較は「規約を守れるかどうか」ではなく、**実務で重要な差**を体験することが目的です。

- 同じガードレール（Step 4 の Custom Instructions）は維持したまま比較する
- それでも差が出る観点（再現性、説明責任、手戻りの少なさ）を観測する
- 「必ず良くなる」ではなく「改善しやすい」ことを定量的に確認する

---

## Custom Agent とは

Custom Agent は `.agent.md` ファイルで定義する「特化型 AI アシスタント」です。

| 項目 | Custom Instructions | Custom Agent | Skill |
|------|-------------------|--------------|-------|
| 定義ファイル | `.github/copilot-instructions.md` | `.github/agents/xxx.agent.md` | `.github/skills/xxx/SKILL.md` |
| 適用範囲 | すべてのチャット | `@agent名` で呼び出した時のみ | Agent が必要と判断した時 |
| 用途 | コーディング規約・プロジェクト情報 | 役割・作業プロセス・完了条件 | ドメイン知識・参照情報のパッケージ |
| 参照方法 | 自動 | `@agent名` で明示呼び出し | Agent が description を見て自律判断 |

**使い分け:**
- Custom Instructions = 「全員が従うべきルール」（常時適用）
- Custom Agent = 「役割と進め方が定義されたチームメンバー」（明示呼び出し）
- Skill = 「専門書の本棚」（Agent が必要な時に自分で取り出す）

---

## 1. アーキテクチャガイドの生成

まず、Custom Agent が参照するプロジェクト情報を Copilot に生成してもらいます。

Copilot Chat（Agent モード）で：

> 🕵️ **Agent モード**

```
#codebase を分析して、docs/architecture-guide.md を作成してください。
以下を含めてください：
- プロジェクト構成（ディレクトリ・ファイル構造）
- 使用技術スタック
- 命名規約（実際のコードから読み取ったもの）
- データモデルの関係
- 新機能追加時の標準パターン（コントローラー、ビュー、テストの配置場所）
```

このファイルが Custom Agent のナレッジベースになります。

---

## 2. Before: ベースライン計測（Custom Agent なし）

まず、**Custom Agent を使わない状態**で実装し、ベースラインを測定します。

> 🕵️ **Agent モード**

```
猫のお気に入り登録機能を追加してください。

要件:
- Cat に対してお気に入り状態（IsFavorite: bool）を管理できること
- 一覧画面（Views/Cats/Index.cshtml）でお気に入りの切り替え操作ができること
- 既存の CRUD 機能を壊さないこと
- テストを追加し、dotnet test が通ること

最後に以下を出力してください:
1. 変更したファイル一覧
2. 実行した確認コマンドと結果
3. 既知のリスク（あれば）
```

### ベースラインで記録する指標

| 指標 | 記録方法 |
|------|----------|
| 初回受け入れ率 | 追加指示なしで要件を満たせたか（Yes/No） |
| 追加入力回数 | 完了までの追加プロンプト回数 |
| 検証の明示性 | build/test の実行結果が報告されているか |
| 変更追跡性 | 変更ファイル一覧が明示されているか |
| リスクの明示 | 既知の制約や未解決点が報告されているか |

> **重要:** ここでは「失敗を探す」のでなく、**比較用の測定値を取る**ことが目的です。

### 比較シート（推奨）

Before / After を同じ基準で比較するため、以下のシートを使うと変化を可視化できます。

| 指標 | Before（Agent のみ） | After（Custom Agent） | 判定基準 |
|------|----------------------|------------------------|----------|
| 初回受け入れ率 | Y / N | Y / N | 追加プロンプトなしで、要件を満たし `dotnet build` と `dotnet test` が成功 |
| 追加入力回数 | 回数 | 回数 | 追加で送ったプロンプトの回数（修正依頼 1 回につき +1） |
| 検証報告の明示性 | ○ / × | ○ / × | 実行コマンドと結果の両方が報告されている |
| 変更追跡性 | ○ / × | ○ / × | 変更ファイル一覧が提示されている |
| リスクの明示 | ○ / × | ○ / × | 未対応事項や既知リスクが明記されている（なければ「なし」） |

---

## 3. Custom Agent の作成（役割と完了条件を明文化）

### `.github/agents/meowworld-dev.agent.md` を作成

````markdown
---
description: "MeowWorld プロジェクトの機能開発を担当するエージェント。新機能の設計・実装・テストを一貫して行う。"
tools:
  - search/codebase
  - terminal
  - file
---
# MeowWorld 開発エージェント

あなたは MeowWorld（猫カフェ管理システム）の専任開発者です。

## プロジェクト知識

#file:docs/architecture-guide.md を必ず参照して、プロジェクトの構造と規約を理解した上で作業してください。

## 新機能の開発プロセス

新機能のリクエストを受けたら、以下の順序で作業してください：

### Step 1: 設計
- 必要なモデル変更を特定する
- 既存のモデル（Cat.cs）との関係を明確にする
- マイグレーションが必要か判断する

### Step 2: 実装
- モデル → DbContext → コントローラー → ビュー の順で実装する
- 既存コードのパターンに厳密に従う：
  - コントローラーは `Controllers/` 直下
  - ビューは `Views/{ControllerName}/` 配下
  - file-scoped namespace を使用
  - 非同期メソッドを使用
  - 日本語の XML ドキュメントコメント

### Step 3: テスト
- テストクラスは `MeowWorld.Tests/Controllers/` に配置
- テストメソッド名は日本語
- AAA パターン + InMemory DB
- 正常系 + 異常系（最低 3 ケース）を含める

### Step 4: 確認
- `dotnet build` でビルド確認
- `dotnet test` で全テストパス確認
- 既存テストが壊れていないか確認

## 制約
- 既存機能を壊さないこと
- 不必要なパッケージを追加しないこと
- 一度に複数のマイグレーションを作らないこと

## 完了条件（Definition of Done）
- ビルド成功（`dotnet build`）
- テスト成功（`dotnet test`）
- 変更ファイル一覧を提示
- 要件ごとの対応状況を提示（満たした/未対応）

## 返信フォーマット
最終返信は以下の順序で簡潔に報告すること：
1. 実装サマリ（3-5行）
2. 変更ファイル一覧
3. 実行した検証コマンドと結果
4. 未対応事項またはリスク（なければ「なし」）
````

---

## 4. After: Custom Agent で同じ機能を実装し比較

先ほどの変更を取り消すか、別ブランチで作業した上で、Custom Agent を使って同じ機能追加を行います。

Copilot Chat で `@meowworld-dev` と入力して Custom Agent を呼び出します：

> 🕵️ **Agent モード** — `@meowworld-dev`

```
@meowworld-dev 猫のお気に入り登録機能を追加してください。
ユーザーがお気に入りボタンを押すと、その猫がお気に入りリストに追加されます。

要件:
- Cat に対してお気に入り状態（IsFavorite: bool）を管理できること
- 一覧画面（Views/Cats/Index.cshtml）でお気に入りの切り替え操作ができること
- 既存の CRUD 機能を壊さないこと
- テストを追加し、dotnet test が通ること
```

> **比較のためのポイント:** 実装要件は Before と揃えます。一方で、変更ファイル一覧・検証結果・リスク報告はプロンプトに書かず、Custom Agent の完了条件と返信フォーマットに任せます。

### Before と After の比較（この章で観るべき差）

| 観点 | Before（Agent のみ） | After（Custom Agent） |
|------|---------------------|---------------------|
| 初回受け入れ率 | 指示依存で変動しやすい | 完了条件があるため安定しやすい |
| 追加入力回数 | 追加確認が増えやすい | 返信フォーマットで往復が減りやすい |
| 検証報告の品質 | 実行有無の報告が曖昧になりやすい | build/test 結果を明示しやすい |
| 変更追跡性 | 変更点の説明粒度がばらつく | 変更ファイル一覧で追跡しやすい |
| チーム運用適合性 | 個人依存になりやすい | DoD と報告形式を共有できる |

> **学びのポイント:** ガードレールを維持しても、Custom Agent は「役割」「完了条件」「報告様式」を固定できるため、**実務で必要な再現性と説明責任**に差が出ます。

---

## 5. Skill でナレッジを外部化する

### `#file` 参照の課題

セクション 3 で作成した `.agent.md` では `#file:docs/architecture-guide.md` でアーキテクチャガイドを参照しています。この方式には以下の課題があります：

| 課題 | 説明 |
|------|------|
| **常時ロード** | Agent を呼び出すたびに参照ファイルが全文コンテキストに読み込まれる |
| **Token 消費** | 単純な質問でも不要なナレッジが Token を消費する |
| **スケーラビリティ** | 参照ファイルが増えるほど Agent の起動コストが増大する |

**Skill** はこの問題を解決します。Agent が `description` フィールドを見て **「今の質問にこのナレッジが必要か？」を自律判断** し、必要な時だけ内容を読み込みます。

### Skill とは

```
.github/skills/
└── meowworld-patterns/
    └── SKILL.md          ← description + 本体ナレッジ
```

- **description（YAML front matter）**: Agent がこの Skill を使うべきか判断する手がかり
- **本文**: 実際のナレッジ（Agent が参照する内容）

### Skill を作成する

`.github/skills/meowworld-patterns/SKILL.md` を作成します：

````markdown
---
description: "MeowWorld プロジェクトのアーキテクチャパターン、ファイル配置規約、命名規則、新機能追加手順を提供する。新しいコントローラー・モデル・ビュー・テストの追加時に参照する。"
---
# MeowWorld アーキテクチャパターン

## プロジェクト構成

```text
MeowWorld/
├── Controllers/       ← コントローラー（{Entity}Controller.cs）
├── Models/            ← エンティティ・ビューモデル
├── Data/              ← DbContext・マイグレーション
├── Views/
│   ├── {Controller}/  ← コントローラー対応ビュー
│   └── Shared/        ← レイアウト・パーシャル
└── wwwroot/           ← 静的ファイル
```

## 技術スタック
- .NET 10 / ASP.NET Core MVC
- Entity Framework Core + SQLite
- xUnit + Moq（テスト）
- Bootstrap 5（UI）

## 命名規約
- エンティティ: PascalCase 単数形（`Cat`, `Favorite`）
- コントローラー: `{Entity}Controller`（複数形でない）
- ビューフォルダ: コントローラー名から `Controller` を除いた名前
- テストクラス: `{Controller名}Tests`
- テストメソッド: 日本語（「猫一覧が正しく取得できること」）

## 新機能追加パターン

### 1. 新エンティティの追加
1. `Models/{Entity}.cs` を作成
2. `Data/AppDbContext.cs` に `DbSet<{Entity}>` を追加
3. マイグレーション作成・適用
4. `Controllers/{Entity}Controller.cs` に CRUD 実装
5. `Views/{Entity}/` に Index, Details, Create, Edit, Delete ビュー作成
6. `MeowWorld.Tests/Controllers/{Entity}ControllerTests.cs` にテスト

### 2. 既存エンティティへのリレーション追加
1. モデルにナビゲーションプロパティ追加
2. `OnModelCreating` でリレーション設定
3. マイグレーション作成・適用
4. 関連コントローラーにアクション追加

## コーディング規約
- file-scoped namespace
- 非同期メソッド（async/await）
- プライマリコンストラクタ（DI 用）
- 日本語 XML ドキュメントコメント
- `required` / nullable 型の適切な使用
````

---

## 6. Agent を Skill 対応に更新する

`.agent.md` から `#file` 直書きを削除し、Skill を参照する形に更新します。

### `.github/agents/meowworld-dev.agent.md` を更新

````markdown
---
description: "MeowWorld プロジェクトの機能開発を担当するエージェント。新機能の設計・実装・テストを一貫して行う。"
tools:
  - search/codebase
  - terminal
  - file
skills:
  - meowworld-patterns
---
# MeowWorld 開発エージェント

あなたは MeowWorld（猫カフェ管理システム）の専任開発者です。

## 新機能の開発プロセス

新機能のリクエストを受けたら、以下の順序で作業してください：

### Step 1: 設計
- 必要なモデル変更を特定する
- 既存のモデル（Cat.cs）との関係を明確にする
- マイグレーションが必要か判断する

### Step 2: 実装
- モデル → DbContext → コントローラー → ビュー の順で実装する
- プロジェクトのアーキテクチャパターンに厳密に従うこと

### Step 3: テスト
- 正常系 + 異常系（最低 3 ケース）を含める
- 既存テストが壊れていないか確認する

### Step 4: 確認
- `dotnet build` でビルド確認
- `dotnet test` で全テストパス確認

## 制約
- 既存機能を壊さないこと
- 不必要なパッケージを追加しないこと
- 一度に複数のマイグレーションを作らないこと

## 完了条件（Definition of Done）
- ビルド成功（`dotnet build`）
- テスト成功（`dotnet test`）
- 変更ファイル一覧を提示
- 要件ごとの対応状況を提示（満たした/未対応）

## 返信フォーマット
最終返信は以下の順序で簡潔に報告すること：
1. 実装サマリ（3-5行）
2. 変更ファイル一覧
3. 実行した検証コマンドと結果
4. 未対応事項またはリスク（なければ「なし」）
````

### `#file` 直書きとの違い

| 比較項目 | `#file` 参照 | Skill |
|---------|-------------|-------|
| コンテキスト読み込み | **毎回全文** | **必要時のみ** |
| Agent 本体のサイズ | ナレッジ分だけ肥大化 | スリムに保てる |
| 複数ナレッジの管理 | `#file` が増え続ける | Skill を追加するだけ |
| 再利用性 | その Agent 専用 | **複数の Agent で共有可能** |
| メンテナンス | Agent 変更 = ナレッジ変更 | **独立して更新可能** |

> **ポイント:** `.agent.md` を「何をするか（プロセス）」に集中させ、「何を知っているか（ナレッジ）」は Skill に分離します。これは責務の分離（SoC）そのものです。

---

## 7. Skill 対応 Agent で動作確認

更新した Agent で、先ほどと同じ機能追加を試みます：

> 🕵️ **Agent モード** — `@meowworld-dev`

```
@meowworld-dev 猫のお気に入り登録機能を追加してください。
ユーザーがお気に入りボタンを押すと、その猫がお気に入りリストに追加されます。

要件:
- Cat に対してお気に入り状態（IsFavorite: bool）を管理できること
- 一覧画面（Views/Cats/Index.cshtml）でお気に入りの切り替え操作ができること
- 既存の CRUD 機能を壊さないこと
- テストを追加し、dotnet test が通ること
```

**確認ポイント:**
- Agent が Skill の内容（ファイル配置・命名規約）に従っているか
- `.agent.md` 自体はスリムになったが、出力品質は維持されているか
- Skill のナレッジ（新機能追加パターン）に沿った手順で実装しているか

### 全体の比較

| 観点 | 素の Agent | Custom Agent（`#file`） | Custom Agent + Skill |
|------|-----------|------------------------|---------------------|
| 作業プロセスの一貫性 | 指示に依存 | ◎（Agent に固定） | ◎（Agent に固定） |
| プロジェクト知識の扱い | 必要に応じて検索 | △（毎回 `#file` 参照） | ◎（必要時に Skill 参照） |
| Token 効率 | タスク次第 | △（毎回全文読込） | ◎（必要時のみ） |
| ナレッジのメンテナンス性 | — | △（Agent と知識が結合） | ◎（Skill 単独で更新） |
| 複数 Agent での知識共有 | — | ✕（個別に `#file`） | ◎（同じ Skill を参照） |

---

## 8. Custom Agent を活用して機能を完成させる

Before/After の比較が終わったら、Custom Agent で生成した「お気に入り機能」を採用して実装を完成させます：

> 🕵️ **Agent モード** — `@meowworld-dev`

```
@meowworld-dev ビルドとテストが通ることを確認して、
何か問題があれば修正してください。
```

---

## 応用：他のエージェントとスキルのアイデア

同じ仕組みで様々な専門エージェント・スキルを作れます：

| エージェント名 | 用途 | 組み合わせる Skill |
|--------------|------|-------------------|
| `@reviewer` | コードレビューの観点でフィードバック | `review-checklist` |
| `@security` | セキュリティ観点のチェック | `owasp-patterns` |
| `@docs` | API ドキュメント / README の生成 | `documentation-standards` |
| `@migration` | DB マイグレーション専門 | `meowworld-patterns`（共有） |

> **ポイント:** `@migration` が `meowworld-patterns` Skill を共有している点に注目してください。ナレッジを Skill として外部化しておくと、複数の Agent が同じ知識ベースを参照でき、メンテナンスも一箇所で済みます。

---

## まとめ

| 機能 | 体験内容 |
|------|---------|
| Custom Agent（`.agent.md`） | プロジェクト専用 AI アシスタントを定義 |
| Skill（`SKILL.md`） | ナレッジを外部化し、Agent が自律的に参照 |
| `#file` 参照 vs Skill | 常時ロードと必要時ロードの使い分け |
| Before/After 比較 | 再現性・説明責任・手戻りの少なさを比較 |
| 開発プロセスの標準化 | 設計→実装→テスト→確認のワークフローを共有 |

### Step 4 からの全体像の振り返り

```
Step 4: Custom Instructions    → 「ルール」を全チャットに自動適用
Step 4: .instructions.md       → 「ファイル種類別ルール」を自動適用
Step 4: .prompt.md             → 「テンプレート」を明示呼び出し
Step 9: .agent.md              → 「専門家の行動指針」を明示呼び出し
Step 9: SKILL.md               → 「専門知識の本棚」を Agent が自律参照
```

抽象度が上がるにつれ、**人間が書く指示は減り、AI の自律判断が増える**ことがわかります。

---

[前へ - ユニットテスト](../8_UnitTesting/README_JA.md) | [次へ - まとめ](../10_LessonsLearned/README_JA.md)
