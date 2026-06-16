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
