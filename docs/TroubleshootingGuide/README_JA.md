# トラブルシューティングガイド（SQLite/EF Core）

## 問題: SQLiteファイルがロックされている/アクセスできない

- アプリケーションが複数起動していないか確認してください。
- DBファイル（例: cats.db）を削除して再作成する場合は、アプリを完全に停止してから行ってください。

## 問題: マイグレーションや初期データ投入でエラーが出る

- `dotnet ef migrations add InitialCreate` などのコマンドが失敗する場合、パッケージのバージョンやDbContextの設定を見直してください。
- `Microsoft.EntityFrameworkCore.Sqlite` パッケージが正しくインストールされているか確認してください。

## 問題: パッケージの不整合やビルドエラー

- `dotnet restore` で依存関係を再取得してください。
- プロジェクトのターゲットフレームワーク（`.csproj` 内の `<TargetFramework>`）が正しいか確認してください（.NET 10 の場合は `net10.0`、.NET 8 の場合は `net8.0`）。
- EF Core 関連パッケージ（`Microsoft.EntityFrameworkCore.Sqlite`、`Design`、`Tools`、`InMemory`）と `dotnet-ef` ツールのバージョンが、ターゲットフレームワークと一致しているか確認してください。.NET 10 なら 10.x、.NET 8 なら 8.x で統一する必要があります。

## 問題: データが表示されない/保存されない

- DBファイルのパスが正しいか、`appsettings.json` の接続文字列を確認してください。
- 初期データ投入処理が正しく動作しているか、`Program.cs` の該当部分を見直してください。

---

困ったときはCopilot Chatでエラーメッセージや状況を伝えて相談しましょう。
