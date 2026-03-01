# Custom Instructions で Copilot の出力を制御しよう（オプション）

[前へ - プロジェクト作成とSQLite導入](../3_CreateProjectAndDB/README_JA.md) | [次へ - DBレイヤー（DTO/DbContext/初期データ）実装](../4_ImplementDBLayer/README_JA.md)

> **Note:** このステップはオプションです。時間が限られている場合はスキップして[次のステップ](../4_ImplementDBLayer/README_JA.md)へ進んでも構いません。ただし、ここで Custom Instructions を設定しておくと、以降のすべてのステップで Copilot の出力品質が向上し、その効果を体感できます。

---

## Custom Instructions とは？

**Custom Instructions**（カスタム指示）は、リポジトリのルートに `.github/copilot-instructions.md` ファイルを配置することで、GitHub Copilot に対してプロジェクト固有のルールやコーディング規約を自動的に適用させる仕組みです。

通常、Copilot にコードを生成させるたびに「日本語でコメントを書いて」「このパターンで書いて」と指示する必要がありますが、Custom Instructions を設定すると、毎回のプロンプトに書かなくても自動的にルールが適用されます。

### 特徴

- リポジトリの `.github/copilot-instructions.md` に記述する
- ファイルが存在するだけで、Copilot Chat のすべての応答に自動的に適用される
- チームメンバー全員が同じルールで Copilot を利用できる
- プロンプトに毎回書く手間が省ける

> **参考:** [GitHub Docs - Adding repository custom instructions for GitHub Copilot](https://docs.github.com/en/copilot/customizing-copilot/adding-repository-custom-instructions-for-github-copilot)

---

## Before: Custom Instructions なしでの出力を確認

まず、Custom Instructions を設定する **前** の Copilot の出力を確認しましょう。

Copilot Chat を **質問モード** にして、以下のプロンプトを入力してください：

```
ASP.NET Core MVC で猫の情報（名前・年齢・品種）を表す DTO クラスを作成してください。
```

出力されたコードを観察してください：
- コメントは何語で書かれていますか？
- 名前空間の形式は `namespace MeowWorld.Models { ... }`（ブロック形式）ですか？ `namespace MeowWorld.Models;`（file-scoped）ですか？
- プロパティに `required` キーワードや null 許容型の考慮はありますか？

この出力を覚えておいてください。Custom Instructions 設定後に同じプロンプトを再度試し、変化を確認します。

---

## Custom Instructions ファイルの作成

### 手順

1. プロジェクトのルートディレクトリに `.github` フォルダーを作成します

2. `.github/copilot-instructions.md` ファイルを作成し、以下の内容を記述します：

```markdown
# MeowWorld プロジェクト - Copilot カスタム指示

## 全般

- 日本語で回答すること

## 言語・コメント規約
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
```

> **Note:** Visual Studio 2026 では、**ツール > オプション > GitHub Copilot > Custom Instructions** から `.github/copilot-instructions.md` の参照が有効になっていることを確認してください。VS Code では設定で `github.copilot.chat.codeGeneration.useInstructionFiles` が有効になっていることを確認してください。

上記はこのワークショップ用のサンプルです。実際のプロジェクトでは、以下のようなチームのコーディング規約やアーキテクチャ方針を記載することが一般的です。今回のワークショップでもカスタマイズしてみましょう。

- OS の種類、バージョン
- フレームワーク、バージョン
- 依存パッケージのリスト、各バージョン
- 新しいパッケージを追加する場合の判断基準 など

---

## After: Custom Instructions ありでの出力を確認

ファイルを保存したら、Copilot Chat を新しいチャットセッションで開き直して、先ほどと同じプロンプトを入力してください：

```
ASP.NET Core MVC で猫の情報（名前・年齢・品種）を表す DTO クラスを作成してください。
```

### 確認ポイント

Before と After を比較して、以下の変化を確認しましょう：

| 観点 | Before（設定前） | After（設定後） |
|------|-----------------|----------------|
| コメントの言語 | 英語（多くの場合） | 日本語 |
| 名前空間の形式 | ブロック形式が多い | file-scoped（`namespace X;`） |
| null 安全性 | 考慮なし（多くの場合） | `required` や `string?` を使用 |

> **Note:** Copilot の出力は毎回異なる場合があります。必ずしもすべての項目で変化が見られるとは限りませんが、Custom Instructions の影響が出力に反映されていることを確認してください。

> **Tip:** 以降のステップで Copilot が生成するコードに Custom Instructions の効果が反映されているか、意識しながら進めてみましょう。模範コードとの違いも楽しんでください。

---

[前へ - プロジェクト作成とSQLite導入](../3_CreateProjectAndDB/README_JA.md) | [次へ - DBレイヤー（DTO/DbContext/初期データ）実装](../4_ImplementDBLayer/README_JA.md)
