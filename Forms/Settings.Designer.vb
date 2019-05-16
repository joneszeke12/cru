<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SettingsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SettingsForm))
        Me.MyPropertyGrid = New System.Windows.Forms.PropertyGrid()
        Me.DefaultButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'MyPropertyGrid
        '
        Me.MyPropertyGrid.Location = New System.Drawing.Point(12, 12)
        Me.MyPropertyGrid.Name = "MyPropertyGrid"
        Me.MyPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized
        Me.MyPropertyGrid.Size = New System.Drawing.Size(303, 369)
        Me.MyPropertyGrid.TabIndex = 0
        '
        'DefaultButton
        '
        Me.DefaultButton.Location = New System.Drawing.Point(126, 402)
        Me.DefaultButton.Name = "DefaultButton"
        Me.DefaultButton.Size = New System.Drawing.Size(75, 23)
        Me.DefaultButton.TabIndex = 1
        Me.DefaultButton.Text = "Default"
        Me.DefaultButton.UseVisualStyleBackColor = True
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightYellow
        Me.ClientSize = New System.Drawing.Size(327, 437)
        Me.Controls.Add(Me.DefaultButton)
        Me.Controls.Add(Me.MyPropertyGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SettingsForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CR Utility - Settings"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MyPropertyGrid As Windows.Forms.PropertyGrid
    Friend WithEvents DefaultButton As Windows.Forms.Button
End Class
