# RocketBooster
デジタルゲーム開発2020 卒業制作

Unity Version: 2020.3.4f1 → 6000.5.1f1

Package パッケージ: Post Processing

開発補助ツール: Claude Code

> **関連ドキュメント**: [PROJECT_OVERVIEW.md](PROJECT_OVERVIEW.md)(概要）・ [IMPROVEMENTS.md](IMPROVEMENTS.md)(改善提案）・ [UNITY6_MIGRATION.md](UNITY6_MIGRATION.md)(Unity 6 移行手順）
> 本 README は上記のうち概要と改善提案を統合したものです。

---

## プロジェクト概要

デジタルゲーム開発2020卒業制作。Unity 製の **ロケットレースゲーム**。
プレイヤーはロケットを操縦し、燃料(ブースト)を管理しながらチェックポイントを通過してゴールを目指す。ソロプレイとマルチプレイ(対戦)の2モードを備える。

## 基本情報

| 項目 | 内容 |
| --- | --- |
| エンジン | Unity 2020.3.4f1 → 6000.5.1f1 |
| 言語 | C#(MonoBehaviour ベース) |
| 主要パッケージ | Post Processing |
| 対応言語 | 日本語 / 英語(ScriptableObject によるローカライズ) |
| プラットフォーム | PC(キーボード操作) |

## ゲームモード

### ソロプレイ(1P)
- 操作タイプ選択 → 難易度選択 → 名前入力 → 操作説明 → ステージ
- 難易度: **Easy / Normal / Hard**
- 制限時間内にゴールを目指し、タイム・順位をランキング保存

### マルチプレイ(対戦)
- 人数選択(2P / 3P / 4P) → チーム選択 → 難易度選択 → 名前入力 → 操作説明 → ステージ
- 各難易度 × 各人数のステージを用意(例: Easy 2P/3P/4P など)
- 画面分割で複数プレイヤーが同時にレース
- **アイテムシステム**で妨害・補助の駆け引きを行う

## アイテム種別(マルチプレイ)

`MultiplayerItems.ItemCategories` で定義(ランダムボックスから取得):

| カテゴリ | 効果 |
| --- | --- |
| AttackItem | 攻撃(ミサイル等で相手を妨害) |
| DefenseItem | 防御(シールド) |
| AbnormalConditionItem | 状態異常付与 |
| BoostCanItem | ブースト回復 |
| DropbackItem | 相手を後退させる |
| ReduceSpeedItem | 相手の速度を低下 |
| ReduceBoostItem | 相手のブーストを減少 |

## シーン構成

```
Title                         タイトル
├─ Options                    設定(BGM/SE音量・言語)
├─ Credits                    クレジット
├─ Stage_Solo/                ソロ
│   ├─ 1P_Choose_Control_Type / Choose_Difficulty / Enter_Player_Name / Instruction
│   └─ Easy / Normal / Hard   ステージ本編
└─ Stage_Multiplay/           マルチプレイ
    ├─ Num_of_Players / TeamSelection / Choose_Difficulty / Enter_Player_Name / Instruction
    └─ Easy / Normal / Hard × 2P / 3P / 4P
```

## スクリプト構成(`Assets/Scripts/`)

### Gameplay_Solo
ソロプレイのゲーム進行。`GameManagerSolo`(進行・UI・ランキング管理)、`MovementSolo`(ロケット操作・推力)、`PlayerStatusSolo`(ライフ・ブースト)、`CameraManagementSolo`、`CollisionHandlerSolo`、`CheckPointFlagSolo`、各種設定・一時保存スクリプト(難易度・名前・タイム)。

### Gameplay_Multiplayer
マルチプレイのゲーム進行。`GameManagerMultiplay`、`MovementMultiplay`、`PlayerStatusMultiplay`、`CameraManagementMultiplay`(分割画面)、アイテム系(`MultiplayerItems` / `PlayerItem` / `RandomItemBox` / `RandomItemSettings`)、攻撃・防御要素(`Missile` / `Shield` / `RockStone`)、チーム選択・人数選択・名前入力など。

### Gameplay_Elements
ステージ共通ギミック。`Waypoint` / `PathMovement` / `CircularMovement` / `Oscillator`(移動障害物)、`Fuel`(燃料アイテム)、`TrapTrigger`、`Robot`、`Tube`、`FlyFabrics`、`FollowPlayerCamera`、`PlayerCurrentPositionArrow`(進行度表示)など。

### Others
横断的機能。`Localization` / `Language`(多言語化)、`BGMController`(BGM/SE 音量・シーン跨ぎ永続化)、`Title` / `Option` / `Credits`、`BackgroundController`、`FadeOutSprite`。

## アセット構成

- `Assets/Scenes` — シーン
- `Assets/Scripts` — C# スクリプト(上記4カテゴリ)
- `Assets/Prefabs` — Solo / Multiplay / Map 別プレハブ
- `Assets/Settings/Localization` — 日英ローカライズ用 ScriptableObject(`*_EN.asset` / `*_JP.asset`)
- `Assets/Audio` — BGM / SE
- `Assets/Asset Packs` — 外部素材(ロケット、爆発エフェクト、ミサイル、ロボット、岩、SF コリドー、UI 素材、シェーダー等)

## 設計上のポイント

- **ローカライズ**: 各画面ごとに `Localization` ScriptableObject(ラベル名→テキストのリスト)を `_EN` / `_JP` で持ち、`Language.gameDisplayLanguage` に応じて切り替え。
- **設定の永続化**: 音量・言語は `PlayerPrefs` に保存。`BGMController` は `DontDestroyOnLoad` でシーンを跨いで存続。
- **GameManager**: ソロ/マルチとも各 GameManager がシングルトン(`Instance`)で進行・UI・チェックポイント・ランキングを統括。
- **一時保存**: 難易度・プレイヤー名・プレイタイムをシーン間で受け渡すための TempSave スクリプト群。

---

# 改善提案

> 完成済みプロジェクトに対する「次に手を入れるなら」という観点の提案です。致命的な不具合の指摘ではありません。詳細は [IMPROVEMENTS.md](IMPROVEMENTS.md) を参照。

## 優先度: 高(設計・保守性)

1. **巨大な God Class の分割** — `GameManagerMultiplay.cs`(約1300行)、`GameManagerSolo.cs`(約1100行)、`Instructions_1P.cs`(約750行)、`Option.cs`(約660行)が進行・UI・カメラ・アイテム・ランキング・ローカライズを抱え込んでいる。`UIManager` / `RankingService` などへ責務分離を推奨。
2. **Solo と Multiplay の重複コード** — `MovementSolo`↔`MovementMultiplay`、`GameManagerSolo`↔`GameManagerMultiplay` など並行クラスが二重管理。共通部分を基底クラスへ抽出。
3. **ローカライズ機構の堅牢化** — `Localization.GetLabelContent` が線形検索かつラベル未定義で例外。`Dictionary` 化とフォールバックを。

## 優先度: 中(パフォーマンス・安定性)

4. **`Update()` 内の `FindObjectOfType`** → シングルトン `Instance` を使用(✅ Step 1 で対応済み)。
5. **文字列連結の `GameObject.Find`**(`"FollowPlayer"+id+"Camera"`)を配列参照に。
6. **タグ比較を `CompareTag` に統一**(✅ 対応済み)。
7. **エラーハンドリング不在** — JSON パースや PlayerPrefs にフォールバックを。
8. **ランキング永続化の整理**(TextAsset + PlayerPrefs の二層構造)。

## 優先度: 低(コード品質)

9. 難易度 × 言語の switch/case 重複(`Option.cs`)をデータ駆動化。
10. グローバルな `static` 可変状態(TempSave 群)の初期化・`ScriptableObject` 化。
11. 命名タイプミス(`DisplayLanauge` / `Closet` / `Elasped` 等)の修正。
12. 残存 `Debug.Log` の除去(✅ 対応済み)。
13. コメントアウトされた死コードの削除。
14. 自動テストの導入(Unity Test Framework)。

## 補足(運用)

15. リポジトリ衛生 — `.unitypackage` 等の大容量バイナリ、Git LFS 設定、`.gitattributes` の見直し。

---

*本 README はコードベースの静的調査に基づく概要・提案です。実行時の挙動は別途検証してください。*
