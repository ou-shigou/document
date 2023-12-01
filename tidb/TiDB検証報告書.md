TiDB検証検証報告書
===========================
# 目次
- [TiDB検証目的](#TiDB検証目的)
- [TiDB検証概要](#TiDB検証概要)
- [TiDB検証環境準備](#TiDB検証環境準備)
- [TiDB検証テストデータ準備](#TiDB検証テストデータ準備)
- [TiDB運用とセッキュリティ検証](#TiDB運用とセッキュリティ検証)
- [TiDB性能検証](#TiDB性能検証)
- [DX環境でのTiDBインストール](#DX環境でのTiDBインストール)
## TiDB検証目的
Azureクラウドでのインストール、大量データ状態での運用手法、セッキュリテイ性、又利用性能などの観点からTiDBという製品がイオンのプロジェクトに適用かどうかを検証するのは今回の目的である。
## TiDB検証概要 
TiDB検証の詳細は下表のように纏めました。 
**結論としては、TiDBはAzureクラウドで運用しやすく、大量データを扱う場合でも性能に満足できる製品になります。**
| NO | 検証内容 | 評価結果 | 備考 |
| ---------- | -----------|-----------|-----------|
| 1| TiDBのAzureクラウドインストール検証 | ◎ | Azureクラウドにインストールしたが、DX環境のインストールはまだ検証していない |
| 2| TiDBセッキュリティ運用検証 | ◎ |  |
| 2.1| Kubunetesバージョンアップ検証 | ◎ | DX基盤ではBlue/Green運用と称する |
| 2.2| データベースバックアップ検証 | ◎ |  |
| 2.3| TiDBバージョンアップ | ◎ |  |
| 2.4| データ保存テスク拡張検証 | ◎ |  |
| 2.5| 安全なDBアクセス方式の確立 | 〇 | サービスプリンシパル方式なし |
| 2.5.1| TiDB 安全な接続| ◎ | TiDB クライアント側の暗号化 |
| 2.5.2| Azure AD authentication | △ | Azure AD authentication サポート対象外 |
| 2.6| 多DBインスタンスの検証 | ◎ | インスタンス毎に細かくリリース振り分けできることを検証する |
| 3| TiDB性能検証 | 〇 | |
| 3.1| 大量データ参照 | ◎ | 三億行ほどのテーブルの参照 |
| 3.2| OrderBy | 〇 | OrderBy対応に懸念が残ったため、一重まるを付けることに(上順と下順の組合せ) |

## TiDB検証環境準備

DX環境に検証する前にAzureのインターネット環境にTiDBをインストールし、性能とセッキュリテイ関連の検証を行った。インストール詳細は以下になります。

* Kubernetes　version： 1.25.11
※Blue/Greenプロセスを検証するためにインストール時に低いバージョンを選んだ
以下６Poolをインストール
* Tidb 2Pod
* Tikv 3Pod 各Pod１Ti　SSDテスクを付ける
* Pd    3Pod 
* Ticdc 3Pod
* Tiflash 3Pod
* Admin 1Pod


## TiDB検証テストデータ準備

### テストアプリ
CCFlowのワークフローシステム（SSFlow）のデータベースがSQL　ServerからTiDBに切り替え、テストアプリとして利用する
CCFlowエンジンなどを解析し、データベーステーブルにワークフローデータをインサートしてワークフローの承認関連動作や一覧などを正常に行えることを確認し、大容量テストデータの作成に入る。

### テストデータ
大容量テストデータ作成用のPythonプログラムを開発し、TiDBのAKS内テストデータ作成用POD（8Core,メモリ16Gi）、PODにテストデータ保存用ディスク１Tを付けることに、TiDB Lightningツールを使って1.5億件フロー（3業務フローにそれぞれ5000万件）データを導入した。
以上の1.5億件フローデータの容量は500Gi超になり、TiDBのセッキュリテイ性能を検証するためTiDB全体のボリュームが１Tiの状態でテストしたいと考えて、TiDBに新しいインスタンスを追加し、上記同じ500Giテストデータが新インスタンスにも導入した。

## TiDB運用とセッキュリティ検証
以下の順でKubunetesバージョンアップ、データベースバックアップ、 TiDBバージョンアップ、データ保存テスク拡張、安全なDBアクセス方式の確立、多DBインスタンスの検証を行いました。

### Kubunetesバージョンアップ
KubunetesバージョンアップはよくBlue/Green運用の形で対応していますが、TiDBでは順次にSlave NodeのKubenetesのバージョンアップ完了し、最後Master NodeのKubenetesのバージョンアップを行う形で作業を進めます。下記のダイナミック図のようにPOD別のKubenetesバージョンアップのプロセスより対応することとしています。
![k8sversionup.gif](img/k8sversionup.gif)  

検証詳細プロセスは以下のようになります。

* Node、Podの現状を確認します。

```
kubectl get nodes
```
![k8sver1.png](img/k8sver1.png)  
```
kubectl get tc -n tidb-cluster
```
![k8sver2.png](img/k8sver2.png)  
```
kubectl get pod -n tidb-cluster -o wide
```
![k8sver3.png](img/k8sver3.png)  

* control plane、PoolのK8Sバージョンを確認します。

```
az aks show --resource-group saas-core --name aeontidb --output table
```
![k8sver4.png](img/k8sver4.png)  
```
az aks nodepool list --resource-group saas-core --cluster-name aeontidb --query "[].{Name:name,k8version:orchestratorVersion}" --output table
```
![k8sver5.png](img/k8sver5.png)  

* control planeのK8Sバージョンアップを行います。
※バージョン1.25.11は1.26.10など限定バージョンにしかバージョンアップできないようになっていて、ほかのバージョン番号を指定されたら、エラーが発生します。
```
az aks upgrade --resource-group saas-core --name aeontidb --control-plane-only --no-wait --kubernetes-version 1.26.10
```
![k8sver6.png](img/k8sver6.png)  

```
az aks show --resource-group saas-core --name aeontidb --output table
```
![k8sver7.png](img/k8sver7.png)  

* 新しいtidb Poolを新規作成します。
```
az aks nodepool add --name newtidb --cluster-name aeontidb --resource-group saas-core --node-vm-size Standard_E8s_v4 --zones 1 2 3 --node-count 2 --labels dedicated=tidb --node-taints dedicated=tidb:NoSchedule
```
![k8sver9.png](img/k8sver9.png)  
![k8sver10.png](img/k8sver10.png)  
* PODを確認します。
```
kubectl get pod -n tidb-cluster -o wide
```
![k8sver11.png](img/k8sver11.png) 

* 古いtidb Poolの紐づけを外します。
```
az aks nodepool update --resource-group saas-core --cluster-name aeontidb --name "tidb" --labels="" --node-taints=""
```
![k8sver12.png](img/k8sver12.png) 

* tidbのPodを削除します（新Tidb Poolに紐づける新しいPODを自動起動する）。
```
kubectl delete pod basic-tidb-0 -n tidb-cluster
kubectl delete pod basic-tidb-1 -n tidb-cluster
```
![k8sver13.png](img/k8sver13.png) 
![k8sver14.png](img/k8sver14.png)
* 古いtidb Poolを削除します。
```
az aks nodepool delete --name tidb --cluster-name aeontidb --resource-group saas-core
```
![k8sver15.png](img/k8sver15.png) 
![k8sver16.png](img/k8sver16.png)

* Pd,tikvも同じように新規Pool,旧pool関連外し、POD削除などを行います、コマンドは以下になります。
```
az aks nodepool add --name newpd --cluster-name aeontidb --resource-group saas-core --node-vm-size Standard_F4s_v2 --zones 1 2 3 --node-count 3 --labels dedicated=pd --node-taints dedicated=pd:NoSchedule

az aks nodepool add --name newtikv --cluster-name aeontidb --resource-group saas-core --node-vm-size Standard_E8s_v4 --zones 1 2 3 --node-count 3 --labels dedicated=tikv --node-taints dedicated=tikv:NoSchedule

 az aks nodepool update --resource-group saas-core --cluster-name aeontidb --name "pd" --labels="" --node-taints=""
 az aks nodepool update --resource-group saas-core --cluster-name aeontidb --name "tikv" --labels="" --node-taints="“
kubectl delete pod basic-pd-0 -n tidb-cluster
kubectl delete pod basic-pd-1 -n tidb-cluster
kubectl delete pod basic-pd-2 -n tidb-cluster
kubectl delete pod basic-tikv-0 -n tidb-cluster
kubectl delete pod basic-tikv-1 -n tidb-cluster
kubectl delete pod basic-tikv-2 -n tidb-cluster
az aks nodepool delete --name pd --cluster-name aeontidb --resource-group saas-core
az aks nodepool delete --name tikv --cluster-name aeontidb --resource-group saas-core
```
* 新ticdc Poolを作成し、旧ticdc Poolを削除します。
```
az aks nodepool add --name newticdc --cluster-name aeontidb --resource-group saas-core --node-vm-size Standard_E16s_v4 --zones 1 2 3  --node-count 3 --labels dedicated=ticdc --node-taints dedicated=ticdc:NoSchedule
az aks nodepool delete --name ticdc --cluster-name aeontidb --resource-group saas-core
az aks nodepool list --cluster-name aeontidb --resource-group saas-core --output table
```
![k8sver17.png](img/k8sver17.png) 

* Master PoolのKubenetesバージョンアップを行います。
```
az aks nodepool upgrade --resource-group saas-core --cluster-name aeontidb --name admin --no-wait --kubernetes-version 1.26.10
```
![k8sver19.png](img/k8sver19.png) 

* テストアプリ画面でフローの承認などの作業をし、正常に動作できることが確認できました！
![k8sver18.png](img/k8sver18.png) 

## TiDB性能検証 
### 性能検証概要
TiDB検証テストデータ準備(#TiDB検証テストデータ準備)の節にも紹介したように三つのワークフローにそれぞれ5000万件のフローテストデータを作成しました。以下の各テーブルのデータボリューム詳細情報です。
| NO | テーブル | データ件数 | 備考 |
| ---------- | -----------|-----------|-----------|
| 1| tt_wf_merchandise_plan | 5000万 | 仕入計画申請フローのトランザクションテーブル |
| 2| nd1track | 5000万 | 仕入計画申請フローのトラックテーブル |
| 3| tt_wf_arrival_returns | 5000万 | 入荷返品申請フローのトランザクションテーブル |
| 4| nd5track | 5000万 | 入荷返品申請フローのトラックテーブル |
| 5| tt_wf_stock_adjustment | 5000万 | 在庫調整申請フローのトランザクションテーブル |
| 6| nd7track | 5000万 | 在庫調整申請フローのトラックテーブル |
| 7| wf_generworkflow | 15000万 | ワークフローの申請状態情報管理テーブル |
| 8| wf_generworkerlist | 30000万 | ワークフロー状態変更履歴管理テーブル |
| 9| tt_wf_order_number | 15000万 | 業務申請番号採番管理テーブル |

以下は単テーブルCount性能検証、複数テーブルCount性能検証、多テーブル一覧検索性能検証、フロー新規（Insert）性能検証、フロー承認（Update）性能検証の順で性能検証を行う

### 単テーブルCount性能検証
wf_generworkerlistのCount検証
```
select count(*) from wf_generworkerlist;
```
![test3.png](img/test3.png) 
tt_wf_merchandise_planのプライベートキー（Oid）を対象にCount検証
```
select count(oid) from tt_wf_merchandise_plan;
```
![test4.png](img/test4.png) 
### 複数テーブルCount性能検証
テーブルwf_generworkflowとwf_generworkerlist以下のように結合後のCount検証
```
SELECT count(*) FROM `wf_generworkflow` AS `a` JOIN (select * from `wf_generworkerlist` where FK_Emp='0220320' ) AS `b` ON (`b`.`IsEnable`=1) AND (`b`.`IsPass`=0) AND (`a`.`WorkID`=`b`.`WorkID`) AND (`a`.`FK_Node`=`b`.`FK_Node`) AND (`a`.`WFState`!=0) AND (`b`.`WhoExeIt`!=1)  AND a.TaskSta=0 AND  a.WFState!=10;
```
```
SELECT count(*) FROM `wf_generworkflow` AS `a` JOIN (select * from `wf_generworkerlist` where FK_Emp='0210069' )
AS `b` ON (`b`.`IsEnable`=1) AND (`b`.`IsPass`=0) AND (`a`.`WorkID`=`b`.`WorkID`) AND (`a`.`FK_Node`=`b`.`FK_Node`) AND
(`a`.`WFState`!=0) AND (`b`.`WhoExeIt`!=1)  AND a.TaskSta=0 AND  a.WFState!=10;
```
![test5.png](img/test5.png) 

### 多テーブル一覧検索性能検証
* 上記複数テーブルCount性能検証と同じ結合条件で10000目レコードから5件レコードを抽出した場合の性能検証(Order by にWordIDだけにする)
```
SELECT `b`.`FK_Emp` AS `FK_Emp`,`a`.`PRI` AS `PRI`,`a`.`WorkID` AS `WorkID`,`b`.`IsRead` AS `IsRead`,`a`.`Starter` AS `Starter`,`a`.`StarterName` AS `StarterName`,`a`.`WFState` AS `WFState`,`a`.`FK_Dept` AS `FK_Dept`,`a`.`DeptName` AS `DeptName`,`a`.`FK_Flow` AS `FK_Flow`,`a`.`FlowName` AS `FlowName`,`a`.`PWorkID` AS `PWorkID`,`a`.`PFlowNo` AS `PFlowNo`,`b`.`FK_Node` AS `FK_Node`,`b`.`FK_NodeText` AS `NodeName`,`b`.`FK_Dept` AS `WorkerDept`,`a`.`Title` AS `Title`,`a`.`RDT` AS `RDT`,`b`.`RDT` AS `ADT`,`b`.`SDT` AS `SDT`,`b`.`FID` AS `FID`,`a`.`FK_FlowSort` AS `FK_FlowSort`,`a`.`SysType` AS `SysType`,`a`.`SDTOfNode` AS `SDTOfNode`,`b`.`PressTimes` AS `PressTimes`,`a`.`GuestNo` AS `GuestNo`,`a`.`GuestName` AS `GuestName`,`a`.`BillNo` AS `BillNo`,`a`.`FlowNote` AS `FlowNote`,`a`.`TodoEmps` AS `TodoEmps`,`a`.`TodoEmpsNum` AS `TodoEmpsNum`,`a`.`TodoSta` AS `TodoSta`,`a`.`TaskSta` AS `TaskSta`,0 AS `ListType`,`a`.`Sender` AS `Sender`,`a`.`AtPara` AS `AtPara`,1 AS `MyNum` FROM `wf_generworkflow` AS `a` JOIN wf_generworkerlist AS `b` ON (`b`.`IsEnable`=1) AND (`b`.`IsPass`=0) AND (`a`.`WorkID`=`b`.`WorkID`) AND (`a`.`FK_Node`=`b`.`FK_Node`) AND (`a`.`WFState`!=0) AND (`b`.`WhoExeIt`!=1)  AND a.TaskSta=0 AND  a.WFState!=10 Order by WorkID desc LIMIT 5 OFFSET 10000
```
![test6.png](img/test6.png) 

* Order by にa.WordID desc ,b.FK_Emp ascにする場合
```
SELECT `b`.`FK_Emp` AS `FK_Emp`,`a`.`PRI` AS `PRI`,`a`.`WorkID` AS `WorkID`,`b`.`IsRead` AS `IsRead`,`a`.`Starter` AS `Starter`,`a`.`StarterName` AS `StarterName`,`a`.`WFState` AS `WFState`,`a`.`FK_Dept` AS `FK_Dept`,`a`.`DeptName` AS `DeptName`,`a`.`FK_Flow` AS `FK_Flow`,`a`.`FlowName` AS `FlowName`,`a`.`PWorkID` AS `PWorkID`,`a`.`PFlowNo` AS `PFlowNo`,`b`.`FK_Node` AS `FK_Node`,`b`.`FK_NodeText` AS `NodeName`,`b`.`FK_Dept` AS `WorkerDept`,`a`.`Title` AS `Title`,`a`.`RDT` AS `RDT`,`b`.`RDT` AS `ADT`,`b`.`SDT` AS `SDT`,`b`.`FID` AS `FID`,`a`.`FK_FlowSort` AS `FK_FlowSort`,`a`.`SysType` AS `SysType`,`a`.`SDTOfNode` AS `SDTOfNode`,`b`.`PressTimes` AS `PressTimes`,`a`.`GuestNo` AS `GuestNo`,`a`.`GuestName` AS `GuestName`,`a`.`BillNo` AS `BillNo`,`a`.`FlowNote` AS `FlowNote`,`a`.`TodoEmps` AS `TodoEmps`,`a`.`TodoEmpsNum` AS `TodoEmpsNum`,`a`.`TodoSta` AS `TodoSta`,`a`.`TaskSta` AS `TaskSta`,0 AS `ListType`,`a`.`Sender` AS `Sender`,`a`.`AtPara` AS `AtPara`,1 AS `MyNum` FROM `wf_generworkflow` AS `a` JOIN wf_generworkerlist AS `b` ON (`b`.`IsEnable`=1) AND (`b`.`IsPass`=0) AND (`a`.`WorkID`=`b`.`WorkID`) AND (`a`.`FK_Node`=`b`.`FK_Node`) AND (`a`.`WFState`!=0) AND (`b`.`WhoExeIt`!=1)  AND a.TaskSta=0 AND  a.WFState!=10 Order by WorkID desc,FK_Emp asc LIMIT 5 OFFSET 10000;
```
下図のように「Out Of Memory」エラーが発生しました。
![test7.png](img/test7.png) 
* 降順(desc)、昇順(asc)を外しても、同じく「Out Of Memory」エラーが発生しました。
![test8.png](img/test8.png) 
懸念：結合後違うテーブル、違うカラムでOrder Byできないのです。

* 同じテーブルでもOrder byに複数カラム、かつ降順(desc)、昇順(asc)を自由に組合せで検索する
wf_generworkerlistテーブルに以下のインデックス追加済みです。
** workid
** fk_emp
** workid,fk_emp
** fk_emp,workid

```
select * from wf_generworkerlist order by workid desc,fk_emp asc limit 5 offset 10000;
```
![test9.png](img/test9.png) 
```
select * from wf_generworkerlist order by fk_emp asc,workid desc limit 5 offset 0;
```
![test10.png](img/test10.png) 
上記いずれも１分以上がかかりました。


懸念：TiDBでは同じテーブルでもOrder byにカラム（カラム組合せインデックス追加済みであることを前提）の降順(desc)、昇順(asc)を自由に組合せすることができないのです。
以下のようにdescやascを外したら、エラーなく検索できる
* 降順(desc)、昇順(asc)を外して、又は全部昇順(asc)で検索を行う（インデックスには昇順(asc)がデフォルトである）
```
select * from wf_generworkerlist order by workid ,fk_emp  limit 5 offset 0;
```
![test11.png](img/test11.png) 
```
select * from wf_generworkerlist order by fk_emp asc,workid asc limit 5 offset 0;
```
![test12.png](img/test12.png) 
上記いずれも0.02秒以内が完了しました。

以下のテストアプリ側に最もアクセスする画面ーー承認待ち/未完了/完了一覧画面である<br/>
※以下はテストデータを表示されている画面イメージです。三画面データが同じタイミングでデータ抽出を行う
* 承認待ち一覧
![test1.png](img/test1.png) 
* 完了一覧
![test2.png](img/test2.png) 

上記の一覧に使う検索文は以下のようものです。
```
SELECT A.* ,F.*,OU.ORDER_NUMBER FROM (SELECT `b`.`FK_Emp` AS `FK_Emp`,`a`.`PRI` AS `PRI`,`a`.`WorkID` AS `WorkID`,`b`.`IsRead` AS `IsRead`,`a`.`Starter` AS `Starter`,`a`.`StarterName` AS `StarterName`,`a`.`WFState` AS `WFState`,`a`.`FK_Dept` AS `FK_Dept`,`a`.`DeptName` AS `DeptName`,`a`.`FK_Flow` AS `FK_Flow`,`a`.`FlowName` AS `FlowName`,`a`.`PWorkID` AS `PWorkID`,`a`.`PFlowNo` AS `PFlowNo`,`b`.`FK_Node` AS `FK_Node`,`b`.`FK_NodeText` AS `NodeName`,`b`.`FK_Dept` AS `WorkerDept`,`a`.`Title` AS `Title`,`a`.`RDT` AS `RDT`,`b`.`RDT` AS `ADT`,`b`.`SDT` AS `SDT`,`b`.`FID` AS `FID`,`a`.`FK_FlowSort` AS `FK_FlowSort`,`a`.`SysType` AS `SysType`,`a`.`SDTOfNode` AS `SDTOfNode`,`b`.`PressTimes` AS `PressTimes`,`a`.`GuestNo` AS `GuestNo`,`a`.`GuestName` AS `GuestName`,`a`.`BillNo` AS `BillNo`,`a`.`FlowNote` AS `FlowNote`,`a`.`TodoEmps` AS `TodoEmps`,`a`.`TodoEmpsNum` AS `TodoEmpsNum`,`a`.`TodoSta` AS `TodoSta`,`a`.`TaskSta` AS `TaskSta`,0 AS `ListType`,`a`.`Sender` AS `Sender`,`a`.`AtPara` AS `AtPara`,1 AS `MyNum` FROM `wf_generworkflow` AS `a` JOIN (select * from `wf_generworkerlist` where FK_Emp='0220320' ) AS `b` ON (`b`.`IsEnable`=1) AND (`b`.`IsPass`=0) AND (`a`.`WorkID`=`b`.`WorkID`) AND (`a`.`FK_Node`=`b`.`FK_Node`) AND (`a`.`WFState`!=0) AND (`b`.`WhoExeIt`!=1)  AND a.TaskSta=0 AND  a.WFState!=10) A left join WF_Flow AS F on A.FK_Flow =F.No LEFT JOIN TT_WF_ORDER_NUMBER AS OU ON A.WorkID = OU.OID WHERE  A.WFState!=10 ORDER BY  A.WorkID DESC LIMIT 12 OFFSET 0;
```
![test13.png](img/test13.png) 
テストアプリのデータベース検索SQL文に色々チューニングした結果、検索（承認待ち、未完了、完了三つの検索を行う）に必要な時間が１秒以内で完成でき、ウェブ画面の描画時間を含めて20秒前後三つのリストを表示できました<br>
<B>億単位のデータを扱うシステムに対して、MySQL、SQL Serverなどのデータベースで対応できない場面に対して、SQL文チューニングに少し工夫が必要であるものの、TiDBは対応できるようであることを検証できました。</B>

### フロー新規（Insert）性能検証
下記の「商品計画申請」リンクを押下されたら、tt_wf_merchandise_plan、nd1track、wf_generworkflow、wf_generworkerlist、tt_wf_order_numberにそれぞれ一レコードを追加し、リンク押下から画面表示するまで10秒以内で完成でき、申請データなしの場合の反応時間との差がなしである。
![test14.png](img/test14.png) 
![test15.png](img/test15.png) 
### フロー承認（Update）性能検証
下記の承認ボタンを押下したら、tt_wf_merchandise_plan、nd1track、wf_generworkflow、wf_generworkerlistなど四つのテーブルにそれぞれレコード更新を行い、承認ボタン押下から承認完了まで10秒以内完了でき、申請データなしの場合の反応時間と差がなしである
![test16.png](img/test16.png) 
![test17.png](img/test17.png) 
### TiDB導入時データ検索チューニングについて
* ビューの使用に控え目に
ビューが絶対に利用できないのではありませんが、全てのRDBにはビューがテーブルのようにレコード集的な存在ではなく、ただのSQL文マクロ変数（変数名はビュー名である）みたいなものである認識を持たせてください。<br>
ビューのSQL文はよくチューニングしていないと検索性能が出ません。又ビューを利用するとチューニングしずく、ビューのSQL文を合わせてチューニングすることを強く推薦する。
* 検索条件、OrderByに表しているカラム、カラムの組合せに対してインデックスの追加は必須になる
あまり沢山なインデックスを追加したら、データベース全体の性能に影響しかねないため、必要不可欠インデックスを原則としする。<br>
※TiDB7.0(2024年5月リリース予定)からインデックス追加補助金ツールを提供する予定です。
* OrderByの性能劣化のためについて
複数テーブルの違いクラムでOrderByに使う場合、又同じテーブルに複数クラムで降順（インデックスにデフォルトで降順）以外でOrderByに設定する場合は性能劣化が発生しました。TiDBは将来のバージョン（2025年以降）で対応する予定ですが、上記の場合は現状プログラムのロジック修正にしかないのです。
