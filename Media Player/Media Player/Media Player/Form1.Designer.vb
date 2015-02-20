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
        Me.getVideoInfoBTN = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.infoTXT.Location = New System.Drawing.Point(3, 181)
        Me.infoTXT.Multiline = True
        Me.infoTXT.Name = "infoTXT"
        Me.infoTXT.Size = New System.Drawing.Size(277, 320)
        Me.infoTXT.TabIndex = 3
        '
        'getVideoInfoBTN
        '
        Me.getVideoInfoBTN.Location = New System.Drawing.Point(184, 12)
        Me.getVideoInfoBTN.Name = "getVideoInfoBTN"
        Me.getVideoInfoBTN.Size = New System.Drawing.Size(75, 23)
        Me.getVideoInfoBTN.TabIndex = 4
        Me.getVideoInfoBTN.Text = "Get info"
        Me.getVideoInfoBTN.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(172, 70)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(100, 50)
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 513)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.getVideoInfoBTN)
        Me.Controls.Add(Me.infoTXT)
        Me.Controls.Add(Me.stopVideoBTN)
        Me.Controls.Add(Me.PlayVidBTN)
        Me.Controls.Add(Me.connectBTN)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents connectBTN As System.Windows.Forms.Button
    Friend WithEvents PlayVidBTN As System.Windows.Forms.Button
    Friend WithEvents stopVideoBTN As System.Windows.Forms.Button
    Friend WithEvents infoTXT As System.Windows.Forms.TextBox
    Friend WithEvents getVideoInfoBTN As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

End Class
