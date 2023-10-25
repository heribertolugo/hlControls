Imports System.ComponentModel
Imports System.Windows.Forms.Design

Friend Class DataGridViewRowHeaderXPropertyGrid

    'Public Sub New(ByVal rowHeader As DataGridViewFooterRowHeader, ByVal editorService As System.IServiceProvider)

    '    ' This call is required by the designer.
    '    InitializeComponent()

    '    ' Add any initialization after the InitializeComponent() call.
    '    Me._rowHeader = rowHeader
    '    Me._editorService = editorService.

    '    Me.DataGridViewFooterRowHeader_PrptyGrd.SelectedObject = Me._rowHeader
    'End Sub

    '<Browsable(False)> _
    '<EditorBrowsable(EditorBrowsableState.Never)>
    'Public Property OldHeaderValue As DataGridViewFooterRowHeader
    '    Get
    '        Return _oldHeaderValue
    '    End Get
    '    Set(value As DataGridViewFooterRowHeader)
    '        _oldHeaderValue = value
    '    End Set
    'End Property

    'Public Property NewHeaderValue As DataGridViewFooterRowHeader
    '    Get
    '        Return _newHeaderValue
    '    End Get
    '    Set(value As DataGridViewFooterRowHeader)
    '        _newHeaderValue = value
    '    End Set
    'End Property

    'Private Sub DataGridViewHeaderCellEditor_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    '    Me.DataGridViewFooterRowHeader_PrptyGrd.SelectedObject = _rowHeader
    'End Sub

    Private Sub OK_But_Click(sender As System.Object, e As System.EventArgs) Handles OK_But.Click
        'If Not IsNothing(_newHeaderValue) Then Me.DataGridViewFooterRowHeader_PrptyGrd.SelectedObject = _newHeaderValue
        'Me._editorService.
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_But_Click(sender As System.Object, e As System.EventArgs) Handles Cancel_But.Click
        'If Not IsNothing(_oldHeaderValue) Then Me.DataGridViewFooterRowHeader_PrptyGrd.SelectedObject = _oldHeaderValue
        Me.Close()
    End Sub

End Class