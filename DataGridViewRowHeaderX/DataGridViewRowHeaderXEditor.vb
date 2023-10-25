Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.ComponentModel.Design

Friend Class DataGridViewRowHeaderXEditor
    Inherits UITypeEditor

    Private WithEvents propEditor As New DataGridViewRowHeaderXPropertyGrid
    Private oldVal As New DataGridViewRowHeaderX


    Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext, provider As System.IServiceProvider, value As Object) As Object
        oldVal = New DataGridViewRowHeaderX
        propEditor = New DataGridViewRowHeaderXPropertyGrid

        value = CType(value, DataGridViewRowHeaderX)

        If (context IsNot Nothing) And (context.Instance IsNot Nothing) And (provider IsNot Nothing) Then
            If Not IsNothing(CType(value, DataGridViewRowHeaderX)) Then
                oldVal = CType(CType(value, DataGridViewRowHeaderX).Clone, DataGridViewRowHeaderX)
                propEditor.DataGridViewFooterRowHeader_PrptyGrd.SelectedObject = value
            End If
            Dim result As Windows.Forms.DialogResult = propEditor.ShowDialog()
            If result = Windows.Forms.DialogResult.OK Then
                value = CType(CType(propEditor.DataGridViewFooterRowHeader_PrptyGrd.SelectedObject, DataGridViewRowHeaderX).Clone, DataGridViewRowHeaderX)
            Else
                value = oldVal ' CType(oldVal.Clone, DataGridViewFooterRowHeader)
            End If
        Else
            value = oldVal.Clone
        End If

        Return MyBase.EditValue(context, provider, CType(value, DataGridViewRowHeaderX))
    End Function

    Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        If (Not IsNothing(context)) And (Not IsNothing(context.Instance)) Then
            Return UITypeEditorEditStyle.Modal
        End If

        Return MyBase.GetEditStyle(context)
    End Function

End Class

'Friend Class DataGridViewFooterHeaderImageEditor
'    Inherits UITypeEditor

'    'Private WithEvents propEditor As New SelectResourceDialog
'    Private oldVal As New DataGridViewFooterRowHeader


'    Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext, provider As System.IServiceProvider, value As Object) As Object
'        If (context IsNot Nothing) And (context.Instance IsNot Nothing) And (provider IsNot Nothing) Then
'            If Not IsNothing(CType(value, DataGridViewFooterRowHeader)) Then
'                oldVal = CType(CType(value, DataGridViewFooterRowHeader).Clone, DataGridViewFooterRowHeader)
'                propEditor.DataGridViewFooterRowHeader_PrptyGrd.SelectedObject = CType(value, DataGridViewFooterRowHeader)
'            End If
'            Dim result As Windows.Forms.DialogResult = propEditor.ShowDialog()
'            If result = Windows.Forms.DialogResult.OK Then
'                value = propEditor.DataGridViewFooterRowHeader_PrptyGrd.SelectedObject
'            Else
'                value = oldVal
'            End If
'        End If

'        Return MyBase.EditValue(context, provider, value)
'    End Function

'    Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
'        If (Not IsNothing(context)) And (Not IsNothing(context.Instance)) Then
'            Return UITypeEditorEditStyle.Modal
'        End If

'        Return MyBase.GetEditStyle(context)
'    End Function
'End Class
