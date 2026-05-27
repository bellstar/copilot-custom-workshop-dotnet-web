# Token 節約とコンテキスト管理

[前へ - Custom Instructions](../4_CustomInstructions/README_JA.md) | [次へ - DB レイヤー実装](../6_ImplementDBLayer/README_JA.md)

Copilot が正確な出力を返すかどうかは、**どのようなコンテキスト（文脈情報）を渡すか** に大きく左右されます。このステップでは、効率的にコンテキストを管理し、Token を節約しながら高品質な出力を得る方法を学びます。

---

## なぜコンテキスト管理が重要なのか

| 問題 | 原因 | 結果 |
|------|------|------|
| 出力が的外れ | 必要な情報が不足 | 既存コードと矛盾する提案 |
| 応答が遅い | 不要な情報を大量に送信 | Token 消費が増大 |
| 途中で切れる | コンテキストウィンドウの上限超過 | 不完全なコード生成 |

**目標:** 必要十分な情報だけを渡し、正確かつ高速な出力を得る。

---

## 比較実験：コンテキストの渡し方で出力がどう変わるか

以下の 3 パターンで同じ質問をして、違いを体感してください。

### 実験用プロンプト

「`appsettings.json` の接続文字列を使って AppDbContext を DI に登録するコードを書いて」

---

### パターン A: コンテキストなし

```
appsettings.json の接続文字列を使って AppDbContext を DI に登録するコードを書いて
```

**予想される問題:**
- 接続文字列のキー名がわからないので推測で書く
- `appsettings.json` のフォーマットを仮定する
- プロジェクト名や namespace が不正確になる

---

### パターン B: `#codebase` で全体を渡す

```
#codebase appsettings.json の接続文字列を使って AppDbContext を DI に登録するコードを書いて
```

**特徴:**
- Copilot がワークスペース全体を検索して関連ファイルを見つける
- 正確な出力が得られやすいが、検索・処理に時間がかかる
- 大規模プロジェクトでは Token 消費が大きい

---

### パターン C: `#file` でピンポイント指定

```
#file:app/MeowWorld/appsettings.json と #file:app/MeowWorld/Program.cs を参照して、
AppDbContext を DI に登録するコードを Program.cs に追加して
```

**特徴:**
- 必要なファイルだけを明示的に渡す
- 最も正確かつ高速
- Token 消費が最小限

---

### 比較結果

| 観点 | A: なし | B: #codebase | C: #file |
|------|---------|-------------|----------|
| 出力の正確さ | △ 推測が多い | ○ 正確 | ◎ 最も正確 |
| 応答速度 | ◎ 速い | △ やや遅い | ○ 速い |
| Token 消費 | ◎ 少ない | △ 多い | ○ 少ない |
| 適切な場面 | 一般的な質問 | 全体構造の把握が必要な時 | 特定ファイルの操作 |

---

## ベストプラクティス

### 1. 必要なファイルだけを `#file` で指定する

```
❌ プロジェクト全体のコンテキストで CRUD コントローラーを作って

✅ #file:app/MeowWorld/Models/CatDto.cs と #file:app/MeowWorld/Data/AppDbContext.cs を参照して
   CatDto の CRUD コントローラーを作って
```

### 2. Custom Instructions で暗黙のコンテキストを持たせる

Step 4 で設定した `.github/copilot-instructions.md` により、以下を毎回書く必要がなくなっています：

```
❌ C# で file-scoped namespace を使い、日本語コメントで、async/await を使って…

✅ CatDto の CRUD コントローラーを作って
   （↑ Custom Instructions が自動適用されるので、規約の記述は不要）
```

### 3. プロンプトは簡潔に、意図を明確に

```
❌ ASP.NET Core MVC の .NET 10 プロジェクトで、Entity Framework Core を使って、
   SQLite データベースに接続して、猫の情報を管理する Web アプリケーションの
   コントローラーを作成してください。コントローラーには一覧表示、詳細表示、
   新規作成、編集、削除の機能を含めてください。

✅ CatDto の CRUD コントローラーを作って。全アクション async で。
   （↑ プロジェクト情報は Custom Instructions にある。必要な指示だけ書く）
```

### 4. Agent モードの自律性を活かす

Agent モードでは、1 回の指示で複数の操作を完了させられます：

```
❌ (3回に分けて質問)
   1. CatDto の CRUD コントローラーを作って
   2. 対応するビューも作って
   3. ルーティングも設定して

✅ (1回で完了)
   CatDto の CRUD 機能（コントローラー + ビュー + ルーティング設定）を実装して。
   動作確認のためビルドも通して。
```

### 5. エラー発生時は `#terminalLastCommand` を活用

ターミナルでエラーが出た場合：

```
❌ (エラーメッセージをコピペ)
   以下のエラーが出ました：
   error CS0246: The type or namespace name 'AppDbContext'...

✅ #terminalLastCommand このエラーを修正して
```

---

## 使い分けチートシート

| やりたいこと | 推奨するコンテキスト指定 |
|-------------|------------------------|
| 特定ファイルを修正したい | `#file:対象ファイル` |
| 既存コードに合わせて新規作成 | `#file:参考にすべきファイル` を複数指定 |
| プロジェクト構造を踏まえた設計 | `#codebase` |
| エラー修正 | `#terminalLastCommand` または Agent に任せる |
| 一般的な知識を聞きたい | コンテキストなし（Ask モード推奨） |

---

## 演習

以下の質問をパターン C（`#file` 指定）で実行し、正確な結果が返ることを確認してください：

```
#file:app/MeowWorld/Program.cs にSQLiteのDB自動マイグレーションと
初期データ投入（猫データ5件）のコードを追加して。
#file:app/MeowWorld/appsettings.json の接続文字列を参照すること。
```

> **Note:** このコードは次の Step 6 で実際に使います。ここでは「コンテキスト指定で正確な出力が得られること」を確認するのが目的です。出力は保存せず、次のステップで改めて実装します。

---

[前へ - Custom Instructions](../4_CustomInstructions/README_JA.md) | [次へ - DB レイヤー実装](../6_ImplementDBLayer/README_JA.md)
