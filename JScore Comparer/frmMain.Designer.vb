<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.grpChooseFile = New System.Windows.Forms.GroupBox()
        Me.lblFile1 = New System.Windows.Forms.Label()
        Me.txtFile1 = New System.Windows.Forms.TextBox()
        Me.btnBrowseFile1 = New System.Windows.Forms.Button()
        Me.btnBrowseFile2 = New System.Windows.Forms.Button()
        Me.txtFile2 = New System.Windows.Forms.TextBox()
        Me.lblFile2 = New System.Windows.Forms.Label()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.lstProcess = New System.Windows.Forms.ListBox()
        Me.opnFile1 = New System.Windows.Forms.OpenFileDialog()
        Me.opnFile2 = New System.Windows.Forms.OpenFileDialog()
        Me.lblSubProgress = New System.Windows.Forms.Label()
        Me.grpChooseFile.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.Color.SkyBlue
        Me.Panel3.Location = New System.Drawing.Point(725, 1)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(13, 342)
        Me.Panel3.TabIndex = 14
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.SkyBlue
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(13, 342)
        Me.Panel1.TabIndex = 15
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.SkyBlue
        Me.Panel2.Location = New System.Drawing.Point(1, 0)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(737, 14)
        Me.Panel2.TabIndex = 16
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BackColor = System.Drawing.Color.SkyBlue
        Me.Panel4.Location = New System.Drawing.Point(1, 329)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(737, 14)
        Me.Panel4.TabIndex = 17
        '
        'grpChooseFile
        '
        Me.grpChooseFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpChooseFile.Controls.Add(Me.btnBrowseFile2)
        Me.grpChooseFile.Controls.Add(Me.txtFile2)
        Me.grpChooseFile.Controls.Add(Me.lblFile2)
        Me.grpChooseFile.Controls.Add(Me.btnBrowseFile1)
        Me.grpChooseFile.Controls.Add(Me.txtFile1)
        Me.grpChooseFile.Controls.Add(Me.lblFile1)
        Me.grpChooseFile.Location = New System.Drawing.Point(21, 22)
        Me.grpChooseFile.Name = "grpChooseFile"
        Me.grpChooseFile.Size = New System.Drawing.Size(697, 102)
        Me.grpChooseFile.TabIndex = 18
        Me.grpChooseFile.TabStop = False
        Me.grpChooseFile.Text = "Choose Files"
        '
        'lblFile1
        '
        Me.lblFile1.AutoSize = True
        Me.lblFile1.Location = New System.Drawing.Point(7, 27)
        Me.lblFile1.Name = "lblFile1"
        Me.lblFile1.Size = New System.Drawing.Size(79, 17)
        Me.lblFile1.TabIndex = 0
        Me.lblFile1.Text = "File 1 Path:"
        '
        'txtFile1
        '
        Me.txtFile1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFile1.Location = New System.Drawing.Point(87, 25)
        Me.txtFile1.Name = "txtFile1"
        Me.txtFile1.Size = New System.Drawing.Size(524, 22)
        Me.txtFile1.TabIndex = 1
        '
        'btnBrowseFile1
        '
        Me.btnBrowseFile1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnBrowseFile1.Location = New System.Drawing.Point(618, 22)
        Me.btnBrowseFile1.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBrowseFile1.Name = "btnBrowseFile1"
        Me.btnBrowseFile1.Size = New System.Drawing.Size(75, 28)
        Me.btnBrowseFile1.TabIndex = 3
        Me.btnBrowseFile1.Text = "Browse"
        Me.btnBrowseFile1.UseVisualStyleBackColor = True
        '
        'btnBrowseFile2
        '
        Me.btnBrowseFile2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnBrowseFile2.Location = New System.Drawing.Point(618, 58)
        Me.btnBrowseFile2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBrowseFile2.Name = "btnBrowseFile2"
        Me.btnBrowseFile2.Size = New System.Drawing.Size(75, 28)
        Me.btnBrowseFile2.TabIndex = 6
        Me.btnBrowseFile2.Text = "Browse"
        Me.btnBrowseFile2.UseVisualStyleBackColor = True
        '
        'txtFile2
        '
        Me.txtFile2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFile2.Location = New System.Drawing.Point(87, 61)
        Me.txtFile2.Name = "txtFile2"
        Me.txtFile2.Size = New System.Drawing.Size(524, 22)
        Me.txtFile2.TabIndex = 5
        '
        'lblFile2
        '
        Me.lblFile2.AutoSize = True
        Me.lblFile2.Location = New System.Drawing.Point(7, 63)
        Me.lblFile2.Name = "lblFile2"
        Me.lblFile2.Size = New System.Drawing.Size(79, 17)
        Me.lblFile2.TabIndex = 4
        Me.lblFile2.Text = "File 2 Path:"
        '
        'btnStop
        '
        Me.btnStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStop.Location = New System.Drawing.Point(618, 131)
        Me.btnStop.Margin = New System.Windows.Forms.Padding(4)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(100, 34)
        Me.btnStop.TabIndex = 19
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStart.Location = New System.Drawing.Point(510, 131)
        Me.btnStart.Margin = New System.Windows.Forms.Padding(4)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(100, 34)
        Me.btnStart.TabIndex = 20
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'lstProcess
        '
        Me.lstProcess.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstProcess.ForeColor = System.Drawing.Color.FromArgb(CType(CType(29, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(29, Byte), Integer))
        Me.lstProcess.FormattingEnabled = True
        Me.lstProcess.ItemHeight = 16
        Me.lstProcess.Location = New System.Drawing.Point(19, 177)
        Me.lstProcess.Margin = New System.Windows.Forms.Padding(4)
        Me.lstProcess.Name = "lstProcess"
        Me.lstProcess.Size = New System.Drawing.Size(699, 148)
        Me.lstProcess.TabIndex = 29
        '
        'opnFile1
        '
        '
        'opnFile2
        '
        '
        'lblSubProgress
        '
        Me.lblSubProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSubProgress.Location = New System.Drawing.Point(21, 127)
        Me.lblSubProgress.Name = "lblSubProgress"
        Me.lblSubProgress.Size = New System.Drawing.Size(445, 46)
        Me.lblSubProgress.TabIndex = 30
        Me.lblSubProgress.Text = "Sub Progress"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(739, 343)
        Me.Controls.Add(Me.lblSubProgress)
        Me.Controls.Add(Me.lstProcess)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.grpChooseFile)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel3)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "JScore Comparer"
        Me.grpChooseFile.ResumeLayout(False)
        Me.grpChooseFile.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents grpChooseFile As GroupBox
    Friend WithEvents lblFile1 As Label
    Friend WithEvents btnBrowseFile1 As Button
    Friend WithEvents txtFile1 As TextBox
    Friend WithEvents btnBrowseFile2 As Button
    Friend WithEvents txtFile2 As TextBox
    Friend WithEvents lblFile2 As Label
    Friend WithEvents btnStop As Button
    Friend WithEvents btnStart As Button
    Friend WithEvents lstProcess As ListBox
    Friend WithEvents opnFile1 As OpenFileDialog
    Friend WithEvents opnFile2 As OpenFileDialog
    Friend WithEvents lblSubProgress As Label
End Class
