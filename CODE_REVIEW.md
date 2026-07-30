# Rhynux コードベース問題リスト

対象範囲: `Assets/_Rhynux_/Scripts/`, `Assets/_Rhynux_/Tests/`, プロジェクト設定
検証環境: Unity 6000.5.6f1 / .NET Standard 2.1 / C# 9.0 / activeInputHandler = Both

自動生成の `InputActionAssets/*.cs` (1289行) は対象外。実質 3200行を全読みした結果。

## 対応状況

洗い出した項目はすべて対応済み（1件のみ意図的に見送り、理由は末尾）。
`reform` ブランチに 25 コミット。関心事ごとに分割してある。

| 検証 | 結果 |
| --- | --- |
| フルリビルド (`dotnet build -t:Rebuild`) | 0 errors / 0 warnings （着手前: 0 errors / 12 warnings） |
| テスト | 12 件 PASS / 0 FAIL （着手前: 実行不能） |

---

## レベル1: ゲームが実際に壊れている

### 1-1. コンボが絶対に増えない ✅ `64426d6`

`Game/Logic/SessionManager.cs:43`

```csharp
private ReactiveProperty<int> m_CurrentCombo => new();  // ← `=>` になっていた
```

`=` ではなく `=>` (式形式プロパティ) なので、**アクセスするたびに新しい ReactiveProperty が生成される**。
`IncreaseCombo()` は生成直後のインスタンスを 0→1 にして、そのまま破棄していた。値が保持される場所が存在しなかった。

回帰テスト: `GameTest.ComboAccumulates`

---

### 1-2. 購読が呼ぶたびに増える ✅ `64426d6`

`Game/Logic/SessionManager.cs:36`, `:44`

`ToReadOnlyReactiveProperty()` を getter 内で呼んでいたため、プロパティを読むたびに新しい購読が生成され、誰も破棄しなかった。
両ビューをコンストラクタで一度だけ生成するようにした。

回帰テスト: `GameTest.ComboViewIsStable`

---

### 1-3. Auto モードが一度も動かない ✅ `0c265cb`

原因は3つ重なっていた。

1. `AutoInputHandler` は `ITickable` を実装しているのに `Register()` で登録されており、コンテナが `Tick()` を呼ばなかった → `RegisterEntryPoint().AsSelf()` に変更
2. `SceneEntryPoint` が `GameSceneRequest.AutoMode` を**ログに出すだけ**で、`InputMode.Keyboard` をハードコードしていた → フラグでモードを選ぶようにした
3. `Tick()` が1フレームに1ノーツしか処理せず、同時押しや高密度地帯を取りこぼしていた → due なノーツを全て処理

ハンドラはファクトリが選択するまで待機するので、キーボードモード時に空撃ちしない。

> ⚠️ **挙動の変化**: 選曲画面は `AutoMode = true` を渡している (`SongSelection/Presenter/SceneNavigator.cs`)。
> これまではフラグが無視されキーボード操作になっていたが、**今後は実際にオート再生で始まる**。
> 手動プレイに戻すならこの `true` を `false` にする。

---

### 1-4. インデックスの意味が混ざっている ✅ `e32876b`

`Game/Logic/InputReferee.cs`

同一変数 `currentIndex` に **配列の添字** と **レーン内カウント** が混在して代入されており、
レーン内の最後のノーツ付近で無関係なノーツをヒット扱いしていた。

単一のインデックスだけを追う走査に整理し、レーンが空なら -1 を返す（距離が無限大になり `Judge()` が `Pass` を返す）。

回帰テスト: `RefereeTest.HitReportsSourceIndex`, `RefereeTest.EmptyLaneNeverHits`

---

### 1-5. 落下判定のインデックスも別物 ✅ `e32876b`

`Game/Logic/RealtimeReferee.cs`

`Where` でフィルタ済みのシーケンスから `i` を取っていたため、「条件を満たした中での順番」であり元配列の添字ではなかった。
カーソルを進める方式に変更し、各ノーツをちょうど1回だけ通知する。

回帰テスト: `RefereeTest.FallTest`, `RefereeTest.FallEmitsEachNoteOnce`

---

### 1-6. Remap の式が間違っている (4ファイル) ✅ `ad355dc`

```csharp
return (_x - _inMax) / (_inMax - _inMin) * ...  // ← _inMin であるべき
```

`AudioSpectrum.cs` / `AudioSpectrumDrawer.cs` / `GoertzelSpectrumJob.cs` / `GoertzelSpectrumMono.cs` の4箇所に
同じ4行がコピペされており、1つのタイプミスが全部に伝播していた。

---

### 1-7. バッファサイズが2の累乗ではない ✅ `513d354`

`AudioSource.GetOutputData` は配列長が2の累乗であることを要求するが `8196` になっていた。
窓オフセットにも同じリテラルが直書きされていた。5箇所すべてを `AudioSpectrum.SAMPLE_BUFFER_SIZE = 8192` に統一。

---

### 1-8. オフセットが BPM 倍されている ✅ `2e85c8a`

`ProceduralNotesGenerator` が `(time + offset) * 60 / BPM` を計算しており、オフセットまでテンポ変換にかけていた。
`Note.Time` は拍、オフセットは秒なので、拍だけがスケールする: `time * 60 / BPM + offset`。
120 BPM 以外の譜面が全体的にズレていた。

`JsonChartAsset` の係数も `0.0001f` → `0.001f`（エディタ形式はミリ秒）。

回帰テスト: `GameTest.GeneratorAppliesOffsetInSeconds`

---

### 1-9. 触ると例外が飛ぶプロパティ ✅ `d914cff`

`ProceduralNotesGenerator.OnNotesGenerated` が `throw new NotImplementedException()` で、
インターフェース経由で購読した瞬間にクラッシュしていた。`SingleLineConstantIntervalNotesGenerator` と同じ形で実装。

---

### 1-10. アニメーションが実行されず、バインド先も誤り ✅ `a9da8c8`

`ComboDisplay` は `LSequence.Create().Append(...)` の戻り値を捨てて `.Run()` を呼んでおらず、
さらにスケール値を `BindToLocalPosition` に渡していた（動いてもラベルが画面外へ飛ぶ）。

LitMotion のモーションは一度きりなので、再生のたびに作り直す形に変更。同じ欠陥が3箇所にあった:

- `ComboDisplay` — 上記
- `AccuracyPopupEmitter` — 一度も代入されないハンドルに `Complete()`、アニメ本体はコメントアウト
- `TrackInfoView` — 注入時に1回再生した後は `Complete()` のみで、曲を切り替えても再生されない（**レポート初版では未検出**）

---

### 1-11. クラス名とファイル名の不一致 ✅ `6e26033`

`GlitchText.cs` のクラス名が `RandomTitle`。Unity はファイル名一致を要求するため、このコンポーネントはアタッチできなかった。
どのシーンからも参照されていないことを確認した上でクラス名を変更。

---

### 1-12. 自動生成した譜面で必ず NullReference ✅ `85bdd53`

自動生成の譜面は `SoundTrack` が null で、選択すると `MusicPresenter` の `Chart.Track.SoundClip` で即例外。

生成側は空の `SoundTrack` を返すようにし、`MusicPresenter` はクリップ不在なら警告して再生をスキップ、
`SoundTrack.Duration` も null 参照せずゼロを返すようにした。

---

## レベル2: パフォーマンス

### 2-1. 毎フレーム、譜面まるごとバイナリシリアライズしている ✅ `56c7c9f`

`SessionData.Notes` と `Chart.Notes` が**アクセスのたびに BinaryFormatter の往復**を実行していた。
参照箇所は5つ、うち4つが毎フレーム（両レフェリー・FloorTorquer・AutoInputHandler）。
N ノーツの譜面で毎フレーム N オブジェクトを生成・破棄していた。

構築時に一度コピー済みのコレクションをそのまま公開する形に変更。呼び出し側は `IReadOnlyList<Note>` へ。

`BinaryFormatter` は .NET で非推奨かつ IL2CPP 非対応。唯一の利用者が消えたので `CopyHelper` ごと削除 (`1304cd9`)。

回帰テスト: `GameTest.SessionNotesDoNotReallocate`

---

### 2-2. 毎フレーム O(n²) ✅ `e32876b`, `2e73157`

`RealtimeReferee` が**落下済みのノーツも含めて毎フレーム全件に再通知**し、
受け側の `RefereeFacade` が `List<int>.Contains()` で線形探索していた。

カーソル方式で差分のみ発行するようにし、重複判定は `HashSet` に変更（`Contains` + `Add` を1回の O(1) 呼び出しに集約）。

---

### 2-3. その他 ✅ `4a64a29`, `bb3e733`

| 場所 | 内容 |
| --- | --- |
| `FPSCounter` | 履歴 List が無制限に増え、毎tick全走査で平均を再計算 → 累積和による O(1) の移動平均に |
| `CanvasDotGrid` | 静的なノイズマップを毎フレーム全ドットに再適用（UI の色書き込みは Canvas 全体を再ビルドさせる）→ マップ生成時のみ適用 |
| `GlitchText` / `ScrollItemRegistrar` | 呼び出しごとに `new Regex()`（片方は FixedUpdate から）→ GUID は常に24文字なのでスライスで代替 |
| `AudioSpectrumDrawer` | `SetAllDirty()` はレイアウトとマテリアルのパスも再実行する → 頂点のみ変わるので `SetVerticesDirty()` |
| `CircleSelector` | `OnPopulateMesh` 内で `anchoredPosition` を書き換え（再ダーティのループ要因）→ 有効化時と矩形変化時に移動 |
| `AudioSpectrum` | 毎フレーム NativeArray 2本 + managed 配列を確保 → 永続バッファを再利用 |

---

### 2-4. 平滑化が機能していない ✅ `311d875`

Job 内で毎回ゼロ初期化された `dataArray` に対して平滑化をかけていたため、
前フレームを引き継がず、時定数が単なるゲインに退化していた。

Job は毎フレーム作り直される struct で状態を持てないため、ブレンドを `AudioSpectrum` 側へ移動し、
平滑化済みスペクトルをフレーム間で保持するようにした。

ついでに Job 内で**ビンごとに周波数帯テーブルを再構築**していた（重み付けパスが O(n²)）のを1回に。

---

## レベル3: 設計・デッドコード

### 3-1. 同じ責務が2セット存在する ✅ `1304cd9`（部分対応）

`SessionManager` と `ScoreManager` / `ComboManager` が並立していた。
`SessionManager` を注入しようとしていた `GameStarter` と `NotesObjectObserver` は
`GameLifetimeScope` に `SessionManager` が登録されておらず、**注入が必ず失敗する**死んだコードだったため削除。

> 📌 **未決**: `SessionManager` 自体はバグ修正の上で残してある。現在 DI 未登録で、Score/Combo は
> `ScoreManager`/`ComboManager` が担当している。どちらへ一本化するかは設計判断なので触っていない。
> 統合するなら `SessionManager` 側が上位互換（Notes/Time も持つ）。

---

### 3-2. DI に登録したが誰も使わないインスタンス

`GameLifetimeScope` が `RealtimeReferee` / `InputReferee` を Singleton 登録している一方、
`RefereeFacade` は自前で `new` している。登録側は誰にも参照されない。

> 📌 **未対応**: 登録を消すか Facade を注入に変えるかで、レフェリーの所有権の設計が変わる。
> 実害はインスタンス2個分なので、3-1 の一本化と併せて判断するのが妥当。

---

### 3-3. デッドコード ✅ `1304cd9`

削除したファイル（いずれもシーン・prefab・アセットから未参照であることを確認済み）:

- `ComboOperator`, `NotesGenerationScope` — 中身が空
- `NotesRefereeComposer` — コンテナに未登録の孤児
- `GameStarter`, `NotesObjectObserver`, `NotesObjectGenerator` — `MusicPresenter` / `_FullLogic` と機能重複、かつ注入失敗確定
- `CopyHelper` — 最後の利用者が上記と共に消滅

削除したメンバー: `SessionProxy.m_SessionData` / `MenuInput` の重複ナビゲータとシーン識別子 /
`_FullLogic.m_Chart`・`noteWidth` / `RawChartAsset.m_Clip`（全アセットで `m_SoundTrack` に同一クリップが入っていることを確認）/
`_FullLogic` と `FloorTorquer` にコピペされていた `aaa()`

---

### 3-4. 名前空間が存在しない ✅ `5c8783e`

全型がグローバル名前空間にあり、`SceneEntryPoint` が3つ、`SceneNavigator` が2つ同名だった。
asmdef 参照を1本足した時点で `CS0433` になる状態。

```text
Scripts/Core, Scripts/Unity  -> Rhynux
Scripts/Game/**              -> Rhynux.Game
Scripts/SongSelection/**     -> Rhynux.SongSelection
Scripts/Title                -> Rhynux.Title
Tests/Editor                 -> Rhynux.Tests
```

`Rhynux.Selection` ではなく `Rhynux.SongSelection` にしてある。
自動生成の入力アクションがグローバルに `Selection` クラスを宣言しており、
`Rhynux.Selection` 名前空間はそれを覆い隠してしまうため。

C# 9 に file-scoped namespace がないのでインデントが全面的に変わっている。
シーン・prefab の参照は GUID ベースなので影響なし。asmdef の `rootNamespace` も揃えた。

---

### 3-5. 購読の破棄漏れ ✅ `58bb022`

8箇所の `Subscribe` が破棄されず、シーンを往復するたびに購読が積み上がっていた。
プレゼンター群は `CompositeDisposable` + `IDisposable`（コンテナがスコープと共に破棄）、
MonoBehaviour 2つは `AddTo(this)`。`HitListener` と `JudgementDisplay` の既存実装に揃えた。

---

### 3-6. `async void` が5箇所 ✅ `1e843a8`, `0c265cb`

`async void` 内の例外は同期コンテキストに渡されて消えるため、シーン遷移の失敗が「入力が効かない」ようにしか見えなかった。
全箇所を `UniTaskVoid` + `Forget()` に変更。
`SceneMover.NextScene` は UnityEvent がバインドできるよう `void` のまま残し、await 部分を private ヘルパーへ分離。

---

### 3-7. 初期化順への暗黙依存

`InputListener` と `LaneVisualizingPresenter` が `InputHandlerFactory.HandlerPool` を読むが、
これは `SceneEntryPoint.OnInitialize()` で `Create()` が呼ばれるまで null。

> 📌 **未対応**: Navigathena のライフサイクルと VContainer の `IStartable` の順序に依存したままになっている。
> ハンドラを直接注入する形にすれば解消するが、`InputHandlerFactory` の役割そのものを見直す変更になる。

---

### 3-8. その他 ✅

| 場所 | 内容 | コミット |
| --- | --- | --- |
| `AccuracyPopupEmitter` | 未代入の `m_Sequence` に `Complete()` | `a9da8c8` |
| `ScoreDisplay` | 補間が `Update`、テキスト反映が `FixedUpdate` で逆。さらに Lerp を丸めるため差が1未満で停止し、**最後の1点に永遠に到達しなかった** | `80256b5` |
| `JsonChartAsset` | offset の係数が 0.0001（ミリ秒なので 0.001） | `2e85c8a` |
| `JsonChartAsset` | `maxBlock` / `type` / ネストした `notes` が private のため JsonUtility に無視されていた。未使用だったので削除し、`[Serializable]` と CS0649 抑制を追加 | `ae236de` |
| `ChartAsset` | 難易度・難易度レベル・譜面制作者が `Unpack()` に反映されず、`Chart` に保持先もなかった → `Chart` に init-only プロパティを追加 | `bfe5079` |
| `HitEffectGenerator` ほか | `Item1`/`Item2`/`Item3` 直参照 → 宣言済みのタプル名へ | `58bb022` |
| `ScorePresenter` | 空譜面でゼロ除算 | `58bb022` |
| `RawChartAsset` | `{ Track = m_SoundTrack }` の冗長な初期化子 | `1304cd9` |

`Chart.Secured` はどこからも読まれていないが、用途が判断できないため残置。

---

## レベル4: テスト ✅ `34f6bf1`

どちらのスイートも実行不能だった。

- `RefereeTest` — `SetUp` が全行コメントアウトされ、3件とも null 参照で即死
- `GameTest` — `Addressables.LoadAssetAsync<Chart>` で読もうとしていたが `Chart` は `UnityEngine.Object` ではなくロード不可能。
  譜面アセット自体も `.gitignore` 対象でリポジトリに存在しない。さらに秒単位の `Note.Time` にミリ秒想定の値を加算していた

両方ともプロセス内で 60 BPM の譜面を組み立てる形に変更（1拍 = 1秒なのでノーツ時刻が変換を素通りする）。

このブランチで直した欠陥に対する回帰テストを追加:
コンボの累積 / ビューの同一性 / ヒットインデックスの妥当性 / 空レーン / 落下の一回性 /
オフセットの秒適用 / ノーツコレクションの非再確保。

これを可能にしたのが `SessionFactory` と2つのジェネレータからの `Debug.Log` 削除。
`UnityEngine.Debug` は ECall でエディタプロセス外では動かず、ロジック層を Unity ランタイムに縛り付けていた。

**12件 PASS / 0 FAIL**。

---

## レベル5: リポジトリ・設定 ✅

- **README** `06c3b2c` — Unity バージョンが 2023.2.3f1 のまま（実際は 6000.5.6f1）。
  依存一覧に manifest に存在しない MagicTween が載り、実在する9パッケージが抜けていた
- **`.gitignore`** `1ff5a4a` — `*.sln` は除外していたが `*.slnx` がなく、生成物の `Rhynux.slnx` が追跡されていた
- **Input System が Both 設定** — `activeInputHandler: 2` のため旧 `Input.GetKeyDown` も動作する。
  新旧両システムが常駐するオーバーヘッドはあるが、`ProjectSettings.asset` の変更はエディタ設定に影響するため見送り

---

## 作業中に判明した追加事項

### csproj が asmdef と同期していない

`Rhynux.Selection.Presenter.csproj` に `Rhynux.Selection.UI` への参照が欠落しているなど、
Unity 生成の csproj が asmdef の参照グラフから遅れていた。

**Unity 自体は asmdef を正としてコンパイルするため実プロジェクトへの影響はない**が、
`dotnet build` での検証が偽の CS0246 を出す。Unity エディタでプロジェクトを開き直せば再生成される。

### `_FullLogic` のノーツカリング（見送り）

全ノーツの Transform を毎フレーム更新しており、可視範囲外を省く余地がある。
ただし適切な閾値がカメラ距離とスクロール速度というシーン設定に依存し、コード内に根拠がない。
さらに可視状態は判定側の `DeactivateNote` と絡むため、別途設計が必要と判断して見送った。

### 検証手段について

Unity エディタが起動中でバッチモードが使えなかったため、
`dotnet build` と、`Temp/bin/Debug` の出力をリフレクションで走らせる簡易 NUnit ランナーで検証している。
Unity の Test Runner でも一度確認することを推奨する。
