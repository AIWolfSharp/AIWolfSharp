# AIWolfSharp
## C# version of AIWolf Server&amp;Library

AIWolf#とは，元々Javaで書かれていた人狼知能サーバ＆ライブラリをC#に移植したものです．
最新のリリースはVersion 0.2.5+です．

1. 配布ファイルについて
  
  ファイルAIWolfSharp.zip内には以下のファイルがあります．
  
  | ファイル名 | 説明 |
  |:---|:---|
  |C#版人狼知能サーバ＆ライブラリについて.pdf|このREADMEと同じ内容|
  |AgentTester.exe|人狼知能大会の事前テスト用クラスを移植したもの|
  |ClientStarter.exe|プレイヤーをTCP/IP経由でサーバに接続する|
  |DirectStarter.exe|サーバとプレイヤーを直接接続してゲームを実行する|
  |RoleRequestStarter.exe|RoleRequestStarterクラスを移植したもの|
  |ServerStarter.exe|TCP/IP経由での接続を受け付けるサーバを立ち上げる|
  |AIWolfLibClient.dll|プレイヤーに必要なライブラリ|
  |AIWolfLibCommon.dll|サーバ，プレイヤー共通のライブラリ|
  |AIWolfLibServer.dll|サーバに必要なライブラリ|
  |TestPlayer.dll|実行テストのため作成したほとんど何もしないプレイヤー|
  
1. 入手が必要なライブラリについて
  
  配布するライブラリの他に，Json.NET および NLog が必要です．
  
1. 起動方法
  
  1. DirectStarter.exeを使用する場合（直接接続）
      
    DirectStarter.exeのコマンドラインオプションは，
    
      `-c プレイヤークラス名 プレイヤーDLLファイル名 当該クラスを使ったプレイヤー数
    -l ログディレクトリ（デフォルトは./log/）`
    
    で，-c オプションは複数回使用可能です．
    ただし，現在ログ機能は殺してありますので –l オプションは無効となっています．
    例えば，
    
    `DirectStarter.exe -c AIWolf.Client.Base.Smpl.SampleRoleAssignPlayer AIWolfLibClient.dll 11
    -c AIWolf.TestPlayer.TestRoleAssignPlayer TestPlayer.dll 1`
    
    で，
    AIWolfLibClient.dll 内のAIWolf.Client.Base.Smpl.SampleRoleAssignPlayerを11体，
    TestPlayer.dll内のAIWolf.TestPlayer.TestRoleAssignPlayerを一体使ってゲームを開始します．
    
  1. RoleRequestStarter.exeを使用する場合（直接接続）
    RoleRequestStarter.exeのコマンドラインオプションは，

    `-n ゲームに参加するプレイヤー数
    -a プレイヤーDLLファイル名
    -c プレイヤークラス名 設定したい役職（省略可）
    -d デフォルトのプレイヤークラス名
    -l ログディレクトリ（デフォルトは./log/）`
    
    で，-c オプションは複数回使用可能です．
    また，デフォルトプレイヤーを指定しなかった場合，
    AIWolf.Client.Base.Smpl.SampleRoleAssignPlayerが使われます．
    例えば，
    
    `RoleRequestStarter.exe –n 15 –a TestPlayer.dll -c AIWolf.TestPlayer.TestRoleAssignPlayer seer
    -c AIWolf.TestPlayer.TestRoleAssignPlayer werewolf`
    
    で，TestPlayer.dll内のAIWolf.TestPlayer.TestRoleAssignPlayerを使ったSEERとWEREWOLF一体ずつと
    AIWolf.Client.Base.Smpl.SampleRoleAssignPlayer13体からなる15人の村でゲームを開始します．
    
  1. ServerStarter.exeによるサーバの起動
    
    ServerStarter.exeのコマンドラインオプションは，
    
    `-n ゲームに参加するプレイヤー数（デフォルトは12）
    -p ポート番号（デフォルトは10000）`
    
    です．例えば，
    
    `ServerStarter.exe –p 12345 –n 15`
    
    で，
    `Start AIWolf Server port:12345 playerNum:15
    Waiting for connection...`
    
    と出力して，12345番ポートを使って15人でプレイするサーバーが起動し，
    プレイヤーからの接続を待ちます．Java版のプレイヤーからの接続も大丈夫（なはず）です．
    
  1. ClientStarter.exeを使用したプレイヤーの起動
    
    ClientStarter.exeのコマンドラインオプションは，
    
    `-h サーバの走っているホスト名
    -p 接続ポート番号
    -c プレイヤークラス名 プレイヤーDLLファイル名 設定したい役職（省略可）`
    
    です．Java版のサーバへの接続も大丈夫（なはず）です．
    
    例：`ClientStarter.exe –h localhost –p 12345 –c AIWolf.TestPlayer.TestRoleAssignPlayer
    TestPlayer.dll villager`
    
  1. AgentTester.exeについて
    
    Java版と異なり，テストするプレイヤーをコマンドラインオプションで指定します．
    
    例：`AgentTester.exe TestPlayer.dll AIWolf.TestPlayer.TestRoleAssignPlayer`
    
1. C#版プレイヤーのプログラム実装について
  
  AIWolf.Common.Data.IPlayer インターフェースを継承したプログラムが
  プレイヤーとしてゲームに参加することが出来ます．
  
  1. IPlayer インターフェースの実装すべきプロパティとメソッド
    
    IPlayer インターフェースを継承したクラスはプロパティ Name と 10 個のメソッドを実装する必要があります． 
    これらのメソッドは以下の 3 種類に分類されます．
    
    - 情報処理メソッド： Initialize, Update, DayStart, Finish
    
    - 対象指定メソッド： Vote, Attack, Guard, Divine
    
    - 会話メソッド： Talk, Whisper
    
    ***
    
    1. プロパティ Name
      
      プレイヤーの名前（String 型）です．ゲーム実行時のログに名前が反映されます．
      
    1. 情報処理メソッド（Initialize, Update, DayStart, Finish）
      これらは情報を処理するためのメソッドであり，何も戻り値を返す必要がありません．
      
      - Initialize(GameInfo, GameSetting)：ゲーム開始時に一度だけ呼ばれます．
        引数として現在のゲーム状態を表す GameInfo とゲームの設定（各役職の人数等）を表す GameSetting が与えられます．
        
      - Update(GameInfo)：Initialize 以外の全てのメソッドの前に呼ばれます．
        引数としてゲーム内の最新の情報を含んだ GameInfo が与えられます．
        Finish()の前に呼ばれる時のみ，全プレイヤーの役職情報を含んだ GameInfo が与えられます．
        
      - DayStart()：毎日の始めに一度だけ呼ばれます．
        
      - Finish()：ゲーム終了時に呼ばれます．
      
    1. 対象指定メソッド（Vote, Attack, Guard, Divine）
      
      対象となる Agent を戻り値として返すメソッドです．
      Attack, Guard, Divine は特定の役職のプレイヤーの場合のみ呼ばれるメソッドです．
      
      - Vote()：その日に投票する対象プレイヤーを返します．
      
      - Attack()：人狼のプレイヤーのみ呼ばれるメソッドです．
        その日に襲撃投票する対象プレイヤーを返します．
      
      - Guard()：狩人のプレイヤーのみ呼ばれるメソッドです．
        その日に護衛する対象プレイヤーを返します．
      
      - Divine()：占い師のプレイヤーのみ呼ばれるメソッドです．
        その日に占う対象プレイヤーを返します．
      
    1. 会話メソッド（Talk, Whisper）
      
      発話する内容（String 型）を返すメソッドです．Whisper は人狼のプレイヤーの場合のみ呼ばれます．
      - Talk()；村全体に対して発話する内容を返します．
        
      - Whisper()：人狼のプレイヤーのみ呼ばれるメソッドです．人狼だけに対して発話する内容を返します． 
        この発話内容は人狼以外のプレイヤーに公開されることはありません．
        
  1. 発話可能な内容
    
    人狼知能大会では，TemplateTalkFactory, TemplateWhisperFactory で生成可能な発話のみで会話を行います．
    生成可能な発話は以下の 11 種類です．
    
    - Estimate：プレイヤーA の役職は○○だと思う．
      
    - Comingout：私の役職は○○だ．
    
    - Divined：プレイヤーA を占った結果，○○（人間 or 人狼）だった．
    
    - Inquested：プレイヤーA は霊能の結果，○○（人間 or 人狼）だった．
    
    - Guarded：プレイヤーA を護衛した．
    
    - Vote：プレイヤーA に投票する．
    
    - Attack：プレイヤーA に襲撃投票する．（TemplateWhisperFactory のみ）
    
    - Agree：発話 T に同意する．
    
    - Disagree：発話 T に反対する．
    
    - Over：もう話すことは無い．（全プレイヤーが OVER なら会話フェーズ終了）
    
    - Skip：様子見（他のプレイヤーが全員 OVER でも会話フェーズが終了しない）
    
  1. GameInfoについて
    
    GameInfoクラスには，ゲームの状態を表す以下のプロパティがあります．
    タイプの詳細は，ソースをご覧ください．
    
    | プロパティ名 | タイプ | 説明 |
    |:---|:---|:---|
    |Day|int|何日目か|
    |Agent|Agent|プレイヤー自身|
    |Role|Role?|プレイヤー自身の役職|
    |MediumResult|Judge|霊能結果（霊能者のみ）|
    |DivineResult|Judge|占い結果（占い師のみ）|
    |ExecutedAgent|Agent|昨夜処刑されたプレイヤー|
    |AttackedAgent|Agent|昨夜襲撃されたプレイヤー|
    |GuardedAgent|Agent|昨夜護衛されたプレイヤー|
    |VoteList|List&lt;Vote&gt;|投票状況のリスト|
    |AttackVoteList|List&lt;Vote&gt;|襲撃先投票状況のリスト（人狼のみ）|
    |TalkList|List&lt;Talk&gt;|今日の会話のリスト|
    |WhisperList|List&lt;Talk&gt;|今日の囁きのリスト（人狼のみ）|
    |StatusMap|Dictionary&lt;Agent, Status&gt;|各プレイヤーの生死|
    |RoleMap|Dictionary&lt;Agent, Role&gt;|各プレイヤーの役職（既知の分）|
    |AgentList|List&lt;Agent&gt;|プレイヤーのリスト|
    |AliveAgentList|List&lt;Agent&gt;|生きているプレイヤーのリスト|
  
  1. GameSettingについて
    
    GameSettingクラスには，ゲームの設定を表す以下のプロパティがあります．
    
    | プロパティ名 | タイプ | 説明 |
    |:---|:---|:---|
    |RoleNumMap|Dictionary&lt;Role, int&gt;|各役職の人数|
    |MaxTalk|int|発話の最大回数|
    |EnableNoAttack|bool|誰も襲撃しないことが可能か|
    |VoteVisible|bool|誰が誰に投票したかがわかるか|
    |RandomSeed|long|乱数の種|
    |PlayerNum|int|プレイヤー数|
