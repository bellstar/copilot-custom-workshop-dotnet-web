<!-- filepath: docs_dotnet/5_BuildDotNetMVC/README_JA.md -->
# MVCコントローラー・ビュー実装（SQLite/DTOパターン）

[前へ - DBレイヤー（DTO/DbContext/初期データ）実装](../4_ImplementDBLayer/README_JA.md) | [次へ - 単体テストを追加しよう](../6_UnitTesting/README_JA.md)

---

## GitHub Copilot Chat推奨プロンプト（このステップ全体をまとめて質問）

まずは以下のようにCopilot Chatに質問して、MVCコントローラー・ビュー実装の全体手順をまとめて提案してもらいましょう。

```
ASP.NET Core MVC（.NET 8）で、SQLite＋Entity Framework CoreのDBレイヤーを使って猫情報の一覧表示・追加・編集・削除ができるMVCコントローラーとビューを実装したいです。
- CatsControllerの作成
- 一覧表示用ビューの作成
- 必要に応じてCreate/Edit/Details/Deleteアクションも
この要件を満たすための手順と主要なコード例を教えてください。
```

Copilotが全体の流れや主要なコード例をまとめて提案してくれます。

![Copilot Ask Steps](./images/0_CopilotAskSteps.png)

> **TIP:**  
> 開発者として「動けばOK」ではなく、セキュリティ・正しさ・効率性も常に意識しましょう。Copilotはあくまでアシスタント、主役はあなたです。

コード完成後にデバッグ実行などでアプリを起動し、猫の一覧が表示されることを確認します。

![Run App](./images/1_RunApp.png)

> **TIP:**  
> 各ステップでCopilot Chatを活用し、よりよいプロンプトやコード提案を試してみましょう。生成されたコードは必ず内容を確認し、正しさ・セキュリティも意識してください。

### ロゴの追加

一覧画面を華やかにするために、プロンプトに以下のように入力してMeowWorldのロゴを表示する方法を尋ねます。

```
Cats Listの上に、MeowWorldのロゴを表示させたいです。どのようにすればよいですか？
```

![Impl Logo](./images/3_ImplLogo.jpg)

ロゴの画像は、M365 Copilotなどを利用して出力することもできます。

![Create Logo](./images/4_CreateLogo.jpg)

ロゴを追加して実行します。

![Run App with Logo](./images/5_RunAppWithLogo.png)

---

## 参考：細分化した質問例と模範コード

### 1. コントローラーの作成

```
猫情報の一覧表示・追加・編集・削除ができるCatsControllerを作成するには？
```

```csharp
using Microsoft.AspNetCore.Mvc;
using MeowWorld.Data;
using MeowWorld.Models;

namespace MeowWorld.Controllers
{
    public class CatsController : Controller
    {
        private readonly AppDbContext _context;
        public CatsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var cats = _context.Cats.ToList();
            return View(cats);
        }
        // Create/Edit/Details/Delete も必要に応じて追加
    }
}
```

---

### 2. ビューの作成

```
CatsControllerのIndexアクションに対応する一覧表示用ビュー（Views/Cats/Index.cshtml）を作成するには？
```

```html
@model IEnumerable<MeowWorld.Models.CatDto>
@{
    ViewData["Title"] = "猫一覧";
}

<h1>猫一覧</h1>
<p>
    <a asp-action="Create" class="btn btn-primary">新規追加</a>
</p>
<table class="table">
    <thead>
        <tr>
            <th>名前</th>
            <th>年齢</th>
            <th>品種</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
@foreach (var item in Model)
{
        <tr>
            <td>@item.Name</td>
            <td>@item.Age</td>
            <td>@item.Breed</td>
            <td>
                <a asp-action="Details" asp-route-id="@item.Id">詳細</a> |
                <a asp-action="Edit" asp-route-id="@item.Id">編集</a> |
                <a asp-action="Delete" asp-route-id="@item.Id">削除</a>
            </td>
        </tr>
}
    </tbody>
</table>
```

---

### 3. ルーティングの設定

```
アプリのデフォルトルートをCats/Indexに変更するには？
```

```csharp
// ...existing code...
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cats}/{action=Index}/{id?}");
// ...existing code...
```

---

### 4. 動作確認

```
アプリを起動し、猫リストが表示されることを確認するには？
```

---

## Advanced: 各アクション用 .cshtml ファイルの作成

猫情報の新規作成・編集・削除・詳細表示のためのビュー（Create/Edit/Delete/Details）も、Copilot Chatを活用して効率的に作成してみましょう。

### Copilot Chatプロンプト

```
CatsControllerのCreate/Edit/Details/Deleteアクションに対応する .cshtml ビュー（Views/Cats/）を作成したいです。各アクションに必要なコード例を教えてください。
```

Copilot Chatの提案を参考に、各 .cshtml ファイル（Create.cshtml, Edit.cshtml, Details.cshtml, Delete.cshtml）を作成しましょう。  
Visual Studioのスキャフォールディング機能を使って自動生成する方法も便利です。

具体例：Create.cshtml

```html
@model MeowWorld.Models.CatDto
@{
    ViewData["Title"] = "猫の新規作成";
}

<h1>猫の新規作成</h1>

<form asp-action="Create" method="post">
    <div class="form-group">
        <label asp-for="Name" class="control-label"></label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="Age" class="control-label"></label>
        <input asp-for="Age" class="form-control" />
        <span asp-validation-for="Age" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="Breed" class="control-label"></label>
        <input asp-for="Breed" class="form-control" />
        <span asp-validation-for="Breed" class="text-danger"></span>
    </div>
    <button type="submit" class="btn btn-primary">作成</button>
    <a asp-action="Index" class="btn btn-secondary">戻る</a>
</form>

@section Scripts {
    @{
        await Html.RenderPartialAsync("_ValidationScriptsPartial");
    }
}
```

> **TIP:**  
> 生成されたコードは必ず内容を確認し、必要に応じてカスタマイズしましょう。  
> Copilot Chatに「Edit用のフォームをもっとシンプルにしたい」など追加で質問するのもおすすめです。

---

[前へ - DBレイヤー（DTO/DbContext/初期データ）実装](../4_ImplementDBLayer/README_JA.md) | [次へ - 単体テストを追加しよう](../6_UnitTesting/README_JA.md)
