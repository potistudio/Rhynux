# Rhynux コードベース問題リスト

対象コミット: `218300b` (develop)
対象範囲: `Assets/_Rhynux_/Scripts/`, `Assets/_Rhynux_/Tests/`, プロジェクト設定
検証環境: Unity 6000.5.6f1 / .NET Standard 2.1 / activeInputHandler = Both

自動生成の `InputActionAssets/*.cs` (1289行) は対象外。実質 3200行を全読みした結果。

---

## レベル1: ゲームが実際に壊れている

### 1-1. コンボが絶対に増えない

`Assets/_Rhynux_/Scripts/Game/Logic/SessionManager.cs:43`

```csharp
private ReactiveProperty<int> m_CurrentCombo => new();  // ← `=>` になっている
```

`=` ではなく `=>` (式形式プロパティ) なので、**アクセスするたびに新しい ReactiveProperty が生成される**。
`IncreaseCombo()` は生成直後のインスタンスを 0→1 にして、そのまま破棄している。値が保持される場所が存在しない。

**修正**: `=>` を `=` に変える。1文字。

---

### 1-2. 購読が呼ぶたびに増える

`Assets/_Rhynux_/Scripts/Game/Logic/SessionManager.cs:36`, `:44`

```csharp
public ReadOnlyReactiveProperty<int> CurrentScore => m_CurrentScore.ToReadOnlyReactiveProperty();
```

`ToReadOnlyReactiveProperty()` を getter 内で呼んでいるため、プロパティを読むたびに新しい購読が生成され、誰も破棄しない。メモリリーク。

**修正**: フィールドに一度だけ生成して保持する。

---

### 1-3. Auto モードが一度も動かない

`Assets/_Rhynux_/Scripts/Game/Infrastructure/GameLifetimeScope.cs:17`

```csharp
builder.Register<AutoInputHandler>(Lifetime.Singleton);
```

`AutoInputHandler` は `ITickable` を実装しているが、VContainer は `RegisterEntryPoint` で登録されたものしか `Tick()` を呼ばない。オート再生が完全に動作しない。

**修正**: `builder.RegisterEntryPoint<AutoInputHandler>(Lifetime.Singleton);`

---

### 1-4. インデックスの意味が混ざっている

`Assets/_Rhynux_/Scripts/Game/Logic/InputReferee.cs:51-53`

```csharp
if (count == notesCountByLane - 1) {
    currentIndex = count;   // count は「レーン内の何個目か」
}                           // currentIndex は「配列全体の添字」
```

同一変数に **配列の添字** と **レーン内カウント** が混在して代入されている。
レーン内の最後のノーツ付近で、まったく別のノーツをヒット扱いする。

---

### 1-5. 落下判定のインデックスも別物

`Assets/_Rhynux_/Scripts/Game/Logic/RealtimeReferee.cs:34`

```csharp
return n.Select((x, i) => i).LastOrDefault();
```

`n` は `Where` でフィルタ済みのシーケンスなので、`i` は「条件を満たした中での順番」であり元配列の添字ではない。
譜面が時系列ソート済みかつ全ノーツが対象の場合にたまたま一致しているだけで、順序が乱れると壊れる。

---

### 1-6. Remap の式が間違っている (4ファイル)

```csharp
return (_x - _inMax) / (_inMax - _inMin) * (_outMax - _outMin) + _outMin;
//           ^^^^^^ _inMin であるべき
```

- `Game/View/AudioSpectrum/AudioSpectrum.cs:34`
- `Game/View/AudioSpectrum/AudioSpectrumDrawer.cs:11`
- `Game/View/AudioSpectrum/GoertzelSpectrumJob.cs:20`
- `Game/View/AudioSpectrum/GoertzelSpectrumMono.cs:26`

同じ誤りがコピペで4箇所に増殖している。正規化の起点がズレるため、周波数バンドのマッピングが全体的に誤っている。

---

### 1-7. バッファサイズが2の累乗ではない

`Assets/_Rhynux_/Scripts/Game/View/AudioSpectrum/AudioSpectrum.cs:30`

```csharp
private readonly float[] m_OutputAudioData = new float[8196];
```

`AudioSource.GetOutputData` は配列長が **2の累乗** であることを要求する。`8192` のタイプミス。
`GoertzelSpectrumJob.cs:100` にも `8196` がハードコードされている。

---

### 1-8. オフセットが BPM 倍されている

`Assets/_Rhynux_/Scripts/Game/Factories/NotesGenerators/ProceduralNotesGenerator.cs:12`

```csharp
new Note((x.Time + _chart.Offset) * (60f / _chart.BPM), x.Position)
```

`Offset` は秒単位のはずなのに、拍→秒の変換係数が掛かっている。
正しくは `x.Time * (60f / _chart.BPM) + _chart.Offset`。BPM が 120 以外の譜面で全体がズレる。

---

### 1-9. 触ると例外が飛ぶプロパティ

`Assets/_Rhynux_/Scripts/Game/Factories/NotesGenerators/ProceduralNotesGenerator.cs:7`

```csharp
public IObservable<IReadOnlyList<Note>> OnNotesGenerated => throw new NotImplementedException();
```

インターフェースを満たすためだけの地雷。

---

### 1-10. アニメーションが実行されず、バインド先も誤り

`Assets/_Rhynux_/Scripts/Game/UI/ComboDisplay.cs:22`

```csharp
LSequence.Create()
    .Append(LMotion.Create(Vector3.one * m_ScaleMultiplier, Vector3.one, m_Duration)
    .BindToLocalPosition(m_Label.transform));
```

- 戻り値を捨てていて `.Run()` を呼んでいない → 実行されない
- スケール値を渡しているのに `BindToLocalPosition` (`BindToLocalScale` であるべき) → 仮に動作してもラベルが画面外へ飛ぶ

---

### 1-11. クラス名とファイル名の不一致

`Assets/_Rhynux_/Scripts/SongSelection/UI/GlitchText.cs:3`

ファイル名は `GlitchText.cs`、クラス名は `RandomTitle`。
Unity は MonoBehaviour のファイル名一致を要求するため、このコンポーネントは GameObject にアタッチできない。

---

### 1-12. 自動生成した譜面で必ず NullReference

`Assets/_Rhynux_/Scripts/SongSelection/Presenter/ScrollItemRegistrar.cs:18`

```csharp
generatedCharts = i.Select (_ => new Chart(RandomBase64(), RandomBase64(), 120f, 0f, null, new Note[0]));
//                                                                              SoundTrack が null ↑
```

この譜面を選択すると `Game/Presenter/MusicPresenter.cs:12` の `Chart.Track.SoundClip` で NullReference。

---

## レベル2: パフォーマンス

### 2-1. 毎フレーム、譜面まるごとバイナリシリアライズしている (最重要)

`Assets/_Rhynux_/Scripts/Game/Core/SessionData.cs:11`

```csharp
public Note[] Notes { get { return m_NotesCollection.DeepCopy().ToArray(); }}
```

`DeepCopy` の実体は `Core/CopyHelper.cs:16` の **BinaryFormatter**。
つまり「MemoryStream に全ノーツをシリアライズ → 巻き戻して全部デシリアライズ」を、このプロパティに触るたびに実行している。

呼び出し箇所:

| ファイル | 頻度 |
|---|---|
| `Game/Logic/RealtimeReferee.cs:17` | 毎フレーム |
| `Game/Logic/RealtimeReferee.cs:29` | 毎フレーム (同フレーム内で2回目) |
| `Game/Logic/InputReferee.cs:24` | キー入力のたび |
| `Game/View/FloorTorquer.cs:20` | 毎フレーム |
| `Game/Input/AutoInputHandler.cs:35` | 毎フレーム |

1000ノーツの譜面なら毎フレーム数千オブジェクトを生成・破棄している。

加えて `BinaryFormatter` は .NET で非推奨 (削除予定) であり、IL2CPP ビルドでの動作保証もない。

**修正案**: 読み取り専用ビュー (`IReadOnlyList<Note>`) を一度だけ生成して使い回す。

---

### 2-2. 毎フレーム O(n²)

`Assets/_Rhynux_/Scripts/Game/Logic/RealtimeReferee.cs:20-23`

```csharp
var targets = notes.Where ((x, i) => i <= newIndex).Select ((x, i) => (x, i));
foreach ((Note x, int i) in targets) {
    FallNote (i);
}
```

**すでに落下済みのノーツも含めて、毎フレーム全件に落下イベントを再発行**している。
受け側の `Game/Logic/RefereeFacade.cs:20` は `List<int>.Contains()` による線形探索。合わせて毎フレーム O(n²)。

**修正案**: 進行位置を保持して差分のみ発行 + `m_CheckedNotes` を `HashSet<int>` に変更。

---

### 2-3. その他

| 場所 | 内容 |
|---|---|
| `Game/View/FPSCounter.cs:11` | `m_FPSHistory` が無限に増え続ける (メモリリーク)。さらに毎回 `Average()` で全走査 |
| `SongSelection/UI/CanvasDotGrid.cs:51-60` | 静的なノイズマップを毎フレーム全ドットに再適用。UI の color 変更は Canvas 全体の再ビルドを誘発するため、ドット数千個で致命的 |
| `Game/View/AudioSpectrum/AudioSpectrumDrawer.cs:35` | 毎フレーム `SetAllDirty()` |
| `Game/View/AudioSpectrum/AudioSpectrum.cs:64-68` | 毎フレーム NativeArray 2本 + `float[]` を確保。`Schedule()` 直後に `Complete()` しているため Job の並列化メリットがゼロ (メインスレッドを止めているだけ) |
| `SongSelection/UI/GlitchText.cs:17` | `FixedUpdate` で毎回 `new Regex()`。`static readonly` にすべき |
| `SongSelection/Presenter/ScrollItemRegistrar.cs:32` | 同上 |
| `Game/View/_FullLogic.cs:38-46` | 全ノーツの Transform を毎フレーム更新。カリングなし |
| `SongSelection/UI/CircleSelector.cs:16` | `OnPopulateMesh` の中で `anchoredPosition` を書き換えている。メッシュ構築中のレイアウト変更は無限ダーティを招きうる |

---

### 2-4. 平滑化が機能していない

`Assets/_Rhynux_/Scripts/Game/View/AudioSpectrum/GoertzelSpectrumJob.cs:110-120`

`dataArray` を新規確保 (全要素ゼロ) した直後に `ApplySmoothingTimeConstant` を適用している。
前フレームの値を持ち越していないため、平滑化ではなく単なる定数倍になっている。

---

## レベル3: 設計・デッドコード

### 3-1. 同じ責務が2セット存在する

`SessionManager` (Score / Combo / Notes / Time を全部保持) と、`ScoreManager` / `ComboManager` が並立している。
DI に登録され実際に稼働しているのは後者のみ。

`SessionManager` を注入しようとしている以下は、`GameLifetimeScope` に `SessionManager` が登録されていないため **解決に失敗する**:

- `Game/Presenter/GameStarter.cs:9`
- `Game/Presenter/NotesObjectObserver.cs:16`

---

### 3-2. DI に登録したが誰も使わないインスタンス

`Game/Infrastructure/GameLifetimeScope.cs:25-26` で `RealtimeReferee` / `InputReferee` を Singleton 登録しているが、
`Game/Logic/RefereeFacade.cs:16-17` が自前で `new` している。登録側のインスタンスは誰にも参照されない。

---

### 3-3. デッドコード

- `Game/Logic/ComboOperator.cs` — 全行コメントアウトされた空クラス
- `Game/Infrastructure/NotesGenerationScope.cs` — 中身が空の LifetimeScope
- `Game/Presenter/NotesRefereeComposer.cs` — どこにも登録されていない孤児クラス
- `aaa()` メソッド — `Game/View/_FullLogic.cs:49` と `Game/View/FloorTorquer.cs:38` に同一内容がコピペで2つ、どちらも未使用
- `Game/Proxy/SessionProxy.cs:2` — `m_SessionData` フィールド未使用 (自動プロパティを使用しているため)
- `SongSelection/Input/MenuInput.cs:4-6` — `m_SceneIdentifier` / `m_Navigator` が死んでいる (`m_SceneNavigator` と重複)
- `Game/View/_FullLogic.cs:13` の `m_Chart`、`:39` の `noteWidth`
- `Unity/RawChartAsset.cs:9` の `m_Clip`

---

### 3-4. 名前空間が存在しない

全クラスがグローバル名前空間にある。その結果:

- `SceneEntryPoint` が **3つ** (`Game.Infrastructure` / `Selection` / `Title`)
- `SceneNavigator` が **2つ** (`Game.Presenter` は plain class、`Selection.Presenter` は MonoBehaviour)

現状は asmdef が分離されているためコンパイルは通るが、asmdef 参照を1本追加した時点で `CS0433` (型があいまい) で破綻する。

**修正案**: `Rhynux.Game` / `Rhynux.Selection` などの名前空間を切る。

---

### 3-5. 購読の破棄漏れ

`AddTo` / `CompositeDisposable` なしで `Subscribe` している箇所:

- `Game/Presenter/ComboPresenter.cs:13`, `:22`
- `Game/Presenter/ScoreDisplayPresenter.cs:13`
- `Game/Presenter/ComboDisplayPresenter.cs:15`
- `Game/Presenter/LaneVisualizingPresenter.cs:14`, `:18`
- `Game/Presenter/ScorePresenter.cs:18`
- `Game/Presenter/HitEffectGenerator.cs:15`
- `Game/Input/InputListener.cs:13`
- `SongSelection/Presenter/AudioEmitter.cs:10`

シーンを往復すると購読が積み上がる。
`Game/Presenter/HitListener.cs` と `Game/Presenter/JudgementDisplay.cs` は正しく処理しているので、その形に揃える。

---

### 3-6. `async void` が5箇所

- `Game/Presenter/SceneNavigator.cs:4`
- `SongSelection/Presenter/SceneNavigator.cs:14`
- `SongSelection/Input/MenuInput.cs:50`
- `Title/SceneMover.cs:10`
- `Game/Input/AutoInputHandler.cs:25`

`async void` は内部で例外が発生しても捕捉できない。UniTask を導入済みなので `UniTaskVoid` + `.Forget()` に置き換えると安全。

---

### 3-7. 初期化順への暗黙依存

`Game/Input/InputListener.cs:13` と `Game/Presenter/LaneVisualizingPresenter.cs:14` が `InputHandlerFactory.HandlerPool` を参照しているが、
これは `Game/Infrastructure/SceneEntryPoint.cs:34` で `Create()` が呼ばれるまで null。

Navigathena のシーンライフサイクルと VContainer の `IStartable` の実行順に暗黙依存している。
ハンドラを直接注入する設計にすれば解消できる。

---

### 3-8. その他

| 場所 | 内容 |
|---|---|
| `Game/UI/AccuracyPopupEmitter.cs:46` | `m_Sequence` が一度も代入されない (default) まま `Complete()` を呼んでいる |
| `Game/UI/ScoreDisplay.cs:10-16` | 補間が `Update`、テキスト反映が `FixedUpdate`。逆 |
| `Unity/JsonChartAsset.cs:21` | `offset * 0.0001f`。ミリ秒なら `0.001f` のはずで10倍ズレている疑い |
| `Unity/JsonChartAsset.cs:26`, `:36`, `:38` | `maxBlock` / `notes` / `type` が private のため `JsonUtility` に無視され、値が読めていない |
| `Unity/ChartAsset.cs:4-6` | `m_Difficulty` / `m_ChartDifficultyLevel` / `m_Charter` が `Unpack()` に一切反映されない。`Chart` 側に難易度を保持する場所がない |
| `Core/Chart.cs:31` | `Secured` フラグをどこも読んでいない |
| `Unity/RawChartAsset.cs:15` | `new Chart(..., m_SoundTrack, ...) { Track = m_SoundTrack }` が冗長 |
| 各所 | タプルの `Item1` / `Item2` / `Item3` 直参照が多い (`Game/Presenter/HitEffectGenerator.cs:16`, `Game/Logic/RefereeFacade.cs:20` 等)。`(int index, int lane, AccuracyLevel accuracy)` と名前を付けているのに活用されていない |

---

## レベル4: テストが1本も通らない

### `Assets/_Rhynux_/Tests/Editor/RefereeTest.cs`

`SetUp` の中身が全行コメントアウトされている。
`m_ReactiveReferee` / `m_RealtimeReferee` が null のままテスト本体に入るため、3本とも NullReference で即死。

### `Assets/_Rhynux_/Tests/Editor/GameTest.cs`

- `Addressables.LoadAssetAsync<Chart>(...)` — `Chart` は plain C# クラスで `UnityEngine.Object` ではないため Addressables でロードできない
- `m_ReactiveReferee` が SetUp でコメントアウトされており null
- `m_SessionManager.UpdateTime()` を呼んでも `RealtimeReferee` と接続されていないため判定が発生しない
- `.Time + 161f` — `Note.Time` は秒単位なのに、ミリ秒のつもりの値を加算している (161秒後になる)

さらに譜面アセットは `.gitignore` の `Assets/_Rhynux_/Charts` で除外されているため、clone した環境ではそもそも実行できない。

---

## レベル5: リポジトリ・設定

- **`README.md` が古い** — Unity 2023.2.3f1 と記載されているが実際は `6000.5.6f1`。
  依存パッケージ一覧から UniTask / Navigathena / FancyScrollView / uPools / Alchemy / Entities が抜けている。
  逆に MagicTween は `Packages/manifest.json` に存在しないのに記載されている
- **`.gitignore` に `*.slnx` がない** — `*.sln` は除外しているが、新形式の `Rhynux.slnx` (自動生成物) がコミットされている
- **Input System が Both 設定** — `activeInputHandler: 2` のため `Game/Presenter/SceneNavigationInput.cs:12` の旧 `Input.GetKeyDown` は動作するが、
  新旧両システムが常駐してオーバーヘッドになる。`KeyboardActions` があるので新 Input System に統一するのが望ましい

---

## 推奨する着手順

| 順 | 内容 | 効果 | 手間 |
|---|---|---|---|
| 1 | `SessionManager` の `=>` を `=` に (1-1) | コンボが機能するようになる | 1文字 |
| 2 | `SessionData.Notes` の DeepCopy 撤廃 (2-1) | フレームレートが大幅改善 | 小 |
| 3 | `RealtimeReferee` を差分発行に + `HashSet` 化 (2-2) | 毎フレーム O(n²) の解消 | 小 |
| 4 | `AutoInputHandler` を `RegisterEntryPoint` に (1-3) | オート再生が動作する | 1行 |
| 5 | 名前空間の導入 (3-4) | `SceneEntryPoint` 3兄弟の衝突を予防 | 中 |

1〜4 は合わせて30分程度で対応可能。5 は早いうちにやるほど安く済む。
