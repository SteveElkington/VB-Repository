Imports CasparCGNETConnector
'get info from https://github.com/mcdikki/CasparCGNetConnector/wiki/Overview
Public Class Form1

    Dim WithEvents con As New CasparCGConnection("localhost", 5250)

    Private Sub connectBTN_Click(sender As Object, e As EventArgs) Handles connectBTN.Click
        connectBTN.BackColor = Color.Red
        ' addHandler(con.disconnected, AddressOf onDisconnect)
        'addHandler(con.connected, AddressOf onConnect)
        con.connect()
        If Me.con.isConnected = False Then
            Me.con.connect()
            If Me.con.isConnected = False Then
                connectBTN.BackColor = Color.Green
            End If
        End If
        If Me.con.isConnected = True Then

            connectBTN.BackColor = Color.Green
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles PlayVidBTN.Click
        Dim cmd As New PlayCommand(1, 1, "amb", True)
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

    Private Sub getVideoInfoBTN_Click(sender As Object, e As EventArgs) Handles getVideoInfoBTN.Click
        Dim media As New CasparCGMovie("amb")
        media.fillMediaInfo(con)
        infoTXT.Text = "All media infos we got from the server:"
        For Each info As String In media.Infos.Keys
            infoTXT.Text = infoTXT.Text + vbCrLf + vbTab & info & ": " & media.getInfo(info)
        Next
        Dim cmd As New PlayCommand(1, 1, media)
        cmd.execute(con)
    End Sub

    Private Sub showBase64Image(p1 As Object)
        Throw New NotImplementedException
    End Sub

End Class
