# トラブルシューティングガイド

## Copilot 関連

### Agent モードが選択できない
- Copilot Chat のバージョンが最新か確認してください
- VS Code の場合：拡張機能「GitHub Copilot Chat」を最新にアップデート
- VS 2026 の場合：ツール > 拡張機能の管理 で更新を確認

### Custom Instructions が反映されない
- **VS Code:** 設定で `github.copilot.chat.codeGeneration.useInstructionFiles` が `true` になっているか確認
- `.github/copilot-instructions.md` のパスが正しいか確認（ワークスペースルート直下の `.github/` フォルダー内）
- **新しいチャットセッション**を開いてから試す（既存セッションでは反映されない場合がある）

### `.instructions.md` の `applyTo` が効かない
- グロブパターンが正しいか確認（例：`**/*.cshtml`、`**/*.Tests/**`）
- ファイルがワークスペース内にあるか確認
- 該当パターンのファイルを**編集対象として操作している**か確認（閲覧だけでは適用されない）

### Custom Agent（`@agent名`）が表示されない
- `.github/agents/` ディレクトリに `.agent.md` ファイルが存在するか確認
- YAML フロントマター（`---` で囲まれた部分）の構文エラーがないか確認
- Copilot Chat を再起動してみてください

### `/tests` や `/fix` が動作しない
- 対象ファイルがエディタで開かれているか確認
- Copilot Chat 拡張機能が最新か確認

---

## 開発環境関連

### SQLite ファイルがロックされている / アクセスできない
- アプリケーションが複数起動していないか確認
- DB ファイル（`meowworld.db`）を削除して再作成する場合は、アプリを完全に停止してから行ってください

### マイグレーションでエラーが出る
- `dotnet-ef` ツールがインストールされているか確認：`dotnet tool list --global`
- 未インストールの場合：`dotnet tool install --global dotnet-ef`
- パッケージバージョンとターゲットフレームワークが一致しているか確認（.NET 10 → EF Core 10.x、.NET 8 → EF Core 8.x）

### ビルドエラー
- `dotnet restore` で依存関係を再取得
- `.csproj` の `<TargetFramework>` を確認（`net10.0` or `net8.0`）
- EF Core 関連パッケージのバージョンが統一されているか確認

### データが表示されない / 保存されない
- `appsettings.json` の接続文字列を確認：`"Data Source=meowworld.db"`
- `Program.cs` に自動マイグレーション適用コードがあるか確認
- `meowworld.db` ファイルが生成されているか確認

---

## 一般的なアドバイス

- **困ったらまず Copilot に聞く:**  Agent モードでエラーメッセージを貼り付けるか、`#terminalLastCommand` を参照させてください
- **チャットをリセットする:** 状態がおかしくなった場合は新しいチャットセッションを開始
- **ビルドし直す:** `dotnet clean` → `dotnet build` で状態をクリーンにする
