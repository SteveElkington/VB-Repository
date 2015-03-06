Imports CasparCGNETConnector
Imports logger
'get info from https://github.com/mcdikki/CasparCGNetConnector/wiki/Overview
Public Class Form1

    Dim WithEvents con As New CasparCGConnection("localhost", 5250)
    Private media As ICasparCGMedia

    Private Sub connectBTN_Click(sender As Object, e As EventArgs) Handles connectBTN.Click
  If Not Me.con.isConnected Then
            Me.con.connect()
        End If
    End Sub


    ' Handle the connected Event
    Private Sub onConnect(ByRef sender As Object) Handles con.connected
        ' If you want to modidy GUI elements, you need this to make sure
        ' it runs in the same thread as the GUI does.
        If connectBTN.InvokeRequired Then
            Dim temp = sender
            connectBTN.Invoke(Sub() onConnect(temp))
        Else
            connectBTN.BackColor = Color.Green
            infoTXT.Text = "Server is connected"
        End If

    End Sub

    ' Handle lost connection
    Private Sub onDisconnect(ByRef sender As Object) Handles con.disconnected
        ' If you want to modidy GUI elements, you need this to make sure
        ' it runs in the same thread as the GUI does.
        If connectBTN.InvokeRequired Then
            Dim temp = sender
            connectBTN.Invoke(Sub() onDisconnect(temp))
        Else
            connectBTN.BackColor = Color.Red
            ' You may want to warn the user or reestablish a conection here...
            infoTXT.Text = "Server is down, please resolve and reconnect before continuing"
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles PlayVidBTN.Click

        Dim clipToPlay As String
        clipToPlay = mediaFilesLB.Text
        clipToPlay = clipToPlay.Substring(0, clipToPlay.Length - 4)

        Dim cmd As New PlayCommand(1, 1, clipToPlay)
        If cmd.execute(con).isOK Then
            infoTXT.Text = "Yeah, it's playing!"
        Else
            infoTXT.Text = "Oh no, something went wrong. Let's check the the server message: " + cmd.getResponse.getServerMessage()
        End If
    End Sub

    Private Sub stopVideoBTN_Click(sender As Object, e As EventArgs) Handles stopVideoBTN.Click
        Dim cmd As New StopCommand(1, 1)
        cmd.execute(con)
    End Sub

    Private Sub refreshFolderInfoBTN_Click(sender As Object, e As EventArgs) Handles refreshFolderInfoBTN.Click
        mediaFilesLB.Items.Clear()
        ' this works but I will try and make caspar do it instead
        ' make ref to directory
        Dim di As New IO.DirectoryInfo("C:\caspar\Server\media")
        Dim diarl As IO.FileInfo() = di.GetFiles()
        Dim dra As IO.FileInfo

        'list names of all files
        For Each dra In diarl
            mediaFilesLB.Items.Add(dra)
        Next
    End Sub

    Private Sub mediaFilesLB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles mediaFilesLB.SelectedIndexChanged
        Dim media As New CasparCGMovie(mediaFilesLB.Text)
        media.fillMediaInfo(con)
        infoTXT.Text = "All media infos we got from the server:"
        For Each info As String In media.Infos.Keys
            infoTXT.Text = infoTXT.Text + vbCrLf + vbTab & info & ": " & media.getInfo(info)
        Next

        ' testing
        infoTXT.Text = infoTXT.Text + vbCrLf + "Media name: " & media.Name
        infoTXT.Text = infoTXT.Text + vbCrLf + "Media type: " & media.MediaType.ToString
        infoTXT.Text = infoTXT.Text + vbCrLf + "Media full name (with path): " & media.FullName
        infoTXT.Text = infoTXT.Text + vbCrLf + "Media path: " & media.Path
        ' infoTXT.Text = infoTXT.Text + vbCrLf + "Media has thumbnail: " & (media.Base64Thumbnail.Length > 0).ToString
        infoTXT.Text = infoTXT.Text + vbCrLf + "Metadata of media:"
        For Each info In media.Infos
            Console.WriteLine(vbTab & info.Key & ": " & info.Value)
        Next


        ' end of testing
        Console.WriteLine("Retrieving thumbnail...")
        Dim cmd2 As New ThumbnailRetrieveCommand(media)
        If cmd2.isCompatible(con) Then
            Console.WriteLine("Thumbs are supported by the server.")
            cmd2.execute(con)
            media.Base64Thumbnail = cmd2.getResponse.getData
        Else
            Console.WriteLine("Thumbs are not supported by the server.")
        End If
    End Sub


    ' Initialise the template and it's update command so that it can be used classwide
    Private template As New CasparCGTemplate("efc_sub_LT")
    Private update As New CgUpdateCommand(1, 1, 1, template.getData)
    Private Sub playTemplateBTN_Click(sender As Object, e As EventArgs) Handles playTemplateBTN.Click
        Dim tempToPlay As String
        tempToPlay = templateFilesLB.Text
        tempToPlay = tempToPlay.Substring(0, tempToPlay.Length - 3)

        'set up to play template
        template = New CasparCGTemplate(tempToPlay)
        template.fillMediaInfo(con)

        ' Set data:
        If template.getData.containsInstance("f0") Then
            template.getData.getInstance("f0").setValue("text", f0TextTB.Text)
        Else
            infoTXT.Text = "Sorry, f0 is not in this template"
        End If

        If template.getData.containsInstance("f1") Then
            template.getData.getInstance("f1").setValue("text", f1textTB.Text)
        Else
            infoTXT.Text = "Sorry, f1 is not in this template"
        End If

        ' Play template (add + autostart):
        'infoTXT.Text = infoTXT.Text + vbCrLf + "Sending new data and playing template: " & template.getDataString
        Dim add As New CgAddCommand(1, 1, template, 1, True)
        add.execute(con)
        playTemplateBTN.Enabled = False
    End Sub

    Private Sub updateTemplateBTN_Click(sender As Object, e As EventArgs) Handles updateTemplateBTN.Click
        ' Update data
        If template.getData.containsInstance("f0") Then
            template.getData.getInstance("f0").setValue("text", f0TextTB.Text)
        Else
            infoTXT.Text = "Sorry, f0 is not in this template"
        End If

        If template.getData.containsInstance("f1") Then
            template.getData.getInstance("f1").setValue("text", f1textTB.Text)
        Else
            infoTXT.Text = "Sorry, f1 is not in this template"
        End If

        ' Update Template
        infoTXT.Text = infoTXT.Text + vbCrLf + "Sending new data to template: " & template.getDataString
        update.setData(template.getData)
        update.execute(con)
    End Sub

    Private Sub stopTemplateBTN_Click(sender As Object, e As EventArgs) Handles stopTemplateBTN.Click
        Dim clearTemplate As New CgStopCommand(1, 1, 1)
        clearTemplate.execute(con)
        playTemplateBTN.Enabled = True
    End Sub




    Private Sub ListTemplatesBTN_Click(sender As Object, e As EventArgs) Handles ListTemplatesBTN.Click
        templateFilesLB.Items.Clear()
        ' this works but I will try and make caspar do it instead
        ' make ref to directory
        Dim di2 As New IO.DirectoryInfo("C:\caspar\Server\templates")
        Dim diarl2 As IO.FileInfo() = di2.GetFiles()
        Dim dra2 As IO.FileInfo

        'list names of all files
        For Each dra2 In diarl2
            templateFilesLB.Items.Add(dra2)
        Next
    End Sub
End Class
