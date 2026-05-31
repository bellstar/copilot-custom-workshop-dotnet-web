# ユニットテスト

[前へ - Token 節約とコンテキスト管理](../7_TokenManagement/README_JA.md) | [次へ - Custom Agent](../9_CustomAgent/README_JA.md)

このステップでは、Copilot の **`/tests` コマンド**と Agent モードを使ってユニットテストを生成・実行します。テストプロジェクトの作成から実装まで、Agent に一括で任せます。

---

## 1. テストプロジェクトの作成

Copilot Chat を **Agent モード**にして以下を入力します：

> 🕵️ **Agent モード**

```
MeowWorld に対する xUnit テストプロジェクトを MeowWorld.Tests として作成してください。
InMemory データベースプロバイダー（Microsoft.EntityFrameworkCore.InMemory）を使います。
ソリューションファイルに追加し、ビルドが通ることを確認してください。
```

Agent が以下を実行します：
- `dotnet new xunit` でテストプロジェクト作成
- プロジェクト参照の追加
- InMemory パッケージの追加
- ソリューションへの追加
- ビルド確認

---

## 2. `/tests` コマンドでテスト生成

### 使い方

テスト対象のファイルをエディタで開き（例：`CatsController.cs`）、Copilot Chat で：

> 🕵️ **Agent モード**

```
/tests
```

と入力するだけで、そのファイルのユニットテストが生成されます。

### 確認ポイント

`.instructions.md`（`applyTo: "**/*.Tests/**"`）で設定したルールが適用されているか確認してください：

- [ ] テストメソッド名が日本語（例：`猫一覧が正しく取得できること`）
- [ ] AAA パターンのコメント区切り（`// Arrange`、`// Act`、`// Assert`）
- [ ] InMemory データベースを使用
- [ ] テストごとに一意の DB 名

> **Note:** `/tests` は Copilot Chat で入力するコマンドです（ターミナルで実行するものではありません）。Ask モードでも使えますが、Agent モードであれば生成されたテストファイルの配置・ビルド・実行まで自動で行います。

---

## 3. Agent モードでテストを充実させる

`/tests` で基本テストが生成された後、追加のシナリオを Agent に依頼します：

> 🕵️ **Agent モード**

```
CatsController のテストに以下のケースを追加してください：
- 存在しない ID で Details を呼んだ場合に NotFound が返ること
- Name が空文字で Create した場合に ModelState エラーになること
- 正常に Edit した後、変更が DB に反映されていること

テストを実行して全件パスすることを確認してください。
```

Agent が以下を行います：
1. テストケースを追加
2. `dotnet test` を実行
3. 失敗があれば自動的に修正
4. 全件パスを確認

---

## 4. テスト失敗時の自動修正を体験する

Agent モードの真価は「テストが失敗したときの自動修正」にあります。

### 意図的にテストを失敗させる

`CatsController.cs` の `Index` アクションで返す猫の順序を変更する等、小さな変更を加えてからテストを実行してみます：

> 🕵️ **Agent モード**

```
dotnet test を実行して。失敗したテストがあれば修正してください。
```

Agent がテストの失敗メッセージを解析し、テストコードまたは本体コードの修正を提案します。

> **学びのポイント:** エラー修正のフローが完全に自動化されていることを体感してください。

---

## 5. 期待される成果物

```
MeowWorld.Tests/
├── MeowWorld.Tests.csproj
├── CatsControllerTests.cs  （またはControllers/CatsControllerTests.cs）
└── GlobalUsings.cs
```

> **Note:** テストファイル名やサブフォルダ構成は Copilot の生成結果によって異なります（例: `UnitTest1.cs` に生成される場合もあります）。重要なのはファイル配置ではなく、**テスト内容が要件を満たしていること**と **`dotnet test` が全件パスすること**です。

テスト結果の例：

```
Passed!  - Failed:     0, Passed:     X, Skipped:     0, Total:     X
```

---

## リファレンス：テストの例

<details>
<summary>CatsControllerTests.cs の例（クリックで展開）</summary>

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MeowWorld.Controllers;
using MeowWorld.Data;
using MeowWorld.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeowWorld.Tests;

/// <summary>
/// CatsController のユニットテスト
/// </summary>
public class CatsControllerTests
{
    private static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    private static CatsController CreateController(AppDbContext context)
    {
        return new CatsController(context, NullLogger<CatsController>.Instance);
    }

    [Fact]
    public async Task 猫一覧が正しく取得できること()
    {
        // Arrange
        using var context = CreateContext(nameof(猫一覧が正しく取得できること));
        context.Cats.AddRange(
            new Cat { Name = "みけ", Age = 3, Breed = "三毛猫" },
            new Cat { Name = "くろ", Age = 5, Breed = "黒猫" }
        );
        await context.SaveChangesAsync();
        var controller = CreateController(context);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Cat>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task 存在しないIDでNotFoundが返ること()
    {
        // Arrange
        using var context = CreateContext(nameof(存在しないIDでNotFoundが返ること));
        var controller = CreateController(context);

        // Act
        var result = await controller.Details(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
```

</details>

---

## まとめ

| 機能 | 体験内容 |
|------|---------|
| `/tests` | ファイルを開くだけでテスト自動生成 |
| Agent モード | テストプロジェクト作成〜実行〜修正まで一気通貫 |
| `.instructions.md` | テスト命名規約（日本語）・AAA パターンが自動適用 |
| 自動修正 | テスト失敗 → 原因分析 → 修正 → 再実行を Agent が自律的に実行 |

---

[前へ - Token 節約とコンテキスト管理](../7_TokenManagement/README_JA.md) | [次へ - Custom Agent](../9_CustomAgent/README_JA.md)
