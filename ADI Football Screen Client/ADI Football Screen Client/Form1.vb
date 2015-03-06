﻿Imports System.IO
Imports System
Imports System.Xml

'with preview
' NB to make this work with Broadcast Play, copy Caspar v2.0.7 over the Caspar contents that are already in BP directory
Public Class CasparTest2NoPvw
    Dim CasparDevice As New Svt.Caspar.CasparDevice
    Dim CasparCGDataCollection As New Svt.Caspar.CasparCGDataCollection
    Dim count As Integer
    Dim countBPS As Integer
    Dim countScores As Integer
    Dim countPlaylist As Integer
    Dim aa As Integer
    Dim crawlToggle As Boolean
    Dim playlistPosition As Integer = 0
    Dim playlistPositionInGame As Integer = 0
    Dim backgroundOnPGM As Boolean = False
    Dim backgroundOnPVW As Boolean = False
    Dim PreMatchPlayNext As Boolean = False
    Dim msg1Colour As String
    Dim msg2Colour As String
    Dim msg3Colour As String
    Dim msg4Colour As String
    Dim msg5Colour As String
    Dim msg6Colour As String
    Dim msg7Colour As String
    Dim msg8Colour As String
    Dim homeTeamCount As Integer
    Dim awayTeamCount As Integer

    Private Sub Connect_Click(sender As Object, e As EventArgs) Handles Connect.Click
        CasparDevice.Settings.Hostname = "localhost"
        CasparDevice.Settings.Port = 5250
        Connect.BackColor = Color.Red
        If Me.CasparDevice.IsConnected = False Then
            Me.CasparDevice.Connect()
            Connect.BackColor = Color.Green
        End If
        If Me.CasparDevice.IsConnected = True Then
            Me.CasparDevice.Disconnect()
            Connect.BackColor = Color.Red
        End If
    End Sub

    Private Sub countTimer_Tick(sender As Object, e As EventArgs) Handles countTimer.Tick
        count = count + 1
        If count >= 10 Then
            CasparDevice.SendString("stop 2-100")
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 0 linear")
            countTimer.Enabled = False
            count = 0
        End If
    End Sub

    Private Sub OnScreenClock_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OnScreenClock.Tick
        On Error Resume Next
        Dim bb = Val(Now.Second.ToString)

        Dim dif As Integer = (bb - aa)
        aa = bb
        If dif < 0 Then dif = dif + 60


        sec.Text = Format(Val(sec.Text + dif), "00")
        If sec.Text > 59 Then
            sec.Text = "00"
            min.Text = min.Text + 1

        End If
        ' to get the clock to stop at the end time set
        'Dim currTimeMins = Convert.ToDecimal(min.Text)
        'Dim endTimeMins = Convert.ToDecimal(stopClockTime.Text)

        If Val(min.Text) >= Val(stopClockTime.Text) Then
            OnScreenClock.Enabled = False
            min.Text = stopClockTime.Text
            sec.Text = "00"
        End If



    End Sub
    Private Sub StartBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' OnScreenClock.Enabled = True
        aa = Val(Now.Second.ToString) 'new code
        'disable button so cant be re-pressed
        'StartBtn.Enabled = False
    End Sub

    Private Sub ResetBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'To reset the Timer to 0

        sec.Text = "00"
        min.Text = startClockTime.Text

    End Sub
    Private Sub StopBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        OnScreenClock.Enabled = False
        If Me.CasparDevice.IsConnected = True Then

            CasparDevice.Channels(0).CG.Stop(4)

        End If
        're-enable start button 
        '  StartBtn.Enabled = True
    End Sub


    Private Sub showClock_Click(sender As Object, e As EventArgs) Handles showClock.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()
            'load clock part of template and calculate start time
            Dim startClockCalculation As Integer
            startClockCalculation = Convert.ToInt32(startClockTime.Text) * 60000
            CasparCGDataCollection.SetData("initialvalue", startClockCalculation)


            'play backing
            CasparDevice.SendString("play 1-400 EFC-CLOCK")

            'Put Data into scores part of clock
            CasparCGDataCollection.SetData("f1", homeThreeLetters.Text)
            CasparCGDataCollection.SetData("f2", HomeScore.Text)
            CasparCGDataCollection.SetData("f3", AwayScore.Text)
            CasparCGDataCollection.SetData("f4", awayThreeLetters.Text)
            'choose which clock to play
            If Convert.ToInt32(startClockTime.Text) < 45 Then
                CasparDevice.Channels(0).CG.Add(402, "count_up_timer", True, CasparCGDataCollection.ToAMCPEscapedXml)
            End If
            If Convert.ToInt32(startClockTime.Text) >= 45 Then
                CasparDevice.Channels(0).CG.Add(402, "count_up_timer_over90", True, CasparCGDataCollection.ToAMCPEscapedXml)
            End If
            CasparDevice.Channels(0).CG.Play(402)


            ' showing
            ' CasparDevice.Channels(0).CG.Add(402, "efc_clock_scoresOnly_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
            ' CasparDevice.Channels(0).CG.Play(402)


            showClock.BackColor = Color.Green
            ShowClockInGameBTN.BackColor = Color.Green
            'disbale button so cant be re-pressed
            showClock.Enabled = False
            '  startAndShowClockBTN.Enabled = False
            ShowClockInGameBTN.Enabled = False

            'start clock on interface
            aa = Val(Now.Second.ToString)
            min.Text = startClockTime.Text
            OnScreenClock.Enabled = True
        End If
    End Sub

    Private Sub HideClock_Click(sender As Object, e As EventArgs) Handles StopClock.Click
        If Me.CasparDevice.IsConnected = True Then
            'CasparDevice.Channels(0).CG.Stop(401)
            CasparDevice.Channels(0).CG.Stop(402)
            CasparDevice.SendString("MIXER 1-400 OPACITY 0 24 linear")
            count = 0
            clockAnimation.Enabled = True
            'stop preview
            'CasparDevice.Channels(1).CG.Stop(401)
            'CasparDevice.SendString("stop 2-400")
            'showClock.BackColor = Color.FromKnownColor(KnownColor.Control)
            showClock.UseVisualStyleBackColor = True

            'stopping added time
            CasparDevice.Channels(0).CG.Stop(391)
            CasparDevice.SendString("stop 1-390")
            showAddedTimeBTN.BackColor = Color.FromKnownColor(KnownColor.Control)
            showAddedTimeBTN.UseVisualStyleBackColor = True
            're-enable show button
            showClock.Enabled = True
            ' startAndShowClockBTN.Enabled = True
            ShowClockInGameBTN.Enabled = True
            'stop on screen clock
            OnScreenClock.Enabled = False
            'reset clock on interface
            min.Text = "0"
            sec.Text = "0"

        End If
    End Sub

    Private Sub clockAnimation_Tick(sender As Object, e As EventArgs) Handles clockAnimation.Tick
        count = count + 1
        If count >= 10 Then
            CasparDevice.SendString("stop 1-400")
            CasparDevice.SendString("MIXER 1-400 OPACITY 1 0 linear")
            clockAnimation.Enabled = False
            count = 0
        End If
    End Sub

    Private Sub CrawlOn_Click(sender As Object, e As EventArgs) Handles CrawlOn.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()

            If RadioButton1.Checked = True Then
                CasparCGDataCollection.SetData("f0", CrawlText1.Text)
            End If

            If RadioButton2.Checked = True Then
                CasparCGDataCollection.SetData("f0", CrawlText2.Text)
            End If

            If RadioButton3.Checked = True Then
                CasparCGDataCollection.SetData("f0", CrawlText3.Text)
            End If

            If RadioButton4.Checked = True Then
                CasparCGDataCollection.SetData("f0", CrawlText4.Text)
            End If



            CasparDevice.Channels(0).CG.Add(101, "efc_crawl_temp_LT", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(0).CG.Play(101)

            'fading in image
            CasparDevice.SendString("MIXER 1-104 OPACITY 0")
            CasparDevice.SendString("play 1-104 LT_crawl_crest")
            CasparDevice.SendString("MIXER 1-104 OPACITY 1 48 linear")

            'fading in image
            CasparDevice.SendString("MIXER 1-100 OPACITY 0")
            CasparDevice.SendString("play 1-100 LT_crawl_nocrest")
            CasparDevice.SendString("MIXER 1-100 OPACITY 1 48 linear")



            'CasparDevice.SendString("play 1-102 LT_crawl_crest")
            CasparDevice.SendString("play 1-103 LTFlare")
            CrawlOn.BackColor = Color.Green
            'disable button
            CrawlOn.Enabled = False

            crawlToggle = True
        End If
    End Sub

    Private Sub CrawlOff_Click(sender As Object, e As EventArgs) Handles CrawlOff.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(0).CG.Stop(101)
            CasparDevice.SendString("MIXER 1-100 OPACITY 0 24 linear")
            countBPS = 0
            BPlayChanFadeOut.Enabled = True
            CasparDevice.SendString("STOP 1-102")
            CasparDevice.SendString("STOP 1-103")
            CasparDevice.SendString("STOP 1-104")
            CrawlOn.BackColor = Color.FromKnownColor(KnownColor.Control)
            CrawlOn.UseVisualStyleBackColor = True
            crawlToggle = False

            're-enable button
            CrawlOn.Enabled = True
        End If
    End Sub

    Private Sub LoadTeams_Click(sender As Object, e As EventArgs) Handles LoadTeams.Click
        ' Clear list boxes in case of reload
        FullHomeSquad.Items.Clear()
        FullAwaySquad.Items.Clear()
        SubOn.Items.Clear()
        SubOff.Items.Clear()
        aw_subOn.Items.Clear()
        aw_subOff.Items.Clear()

        Try
            ' Create an instance of StreamReader to read from a file. 
            Dim sr As StreamReader = New StreamReader("C:\home_team.txt", System.Text.Encoding.Default)
            Dim line As String
            'Read and display the lines from the file until the end 
            ' of the file is reached. 
            Do

                line = sr.ReadLine()
                If line <> "" Then
                    FullHomeSquad.Items.Add(UCase(line))
                End If
                ' SubOn.Items.Add(UCase(line))
                ' SubOff.Items.Add(UCase(line))
            Loop Until line Is Nothing
            sr.Close()
        Catch ex As Exception
            ' Let the user know what went wrong.
            Console.WriteLine("The file could not be read:")
            Console.WriteLine(ex.Message)
        End Try
        HomeTeamName.Text = FullHomeSquad.Items(0).ToString
        FullHomeSquad.Items.Remove(FullHomeSquad.Items(0))

        Try
            ' Create an instance of StreamReader to read from a file. 
            Dim sr As StreamReader = New StreamReader("C:\away_team.txt", System.Text.Encoding.Default)
            Dim line As String
            'Read and display the lines from the file until the end 
            ' of the file is reached. 
            Do
                line = sr.ReadLine()
                If line <> "" Then
                    FullAwaySquad.Items.Add(UCase(line))
                End If
                'aw_subOn.Items.Add(UCase(line))
                ' aw_subOff.Items.Add(UCase(line))
            Loop Until line Is Nothing
            sr.Close()
        Catch ex As Exception
            ' Let the user know what went wrong.
            Console.WriteLine("The file could not be read:")
            Console.WriteLine(ex.Message)
        End Try
        AwayTeamName.Text = FullAwaySquad.Items(0).ToString
        FullAwaySquad.Items.Remove(FullAwaySquad.Items(0))

        'set team scores
        HomeScore.Text = "0"
        AwayScore.Text = "0"

        'set date in Lower Third Strap
        LTStrapDate.Text = Now.ToShortDateString

        'set team names for label
        'HomeTeamLabel = FullHomeSquad.Items(0)
        ' AwayTeamLabel = FullAwaySquad.Items(0)

    End Sub

    Private Sub showSub_Click(sender As Object, e As EventArgs) Handles showSub.Click
        If Me.CasparDevice.IsConnected = True Then
            If Me.SubOff.SelectedIndex >= 0 Then
                'CasparDevice.Channels(0).CG.Stop(2)
                CasparCGDataCollection.Clear() 'cgData.Clear()
                CasparCGDataCollection.SetData("f0", SubOn.Text)
                CasparCGDataCollection.SetData("f1", SubOff.Text)

                If FullScreenSubsCheck.Checked = True Then
                    CasparDevice.Channels(1).CG.Add(101, "efc_sub_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
                    CasparDevice.Channels(1).CG.Play(101)
                    CasparDevice.SendString("play 2-102 Sub_flare02")

                    'fading in image
                    CasparDevice.SendString("MIXER 2-100 OPACITY 0")
                    CasparDevice.SendString("play 2-100 efcSub")
                    CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")

                    showSub.BackColor = Color.Green
                End If

                If LowerThirdSubsCheck.Checked = True Then
                    CasparDevice.Channels(0).CG.Add(101, "efc_sub_LT", True, CasparCGDataCollection.ToAMCPEscapedXml)
                    CasparDevice.Channels(0).CG.Play(101)
                    CasparDevice.SendString("play 1-102 LTFlare")
                    'fading in image
                    CasparDevice.SendString("MIXER 1-100 OPACITY 0")
                    CasparDevice.SendString("play 1-100 LOWER_THIRD_SUB")
                    CasparDevice.SendString("MIXER 1-100 OPACITY 1 48 linear")

                    showSub.BackColor = Color.Green
                End If

                'disable button
                showSub.Enabled = False
            End If
        End If
    End Sub

    Private Sub SubOFFBtn_Click(sender As Object, e As EventArgs) Handles SubOFFBtn.Click
        If Me.CasparDevice.IsConnected = True Then
            If Me.SubOff.SelectedIndex >= 0 Then
                If FullScreenSubsCheck.Checked = True Then
                    CasparDevice.Channels(1).CG.Stop(101)
                    CasparDevice.SendString("STOP 2-102")
                    CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
                    count = 0
                    countTimer.Enabled = True

                End If
                If LowerThirdSubsCheck.Checked = True Then
                    CasparDevice.Channels(0).CG.Stop(101)
                    CasparDevice.SendString("STOP 1-102")
                    CasparDevice.SendString("MIXER 1-100 OPACITY 0 24 linear")
                    countBPS = 0
                    BPlayChanFadeOut.Enabled = True

                End If
                showSub.BackColor = Color.FromKnownColor(KnownColor.Control)
                showSub.UseVisualStyleBackColor = True
                're-enable button
                showSub.Enabled = True


                'and to switch round the subs with their first eleven player
                Dim subOnIndex = SubOn.SelectedIndex
                Dim subOffIndex = SubOff.SelectedIndex

                ' first to move sub on to Playing section of list
                SubOff.Items.Insert(subOnIndex, SubOff.SelectedItem)
                SubOff.Items.RemoveAt(SubOff.SelectedIndex)
                ' then to move sub off to subs list
                SubOff.Items.Insert(subOffIndex + 1, SubOn.SelectedItem)
                SubOff.Items.RemoveAt(SubOn.SelectedIndex + 1)
                'finaly to update all instances of this list
                ListBox1.Items.Clear()
                ListBox3.Items.Clear()
                SubOn.Items.Clear()
                'SubOff.Items.Clear()
                For i As Integer = 0 To HomeTeam.Items.Count - 1
                    ListBox1.Items.Add(SubOff.Items(i))
                    ListBox3.Items.Add(SubOff.Items(i))
                    SubOn.Items.Add(SubOff.Items(i))
                    'SubOff.Items.Add(SubOff.Items(i))
                Next

                'and to select real items
                SubOn.SelectedIndex = -1
                SubOn.Text = ""
                SubOff.SelectedIndex = -1
                SubOff.Text = ""
            End If
        End If
    End Sub

    Private Sub ShowTeamSheet_Click(sender As Object, e As EventArgs) Handles ShowTeamSheet.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()

            For i As Integer = 0 To ListBox1.Items.Count - 8
                CasparCGDataCollection.SetData("f" + (i).ToString, ListBox1.Items(i).ToString)
            Next i

            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 efcTeamSheet")
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")


            CasparDevice.Channels(1).CG.Add(101, "efc_teamsheet_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            CasparDevice.SendString("play 2-102 FIRST_11")

            ShowTeamSheet.BackColor = Color.Green
            ShowSubsSheet.UseVisualStyleBackColor = True


            'disable button so cant be pressed again
            ShowTeamSheet.Enabled = False
        End If
    End Sub

    Private Sub Ts_Off_Click(sender As Object, e As EventArgs) Handles Ts_Off.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            ShowTeamSheet.BackColor = Color.FromKnownColor(KnownColor.Control)
            ShowSubsSheet.BackColor = Color.FromKnownColor(KnownColor.Control)
            ShowTeamSheet.UseVisualStyleBackColor = True
            ShowSubsSheet.UseVisualStyleBackColor = True

            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.SendString("stop 2-102")
            'CasparDevice.SendString("stop 2-100")
            're-enable buttons
            ShowTeamSheet.Enabled = True
            ShowSubsSheet.Enabled = True
        End If
    End Sub

    Private Sub ShowSubsSheet_Click(sender As Object, e As EventArgs) Handles ShowSubsSheet.Click
        If Me.CasparDevice.IsConnected = True Then
            ' If Me.SubOff.SelectedIndex >= 0 Then
            'CasparDevice.Channels(0).CG.Stop(2)
            CasparCGDataCollection.Clear() 'cgData.Clear()

            For i As Integer = 11 To ListBox1.Items.Count - 1
                CasparCGDataCollection.SetData("f" + (i - 11).ToString, ListBox1.Items(i).ToString)
            Next i
            CasparDevice.Channels(1).CG.Add(101, "efc_subsheet_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            CasparDevice.SendString("play 2-102 SUBS")
            'fade in
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 efcTeamSubs")
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
            ShowSubsSheet.BackColor = Color.Green
            ShowTeamSheet.UseVisualStyleBackColor = True
            'disable button so cant be pressed again
            ShowSubsSheet.Enabled = False
        End If
    End Sub


    Private Sub backgroundOn_CheckedChanged(sender As Object, e As EventArgs) Handles backgroundOn.CheckedChanged
        If backgroundOn.CheckState = CheckState.Checked Then
            If Me.CasparDevice.IsConnected = True Then
                CasparDevice.SendString("play 2-50 Everton_LightsBG loop auto")
            End If
        End If
        If backgroundOn.CheckState = CheckState.Unchecked Then
            If Me.CasparDevice.IsConnected = True Then
                CasparDevice.SendString("stop 2-50")
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles showPremScores.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()
            CasparCGDataCollection.SetData("TITLE", PremScoresTitle.Text)
            CasparCGDataCollection.SetData("f0", Score1.Text)
            CasparCGDataCollection.SetData("f1", Score2.Text)
            CasparCGDataCollection.SetData("f2", Score3.Text)
            CasparCGDataCollection.SetData("f3", Score4.Text)
            CasparCGDataCollection.SetData("f4", Score5.Text)
            CasparCGDataCollection.SetData("f5", Score6.Text)
            CasparCGDataCollection.SetData("f6", Score7.Text)
            CasparCGDataCollection.SetData("f7", Score8.Text)
            CasparCGDataCollection.SetData("f8", Score9.Text)
            CasparCGDataCollection.SetData("f9", Score10.Text)
            CasparCGDataCollection.SetData("f10", Score11.Text)
            CasparCGDataCollection.SetData("f11", Score12.Text)
            CasparCGDataCollection.SetData("f12", Score13.Text)
            CasparCGDataCollection.SetData("f13", Score14.Text)
            CasparCGDataCollection.SetData("f14", Score15.Text)
            CasparCGDataCollection.SetData("f15", Score16.Text)
            CasparCGDataCollection.SetData("f16", Score17.Text)
            CasparCGDataCollection.SetData("f17", Score18.Text)
            CasparCGDataCollection.SetData("f18", Score19.Text)
            CasparCGDataCollection.SetData("f19", Score20.Text)
            CasparCGDataCollection.SetData("f20", Score21.Text)
            CasparCGDataCollection.SetData("f21", Score22.Text)
            CasparCGDataCollection.SetData("f22", Score23.Text)
            CasparCGDataCollection.SetData("f23", Score24.Text)

            CasparCGDataCollection.SetData("m1", middle13.Text)
            CasparCGDataCollection.SetData("m2", middle14.Text)
            CasparCGDataCollection.SetData("m3", middle15.Text)
            CasparCGDataCollection.SetData("m4", middle16.Text)
            CasparCGDataCollection.SetData("m5", middle17.Text)
            CasparCGDataCollection.SetData("m6", middle18.Text)


            ' altering colour
            If FT1.Checked = True Then
                CasparCGDataCollection.SetData("ft1", "0xff0000")
            End If
            If FT2.Checked = True Then
                CasparCGDataCollection.SetData("ft2", "0xff0000")
            End If
            If FT3.Checked = True Then
                CasparCGDataCollection.SetData("ft3", "0xff0000")
            End If
            If FT4.Checked = True Then
                CasparCGDataCollection.SetData("ft4", "0xff0000")
            End If
            If FT5.Checked = True Then
                CasparCGDataCollection.SetData("ft5", "0xff0000")
            End If
            If FT6.Checked = True Then
                CasparCGDataCollection.SetData("ft6", "0xff0000")
            End If





            'showing layers of bars
            If CheckBox13.Checked = True Then
                CasparDevice.SendString("MIXER 2-104 OPACITY 0")
                CasparDevice.SendString("play 2-104 SCORESBAR_1")
                CasparDevice.SendString("MIXER 2-104 OPACITY 1 48 linear")
            End If
            If CheckBox14.Checked = True Then
                CasparDevice.SendString("MIXER 2-105 OPACITY 0")
                CasparDevice.SendString("play 2-105 SCORESBAR_2")
                CasparDevice.SendString("MIXER 2-105 OPACITY 1 48 linear")
            End If

            If CheckBox15.Checked = True Then
                CasparDevice.SendString("MIXER 2-106 OPACITY 0")
                CasparDevice.SendString("play 2-106 SCORESBAR_3")
                CasparDevice.SendString("MIXER 2-106 OPACITY 1 48 linear")
            End If

            If CheckBox16.Checked = True Then
                CasparDevice.SendString("MIXER 2-107 OPACITY 0")
                CasparDevice.SendString("play 2-107 SCORESBAR_4")
                CasparDevice.SendString("MIXER 2-107 OPACITY 1 48 linear")
            End If

            If CheckBox17.Checked = True Then
                CasparDevice.SendString("MIXER 2-108 OPACITY 0")
                CasparDevice.SendString("play 2-108 SCORESBAR_5")
                CasparDevice.SendString("MIXER 2-108 OPACITY 1 48 linear")
            End If

            If CheckBox18.Checked = True Then
                CasparDevice.SendString("MIXER 2-109 OPACITY 0")
                CasparDevice.SendString("play 2-109 SCORESBAR_6")
                CasparDevice.SendString("MIXER 2-109 OPACITY 1 48 linear")
            End If


            CasparDevice.Channels(1).CG.Add(101, "efc_premscores", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            ' CasparDevice.SendString("play 2-102 LATESTSCORES")

            ' select which logo to show
            If tab1Logo1Select.Text = "Premier League" Then
                CasparDevice.SendString("play 2-100 SCORES_BPL_LOGO")
            End If
            If tab1Logo1Select.Text = "Championship" Then
                CasparDevice.SendString("play 2-100 SCORES_CHAMPIONSHIP_LOGO")
            End If
            If tab1Logo1Select.Text = "Capital One Cup" Then
                CasparDevice.SendString("play 2-100 SCORES_C1C_LOGO")
            End If
            If tab1Logo1Select.Text = "Europa League" Then
                CasparDevice.SendString("play 2-100 SCORES_EUROPALEAGUE_LOGO")
            End If
            If tab1Logo1Select.Text = "FA Cup" Then
                CasparDevice.SendString("play 2-100 SCORES_FACUP_LOGO")
            End If
            If tab1Logo1Select.Text = "Champions League" Then
                CasparDevice.SendString("play 2-100 SCORES_CHAMPLEAGUE_LOGO")
            End If


            'set colours on buttons
            showPremScores.BackColor = Color.Green
            TXPremScores_2BTN.UseVisualStyleBackColor = True

            'disable button
            showPremScores.Enabled = False

        End If
    End Sub

    Private Sub HidePremScores_Click(sender As Object, e As EventArgs) Handles HidePremScores.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-104 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-105 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-106 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-107 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-108 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-109 OPACITY 0 24 linear")
            countScores = 0
            scoresFadeOut.Enabled = True
            CasparDevice.SendString("STOP 2-110")
            showPremScores.BackColor = Color.FromKnownColor(KnownColor.Control)
            showPremScores.UseVisualStyleBackColor = True
            showPremScores.UseVisualStyleBackColor = True
            TXPremScores_2BTN.UseVisualStyleBackColor = True
            'disable button
            showPremScores.Enabled = True

        End If
    End Sub

    Private Sub saveData_Click(sender As Object, e As EventArgs)
        'first let's check if there is a file MyXML.xml into our application folder
        'if there wasn't a file something like that, then let's create a new one.

        'If IO.File.Exists("MyXML.xml") = False Then

        'declare our xmlwritersettings object
        Dim settings As New XmlWriterSettings()

        'lets tell to our xmlwritersettings that it must use indention for our xml
        settings.Indent = True

        'lets create the MyXML.xml document, the first parameter was the Path/filename of xml file
        ' the second parameter was our xml settings
        Dim XmlWrt As XmlWriter = XmlWriter.Create("MyXML.xml", settings)

        With XmlWrt

            ' Write the Xml declaration.
            .WriteStartDocument()

            ' Write a comment.
            .WriteComment("XML Database.")

            ' Write the root element.
            .WriteStartElement("PremiershipScores")

            ' Start our first person.
            .WriteStartElement("Row1")

            ' The person nodes.

            .WriteStartElement("HomeTeam1")
            .WriteString(Score1.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore1")
            .WriteString(Score2.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam1")
            .WriteString(Score4.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore1")
            .WriteString(Score3.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row2")

            ' The person nodes.

            .WriteStartElement("HomeTeam2")
            .WriteString(Score5.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore2")
            .WriteString(Score6.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam2")
            .WriteString(Score8.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore2")
            .WriteString(Score7.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            .WriteStartElement("Row3")

            ' The person nodes.

            .WriteStartElement("HomeTeam3")
            .WriteString(Score9.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore3")
            .WriteString(Score10.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam3")
            .WriteString(Score12.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore3")
            .WriteString(Score11.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row4")

            ' The person nodes.

            .WriteStartElement("HomeTeam4")
            .WriteString(Score13.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore4")
            .WriteString(Score14.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam4")
            .WriteString(Score16.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore4")
            .WriteString(Score15.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row5")

            ' The person nodes.

            .WriteStartElement("HomeTeam5")
            .WriteString(Score17.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore5")
            .WriteString(Score18.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam5")
            .WriteString(Score20.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore5")
            .WriteString(Score19.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            .WriteStartElement("Row6")

            ' The person nodes.

            .WriteStartElement("HomeTeam6")
            .WriteString(Score21.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore6")
            .WriteString(Score22.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam6")
            .WriteString(Score24.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore6")
            .WriteString(Score23.Text)
            .WriteEndElement()


            ' SET TWO 
            .WriteStartElement("Row7")

            ' The person nodes.

            .WriteStartElement("HomeTeam7")
            .WriteString(Score25.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore7")
            .WriteString(Score26.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam7")
            .WriteString(Score28.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore7")
            .WriteString(Score27.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row8")

            ' The person nodes.

            .WriteStartElement("HomeTeam8")
            .WriteString(Score29.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore8")
            .WriteString(Score30.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam8")
            .WriteString(Score32.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore8")
            .WriteString(Score31.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            .WriteStartElement("Row9")

            ' The person nodes.

            .WriteStartElement("HomeTeam9")
            .WriteString(Score33.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore9")
            .WriteString(Score34.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam9")
            .WriteString(Score36.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore9")
            .WriteString(Score35.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row10")

            ' The person nodes.

            .WriteStartElement("HomeTeam10")
            .WriteString(Score37.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore10")
            .WriteString(Score38.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam10")
            .WriteString(Score40.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore10")
            .WriteString(Score39.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row11")

            ' The person nodes.

            .WriteStartElement("HomeTeam11")
            .WriteString(Score41.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore11")
            .WriteString(Score42.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam11")
            .WriteString(Score44.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore11")
            .WriteString(Score43.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            .WriteStartElement("Row12")

            ' The person nodes.

            .WriteStartElement("HomeTeam12")
            .WriteString(Score45.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore12")
            .WriteString(Score46.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam12")
            .WriteString(Score48.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore12")
            .WriteString(Score47.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            ' Close the XmlTextWriter.
            .WriteEndDocument()
            .Close()

        End With

        MessageBox.Show("XML file saved.")
        ' End If
    End Sub

    Private Sub loadData_Click(sender As Object, e As EventArgs)
        'check if file myxml.xml is existing
        If (IO.File.Exists("MyXML.xml")) Then

            'create a new xmltextreader object
            'this is the object that we will loop and will be used to read the xml file
            Dim document As XmlReader = New XmlTextReader("MyXML.xml")

            'loop through the xml file
            While (document.Read())

                Dim type = document.NodeType

                'if node type was element
                If (type = XmlNodeType.Element) Then
                    If (document.Name = "HomeTeam1") Then
                        Score1.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore1") Then
                        Score2.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam1") Then
                        Score4.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore1") Then
                        Score3.Text = document.ReadInnerXml.ToString()
                    End If

                    'row 2
                    If (document.Name = "HomeTeam2") Then
                        Score5.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore2") Then
                        Score6.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam2") Then
                        Score8.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore2") Then
                        Score7.Text = document.ReadInnerXml.ToString()
                    End If

                    'row3
                    If (document.Name = "HomeTeam3") Then
                        Score9.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore3") Then
                        Score10.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam3") Then
                        Score12.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore3") Then
                        Score11.Text = document.ReadInnerXml.ToString()
                    End If

                    'row4
                    If (document.Name = "HomeTeam4") Then
                        Score13.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore4") Then
                        Score14.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam4") Then
                        Score16.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore4") Then
                        Score15.Text = document.ReadInnerXml.ToString()
                    End If

                    'row5
                    If (document.Name = "HomeTeam5") Then
                        Score17.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore5") Then
                        Score18.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam5") Then
                        Score20.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore5") Then
                        Score19.Text = document.ReadInnerXml.ToString()
                    End If

                    'row6
                    If (document.Name = "HomeTeam6") Then
                        Score21.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore6") Then
                        Score22.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam6") Then
                        Score24.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore6") Then
                        Score23.Text = document.ReadInnerXml.ToString()
                    End If

                    'second set
                    'row 7
                    If (document.Name = "HomeTeam7") Then
                        Score25.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore7") Then
                        Score26.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam7") Then
                        Score28.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore7") Then
                        Score27.Text = document.ReadInnerXml.ToString()
                    End If

                    'row 2
                    If (document.Name = "HomeTeam8") Then
                        Score29.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore8") Then
                        Score30.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam8") Then
                        Score32.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore8") Then
                        Score31.Text = document.ReadInnerXml.ToString()
                    End If

                    'row3
                    If (document.Name = "HomeTeam9") Then
                        Score33.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore9") Then
                        Score34.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam9") Then
                        Score36.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore9") Then
                        Score35.Text = document.ReadInnerXml.ToString()
                    End If

                    'row4
                    If (document.Name = "HomeTeam10") Then
                        Score37.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore10") Then
                        Score38.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam10") Then
                        Score40.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore10") Then
                        Score39.Text = document.ReadInnerXml.ToString()
                    End If

                    'row5
                    If (document.Name = "HomeTeam11") Then
                        Score41.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore11") Then
                        Score42.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam11") Then
                        Score44.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore11") Then
                        Score43.Text = document.ReadInnerXml.ToString()
                    End If

                    'row6
                    If (document.Name = "HomeTeam12") Then
                        Score45.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore12") Then
                        Score46.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam12") Then
                        Score48.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore12") Then
                        Score47.Text = document.ReadInnerXml.ToString()
                    End If

                End If

            End While
            document.Close()
        Else

            MessageBox.Show("The filename you selected was not found.")
        End If
    End Sub

    Private Sub rereshVids_Click(sender As Object, e As EventArgs) Handles rereshVids.Click

        Dim File As Svt.Caspar.MediaInfo
        CasparDevice.RefreshMediafiles()
        'Clear list box in case of reload
        SourceFiles.Items.Clear()
        Threading.Thread.Sleep(250)

        For Each File In CasparDevice.Mediafiles
            SourceFiles.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
        Next

    End Sub

    Private Sub RemovePlaylist_Click(sender As Object, e As EventArgs) Handles RemovePlaylist.Click
        playlistFiles.Items.Remove(playlistFiles.SelectedItem)
    End Sub

    Private Sub ClearPlaylist_Click(sender As Object, e As EventArgs) Handles ClearPlaylist.Click
        playlistFiles.Items.Clear()
    End Sub

    Private Sub AddPlaylist_Click(sender As Object, e As EventArgs) Handles AddPlaylist.Click
        playlistFiles.Items.Add(SourceFiles.Text)
    End Sub

    Private Sub playVid_Click(sender As Object, e As EventArgs) Handles playVid.Click
        If Me.CasparDevice.IsConnected = True Then
            If Me.playlistFiles.SelectedIndex >= 0 Then
                ' If PreMatchPlayNext = False Then
                ''fading in image
                'CasparDevice.SendString("MIXER 2-99 OPACITY 0")
                'CasparDevice.SendString("play 2-99 " & playlistFiles.Text)
                'CasparDevice.SendString("MIXER 2-99 OPACITY 1 48 linear")
                ''fade out other layer
                'CasparDevice.SendString("MIXER 2-100 OPACITY 0 48 linear")
                'PreMatchPlayNext = True
                'End If
                'If PreMatchPlayNext = True Then
                ''fading in image
                'CasparDevice.SendString("MIXER 2-100 OPACITY 0")
                'CasparDevice.SendString("play 2-100 " & playlistFiles.Text)
                'CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
                ''fade out other layer
                'CasparDevice.SendString("MIXER 2-99 OPACITY 0 48 linear")
                ''reset for next if
                'PreMatchPlayNext = False
                'End If



                'select transition and play file
                If MixTrans.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " MIX 12 LINEAR")
                End If
                If WipeTrans.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " SLIDE 20 LEFT")
                End If
                If PushTrans.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " PUSH 20 EASEINSINE")
                End If
                ' CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " MIX 12 LINEAR")
                'CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " PUSH 20 EASEINSINE")
                'CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " SLIDE 20 LEFT")
                playVid.BackColor = Color.Green
                ' LoopVid.BackColor = Color.FromKnownColor(KnownColor.Control)
                'LoopVid.UseVisualStyleBackColor = True
                playNext.BackColor = Color.FromKnownColor(KnownColor.Control)
                playNext.UseVisualStyleBackColor = True
            End If
        End If
    End Sub

    Private Sub LoopVid_Click(sender As Object, e As EventArgs)
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " loop auto")
            'LoopVid.BackColor = Color.Green
            playVid.BackColor = Color.FromKnownColor(KnownColor.Control)
            playVid.UseVisualStyleBackColor = True
            playNext.BackColor = Color.FromKnownColor(KnownColor.Control)
            playNext.UseVisualStyleBackColor = True
        End If
    End Sub

    Private Sub stopVid_Click(sender As Object, e As EventArgs) Handles stopVid.Click
        If Me.CasparDevice.IsConnected = True Then
            ' fade out opacity and start timer to fade channel back in 
            CasparDevice.SendString("MIXER 2-99 OPACITY 0 12 linear")
            playlistStop.Enabled = True
            'set button colours back
            playVid.BackColor = Color.FromKnownColor(KnownColor.Control)
            playVid.UseVisualStyleBackColor = True
            playNext.BackColor = Color.FromKnownColor(KnownColor.Control)
            playNext.UseVisualStyleBackColor = True
        End If
    End Sub

    Private Sub ShowAwayFirstEleven_Click(sender As Object, e As EventArgs) Handles ShowAwayFirstEleven.Click
        If Me.CasparDevice.IsConnected = True Then
            ' If Me.SubOff.SelectedIndex >= 0 Then
            'CasparDevice.Channels(0).CG.Stop(2)
            CasparCGDataCollection.Clear() 'cgData.Clear()
            For i As Integer = 0 To ListBox2.Items.Count - 8
                CasparCGDataCollection.SetData("f" + (i).ToString, ListBox2.Items(i).ToString)
            Next i
            CasparDevice.Channels(1).CG.Add(101, "efc_teamsheet_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            CasparDevice.SendString("play 2-102 FIRST_11")
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 awayEFCTeamSheet")
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")

            ShowAwayFirstEleven.BackColor = Color.Green
            ShowAwaySubsSheet.UseVisualStyleBackColor = True
            ShowSubsSheet.UseVisualStyleBackColor = True
            ' disable button so cant be pressed again
            ShowAwayFirstEleven.Enabled = False
        End If
    End Sub

    Private Sub ShowAwaySubsSheet_Click(sender As Object, e As EventArgs) Handles ShowAwaySubsSheet.Click
        If Me.CasparDevice.IsConnected = True Then
            ' If Me.SubOff.SelectedIndex >= 0 Then
            'CasparDevice.Channels(0).CG.Stop(2)
            CasparCGDataCollection.Clear() 'cgData.Clear()
            For i As Integer = 11 To ListBox2.Items.Count - 1
                CasparCGDataCollection.SetData("f" + (i - 11).ToString, ListBox2.Items(i).ToString)
            Next i
            CasparDevice.Channels(1).CG.Add(101, "efc_subsheet_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            CasparDevice.SendString("play 2-102 SUBS")
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 awayTeamSubs")
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")

            ShowAwaySubsSheet.BackColor = Color.Green
            ShowAwayFirstEleven.UseVisualStyleBackColor = True
            ' disable button so cant be pressed again
            ShowAwaySubsSheet.Enabled = False
        End If
    End Sub

    Private Sub AwayTeamsOff_Click(sender As Object, e As EventArgs) Handles AwayTeamsOff.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            CasparDevice.SendString("stop 2-102")
            ShowAwayFirstEleven.BackColor = Color.FromKnownColor(KnownColor.Control)
            ShowAwaySubsSheet.BackColor = Color.FromKnownColor(KnownColor.Control)
            ShowAwayFirstEleven.UseVisualStyleBackColor = True
            ShowAwaySubsSheet.UseVisualStyleBackColor = True
            're-enable buttons
            ShowAwayFirstEleven.Enabled = True
            ShowAwaySubsSheet.Enabled = True
            ShowTeamSheet.Enabled = True
            ShowSubsSheet.Enabled = True
        End If
    End Sub

    Private Sub AwaySubOn_Click(sender As Object, e As EventArgs) Handles AwaySubOn.Click
        If Me.CasparDevice.IsConnected = True Then
            If Me.aw_subOff.SelectedIndex >= 0 Then
                'CasparDevice.Channels(0).CG.Stop(2)
                CasparCGDataCollection.Clear() 'cgData.Clear()
                CasparCGDataCollection.SetData("f0", aw_subOn.Text)
                CasparCGDataCollection.SetData("f1", aw_subOff.Text)
                If FullScreenSubsCheck.Checked = True Then
                    CasparDevice.Channels(1).CG.Add(101, "efc_sub_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
                    CasparDevice.Channels(1).CG.Play(101)
                    CasparDevice.SendString("play 2-102 Sub_flare02")
                    'fading in image
                    CasparDevice.SendString("MIXER 2-100 OPACITY 0")
                    CasparDevice.SendString("play 2-100 awayEFCSub")
                    CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")

                    'showSub.BackColor = Color.Green
                End If

                If LowerThirdSubsCheck.Checked = True Then
                    CasparDevice.Channels(0).CG.Add(101, "efc_sub_LT", True, CasparCGDataCollection.ToAMCPEscapedXml)
                    CasparDevice.Channels(0).CG.Play(101)
                    CasparDevice.SendString("play 1-102 LTFlare")
                    'fading in image
                    CasparDevice.SendString("MIXER 1-100 OPACITY 0")
                    CasparDevice.SendString("play 1-100 LOWER_THIRD_SUB_AWAY")
                    CasparDevice.SendString("MIXER 1-100 OPACITY 1 48 linear")

                    ' showSub.BackColor = Color.Green
                End If
                AwaySubOn.BackColor = Color.Green
                'disable button
                AwaySubOn.Enabled = False
            End If
        End If
    End Sub

    Private Sub AwaySubOff_Click(sender As Object, e As EventArgs) Handles AwaySubOff.Click
        If Me.CasparDevice.IsConnected = True Then
            If Me.aw_subOff.SelectedIndex >= 0 Then
                If FullScreenSubsCheck.Checked = True Then
                    CasparDevice.Channels(1).CG.Stop(101)
                    CasparDevice.SendString("STOP 2-102")
                    CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
                    count = 0
                    countTimer.Enabled = True

                End If
                If LowerThirdSubsCheck.Checked = True Then
                    CasparDevice.Channels(0).CG.Stop(101)
                    CasparDevice.SendString("STOP 1-102")
                    CasparDevice.SendString("MIXER 1-100 OPACITY 0 24 linear")
                    countBPS = 0
                    BPlayChanFadeOut.Enabled = True

                End If
                AwaySubOn.BackColor = Color.FromKnownColor(KnownColor.Control)
                AwaySubOn.UseVisualStyleBackColor = True
                're-enable button
                AwaySubOn.Enabled = True

                'and to switch round the subs with their first eleven player
                Dim aw_subOnIndex = aw_subOn.SelectedIndex
                Dim aw_subOffIndex = aw_subOff.SelectedIndex

                ' first to move sub on to Playing section of list
                aw_subOff.Items.Insert(aw_subOnIndex, aw_subOff.SelectedItem)
                aw_subOff.Items.RemoveAt(aw_subOff.SelectedIndex)
                ' then to move sub off to subs list
                aw_subOff.Items.Insert(aw_subOffIndex + 1, aw_subOn.SelectedItem)
                aw_subOff.Items.RemoveAt(aw_subOn.SelectedIndex + 1)
                'finaly to update all instances of this list
                ListBox2.Items.Clear()
                ListBox4.Items.Clear()
                aw_subOn.Items.Clear()
                'SubOff.Items.Clear()
                For i As Integer = 0 To HomeTeam.Items.Count - 1
                    ListBox2.Items.Add(aw_subOff.Items(i))
                    ListBox4.Items.Add(aw_subOff.Items(i))
                    aw_subOn.Items.Add(aw_subOff.Items(i))
                    'aw_subOff.Items.Add(aw_subOff.Items(i))
                Next
                'and to select real items
                aw_subOn.SelectedIndex = -1
                aw_subOn.Text = ""
                aw_subOff.SelectedIndex = -1
                aw_subOff.Text = ""
            End If
        End If
    End Sub

    Private Sub playNext_Click(sender As Object, e As EventArgs) Handles playNext.Click
        If Me.playlistFiles.SelectedIndex >= 0 Then
            playlistPosition += 1
            If (playlistFiles.SelectedIndex < (playlistFiles.Items.Count() - 1)) Then
                playlistFiles.SelectedIndex += 1
            End If
            If (playlistPosition > playlistFiles.SelectedIndex) Then
                playlistFiles.SelectedIndex = 0
                playlistPosition = 0
            End If
            ' something is wrong here, it should loop but i think the previous statement is stopping it getting there.
            If Me.CasparDevice.IsConnected = True Then
                ' If PreMatchPlayNext = False Then

                ' 'fading in image
                ' CasparDevice.SendString("MIXER 2-99 OPACITY 0")
                ' CasparDevice.SendString("play 2-99 " & playlistFiles.Text)
                'CasparDevice.SendString("MIXER 2-99 OPACITY 1 48 linear")
                ''fade out other layer
                'CasparDevice.SendString("MIXER 2-100 OPACITY 0 48 linear")

                ' playNext.BackColor = Color.Green
                'playVid.BackColor = Color.FromKnownColor(KnownColor.Control)
                'playVid.UseVisualStyleBackColor = True
                ' LoopVid.BackColor = Color.FromKnownColor(KnownColor.Control)
                ' LoopVid.UseVisualStyleBackColor = True

                'reset for next if
                'PreMatchPlayNext = True
                'End If

                '           If PreMatchPlayNext = True Then
                ''fading in image
                'CasparDevice.SendString("MIXER 2-100 OPACITY 0")
                'CasparDevice.SendString("play 2-100 " & playlistFiles.Text)
                'CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
                'fade out other layer
                'CasparDevice.SendString("MIXER 2-99 OPACITY 0 48 linear")


                'select transition and play file
                If MixTrans.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " MIX 12 LINEAR")
                End If
                If WipeTrans.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " SLIDE 20 LEFT")
                End If
                If PushTrans.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFiles.Text & " PUSH 20 EASEINSINE")
                End If
                playNext.BackColor = Color.Green
                playVid.BackColor = Color.FromKnownColor(KnownColor.Control)
                playVid.UseVisualStyleBackColor = True
                ' LoopVid.BackColor = Color.FromKnownColor(KnownColor.Control)
                ' LoopVid.UseVisualStyleBackColor = True

                'reset for next if
                PreMatchPlayNext = False
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles UpdateTextButton.Click
        If Me.CasparDevice.IsConnected = True Then
            If crawlToggle = True Then
                CasparCGDataCollection.Clear()

                If RadioButton1.Checked = True Then
                    CasparCGDataCollection.SetData("f0", CrawlText1.Text)
                End If

                If RadioButton2.Checked = True Then
                    CasparCGDataCollection.SetData("f0", CrawlText2.Text)
                End If

                If RadioButton3.Checked = True Then
                    CasparCGDataCollection.SetData("f0", CrawlText3.Text)
                End If

                If RadioButton4.Checked = True Then
                    CasparCGDataCollection.SetData("f0", CrawlText4.Text)
                End If



                CasparDevice.Channels(0).CG.Add(101, "efc_crawl_temp_LT", True, CasparCGDataCollection.ToAMCPEscapedXml)
                CasparDevice.Channels(0).CG.Play(101)
                ' CasparDevice.SendString("play 1-100 efcAddedTime")
                ' CrawlOn.BackColor = Color.Green
            End If
        End If
    End Sub



    Private Sub showBigScore_Click(sender As Object, e As EventArgs) Handles showBigScore.Click
        If Me.HomeScorers.Items.Count <= 5 And Me.awayScorers.Items.Count <= 5 Then
            If Me.CasparDevice.IsConnected = True Then
                CasparCGDataCollection.Clear()
                CasparCGDataCollection.SetData("f0", HomeScore.Text)
                CasparCGDataCollection.SetData("f1", AwayScore.Text)

                If HomeScorers.Items.Count = 1 Then
                    CasparCGDataCollection.SetData("f2", HomeScorers.Items(0).ToString)
                End If
                If HomeScorers.Items.Count = 2 Then
                    CasparCGDataCollection.SetData("f2", HomeScorers.Items(0).ToString)
                    CasparCGDataCollection.SetData("f3", HomeScorers.Items(1).ToString)
                End If
                If HomeScorers.Items.Count = 3 Then
                    CasparCGDataCollection.SetData("f2", HomeScorers.Items(0).ToString)
                    CasparCGDataCollection.SetData("f3", HomeScorers.Items(1).ToString)
                    CasparCGDataCollection.SetData("f4", HomeScorers.Items(2).ToString)
                End If
                If HomeScorers.Items.Count = 4 Then
                    CasparCGDataCollection.SetData("f2", HomeScorers.Items(0).ToString)
                    CasparCGDataCollection.SetData("f3", HomeScorers.Items(1).ToString)
                    CasparCGDataCollection.SetData("f4", HomeScorers.Items(2).ToString)
                    CasparCGDataCollection.SetData("f5", HomeScorers.Items(3).ToString)
                End If
                If HomeScorers.Items.Count = 5 Then
                    CasparCGDataCollection.SetData("f2", HomeScorers.Items(0).ToString)
                    CasparCGDataCollection.SetData("f3", HomeScorers.Items(1).ToString)
                    CasparCGDataCollection.SetData("f4", HomeScorers.Items(2).ToString)
                    CasparCGDataCollection.SetData("f5", HomeScorers.Items(3).ToString)
                    CasparCGDataCollection.SetData("f6", HomeScorers.Items(4).ToString)
                End If


                If awayScorers.Items.Count = 1 Then
                    CasparCGDataCollection.SetData("f7", awayScorers.Items(0).ToString)
                End If
                If awayScorers.Items.Count = 2 Then
                    CasparCGDataCollection.SetData("f7", awayScorers.Items(0).ToString)
                    CasparCGDataCollection.SetData("f8", awayScorers.Items(1).ToString)
                End If
                If awayScorers.Items.Count = 3 Then
                    CasparCGDataCollection.SetData("f7", awayScorers.Items(0).ToString)
                    CasparCGDataCollection.SetData("f8", awayScorers.Items(1).ToString)
                    CasparCGDataCollection.SetData("f9", awayScorers.Items(2).ToString)
                End If
                If awayScorers.Items.Count = 4 Then
                    CasparCGDataCollection.SetData("f7", awayScorers.Items(0).ToString)
                    CasparCGDataCollection.SetData("f8", awayScorers.Items(1).ToString)
                    CasparCGDataCollection.SetData("f9", awayScorers.Items(2).ToString)
                    CasparCGDataCollection.SetData("f10", awayScorers.Items(3).ToString)
                End If
                If awayScorers.Items.Count = 5 Then
                    CasparCGDataCollection.SetData("f7", awayScorers.Items(0).ToString)
                    CasparCGDataCollection.SetData("f8", awayScorers.Items(1).ToString)
                    CasparCGDataCollection.SetData("f9", awayScorers.Items(2).ToString)
                    CasparCGDataCollection.SetData("f10", awayScorers.Items(3).ToString)
                    CasparCGDataCollection.SetData("f11", awayScorers.Items(4).ToString)
                End If



                ' just need to work on only showing data if its there

                'CasparCGDataCollection.SetData("f3", HomeScorers.Items(1).ToString)
                'CasparCGDataCollection.SetData("f4", HomeScorers.Items(2).ToString)
                'CasparCGDataCollection.SetData("f5", HomeScorers.Items(3).ToString)
                'CasparCGDataCollection.SetData("f6", HomeScorers.Items(4).ToString)
                'CasparCGDataCollection.SetData("f7", awayScorers.Items(0).ToString)
                'CasparCGDataCollection.SetData("f8", awayScorers.Items(1).ToString)
                'CasparCGDataCollection.SetData("f9", awayScorers.Items(2).ToString)
                'CasparCGDataCollection.SetData("f10", awayScorers.Items(3).ToString)
                'CasparCGDataCollection.SetData("f11", awayScorers.Items(4).ToString)

                CasparDevice.Channels(1).CG.Add(101, "efc_bigScore", True, CasparCGDataCollection.ToAMCPEscapedXml)
                CasparDevice.Channels(1).CG.Play(101)
                'fading in image
                CasparDevice.SendString("MIXER 2-100 OPACITY 0")
                CasparDevice.SendString("play 2-100 efc_bigScoreBack")
                CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")

                '"play 1-1 " & ListBox3.Text & " loop auto"
                showBigScore.BackColor = Color.Green
                'disabale button
                showBigScore.Enabled = False

            End If
        Else
            MessageBox.Show("Unfortunately this graphic only works if both teams have scored less than five goals.", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub hideBigScore_Click(sender As Object, e As EventArgs) Handles hideBigScore.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            showBigScore.BackColor = Color.FromKnownColor(KnownColor.Control)
            showBigScore.UseVisualStyleBackColor = True
            'reenable button
            showBigScore.Enabled = True
        End If
    End Sub

   
    Private Sub startAndShowClockBTN_Click(sender As Object, e As EventArgs)
        '  OnScreenClock.Enabled = True
        aa = Val(Now.Second.ToString) 'new code
        CasparCGDataCollection.Clear()
        CasparCGDataCollection.SetData("initialvalue", "180000")
        CasparDevice.Channels(0).CG.Add(401, "count_up_timer", True, CasparCGDataCollection.ToAMCPEscapedXml)
        CasparDevice.Channels(0).CG.Play(401)
        '  CasparCGDataCollection.SetData("f0", min.Text & ":" & sec.Text)
        'note - the following are only for Jamies graphics
        '  CasparCGDataCollection.SetData("f1", homeThreeLetters.Text)
        '  CasparCGDataCollection.SetData("f2", HomeScore.Text)
        '  CasparCGDataCollection.SetData("f3", AwayScore.Text)
        '  CasparCGDataCollection.SetData("f4", awayThreeLetters.Text)
        ' showing
        '   CasparDevice.Channels(0).CG.Add(401, "efc_clock_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
        '   CasparDevice.Channels(0).CG.Play(401)
        '    CasparDevice.SendString("play 1-400 EFC-CLOCK")
        ' prewviewin
        'CasparDevice.Channels(1).CG.Add(401, "efc_clock_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
        'CasparDevice.Channels(1).CG.Play(401)
        ' CasparDevice.SendString("play 2-400 EFC-CLOCK")
        showClock.BackColor = Color.Green
        'Timer3.Enabled = True
        'Timer2.Enabled = True

        'disbale button so cant be re-pressed
        showClock.Enabled = False
        ' startAndShowClockBTN.Enabled = False
        ShowClockInGameBTN.Enabled = False
    End Sub

    Private Sub TXPremScores_2BTN_Click(sender As Object, e As EventArgs) Handles TXPremScores_2BTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()
            CasparCGDataCollection.SetData("TITLE", PremScoresTitle2.Text)
            CasparCGDataCollection.SetData("f0", Score25.Text)
            CasparCGDataCollection.SetData("f1", Score26.Text)
            CasparCGDataCollection.SetData("f2", Score27.Text)
            CasparCGDataCollection.SetData("f3", Score28.Text)
            CasparCGDataCollection.SetData("f4", Score29.Text)
            CasparCGDataCollection.SetData("f5", Score30.Text)
            CasparCGDataCollection.SetData("f6", Score31.Text)
            CasparCGDataCollection.SetData("f7", Score32.Text)
            CasparCGDataCollection.SetData("f8", Score33.Text)
            CasparCGDataCollection.SetData("f9", Score34.Text)
            CasparCGDataCollection.SetData("f10", Score35.Text)
            CasparCGDataCollection.SetData("f11", Score36.Text)
            CasparCGDataCollection.SetData("f12", Score37.Text)
            CasparCGDataCollection.SetData("f13", Score38.Text)
            CasparCGDataCollection.SetData("f14", Score39.Text)
            CasparCGDataCollection.SetData("f15", Score40.Text)
            CasparCGDataCollection.SetData("f16", Score41.Text)
            CasparCGDataCollection.SetData("f17", Score42.Text)
            CasparCGDataCollection.SetData("f18", Score43.Text)
            CasparCGDataCollection.SetData("f19", Score44.Text)
            CasparCGDataCollection.SetData("f20", Score45.Text)
            CasparCGDataCollection.SetData("f21", Score46.Text)
            CasparCGDataCollection.SetData("f22", Score47.Text)
            CasparCGDataCollection.SetData("f23", Score48.Text)

            CasparCGDataCollection.SetData("m1", middle19.Text)
            CasparCGDataCollection.SetData("m2", middle20.Text)
            CasparCGDataCollection.SetData("m3", middle21.Text)
            CasparCGDataCollection.SetData("m4", middle22.Text)
            CasparCGDataCollection.SetData("m5", middle23.Text)
            CasparCGDataCollection.SetData("m6", middle24.Text)



            ' altering colour
            If FT7.Checked = True Then
                CasparCGDataCollection.SetData("ft1", "0xff0000")
            End If
            If FT8.Checked = True Then
                CasparCGDataCollection.SetData("ft2", "0xff0000")
            End If
            If FT9.Checked = True Then
                CasparCGDataCollection.SetData("ft3", "0xff0000")
            End If
            If FT10.Checked = True Then
                CasparCGDataCollection.SetData("ft4", "0xff0000")
            End If
            If FT11.Checked = True Then
                CasparCGDataCollection.SetData("ft5", "0xff0000")
            End If
            If FT12.Checked = True Then
                CasparCGDataCollection.SetData("ft6", "0xff0000")
            End If


            'showing layers of bars
            If CheckBox19.Checked = True Then
                CasparDevice.SendString("MIXER 2-104 OPACITY 0")
                CasparDevice.SendString("play 2-104 SCORESBAR_1")
                CasparDevice.SendString("MIXER 2-104 OPACITY 1 48 linear")
            End If
            If CheckBox20.Checked = True Then
                CasparDevice.SendString("MIXER 2-105 OPACITY 0")
                CasparDevice.SendString("play 2-105 SCORESBAR_2")
                CasparDevice.SendString("MIXER 2-105 OPACITY 1 48 linear")
            End If

            If CheckBox21.Checked = True Then
                CasparDevice.SendString("MIXER 2-106 OPACITY 0")
                CasparDevice.SendString("play 2-106 SCORESBAR_3")
                CasparDevice.SendString("MIXER 2-106 OPACITY 1 48 linear")
            End If

            If CheckBox22.Checked = True Then
                CasparDevice.SendString("MIXER 2-107 OPACITY 0")
                CasparDevice.SendString("play 2-107 SCORESBAR_4")
                CasparDevice.SendString("MIXER 2-107 OPACITY 1 48 linear")
            End If

            If CheckBox23.Checked = True Then
                CasparDevice.SendString("MIXER 2-108 OPACITY 0")
                CasparDevice.SendString("play 2-108 SCORESBAR_5")
                CasparDevice.SendString("MIXER 2-108 OPACITY 1 48 linear")
            End If

            If CheckBox24.Checked = True Then
                CasparDevice.SendString("MIXER 2-109 OPACITY 0")
                CasparDevice.SendString("play 2-109 SCORESBAR_6")
                CasparDevice.SendString("MIXER 2-109 OPACITY 1 48 linear")
            End If


            CasparDevice.Channels(1).CG.Add(101, "efc_premscores", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            ' select which logo to show
            If tab1Logo2Select.Text = "Premier League" Then
                CasparDevice.SendString("play 2-100 SCORES_BPL_LOGO")
            End If
            If tab1Logo2Select.Text = "Championship" Then
                CasparDevice.SendString("play 2-100 SCORES_CHAMPIONSHIP_LOGO")
            End If
            If tab1Logo2Select.Text = "Capital One Cup" Then
                CasparDevice.SendString("play 2-100 SCORES_C1C_LOGO")
            End If
            If tab1Logo2Select.Text = "Europa League" Then
                CasparDevice.SendString("play 2-100 SCORES_EUROPALEAGUE_LOGO")
            End If
            If tab1Logo2Select.Text = "FA Cup" Then
                CasparDevice.SendString("play 2-100 SCORES_FACUP_LOGO")
            End If
            If tab1Logo2Select.Text = "Champions League" Then
                CasparDevice.SendString("play 2-100 SCORES_CHAMPLEAGUE_LOGO")
            End If


            'set colours on buttons

            TXPremScores_2BTN.BackColor = Color.Green
            showPremScores.UseVisualStyleBackColor = True

            'stop preview
            'CasparDevice.Channels(1).CG.Stop(101)
            ' CasparDevice.SendString("stop 2-100")

            'disable button
            TXPremScores_2BTN.Enabled = False
            showPremScores.Enabled = True
        End If
    End Sub


    Private Sub HidePremScoresBTN_Click(sender As Object, e As EventArgs) Handles HidePremScoresBTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-104 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-105 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-106 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-107 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-108 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-109 OPACITY 0 24 linear")
            countScores = 0
            scoresFadeOut.Enabled = True
            CasparDevice.SendString("STOP 2-110")
            showPremScores.BackColor = Color.FromKnownColor(KnownColor.Control)
            showPremScores.UseVisualStyleBackColor = True
            TXPremScores_2BTN.UseVisualStyleBackColor = True

            'reenable buttons
            'disable button
            showPremScores.Enabled = True
            TXPremScores_2BTN.Enabled = True
        End If
    End Sub

    Private Sub ChampPreview1Btn_Click(sender As Object, e As EventArgs)
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()
            CasparCGDataCollection.SetData("TITLE", ChampScoresTitle.Text)
            CasparCGDataCollection.SetData("f0", ChampScore1.Text)
            CasparCGDataCollection.SetData("f1", ChampScore2.Text)
            CasparCGDataCollection.SetData("f2", ChampScore3.Text)
            CasparCGDataCollection.SetData("f3", ChampScore4.Text)
            CasparCGDataCollection.SetData("f4", ChampScore5.Text)
            CasparCGDataCollection.SetData("f5", ChampScore6.Text)
            CasparCGDataCollection.SetData("f6", ChampScore7.Text)
            CasparCGDataCollection.SetData("f7", ChampScore8.Text)
            CasparCGDataCollection.SetData("f8", ChampScore9.Text)
            CasparCGDataCollection.SetData("f9", ChampScore10.Text)
            CasparCGDataCollection.SetData("f10", ChampScore11.Text)
            CasparCGDataCollection.SetData("f11", ChampScore12.Text)
            CasparCGDataCollection.SetData("f12", ChampScore13.Text)
            CasparCGDataCollection.SetData("f13", ChampScore14.Text)
            CasparCGDataCollection.SetData("f14", ChampScore15.Text)
            CasparCGDataCollection.SetData("f15", ChampScore16.Text)
            CasparCGDataCollection.SetData("f16", ChampScore17.Text)
            CasparCGDataCollection.SetData("f17", ChampScore18.Text)
            CasparCGDataCollection.SetData("f18", ChampScore19.Text)
            CasparCGDataCollection.SetData("f19", ChampScore20.Text)
            CasparCGDataCollection.SetData("f20", ChampScore21.Text)
            CasparCGDataCollection.SetData("f21", ChampScore22.Text)
            CasparCGDataCollection.SetData("f22", ChampScore23.Text)
            CasparCGDataCollection.SetData("f23", ChampScore24.Text)

            CasparDevice.Channels(1).CG.Add(101, "efc_premscores", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            'CasparDevice.SendString("play 2-102 LATESTSCORES")
            CasparDevice.SendString("play 2-100 efcLatestScoresChamp")
        End If
    End Sub

    Private Sub ChampTX1Btn_Click(sender As Object, e As EventArgs) Handles ChampTX1Btn.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()
            CasparCGDataCollection.SetData("TITLE", ChampScoresTitle.Text)
            CasparCGDataCollection.SetData("f0", ChampScore1.Text)
            CasparCGDataCollection.SetData("f1", ChampScore2.Text)
            CasparCGDataCollection.SetData("f2", ChampScore3.Text)
            CasparCGDataCollection.SetData("f3", ChampScore4.Text)
            CasparCGDataCollection.SetData("f4", ChampScore5.Text)
            CasparCGDataCollection.SetData("f5", ChampScore6.Text)
            CasparCGDataCollection.SetData("f6", ChampScore7.Text)
            CasparCGDataCollection.SetData("f7", ChampScore8.Text)
            CasparCGDataCollection.SetData("f8", ChampScore9.Text)
            CasparCGDataCollection.SetData("f9", ChampScore10.Text)
            CasparCGDataCollection.SetData("f10", ChampScore11.Text)
            CasparCGDataCollection.SetData("f11", ChampScore12.Text)
            CasparCGDataCollection.SetData("f12", ChampScore13.Text)
            CasparCGDataCollection.SetData("f13", ChampScore14.Text)
            CasparCGDataCollection.SetData("f14", ChampScore15.Text)
            CasparCGDataCollection.SetData("f15", ChampScore16.Text)
            CasparCGDataCollection.SetData("f16", ChampScore17.Text)
            CasparCGDataCollection.SetData("f17", ChampScore18.Text)
            CasparCGDataCollection.SetData("f18", ChampScore19.Text)
            CasparCGDataCollection.SetData("f19", ChampScore20.Text)
            CasparCGDataCollection.SetData("f20", ChampScore21.Text)
            CasparCGDataCollection.SetData("f21", ChampScore22.Text)
            CasparCGDataCollection.SetData("f22", ChampScore23.Text)
            CasparCGDataCollection.SetData("f23", ChampScore24.Text)

            CasparCGDataCollection.SetData("m1", middle1.Text)
            CasparCGDataCollection.SetData("m2", middle2.Text)
            CasparCGDataCollection.SetData("m3", middle3.Text)
            CasparCGDataCollection.SetData("m4", middle4.Text)
            CasparCGDataCollection.SetData("m5", middle5.Text)
            CasparCGDataCollection.SetData("m6", middle6.Text)

            ' altering colour
            If FT13.Checked = True Then
                CasparCGDataCollection.SetData("ft1", "0xff0000")
            End If
            If FT14.Checked = True Then
                CasparCGDataCollection.SetData("ft2", "0xff0000")
            End If
            If FT15.Checked = True Then
                CasparCGDataCollection.SetData("ft3", "0xff0000")
            End If
            If FT16.Checked = True Then
                CasparCGDataCollection.SetData("ft4", "0xff0000")
            End If
            If FT17.Checked = True Then
                CasparCGDataCollection.SetData("ft5", "0xff0000")
            End If
            If FT18.Checked = True Then
                CasparCGDataCollection.SetData("ft6", "0xff0000")
            End If

            'showing layers of bars
            If CheckBox1.Checked = True Then
                CasparDevice.SendString("MIXER 2-104 OPACITY 0")
                CasparDevice.SendString("play 2-104 SCORESBAR_1")
                CasparDevice.SendString("MIXER 2-104 OPACITY 1 48 linear")
            End If
            If CheckBox2.Checked = True Then
                CasparDevice.SendString("MIXER 2-105 OPACITY 0")
                CasparDevice.SendString("play 2-105 SCORESBAR_2")
                CasparDevice.SendString("MIXER 2-105 OPACITY 1 48 linear")
            End If

            If CheckBox3.Checked = True Then
                CasparDevice.SendString("MIXER 2-106 OPACITY 0")
                CasparDevice.SendString("play 2-106 SCORESBAR_3")
                CasparDevice.SendString("MIXER 2-106 OPACITY 1 48 linear")
            End If

            If CheckBox4.Checked = True Then
                CasparDevice.SendString("MIXER 2-107 OPACITY 0")
                CasparDevice.SendString("play 2-107 SCORESBAR_4")
                CasparDevice.SendString("MIXER 2-107 OPACITY 1 48 linear")
            End If

            If CheckBox5.Checked = True Then
                CasparDevice.SendString("MIXER 2-108 OPACITY 0")
                CasparDevice.SendString("play 2-108 SCORESBAR_5")
                CasparDevice.SendString("MIXER 2-108 OPACITY 1 48 linear")
            End If

            If CheckBox6.Checked = True Then
                CasparDevice.SendString("MIXER 2-109 OPACITY 0")
                CasparDevice.SendString("play 2-109 SCORESBAR_6")
                CasparDevice.SendString("MIXER 2-109 OPACITY 1 48 linear")
            End If




            CasparDevice.Channels(1).CG.Add(101, "efc_premscores", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            ' select which logo to show
            If tab2Logo1Select.Text = "Premier League" Then
                CasparDevice.SendString("play 2-100 SCORES_BPL_LOGO")
            End If
            If tab2Logo1Select.Text = "Championship" Then
                CasparDevice.SendString("play 2-100 SCORES_CHAMPIONSHIP_LOGO")
            End If
            If tab2Logo1Select.Text = "Capital One Cup" Then
                CasparDevice.SendString("play 2-100 SCORES_C1C_LOGO")
            End If
            If tab2Logo1Select.Text = "Europa League" Then
                CasparDevice.SendString("play 2-100 SCORES_EUROPALEAGUE_LOGO")
            End If
            If tab2Logo1Select.Text = "FA Cup" Then
                CasparDevice.SendString("play 2-100 SCORES_FACUP_LOGO")
            End If
            If tab2Logo1Select.Text = "Champions League" Then
                CasparDevice.SendString("play 2-100 SCORES_CHAMPLEAGUE_LOGO")
            End If


            'set colours on buttons
            ChampTX1Btn.BackColor = Color.Green
            ChampTX2Btn.UseVisualStyleBackColor = True

            'disable button
            ChampTX1Btn.Enabled = False

        End If
    End Sub

    Private Sub ChampHide1BTN_Click(sender As Object, e As EventArgs) Handles ChampHide1BTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-104 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-105 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-106 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-107 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-108 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-109 OPACITY 0 24 linear")
            countScores = 0
            scoresFadeOut.Enabled = True
            CasparDevice.SendString("STOP 2-110")
            ChampTX1Btn.BackColor = Color.FromKnownColor(KnownColor.Control)
            ChampTX1Btn.UseVisualStyleBackColor = True
            ChampTX2Btn.UseVisualStyleBackColor = True

            're-enable button
            ChampTX1Btn.Enabled = True
        End If
    End Sub

    Private Sub ChampTX2Btn_Click(sender As Object, e As EventArgs) Handles ChampTX2Btn.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()
            CasparCGDataCollection.SetData("TITLE", ChampScoresTitle2.Text)
            CasparCGDataCollection.SetData("f0", ChampScore25.Text)
            CasparCGDataCollection.SetData("f1", ChampScore26.Text)
            CasparCGDataCollection.SetData("f2", ChampScore27.Text)
            CasparCGDataCollection.SetData("f3", ChampScore28.Text)
            CasparCGDataCollection.SetData("f4", ChampScore29.Text)
            CasparCGDataCollection.SetData("f5", ChampScore30.Text)
            CasparCGDataCollection.SetData("f6", ChampScore31.Text)
            CasparCGDataCollection.SetData("f7", ChampScore32.Text)
            CasparCGDataCollection.SetData("f8", ChampScore33.Text)
            CasparCGDataCollection.SetData("f9", ChampScore34.Text)
            CasparCGDataCollection.SetData("f10", ChampScore35.Text)
            CasparCGDataCollection.SetData("f11", ChampScore36.Text)
            CasparCGDataCollection.SetData("f12", ChampScore37.Text)
            CasparCGDataCollection.SetData("f13", ChampScore38.Text)
            CasparCGDataCollection.SetData("f14", ChampScore39.Text)
            CasparCGDataCollection.SetData("f15", ChampScore40.Text)
            CasparCGDataCollection.SetData("f16", ChampScore41.Text)
            CasparCGDataCollection.SetData("f17", ChampScore42.Text)
            CasparCGDataCollection.SetData("f18", ChampScore43.Text)
            CasparCGDataCollection.SetData("f19", ChampScore44.Text)
            CasparCGDataCollection.SetData("f20", ChampScore45.Text)
            CasparCGDataCollection.SetData("f21", ChampScore46.Text)
            CasparCGDataCollection.SetData("f22", ChampScore47.Text)
            CasparCGDataCollection.SetData("f23", ChampScore48.Text)

            CasparCGDataCollection.SetData("m1", middle7.Text)
            CasparCGDataCollection.SetData("m2", middle8.Text)
            CasparCGDataCollection.SetData("m3", middle9.Text)
            CasparCGDataCollection.SetData("m4", middle10.Text)
            CasparCGDataCollection.SetData("m5", middle11.Text)
            CasparCGDataCollection.SetData("m6", middle12.Text)



            ' altering colour
            If FT19.Checked = True Then
                CasparCGDataCollection.SetData("ft1", "0xff0000")
            End If
            If FT20.Checked = True Then
                CasparCGDataCollection.SetData("ft2", "0xff0000")
            End If
            If FT21.Checked = True Then
                CasparCGDataCollection.SetData("ft3", "0xff0000")
            End If
            If FT22.Checked = True Then
                CasparCGDataCollection.SetData("ft4", "0xff0000")
            End If
            If FT23.Checked = True Then
                CasparCGDataCollection.SetData("ft5", "0xff0000")
            End If
            If FT24.Checked = True Then
                CasparCGDataCollection.SetData("ft6", "0xff0000")
            End If

            'showing layers of bars
            If CheckBox12.Checked = True Then
                CasparDevice.SendString("MIXER 2-104 OPACITY 0")
                CasparDevice.SendString("play 2-104 SCORESBAR_1")
                CasparDevice.SendString("MIXER 2-104 OPACITY 1 48 linear")
            End If
            If CheckBox11.Checked = True Then
                CasparDevice.SendString("MIXER 2-105 OPACITY 0")
                CasparDevice.SendString("play 2-105 SCORESBAR_2")
                CasparDevice.SendString("MIXER 2-105 OPACITY 1 48 linear")
            End If

            If CheckBox10.Checked = True Then
                CasparDevice.SendString("MIXER 2-106 OPACITY 0")
                CasparDevice.SendString("play 2-106 SCORESBAR_3")
                CasparDevice.SendString("MIXER 2-106 OPACITY 1 48 linear")
            End If

            If CheckBox9.Checked = True Then
                CasparDevice.SendString("MIXER 2-107 OPACITY 0")
                CasparDevice.SendString("play 2-107 SCORESBAR_4")
                CasparDevice.SendString("MIXER 2-107 OPACITY 1 48 linear")
            End If

            If CheckBox8.Checked = True Then
                CasparDevice.SendString("MIXER 2-108 OPACITY 0")
                CasparDevice.SendString("play 2-108 SCORESBAR_5")
                CasparDevice.SendString("MIXER 2-108 OPACITY 1 48 linear")
            End If

            If CheckBox7.Checked = True Then
                CasparDevice.SendString("MIXER 2-109 OPACITY 0")
                CasparDevice.SendString("play 2-109 SCORESBAR_6")
                CasparDevice.SendString("MIXER 2-109 OPACITY 1 48 linear")
            End If


            CasparDevice.Channels(1).CG.Add(101, "efc_premscores", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)

            ' select which logo to show
            If tab2Logo2Select.Text = "Premier League" Then
                CasparDevice.SendString("play 2-100 SCORES_BPL_LOGO")
            End If
            If tab2Logo2Select.Text = "Championship" Then
                CasparDevice.SendString("play 2-100 SCORES_CHAMPIONSHIP_LOGO")
            End If
            If tab2Logo2Select.Text = "Capital One Cup" Then
                CasparDevice.SendString("play 2-100 SCORES_C1C_LOGO")
            End If
            If tab2Logo2Select.Text = "Europa League" Then
                CasparDevice.SendString("play 2-100 SCORES_EUROPALEAGUE_LOGO")
            End If
            If tab2Logo2Select.Text = "FA Cup" Then
                CasparDevice.SendString("play 2-100 SCORES_FACUP_LOGO")
            End If
            If tab2Logo2Select.Text = "Champions League" Then
                CasparDevice.SendString("play 2-100 SCORES_CHAMPLEAGUE_LOGO")
            End If


            'set colours on buttons

            ChampTX2Btn.BackColor = Color.Green
            ChampTX1Btn.UseVisualStyleBackColor = True

            'disable button
            ChampTX2Btn.Enabled = False

            'disable button
            ChampTX1Btn.Enabled = True
        End If
    End Sub

    Private Sub ChampHide2BTN_Click(sender As Object, e As EventArgs) Handles ChampHide2BTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-104 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-105 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-106 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-107 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-108 OPACITY 0 24 linear")
            CasparDevice.SendString("MIXER 2-109 OPACITY 0 24 linear")
            countScores = 0
            scoresFadeOut.Enabled = True
            CasparDevice.SendString("STOP 2-110")
            ChampTX1Btn.BackColor = Color.FromKnownColor(KnownColor.Control)
            ChampTX1Btn.UseVisualStyleBackColor = True
            ChampTX2Btn.UseVisualStyleBackColor = True

            'disable button
            ChampTX1Btn.Enabled = True
            ChampTX2Btn.Enabled = True
        End If
    End Sub

    Private Sub saveDataChampBTN_Click(sender As Object, e As EventArgs)
        'first let's check if there is a file MyXML.xml into our application folder
        'if there wasn't a file something like that, then let's create a new one.

        'If IO.File.Exists("MyXML.xml") = False Then

        'declare our xmlwritersettings object
        Dim settings As New XmlWriterSettings()

        'lets tell to our xmlwritersettings that it must use indention for our xml
        settings.Indent = True

        'lets create the MyXML.xml document, the first parameter was the Path/filename of xml file
        ' the second parameter was our xml settings
        Dim XmlWrt As XmlWriter = XmlWriter.Create("ChampXML.xml", settings)

        With XmlWrt

            ' Write the Xml declaration.
            .WriteStartDocument()

            ' Write a comment.
            .WriteComment("XML Database.")

            ' Write the root element.
            .WriteStartElement("ChampionshipScores")

            ' Start our first person.
            .WriteStartElement("Row1")

            ' The person nodes.

            .WriteStartElement("HomeTeam1")
            .WriteString(ChampScore1.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore1")
            .WriteString(ChampScore2.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam1")
            .WriteString(ChampScore4.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore1")
            .WriteString(ChampScore3.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row2")

            ' The person nodes.

            .WriteStartElement("HomeTeam2")
            .WriteString(ChampScore5.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore2")
            .WriteString(ChampScore6.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam2")
            .WriteString(ChampScore8.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore2")
            .WriteString(ChampScore7.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            .WriteStartElement("Row3")

            ' The person nodes.

            .WriteStartElement("HomeTeam3")
            .WriteString(ChampScore9.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore3")
            .WriteString(ChampScore10.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam3")
            .WriteString(ChampScore12.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore3")
            .WriteString(ChampScore11.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row4")

            ' The person nodes.

            .WriteStartElement("HomeTeam4")
            .WriteString(ChampScore13.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore4")
            .WriteString(ChampScore14.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam4")
            .WriteString(ChampScore16.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore4")
            .WriteString(ChampScore15.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row5")

            ' The person nodes.

            .WriteStartElement("HomeTeam5")
            .WriteString(ChampScore17.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore5")
            .WriteString(ChampScore18.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam5")
            .WriteString(ChampScore20.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore5")
            .WriteString(ChampScore19.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            .WriteStartElement("Row6")

            ' The person nodes.

            .WriteStartElement("HomeTeam6")
            .WriteString(ChampScore21.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore6")
            .WriteString(ChampScore22.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam6")
            .WriteString(ChampScore24.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore6")
            .WriteString(ChampScore23.Text)
            .WriteEndElement()


            ' SET TWO 
            .WriteStartElement("Row7")

            ' The person nodes.

            .WriteStartElement("HomeTeam7")
            .WriteString(ChampScore25.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore7")
            .WriteString(ChampScore26.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam7")
            .WriteString(ChampScore28.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore7")
            .WriteString(ChampScore27.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row8")

            ' The person nodes.

            .WriteStartElement("HomeTeam8")
            .WriteString(ChampScore29.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore8")
            .WriteString(ChampScore30.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam8")
            .WriteString(ChampScore32.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore8")
            .WriteString(ChampScore31.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            .WriteStartElement("Row9")

            ' The person nodes.

            .WriteStartElement("HomeTeam9")
            .WriteString(ChampScore33.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore9")
            .WriteString(ChampScore34.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam9")
            .WriteString(ChampScore36.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore9")
            .WriteString(ChampScore35.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row10")

            ' The person nodes.

            .WriteStartElement("HomeTeam10")
            .WriteString(ChampScore37.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore10")
            .WriteString(ChampScore38.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam10")
            .WriteString(ChampScore40.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore10")
            .WriteString(ChampScore39.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()


            .WriteStartElement("Row11")

            ' The person nodes.

            .WriteStartElement("HomeTeam11")
            .WriteString(ChampScore41.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore11")
            .WriteString(ChampScore42.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam11")
            .WriteString(ChampScore44.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore11")
            .WriteString(ChampScore43.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            .WriteStartElement("Row12")

            ' The person nodes.

            .WriteStartElement("HomeTeam12")
            .WriteString(ChampScore45.Text)
            .WriteEndElement()

            .WriteStartElement("HomeTeamScore12")
            .WriteString(ChampScore46.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeam12")
            .WriteString(ChampScore48.Text)
            .WriteEndElement()

            .WriteStartElement("AwayTeamScore12")
            .WriteString(ChampScore47.Text)
            .WriteEndElement()


            ' The end of this person.
            .WriteEndElement()

            ' Close the XmlTextWriter.
            .WriteEndDocument()
            .Close()

        End With

        MessageBox.Show("XML file saved.")
        ' End If
    End Sub

    Private Sub loadDataChampBTN_Click(sender As Object, e As EventArgs)
        'check if file myxml.xml is existing
        If (IO.File.Exists("MyXML.xml")) Then

            'create a new xmltextreader object
            'this is the object that we will loop and will be used to read the xml file
            Dim document As XmlReader = New XmlTextReader("ChampXML.xml")

            'loop through the xml file
            While (document.Read())

                Dim type = document.NodeType

                'if node type was element
                If (type = XmlNodeType.Element) Then
                    If (document.Name = "HomeTeam1") Then
                        ChampScore1.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore1") Then
                        ChampScore2.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam1") Then
                        ChampScore4.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore1") Then
                        ChampScore3.Text = document.ReadInnerXml.ToString()
                    End If

                    'row 2
                    If (document.Name = "HomeTeam2") Then
                        ChampScore5.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore2") Then
                        ChampScore6.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam2") Then
                        ChampScore8.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore2") Then
                        ChampScore7.Text = document.ReadInnerXml.ToString()
                    End If

                    'row3
                    If (document.Name = "HomeTeam3") Then
                        ChampScore9.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore3") Then
                        ChampScore10.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam3") Then
                        ChampScore12.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore3") Then
                        ChampScore11.Text = document.ReadInnerXml.ToString()
                    End If

                    'row4
                    If (document.Name = "HomeTeam4") Then
                        ChampScore13.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore4") Then
                        ChampScore14.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam4") Then
                        ChampScore16.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore4") Then
                        ChampScore15.Text = document.ReadInnerXml.ToString()
                    End If

                    'row5
                    If (document.Name = "HomeTeam5") Then
                        ChampScore17.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore5") Then
                        ChampScore18.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam5") Then
                        ChampScore20.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore5") Then
                        ChampScore19.Text = document.ReadInnerXml.ToString()
                    End If

                    'row6
                    If (document.Name = "HomeTeam6") Then
                        ChampScore21.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore6") Then
                        ChampScore22.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam6") Then
                        ChampScore24.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore6") Then
                        ChampScore23.Text = document.ReadInnerXml.ToString()
                    End If

                    'second set
                    'row 7
                    If (document.Name = "HomeTeam7") Then
                        ChampScore25.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore7") Then
                        ChampScore26.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam7") Then
                        ChampScore28.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore7") Then
                        ChampScore27.Text = document.ReadInnerXml.ToString()
                    End If

                    'row 2
                    If (document.Name = "HomeTeam8") Then
                        ChampScore29.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore8") Then
                        ChampScore30.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam8") Then
                        ChampScore32.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore8") Then
                        ChampScore31.Text = document.ReadInnerXml.ToString()
                    End If

                    'row3
                    If (document.Name = "HomeTeam9") Then
                        ChampScore33.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore9") Then
                        ChampScore34.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam9") Then
                        ChampScore36.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore9") Then
                        ChampScore35.Text = document.ReadInnerXml.ToString()
                    End If

                    'row4
                    If (document.Name = "HomeTeam10") Then
                        ChampScore37.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore10") Then
                        ChampScore38.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam10") Then
                        ChampScore40.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore10") Then
                        ChampScore39.Text = document.ReadInnerXml.ToString()
                    End If

                    'row5
                    If (document.Name = "HomeTeam11") Then
                        ChampScore41.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore11") Then
                        ChampScore42.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam11") Then
                        ChampScore44.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore11") Then
                        ChampScore43.Text = document.ReadInnerXml.ToString()
                    End If

                    'row6
                    If (document.Name = "HomeTeam12") Then
                        ChampScore45.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "HomeTeamScore12") Then
                        ChampScore46.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeam12") Then
                        ChampScore48.Text = document.ReadInnerXml.ToString()
                    End If
                    If (document.Name = "AwayTeamScore12") Then
                        ChampScore47.Text = document.ReadInnerXml.ToString()
                    End If

                End If

            End While
            document.Close()
        Else

            MessageBox.Show("The filename you selected was not found.")
        End If
    End Sub

    Private Sub LTStrapTXBTN_Click(sender As Object, e As EventArgs) Handles LTStrapTXBTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()

            If Me.ListBox3.Items.Count > 0 And Me.ListBox4.Items.Count > 0 Then

                If ScoreMatchIDRadioBut.Checked = True Then
                    CasparCGDataCollection.SetData("f0", HomeTeamName.Text)
                    CasparCGDataCollection.SetData("f3", AwayTeamName.Text)
                    CasparCGDataCollection.SetData("f1", HomeScore.Text)
                    CasparCGDataCollection.SetData("f2", AwayScore.Text)

                    CasparDevice.Channels(0).CG.Add(101, "LowerThirdMatchScore", True, CasparCGDataCollection.ToAMCPEscapedXml)
                    CasparDevice.Channels(0).CG.Play(101)
                    CasparDevice.SendString("play 1-102 LTFlare")

                    'fading in image
                    CasparDevice.SendString("MIXER 1-100 OPACITY 0")
                    CasparDevice.SendString("play 1-100 LOWER_THIRD2")
                    CasparDevice.SendString("MIXER 1-100 OPACITY 1 48 linear")

                End If
                If LTMatchIDRadioBut.Checked = True Then
                    CasparCGDataCollection.SetData("f0", HomeTeamName.Text)
                    CasparCGDataCollection.SetData("f1", AwayTeamName.Text)
                    CasparCGDataCollection.SetData("f2", LTStrapDate.Text)
                    CasparCGDataCollection.SetData("f3", LTStrapKO.Text)

                    CasparDevice.Channels(0).CG.Add(101, "LowerThirdMatchIDst", True, CasparCGDataCollection.ToAMCPEscapedXml)
                    CasparDevice.Channels(0).CG.Play(101)
                    CasparDevice.SendString("play 1-102 LTFlare")
                    'fading in image
                    CasparDevice.SendString("MIXER 1-100 OPACITY 0")
                    CasparDevice.SendString("play 1-100 LOWER_THIRD2")
                    CasparDevice.SendString("MIXER 1-100 OPACITY 1 48 linear")
                End If

                '"play 1-1 " & ListBox3.Text & " loop auto"
                LTStrapTXBTN.BackColor = Color.Green
                'disable button
                LTStrapTXBTN.Enabled = False
            Else
                MessageBox.Show("You need to load some squad sheets", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub LTStrapHideBTN_Click(sender As Object, e As EventArgs) Handles LTStrapHideBTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(0).CG.Stop(101)
            CasparDevice.SendString("stop 1-102")
            CasparDevice.SendString("MIXER 1-100 OPACITY 0 24 linear")
            countBPS = 0
            BPlayChanFadeOut.Enabled = True
            LTStrapTXBTN.BackColor = Color.FromKnownColor(KnownColor.Control)
            LTStrapTXBTN.UseVisualStyleBackColor = True
            're-enebla button
            LTStrapTXBTN.Enabled = True
        End If
    End Sub

    Private Sub showAddedTimeBTN_Click(sender As Object, e As EventArgs) Handles showAddedTimeBTN.Click
        CasparCGDataCollection.SetData("f0", addedTimeVals.Text)
        ' showing
        CasparDevice.Channels(0).CG.Add(391, "efc_addedTime_temp_BUG", True, CasparCGDataCollection.ToAMCPEscapedXml)
        CasparDevice.Channels(0).CG.Play(391)
        CasparDevice.SendString("play 1-390 EFC_CLOCK_ADDEDTIME_PLINTH")
        ' prewviewin
        'CasparDevice.Channels(1).CG.Add(391, "efc_addedTime_temp_BUG", True, CasparCGDataCollection.ToAMCPEscapedXml)
        'CasparDevice.Channels(1).CG.Play(391)
        ' CasparDevice.SendString("play 2-390 EFC_CLOCK_ADDEDTIME_PLINTH")
        showAddedTimeBTN.BackColor = Color.Green
    End Sub

    Private Sub hideAddedTimeButton_Click(sender As Object, e As EventArgs) Handles hideAddedTimeButton.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(0).CG.Stop(391)
            CasparDevice.SendString("stop 1-390")
            'count = 0
            'clockAnimation.Enabled = True
            'stop preview
            'CasparDevice.Channels(1).CG.Stop(391)
            ' CasparDevice.SendString("stop 2-390")
            showAddedTimeBTN.BackColor = Color.FromKnownColor(KnownColor.Control)
            showAddedTimeBTN.UseVisualStyleBackColor = True
        End If
    End Sub

    Private Sub ClearTXBTN_Click(sender As Object, e As EventArgs) Handles ClearTXBTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(0).CG.Stop(50)
            CasparDevice.Channels(0).CG.Stop(99)
            CasparDevice.Channels(0).CG.Stop(100)
            CasparDevice.Channels(0).CG.Stop(101)
            CasparDevice.Channels(0).CG.Stop(102)
            CasparDevice.Channels(0).CG.Stop(400)
            CasparDevice.Channels(0).CG.Stop(401)
            CasparDevice.Channels(0).CG.Stop(390)
            CasparDevice.Channels(0).CG.Stop(391)
            CasparDevice.SendString("stop 1-50")
            CasparDevice.SendString("stop 1-99")
            CasparDevice.SendString("stop 1-100")
            CasparDevice.SendString("stop 1-101")
            CasparDevice.SendString("stop 1-102")
            CasparDevice.SendString("stop 1-400")
            CasparDevice.SendString("stop 1-401")
            CasparDevice.SendString("stop 1-390")
            CasparDevice.SendString("stop 1-391")
            CasparDevice.SendString("stop 1-104")

            CasparDevice.Channels(1).CG.Stop(50)
            CasparDevice.Channels(1).CG.Stop(99)
            CasparDevice.Channels(1).CG.Stop(100)
            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.Channels(1).CG.Stop(102)
            CasparDevice.Channels(1).CG.Stop(400)
            CasparDevice.Channels(1).CG.Stop(401)
            CasparDevice.Channels(1).CG.Stop(390)
            CasparDevice.Channels(1).CG.Stop(391)
            CasparDevice.SendString("stop 2-50")
            CasparDevice.SendString("stop 2-99")
            CasparDevice.SendString("stop 2-100")
            CasparDevice.SendString("stop 2-101")
            CasparDevice.SendString("stop 2-102")
            CasparDevice.SendString("stop 2-400")
            CasparDevice.SendString("stop 2-401")
            CasparDevice.SendString("stop 2-390")
            CasparDevice.SendString("stop 2-391")
            'scores page
            CasparDevice.SendString("stop 2-104")
            CasparDevice.SendString("stop 2-105")
            CasparDevice.SendString("stop 2-106")
            CasparDevice.SendString("stop 2-107")
            CasparDevice.SendString("stop 2-108")
            CasparDevice.SendString("stop 2-109")

            ' this also needs to reset buttons
            ShowTeamSheet.UseVisualStyleBackColor = True
            ShowSubsSheet.UseVisualStyleBackColor = True
            ShowAwayFirstEleven.UseVisualStyleBackColor = True
            ShowAwaySubsSheet.UseVisualStyleBackColor = True
            showClock.UseVisualStyleBackColor = True
            playVid.UseVisualStyleBackColor = True
            playNext.UseVisualStyleBackColor = True
            'LoopVid.UseVisualStyleBackColor = True
            showBigScore.UseVisualStyleBackColor = True
            showPremScores.UseVisualStyleBackColor = True
            TXPremScores_2BTN.UseVisualStyleBackColor = True
            ChampTX1Btn.UseVisualStyleBackColor = True
            ChampTX2Btn.UseVisualStyleBackColor = True
            LTStrapTXBTN.UseVisualStyleBackColor = True
            CrawlOn.UseVisualStyleBackColor = True
            showSub.UseVisualStyleBackColor = True
            AwaySubOn.UseVisualStyleBackColor = True
            showAddedTimeBTN.UseVisualStyleBackColor = True
            ShowClockInGameBTN.UseVisualStyleBackColor = True
            PlayVidInGame.UseVisualStyleBackColor = True
            PlayNextVidInGame.UseVisualStyleBackColor = True
            'LoopVidInGame.UseVisualStyleBackColor = True
            showClock.UseVisualStyleBackColor = True
            '  startAndShowClockBTN.UseVisualStyleBackColor = True
            backgroundOn.Checked = False
        End If
    End Sub

    Private Sub TimeOfDayCLock_Tick(sender As Object, e As EventArgs) Handles TimeOfDayCLock.Tick
        timeOfDayText.Text = Now.ToLongTimeString
    End Sub


    Private Sub refreshVideInGame_Click(sender As Object, e As EventArgs) Handles refreshVideInGame.Click
        Dim File As Svt.Caspar.MediaInfo
        CasparDevice.RefreshMediafiles()
        'Clear list box in case of reload
        SourceFilesInGame.Items.Clear()
        Threading.Thread.Sleep(250)

        For Each File In CasparDevice.Mediafiles
            SourceFilesInGame.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
        Next

    End Sub

    Private Sub AddPlaylistInGame_Click(sender As Object, e As EventArgs) Handles AddPlaylistInGame.Click
        playlistFilesInGame.Items.Add(SourceFilesInGame.Text)
    End Sub

    Private Sub ClearPlaylistInGame_Click(sender As Object, e As EventArgs) Handles ClearPlaylistInGame.Click
        playlistFilesInGame.Items.Clear()
    End Sub

    Private Sub RemovePlaylistInGame_Click(sender As Object, e As EventArgs) Handles RemovePlaylistInGame.Click
        playlistFilesInGame.Items.Remove(playlistFilesInGame.SelectedItem)
    End Sub

    Private Sub PlayVidInGame_Click(sender As Object, e As EventArgs) Handles PlayVidInGame.Click
        If Me.CasparDevice.IsConnected = True Then
            If Me.playlistFilesInGame.SelectedIndex >= 0 Then
                '   If PreMatchPlayNext = False Then
                ''fading in image
                ' CasparDevice.SendString("MIXER 2-99 OPACITY 0")
                ' CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text)
                ' CasparDevice.SendString("MIXER 2-99 OPACITY 1 48 linear")
                ''fade out other layer
                'CasparDevice.SendString("MIXER 2-100 OPACITY 0 48 linear")
                'PreMatchPlayNext = True
                'End If
                'If PreMatchPlayNext = True Then
                ''fading in image
                ' CasparDevice.SendString("MIXER 2-100 OPACITY 0")
                ' CasparDevice.SendString("play 2-100 " & playlistFilesInGame.Text)
                '  CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
                '  'fade out other layer
                '  CasparDevice.SendString("MIXER 2-99 OPACITY 0 48 linear")
                'reset for next if
                '   PreMatchPlayNext = False
                '  End If

                'select transition and play file
                If MixTransInGame.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text & " MIX 12 LINEAR")
                End If
                If WipeTransInGame.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text & " SLIDE 20 LEFT")
                End If
                If PushTransInGame.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text & " PUSH 20 EASEINSINE")
                End If

                'CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text)
                PlayVidInGame.BackColor = Color.Green
                'LoopVidInGame.BackColor = Color.FromKnownColor(KnownColor.Control)
                ' LoopVidInGame.UseVisualStyleBackColor = True
                PlayNextVidInGame.BackColor = Color.FromKnownColor(KnownColor.Control)
                PlayNextVidInGame.UseVisualStyleBackColor = True
            End If
        End If
    End Sub

    Private Sub PlayNextVidInGame_Click(sender As Object, e As EventArgs) Handles PlayNextVidInGame.Click
        If Me.playlistFilesInGame.SelectedIndex >= 0 Then
            playlistPositionInGame += 1
            If (playlistFilesInGame.SelectedIndex < (playlistFilesInGame.Items.Count() - 1)) Then
                playlistFilesInGame.SelectedIndex += 1

            End If
            If (playlistPositionInGame > playlistFilesInGame.SelectedIndex) Then
                playlistFilesInGame.SelectedIndex = 0
                playlistPositionInGame = 0
            End If
            ' something is wrong here, it should loop but i think the previous statement is stopping it getting there.


            If Me.CasparDevice.IsConnected = True Then

                '        If PreMatchPlayNext = False Then
                ''fading in image
                'CasparDevice.SendString("MIXER 2-99 OPACITY 0")
                'CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text)
                'CasparDevice.SendString("MIXER 2-99 OPACITY 1 48 linear")
                'fade out other layer
                'CasparDevice.SendString("MIXER 2-100 OPACITY 0 48 linear")
                'PreMatchPlayNext = True
                'End If
                'If PreMatchPlayNext = True Then
                ' 'fading in image
                ' CasparDevice.SendString("MIXER 2-100 OPACITY 0")
                ' CasparDevice.SendString("play 2-100 " & playlistFilesInGame.Text)
                ' CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
                'fade out other layer
                'CasparDevice.SendString("MIXER 2-99 OPACITY 0 48 linear")
                'reset for next if
                'PreMatchPlayNext = False
                'End If

                'select transition and play file
                If MixTransInGame.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text & " MIX 12 LINEAR")
                End If
                If WipeTransInGame.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text & " SLIDE 20 LEFT")
                End If
                If PushTransInGame.Checked = True Then
                    CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text & " PUSH 20 EASEINSINE")
                End If

                PlayNextVidInGame.BackColor = Color.Green
                PlayVidInGame.BackColor = Color.FromKnownColor(KnownColor.Control)
                PlayVidInGame.UseVisualStyleBackColor = True
                ' LoopVidInGame.BackColor = Color.FromKnownColor(KnownColor.Control)
                ' LoopVidInGame.UseVisualStyleBackColor = True
            End If
        End If

    End Sub

    Private Sub LoopVidInGame_Click(sender As Object, e As EventArgs)
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.SendString("play 2-99 " & playlistFilesInGame.Text & " loop auto")
            ' LoopVidInGame.BackColor = Color.Green
            PlayVidInGame.BackColor = Color.FromKnownColor(KnownColor.Control)
            PlayVidInGame.UseVisualStyleBackColor = True
            PlayNextVidInGame.BackColor = Color.FromKnownColor(KnownColor.Control)
            PlayNextVidInGame.UseVisualStyleBackColor = True
        End If
    End Sub

    Private Sub stopVidInGame_Click(sender As Object, e As EventArgs) Handles stopVidInGame.Click
        If Me.CasparDevice.IsConnected = True Then
            ' fade out opacity and start timer to fade channel back in 
            CasparDevice.SendString("MIXER 2-99 OPACITY 0 12 linear")
            playlistStop.Enabled = True
            'set button colours back
            PlayVidInGame.BackColor = Color.FromKnownColor(KnownColor.Control)
            PlayVidInGame.UseVisualStyleBackColor = True
            PlayNextVidInGame.BackColor = Color.FromKnownColor(KnownColor.Control)
            PlayNextVidInGame.UseVisualStyleBackColor = True
        End If
    End Sub

    Private Sub ShowClockInGameBTN_Click(sender As Object, e As EventArgs) Handles ShowClockInGameBTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparCGDataCollection.Clear()
            'load clock part of template and calculate start time
            Dim startClockCalculation As Integer
            startClockCalculation = Convert.ToInt32(startClockTime.Text) * 60000
            CasparCGDataCollection.SetData("initialvalue", startClockCalculation)

            'play backing
            CasparDevice.SendString("play 1-400 EFC-CLOCK")

            'Put Data into scores part of clock
            CasparCGDataCollection.SetData("f1", homeThreeLetters.Text)
            CasparCGDataCollection.SetData("f2", HomeScore.Text)
            CasparCGDataCollection.SetData("f3", AwayScore.Text)
            CasparCGDataCollection.SetData("f4", awayThreeLetters.Text)
            'choose which clock to play
            If Convert.ToInt32(startClockTime.Text) < 45 Then
                CasparDevice.Channels(0).CG.Add(402, "count_up_timer", True, CasparCGDataCollection.ToAMCPEscapedXml)
            End If
            If Convert.ToInt32(startClockTime.Text) >= 45 Then
                CasparDevice.Channels(0).CG.Add(402, "count_up_timer_over90", True, CasparCGDataCollection.ToAMCPEscapedXml)
            End If
            CasparDevice.Channels(0).CG.Play(402)


            ' showing
            ' CasparDevice.Channels(0).CG.Add(402, "efc_clock_scoresOnly_temp", True, CasparCGDataCollection.ToAMCPEscapedXml)
            ' CasparDevice.Channels(0).CG.Play(402)

            showClock.BackColor = Color.Green
            ShowClockInGameBTN.BackColor = Color.Green
            'disbale button so cant be re-pressed
            showClock.Enabled = False
            '  startAndShowClockBTN.Enabled = False
            ShowClockInGameBTN.Enabled = False

            'start clock on interface
            aa = Val(Now.Second.ToString)
            min.Text = startClockTime.Text
            OnScreenClock.Enabled = True
        End If
    End Sub

    Private Sub HideClockInGameBTN_Click(sender As Object, e As EventArgs) Handles HideClockInGameBTN.Click
        If Me.CasparDevice.IsConnected = True Then
            '  CasparDevice.Channels(0).CG.Stop(401)
            CasparDevice.Channels(0).CG.Stop(402)
            CasparDevice.SendString("MIXER 1-400 OPACITY 0 24 linear")
            count = 0
            clockAnimation.Enabled = True
            'stop preview
            'CasparDevice.Channels(1).CG.Stop(401)
            'CasparDevice.SendString("stop 2-400")
            'showClock.BackColor = Color.FromKnownColor(KnownColor.Control)
            showClock.UseVisualStyleBackColor = True

            'stopping added time
            CasparDevice.Channels(0).CG.Stop(391)
            CasparDevice.SendString("stop 1-390")
            showAddedTimeBTN.BackColor = Color.FromKnownColor(KnownColor.Control)
            showAddedTimeBTN.UseVisualStyleBackColor = True
            're-enable show button
            showClock.Enabled = True
            ' startAndShowClockBTN.Enabled = True
            ShowClockInGameBTN.Enabled = True
            ShowClockInGameBTN.UseVisualStyleBackColor = True
            'stop on screen clock
            OnScreenClock.Enabled = False
            'reset clock on interface
            min.Text = "0"
            sec.Text = "0"
        End If
    End Sub

    Private Sub BPlayChanFadeOut_Tick(sender As Object, e As EventArgs) Handles BPlayChanFadeOut.Tick
        countBPS = countBPS + 1
        If countBPS >= 10 Then
            CasparDevice.SendString("stop 1-100")
            CasparDevice.SendString("MIXER 1-100 OPACITY 1 0 linear")
            BPlayChanFadeOut.Enabled = False
            countBPS = 0
        End If
    End Sub

    Private Sub TSCrawlOnBTN_Click(sender As Object, e As EventArgs) Handles TSCrawlOnBTN.Click



        If Me.CasparDevice.IsConnected = True Then
            ' If Me.SubOff.SelectedIndex >= 0 Then
            'CasparDevice.Channels(0).CG.Stop(2)
            CasparCGDataCollection.Clear() 'cgData.Clear()

            If ShowHomeFirstElevenCrawl.Checked = True Then
                CasparCGDataCollection.SetData("f0", HomeTeamName.Text + " TEAMSHEET:       " + ListBox3.Items(0).ToString + "      " + ListBox3.Items(1).ToString + "      " + ListBox3.Items(2).ToString + "      " + ListBox3.Items(3).ToString + "      " + ListBox3.Items(4).ToString + "      " + ListBox3.Items(5).ToString + "      " + ListBox3.Items(6).ToString + "      " + ListBox3.Items(7).ToString + "      " + ListBox3.Items(8).ToString + "      " + ListBox3.Items(9).ToString + "      " + ListBox3.Items(10).ToString + "     SUBSTITUTES:        " + ListBox3.Items(11).ToString + "     " + ListBox3.Items(12).ToString + "     " + ListBox3.Items(13).ToString + "     " + ListBox3.Items(14).ToString + "     " + ListBox3.Items(15).ToString + "     " + ListBox3.Items(16).ToString + "     " + ListBox3.Items(17).ToString)
            End If

            If ShowAwayFirstElevenCrawl.Checked = True Then
                CasparCGDataCollection.SetData("f0", AwayTeamName.Text + " TEAMSHEET:       " + ListBox4.Items(0).ToString + "      " + ListBox4.Items(1).ToString + "      " + ListBox4.Items(2).ToString + "      " + ListBox4.Items(3).ToString + "      " + ListBox4.Items(4).ToString + "      " + ListBox4.Items(5).ToString + "      " + ListBox4.Items(6).ToString + "      " + ListBox4.Items(7).ToString + "      " + ListBox4.Items(8).ToString + "      " + ListBox4.Items(9).ToString + "      " + ListBox4.Items(10).ToString + "     SUBSTITUTES:        " + ListBox4.Items(11).ToString + "     " + ListBox4.Items(12).ToString + "     " + ListBox4.Items(13).ToString + "     " + ListBox4.Items(14).ToString + "     " + ListBox4.Items(15).ToString + "     " + ListBox4.Items(16).ToString + "     " + ListBox4.Items(17).ToString)
            End If
            If Home1stElevenCrawler.Checked = True Then
                CasparCGDataCollection.SetData("f0", HomeTeamName.Text + " TEAMSHEET:       " + ListBox3.Items(0).ToString + "      " + ListBox3.Items(1).ToString + "      " + ListBox3.Items(2).ToString + "      " + ListBox3.Items(3).ToString + "      " + ListBox3.Items(4).ToString + "      " + ListBox3.Items(5).ToString + "      " + ListBox3.Items(6).ToString + "      " + ListBox3.Items(7).ToString + "      " + ListBox3.Items(8).ToString + "      " + ListBox3.Items(9).ToString + "      " + ListBox3.Items(10).ToString)
            End If

            If Away1stElevenCrawler.Checked = True Then
                CasparCGDataCollection.SetData("f0", AwayTeamName.Text + " TEAMSHEET:       " + ListBox4.Items(0).ToString + "      " + ListBox4.Items(1).ToString + "      " + ListBox4.Items(2).ToString + "      " + ListBox4.Items(3).ToString + "      " + ListBox4.Items(4).ToString + "      " + ListBox4.Items(5).ToString + "      " + ListBox4.Items(6).ToString + "      " + ListBox4.Items(7).ToString + "      " + ListBox4.Items(8).ToString + "      " + ListBox4.Items(9).ToString + "      " + ListBox4.Items(10).ToString)
            End If



            CasparDevice.Channels(0).CG.Add(101, "efc_TSheet_crawl_temp_LT", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(0).CG.Play(101)

            'fading in image
            CasparDevice.SendString("MIXER 1-100 OPACITY 0")
            CasparDevice.SendString("play 1-104 LT_crawl_crest")
            CasparDevice.SendString("MIXER 1-100 OPACITY 1 48 linear")


            'fading in image
            CasparDevice.SendString("MIXER 1-100 OPACITY 0")
            CasparDevice.SendString("play 1-100 LT_crawl_nocrest")
            CasparDevice.SendString("MIXER 1-100 OPACITY 1 48 linear")

            'CasparDevice.SendString("play 1-102 LT_crawl_crest")
            CasparDevice.SendString("play 1-103 LTFlare")
            TSCrawlOnBTN.BackColor = Color.Green
            'disable button
            ' TSCrawlOnBTN.Enabled = False
            crawlToggle = True
        End If
    End Sub

    Private Sub TSCrawlOffBTN_Click(sender As Object, e As EventArgs) Handles TSCrawlOffBTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(0).CG.Stop(101)
            CasparDevice.SendString("MIXER 1-100 OPACITY 0 24 linear")
            countBPS = 0
            BPlayChanFadeOut.Enabled = True
            CasparDevice.SendString("STOP 1-102")
            CasparDevice.SendString("STOP 1-103")
            CasparDevice.SendString("STOP 1-104")
            CrawlOn.BackColor = Color.FromKnownColor(KnownColor.Control)
            TSCrawlOnBTN.UseVisualStyleBackColor = True
            'crawlToggle = False
            'disable button
            ' TSCrawlOnBTN.Enabled = True

        End If
    End Sub

    Private Sub RemoveHomerScoreresLB_Click(sender As Object, e As EventArgs) Handles RemoveHomerScoreresLB.Click
        If Me.HomeScorers.SelectedIndex >= 0 Then
            HomeScorers.Items.Remove(HomeScorers.SelectedItem)
            HomeScore.Text = Convert.ToInt32(HomeScore.Text) - 1
        Else
            MessageBox.Show("You need to select a player to remove", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles RemoveAwayScoreresLB.Click
        If Me.awayScorers.SelectedIndex >= 0 Then
            awayScorers.Items.Remove(awayScorers.SelectedItem)
            AwayScore.Text = Convert.ToInt32(AwayScore.Text) - 1
        Else
            MessageBox.Show("You need to select a player to remove", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub EditHomeScorers_Click(sender As Object, e As EventArgs) Handles EditHomeScorers.Click
        If Me.HomeScorers.SelectedIndex >= 0 Then
            EditTextHomeScorers.Text = HomeScorers.SelectedItem
        Else
            MessageBox.Show("You need to select a player to edit", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub SendEditedHomeScorers_Click(sender As Object, e As EventArgs) Handles SendEditedHomeScorers.Click
        If Me.HomeScorers.SelectedIndex >= 0 Then
            'HomeScorers.SelectedItem = EditTextHomeScorers.Text

            Dim si As Integer = Me.HomeScorers.SelectedIndex

            Me.HomeScorers.Items.RemoveAt(si)
            Me.HomeScorers.Items.Insert(si, EditTextHomeScorers.Text)
        Else
            MessageBox.Show("You need to select a player to replace", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub EditAwayScorers_Click(sender As Object, e As EventArgs) Handles EditAwayScorers.Click
        If Me.awayScorers.SelectedIndex >= 0 Then
            EditTextAwayScorers.Text = awayScorers.SelectedItem
        Else
            MessageBox.Show("You need to select a player to edit", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub SendEditedAwayScorers_Click(sender As Object, e As EventArgs) Handles SendEditedAwayScorers.Click
        If Me.awayScorers.SelectedIndex >= 0 Then
            Dim si2 As Integer = Me.awayScorers.SelectedIndex

            Me.awayScorers.Items.RemoveAt(si2)
            Me.awayScorers.Items.Insert(si2, EditTextAwayScorers.Text)
        Else
            MessageBox.Show("You need to select a player to replace", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub addHomeSquad_Click(sender As Object, e As EventArgs) Handles addHomeSquad.Click
        If Me.FullHomeSquad.SelectedIndex >= 0 Then
            HomeTeam.Items.Add(FullHomeSquad.Text)
            FullHomeSquad.Items.Remove(FullHomeSquad.Text)

            homeTeamCount = homeTeamCount + 1
            If homeTeamCount <= 11 Then
                homePlayerCount.Text = homeTeamCount
            End If
            If homeTeamCount > 11 Then
                homeSubsCount.Text = homeTeamCount - 11
            End If
            'set colours
            If homeTeamCount > 18 Then
                homePlayerCount.ForeColor = Color.Red
                homeSubsCount.ForeColor = Color.Red
            End If
            If FullHomeSquad.Items.Count > 0 Then
                FullHomeSquad.SetSelected(0, True)
            End If
        End If
    End Sub

    Private Sub removeHomeSquad_Click(sender As Object, e As EventArgs) Handles removeHomeSquad.Click
        If Me.HomeTeam.SelectedIndex >= 0 Then
            FullHomeSquad.Items.Add(HomeTeam.Text)
            HomeTeam.Items.Remove(HomeTeam.Text)
            homeTeamCount = homeTeamCount - 1
            If homeTeamCount <= 11 Then
                homePlayerCount.Text = homeTeamCount
                homeSubsCount.Text = 0
            End If
            If homeTeamCount > 11 Then
                homePlayerCount.Text = 11
                homeSubsCount.Text = homeTeamCount - 11
            End If
            'homePlayerCount.Text = Convert.ToInt32(homePlayerCount.Text) - 1
            If homeTeamCount >= 18 Then
                homePlayerCount.ForeColor = Color.Green
                homeSubsCount.ForeColor = Color.RoyalBlue
            End If

        Else
            MessageBox.Show("You need to select a player to remove", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub HomeClearSquadList_Click(sender As Object, e As EventArgs) Handles HomeClearSquadList.Click
        HomeTeam.Items.Clear()
        homePlayerCount.Text = 0
        homeSubsCount.Text = 0
        homePlayerCount.ForeColor = Color.Green
        homeSubsCount.ForeColor = Color.RoyalBlue
        homeTeamCount = 0

        ' reload teams on home side
        ' Clear list boxes in case of reload
        FullHomeSquad.Items.Clear()
        SubOn.Items.Clear()
        SubOff.Items.Clear()
        Try
            ' Create an instance of StreamReader to read from a file. 
            Dim sr As StreamReader = New StreamReader("C:\home_team.txt", System.Text.Encoding.Default)
            Dim line As String
            'Read and display the lines from the file until the end 
            ' of the file is reached. 
            Do

                line = sr.ReadLine()
                If line <> "" Then
                    FullHomeSquad.Items.Add(UCase(line))
                End If
                ' SubOn.Items.Add(UCase(line))
                ' SubOff.Items.Add(UCase(line))
            Loop Until line Is Nothing
            sr.Close()
        Catch ex As Exception
            ' Let the user know what went wrong.
            Console.WriteLine("The file could not be read:")
            Console.WriteLine(ex.Message)
        End Try
        HomeTeamName.Text = FullHomeSquad.Items(0).ToString
        FullHomeSquad.Items.Remove(FullHomeSquad.Items(0))

    End Sub

    Private Sub addAwaySquad_Click(sender As Object, e As EventArgs) Handles addAwaySquad.Click
        If Me.FullAwaySquad.SelectedIndex >= 0 Then
            AwayTeam.Items.Add(FullAwaySquad.Text)
            FullAwaySquad.Items.Remove(FullAwaySquad.Text)
            'AwayPlayerCount.Text = Convert.ToInt32(AwayPlayerCount.Text) + 1
            awayTeamCount = awayTeamCount + 1
            If awayTeamCount <= 11 Then
                AwayPlayerCount.Text = awayTeamCount
            End If
            If awayTeamCount > 11 Then
                AwaySubsCount.Text = awayTeamCount - 11
            End If
            'set colours
            If awayTeamCount > 18 Then
                AwayPlayerCount.ForeColor = Color.Red
                AwaySubsCount.ForeColor = Color.Red
            End If

            If FullAwaySquad.Items.Count > 0 Then
                FullAwaySquad.SetSelected(0, True)
            End If
            'change colours
            If Convert.ToInt32(AwayPlayerCount.Text) > 18 Then
                AwayPlayerCount.ForeColor = Color.Red
            End If
            If FullAwaySquad.Items.Count > 0 Then
                FullAwaySquad.SetSelected(0, True)
            End If
        End If
    End Sub

    Private Sub removeAwaySquad_Click(sender As Object, e As EventArgs) Handles removeAwaySquad.Click
        If Me.AwayTeam.SelectedIndex >= 0 Then
            FullAwaySquad.Items.Add(AwayTeam.Text)
            AwayTeam.Items.Remove(AwayTeam.Text)
            'AwayPlayerCount.Text = Convert.ToInt32(AwayPlayerCount.Text) - 1
            awayTeamCount = awayTeamCount - 1
            If awayTeamCount <= 11 Then
                AwayPlayerCount.Text = awayTeamCount
                AwaySubsCount.Text = 0
            End If
            If awayTeamCount > 11 Then
                AwayPlayerCount.Text = 11
                AwaySubsCount.Text = awayTeamCount - 11
            End If

            If awayTeamCount <= 18 Then
                AwayPlayerCount.ForeColor = Color.Green
                AwaySubsCount.ForeColor = Color.RoyalBlue
            End If
        Else
            MessageBox.Show("You need to select a player to remove", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub clearAwaySquad_Click(sender As Object, e As EventArgs) Handles clearAwaySquad.Click
        AwayTeam.Items.Clear()
        AwayPlayerCount.Text = 0
        AwaySubsCount.Text = 0
        AwayPlayerCount.ForeColor = Color.Green
        AwaySubsCount.ForeColor = Color.RoyalBlue
        awayTeamCount = 0

        'clear boxes
        FullAwaySquad.Items.Clear()
        aw_subOn.Items.Clear()
        aw_subOff.Items.Clear()

        'reload
        Try
            ' Create an instance of StreamReader to read from a file. 
            Dim sr As StreamReader = New StreamReader("C:\away_team.txt", System.Text.Encoding.Default)
            Dim line As String
            'Read and display the lines from the file until the end 
            ' of the file is reached. 
            Do
                line = sr.ReadLine()
                If line <> "" Then
                    FullAwaySquad.Items.Add(UCase(line))
                End If
                'aw_subOn.Items.Add(UCase(line))
                ' aw_subOff.Items.Add(UCase(line))
            Loop Until line Is Nothing
            sr.Close()
        Catch ex As Exception
            ' Let the user know what went wrong.
            Console.WriteLine("The file could not be read:")
            Console.WriteLine(ex.Message)
        End Try
        AwayTeamName.Text = FullAwaySquad.Items(0).ToString
        FullAwaySquad.Items.Remove(FullAwaySquad.Items(0))


    End Sub

    Private Sub updateAllTeams_Click(sender As Object, e As EventArgs) Handles updateAllTeams.Click
        ListBox1.Items.Clear()
        ListBox3.Items.Clear()
        'ListBox1.Items.AddRange(HomeTeam.Items)

        SubOn.Items.Clear()
        SubOff.Items.Clear()

        If HomeTeam.Items.Count > 17 Then

            For i As Integer = 0 To HomeTeam.Items.Count - 1
                ListBox1.Items.Add(HomeTeam.Items(i))
                ListBox3.Items.Add(HomeTeam.Items(i))
                SubOn.Items.Add(HomeTeam.Items(i))
                SubOff.Items.Add(HomeTeam.Items(i))
            Next
        Else
            MessageBox.Show("You need to have 11 players and 7 subs in each team. Currently you don't have enough players in your home team.", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If


        ListBox2.Items.Clear()
        ListBox4.Items.Clear()
        'ListBox2.Items.AddRange(AwayTeam.Items)

        If AwayTeam.Items.Count > 17 Then
            aw_subOn.Items.Clear()
            aw_subOff.Items.Clear()
            For j As Integer = 0 To AwayTeam.Items.Count - 1
                ListBox2.Items.Add(AwayTeam.Items(j))
                ListBox4.Items.Add(AwayTeam.Items(j))
                aw_subOn.Items.Add(AwayTeam.Items(j))
                aw_subOff.Items.Add(AwayTeam.Items(j))
            Next
        Else
            MessageBox.Show("You need to have 11 players and 7 subs in each team. Currently you don't have enough players in your away team.", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If


    End Sub


    Private Sub scoresFadeOut_Tick(sender As Object, e As EventArgs) Handles scoresFadeOut.Tick
        countScores = countScores + 1
        If countScores >= 10 Then
            CasparDevice.SendString("stop 2-100")
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 0 linear")

            CasparDevice.SendString("stop 2-104")
            CasparDevice.SendString("MIXER 2-104 OPACITY 1 0 linear")

            CasparDevice.SendString("stop 2-105")
            CasparDevice.SendString("MIXER 2-105 OPACITY 1 0 linear")

            CasparDevice.SendString("stop 2-106")
            CasparDevice.SendString("MIXER 2-106 OPACITY 1 0 linear")

            CasparDevice.SendString("stop 2-107")
            CasparDevice.SendString("MIXER 2-107 OPACITY 1 0 linear")

            CasparDevice.SendString("stop 2-108")
            CasparDevice.SendString("MIXER 2-108 OPACITY 1 0 linear")

            CasparDevice.SendString("stop 2-109")
            CasparDevice.SendString("MIXER 2-109 OPACITY 1 0 linear")
            scoresFadeOut.Enabled = False
            countScores = 0
        End If
    End Sub

    Private Sub AddtoSquadNotListed_Click(sender As Object, e As EventArgs) Handles AddtoSquadNotListed.Click
        If HomePlayerNotListed.Text <> "" Then


            FullHomeSquad.Items.Add(UCase(HomePlayerNotListed.Text))
            'homePlayerCount.Text = Convert.ToInt32(homePlayerCount.Text) + 1
            'If Convert.ToInt32(homePlayerCount.Text) > 18 Then
            'homePlayerCount.ForeColor = Color.Red
            ' End If

            'save to text file
            '       Dim FileNumber As Integer = FreeFile()
            '   FileOpen(FileNumber, "c:\home_team.txt", OpenMode.Output)
            '    PrintLine(FileNumber, HomeTeamName.Text)
            ' '    For Each Item As Object In FullHomeSquad.Items
            'PrintLine(FileNumber, Item.ToString)
            '     Next
            '     FileClose(FileNumber)


            Dim FILE_NAME As String = "c:\home_team.txt"
            Dim objWriter As New System.IO.StreamWriter(FILE_NAME, True)
            objWriter.WriteLine(HomePlayerNotListed.Text)
            objWriter.Close()

        Else
            MessageBox.Show("You need to type a name in here", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub AddtoSqaudNotListedAway_Click(sender As Object, e As EventArgs) Handles AddtoSqaudNotListedAway.Click
        If AwayPlayerNotListed.Text <> "" Then
            FullAwaySquad.Items.Add(UCase(AwayPlayerNotListed.Text))
            'AwayPlayerCount.Text = Convert.ToInt32(AwayPlayerCount.Text) + 1
            ' If Convert.ToInt32(AwayPlayerCount.Text) > 18 Then
            'AwayPlayerCount.ForeColor = Color.Red
            'End If

            'save to text file
            ' Dim FileNumber As Integer = FreeFile()
            ' FileOpen(FileNumber, "c:\away_team.txt", OpenMode.Output)
            ' PrintLine(FileNumber, AwayTeamName.Text)
            ' For Each Item As Object In FullAwaySquad.Items
            ' PrintLine(FileNumber, Item.ToString)
            ' Next
            ' FileClose(FileNumber)

            Dim FILE_NAME As String = "c:\away_team.txt"
            Dim objWriter As New System.IO.StreamWriter(FILE_NAME, True)
            objWriter.WriteLine(AwayPlayerNotListed.Text)
            objWriter.Close()

        Else
            MessageBox.Show("You need to type a name in here", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = False Then
            ChampScore1.Enabled = False
            ChampScore1.Text = ""
            ChampScore2.Enabled = False
            ChampScore2.Text = ""
            ChampScore3.Enabled = False
            ChampScore3.Text = ""
            ChampScore4.Enabled = False
            ChampScore4.Text = ""
            middle1.Enabled = False
            middle1.Text = ""
        End If
        If CheckBox1.Checked = True Then
            ChampScore1.Enabled = True
            ChampScore2.Enabled = True
            ChampScore3.Enabled = True
            ChampScore4.Enabled = True
            middle1.Enabled = True
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = False Then
            ChampScore5.Enabled = False
            ChampScore5.Text = ""
            ChampScore6.Enabled = False
            ChampScore6.Text = ""
            ChampScore7.Enabled = False
            ChampScore7.Text = ""
            ChampScore8.Enabled = False
            ChampScore8.Text = ""
            middle2.Enabled = False
            middle2.Text = ""
        End If
        If CheckBox2.Checked = True Then
            ChampScore5.Enabled = True
            ChampScore6.Enabled = True
            ChampScore7.Enabled = True
            ChampScore8.Enabled = True
            middle2.Enabled = True
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = False Then
            ChampScore9.Enabled = False
            ChampScore9.Text = ""
            ChampScore10.Enabled = False
            ChampScore10.Text = ""
            ChampScore11.Enabled = False
            ChampScore11.Text = ""
            ChampScore12.Enabled = False
            ChampScore12.Text = ""
            middle3.Enabled = False
            middle3.Text = ""
        End If
        If CheckBox3.Checked = True Then
            ChampScore9.Enabled = True
            ChampScore10.Enabled = True
            ChampScore11.Enabled = True
            ChampScore12.Enabled = True
            middle3.Enabled = True
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = False Then
            ChampScore13.Enabled = False
            ChampScore13.Text = ""
            ChampScore14.Enabled = False
            ChampScore14.Text = ""
            ChampScore15.Enabled = False
            ChampScore15.Text = ""
            ChampScore16.Enabled = False
            ChampScore16.Text = ""
            middle4.Enabled = False
            middle4.Text = ""
        End If
        If CheckBox4.Checked = True Then
            ChampScore13.Enabled = True
            ChampScore14.Enabled = True
            ChampScore15.Enabled = True
            ChampScore16.Enabled = True
            middle4.Enabled = True
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = False Then
            ChampScore17.Enabled = False
            ChampScore17.Text = ""
            ChampScore18.Enabled = False
            ChampScore18.Text = ""
            ChampScore19.Enabled = False
            ChampScore19.Text = ""
            ChampScore20.Enabled = False
            ChampScore20.Text = ""
            middle5.Enabled = False
            middle5.Text = ""
        End If
        If CheckBox5.Checked = True Then
            ChampScore17.Enabled = True
            ChampScore18.Enabled = True
            ChampScore19.Enabled = True
            ChampScore20.Enabled = True
            middle5.Enabled = True
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked = False Then
            ChampScore21.Enabled = False
            ChampScore21.Text = ""
            ChampScore22.Enabled = False
            ChampScore22.Text = ""
            ChampScore23.Enabled = False
            ChampScore23.Text = ""
            ChampScore24.Enabled = False
            ChampScore24.Text = ""
            middle6.Enabled = False
            middle6.Text = ""
        End If
        If CheckBox6.Checked = True Then
            ChampScore21.Enabled = True
            ChampScore22.Enabled = True
            ChampScore23.Enabled = True
            ChampScore24.Enabled = True
            middle6.Enabled = True
        End If
    End Sub

    Private Sub CheckBox12_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox12.CheckedChanged
        If CheckBox12.Checked = False Then
            ChampScore25.Enabled = False
            ChampScore25.Text = ""
            ChampScore26.Enabled = False
            ChampScore26.Text = ""
            ChampScore27.Enabled = False
            ChampScore27.Text = ""
            ChampScore28.Enabled = False
            ChampScore28.Text = ""
            middle7.Enabled = False
            middle7.Text = ""
        End If
        If CheckBox12.Checked = True Then
            ChampScore25.Enabled = True
            ChampScore26.Enabled = True
            ChampScore27.Enabled = True
            ChampScore28.Enabled = True
            middle7.Enabled = True
        End If

    End Sub

    Private Sub CheckBox11_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox11.CheckedChanged
        If CheckBox11.Checked = False Then
            ChampScore29.Enabled = False
            ChampScore29.Text = ""
            ChampScore30.Enabled = False
            ChampScore30.Text = ""
            ChampScore31.Enabled = False
            ChampScore31.Text = ""
            ChampScore32.Enabled = False
            ChampScore32.Text = ""
            middle8.Enabled = False
            middle8.Text = ""
        End If
        If CheckBox11.Checked = True Then
            ChampScore29.Enabled = True
            ChampScore30.Enabled = True
            ChampScore31.Enabled = True
            ChampScore32.Enabled = True
            middle8.Enabled = True
        End If
    End Sub

    Private Sub CheckBox10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked = False Then
            ChampScore33.Enabled = False
            ChampScore33.Text = ""
            ChampScore34.Enabled = False
            ChampScore34.Text = ""
            ChampScore35.Enabled = False
            ChampScore35.Text = ""
            ChampScore36.Enabled = False
            ChampScore36.Text = ""
            middle9.Enabled = False
            middle9.Text = ""
        End If
        If CheckBox10.Checked = True Then
            ChampScore33.Enabled = True
            ChampScore34.Enabled = True
            ChampScore35.Enabled = True
            ChampScore36.Enabled = True
            middle9.Enabled = True
        End If
    End Sub

    Private Sub CheckBox9_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked = False Then
            ChampScore37.Enabled = False
            ChampScore37.Text = ""
            ChampScore38.Enabled = False
            ChampScore38.Text = ""
            ChampScore39.Enabled = False
            ChampScore39.Text = ""
            ChampScore40.Enabled = False
            ChampScore40.Text = ""
            middle10.Enabled = False
            middle10.Text = ""
        End If
        If CheckBox9.Checked = True Then
            ChampScore37.Enabled = True
            ChampScore38.Enabled = True
            ChampScore39.Enabled = True
            ChampScore40.Enabled = True
            middle10.Enabled = True
        End If
    End Sub

    Private Sub CheckBox8_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked = False Then
            ChampScore41.Enabled = False
            ChampScore41.Text = ""
            ChampScore42.Enabled = False
            ChampScore42.Text = ""
            ChampScore43.Enabled = False
            ChampScore43.Text = ""
            ChampScore44.Enabled = False
            ChampScore44.Text = ""
            middle11.Enabled = False
            middle11.Text = ""
        End If
        If CheckBox8.Checked = True Then
            ChampScore41.Enabled = True
            ChampScore42.Enabled = True
            ChampScore43.Enabled = True
            ChampScore44.Enabled = True
            middle11.Enabled = True
        End If
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        If CheckBox7.Checked = False Then
            ChampScore45.Enabled = False
            ChampScore45.Text = ""
            ChampScore46.Enabled = False
            ChampScore46.Text = ""
            ChampScore47.Enabled = False
            ChampScore47.Text = ""
            ChampScore48.Enabled = False
            ChampScore48.Text = ""
            middle12.Enabled = False
            middle12.Text = ""
        End If
        If CheckBox7.Checked = True Then
            ChampScore45.Enabled = True
            ChampScore46.Enabled = True
            ChampScore47.Enabled = True
            ChampScore48.Enabled = True
            middle12.Enabled = True
        End If
    End Sub

    Private Sub CheckBox13_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox13.CheckedChanged
        If CheckBox13.Checked = False Then
            Score1.Enabled = False
            Score1.Text = ""
            Score2.Enabled = False
            Score2.Text = ""
            Score3.Enabled = False
            Score3.Text = ""
            Score4.Enabled = False
            Score4.Text = ""
            middle13.Enabled = False
            middle13.Text = ""
        End If
        If CheckBox13.Checked = True Then
            Score1.Enabled = True
            Score2.Enabled = True
            Score3.Enabled = True
            Score4.Enabled = True
            middle13.Enabled = True
        End If
    End Sub

    Private Sub CheckBox14_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox14.CheckedChanged
        If CheckBox14.Checked = False Then
            Score5.Enabled = False
            Score5.Text = ""
            Score6.Enabled = False
            Score6.Text = ""
            Score7.Enabled = False
            Score7.Text = ""
            Score8.Enabled = False
            Score8.Text = ""
            middle14.Enabled = False
            middle14.Text = ""
        End If
        If CheckBox14.Checked = True Then
            Score5.Enabled = True
            Score6.Enabled = True
            Score7.Enabled = True
            Score8.Enabled = True
            middle14.Enabled = True
        End If
    End Sub

    Private Sub CheckBox15_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox15.CheckedChanged
        If CheckBox15.Checked = False Then
            Score9.Enabled = False
            Score9.Text = ""
            Score10.Enabled = False
            Score10.Text = ""
            Score11.Enabled = False
            Score11.Text = ""
            Score12.Enabled = False
            Score12.Text = ""
            middle15.Enabled = False
            middle15.Text = ""
        End If
        If CheckBox15.Checked = True Then
            Score9.Enabled = True
            Score10.Enabled = True
            Score11.Enabled = True
            Score12.Enabled = True
            middle15.Enabled = True
        End If
    End Sub

    Private Sub CheckBox16_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox16.CheckedChanged
        If CheckBox16.Checked = False Then
            Score13.Enabled = False
            Score13.Text = ""
            Score14.Enabled = False
            Score14.Text = ""
            Score15.Enabled = False
            Score15.Text = ""
            Score16.Enabled = False
            Score16.Text = ""
            middle16.Enabled = False
            middle16.Text = ""
        End If
        If CheckBox16.Checked = True Then
            Score13.Enabled = True
            Score14.Enabled = True
            Score15.Enabled = True
            Score16.Enabled = True
            middle16.Enabled = True
        End If
    End Sub

    Private Sub CheckBox17_CheckedChanged_1(sender As Object, e As EventArgs) Handles CheckBox17.CheckedChanged
        If CheckBox17.Checked = False Then
            Score17.Enabled = False
            Score17.Text = ""
            Score18.Enabled = False
            Score18.Text = ""
            Score19.Enabled = False
            Score19.Text = ""
            Score20.Enabled = False
            Score20.Text = ""
            middle17.Enabled = False
            middle17.Text = ""
        End If
        If CheckBox17.Checked = True Then
            Score17.Enabled = True
            Score18.Enabled = True
            Score19.Enabled = True
            Score20.Enabled = True
            middle17.Enabled = True
        End If
    End Sub

    Private Sub CheckBox18_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox18.CheckedChanged
        If CheckBox18.Checked = False Then
            Score21.Enabled = False
            Score21.Text = ""
            Score22.Enabled = False
            Score22.Text = ""
            Score23.Enabled = False
            Score23.Text = ""
            Score24.Enabled = False
            Score24.Text = ""
            middle18.Enabled = False
            middle18.Text = ""
        End If
        If CheckBox18.Checked = True Then
            Score21.Enabled = True
            Score22.Enabled = True
            Score23.Enabled = True
            Score24.Enabled = True
            middle18.Enabled = True
        End If
    End Sub

    Private Sub CheckBox19_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox19.CheckedChanged
        If CheckBox19.Checked = False Then
            Score25.Enabled = False
            Score25.Text = ""
            Score26.Enabled = False
            Score26.Text = ""
            Score27.Enabled = False
            Score27.Text = ""
            Score28.Enabled = False
            Score28.Text = ""
            middle19.Enabled = False
            middle19.Text = ""
        End If
        If CheckBox19.Checked = True Then
            Score25.Enabled = True
            Score26.Enabled = True
            Score27.Enabled = True
            Score28.Enabled = True
            middle19.Enabled = True
        End If
    End Sub

    Private Sub CheckBox20_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox20.CheckedChanged
        If CheckBox20.Checked = False Then
            Score29.Enabled = False
            Score29.Text = ""
            Score30.Enabled = False
            Score30.Text = ""
            Score31.Enabled = False
            Score31.Text = ""
            Score32.Enabled = False
            Score32.Text = ""
            middle20.Enabled = False
            middle20.Text = ""
        End If
        If CheckBox20.Checked = True Then
            Score29.Enabled = True
            Score30.Enabled = True
            Score31.Enabled = True
            Score32.Enabled = True
            middle20.Enabled = True
        End If
    End Sub

    Private Sub CheckBox21_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox21.CheckedChanged
        If CheckBox21.Checked = False Then
            Score33.Enabled = False
            Score33.Text = ""
            Score34.Enabled = False
            Score34.Text = ""
            Score35.Enabled = False
            Score35.Text = ""
            Score36.Enabled = False
            Score36.Text = ""
            middle21.Enabled = False
            middle21.Text = ""
        End If
        If CheckBox21.Checked = True Then
            Score33.Enabled = True
            Score34.Enabled = True
            Score35.Enabled = True
            Score36.Enabled = True
            middle21.Enabled = True
        End If
    End Sub

    Private Sub CheckBox22_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox22.CheckedChanged
        If CheckBox22.Checked = False Then
            Score37.Enabled = False
            Score37.Text = ""
            Score38.Enabled = False
            Score38.Text = ""
            Score39.Enabled = False
            Score39.Text = ""
            Score40.Enabled = False
            Score40.Text = ""
            middle22.Enabled = False
            middle22.Text = ""
        End If
        If CheckBox22.Checked = True Then
            Score37.Enabled = True
            Score38.Enabled = True
            Score39.Enabled = True
            Score40.Enabled = True
            middle22.Enabled = True
        End If
    End Sub

    Private Sub CheckBox23_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox23.CheckedChanged
        If CheckBox23.Checked = False Then
            Score41.Enabled = False
            Score41.Text = ""
            Score42.Enabled = False
            Score42.Text = ""
            Score43.Enabled = False
            Score43.Text = ""
            Score44.Enabled = False
            Score44.Text = ""
            middle23.Enabled = False
            middle23.Text = ""
        End If
        If CheckBox23.Checked = True Then
            Score41.Enabled = True
            Score42.Enabled = True
            Score43.Enabled = True
            Score44.Enabled = True
            middle23.Enabled = True
        End If
    End Sub

    Private Sub CheckBox24_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox24.CheckedChanged
        If CheckBox24.Checked = False Then
            Score45.Enabled = False
            Score45.Text = ""
            Score46.Enabled = False
            Score46.Text = ""
            Score47.Enabled = False
            Score47.Text = ""
            Score48.Enabled = False
            Score48.Text = ""
            middle24.Enabled = False
            middle24.Text = ""
        End If
        If CheckBox24.Checked = True Then
            Score45.Enabled = True
            Score46.Enabled = True
            Score47.Enabled = True
            Score48.Enabled = True
            middle24.Enabled = True
        End If
    End Sub

    Private Sub HomeMoveUp_Click(sender As Object, e As EventArgs) Handles HomeMoveUp.Click
        'Make sure our item is not the first one on the list.
        If HomeTeam.SelectedIndex > 0 Then
            Dim I = HomeTeam.SelectedIndex - 1
            HomeTeam.Items.Insert(I, HomeTeam.SelectedItem)
            HomeTeam.Items.RemoveAt(HomeTeam.SelectedIndex)
            HomeTeam.SelectedIndex = I
        End If
    End Sub

    Private Sub HomeDown_Click(sender As Object, e As EventArgs) Handles HomeDown.Click
        'Make sure our item is not the last one on the list.
        If HomeTeam.SelectedIndex < HomeTeam.Items.Count - 1 Then
            'Insert places items above the index you supply, since we want
            'to move it down the list we have to do + 2
            Dim I = HomeTeam.SelectedIndex + 2
            HomeTeam.Items.Insert(I, HomeTeam.SelectedItem)
            HomeTeam.Items.RemoveAt(HomeTeam.SelectedIndex)
            HomeTeam.SelectedIndex = I - 1
        End If
    End Sub

    Private Sub AwayMoveUp_Click(sender As Object, e As EventArgs) Handles AwayMoveUp.Click
        'Make sure our item is not the first one on the list.
        If AwayTeam.SelectedIndex > 0 Then
            Dim I = AwayTeam.SelectedIndex - 1
            AwayTeam.Items.Insert(I, AwayTeam.SelectedItem)
            AwayTeam.Items.RemoveAt(AwayTeam.SelectedIndex)
            AwayTeam.SelectedIndex = I
        End If
    End Sub

    Private Sub AwayMoveDown_Click(sender As Object, e As EventArgs) Handles AwayMoveDown.Click
        'Make sure our item is not the last one on the list.
        If AwayTeam.SelectedIndex < AwayTeam.Items.Count - 1 Then
            'Insert places items above the index you supply, since we want
            'to move it down the list we have to do + 2
            Dim I = AwayTeam.SelectedIndex + 2
            AwayTeam.Items.Insert(I, AwayTeam.SelectedItem)
            AwayTeam.Items.RemoveAt(AwayTeam.SelectedIndex)
            AwayTeam.SelectedIndex = I - 1
        End If
    End Sub

    Private Sub PL1MoveUp_Click(sender As Object, e As EventArgs) Handles PL1MoveUp.Click
        'Make sure our item is not the first one on the list.
        If playlistFiles.SelectedIndex > 0 Then
            Dim I = playlistFiles.SelectedIndex - 1
            playlistFiles.Items.Insert(I, playlistFiles.SelectedItem)
            playlistFiles.Items.RemoveAt(playlistFiles.SelectedIndex)
            playlistFiles.SelectedIndex = I
        End If
    End Sub

    Private Sub PL1MoveDown_Click(sender As Object, e As EventArgs) Handles PL1MoveDown.Click
        'Make sure our item is not the last one on the list.
        If playlistFiles.SelectedIndex < playlistFiles.Items.Count - 1 Then
            'Insert places items above the index you supply, since we want
            'to move it down the list we have to do + 2
            Dim I = playlistFiles.SelectedIndex + 2
            playlistFiles.Items.Insert(I, playlistFiles.SelectedItem)
            playlistFiles.Items.RemoveAt(playlistFiles.SelectedIndex)
            playlistFiles.SelectedIndex = I - 1
        End If
    End Sub

    Private Sub PLInGameMoveUp_Click(sender As Object, e As EventArgs) Handles PLInGameMoveUp.Click
        'Make sure our item is not the first one on the list.
        If playlistFilesInGame.SelectedIndex > 0 Then
            Dim I = playlistFilesInGame.SelectedIndex - 1
            playlistFilesInGame.Items.Insert(I, playlistFilesInGame.SelectedItem)
            playlistFilesInGame.Items.RemoveAt(playlistFilesInGame.SelectedIndex)
            playlistFilesInGame.SelectedIndex = I
        End If
    End Sub

    Private Sub PLInGameMoveDown_Click(sender As Object, e As EventArgs) Handles PLInGameMoveDown.Click
        'Make sure our item is not the last one on the list.
        If playlistFilesInGame.SelectedIndex < playlistFilesInGame.Items.Count - 1 Then
            'Insert places items above the index you supply, since we want
            'to move it down the list we have to do + 2
            Dim I = playlistFilesInGame.SelectedIndex + 2
            playlistFilesInGame.Items.Insert(I, playlistFilesInGame.SelectedItem)
            playlistFilesInGame.Items.RemoveAt(playlistFilesInGame.SelectedIndex)
            playlistFilesInGame.SelectedIndex = I - 1
        End If
    End Sub

    Private Sub goalHome_Click_1(sender As Object, e As EventArgs) Handles goalHome.Click
        If Me.ListBox3.SelectedIndex >= 0 Then
            HomeScore.Text = Convert.ToInt32(HomeScore.Text) + 1
            Dim HomeScorerConvert = Convert.ToString(ListBox3.SelectedItem)
            ' remove numbers
            Dim NewHomeScorer As String = HomeScorerConvert.Remove(0, 2)
            'remove white space
            Dim TrimmedNewHomeScorer As String = NewHomeScorer.Trim()
            'get goal time
            Dim GoalTime As String = Convert.ToInt32(min.Text) + 1
            'make sure if goal is after clocks stopped it shows time of end of clock, not clock plus 1
            If (Convert.ToInt32(GoalTime) >= Convert.ToInt32(stopClockTime.Text)) Then
                GoalTime = Convert.ToString(stopClockTime.Text)
            End If
            'homeScorers.Text = homeScorers.Text + TrimmedNewHomeScorer + "    " + GoalTime + "'" + Environment.NewLine
            HomeScorers.Items.Add(TrimmedNewHomeScorer + "    " + GoalTime + "'")

            'update score bug
            CasparCGDataCollection.Clear()
            CasparCGDataCollection.SetData("f2", HomeScore.Text)
            CasparCGDataCollection.SetData("f3", AwayScore.Text)
            Me.CasparDevice.Channels(0).CG.Update(402, CasparCGDataCollection)
        Else
            MessageBox.Show("You need to select a player to score", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub goalAway_Click(sender As Object, e As EventArgs) Handles goalAway.Click
        If Me.ListBox4.SelectedIndex >= 0 Then
            AwayScore.Text = Convert.ToInt32(AwayScore.Text) + 1
            Dim AwayScorerConvert = Convert.ToString(ListBox4.SelectedItem)
            ' remove numbers
            Dim NewAwayScorer As String = AwayScorerConvert.Remove(0, 2)
            'remove white space
            Dim TrimmedNewAwayScorer As String = NewAwayScorer.Trim()
            'get goal time
            Dim GoalTime As String = Convert.ToInt32(min.Text) + 1
            'make sure if goal is after clocks stopped it shows time of end of clock, not clock plus 1
            If (Convert.ToInt32(GoalTime) >= Convert.ToInt32(stopClockTime.Text)) Then
                GoalTime = Convert.ToString(stopClockTime.Text)
            End If
            'awayScorers.Text = awayScorers.Text + GoalTime + "'    " + TrimmedNewAwayScorer + Environment.NewLine
            awayScorers.Items.Add(GoalTime + "'" + "    " + TrimmedNewAwayScorer)
            'update score bug
            CasparCGDataCollection.Clear()
            CasparCGDataCollection.SetData("f2", HomeScore.Text)
            CasparCGDataCollection.SetData("f3", AwayScore.Text)
            Me.CasparDevice.Channels(0).CG.Update(402, CasparCGDataCollection)
        Else
            MessageBox.Show("You need to select a player to score", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub HomeOwnGoalBTN_Click(sender As Object, e As EventArgs) Handles HomeOwnGoalBTN.Click
        If Me.ListBox3.SelectedIndex >= 0 Then
            AwayScore.Text = Convert.ToInt32(AwayScore.Text) + 1
            Dim AwayScorerConvert = Convert.ToString(ListBox3.SelectedItem)
            ' remove numbers
            Dim NewAwayScorer As String = AwayScorerConvert.Remove(0, 2)
            'remove white space
            Dim TrimmedNewAwayScorer As String = NewAwayScorer.Trim()
            'get goal time
            Dim GoalTime As String = Convert.ToInt32(min.Text) + 1
            'make sure if goal is after clocks stopped it shows time of end of clock, not clock plus 1
            If (Convert.ToInt32(GoalTime) >= Convert.ToInt32(stopClockTime.Text)) Then
                GoalTime = Convert.ToString(stopClockTime.Text)
            End If
            'homeScorers.Text = homeScorers.Text + TrimmedNewHomeScorer + "    " + GoalTime + "'" + Environment.NewLine
            awayScorers.Items.Add(GoalTime + "'" + "    " + TrimmedNewAwayScorer + " (OG)")
            'update score bug
            CasparCGDataCollection.Clear()
            CasparCGDataCollection.SetData("f2", HomeScore.Text)
            CasparCGDataCollection.SetData("f3", AwayScore.Text)
            Me.CasparDevice.Channels(0).CG.Update(402, CasparCGDataCollection)
        Else
            MessageBox.Show("You need to select a player to score", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub AwayHomeGoalBTN_Click(sender As Object, e As EventArgs) Handles AwayHomeGoalBTN.Click
        If Me.ListBox4.SelectedIndex >= 0 Then
            HomeScore.Text = Convert.ToInt32(HomeScore.Text) + 1
            Dim AwayScorerConvert = Convert.ToString(ListBox4.SelectedItem)
            ' remove numbers
            Dim NewAwayScorer As String = AwayScorerConvert.Remove(0, 2)
            'remove white space
            Dim TrimmedNewAwayScorer As String = NewAwayScorer.Trim()
            'get goal time
            Dim GoalTime As String = Convert.ToInt32(min.Text) + 1
            'make sure if goal is after clocks stopped it shows time of end of clock, not clock plus 1
            If (Convert.ToInt32(GoalTime) >= Convert.ToInt32(stopClockTime.Text)) Then
                GoalTime = Convert.ToString(stopClockTime.Text)
            End If
            'homeScorers.Text = homeScorers.Text + TrimmedNewHomeScorer + "    " + GoalTime + "'" + Environment.NewLine
            HomeScorers.Items.Add("(OG) " + TrimmedNewAwayScorer + "    " + GoalTime + "'")
            'update score bug
            CasparCGDataCollection.Clear()
            CasparCGDataCollection.SetData("f2", HomeScore.Text)
            CasparCGDataCollection.SetData("f3", AwayScore.Text)
            Me.CasparDevice.Channels(0).CG.Update(402, CasparCGDataCollection)
        Else
            MessageBox.Show("You need to select a player to score", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

  
    Private Sub unknownGoalHome_Click(sender As Object, e As EventArgs) Handles unknownGoalHome.Click
        HomeScore.Text = Convert.ToInt32(HomeScore.Text) + 1

        'get goal time
        Dim GoalTime As String = Convert.ToInt32(min.Text) + 1
        'make sure if goal is after clocks stopped it shows time of end of clock, not clock plus 1
        If (Convert.ToInt32(GoalTime) >= Convert.ToInt32(stopClockTime.Text)) Then
            GoalTime = Convert.ToString(stopClockTime.Text)
        End If
        'homeScorers.Text = homeScorers.Text + TrimmedNewHomeScorer + "    " + GoalTime + "'" + Environment.NewLine
        HomeScorers.Items.Add("A. PLAYER    " + GoalTime + "'")
        'update score bug
        CasparCGDataCollection.Clear()
        CasparCGDataCollection.SetData("f2", HomeScore.Text)
        CasparCGDataCollection.SetData("f3", AwayScore.Text)
        Me.CasparDevice.Channels(0).CG.Update(402, CasparCGDataCollection)
    End Sub

    Private Sub unknownGoalAway_Click(sender As Object, e As EventArgs) Handles unknownGoalAway.Click
        AwayScore.Text = Convert.ToInt32(AwayScore.Text) + 1
        'get goal time
        Dim GoalTime As String = Convert.ToInt32(min.Text) + 1
        'make sure if goal is after clocks stopped it shows time of end of clock, not clock plus 1
        If (Convert.ToInt32(GoalTime) >= Convert.ToInt32(stopClockTime.Text)) Then
            GoalTime = Convert.ToString(stopClockTime.Text)
        End If
        'awayScorers.Text = awayScorers.Text + GoalTime + "'    " + TrimmedNewAwayScorer + Environment.NewLine
        awayScorers.Items.Add(GoalTime + "'" + "    A. PLAYER")
        'update score bug
        CasparCGDataCollection.Clear()
        CasparCGDataCollection.SetData("f2", HomeScore.Text)
        CasparCGDataCollection.SetData("f3", AwayScore.Text)
        Me.CasparDevice.Channels(0).CG.Update(402, CasparCGDataCollection)
    End Sub

    Private Sub ReloadBackgroundsComboBx_Click(sender As Object, e As EventArgs) Handles ReloadBackgroundsComboBx.Click
        If Me.CasparDevice.IsConnected = True Then
            Dim File As Svt.Caspar.MediaInfo
            CasparDevice.RefreshMediafiles()
            'Clear list box in case of reload
            backgrounds1.Items.Clear()
            Threading.Thread.Sleep(250)

            For Each File In CasparDevice.Mediafiles
                backgrounds1.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
            Next
        End If
    End Sub


    Private Sub msg1OnBtn_Click(sender As Object, e As EventArgs) Handles msg1OnBtn.Click
        If Me.CasparDevice.IsConnected = True Then

            CasparCGDataCollection.Clear() 'cgData.Clear()
            CasparCGDataCollection.SetData("f0", msg1Title.Text)
            CasparCGDataCollection.SetData("f1", msg1Line1.Text)
            CasparCGDataCollection.SetData("f2", msg1Line2.Text)
            CasparCGDataCollection.SetData("f3", msg1Line3.Text)
            CasparCGDataCollection.SetData("f4", msg1Line4.Text)
            CasparCGDataCollection.SetData("col1", "0x" + msg1Colour)

            CasparDevice.Channels(1).CG.Add(101, "generalMessage", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 " & backgrounds1.Text)
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")

            msg1OnBtn.BackColor = Color.Green
            'disable button
            msg1OnBtn.Enabled = False
        End If
    End Sub

    Private Sub msg1OffBtn_Click(sender As Object, e As EventArgs) Handles msg1OffBtn.Click
        If Me.CasparDevice.IsConnected = True Then
            'fade off image in background
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            'then fade out the rest and set buttons
            CasparDevice.Channels(1).CG.Stop(101)
            msg1OnBtn.UseVisualStyleBackColor = True
            msg2OnBtn.UseVisualStyleBackColor = True
            msg3OnBtn.UseVisualStyleBackColor = True
            msg4OnBtn.UseVisualStyleBackColor = True
            msg5OnBtn.UseVisualStyleBackColor = True
            msg6OnBtn.UseVisualStyleBackColor = True
            msg7OnBtn.UseVisualStyleBackColor = True
            msg8OnBtn.UseVisualStyleBackColor = True
            'reenable button
            msg1OnBtn.Enabled = True
            msg2OnBtn.Enabled = True
            msg3OnBtn.Enabled = True
            msg4OnBtn.Enabled = True
            msg5OnBtn.Enabled = True
            msg6OnBtn.Enabled = True
            msg7OnBtn.Enabled = True
            msg8OnBtn.Enabled = True
        End If
    End Sub

    Private Sub ReloadBackgroundsComboBx2_Click(sender As Object, e As EventArgs) Handles ReloadBackgroundsComboBx2.Click
        If Me.CasparDevice.IsConnected = True Then
            Dim File As Svt.Caspar.MediaInfo
            CasparDevice.RefreshMediafiles()
            'Clear list box in case of reload
            backgrounds2.Items.Clear()
            Threading.Thread.Sleep(250)

            For Each File In CasparDevice.Mediafiles
                backgrounds2.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
            Next
        End If
    End Sub

    Private Sub ReloadBackgroundsComboBx3_Click(sender As Object, e As EventArgs) Handles ReloadBackgroundsComboBx3.Click
        If Me.CasparDevice.IsConnected = True Then
            Dim File As Svt.Caspar.MediaInfo
            CasparDevice.RefreshMediafiles()
            'Clear list box in case of reload
            backgrounds3.Items.Clear()
            Threading.Thread.Sleep(250)

            For Each File In CasparDevice.Mediafiles
                backgrounds3.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
            Next
        End If
    End Sub

    Private Sub ReloadBackgroundsComboBx4_Click(sender As Object, e As EventArgs) Handles ReloadBackgroundsComboBx4.Click
        If Me.CasparDevice.IsConnected = True Then
            Dim File As Svt.Caspar.MediaInfo
            CasparDevice.RefreshMediafiles()
            'Clear list box in case of reload
            backgrounds4.Items.Clear()
            Threading.Thread.Sleep(250)

            For Each File In CasparDevice.Mediafiles
                backgrounds4.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
            Next
        End If
    End Sub

    Private Sub msg2OnBtn_Click(sender As Object, e As EventArgs) Handles msg2OnBtn.Click
        If Me.CasparDevice.IsConnected = True Then

            CasparCGDataCollection.Clear() 'cgData.Clear()
            CasparCGDataCollection.SetData("f0", msg2Title.Text)
            CasparCGDataCollection.SetData("f1", msg2Line1.Text)
            CasparCGDataCollection.SetData("f2", msg2Line2.Text)
            CasparCGDataCollection.SetData("f3", msg2Line3.Text)
            CasparCGDataCollection.SetData("f4", msg2Line4.Text)
            CasparCGDataCollection.SetData("col1", "0x" + msg2Colour)

            CasparDevice.Channels(1).CG.Add(101, "generalMessage", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 " & backgrounds2.Text)
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
            msg2OnBtn.BackColor = Color.Green
            'disable button
            msg2OnBtn.Enabled = False
        End If
    End Sub

    Private Sub msg3OnBtn_Click(sender As Object, e As EventArgs) Handles msg3OnBtn.Click
        If Me.CasparDevice.IsConnected = True Then

            CasparCGDataCollection.Clear() 'cgData.Clear()
            CasparCGDataCollection.SetData("f0", msg3Title.Text)
            CasparCGDataCollection.SetData("f1", msg3Line1.Text)
            CasparCGDataCollection.SetData("f2", msg3Line2.Text)
            CasparCGDataCollection.SetData("f3", msg3Line3.Text)
            CasparCGDataCollection.SetData("f4", msg3Line4.Text)
            CasparCGDataCollection.SetData("col1", "0x" + msg3Colour)

            CasparDevice.Channels(1).CG.Add(101, "generalMessage", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 " & backgrounds3.Text)
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
            msg3OnBtn.BackColor = Color.Green
            'disable button
            msg3OnBtn.Enabled = False
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles msg4OnBtn.Click
        If Me.CasparDevice.IsConnected = True Then

            CasparCGDataCollection.Clear() 'cgData.Clear()
            CasparCGDataCollection.SetData("f0", msg4Title.Text)
            CasparCGDataCollection.SetData("f1", msg4Line1.Text)
            CasparCGDataCollection.SetData("f2", msg4Line2.Text)
            CasparCGDataCollection.SetData("f3", msg4Line3.Text)
            CasparCGDataCollection.SetData("f4", msg4Line4.Text)
            CasparCGDataCollection.SetData("col1", "0x" + msg4Colour)

            CasparDevice.Channels(1).CG.Add(101, "generalMessage", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 " & backgrounds4.Text)
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
            msg4OnBtn.BackColor = Color.Green
            'disable button
            msg4OnBtn.Enabled = False
        End If
    End Sub

    Private Sub msg2OffBtn_Click(sender As Object, e As EventArgs) Handles msg2OffBtn.Click
        If Me.CasparDevice.IsConnected = True Then
            'fade off image in background
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            'then fade out the rest and set buttons
            CasparDevice.Channels(1).CG.Stop(101)
            msg1OnBtn.UseVisualStyleBackColor = True
            msg2OnBtn.UseVisualStyleBackColor = True
            msg3OnBtn.UseVisualStyleBackColor = True
            msg4OnBtn.UseVisualStyleBackColor = True
            msg5OnBtn.UseVisualStyleBackColor = True
            msg6OnBtn.UseVisualStyleBackColor = True
            msg7OnBtn.UseVisualStyleBackColor = True
            msg8OnBtn.UseVisualStyleBackColor = True
            'reenable button
            msg1OnBtn.Enabled = True
            msg2OnBtn.Enabled = True
            msg3OnBtn.Enabled = True
            msg4OnBtn.Enabled = True
            msg5OnBtn.Enabled = True
            msg6OnBtn.Enabled = True
            msg7OnBtn.Enabled = True
            msg8OnBtn.Enabled = True
        End If
    End Sub

    Private Sub msg3OffBtn_Click(sender As Object, e As EventArgs) Handles msg3OffBtn.Click
        If Me.CasparDevice.IsConnected = True Then
            'fade off image in background
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            'then fade out the rest and set buttons
            CasparDevice.Channels(1).CG.Stop(101)
            msg1OnBtn.UseVisualStyleBackColor = True
            msg2OnBtn.UseVisualStyleBackColor = True
            msg3OnBtn.UseVisualStyleBackColor = True
            msg4OnBtn.UseVisualStyleBackColor = True
            msg5OnBtn.UseVisualStyleBackColor = True
            msg6OnBtn.UseVisualStyleBackColor = True
            msg7OnBtn.UseVisualStyleBackColor = True
            msg8OnBtn.UseVisualStyleBackColor = True
            'reenable button
            msg1OnBtn.Enabled = True
            msg2OnBtn.Enabled = True
            msg3OnBtn.Enabled = True
            msg4OnBtn.Enabled = True
            msg5OnBtn.Enabled = True
            msg6OnBtn.Enabled = True
            msg7OnBtn.Enabled = True
            msg8OnBtn.Enabled = True
        End If
    End Sub

    Private Sub msg4OffBtn_Click(sender As Object, e As EventArgs) Handles msg4OffBtn.Click
        If Me.CasparDevice.IsConnected = True Then
            'fade off image in background
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            'then fade out the rest and set buttons
            CasparDevice.Channels(1).CG.Stop(101)
            msg1OnBtn.UseVisualStyleBackColor = True
            msg2OnBtn.UseVisualStyleBackColor = True
            msg3OnBtn.UseVisualStyleBackColor = True
            msg4OnBtn.UseVisualStyleBackColor = True
            msg5OnBtn.UseVisualStyleBackColor = True
            msg6OnBtn.UseVisualStyleBackColor = True
            msg7OnBtn.UseVisualStyleBackColor = True
            msg8OnBtn.UseVisualStyleBackColor = True
            'reenable button
            msg1OnBtn.Enabled = True
            msg2OnBtn.Enabled = True
            msg3OnBtn.Enabled = True
            msg4OnBtn.Enabled = True
            msg5OnBtn.Enabled = True
            msg6OnBtn.Enabled = True
            msg7OnBtn.Enabled = True
            msg8OnBtn.Enabled = True
        End If
    End Sub

    Private Sub ReloadBackgroundsComboBx5_Click(sender As Object, e As EventArgs) Handles ReloadBackgroundsComboBx5.Click
        If Me.CasparDevice.IsConnected = True Then
            Dim File As Svt.Caspar.MediaInfo
            CasparDevice.RefreshMediafiles()
            'Clear list box in case of reload
            backgrounds5.Items.Clear()
            Threading.Thread.Sleep(250)

            For Each File In CasparDevice.Mediafiles
                backgrounds5.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
            Next
        End If
    End Sub

    Private Sub ReloadBackgroundsComboBx6_Click(sender As Object, e As EventArgs) Handles ReloadBackgroundsComboBx6.Click
        If Me.CasparDevice.IsConnected = True Then
            Dim File As Svt.Caspar.MediaInfo
            CasparDevice.RefreshMediafiles()
            'Clear list box in case of reload
            backgrounds6.Items.Clear()
            Threading.Thread.Sleep(250)

            For Each File In CasparDevice.Mediafiles
                backgrounds6.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
            Next
        End If
    End Sub

    Private Sub ReloadBackgroundsComboBx7_Click(sender As Object, e As EventArgs) Handles ReloadBackgroundsComboBx7.Click
        If Me.CasparDevice.IsConnected = True Then
            Dim File As Svt.Caspar.MediaInfo
            CasparDevice.RefreshMediafiles()
            'Clear list box in case of reload
            backgrounds7.Items.Clear()
            Threading.Thread.Sleep(250)

            For Each File In CasparDevice.Mediafiles
                backgrounds7.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
            Next
        End If
    End Sub

    Private Sub ReloadBackgroundsComboBx8_Click(sender As Object, e As EventArgs) Handles ReloadBackgroundsComboBx8.Click
        If Me.CasparDevice.IsConnected = True Then
            Dim File As Svt.Caspar.MediaInfo
            CasparDevice.RefreshMediafiles()
            'Clear list box in case of reload
            backgrounds8.Items.Clear()
            Threading.Thread.Sleep(250)

            For Each File In CasparDevice.Mediafiles
                backgrounds8.Items.Add((UCase(Replace((File.FullName), "\", "/"))))
            Next
        End If
    End Sub

    Private Sub msg5OnBtn_Click(sender As Object, e As EventArgs) Handles msg5OnBtn.Click
        If Me.CasparDevice.IsConnected = True Then

            CasparCGDataCollection.Clear() 'cgData.Clear()
            CasparCGDataCollection.SetData("f0", msg5Title.Text)
            CasparCGDataCollection.SetData("f1", msg5Line1.Text)
            CasparCGDataCollection.SetData("f2", msg5Line2.Text)
            CasparCGDataCollection.SetData("f3", msg5Line3.Text)
            CasparCGDataCollection.SetData("f4", msg5Line4.Text)
            CasparCGDataCollection.SetData("col1", "0x" + msg5Colour)

            CasparDevice.Channels(1).CG.Add(101, "generalMessage", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 " & backgrounds5.Text)
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
            msg5OnBtn.BackColor = Color.Green
            'disable button
            msg5OnBtn.Enabled = False
        End If
    End Sub

    Private Sub msg6OnBtn_Click(sender As Object, e As EventArgs) Handles msg6OnBtn.Click
        If Me.CasparDevice.IsConnected = True Then

            CasparCGDataCollection.Clear() 'cgData.Clear()
            CasparCGDataCollection.SetData("f0", msg6Title.Text)
            CasparCGDataCollection.SetData("f1", msg6Line1.Text)
            CasparCGDataCollection.SetData("f2", msg6Line2.Text)
            CasparCGDataCollection.SetData("f3", msg6Line3.Text)
            CasparCGDataCollection.SetData("f4", msg6Line4.Text)
            CasparCGDataCollection.SetData("col1", "0x" + msg6Colour)

            CasparDevice.Channels(1).CG.Add(101, "generalMessage", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 " & backgrounds6.Text)
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
            msg6OnBtn.BackColor = Color.Green
            'disable button
            msg6OnBtn.Enabled = False
        End If
    End Sub

    Private Sub msg7OnBtn_Click(sender As Object, e As EventArgs) Handles msg7OnBtn.Click
        If Me.CasparDevice.IsConnected = True Then

            CasparCGDataCollection.Clear() 'cgData.Clear()
            CasparCGDataCollection.SetData("f0", msg7Title.Text)
            CasparCGDataCollection.SetData("f1", msg7Line1.Text)
            CasparCGDataCollection.SetData("f2", msg7Line2.Text)
            CasparCGDataCollection.SetData("f3", msg7Line3.Text)
            CasparCGDataCollection.SetData("f4", msg7Line4.Text)
            CasparCGDataCollection.SetData("col1", "0x" + msg7Colour)

            CasparDevice.Channels(1).CG.Add(101, "generalMessage", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 " & backgrounds7.Text)
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
            msg7OnBtn.BackColor = Color.Green
            'disable button
            msg7OnBtn.Enabled = False
        End If
    End Sub

    Private Sub msg8OnBtn_Click(sender As Object, e As EventArgs) Handles msg8OnBtn.Click
        If Me.CasparDevice.IsConnected = True Then

            CasparCGDataCollection.Clear() 'cgData.Clear()
            CasparCGDataCollection.SetData("f0", msg8Title.Text)
            CasparCGDataCollection.SetData("f1", msg8Line1.Text)
            CasparCGDataCollection.SetData("f2", msg8Line2.Text)
            CasparCGDataCollection.SetData("f3", msg8Line3.Text)
            CasparCGDataCollection.SetData("f4", msg8Line4.Text)
            CasparCGDataCollection.SetData("col1", "0x" + msg8Colour)

            CasparDevice.Channels(1).CG.Add(101, "generalMessage", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(1).CG.Play(101)
            'fading in image
            CasparDevice.SendString("MIXER 2-100 OPACITY 0")
            CasparDevice.SendString("play 2-100 " & backgrounds8.Text)
            CasparDevice.SendString("MIXER 2-100 OPACITY 1 48 linear")
            msg8OnBtn.BackColor = Color.Green
            'disable button
            msg8OnBtn.Enabled = False
        End If
    End Sub

    Private Sub msg5OffBtn_Click(sender As Object, e As EventArgs) Handles msg5OffBtn.Click
        If Me.CasparDevice.IsConnected = True Then
            'fade off image in background
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            'then fade out the rest and set buttons
            CasparDevice.Channels(1).CG.Stop(101)
            msg1OnBtn.UseVisualStyleBackColor = True
            msg2OnBtn.UseVisualStyleBackColor = True
            msg3OnBtn.UseVisualStyleBackColor = True
            msg4OnBtn.UseVisualStyleBackColor = True
            msg5OnBtn.UseVisualStyleBackColor = True
            msg6OnBtn.UseVisualStyleBackColor = True
            msg7OnBtn.UseVisualStyleBackColor = True
            msg8OnBtn.UseVisualStyleBackColor = True
            'reenable button
            msg1OnBtn.Enabled = True
            msg2OnBtn.Enabled = True
            msg3OnBtn.Enabled = True
            msg4OnBtn.Enabled = True
            msg5OnBtn.Enabled = True
            msg6OnBtn.Enabled = True
            msg7OnBtn.Enabled = True
            msg8OnBtn.Enabled = True
        End If
    End Sub

    Private Sub msg6OffBtn_Click(sender As Object, e As EventArgs) Handles msg6OffBtn.Click
        If Me.CasparDevice.IsConnected = True Then
            'fade off image in background
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            'then fade out the rest and set buttons
            CasparDevice.Channels(1).CG.Stop(101)
            msg1OnBtn.UseVisualStyleBackColor = True
            msg2OnBtn.UseVisualStyleBackColor = True
            msg3OnBtn.UseVisualStyleBackColor = True
            msg4OnBtn.UseVisualStyleBackColor = True
            msg5OnBtn.UseVisualStyleBackColor = True
            msg6OnBtn.UseVisualStyleBackColor = True
            msg7OnBtn.UseVisualStyleBackColor = True
            msg8OnBtn.UseVisualStyleBackColor = True
            'reenable button
            msg1OnBtn.Enabled = True
            msg2OnBtn.Enabled = True
            msg3OnBtn.Enabled = True
            msg4OnBtn.Enabled = True
            msg5OnBtn.Enabled = True
            msg6OnBtn.Enabled = True
            msg7OnBtn.Enabled = True
            msg8OnBtn.Enabled = True
        End If
    End Sub

    Private Sub msg7OffBtn_Click(sender As Object, e As EventArgs) Handles msg7OffBtn.Click
        If Me.CasparDevice.IsConnected = True Then
            'fade off image in background
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            'then fade out the rest and set buttons
            CasparDevice.Channels(1).CG.Stop(101)
            msg1OnBtn.UseVisualStyleBackColor = True
            msg2OnBtn.UseVisualStyleBackColor = True
            msg3OnBtn.UseVisualStyleBackColor = True
            msg4OnBtn.UseVisualStyleBackColor = True
            msg5OnBtn.UseVisualStyleBackColor = True
            msg6OnBtn.UseVisualStyleBackColor = True
            msg7OnBtn.UseVisualStyleBackColor = True
            msg8OnBtn.UseVisualStyleBackColor = True
            'reenable button
            msg1OnBtn.Enabled = True
            msg2OnBtn.Enabled = True
            msg3OnBtn.Enabled = True
            msg4OnBtn.Enabled = True
            msg5OnBtn.Enabled = True
            msg6OnBtn.Enabled = True
            msg7OnBtn.Enabled = True
            msg8OnBtn.Enabled = True
        End If
    End Sub

    Private Sub msg8OffBtn_Click(sender As Object, e As EventArgs) Handles msg8OffBtn.Click
        If Me.CasparDevice.IsConnected = True Then
            'fade off image in background
            CasparDevice.SendString("MIXER 2-100 OPACITY 0 24 linear")
            count = 0
            countTimer.Enabled = True
            'then fade out the rest and set buttons
            CasparDevice.Channels(1).CG.Stop(101)
            msg1OnBtn.UseVisualStyleBackColor = True
            msg2OnBtn.UseVisualStyleBackColor = True
            msg3OnBtn.UseVisualStyleBackColor = True
            msg4OnBtn.UseVisualStyleBackColor = True
            msg5OnBtn.UseVisualStyleBackColor = True
            msg6OnBtn.UseVisualStyleBackColor = True
            msg7OnBtn.UseVisualStyleBackColor = True
            msg8OnBtn.UseVisualStyleBackColor = True
            'reenable button
            msg1OnBtn.Enabled = True
            msg2OnBtn.Enabled = True
            msg3OnBtn.Enabled = True
            msg4OnBtn.Enabled = True
            msg5OnBtn.Enabled = True
            msg6OnBtn.Enabled = True
            msg7OnBtn.Enabled = True
            msg8OnBtn.Enabled = True
        End If
    End Sub

    Private Sub Msg1ColPickBTN_Click(sender As Object, e As EventArgs) Handles Msg1ColPickBTN.Click
        If ColorDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            ' Label24.ForeColor = ColorDialog1.Color
            '  msg1Colour = System.Drawing.ColorTranslator.ToHtml(ColorDialog1.Color)

            msg1Colour = String.Format("{0:X2}{1:X2}{2:X2}", ColorDialog1.Color.R, ColorDialog1.Color.G, ColorDialog1.Color.B)
           
        End If
    End Sub

 
    Private Sub Msg2ColPickBTN_Click(sender As Object, e As EventArgs) Handles Msg2ColPickBTN.Click
        If ColorDialog2.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            msg2Colour = String.Format("{0:X2}{1:X2}{2:X2}", ColorDialog2.Color.R, ColorDialog2.Color.G, ColorDialog2.Color.B)
        End If
    End Sub

    Private Sub Msg3ColPickBTN_Click(sender As Object, e As EventArgs) Handles Msg3ColPickBTN.Click
        If ColorDialog3.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            msg3Colour = String.Format("{0:X2}{1:X2}{2:X2}", ColorDialog3.Color.R, ColorDialog3.Color.G, ColorDialog3.Color.B)
        End If
    End Sub

    Private Sub Msg4ColPickBTN_Click(sender As Object, e As EventArgs) Handles Msg4ColPickBTN.Click
        If ColorDialog4.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            msg4Colour = String.Format("{0:X2}{1:X2}{2:X2}", ColorDialog4.Color.R, ColorDialog4.Color.G, ColorDialog4.Color.B)
        End If
    End Sub

    Private Sub Msg5ColPickBTN_Click(sender As Object, e As EventArgs) Handles Msg5ColPickBTN.Click
        If ColorDialog5.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            msg5Colour = String.Format("{0:X2}{1:X2}{2:X2}", ColorDialog5.Color.R, ColorDialog5.Color.G, ColorDialog5.Color.B)
        End If
    End Sub

    Private Sub Msg6ColPickBTN_Click(sender As Object, e As EventArgs) Handles Msg6ColPickBTN.Click
        If ColorDialog6.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            msg6Colour = String.Format("{0:X2}{1:X2}{2:X2}", ColorDialog6.Color.R, ColorDialog6.Color.G, ColorDialog6.Color.B)
        End If
    End Sub


    Private Sub Msg7ColPickBTN_Click(sender As Object, e As EventArgs) Handles Msg7ColPickBTN.Click
        If ColorDialog7.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            msg7Colour = String.Format("{0:X2}{1:X2}{2:X2}", ColorDialog7.Color.R, ColorDialog7.Color.G, ColorDialog7.Color.B)
        End If
    End Sub

    Private Sub Msg8ColPickBTN_Click(sender As Object, e As EventArgs) Handles Msg8ColPickBTN.Click
        If ColorDialog8.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            msg8Colour = String.Format("{0:X2}{1:X2}{2:X2}", ColorDialog8.Color.R, ColorDialog8.Color.G, ColorDialog8.Color.B)
        End If
    End Sub

    Private Sub ClearGFXOnlyBTN_Click(sender As Object, e As EventArgs) Handles ClearGFXOnlyBTN.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(0).CG.Stop(50)
            CasparDevice.Channels(0).CG.Stop(99)
            CasparDevice.Channels(0).CG.Stop(100)
            CasparDevice.Channels(0).CG.Stop(101)
            CasparDevice.Channels(0).CG.Stop(102)
            CasparDevice.Channels(0).CG.Stop(400)
            'CasparDevice.Channels(0).CG.Stop(401)
            CasparDevice.Channels(0).CG.Stop(390)
            CasparDevice.Channels(0).CG.Stop(391)
            CasparDevice.SendString("stop 1-50")
            CasparDevice.SendString("stop 1-99")
            CasparDevice.SendString("stop 1-100")
            CasparDevice.SendString("stop 1-101")
            CasparDevice.SendString("stop 1-102")
            'CasparDevice.SendString("stop 1-400")
            'CasparDevice.SendString("stop 1-401")
            CasparDevice.SendString("stop 1-390")
            CasparDevice.SendString("stop 1-391")
            CasparDevice.SendString("stop 1-104")

            CasparDevice.Channels(1).CG.Stop(50)
            CasparDevice.Channels(1).CG.Stop(99)
            CasparDevice.Channels(1).CG.Stop(100)
            CasparDevice.Channels(1).CG.Stop(101)
            CasparDevice.Channels(1).CG.Stop(102)
            CasparDevice.Channels(1).CG.Stop(400)
            CasparDevice.Channels(1).CG.Stop(401)
            CasparDevice.Channels(1).CG.Stop(390)
            CasparDevice.Channels(1).CG.Stop(391)
            ' CasparDevice.SendString("stop 2-50")
            CasparDevice.SendString("stop 2-99")
            CasparDevice.SendString("stop 2-100")
            CasparDevice.SendString("stop 2-101")
            CasparDevice.SendString("stop 2-102")
            CasparDevice.SendString("stop 2-400")
            CasparDevice.SendString("stop 2-401")
            CasparDevice.SendString("stop 2-390")
            CasparDevice.SendString("stop 2-391")
            'scores page
            CasparDevice.SendString("stop 2-104")
            CasparDevice.SendString("stop 2-105")
            CasparDevice.SendString("stop 2-106")
            CasparDevice.SendString("stop 2-107")
            CasparDevice.SendString("stop 2-108")
            CasparDevice.SendString("stop 2-109")

            ' this also needs to reset buttons
            ShowTeamSheet.UseVisualStyleBackColor = True
            ShowSubsSheet.UseVisualStyleBackColor = True
            ShowAwayFirstEleven.UseVisualStyleBackColor = True
            ShowAwaySubsSheet.UseVisualStyleBackColor = True
            'showClock.UseVisualStyleBackColor = True
            playVid.UseVisualStyleBackColor = True
            playNext.UseVisualStyleBackColor = True
            'LoopVid.UseVisualStyleBackColor = True
            showBigScore.UseVisualStyleBackColor = True
            showPremScores.UseVisualStyleBackColor = True
            TXPremScores_2BTN.UseVisualStyleBackColor = True
            ChampTX1Btn.UseVisualStyleBackColor = True
            ChampTX2Btn.UseVisualStyleBackColor = True
            LTStrapTXBTN.UseVisualStyleBackColor = True
            CrawlOn.UseVisualStyleBackColor = True
            showSub.UseVisualStyleBackColor = True
            AwaySubOn.UseVisualStyleBackColor = True
            showAddedTimeBTN.UseVisualStyleBackColor = True
            'ShowClockInGameBTN.UseVisualStyleBackColor = True
            PlayVidInGame.UseVisualStyleBackColor = True
            PlayNextVidInGame.UseVisualStyleBackColor = True
            ' LoopVidInGame.UseVisualStyleBackColor = True
        End If
    End Sub



    Private Sub showScoresScroller_Click(sender As Object, e As EventArgs) Handles showScoresScroller.Click
        If Me.CasparDevice.IsConnected = True Then

            CasparCGDataCollection.Clear()

            If sscoresT1Left.Checked = True Then
                CasparCGDataCollection.SetData("f0", tab1Logo1Select.Text + " Latest Scores:       " + Score1.Text + "  " + Score2.Text + "  " + middle13.Text + "  " + Score3.Text + "  " + Score4.Text + "       " + Score5.Text + "  " + Score6.Text + "  " + middle14.Text + "  " + Score7.Text + "  " + Score8.Text + "       " + Score9.Text + "  " + Score10.Text + "  " + middle15.Text + "  " + Score11.Text + "  " + Score12.Text + "       " + Score13.Text + "  " + Score14.Text + "  " + middle16.Text + "  " + Score15.Text + "  " + Score16.Text + "       " + Score17.Text + "  " + Score18.Text + "  " + middle17.Text + "  " + Score19.Text + "  " + Score20.Text + "       " + Score21.Text + "  " + Score22.Text + "  " + middle18.Text + "  " + Score23.Text + "  " + Score24.Text + "  ")
            End If
            If sscoresT1Right.Checked = True Then
                CasparCGDataCollection.SetData("f0", tab1Logo2Select.Text + " Latest Scores:       " + Score25.Text + "  " + Score26.Text + "  " + middle19.Text + "  " + Score27.Text + "  " + Score28.Text + "       " + Score29.Text + "  " + Score30.Text + "  " + middle20.Text + "  " + Score31.Text + "  " + Score32.Text + "       " + Score33.Text + "  " + Score34.Text + "  " + middle21.Text + "  " + Score35.Text + "  " + Score36.Text + "       " + Score37.Text + "  " + Score38.Text + "  " + middle22.Text + "  " + Score39.Text + "  " + Score40.Text + "       " + Score41.Text + "  " + Score42.Text + "  " + middle23.Text + "  " + Score43.Text + "  " + Score44.Text + "       " + Score45.Text + "  " + Score45.Text + "  " + middle24.Text + "  " + Score47.Text + "  " + Score48.Text + "  ")
            End If
            If sscoresT2Left.Checked = True Then
                CasparCGDataCollection.SetData("f0", tab2Logo1Select.Text + " Latest Scores:       " + ChampScore1.Text + "  " + ChampScore2.Text + "  " + middle1.Text + "  " + ChampScore3.Text + "  " + ChampScore4.Text + "       " + ChampScore5.Text + "  " + ChampScore6.Text + "  " + middle2.Text + "  " + ChampScore7.Text + "  " + ChampScore8.Text + "       " + ChampScore9.Text + "  " + ChampScore10.Text + "  " + middle3.Text + "  " + ChampScore11.Text + "  " + ChampScore12.Text + "       " + ChampScore13.Text + "  " + ChampScore14.Text + "  " + middle4.Text + "  " + ChampScore15.Text + "  " + ChampScore16.Text + "       " + ChampScore17.Text + "  " + ChampScore18.Text + "  " + middle5.Text + " " + ChampScore19.Text + "  " + ChampScore20.Text + "      " + ChampScore21.Text + "  " + ChampScore22.Text + "  " + middle6.Text + "  " + ChampScore23.Text + "  " + ChampScore24.Text + "  ")
            End If
            If sscoresT2Right.Checked = True Then
                CasparCGDataCollection.SetData("f0", tab2Logo2Select.Text + " Latest Scores:       " + ChampScore25.Text + "  " + ChampScore26.Text + "  " + middle7.Text + "  " + ChampScore27.Text + "  " + ChampScore28.Text + "       " + ChampScore29.Text + "  " + ChampScore30.Text + "  " + middle8.Text + "  " + ChampScore31.Text + "  " + ChampScore32.Text + "       " + ChampScore33.Text + "  " + ChampScore34.Text + "  " + middle9.Text + "  " + ChampScore35.Text + "  " + ChampScore36.Text + "       " + ChampScore37.Text + "  " + ChampScore38.Text + "  " + middle10.Text + "  " + ChampScore39.Text + "  " + ChampScore40.Text + "       " + ChampScore41.Text + "  " + ChampScore42.Text + "  " + middle11.Text + "  " + ChampScore43.Text + "  " + ChampScore44.Text + "       " + ChampScore45.Text + "  " + ChampScore46.Text + "  " + middle12.Text + " " + ChampScore47.Text + "  " + ChampScore48.Text + "  ")
            End If



            CasparDevice.Channels(0).CG.Add(101, "efc_TSheet_crawl_temp_LT", True, CasparCGDataCollection.ToAMCPEscapedXml)
            CasparDevice.Channels(0).CG.Play(101)

            'fading in image
            CasparDevice.SendString("MIXER 1-100 OPACITY 0")
            CasparDevice.SendString("play 1-104 LT_crawl_crest")
            CasparDevice.SendString("MIXER 1-100 OPACITY 1 48 linear")


            'fading in image
            CasparDevice.SendString("MIXER 1-100 OPACITY 0")
            CasparDevice.SendString("play 1-100 LT_crawl_nocrest")
            CasparDevice.SendString("MIXER 1-100 OPACITY 1 48 linear")

            'CasparDevice.SendString("play 1-102 LT_crawl_crest")
            CasparDevice.SendString("play 1-103 LTFlare")
            showScoresScroller.BackColor = Color.Green
            'disable button
            'showScoresScroller.Enabled = False
            crawlToggle = True
        End If
    End Sub

    Private Sub hideScoresScroller_Click(sender As Object, e As EventArgs) Handles hideScoresScroller.Click
        If Me.CasparDevice.IsConnected = True Then
            CasparDevice.Channels(0).CG.Stop(101)
            CasparDevice.SendString("MIXER 1-100 OPACITY 0 24 linear")
            countBPS = 0
            BPlayChanFadeOut.Enabled = True
            CasparDevice.SendString("STOP 1-102")
            CasparDevice.SendString("STOP 1-103")
            CasparDevice.SendString("STOP 1-104")
            showScoresScroller.BackColor = Color.FromKnownColor(KnownColor.Control)
            showScoresScroller.UseVisualStyleBackColor = True
            'crawlToggle = False
            'disable button
            ' showScoresScroller.Enabled = True

        End If
    End Sub

    Private Sub playlistStop_Tick(sender As Object, e As EventArgs) Handles playlistStop.Tick
        countplaylist = countplaylist + 1
        If countPlaylist >= 10 Then
            CasparDevice.SendString("stop 2-99")
            CasparDevice.SendString("MIXER 2-99 OPACITY 1 0 linear")
            playlistStop.Enabled = False
            countPlaylist = 0
        End If
    End Sub

    Private Sub firstHalfRadBTN_CheckedChanged(sender As Object, e As EventArgs) Handles firstHalfRadBTN.CheckedChanged
        If firstHalfRadBTN.Checked = True Then
            startClockTime.Text = "0"
            stopClockTime.Text = "45"
        End If
    End Sub

    Private Sub secondHalfRadBTN_CheckedChanged(sender As Object, e As EventArgs) Handles secondHalfRadBTN.CheckedChanged
        If secondHalfRadBTN.Checked = True Then
            startClockTime.Text = "45"
            stopClockTime.Text = "90"
        End If
    End Sub
End Class
