<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DataGridViewRowHeaderXPropertyGrid
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
        Me.DataGridViewFooterRowHeader_PrptyGrd = New System.Windows.Forms.PropertyGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Cancel_But = New System.Windows.Forms.Button()
        Me.OK_But = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridViewFooterRowHeader_PrptyGrd
        '
        Me.DataGridViewFooterRowHeader_PrptyGrd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewFooterRowHeader_PrptyGrd.Location = New System.Drawing.Point(0, 0)
        Me.DataGridViewFooterRowHeader_PrptyGrd.Name = "DataGridViewFooterRowHeader_PrptyGrd"
        Me.DataGridViewFooterRowHeader_PrptyGrd.Size = New System.Drawing.Size(447, 396)
        Me.DataGridViewFooterRowHeader_PrptyGrd.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Cancel_But)
        Me.Panel1.Controls.Add(Me.OK_But)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 396)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(447, 49)
        Me.Panel1.TabIndex = 1
        '
        'Cancel_But
        '
        Me.Cancel_But.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_But.Location = New System.Drawing.Point(360, 13)
        Me.Cancel_But.Name = "Cancel_But"
        Me.Cancel_But.Size = New System.Drawing.Size(75, 23)
        Me.Cancel_But.TabIndex = 1
        Me.Cancel_But.Text = "Cancel"
        Me.Cancel_But.UseVisualStyleBackColor = True
        '
        'OK_But
        '
        Me.OK_But.Location = New System.Drawing.Point(279, 13)
        Me.OK_But.Name = "OK_But"
        Me.OK_But.Size = New System.Drawing.Size(75, 23)
        Me.OK_But.TabIndex = 0
        Me.OK_But.Text = "OK"
        Me.OK_But.UseVisualStyleBackColor = True
        '
        'DataGridViewHeaderPropertyGrid
        '
        Me.AcceptButton = Me.OK_But
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_But
        Me.ClientSize = New System.Drawing.Size(447, 445)
        Me.Controls.Add(Me.DataGridViewFooterRowHeader_PrptyGrd)
        Me.Controls.Add(Me.Panel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DataGridViewHeaderPropertyGrid"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Footer HeaderCell Builder"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridViewFooterRowHeader_PrptyGrd As System.Windows.Forms.PropertyGrid
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Cancel_But As System.Windows.Forms.Button
    Friend WithEvents OK_But As System.Windows.Forms.Button
End Class
