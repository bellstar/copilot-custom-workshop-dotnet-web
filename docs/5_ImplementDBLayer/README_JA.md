# DB レイヤー実装

[前へ - Custom Instructions](../4_CustomInstructions/README_JA.md) | [次へ - MVC 実装 + Vision](../6_ImplementMVC/README_JA.md)

このステップでは、Agent モードと Custom Instructions を活用して Entity Framework Core のモデル・DbContext・マイグレーションを実装します。

> **✨ Step 4 の効果を体感するステップ:** ここで Agent が生成するコードには、Step 4 で設定した Custom Instructions（日本語コメント、file-scoped namespace、プライマリコンストラクタ等）が自動適用されます。「何も指定していないのに規約に従ったコードが出てくる」ことを確認してください。

---

## 目標

- `Cat` エンティティと `AppDbContext` を作成する
- SQLite の接続文字列を構成する
- マイグレーションを実行し、初期データを投入する
- エラーが出た場合に Agent が自動修復する流れを体験する

---

## Agent モードで実装する

### 1. モデルと DbContext の作成

> 🕵️ **Agent モード**

```
MeowWorld プロジェクトに以下を実装してください：

1. Models/Cat.cs - 猫エンティティ
   - Id (int, PK)
   - Name (string, 必須, 最大50文字)
   - Age (int, 0以上)
   - Breed (string, 必須, 最大50文字)
   - Description (string?, 任意, 最大500文字)
   - CreatedAt (DateTime, デフォルト=現在日時)

2. Data/AppDbContext.cs - DbContext
   - DbSet<Cat> Cats プロパティ
   - OnModelCreating でシードデータ5件を追加

3. appsettings.json に SQLite の接続文字列を追加
   - "ConnectionStrings": { "DefaultConnection": "Data Source=meowworld.db" }

4. Program.cs に DbContext の DI 登録を追加

ビルドが通ることを確認してください。
```

> **ポイント:** Step 4 で設定した `.github/copilot-instructions.md` が自動適用されるため、「file-scoped namespace を使え」「日本語コメントで」といった指示は不要です。

### Agent の動きを観察する

Agent は以下のような流れで作業します：

1. 必要なファイルを作成・編集
2. `dotnet build` を実行
3. エラーがあれば自動的に解析・修正
4. 再度ビルドして成功を確認

> **Note:** ビルドエラーが発生するのは自然なことです。Agent が自動で修正する様子を観察してください。これは「Ask モードでは体験できない」Agent の自律性の実演です。

---

### 2. マイグレーションの実行

ビルドが成功したら、マイグレーションを作成・適用します：

> 🕵️ **Agent モード**

```
EF Core のマイグレーションを作成して適用してください。
マイグレーション名は "InitialCreate" で。
```

Agent が以下を実行します：
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> **エラーが出た場合:** `dotnet-ef` ツールが未インストールの場合があります。Agent にそのまま任せれば、`dotnet tool install --global dotnet-ef` を実行してくれます。

---

### 3. 動作確認

> 🕵️ **Agent モード**

```
Program.cs にデータベースの自動マイグレーション適用コードを追加して、
アプリ起動時に DB が存在しなければ自動作成されるようにしてください。
その後 dotnet run で起動して動作確認してください。
```

---

## 期待される成果物

このステップ完了後、以下のファイルが存在するはずです：

```
MeowWorld/
├── Models/
│   └── Cat.cs
├── Data/
│   └── AppDbContext.cs
├── Migrations/
│   ├── YYYYMMDDHHMMSS_InitialCreate.cs
│   └── AppDbContextModelSnapshot.cs
├── appsettings.json  (接続文字列追加済み)
├── Program.cs        (DI登録 + 自動マイグレーション追加済み)
└── meowworld.db      (SQLiteファイル)
```

---

## リファレンス：期待されるコードの例

Agent が生成したコードと比較して、Custom Instructions が正しく適用されているか確認してください：

<details>
<summary>Models/Cat.cs の例（クリックで展開）</summary>

```csharp
namespace MeowWorld.Models;

/// <summary>
/// 猫のエンティティ
/// </summary>
public class Cat
{
    public int Id { get; set; }

    /// <summary>猫の名前</summary>
    [Required, MaxLength(50)]
    public required string Name { get; set; }

    /// <summary>年齢</summary>
    [Range(0, 30)]
    public int Age { get; set; }

    /// <summary>品種</summary>
    [Required, MaxLength(50)]
    public required string Breed { get; set; }

    /// <summary>説明（任意）</summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>登録日時</summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
```

**確認ポイント:**
- [x] file-scoped namespace
- [x] 日本語 XML コメント
- [x] `required` キーワード
- [x] nullable 型（`string?`）

</details>

<details>
<summary>Data/AppDbContext.cs の例（クリックで展開）</summary>

```csharp
using Microsoft.EntityFrameworkCore;
using MeowWorld.Models;

namespace MeowWorld.Data;

/// <summary>
/// アプリケーションデータベースコンテキスト
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>猫テーブル</summary>
    public DbSet<Cat> Cats => Set<Cat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // シードデータ
        modelBuilder.Entity<Cat>().HasData(
            new Cat { Id = 1, Name = "みけ", Age = 3, Breed = "三毛猫", Description = "おとなしい性格", CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 2, Name = "くろ", Age = 5, Breed = "黒猫", Description = "甘えん坊", CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 3, Name = "しろ", Age = 2, Breed = "白猫", Description = null, CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 4, Name = "チャチャ", Age = 1, Breed = "茶トラ", Description = "元気いっぱい", CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 5, Name = "ソラ", Age = 4, Breed = "ロシアンブルー", Description = "静かな環境が好き", CreatedAt = new DateTime(2024, 1, 1) }
        );
    }
}
```

**確認ポイント:**
- [x] プライマリコンストラクタ
- [x] 日本語シードデータ

</details>

---

## /fix を使ったエラー修正

Agent モードでのビルドエラー自動修正以外に、**`/fix` コマンド**も使えます。

エディタでエラーの赤波線が表示されているファイルを開き、Copilot Chat で：

```
/fix
```

と入力するだけで、現在のファイルのエラーを修正する提案が得られます。

> **使い分け:**
> - Agent モード中のエラー → Agent が自動で修正（介入不要）
> - 手動編集中のエラー → `/fix` が便利

---

[前へ - Custom Instructions](../4_CustomInstructions/README_JA.md) | [次へ - MVC 実装 + Vision](../6_ImplementMVC/README_JA.md)
