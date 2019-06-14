<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.FileButton = New System.Windows.Forms.Button()
        Me.FolderButton = New System.Windows.Forms.Button()
        Me.SettingsButton = New System.Windows.Forms.Button()
        Me.MFNotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.MFContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ShowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MFContextMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'FileButton
        '
        Me.FileButton.BackColor = System.Drawing.Color.Transparent
        Me.FileButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.FileButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FileButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FileButton.Location = New System.Drawing.Point(12, 22)
        Me.FileButton.Name = "FileButton"
        Me.FileButton.Size = New System.Drawing.Size(102, 52)
        Me.FileButton.TabIndex = 1
        Me.FileButton.TabStop = False
        Me.FileButton.Text = "Extract " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "From " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "File(s)"
        Me.FileButton.UseVisualStyleBackColor = False
        '
        'FolderButton
        '
        Me.FolderButton.BackColor = System.Drawing.Color.Transparent
        Me.FolderButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.FolderButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FolderButton.Location = New System.Drawing.Point(120, 22)
        Me.FolderButton.Name = "FolderButton"
        Me.FolderButton.Size = New System.Drawing.Size(102, 52)
        Me.FolderButton.TabIndex = 2
        Me.FolderButton.TabStop = False
        Me.FolderButton.Text = "Extract " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "From " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Folder(s)"
        Me.FolderButton.UseVisualStyleBackColor = False
        '
        'SettingsButton
        '
        Me.SettingsButton.BackColor = System.Drawing.Color.Transparent
        Me.SettingsButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SettingsButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SettingsButton.Location = New System.Drawing.Point(46, 95)
        Me.SettingsButton.Name = "SettingsButton"
        Me.SettingsButton.Size = New System.Drawing.Size(141, 23)
        Me.SettingsButton.TabIndex = 4
        Me.SettingsButton.TabStop = False
        Me.SettingsButton.Text = "Settings"
        Me.SettingsButton.UseVisualStyleBackColor = False
        '
        'MFNotifyIcon
        '
        Me.MFNotifyIcon.ContextMenuStrip = Me.MFContextMenuStrip
        Me.MFNotifyIcon.Text = "CR Utility"
        Me.MFNotifyIcon.Visible = True
        '
        'MFContextMenuStrip
        '
        Me.MFContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MFContextMenuStrip.Name = "MFNIContextMenuStrip"
        Me.MFContextMenuStrip.Size = New System.Drawing.Size(104, 48)
        '
        'ShowToolStripMenuItem
        '
        Me.ShowToolStripMenuItem.Name = "ShowToolStripMenuItem"
        Me.ShowToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.ShowToolStripMenuItem.Text = "Show"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightYellow
        Me.ClientSize = New System.Drawing.Size(232, 130)
        Me.Controls.Add(Me.FileButton)
        Me.Controls.Add(Me.SettingsButton)
        Me.Controls.Add(Me.FolderButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CR Utility"
        Me.MFContextMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FileButton As Windows.Forms.Button
    Friend WithEvents FolderButton As Windows.Forms.Button
    Friend WithEvents SettingsButton As Windows.Forms.Button
    Friend WithEvents MFNotifyIcon As Windows.Forms.NotifyIcon
    Friend WithEvents MFContextMenuStrip As Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowToolStripMenuItem As Windows.Forms.ToolStripMenuItem
End Class
