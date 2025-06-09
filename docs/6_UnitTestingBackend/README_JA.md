<!-- filepath: docs_dotnet/6_UnitTestingBackend/README_JA.md -->
# 単体テストを追加しよう

[前へ - .NET MVCアプリケーション（DTOパターン）をPostgreSQLとともに構築しよう](../5_BuildDotNetMVC/README_JA.md) | [次へ - 学びと振り返り](../7_LessonsLearned/README_JA.md)

このステップでは、xUnitを使って.NET 8 MVCバックエンドの単体テストプロジェクトを作成し、GitHub Copilotの力を借りてテストコードを作成・改善します。

まずは、テストプロジェクトの作成を作成する方法を、次のようにCopilot Chatで聞いてみましょう：

```
既存の.NET 8 MVCソリューションにxUnitテストプロジェクトを追加するには？
```

Copilotが教えてくれるので、指示に沿って進めます。

![Copilot Ask Test step](./images/0_CopilotAskTestStep.jpg)

テストプロジェクトを作成して、本体プロジェクトや依存パッケージを追加します。

![Install Packages](./images/1_InstallPackages.jpg)

コントローラーのテストを追加します。

![Impl UnitTest](./images/2_ImplUnitTest.jpg)

実装が終わったら単体テストを実行します。

![Run UnitTest](./images/3_RunUnitTest.jpg)

> **Note:** ビルドエラーになる場合は、修正の仕方をCopilot Chatに相談します。

```
ビルドエラーの原因を考えて解決策を提示してください。
```

追加テストの作成するように依頼します。

Copilot Chatを活用して、
- 正常・異常系の入力テスト
- CRUD操作（作成・取得・更新・削除）
- DTOマッピングのテスト
なども作成しましょう。

プロンプト例：

```
テストのカバレッジを100%になるようにテストを追加してください。また、異常系のテストも追加してください。
```

![Add UnitTest](./images/4_AddUnitTest.jpg)

## Copilotで改善・リファクタリング

- Moqが利用できるようにするにはどうしたらよいでしょうか。
- ASP.NET Coreの単体テストのベストプラクティスも質問してみましょう。

---

[前へ - .NET 8 MVCアプリケーション（DTOパターン）をPostgreSQLとともに構築しよう](../5_BuildDotNetMVC/README_JA.md) | [次へ - 学びと振り返り](../7_LessonsLearned/README_JA.md)
