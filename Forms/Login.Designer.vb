<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")>
Partial Class LoginForm
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
    Friend WithEvents UsernameLabel As System.Windows.Forms.Label
    Friend WithEvents PasswordLabel As System.Windows.Forms.Label
    Friend WithEvents UsernameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
    Friend WithEvents LoginButton As System.Windows.Forms.Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoginForm))
        Me.UsernameLabel = New System.Windows.Forms.Label()
        Me.PasswordLabel = New System.Windows.Forms.Label()
        Me.UsernameTextBox = New System.Windows.Forms.TextBox()
        Me.PasswordTextBox = New System.Windows.Forms.TextBox()
        Me.LoginButton = New System.Windows.Forms.Button()
        Me.CancelButton1 = New System.Windows.Forms.Button()
        Me.ServerLabel = New System.Windows.Forms.Label()
        Me.ServerComboBox = New System.Windows.Forms.ComboBox()
        Me.AuthenticationLabel = New System.Windows.Forms.Label()
        Me.AuthenticationComboBox = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'UsernameLabel
        '
        Me.UsernameLabel.Location = New System.Drawing.Point(6, 70)
        Me.UsernameLabel.Name = "UsernameLabel"
        Me.UsernameLabel.Size = New System.Drawing.Size(220, 23)
        Me.UsernameLabel.TabIndex = 0
        Me.UsernameLabel.Text = "User name"
        Me.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PasswordLabel
        '
        Me.PasswordLabel.Location = New System.Drawing.Point(6, 119)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New System.Drawing.Size(220, 23)
        Me.PasswordLabel.TabIndex = 2
        Me.PasswordLabel.Text = "Password"
        Me.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'UsernameTextBox
        '
        Me.UsernameTextBox.Location = New System.Drawing.Point(9, 96)
        Me.UsernameTextBox.Name = "UsernameTextBox"
        Me.UsernameTextBox.Size = New System.Drawing.Size(220, 20)
        Me.UsernameTextBox.TabIndex = 1
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Location = New System.Drawing.Point(9, 145)
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordTextBox.Size = New System.Drawing.Size(220, 20)
        Me.PasswordTextBox.TabIndex = 3
        '
        'LoginButton
        '
        Me.LoginButton.BackColor = System.Drawing.Color.Transparent
        Me.LoginButton.Location = New System.Drawing.Point(12, 226)
        Me.LoginButton.Name = "LoginButton"
        Me.LoginButton.Size = New System.Drawing.Size(94, 23)
        Me.LoginButton.TabIndex = 4
        Me.LoginButton.TabStop = False
        Me.LoginButton.Text = "Login"
        Me.LoginButton.UseVisualStyleBackColor = False
        '
        'CancelButton1
        '
        Me.CancelButton1.BackColor = System.Drawing.Color.Transparent
        Me.CancelButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelButton1.Location = New System.Drawing.Point(135, 226)
        Me.CancelButton1.Name = "CancelButton1"
        Me.CancelButton1.Size = New System.Drawing.Size(94, 23)
        Me.CancelButton1.TabIndex = 5
        Me.CancelButton1.TabStop = False
        Me.CancelButton1.Text = "Cancel"
        Me.CancelButton1.UseVisualStyleBackColor = False
        '
        'ServerLabel
        '
        Me.ServerLabel.Location = New System.Drawing.Point(6, 21)
        Me.ServerLabel.Name = "ServerLabel"
        Me.ServerLabel.Size = New System.Drawing.Size(220, 23)
        Me.ServerLabel.TabIndex = 6
        Me.ServerLabel.Text = "Server"
        Me.ServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ServerComboBox
        '
        Me.ServerComboBox.FormattingEnabled = True
        Me.ServerComboBox.Location = New System.Drawing.Point(9, 46)
        Me.ServerComboBox.Name = "ServerComboBox"
        Me.ServerComboBox.Size = New System.Drawing.Size(220, 21)
        Me.ServerComboBox.TabIndex = 7
        '
        'AuthenticationLabel
        '
        Me.AuthenticationLabel.Location = New System.Drawing.Point(6, 168)
        Me.AuthenticationLabel.Name = "AuthenticationLabel"
        Me.AuthenticationLabel.Size = New System.Drawing.Size(220, 23)
        Me.AuthenticationLabel.TabIndex = 8
        Me.AuthenticationLabel.Text = "Authentication"
        Me.AuthenticationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'AuthenticationComboBox
        '
        Me.AuthenticationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AuthenticationComboBox.FormattingEnabled = True
        Me.AuthenticationComboBox.Items.AddRange(New Object() {"LDAP", "Enterprise"})
        Me.AuthenticationComboBox.Location = New System.Drawing.Point(9, 194)
        Me.AuthenticationComboBox.Name = "AuthenticationComboBox"
        Me.AuthenticationComboBox.Size = New System.Drawing.Size(220, 21)
        Me.AuthenticationComboBox.TabIndex = 9
        Me.AuthenticationComboBox.TabStop = False
        '
        'LoginForm
        '
        Me.AcceptButton = Me.LoginButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightYellow
        Me.ClientSize = New System.Drawing.Size(240, 261)
        Me.Controls.Add(Me.AuthenticationComboBox)
        Me.Controls.Add(Me.AuthenticationLabel)
        Me.Controls.Add(Me.ServerComboBox)
        Me.Controls.Add(Me.ServerLabel)
        Me.Controls.Add(Me.CancelButton1)
        Me.Controls.Add(Me.LoginButton)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.UsernameTextBox)
        Me.Controls.Add(Me.PasswordLabel)
        Me.Controls.Add(Me.UsernameLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginForm"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CR Utility - Login"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ServerLabel As Windows.Forms.Label
    Friend WithEvents ServerComboBox As Windows.Forms.ComboBox
    Friend WithEvents AuthenticationLabel As Windows.Forms.Label
    Friend WithEvents AuthenticationComboBox As Windows.Forms.ComboBox
    Friend WithEvents CancelButton1 As Windows.Forms.Button
End Class
