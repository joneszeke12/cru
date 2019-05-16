<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RepositoryForm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RepositoryForm))
        Me.RepositoryImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.RepositorySplitContainer = New System.Windows.Forms.SplitContainer()
        Me.RepositoryContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExtractToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RepositoryTree = New cru.RepositoryTree()
        Me.RepositoryList = New cru.RepositoryList()
        Me.ObjName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ObjType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ObjCreated = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ObjModified = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        CType(Me.RepositorySplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RepositorySplitContainer.Panel1.SuspendLayout()
        Me.RepositorySplitContainer.Panel2.SuspendLayout()
        Me.RepositorySplitContainer.SuspendLayout()
        Me.RepositoryContextMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'RepositoryImageList
        '
        Me.RepositoryImageList.ImageStream = CType(resources.GetObject("RepositoryImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.RepositoryImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.RepositoryImageList.Images.SetKeyName(0, "Blank")
        Me.RepositoryImageList.Images.SetKeyName(1, "Folder")
        Me.RepositoryImageList.Images.SetKeyName(2, "Report")
        '
        'RepositorySplitContainer
        '
        Me.RepositorySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RepositorySplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.RepositorySplitContainer.Name = "RepositorySplitContainer"
        '
        'RepositorySplitContainer.Panel1
        '
        Me.RepositorySplitContainer.Panel1.Controls.Add(Me.RepositoryTree)
        '
        'RepositorySplitContainer.Panel2
        '
        Me.RepositorySplitContainer.Panel2.Controls.Add(Me.RepositoryList)
        Me.RepositorySplitContainer.Size = New System.Drawing.Size(873, 454)
        Me.RepositorySplitContainer.SplitterDistance = 291
        Me.RepositorySplitContainer.TabIndex = 0
        '
        'RepositoryContextMenuStrip
        '
        Me.RepositoryContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExtractToolStripMenuItem, Me.DownloadToolStripMenuItem})
        Me.RepositoryContextMenuStrip.Name = "RepoContextMenuStrip"
        Me.RepositoryContextMenuStrip.Size = New System.Drawing.Size(129, 48)
        '
        'ExtractToolStripMenuItem
        '
        Me.ExtractToolStripMenuItem.Name = "ExtractToolStripMenuItem"
        Me.ExtractToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExtractToolStripMenuItem.Text = "Extract"
        '
        'DownloadToolStripMenuItem
        '
        Me.DownloadToolStripMenuItem.Name = "DownloadToolStripMenuItem"
        Me.DownloadToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.DownloadToolStripMenuItem.Text = "Download"
        '
        'RepositoryTree
        '
        Me.RepositoryTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RepositoryTree.ImageIndex = 0
        Me.RepositoryTree.ImageList = Me.RepositoryImageList
        Me.RepositoryTree.Location = New System.Drawing.Point(0, 0)
        Me.RepositoryTree.Name = "RepositoryTree"
        Me.RepositoryTree.SelectedImageIndex = 0
        Me.RepositoryTree.Size = New System.Drawing.Size(291, 454)
        Me.RepositoryTree.TabIndex = 0
        Me.RepositoryTree.TabStop = False
        '
        'RepositoryList
        '
        Me.RepositoryList.Activation = System.Windows.Forms.ItemActivation.TwoClick
        Me.RepositoryList.AllowColumnReorder = True
        Me.RepositoryList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ObjName, Me.ObjType, Me.ObjCreated, Me.ObjModified})
        Me.RepositoryList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RepositoryList.Location = New System.Drawing.Point(0, 0)
        Me.RepositoryList.Name = "RepositoryList"
        Me.RepositoryList.Size = New System.Drawing.Size(578, 454)
        Me.RepositoryList.SmallImageList = Me.RepositoryImageList
        Me.RepositoryList.TabIndex = 0
        Me.RepositoryList.TabStop = False
        Me.RepositoryList.UseCompatibleStateImageBehavior = False
        Me.RepositoryList.View = System.Windows.Forms.View.Details
        '
        'ObjName
        '
        Me.ObjName.Text = "Name"
        Me.ObjName.Width = 267
        '
        'ObjType
        '
        Me.ObjType.Text = "Type"
        Me.ObjType.Width = 62
        '
        'ObjCreated
        '
        Me.ObjCreated.Text = "Created Date"
        Me.ObjCreated.Width = 122
        '
        'ObjModified
        '
        Me.ObjModified.Text = "Last Modified"
        Me.ObjModified.Width = 123
        '
        'RepositoryForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(873, 454)
        Me.Controls.Add(Me.RepositorySplitContainer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RepositoryForm"
        Me.RepositorySplitContainer.Panel1.ResumeLayout(False)
        Me.RepositorySplitContainer.Panel2.ResumeLayout(False)
        CType(Me.RepositorySplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RepositorySplitContainer.ResumeLayout(False)
        Me.RepositoryContextMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RepositoryImageList As Windows.Forms.ImageList
    Friend WithEvents RepositorySplitContainer As Windows.Forms.SplitContainer
    Friend WithEvents RepositoryTree As RepositoryTree
    Friend WithEvents RepositoryList As RepositoryList
    Friend WithEvents ObjName As Windows.Forms.ColumnHeader
    Friend WithEvents ObjType As Windows.Forms.ColumnHeader
    Friend WithEvents ObjCreated As Windows.Forms.ColumnHeader
    Friend WithEvents ObjModified As Windows.Forms.ColumnHeader
    Friend WithEvents RepositoryContextMenuStrip As Windows.Forms.ContextMenuStrip
    Friend WithEvents ExtractToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents DownloadToolStripMenuItem As Windows.Forms.ToolStripMenuItem
End Class
