Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel.Design

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''' <summary>
''' The Design Time Designer for DGVfooter. Allows the footer to be inserted during design time.
''' </summary>
''' <remarks></remarks>
<System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name:="FullTrust")> _
Friend NotInheritable Class DataGridViewFooterDesigner
    Inherits System.Windows.Forms.Design.ControlDesigner

    Public Overrides Function CanBeParentedTo(parentDesigner As System.ComponentModel.Design.IDesigner) As Boolean

        If TypeOf parentDesigner.Component Is DataGridView Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides ReadOnly Property SelectionRules() As  _
System.Windows.Forms.Design.SelectionRules
        Get
            Return Windows.Forms.Design.SelectionRules.Visible Or Windows.Forms.Design.SelectionRules.Locked
        End Get
    End Property

    Public Overrides Sub Initialize(ByVal component As System.ComponentModel.IComponent)
        MyBase.Initialize(component)

        Dim control As DataGridViewFooter = CType(component, DataGridViewFooter)

        If control Is Nothing Then
            Throw New ArgumentException()
        End If

        '' pick up the current state and push it
        '' into our shadow props to 
        '' initialize them.
        ''
        Me.IsDesigner = control.IsDesigner

        control.IsDesigner = True
    End Sub


    Public Overrides Sub InitializeNewComponent(defaultValues As System.Collections.IDictionary)

        MyBase.InitializeNewComponent(defaultValues)

        Dim f As Form = CType(Me.Component, Control).FindForm
        Dim aPoint As Point = New Point(CType(Me.Component, DataGridViewFooter).Location.X - 1, CType(Me.Component, DataGridViewFooter).Location.Y - 1)
        Dim target As Control = f.GetChildAtPoint(aPoint)
        Dim dgvType As Type = GetType(DataGridView)

        If target.GetType = dgvType Then
            If target.Controls.OfType(Of DataGridViewFooter).Count > 0 Then
                KillComponent(CType(Me.Component, DataGridViewFooter).Name & " Not Allowed! " & vbCr & "Only one footer per DataGridView!")
            Else
                If Not CType(Component, DataGridViewFooter).Parent.GetType = GetType(DataGridView) Then

                    CType(Me.Component, DataGridViewFooter).Parent = CType(target, DataGridView)
                    CType(Me.Component, DataGridViewFooter).SetParent(CType(target, DataGridView))
                    Dim host As IDesignerHost

                    host = CType(Me.ParentComponent.Site.GetService(GetType(IDesignerHost)), IDesignerHost)
                    'host.CreateComponent(GetType(Component))
                    host.Container.Add(Component)
                End If

            End If
        Else
            KillComponent("Footer can only be added to DataGridView!")
        End If
    End Sub

    Public Property IsDesigner As Boolean
        Get
            Return CBool(ShadowProperties("IsDesigner"))
        End Get
        Set(value As Boolean)
            Me.ShadowProperties("IsDesigner") = True
        End Set
    End Property

    Private Sub KillComponent(Optional ByVal err As String = "")
        Dim e As New Exception(err)
        Dim host As IDesignerHost

        If Not err = "" Then MyBase.DisplayError(e)

        host = CType(Me.ParentComponent.Site.GetService(GetType(IDesignerHost)), IDesignerHost)
        host.DestroyComponent(Component)
    End Sub

End Class
