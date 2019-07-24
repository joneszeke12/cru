Imports Microsoft.Win32
Imports System.ComponentModel
Imports System.Security.Principal
Module Header
    Public Property AppWorker As New Worker
    Public Property AppTextBox As TextBox = LogForm.LogTextBox
    Public Property DesktopDirectory As String = My.Computer.FileSystem.SpecialDirectories.Desktop
    Delegate Sub PostToLogDelegate(text As String)
    Public Sub PostToLog(text As String)
        If AppTextBox.InvokeRequired Then
            AppTextBox.Invoke(New PostToLogDelegate(AddressOf AppTextBox.AppendText), {text + vbCrLf})
        Else
            AppTextBox.AppendText(text + vbCrLf)
        End If
    End Sub
    Class Options
        <
            Category("Extraction"),
            DisplayName("Tables"),
            Description("Choose how to display tables."),
            TypeConverter(GetType(Converters.TableStringConverter))
        >
        Public Property Table
            Get
                Return My.Settings.T
            End Get
            Set(value)
                My.Settings.T = value
            End Set
        End Property
        <
            Category("Extraction"),
            DisplayName("Table Fields"),
            Description("Choose how display fields."),
            TypeConverter(GetType(Converters.TableFieldsStringConverter))
        >
        Public Property TableFields
            Get
                Return My.Settings.TF
            End Get
            Set(value)
                My.Settings.TF = value
            End Set
        End Property
        <
            Category("Extraction"),
            DisplayName("Parameters"),
            Description("Choose how to display parameters."),
            TypeConverter(GetType(Converters.ParameterStringConverter))
        >
        Public Property Parameter
            Get
                Return My.Settings.P
            End Get
            Set(value)
                My.Settings.P = value
            End Set
        End Property
        <
            Category("Extraction"),
            DisplayName("Formulas"),
            Description("Choose how to display formulas."),
            TypeConverter(GetType(Converters.FormulaStringConverter))
        >
        Public Property Formula
            Get
                Return My.Settings.F
            End Get
            Set(value)
                My.Settings.F = value
            End Set
        End Property
        <
            Category("Extraction"),
            DisplayName("Report Layout"),
            Description("Choose whether to display report layout."),
            TypeConverter(GetType(Converters.ReportLayoutStringConverter))
        >
        Public Property ReportLayout
            Get
                Return My.Settings.RL
            End Get
            Set(value)
                My.Settings.RL = value
            End Set
        End Property
        <
            Category("Extraction"),
            DisplayName("Subreports"),
            Description("Choose whether to display subreports."),
            TypeConverter(GetType(Converters.SubreportStringConverter))
        >
        Public Property SubReport
            Get
                Return My.Settings.SR
            End Get
            Set(value)
                My.Settings.SR = value
            End Set
        End Property
        <
            Category("Extraction"),
            DisplayName("Preferred Extension"),
            Description("Choose the preferred extension of the extraction file."),
            TypeConverter(GetType(Converters.PreferredExtensionStringConverter))
        >
        Public Property Extension
            Get
                Return My.Settings.PE
            End Get
            Set(value)
                My.Settings.PE = value
            End Set
        End Property
        <
            Category("Directories"),
            DisplayName("Extracts"),
            Description("Choose where extractions will go. Set to 'Default' for desktop."),
            TypeConverter(GetType(Converters.ExtractDirectoryStringConverter))
        >
        Public Property ExtractDirectory
            Get
                Return My.Settings.ED
            End Get
            Set(value)
                Dim dialogResult As DialogResult
                Dim folderBrowserDialog As FolderBrowserDialog
                If value = "New Directory..." Then
                    folderBrowserDialog = New FolderBrowserDialog
                    dialogResult = folderBrowserDialog.ShowDialog()
                    If dialogResult = DialogResult.Cancel Then
                        Exit Property
                    Else
                        My.Settings.ED = folderBrowserDialog.SelectedPath
                    End If
                Else
                    My.Settings.ED = value
                End If
            End Set
        End Property
        <
            Category("Other"),
            DisplayName("Open On Success"),
            Description("Opens output file in default program after a successful operation. Only works for single files."),
            TypeConverter(GetType(Converters.OpenOnSuccessBooleanConverter))
        >
        Public Property OpenOnSuccess
            Get
                Return My.Settings.OOS
            End Get
            Set(value)
                My.Settings.OOS = value
            End Set
        End Property
        <
            Category("Other"),
            DisplayName("Context Menu"),
            Description("Extract when right clicking on RPT files and folders.             
            
            ***Note: If Context Menu setting is currently on and the application has been moved, 
            please toggle the setting off and on so that right-click extraction will work properly.***"),
            TypeConverter(GetType(Converters.ContextMenuStringConverter))
        >
        Public Property ContextMenu
            Get
                Return My.Settings.CM
            End Get
            Set(value)
                Dim appPath As String = Application.ExecutablePath
                Dim subkeyFile As String = "SystemFileAssociations\.rpt\shell\Extract RPT"
                Dim subkeyFolder As String = "Directory\shell\Extract RPT(s) From Folder"
                Dim registryRoot As RegistryKey = Registry.ClassesRoot
                If WindowsIdentity.GetCurrent.Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid) Then
                    Try
                        Select Case value
                            Case "On"
                                registryRoot.CreateSubKey(subkeyFile).CreateSubKey("command").SetValue(String.Empty, appPath + " %1")
                                registryRoot.CreateSubKey(subkeyFolder).CreateSubKey("command").SetValue(String.Empty, appPath + " %1")
                            Case "Off"
                                With registryRoot
                                    .DeleteSubKeyTree(subkeyFile, False)
                                    .DeleteSubKeyTree(subkeyFolder, False)
                                End With
                        End Select
                        My.Settings.CM = value
                    Catch ex As Exception
                        MessageBox.Show(New Form With {.TopMost = True},
                                        ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)
                    End Try
                Else
                    MessageBox.Show(New Form With {.TopMost = True},
                                    "Please re-open program as administrator to change this setting.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)
                End If
            End Set
        End Property
        Class Converters
            Class TableStringConverter
                Inherits StringConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                    Return New StandardValuesCollection(New String() {"Command", "All", "None"})
                End Function
            End Class
            Class TableFieldsStringConverter
                Inherits StringConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                    Return New StandardValuesCollection(New String() {"Used", "All", "None"})
                End Function
            End Class
            Class ParameterStringConverter
                Inherits StringConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                    Return New StandardValuesCollection(New String() {"Used", "All", "None"})
                End Function
            End Class
            Class FormulaStringConverter
                Inherits StringConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                    Return New StandardValuesCollection(New String() {"Used", "All", "None"})
                End Function
            End Class
            Class ReportLayoutStringConverter
                Inherits StringConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                    Return New StandardValuesCollection(New String() {"Include", "Exclude"})
                End Function
            End Class
            Class SubreportStringConverter
                Inherits StringConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                    Return New StandardValuesCollection(New String() {"Include", "Exclude"})
                End Function
            End Class
            Class PreferredExtensionStringConverter
                Inherits StringConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                    Return New StandardValuesCollection(New String() {".txt", ".sql"})
                End Function
            End Class
            Class ExtractDirectoryStringConverter
                Inherits StringConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                    Return New StandardValuesCollection(New String() {"Default", "New Directory..."})
                End Function
            End Class
            Class OpenOnSuccessBooleanConverter
                Inherits BooleanConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
            End Class
            Class ContextMenuStringConverter
                Inherits StringConverter
                Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                    Return True
                End Function
                Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                    Return New StandardValuesCollection(New String() {"On", "Off"})
                End Function
            End Class
        End Class
    End Class
End Module
