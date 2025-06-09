<!-- filepath: docs_dotnet/4_StoringPostgreSQLImageRegistry/README_JA.md -->
# PostgreSQLパッケージをGitHub Packagesにデプロイしよう

[前へ - PostgreSQLデータベースレイヤーを構築しよう](../3_BuildPostgreSQL/README_VS2022_JA.md) | [次へ - .NET MVCアプリケーション（DTOパターン）をPostgreSQLとともに構築しよう](../5_BuildDotNetMVC/README_JA.md)

前のステップでは、ローカルのDockerコンテナでPostgreSQLデータベースを構築・実行しました。今度は、チームで共有したり他の環境にデプロイできるよう、コンテナイメージをイメージレジストリに登録します。

このアーキテクチャ図を見てください。PostgreSQLクライアントがコンテナ化されているので、サポートされているイメージレジストリに保存できます。

![Architecture diagram](./images/0_ArchitecturePostgreSQL.jpg)

イメージレジストリ（GitHub PackagesやAzure Container Registryなど）は、他のプロジェクトや自動デプロイで利用できるコンテナイメージを保存・共有する場所です。

このワークショップではGitHub Packages（GHCR）を使います。他のレジストリでも手順はほぼ同じです。

Copilot Chatで次のように聞いてみましょう（<REPLACE WITH YOUR ORG NAME> の部分はご自身の組織名に置き換えてください）:

```
"custom-database-layer:2.0" という名前のローカルコンテナイメージがあります。これを自分の組織 "<REPLACE WITH YOUR ORG NAME>" のGHCR GitHub Packagesレジストリにデプロイするにはどうすればよいですか？
```

Copilotが手順を教えてくれます。

![Copilot Build Container](./images/1_CopilotBuildContainer_vs.jpg)

まずはローカルイメージにタグを付けます：

```bash
docker tag custom-database-layer:2.0 ghcr.io/YOUR-ORG/custom-database-layer:2.0
```

次に、パッケージ権限付きのPersonal Access Token（PAT）を作成し、GHCRにログインします。

![Create PAT token](./images/2_CreatePAT.jpg)

```bash
docker login ghcr.io -u USERNAME -p TOKEN
```

イメージをプッシュします：

```bash
docker push ghcr.io/YOUR-ORG/custom-database-layer:2.0
```

これでイメージが組織のレジストリに保存されます。

自動化したい場合はGitHub Actionsを使いましょう。Copilot Chatで次のように聞いてみてください（<REPLACE WITH YOUR ORG NAME> の部分はご自身の組織名に置き換えてください）:

```
素晴らしいです。ただし、ローカルでビルドするのではなく、GitHub Actionsが自動的に "Dockerfile" と "create-data.sql" を取得してDockerコンテナをビルドし、私の組織 "<REPLACE WITH YOUR ORG NAME>" のGHCR GitHub PackageコンテナレジストリにプッシュできるGitHubリポジトリを作成する方法を教えてもらえますか？
```

Copilotがリポジトリ作成やワークフロー設定を案内してくれます。

![Action to automate PostgreSQL container build](./images/3_CopilotAutomatingBuild_vs.jpg)

Actionファイルは次のようになるかもしれません。ビルドアクションがPostgreSQLデータベースの引数を取っていないことに注意してください。

![Copilot generated Actions file](./images/4_CopilotGeneratedActions.jpg)

手順に従ってリポジトリを作成しましょう。あなたのリポジトリは次のようになるかもしれません。

![Repository with Actions file](./images/5_RepositoryPostgreSQL.jpg)

Actionsを実行すると、PostgreSQLのビルドを完全に自動化するワークフローができあがります。

![Actions build](./images/6_SuccessActions.jpg)

ワークフローは自動でイメージをビルド＆プッシュします。ビルド引数（DB認証情報など）を渡したい場合は、`--build-arg`オプションを追加する必要があります。

### GitHub Actionsでbuild-argを追加する方法

1. リポジトリをクローンし、`AUTOMATION`フォルダなどに配置します。
2. `.github/workflows`フォルダ内のActionsワークフローファイルをエディタで開きます。

![Actions file that need to be modified](./images/8_NeedFixingAction_vs.jpg)

3. `docker build`のステップに`--build-arg`を追加します。やり方が分からない場合は[build-push-actionのGitHubリポジトリ](https://github.com/docker/build-push-action)を参照してください。

![build-push-action Repository](./images/9_BuildPushActionRepository.jpg)

下の方に`build-args`の使い方が載っています。

![Build Args](./images/10_BuildArgsInstruction.jpg)

Copilotに聞いても良いですが、公式ドキュメントを参考に自分で判断することも大切です。

実際に`--build-arg`を追加した例：

![GitHub Copilot to the rescue](./images/11_CopilotSuggestionActions.jpg)

最終的なActionsファイル例。`docker build`コマンドに`--build-arg`が追加され、必要に応じてGitHub Secretsも設定します。

![Modified Actions file](./images/12_FinalActions.jpg)

[前へ - PostgreSQLデータベースレイヤーを構築しよう](../3_BuildPostgreSQL/README_VS2022_JA.md) | [次へ - .NET MVCアプリケーション（DTOパターン）をPostgreSQLとともに構築しよう](../5_BuildDotNetMVC/README_JA.md)
