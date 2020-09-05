# 開発方法

## Core を実装

まずは Core 側 に機能を実装します。 [この辺りを参考に](https://github.com/altseed/Altseed2/blob/master/documents/development/HowToDevelop_Ja.md)。

### Bindingを生成

[やはりこの辺りを参考に](https://github.com/altseed/Altseed2/blob/master/documents/development/HowToDevelop_Ja.md) 、Engine側に公開するメソッドなどについて Binding 定義を追加します。

正しく追加できたかを確認するため、Wrapper を生成したうえで、再度 Core をビルドします。ここでビルドができたらコミットして次に進みます。

### Engine側に取り込み

`git submodule update --init --force --merge --remote -- "Core"` などしてコアを取り込みます。ここで、このあとの記述と混ざらないようにするためいったんコミットしておきます。

### ビルド

[ビルド手順](HowToBuild_Ja.md) を参考に、

- Core のビルド
- Binding の生成
- Engine のビルド

を行います。

### 動作チェック

ここまでで追加したクラスやメソッドが IntelliSense から見えるか、実際にそれを呼び出すようなコードを書いてみて正常に実行できるかをチェックします。

### 単体テストの記述

既存のテストをマネしながら書く。
実行は `テストエクスプローラ` か `dotnet Test` で

c.f.: 
- [テスト エクスプローラーを使用した単体テストの実行とデバッグ - Visual Studio](https://docs.microsoft.com/ja-jp/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019)
- [Exploring and Managing Unit Tests Using Test Explorer in Visual Studio - Daily .NET Tips](https://dailydotnettips.com/exploring-and-managing-unit-tests-using-test-explorer-in-visual-studio/)

### スクリーンショット比較

Altseed2では、テストをスクリーンショットで比較しています。
スクリーンショットによる比較の対象を追加する場合は、下記の手順を行います。

1. [Altseed2-csharp](https://github.com/altseed/Altseed2-csharp)の CIからtest-resultをダウンロードする。

2. ダウンロードしたファイルの中から比較したいスクリーンショットを取得する。

3. [TestResult](https://github.com/altseed/Altseed2-csharp-test-result)に比較したいスクリーンショットを追加する。

4. Altseed2-csharpのリポジトリのTestResultを更新する。

環境ごとにスクリーンショットは細かい部分で異なるので、CIからダウンロードする必要があります。
比較する場合、実行するたびに表示が変わる内容は実装してはいけません。


