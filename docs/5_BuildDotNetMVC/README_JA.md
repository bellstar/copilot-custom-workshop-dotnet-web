<!-- filepath: docs_dotnet/5_BuildDotNetMVC/README_JA.md -->
# .NET MVCアプリケーション（DTOパターン）をPostgreSQLとともに構築しよう

[前へ - PostgreSQLパッケージをGitHub Packagesにデプロイしよう](../4_StoringPostgreSQLImageRegistry/README_JA.md) | [次へ - 単体テストを追加しよう](../6_UnitTestingBackend/README_JA.md)

前のステップでPostgreSQLコンテナイメージをビルド・公開しました。ここでは、DTO（Data Transfer Object）パターンを用いて、PostgreSQLと連携する.NET 8（ASP.NET Core MVC）アプリケーションを構築します。

> このステップは `app` ディレクトリ内で作業してください。

Copilot Chatのプロンプトに次のように入力しましょう。

```
.NET 8をターゲットにした "MeowWorld" というASP.NET Core MVCプロジェクトを新規作成するには？
```

Copilotがプロジェクトの作成方法を指示してくれますので、手順に従ってプロジェクトを作成します。

![Copilot Asking for MVC](./images/0_CopilotAskProject.jpg)

次の手順からは、作成したソリューションのCopilot Chatで作業をします。

プロンプトに次のように入力し、アプリケーションの実装手順を質問します。
ソリューション全体についての質問のため、ソリューション全体をコンテキストに含めます。

> **Note:** ソリューション全体を含めるドロップダウンは、Visual Studio 2022のバージョン17.14.5以降が必要です。

![Copilot Add Context](./images/1_CopilotAddContents.jpg)

>**TIPS**
>独立したソリューション内で質問するため、プロンプトにはDB情報（DockerfilesとSQL文）を渡す必要がありますので、工夫して渡してみましょう。

```
PostgreSQLサーバーに接続し、DTOパターンでテーブルデータを取得して、画面に一覧として表示するアプリケーションを作成するにはどのようなステップで実装すればよいですか？
RazorPageパターンではなく、MVCパターンで実装をしたいです。
```

Copilotが手順を教えてくれます。

![Copilot Asking for Implementation](./images/2_CopilotAskImpl.jpg)

パッケージのインストールをします。

![Install Pacakges](./images/3_InstallPackages.jpg)

接続文字列の設定をします。

![Setting appsettings](./images/3_1_SettingAppsettings.jpg)

DbContextとDTOを適用しながら進めます。

>**NOTE**
>DTOはDBアクセス方式に依存しませんので、ほかのアクセス方法がよい場合は、指示を与えて手順を聞いてみてください。

![Impl DBContext](./images/4_ImplDTO.jpg)

![Impl DTO](./images/4_1_ImplDTO.jpg)

DIコンテナにDbContextを登録します。

![Impl DI Container](./images/5_ImplDIContainer.jpg)

コントローラーの実装をします。

![Impl Controller](./images/6_ImplController.jpg)

ビューを実装します。

![Impl View](./images/7_ImplView.jpg)

Catsリストを初期表示させるために、ルーティングを変更します。

![Impl Routing](./images/8_ImplRouting.jpg)

アプリケーションの実行とテスト

PostgreSQLコンテナが起動していることを確認し、デバッグ実行をして動作を確認します。
この例では、エラーが表示されました。

![Debug Error](./images/9_DebugError.jpg)

エラーの内容を添付してCopilotに質問することで、考えられる原因を挙げてくれます。

![Insight Error](./images/10_InsightError.jpg)

今回は大文字・小文字の問題ですので、修正したコードを依頼します。

![Update DbContext](./images/11_UpdateDbContext.jpg)

再度デバッグ実行し、想定したテーブルデータの一覧が表示されることを確認します。

![Cats List](./images/12_CatsList.jpg)

> **TIP:**  
> 各ステップでCopilot Chatを活用し、よりよいプロンプトやコード提案を試してみましょう。生成されたコードは必ず内容を確認し、正しさ・セキュリティも意識してください。

### ロゴの追加

一覧画面を華やかにするために、プロンプトに以下のように入力してMeowWorldのロゴを表示する方法を尋ねます。

```
CatsListの上に、MeowWorldのロゴを表示させたいです。どのようにすればよいですか？
```

![Impl Logo](./images/13_ImplLogo.jpg)

ロゴの画像は、M365 Copilotなどを利用して出力することもできます。

![Create Logo](./images/14_CreateLogo.jpg)

![Cats List with Logo](./images/15_CatsListwithLogo.jpg)

[前へ - PostgreSQLパッケージをGitHub Packagesにデプロイしよう](../4_StoringPostgreSQLImageRegistry/README_JA.md) | [次へ - 単体テストを追加しよう](../6_UnitTestingBackend/README_JA.md)
