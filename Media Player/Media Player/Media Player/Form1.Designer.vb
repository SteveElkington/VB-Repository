<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.connectBTN = New System.Windows.Forms.Button()
        Me.PlayVidBTN = New System.Windows.Forms.Button()
        Me.stopVideoBTN = New System.Windows.Forms.Button()
        Me.infoTXT = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.mediaFilesLB = New System.Windows.Forms.ListBox()
        Me.refreshFolderInfoBTN = New System.Windows.Forms.Button()
        Me.stopTemplateBTN = New System.Windows.Forms.Button()
        Me.playTemplateBTN = New System.Windows.Forms.Button()
        Me.updateTemplateBTN = New System.Windows.Forms.Button()
        Me.templateFilesLB = New System.Windows.Forms.ListBox()
        Me.ListTemplatesBTN = New System.Windows.Forms.Button()
        Me.f0TextTB = New System.Windows.Forms.TextBox()
        Me.f1textTB = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'connectBTN
        '
        Me.connectBTN.Location = New System.Drawing.Point(12, 12)
        Me.connectBTN.Name = "connectBTN"
        Me.connectBTN.Size = New System.Drawing.Size(75, 23)
        Me.connectBTN.TabIndex = 0
        Me.connectBTN.Text = "Connect"
        Me.connectBTN.UseVisualStyleBackColor = True
        '
        'PlayVidBTN
        '
        Me.PlayVidBTN.Location = New System.Drawing.Point(103, 12)
        Me.PlayVidBTN.Name = "PlayVidBTN"
        Me.PlayVidBTN.Size = New System.Drawing.Size(75, 23)
        Me.PlayVidBTN.TabIndex = 1
        Me.PlayVidBTN.Text = "Play Video"
        Me.PlayVidBTN.UseVisualStyleBackColor = True
        '
        'stopVideoBTN
        '
        Me.stopVideoBTN.Location = New System.Drawing.Point(103, 41)
        Me.stopVideoBTN.Name = "stopVideoBTN"
        Me.stopVideoBTN.Size = New System.Drawing.Size(75, 23)
        Me.stopVideoBTN.TabIndex = 2
        Me.stopVideoBTN.Text = "Stop Video"
        Me.stopVideoBTN.UseVisualStyleBackColor = True
        '
        'infoTXT
        '
        Me.infoTXT.Location = New System.Drawing.Point(552, 43)
        Me.infoTXT.Multiline = True
        Me.infoTXT.Name = "infoTXT"
        Me.infoTXT.Size = New System.Drawing.Size(277, 320)
        Me.infoTXT.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(549, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "INFO"
        '
        'mediaFilesLB
        '
        Me.mediaFilesLB.FormattingEnabled = True
        Me.mediaFilesLB.Location = New System.Drawing.Point(12, 92)
        Me.mediaFilesLB.Name = "mediaFilesLB"
        Me.mediaFilesLB.Size = New System.Drawing.Size(247, 264)
        Me.mediaFilesLB.TabIndex = 7
        '
        'refreshFolderInfoBTN
        '
        Me.refreshFolderInfoBTN.Location = New System.Drawing.Point(12, 41)
        Me.refreshFolderInfoBTN.Name = "refreshFolderInfoBTN"
        Me.refreshFolderInfoBTN.Size = New System.Drawing.Size(75, 23)
        Me.refreshFolderInfoBTN.TabIndex = 8
        Me.refreshFolderInfoBTN.Text = "List Clips"
        Me.refreshFolderInfoBTN.UseVisualStyleBackColor = True
        '
        'stopTemplateBTN
        '
        Me.stopTemplateBTN.Location = New System.Drawing.Point(280, 41)
        Me.stopTemplateBTN.Name = "stopTemplateBTN"
        Me.stopTemplateBTN.Size = New System.Drawing.Size(75, 23)
        Me.stopTemplateBTN.TabIndex = 10
        Me.stopTemplateBTN.Text = "Stop Temp"
        Me.stopTemplateBTN.UseVisualStyleBackColor = True
        '
        'playTemplateBTN
        '
        Me.playTemplateBTN.Location = New System.Drawing.Point(361, 12)
        Me.playTemplateBTN.Name = "playTemplateBTN"
        Me.playTemplateBTN.Size = New System.Drawing.Size(75, 23)
        Me.playTemplateBTN.TabIndex = 9
        Me.playTemplateBTN.Text = "Play Temp"
        Me.playTemplateBTN.UseVisualStyleBackColor = True
        '
        'updateTemplateBTN
        '
        Me.updateTemplateBTN.Location = New System.Drawing.Point(361, 41)
        Me.updateTemplateBTN.Name = "updateTemplateBTN"
        Me.updateTemplateBTN.Size = New System.Drawing.Size(75, 23)
        Me.updateTemplateBTN.TabIndex = 11
        Me.updateTemplateBTN.Text = "Update"
        Me.updateTemplateBTN.UseVisualStyleBackColor = True
        '
        'templateFilesLB
        '
        Me.templateFilesLB.FormattingEnabled = True
        Me.templateFilesLB.Location = New System.Drawing.Point(280, 92)
        Me.templateFilesLB.Name = "templateFilesLB"
        Me.templateFilesLB.Size = New System.Drawing.Size(247, 264)
        Me.templateFilesLB.TabIndex = 12
        '
        'ListTemplatesBTN
        '
        Me.ListTemplatesBTN.Location = New System.Drawing.Point(280, 12)
        Me.ListTemplatesBTN.Name = "ListTemplatesBTN"
        Me.ListTemplatesBTN.Size = New System.Drawing.Size(75, 23)
        Me.ListTemplatesBTN.TabIndex = 13
        Me.ListTemplatesBTN.Text = "List FT's"
        Me.ListTemplatesBTN.UseVisualStyleBackColor = True
        '
        'f0TextTB
        '
        Me.f0TextTB.Location = New System.Drawing.Point(442, 15)
        Me.f0TextTB.Name = "f0TextTB"
        Me.f0TextTB.Size = New System.Drawing.Size(100, 20)
        Me.f0TextTB.TabIndex = 14
        Me.f0TextTB.Text = "f0 Text"
        '
        'f1textTB
        '
        Me.f1textTB.Location = New System.Drawing.Point(442, 44)
        Me.f1textTB.Name = "f1textTB"
        Me.f1textTB.Size = New System.Drawing.Size(100, 20)
        Me.f1textTB.TabIndex = 15
        Me.f1textTB.Text = "f1 Text"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(841, 372)
        Me.Controls.Add(Me.f1textTB)
        Me.Controls.Add(Me.f0TextTB)
        Me.Controls.Add(Me.ListTemplatesBTN)
        Me.Controls.Add(Me.templateFilesLB)
        Me.Controls.Add(Me.updateTemplateBTN)
        Me.Controls.Add(Me.stopTemplateBTN)
        Me.Controls.Add(Me.playTemplateBTN)
        Me.Controls.Add(Me.refreshFolderInfoBTN)
        Me.Controls.Add(Me.mediaFilesLB)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.infoTXT)
        Me.Controls.Add(Me.stopVideoBTN)
        Me.Controls.Add(Me.PlayVidBTN)
        Me.Controls.Add(Me.connectBTN)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents connectBTN As System.Windows.Forms.Button
    Friend WithEvents PlayVidBTN As System.Windows.Forms.Button
    Friend WithEvents stopVideoBTN As System.Windows.Forms.Button
    Friend WithEvents infoTXT As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents mediaFilesLB As System.Windows.Forms.ListBox
    Friend WithEvents refreshFolderInfoBTN As System.Windows.Forms.Button
    Friend WithEvents stopTemplateBTN As System.Windows.Forms.Button
    Friend WithEvents playTemplateBTN As System.Windows.Forms.Button
    Friend WithEvents updateTemplateBTN As System.Windows.Forms.Button
    Friend WithEvents templateFilesLB As System.Windows.Forms.ListBox
    Friend WithEvents ListTemplatesBTN As System.Windows.Forms.Button
    Friend WithEvents f0TextTB As System.Windows.Forms.TextBox
    Friend WithEvents f1textTB As System.Windows.Forms.TextBox

End Class
