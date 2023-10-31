

Private Sub sqldata_create_Click()


    Dim out_path1 As String
    Dim WorkID As Integer
    Dim MyPK1, MyPK2, MyPK3 As Long
    Dim fso As Object


    'ts1:テーブル：tt_wf_order_number
    'ts2:テーブル：nd1track
    'ts3:テーブル：tt_wf_merchandise_plan
    'ts22:テーブル：nd5track
    'ts23:テーブル：tt_wf_arrival_returns
    't32:テーブル：nd7track
    'ts33:テーブル：tt_wf_stock_adjustment
    'ts4:テーブル：wf_generworkerlist
    'ts5:テーブル：wf_generworkflow
    Dim ts1, ts2, ts3, ts4, ts5, ts22, ts23, ts32, ts33 As Object

    out_path1 = txt_out_path.Text
    WorkID = txt_workID.Text
    Set fso = CreateObject("Scripting.FileSystemObject")

    'ファイルOPEN
    If fso.FileExists(out_path1 + "\tt_wf_order_number01.csv") Then
         Set ts1 = fso.OpenTextFile(out_path1 + "\tt_wf_order_number01.csv", ForAppending, False, TristateTrue)
    Else
         Set ts1 = fso.OpenTextFile(out_path1 + "\tt_wf_order_number01.csv", ForWriting, True, TristateTrue)
    End If


    If fso.FileExists(out_path1 + "\nd1track01.csv") Then
        Set ts2 = fso.OpenTextFile(out_path1 + "\nd1track01.csv", ForAppending, False, TristateTrue)
    Else
        Set ts2 = fso.OpenTextFile(out_path1 + "\nd1track01.csv", ForWriting, True, TristateTrue)
    End If

    If fso.FileExists(out_path1 + "\tt_wf_merchandise_plan01.csv") Then
        Set ts3 = fso.OpenTextFile(out_path1 + "\tt_wf_merchandise_plan01.csv", ForAppending, False, TristateTrue)
    Else
        Set ts3 = fso.OpenTextFile(out_path1 + "\tt_wf_merchandise_plan01.csv", ForWriting, True, TristateTrue)
    End If

    If fso.FileExists(out_path1 + "\nd5track01.csv") Then
        Set ts22 = fso.OpenTextFile(out_path1 + "\nd5track01.csv", ForAppending, False, TristateTrue)
    Else
        Set ts22 = fso.OpenTextFile(out_path1 + "\nd5track01.csv", ForWriting, True, TristateTrue)
    End If

    If fso.FileExists(out_path1 + "\tt_wf_arrival_returns01.csv") Then
        Set ts23 = fso.OpenTextFile(out_path1 + "\tt_wf_arrival_returns01.csv", ForAppending, False, TristateTrue)
    Else
        Set ts23 = fso.OpenTextFile(out_path1 + "\tt_wf_arrival_returns01.csv", ForWriting, True, TristateTrue)
    End If

    If fso.FileExists(out_path1 + "\nd7track01.csv") Then
        Set ts32 = fso.OpenTextFile(out_path1 + "\nd7track01.csv", ForAppending, False, TristateTrue)
    Else
        Set ts32 = fso.OpenTextFile(out_path1 + "\nd7track01.csv", ForWriting, True, TristateTrue)
    End If

    If fso.FileExists(out_path1 + "\tt_wf_stock_adjustment01.csv") Then
        Set ts33 = fso.OpenTextFile(out_path1 + "\tt_wf_stock_adjustment01.csv", ForAppending, False, TristateTrue)
    Else
        Set ts33 = fso.OpenTextFile(out_path1 + "\tt_wf_stock_adjustment01.csv", ForWriting, True, TristateTrue)
    End If

    If fso.FileExists(out_path1 + "\wf_generworkerlist01.csv") Then
        Set ts4 = fso.OpenTextFile(out_path1 + "\wf_generworkerlist01.csv", ForAppending, False, TristateTrue)
    Else
        Set ts4 = fso.OpenTextFile(out_path1 + "\wf_generworkerlist01.csv", ForWriting, True, TristateTrue)
    End If

    If fso.FileExists(out_path1 + "\wf_generworkflow01.csv") Then
        Set ts5 = fso.OpenTextFile(out_path1 + "\wf_generworkflow01.csv", ForAppending, False, TristateTrue)
    Else
        Set ts5 = fso.OpenTextFile(out_path1 + "\wf_generworkflow01.csv", ForWriting, True, TristateTrue)
    End If

    'ファイル設定
    'Set ts1 = fso.OpenTextFile(out_path1 + "\insert_tt_wf_order_number.csv", ForWriting, True, TristateTrue)
    'Set ts2 = fso.OpenTextFile(out_path1 + "\insert_nd1track.csv", ForWriting, True, TristateTrue)
    'Set ts3 = fso.OpenTextFile(out_path1 + "\tt_wf_merchandise_plan.csv", ForWriting, True, TristateTrue)
    'Set ts4 = fso.OpenTextFile(out_path1 + "\wf_generworkerlist.csv", ForWriting, True, TristateTrue)
    'Set ts5 = fso.OpenTextFile(out_path1 + "\wf_generworkflow.csv", ForWriting, True, TristateTrue)


    '商品計画申請

    'テーブル：tt_wf_order_numberデータ作成
    Dim tt_wf_order_number_csv As String   ' CSV に書き込む全データ
    Dim line As String ' 1 行分のデータ
    Dim delimiter, csvdl As String  ' 区切り
    Dim item_OID As Variant ' 項目設定
    Dim item_ordernum1, item_ordernum2, item_ordernum3 As String
    Dim int_ordernum1, int_ordernum2, int_ordernum3 As Variant

    Dim item_frmb As String
    Dim item_summry As String
    Dim item_title As String
    Dim item_02 As String
    Dim dd As Date

    Dim i, total As Long



    'データ件数
    total = 10

    '最新ordernum取得
    item_OID = txt_workID.Text
    item_ordernum1 = txt_ordernum1.Text
    item_ordernum2 = txt_ordernum2.Text
    item_ordernum3 = txt_ordernum3.Text

    '2110239162
    MyPK1 = Val(txt_pkid1.Text)
    MyPK2 = Val(txt_pkid2.Text)
    MyPK3 = Val(txt_pkid3.Text)

    line = ""
    delimiter = ","   ' カンマ区切り
    csvdl = "'"


    ''===================================================================================================================
    ''
    ''商品計画申請　001
    ''
    ''===================================================================================================================
     'ORDERNUM初期値設定
    int_ordernum1 = Val(Mid(item_ordernum1, 8, Len(item_ordernum1) - 7))

    For i = 1 To total Step 1

        item_OID = Val(item_OID) + 1
        'MEP202350000007
        int_ordernum1 = int_ordernum1 + 1
        item_ordernum1 = "MEP2023" & CStr(int_ordernum1)

        'tt_wf_order_numberのデータを書き込む
        '122, '001', date_format(now(),'%Y%m%d'), 'MEP202350000007', sysdate(), '0543956',NULL,NULL
        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "001" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyymmdd") & csvdl & delimiter & _
                csvdl & item_ordernum1 & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & Null & csvdl & delimiter & csvdl & Null & csvdl

         ts1.writeline (line)


        ''nd1trackのデータを書き込む
        ''(1244156279,1,'発送',0,122,101,'新規申請',102,'上司承認','0543956','真洲句 優祈音','0220320','駄御井 進',now(),0.0,'','','@SendNode=101','(0543956,真洲句 優祈音)',
        ''{"ShenQingRen":"真洲句 優祈音","MyNum":"1","CDT":"2023-10-04 14:30","ShenQingRiJi":"2023-10-02","ShenQingRenBuMen":"ＤＸシステム開発Ｇ","FK_DeptText":"ＤＸシステム開発Ｇ",
        ''"RDT":"2023-10-04 14:30","Rec":"0543956","Title":"ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は2023-10-04 14:30で開始.","OID":"114","FK_Dept":"11637","Emps":"0543956","FID":"0","FK_NY":"2023-10","EndJSON":"0"}');

        MyPK1 = MyPK1 + 1
        item_frmb = "{" & """" & "ShenQingRen" & """" & ":" & """" & "真洲句 優祈音" & """" & _
            "," & """" & "MyNum" & """" & ":" & """" & "1" & """" & _
            "," & """" & "CDT" & """" & ":" & """" & "2023-10-04 14:30" & """" & _
            "," & """" & "ShenQingRiJi" & """" & ":" & """" & "2023-10-02" & """" & _
            "," & """" & "ShenQingRenBuMen" & """" & ":" & """" & "ＤＸシステム開発Ｇ" & """" & _
            "," & """" & "FK_DeptText" & """" & ":" & """" & "ＤＸシステム開発Ｇ" & """" & _
            "," & """" & "RDT" & """" & ":" & """" & "2023-10-04 14:30" & """" & _
            "," & """" & "Rec" & """" & ":" & """" & "0543956" & """" & _
            "," & """" & "Title" & """" & ":" & """" & "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は2023-10-05 17:22で開始." & """" & _
            "," & """" & "OID" & """" & ":" & """" & item_OID & """" & _
            "," & """" & "FK_Dept" & """" & ":" & """" & "11637" & """" & _
            "," & """" & "Emps" & """" & ":" & """" & "0543956" & """" & "," & """" & "FID" & """" & ":" & """" & "0" & """" & _
            "," & """" & "FK_NY" & """" & ":" & """" & "2023-10" & """" & "," & """" & "EndJSON" & """" & ":" & """" & "0" & """" & "}"

        line = csvdl & MyPK1 & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & "発送" & csvdl & delimiter & _
                csvdl & 0 & csvdl & delimiter & item_OID & delimiter & _
                csvdl & 101 & csvdl & delimiter & csvdl & "新規申請" & csvdl & delimiter & _
                csvdl & 102 & csvdl & delimiter & csvdl & "上司承認" & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & "真洲句 優祈音" & csvdl & delimiter & _
                csvdl & "0220320" & csvdl & delimiter & csvdl & "駄御井 進" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "@SendNode=101" & csvdl & delimiter & csvdl & "(0543956,真洲句 優祈音)" & csvdl & delimiter & _
                csvdl & item_frmb & csvdl

          ts2.writeline (line)


        ''tt_wf_merchandise_planのデータを書き込む
        ''(122,'イオンアイビス（株）','ＤＸシステム開発Ｇ','0543956','高田　優祈音','商品計画テスト（王）',NULL,NULL,NULL,
        ''{"AgentMode":"0","AutoApprovalMode":"N","content":[{"value":"高田　優祈音","name":"従業員氏名"},{"value":"0543956","name":"社員番号"},{"value":"イオンアイビス（株）","name":"会社名称"},{"value":"ＤＸシステム開発Ｇ","name":"所属"}]}',
        'date_format(now(),'%Y-%m-%d %h:%i:%s'),'0543956',date_format(now(),'%Y-%m-%d %h:%i:%s'),'0543956',2,'11637',
        'concat('ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は',date_format(now(),'%Y-%m-%d %h:%i') ,'で開始.'),'0','',102,0,'@0543956,真洲句 優祈音@','真洲句 優祈音','2023-10-06','ＤＸシステム開発Ｇ',date_format(now(),'%Y-%m-%d %h:%i'),0,date_format(now(),'%Y-%m-%d %h:%i'),'0543956','0543956',
        '1,0,'','','','','','','',date_format(now(),'%Y-%m-%d %h:%i'),102,date_format(now(),'%Y-%m-%d %h:%i'),0.0,'0543956','0543956','2023-10');
        item_summry = "{" & """" & "AgentMode" & """" & ":" & """" & "0" & """" & _
            "," & """" & "AutoApprovalMode" & """" & ":" & """" & "N" & """" & _
            "," & """" & "content" & """" & ":" & _
            "[{" & """" & "value" & """" & ":" & """" & "高田　優祈音" & """" & "," & """" & "name" & """" & ":" & """" & "従業員氏名" & """" & "}" & _
            ",{" & """" & "value" & """" & ":" & """" & "0543956" & """" & "," & """" & "name" & """" & ":" & """" & "社員番号" & """" & "}" & _
            ",{" & """" & "value" & """" & ":" & """" & "イオンアイビス（株）" & """" & "," & """" & "name" & """" & ":" & """" & "会社名称" & """" & "}" & _
            ",{" & """" & "value" & """" & ":" & """" & "ＤＸシステム開発Ｇ" & """" & "," & """" & "name" & """" & ":" & """" & 所属 & """" & "}]}"

         line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "イオンアイビス（株）" & csvdl & delimiter & csvdl & "ＤＸシステム開発Ｇ" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & "高田　優祈音" & csvdl & delimiter & _
                csvdl & "商品計画テスト（王）" & i & csvdl & delimiter & csvdl & Null & csvdl & delimiter & csvdl & Null & csvdl & delimiter & csvdl & Null & csvdl & delimiter & csvdl & item_summry & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & "0" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & 102 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & "@0543956,真洲句 優祈音@" & csvdl & delimiter & csvdl & "真洲句 優祈音" & csvdl & delimiter & _
                csvdl & "2023-10-06" & csvdl & delimiter & csvdl & "ＤＸシステム開発Ｇ" & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & 0 & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & _
                csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm") & csvdl & delimiter & csvdl & 102 & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm") & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & "2023-10" & csvdl

          ts3.writeline (line)


        ''wf_generworkerlistのデータを書き込む
        '(122,'0220320',102,0,'駄御井 進','上司承認','001','39510',DATE_ADD(now(), INTERVAL 1 DAY),DATE_ADD(now(), INTERVAL 1 DAY),now(),now(),1,0,0,0,'0543956,真洲句 優祈音',1,0,'','',0,'','','@FK_DeptT=デジタルＳ本部長'),
        dd = DateAdd("d", 1, Now())
        item_title = "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は" & Format(Now(), "yyyy-mm-dd hh:mm") & "で開始."
        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "0220320" & csvdl & delimiter & csvdl & 102 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "駄御井 進" & csvdl & delimiter & csvdl & "上司承認" & csvdl & delimiter & csvdl & "001" & csvdl & delimiter & csvdl & "39510" & csvdl & delimiter & _
                csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "0543956,真洲句 優祈音" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "@FK_DeptT=デジタルＳ本部長" & csvdl

          ts4.writeline (line)

        '(122,'0543956',101,0,'真洲句 優祈音','新規申請','001','11637','無',now(),now(),now(),1,0,1,0,'',1,0,'','',0,'','','');
        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & 101 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "真洲句 優祈音" & csvdl & delimiter & csvdl & "新規申請" & csvdl & delimiter & csvdl & "001" & csvdl & delimiter & csvdl & "11637" & csvdl & delimiter & csvdl & "無" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "" & csvdl

          ts4.writeline (line)

        ''wf_generworkflowのデータを書き込む
        '(122,0,'102','','001','商品計画申請',concat('ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は',date_format(now(),'%Y-%m-%d %H:%i') ,'で開始.'),0,2,'0543956','真洲句 優祈音','0543956',date_format(now(),'%Y-%m-%d %H:%i:%s'),date_format(now(),'%Y-%m-%d %H:%i'),102,'上司承認',
        ''11637','ＤＸシステム開発Ｇ',1,DATE_ADD(now(), INTERVAL 1 DAY),'','','',0,0,0,'','','','','','0220320,駄御井進;',1,0,'@LastTruckID=1244156279','@0543956,真洲句 優祈音@','','2023-10',0,0,0,'','','')
        dd = DateAdd("d", 1, Now())
        item_01 = "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は" & Format(Now(), "yyyy-mm-dd hh:mm") & "で開始."
        item_02 = "@LastTruckID=" & item_ordernum1

        line = csvdl & item_OID & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "102" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "001" & csvdl & delimiter & _
                csvdl & "商品計画申請" & csvdl & delimiter & csvdl & item_01 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 2 & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & "真洲句 優祈音" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & 102 & csvdl & delimiter & csvdl & "上司承認" & csvdl & delimiter & _
                csvdl & "11637" & csvdl & delimiter & csvdl & "ＤＸシステム開発Ｇ" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & _
                csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "0220320,駄御井進;" & csvdl & delimiter & 1 & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & item_02 & csvdl & delimiter & _
                csvdl & "@0543956,真洲句 優祈音@" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "2023-10" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl


          ts5.writeline (line)

    Next

    ' ファイルを閉じる
    ts2.Close
    ts3.Close

    ' 後始末
    Set ts2 = Nothing
    Set ts3 = Nothing

    txt_ordernum1.Text = CStr(item_ordernum1)
    txt_pkid1.Text = MyPK1


   ''===================================================================================================================
    ''
    ''入荷返品申請　005
    ''
    ''===================================================================================================================
    'ORDERNUM初期値設定
    int_ordernum2 = Val(Mid(item_ordernum2, 9, Len(item_ordernum2) - 8))
    MyPK2 = Val(txt_pkid2.Text)

    For i = 1 To total Step 1

        item_OID = Val(item_OID) + 1
        'ALRT202350000002
        int_ordernum2 = int_ordernum2 + 1
        item_ordernum2 = "ALRT2023" & CStr(int_ordernum2)

        '①tt_wf_order_numberのデータを書き込む
        '(123,'005', date_format(now(),'%Y%m%d'), 'ALRT202350000002', sysdate(), '0543956',NULL,NULL);
        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "005" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyymmdd") & csvdl & delimiter & _
                csvdl & item_ordernum2 & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & Null & csvdl & delimiter & csvdl & Null & csvdl

         ts1.writeline (line)


        ''②nd5trackのデータを書き込む
        '(1609962365,1,'発送',0,123,501,'新規申請',502,'上司承認','0543956','真洲句 優祈音','0220320','駄御井 進',now(),0.0,'','','@SendNode=501','(0543956,真洲句 優祈音)',
        ''{"ShenQingRen":"真洲句 優祈音","MyNum":"1","CDT":"2023-10-04 14:38","ShenQingRiJi":"2023-10-04","ShenQingRenBuMen":"ＤＸシステム開発Ｇ","FK_DeptText":"ＤＸシステム開発Ｇ",
        ''''"RDT":"2023-10-04 14:38","Rec":"0543956","Title":"ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は2023-10-04 14:38で開始.","OID":"120","FK_Dept":"11637","Emps":"0543956","FID":"0","FK_NY":"2023-10","EndJSON":"0"}');

        MyPK2 = MyPK2 + 1
        item_frmb = "{" & """" & "ShenQingRen" & """" & ":" & """" & "真洲句 優祈音" & """" & _
            "," & """" & "MyNum" & """" & ":" & """" & "1" & """" & _
            "," & """" & "CDT" & """" & ":" & """" & "2023-10-04 14:38" & """" & _
            "," & """" & "ShenQingRiJi" & """" & ":" & """" & "2023-10-04" & """" & _
            "," & """" & "ShenQingRenBuMen" & """" & ":" & """" & "ＤＸシステム開発Ｇ" & """" & _
            "," & """" & "FK_DeptText" & """" & ":" & """" & "ＤＸシステム開発Ｇ" & """" & _
            "," & """" & "RDT" & """" & ":" & """" & "2023-10-04 14:38" & """" & _
            "," & """" & "Rec" & """" & ":" & """" & "0543956" & """" & _
            "," & """" & "Title" & """" & ":" & """" & "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は2023-10-05 17:22で開始." & """" & _
            "," & """" & "OID" & """" & ":" & """" & item_OID & """" & _
            "," & """" & "FK_Dept" & """" & ":" & """" & "11637" & """" & _
            "," & """" & "Emps" & """" & ":" & """" & "0543956" & """" & "," & """" & "FID" & """" & ":" & """" & "0" & """" & _
            "," & """" & "FK_NY" & """" & ":" & """" & "2023-10" & """" & "," & """" & "EndJSON" & """" & ":" & """" & "0" & """" & "}"

        line = csvdl & MyPK2 & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & _
                csvdl & "発送" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & item_OID & csvdl & delimiter & csvdl & 501 & csvdl & delimiter & csvdl & "新規申請" & csvdl & delimiter & csvdl & 502 & csvdl & delimiter & csvdl & "上司承認" & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & "真洲句 優祈音" & csvdl & delimiter & _
                csvdl & "0220320" & csvdl & delimiter & csvdl & "駄御井 進" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "@SendNode=101" & csvdl & delimiter & csvdl & "(0543956,真洲句 優祈音)" & csvdl & delimiter & _
                csvdl & item_frmb & csvdl

          ts22.writeline (line)


        ''③tt_wf_arrival_returnsのデータを書き込む
        '(123,'イオンアイビス（株）','ＤＸシステム開発Ｇ','0543956','高田　優祈音','111','test','A01','test','11101','test','入荷返品テスト（王）','入荷日,仕入伝票No.,返品理由コード,商品名,型番,JANコード,数量,原単価,売単価,総原価,総売価 ? ',NULL,NULL,NULL,
        ''{"AgentMode":"0","AutoApprovalMode":"N","content":[{"value":"高田　優祈音","name":"従業員氏名"},{"value":"0543956","name":"社員番号"},{"value":"イオンアイビス（株）","name":"会社名称"},{"value":"ＤＸシステム開発Ｇ","name":"所属"}]}',
        ''2023-10-04 14:39:17','0543956',date_format(now(),'%Y-%m-%d %H:%i:%s'),'0543956',2,'11637',
        ''concat('ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は',date_format(now(),'%Y-%m-%d %h:%i') ,'で開始.'),'0','',502,0,'','真洲句 優祈音',
        ''date_format(now(),'%Y-%m-%d'),'ＤＸシステム開発Ｇ',date_format(now(),'%Y-%m-%d %H:%i'),0,date_format(now(),'%Y-%m-%d %H:%i'),'0543956','0543956',1,0,'','','','','','','',
        ''date_format(now(),'%Y-%m-%d %H:%i:%s'),502,date_format(now(),'%Y-%m-%d %H:%i:%s'),0.0,'0543956','0543956','2023-10');



        '{"AgentMode":"0","AutoApprovalMode":"N","content":[{"value":"高田　優祈音","name":"従業員氏名"},{"value":"0543956","name":"社員番号"},{"value":"イオンアイビス（株）","name":"会社名称"},{"value":"ＤＸシステム開発Ｇ","name":"所属"}]}',
        item_summry = "{" & """" & "AgentMode" & """" & ":" & """" & "0" & """" & _
            "," & """" & "AutoApprovalMode" & """" & ":" & """" & "N" & """" & _
            "," & """" & "content" & """" & ":" & _
            "[{" & """" & "value" & """" & ":" & """" & "高田　優祈音" & """" & "," & """" & "name" & """" & ":" & """" & "従業員氏名" & """" & "}" & _
            ",{" & """" & "value" & """" & ":" & """" & "0543956" & """" & "," & """" & "name" & """" & ":" & """" & "社員番号" & """" & "}" & _
            ",{" & """" & "value" & """" & ":" & """" & "イオンアイビス（株）" & """" & "," & """" & "name" & """" & ":" & """" & "会社名称" & """" & "}" & _
            ",{" & """" & "value" & """" & ":" & """" & "ＤＸシステム開発Ｇ" & """" & "," & """" & "name" & """" & ":" & """" & 所属 & """" & "}]}"

        item_title = "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は" & Format(Now(), "yyyy-mm-dd hh:mm") & "で開始."

        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "イオンアイビス（株）" & csvdl & delimiter & csvdl & "ＤＸシステム開発Ｇ" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & "高田　優祈音" & csvdl & delimiter & _
                csvdl & "111" & csvdl & delimiter & csvdl & "test" & csvdl & delimiter & csvdl & "A01" & csvdl & delimiter & csvdl & "test" & csvdl & delimiter & _
                csvdl & "入荷返品テスト（王）" & i & csvdl & delimiter & _
                csvdl & "入荷日,仕入伝票No.,返品理由コード,商品名,型番,JANコード,数量,原単価,売単価,総原価,総売価 ? " & csvdl & delimiter & csvdl & Null & csvdl & delimiter & csvdl & Null & csvdl & delimiter & csvdl & Null & csvdl & delimiter & _
                csvdl & item_summry & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & 2 & csvdl & delimiter & csvdl & "11637" & csvdl & delimiter & _
                csvdl & item_title & csvdl & delimiter & csvdl & "0" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 502 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "真洲句 優祈音" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd") & csvdl & delimiter & csvdl & "ＤＸシステム開発Ｇ" & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm") & csvdl & delimiter & _
                csvdl & 0 & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm") & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 502 & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & "2023-10" & csvdl

          ts23.writeline (line)


        ''④wf_generworkerlistのデータを書き込む
        '(123,'0220320',502,0,'駄御井 進','上司承認','005','39510',DATE_ADD(now(), INTERVAL 1 DAY),DATE_ADD(now(), INTERVAL 1 DAY),now(),now(),1,0,0,0,'0543956,真洲句 優祈音',1,0,'','',0,'','','@FK_DeptT=デジタルＳ本部長'),

        dd = DateAdd("d", 1, Now())
        item_title = "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は" & Format(Now(), "yyyy-mm-dd hh:mm") & "で開始."
        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "0220320" & csvdl & delimiter & csvdl & 502 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "駄御井 進" & csvdl & delimiter & csvdl & "上司承認" & csvdl & delimiter & csvdl & "005" & csvdl & delimiter & csvdl & "39510" & csvdl & delimiter & _
                csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "0543956,真洲句 優祈音" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "@FK_DeptT=デジタルＳ本部長" & csvdl

          ts4.writeline (line)

        '(123,'0543956',501,0,'真洲句 優祈音','新規申請','005','11637','無',now(),now(),now(),1,0,1,0,'',1,0,'','',0,'','','');;
        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & 501 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "真洲句 優祈音" & csvdl & delimiter & csvdl & "新規申請" & csvdl & delimiter & csvdl & "005" & csvdl & delimiter & csvdl & "11637" & csvdl & delimiter & csvdl & "無" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl

          ts4.writeline (line)

        ''⑤wf_generworkflowのデータを書き込む
        ''123,0,'102','','005','入荷返品申請',concat('ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は',date_format(now(),'%Y-%m-%d %H:%i') ,'で開始.'),0,2,'0543956','真洲句 優祈音','0543956',date_format(now(),'%Y-%m-%d %H:%i:%s'),date_format(now(),'%Y-%m-%d %H:%i'),
        ''502,'上司承認','11637','ＤＸシステム開発Ｇ',1,DATE_ADD(now(), INTERVAL 1 DAY),'','','',0,0,0,'','','','','','0220320,駄御井進;',1,0,'@LastTruckID=1609962365','@0543956,真洲句 優祈音@','','2023-10',0,0,0,'','','');
        dd = DateAdd("d", 1, Now())
        item_01 = "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は" & Format(Now(), "yyyy-mm-dd hh:mm") & "で開始."
        item_02 = "@LastTruckID=" & item_ordernum2

        line = csvdl & item_OID & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "102" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "005" & csvdl & delimiter & _
                csvdl & "入荷返品申請" & csvdl & delimiter & csvdl & item_01 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 2 & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & "真洲句 優祈音" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & 502 & csvdl & delimiter & csvdl & "上司承認" & csvdl & delimiter & _
                csvdl & "11637" & csvdl & delimiter & csvdl & "ＤＸシステム開発Ｇ" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & _
                csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "0220320,駄御井進;" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & item_02 & csvdl & delimiter & _
                csvdl & "@0543956,真洲句 優祈音@" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "2023-10" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl



          ts5.writeline (line)

    Next


    ' ファイルを閉じる
    ts22.Close
    ts23.Close

    ' 後始末
    Set ts22 = Nothing
    Set ts23 = Nothing

    txt_ordernum2.Text = CStr(item_ordernum2)
    txt_pkid2.Text = MyPK2


    ''===================================================================================================================
    ''
    ''在庫調整 007
    ''
    ''===================================================================================================================
    'ORDERNUM初期値設定
    int_ordernum3 = Val(Mid(item_ordernum3, 9, Len(item_ordernum3) - 8))
    MyPK3 = Val(txt_pkid3.Text)

    For i = 1 To total Step 1

        item_OID = Val(item_OID) + 1
        'SKAT202350000002
        int_ordernum3 = int_ordernum3 + 1
        item_ordernum3 = "SKAT2023" & CStr(int_ordernum3)

        '①tt_wf_order_numberのデータを書き込む
        '(124,'007', date_format(now(),'%Y%m%d'),'SKAT202350000002',sysdate(),'0543956',NULL,NULL);
        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "007" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyymmdd") & csvdl & delimiter & _
                csvdl & item_ordernum3 & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & Null & csvdl & delimiter & csvdl & Null & csvdl

         ts1.writeline (line)


        ''②nd7trackのデータを書き込む
        ''(781878025,1,'発送',0,124,701,'新規申請',702,'上司承認','0543956','真洲句 優祈音','0220320','駄御井 進',now(),0.0,'','','@SendNode=701','(0543956,真洲句 優祈音)',
        '''{"ShenQingRen":"真洲句 優祈音","MyNum":"1","CDT":"2023-10-04 14:35","ShenQingRiJi":"2023-10-04","ShenQingRenBuMen":"ＤＸシステム開発Ｇ","FK_DeptText":"ＤＸシステム開発Ｇ","RDT":"2023-10-04 14:35","Rec":"0543956",
        ''''"Title":"ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は2023-10-04 14:35で開始.","OID":"119","FK_Dept":"11637","Emps":"0543956","FID":"0","FK_NY":"2023-10","EndJSON":"0"}');


        MyPK3 = MyPK3 + 1
        item_frmb = "{" & """" & "ShenQingRen" & """" & ":" & """" & "真洲句 優祈音" & """" & _
            "," & """" & "MyNum" & """" & ":" & """" & "1" & """" & _
            "," & """" & "CDT" & """" & ":" & """" & "2023-10-04 14:38" & """" & _
            "," & """" & "ShenQingRiJi" & """" & ":" & """" & "2023-10-04" & """" & _
            "," & """" & "ShenQingRenBuMen" & """" & ":" & """" & "ＤＸシステム開発Ｇ" & """" & _
            "," & """" & "FK_DeptText" & """" & ":" & """" & "ＤＸシステム開発Ｇ" & """" & _
            "," & """" & "RDT" & """" & ":" & """" & "2023-10-04 14:38" & """" & _
            "," & """" & "Rec" & """" & ":" & """" & "0543956" & """" & _
            "," & """" & "Title" & """" & ":" & """" & "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は2023-10-05 17:22で開始." & """" & _
            "," & """" & "OID" & """" & ":" & """" & item_OID & """" & _
            "," & """" & "FK_Dept" & """" & ":" & """" & "11637" & """" & _
            "," & """" & "Emps" & """" & ":" & """" & "0543956" & """" & "," & """" & "FID" & """" & ":" & """" & "0" & """" & _
            "," & """" & "FK_NY" & """" & ":" & """" & "2023-10" & """" & "," & """" & "EndJSON" & """" & ":" & """" & "0" & """" & "}"

        line = csvdl & MyPK3 & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & _
                csvdl & "発送" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & item_OID & csvdl & delimiter & csvdl & 701 & csvdl & delimiter & csvdl & "新規申請" & csvdl & delimiter & csvdl & 702 & csvdl & delimiter & csvdl & "上司承認" & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & "真洲句 優祈音" & csvdl & delimiter & _
                csvdl & "0220320" & csvdl & delimiter & csvdl & "駄御井 進" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "@SendNode=701" & csvdl & delimiter & csvdl & "(0543956,真洲句 優祈音)" & csvdl & delimiter & _
                csvdl & item_frmb & csvdl

          ts32.writeline (line)


        ''③tt_wf_stock_adjustmentのデータを書き込む
        ''(124,'イオンアイビス（株）','ＤＸシステム開発Ｇ','0543956','高田　優祈音','在庫調整申請テスト（王）',
        ''倉庫コード,倉庫名,部門コード,部門名,商品コード,品種名,メーカー名,型番,入荷日,ロットNo,原単価,売単価,取引先,仕入伝票No,販売期間,入荷数,単品追加数,単品削除数,入庫数計,出荷済数,ロス数,返品数,不良品数,出荷数計,在庫数,展示品数,出荷準備中数,在庫数計 ? ?111,test,11102,部門テスト1,1110201,,,,,,,,,,,,,,,,,,,,,,, ?',
        ''NULL,NULL,NULL,
        '''{"AgentMode":"0","AutoApprovalMode":"N","content":[{"value":"高田　優祈音","name":"従業員氏名"},{"value":"0543956","name":"社員番号"},{"value":"イオンアイビス（株）","name":"会社名称"},{"value":"ＤＸシステム開発Ｇ","name":"所属"}]}',
        ''date_format(now(),'%Y-%m-%d %H:%i:%s'),'0543956',date_format(now(),'%Y-%m-%d %H:%i:%s'),'0543956',2,'11637',concat('ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は',date_format(now(),'%Y-%m-%d %H:%i') ,'で開始.'),
        '''0','',702,0,'@0543956,真洲句 優祈音@','真洲句 優祈音',date_format(now(),'%Y-%m-%d'),'ＤＸシステム開発Ｇ',date_format(now(),'%Y-%m-%d %H:%i'),0,date_format(now(),'%Y-%m-%d %H:%i'),'0543956','0543956',1,0,'','','','','','','',
        ''date_format(now(),'%Y-%m-%d %H:%i:%s'),702,date_format(now(),'%Y-%m-%d %H:%i:%s'),0.0,'0543956','0543956','2023-10');

        '''{"AgentMode":"0","AutoApprovalMode":"N","content":[{"value":"高田　優祈音","name":"従業員氏名"},{"value":"0543956","name":"社員番号"},{"value":"イオンアイビス（株）","name":"会社名称"},{"value":"ＤＸシステム開発Ｇ","name":"所属"}]}',
        item_summry = "{" & """" & "AgentMode" & """" & ":" & """" & "0" & """" & _
            "," & """" & "AutoApprovalMode" & """" & ":" & """" & "N" & """" & _
            "," & """" & "content" & """" & ":" & _
            "[{" & """" & "value" & """" & ":" & """" & "高田　優祈音" & """" & "," & """" & "name" & """" & ":" & """" & "従業員氏名" & """" & "}" & _
            ",{" & """" & "value" & """" & ":" & """" & "0543956" & """" & "," & """" & "name" & """" & ":" & """" & "社員番号" & """" & "}" & _
            ",{" & """" & "value" & """" & ":" & """" & "イオンアイビス（株）" & """" & "," & """" & "name" & """" & ":" & """" & "会社名称" & """" & "}" & _
            ",{" & """" & "value" & """" & ":" & """" & "ＤＸシステム開発Ｇ" & """" & "," & """" & "name" & """" & ":" & """" & 所属 & """" & "}]}"


        item_title = "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は" & Format(Now(), "yyyy-mm-dd hh:mm") & "で開始."

        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "イオンアイビス（株）" & csvdl & delimiter & csvdl & "ＤＸシステム開発Ｇ" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & "高田　優祈音" & csvdl & delimiter & csvdl & "在庫調整申請テスト（王）" & i & csvdl & delimiter & _
                csvdl & "倉庫コード,倉庫名,部門コード,部門名,商品コード,品種名,メーカー名,型番,入荷日,ロットNo,原単価,売単価,取引先,仕入伝票No,販売期間,入荷数,単品追加数,単品削除数,入庫数計,出荷済数,ロス数,返品数,不良品数,出荷数計,在庫数,展示品数,出荷準備中数,在庫数計 ? ?111,test,11102,部門テスト1,1110201,,,,,,,,,,,,,,,,,,,,,,, ? " & csvdl & delimiter & _
                csvdl & Null & csvdl & delimiter & csvdl & Null & csvdl & delimiter & csvdl & Null & csvdl & delimiter & _
                csvdl & item_summry & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & 2 & csvdl & delimiter & csvdl & "11637" & csvdl & delimiter & _
                csvdl & item_title & csvdl & delimiter & csvdl & "0" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 702 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "@0543956,真洲句 優祈音@" & csvdl & delimiter & csvdl & "真洲句 優祈音" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd") & csvdl & delimiter & csvdl & "ＤＸシステム開発Ｇ" & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm") & csvdl & delimiter & _
                csvdl & 0 & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm") & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 702 & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & "0543956" & csvdl & delimiter & csvdl & "2023-10" & csvdl

          ts33.writeline (line)


        ''④wf_generworkerlistのデータを書き込む
        ''(124,'0220320',702,0,'駄御井 進','上司承認','007','39510',DATE_ADD(now(), INTERVAL 1 DAY),DATE_ADD(now(), INTERVAL 1 DAY),now(),now(),1,0,0,0,'0543956,真洲句 優祈音',1,0,'','',0,'','','@FK_DeptT=デジタルＳ本部長'),
        dd = DateAdd("d", 1, Now())
        item_title = "ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は" & Format(Now(), "yyyy-mm-dd hh:mm") & "で開始."
        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "0220320" & csvdl & delimiter & csvdl & 702 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "駄御井 進" & csvdl & delimiter & csvdl & "上司承認" & csvdl & delimiter & csvdl & "007" & csvdl & delimiter & csvdl & "39510" & csvdl & delimiter & _
                csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(dd, "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "0543956,真洲句 優祈音" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & _
                csvdl & "@FK_DeptT=デジタルＳ本部長" & csvdl

          ts4.writeline (line)

        ''(124,'0543956',701,0,'真洲句 優祈音','新規申請','007','11637','無',now(),now(),now(),1,0,1,0,'',1,0,'','',0,'','','');
        line = csvdl & item_OID & csvdl & delimiter & _
                csvdl & "0543956" & csvdl & delimiter & csvdl & 701 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "真洲句 優祈音" & csvdl & delimiter & csvdl & "新規申請" & csvdl & delimiter & csvdl & "007" & csvdl & delimiter & csvdl & "11637" & csvdl & delimiter & csvdl & "無" & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & _
                csvdl & Format(Now(), "yyyy-mm-dd hh:mm:ss") & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & 1 & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & 0 & csvdl & delimiter & _
                csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl & delimiter & csvdl & "" & csvdl

          ts4.writeline (line)

        ''⑤wf_generworkflowのデータを書き込む
        ''(124,0,'102','','007','在庫調整申請',concat('ＤＸシステム開発Ｇ-0543956,真洲句 優祈音は',date_format(now(),'%Y-%m-%d %H:%i') ,'で開始.'),0,2,'0543956','真洲句 優祈音','0543956',date_format(now(),'%Y-%m-%d %H:%i:%s'),date_format(now(),'%Y-%m-%d %H:%i'),
        ''702,'上司承認','11637','ＤＸシステム開発Ｇ',1,DATE_ADD(now(), INTERVAL 1 DAY),'','','',0,0,0,'','','','','','0220320,駄御井進;',1,0,'@LastTruckID=781878025','@0543956,真洲句 優祈音@','','2023-10',0,0,0,'','','');


    Next


    ' ファイルを閉じる
    ts32.Close
    ts33.Close
    ts1.Close
    ts4.Close
    ts5.Close

    ' 後始末
    Set ts32 = Nothing
    Set ts33 = Nothing
    Set ts1 = Nothing
    Set ts4 = Nothing
    Set ts5 = Nothing
    Set fso = Nothing

    txt_ordernum3.Text = CStr(item_ordernum3)
    txt_pkid3.Text = MyPK3

    txt_workID.Text = CStr(item_OID)

    MsgBox ("ファイルにデータを出力しました。")



End Sub


