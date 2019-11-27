﻿## 必須

- .netframework 4.8 (Windows only)

[Download](https://dotnet.microsoft.com/download/visual-studio-sdks)

- .NET SDK 3.0.100

[Download](https://dotnet.microsoft.com/download/dotnet-core/3.0)

- その他: [Coreのdocument](Core/documents/development/HowToBuild_Ja.md)を参照

## Build

### Windows

いまのところ。Core周りのビルドとかも自動化したいよね。

1. `git submodule update --init --recursive` を実行
1. `Scripts/generate_bindings.py` を実行
1. `Core/scripts/GenerateProjects(_x64_).bat もしくは .sh`を実行
1. `Core/build/Altseed.sln` を開き Core をビルド
1. `Alseed2.sln` を開き Engine をビルド


### Mac?

Debug
```shell
dotnet build
```
Release
```shell
dotnet build -c Release
```
detail: 
[dotnet build - Microsoft Docs](https://docs.microsoft.com/ja-jp/dotnet/core/tools/dotnet-build)