# トラブルシューティングガイド

## 問題: ポートが使用中でエラーが発生する場合: docker: Error response from daemon: Ports are not available: listen tcp 0.0.0.0:5432: bind: address already in use.

ターミナルを終了し、以下のコマンドを実行してください。

```bash
sudo lsof -i :5432     

sudo kill -9 <PID>
```

## 問題: Dockerコンテナ名の重複エラーが発生する場合: docker: Error response from daemon: Conflict. The container name "/custom-database-layer" is already in use by container. You have to remove (or rename) that container to be able to reuse that name.

```bash
docker ps -a

docker rm <PS ID>
```

## 問題: Dockerコンテナが起動しない場合はどうすればよいですか？

以下のようにshでイメージを起動してみてください。

```bash
docker run -it --rm --entrypoint sh コンテナイメージ名:バージョン
```

例：


```bash
docker run -it --rm --entrypoint sh custom-database-layer:2.0
```

printenvなどのコマンドでトラブルシュートしてください。
