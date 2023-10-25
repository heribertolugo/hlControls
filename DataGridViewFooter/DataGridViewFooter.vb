
Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Imports HL
Imports System.Drawing.Drawing2D

'***********************************************************************
'   Author: Heriberto Lugo
'   Website: heribertolugo.com
'   Description: DGVfooter_V1. 
'   A basic pseudo footer for use in a .net datagridview.
'   Please keep these lines in any source files.
'***********************************************************************


'Namespace HL.Managed.Forms.Controls
''' <summary>
''' A Footer for a DataGridView with autosum capabilities.
''' </summary>
''' <remarks></remarks>
<Designer(GetType(HL.Managed.Forms.Controls.DataGridViewFooterDesigner)), DesignerCategory("Component")> _
<ToolboxBitmap(GetType(DataGridViewFooterDesigner), "DataGridViewFooter.bmp")> _
Public Class DataGridViewFooter
    Inherits System.Windows.Forms.Control

#Region "Private Class Members"
    ''' <summary>
    ''' Holds our parent DGV, to which DGVfooter is bound to as footer.
    ''' </summary>
    Private WithEvents _parentDGV As DataGridView
    ''' <summary>
    ''' Used to store value as to whether or not to kill our parents RowAddedEvent.
    ''' </summary>
    ''' <remarks>This is used to help do operations which would recursively call our parent's RowAddedEvent. So me must stop the event from being called, and store whether we have done so in this var.</remarks>
    Private _killParentRowAddedEvent As Boolean = False

    Private _killAddColumns As Boolean = True
    Private _killRemoveColumns As Boolean = True
    ''' <summary>
    ''' Whether we perform calculations on columns as data is insrted into cells
    ''' </summary>
    ''' <remarks></remarks>
    Private _autoCalc As Boolean = True
    ''' <summary>
    ''' How many decimal places a double type should display
    ''' </summary>
    ''' <remarks></remarks>
    Private _decimalPlaces As Integer = 2
    ''' <summary>
    ''' The descriptive suffix apended to the end of the totals in footer cells.
    ''' </summary>
    ''' <remarks></remarks>
    Private _valueSuffix As String = ""
    ''' <summary>
    ''' The descriptive prefix apended to the beginning of the totals in footer cells.
    ''' </summary>
    ''' <remarks></remarks>
    Private _valuePrefix As String = ""
    ''' <summary>
    ''' Whether the value in footer cell should be displayed as currency.
    ''' </summary>
    ''' <remarks></remarks>
    Private _isCurrency As Boolean = False
    ''' <summary>
    ''' Whether the first footer cell should be a descriptive header cell.
    ''' </summary>
    ''' <remarks></remarks>
    Private _useHeader As Boolean = False
    ''' <summary>
    ''' The heading in the footer's first cell. Default is "Totals"
    ''' </summary>
    ''' <remarks></remarks>
    Private _headerCellText As String = "Totals"
    ''' <summary>
    ''' The backcolor of the first cell in footer if used as header cell.
    ''' </summary>
    ''' <remarks></remarks>
    Private _headerCellBackColor As Color = Color.FromKnownColor(KnownColor.White)
    ''' <summary>
    ''' The forecolor of the first cell in footer if used as header cell.
    ''' </summary>
    ''' <remarks></remarks>
    Private _headerCellForeColor As Color = Color.Red
    ''' <summary>
    ''' List of columns which are to be summed.
    ''' </summary>
    ''' <remarks></remarks>
    Private _columnsToSum As New List(Of String)
    ''' <summary>
    ''' Rounds the sum upto the decimalPlaces chosen to display
    ''' </summary>
    ''' <remarks></remarks>
    Private _roundSum As Boolean = False
    ''' <summary>
    ''' Whether to use AwayFromZero rounding or ToEven (bankers) rounding.
    ''' </summary>
    ''' <remarks></remarks>
    Private _bankersRounding As Boolean = False
    ''' <summary>
    ''' The Font for the Cells in the Footer.
    ''' </summary>
    ''' <remarks></remarks>
    Private _font As Font
    ''' <summary>
    ''' The Foreground Color for the Footer.
    ''' </summary>
    ''' <remarks></remarks>
    Private _foreColor As Color
    ''' <summary>
    ''' The Background Color for the footer.
    ''' </summary>
    ''' <remarks></remarks>
    Private _backColor As Color
    ''' <summary>
    ''' The Grid Color for the footer.
    ''' </summary>
    ''' <remarks></remarks>
    Private _gridColor As Color
    ''' <summary>
    ''' The Background Color for the Cells in the Footer.
    ''' </summary>
    ''' <remarks></remarks>
    Private _cellBackColor As Color
    ''' <summary>
    ''' Whether we are in Designer mode.
    ''' </summary>
    ''' <remarks></remarks>
    Private _isDesigner As Boolean = False
    ''' <summary>
    ''' The DataGridView which is used as footer
    ''' </summary>
    ''' <remarks>This will be changed in future version. A custom control will be used to replace this DataGridView</remarks>
    Private localDGV As DataGridView

    Private WithEvents _RowHeader As DataGridViewRowHeaderX

#End Region

#Region "Constructor, and initial set-up"

    ''' <summary>
    ''' Contructor
    ''' </summary>
    ''' <param name="parentDGV">DataGridView which we will be bound to as a footer.</param>
    ''' <remarks></remarks>
    Public Sub New(ByRef parentDGV As DataGridView)
        localDGV = New DataGridView
        _RowHeader = New DataGridViewRowHeaderX
        SetUp(parentDGV)
    End Sub

    ''' <summary>
    ''' This constructor should not be used. Please specify the parent/host datagridview. This is for Designer use.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        localDGV = New DataGridView
        _RowHeader = New DataGridViewRowHeaderX
    End Sub

    ''' <summary>
    ''' Actions that need to be performed when creating a new instance.
    ''' </summary>
    ''' <param name="parentDGV">The host DataGridView which will contain the footer.</param>
    ''' <remarks>This was seperated from the constructor for Design Time Designer compatability.</remarks>
    Private Sub SetUp(ByRef parentDGV As DataGridView)
        _parentDGV = parentDGV

        localDGV = New DataGridView
        localDGV.Name = parentDGV.Name & "_footerDGV"

        'This kills me. Not sure why, but both localDGV and _RowHeader must be re-instantiated.
        'Otherwise an exception occurs. 
        'In order for us to keep any changes made to RowHeader before a row is added to footer,
        'we have to create a copy, re-instantiate _RowHeader, and copy the copy we just created back to _RowHeader
        Dim tmpRowHeader As New DataGridViewRowHeaderX
        tmpRowHeader = CType(_RowHeader.Clone, DataGridViewRowHeaderX)

        _RowHeader = New DataGridViewRowHeaderX
        _RowHeader = CType(tmpRowHeader.Clone, DataGridViewRowHeaderX)

        SetBaseProperties()
        Me.Controls.Add(localDGV)

        parentDGV.Controls.Add(Me)

        If IsNothing(_font) Then _font = If(Not IsNothing(parentDGV.DefaultCellStyle), parentDGV.DefaultCellStyle.Font, If(Not IsNothing(parentDGV.RowsDefaultCellStyle), parentDGV.RowsDefaultCellStyle.Font, New Font(FontFamily.GenericSansSerif, 9, FontStyle.Regular, GraphicsUnit.Point)))
    End Sub

    ''' <summary>
    ''' Sets the fundamental properties required for this DGV which acts as a footer row for another DGV
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetBaseProperties()
        Me.Height = 25
        Me.Width = _parentDGV.Width

        With Me.localDGV
            .RowHeadersVisible = False
            .ColumnHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .ScrollBars = Windows.Forms.ScrollBars.None
            .DefaultCellStyle.SelectionBackColor = Me._parentDGV.DefaultCellStyle.BackColor
            .DefaultCellStyle.SelectionForeColor = Me._parentDGV.ForeColor
            .DefaultCellStyle.Font = Me._font
            .DefaultCellStyle.Alignment = Me._parentDGV.DefaultCellStyle.Alignment
            .Location = New Point(0, 0)
            .Dock = DockStyle.Fill

            .BackgroundColor = If(Me._backColor = Nothing, If(Me._parentDGV.BackgroundColor = Nothing, Color.FromKnownColor(KnownColor.AppWorkspace), Me._parentDGV.BackgroundColor), Me._backColor)
            Me._backColor = .BackgroundColor
            .DefaultCellStyle.ForeColor = If(Me._foreColor = Nothing, If(Me._parentDGV.DefaultCellStyle.ForeColor = Nothing, Color.Black, Me._parentDGV.DefaultCellStyle.ForeColor), Me._foreColor)
            Me._foreColor = .DefaultCellStyle.ForeColor
            .DefaultCellStyle.BackColor = If(Me._cellBackColor = Nothing, If(Me._parentDGV.DefaultCellStyle.BackColor = Nothing, Color.White, Me._parentDGV.DefaultCellStyle.BackColor), Me._cellBackColor)
            Me._cellBackColor = .DefaultCellStyle.BackColor
            .GridColor = If(Me._gridColor = Nothing, If(Me._parentDGV.GridColor = Nothing, Color.FromKnownColor(KnownColor.ControlDark), Me._parentDGV.GridColor), Me._gridColor)
            Me._gridColor = .GridColor
        End With

        Me.Dock = DockStyle.Bottom

        If _parentDGV.ColumnCount > 0 Then
            Me.SetColumns(_parentDGV)
            Me.SumAllColumns()
        End If
    End Sub

    ''' <summary>
    ''' Adds corresponding columns to our footer from our parent DGV
    ''' </summary>
    ''' <param name="parentsdgv">The parent/owing datagridview to which this footer is added to.</param>
    ''' <remarks></remarks>
    Public Sub SetColumns(ByRef parentsdgv As DataGridView)

        If Me._parentDGV.Columns.Count > 0 Then
            Me._killAddColumns = False

            For Each c As DataGridViewColumn In _parentDGV.Columns
                Dim childCol As New DataGridViewTextBoxColumn
                Dim childColName As String = c.Name & "_footer"

                If localDGV.Columns.Contains(childColName) Then Continue For

                childCol.Name = childColName
                childCol.Width = c.Width
                childCol.ReadOnly = True
                childCol.Resizable = DataGridViewTriState.False
                childCol.HeaderText = c.Name
                childCol.DefaultCellStyle = c.DefaultCellStyle

                localDGV.Columns.Add(childCol)
                localDGV.Columns(c.Index).Frozen = c.Frozen
                localDGV.Columns(c.Index).FillWeight = c.FillWeight

                If localDGV.RowCount = 0 Then Me.AddRow()

                Me.ColumnToSum(c.Name) = True
            Next

            localDGV.RowHeadersVisible = _parentDGV.RowHeadersVisible

            Me._killAddColumns = True
        End If
    End Sub

    ''' <summary>
    ''' Used when an instance must be initialized without specifying parent DGV.
    ''' Should only be used for Design Time Designer.
    ''' </summary>
    ''' <param name="dgv">The parent DataGridView</param>
    ''' <remarks>This calls all the actions called when creating an instance with specifying the parent DataGridView.</remarks>
    Friend Sub SetParent(ByRef dgv As DataGridView)
        Me.Parent = dgv
        Me.SetUp(dgv)
    End Sub
#End Region

#Region "Properties"


    ''' <summary>
    ''' If set to true, footer will autosum the columns in parent datagridview
    ''' </summary>
    ''' <value>A boolean indicating whether footer should autosum parent dgv columns.</value>
    ''' <returns>True if set to autocalc. Otherwise false.</returns>
    ''' <remarks>If this is set to false after footer has already sumed columns, the values will not be removed from footer.
    ''' But no further autosum will be performed.</remarks>
    <Description("If the Footer will calculate the sum of Columns in parent as values are entered."), _
    Category("Action")> _
    <DefaultValue(True)> _
    Public Property AutoCalc As Boolean
        Get
            Return _autoCalc
        End Get
        Set(value As Boolean)
            _autoCalc = value

            If IsDesigner Then Exit Property
            SumAllColumns()
        End Set
    End Property

    ''' <summary>
    ''' The descriptive prefix apended to the beginning of the totals in footer cells.
    ''' </summary>
    ''' <value>String to be used as the descriptive suffix apended to the end of the totals in footer cells.</value>
    ''' <returns>The descriptive suffix apended to the end of the totals in footer cells.</returns>
    ''' <remarks></remarks>
    <Description("A value that will be appended to the beginning of the sum for each Column in parent."), _
    Category("Format")> _
    <DefaultValue("")> _
    Public Property ValuePrefix As String
        Get
            Return _valuePrefix
        End Get
        Set(value As String)
            _valuePrefix = value

            If IsDesigner Then Exit Property
            SumAllColumns()
        End Set
    End Property

    ''' <summary>
    ''' The descriptive suffix apended to the end of the totals in footer cells.
    ''' </summary>
    ''' <value>String to be used as the descriptive suffix apended to the end of the totals in footer cells.</value>
    ''' <returns>The descriptive suffix apended to the end of the totals in footer cells.</returns>
    ''' <remarks></remarks>
    <Description("A value that will be appended to the end of the sum for each Column in parent."), _
    Category("Format")> _
    <DefaultValue("")> _
    Public Property ValueSuffix As String
        Get
            Return _valueSuffix
        End Get
        Set(value As String)
            _valueSuffix = value

            If IsDesigner Then Exit Property
            SumAllColumns()
        End Set
    End Property

    ''' <summary>
    ''' Whether the value in footer cell should be displayed as currency
    ''' </summary>
    ''' <value>Boolean indicating whether the value in footer cell should be displayed as currency.</value>
    ''' <returns>Boolean indicating whether the value in footer cell is displayed as currency.</returns>
    ''' <remarks></remarks>
    <Description("A Boolean indicating if values are to be displayed as the local currency."), _
    Category("Format")> _
    <DefaultValue(False)> _
    Public Property IsCurrency As Boolean
        Get
            Return _isCurrency
        End Get
        Set(value As Boolean)
            _isCurrency = value
            SumAllColumns()
        End Set
    End Property

    ''' <summary>
    ''' Whether the first footer cell should be a descriptive header cell.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Whether the first footer cell will be used as a descriptive 'Header'."), _
    Category("Format")> _
    <DefaultValue(False)> _
    Public Property UseHeaderCell As Boolean
        Get
            Return _useHeader
        End Get
        Set(value As Boolean)
            _useHeader = value

            If localDGV.Columns.Count > 0 Then

                ColumnToSum(0) = Not value

                If localDGV.RowCount > 0 Then
                    SumAllColumns()

                    If value Then
                        SetHeader()
                    Else
                        UnSetHeader()
                    End If
                End If
            End If

        End Set
    End Property

    ''' <summary>
    ''' The heading in the footer's header cell. Default is "Totals"
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("The Text displayed in first cell when UseHeader is enabled."), _
    Category("Format")> _
    <DefaultValue("Totals")> _
    Public Property HeaderCellText As String
        Get
            Return _headerCellText
        End Get
        Set(value As String)
            _headerCellText = value
            SetHeader()
        End Set
    End Property

    ''' <summary>
    ''' The backcolor for the header cell.
    ''' </summary>
    ''' <value>A color for which to set the header backcolor to.</value>
    ''' <returns>A color which corresponds to the backcolor for the header cell.</returns>
    ''' <remarks></remarks>
    <Description("The background color of the Text is the first cell when UseHeader is enabled."), _
    Category("Appearance")> _
    <DefaultValue(KnownColor.White)> _
    Public Property HeaderCellBackColor As Color
        Get
            Return _headerCellBackColor
        End Get
        Set(value As Color)
            _headerCellBackColor = value
            SetHeader()
        End Set
    End Property


    ''' <summary>
    ''' The forecolor for the header cell.
    ''' </summary>
    ''' <value>A color for which to set the header forecolor to.</value>
    ''' <returns>A color which corresponds to the forecolor for the header cell.</returns>
    <Description("The foreground color of the Text is the first cell when UseHeader is enabled."), _
    Category("Appearance")> _
    <DefaultValue(KnownColor.Red)> _
    Public Property HeaderCellForeColor As Color
        Get
            Return _headerCellForeColor
        End Get
        Set(value As Color)
            _headerCellForeColor = value
            SetHeader()
        End Set
    End Property

    ''' <summary>
    ''' How many decimal places will the values displayed have.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("The amount of digits allowed to the right of the decimal in the cells' values."), _
    Category("Format")> _
    <DefaultValue(2)> _
    Public Property DecimalPlaces As Integer
        Get
            Return _decimalPlaces
        End Get
        Set(value As Integer)
            _decimalPlaces = value

            If IsDesigner Then Exit Property
            SumAllColumns()
        End Set
    End Property

    ''' <summary>
    ''' Value indicating whether the column in parent dgv will be totalled.
    ''' </summary>
    ''' <param name="columnName">The name of a column in parent dgv.</param>
    ''' <value>Boolean indicating whether column will be totalled.</value>
    ''' <returns>Boolean indicating whether column will be totalled.</returns>
    ''' <remarks></remarks>
    <Description("Specifies whether the column in the parent DataGridView will have its values summed."), _
    Category("Data")> _
    Public Property ColumnToSum(ByVal columnName As String) As Boolean
        '<DefaultValue(""), EditorAttribute(GetType(myTreeViewPropertyEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        '<Browsable(False)> _
        '<DefaultValue(True)> _
        Get
            'If the _columnsToSum contains the name of column, then that column will be totaled.
            Return _columnsToSum.Contains(columnName)
        End Get
        Set(value As Boolean)
            If localDGV.Columns.Count < 1 Then Exit Property
            If _parentDGV.Columns.Count > 0 Then
                If UseHeaderCell And _parentDGV.Columns(0).Name = columnName Then value = False
            End If

            If value Then
                'If we are setting a column to be totaled, and it is not in _columnsToSum list, we must add it - so it can be totaled.
                If Not _columnsToSum.Contains(columnName) Then
                    'Insert the column we are setting to be totaled.
                    _columnsToSum.Add(columnName)

                    SumColumn(columnName)
                End If
            Else
                'If we are setting a column to not be totaled, and it is in _columnsToSum lsit, we must remove it - so it can not be totaled.
                If _columnsToSum.Contains(columnName) Then
                    _columnsToSum.Remove(columnName)
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Value indicating whether the column in parent dgv will be totalled.
    ''' </summary>
    ''' <param name="columnIndex">The index of a column in parent dgv.</param>
    ''' <value>Boolean indicating whether column will be totalled.</value>
    ''' <returns>Boolean indicating whether column will be totalled.</returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public Property ColumnToSum(ByVal columnIndex As Integer) As Boolean
        '<Description("Specifies whether the column in the parent DataGridView will have its values summed."), _
        'Category("Data")> _
        '<DefaultValue(True)> _
        'Lets be a little lazy/smart and just call this property using the name.
        'We could just perform needed actions using the index, but im sure we are getting a displayindex number, and not the actual index.
        'So to be safe, we will get the name from the index passed and call the property using columnName instead.
        'Besides this avoids recoding the same exact thing more than once, just to use index rather than columnName.
        Get
            Dim columnName As String = Me._parentDGV.Columns(columnIndex).Name
            Return ColumnToSum(columnName)
        End Get
        Set(value As Boolean)
            Dim columnName As String = Me._parentDGV.Columns(columnIndex).Name
            ColumnToSum(columnName) = value
        End Set
    End Property

    ''' <summary>
    ''' Whether to round the totals displayed in footer.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Specifies whether values in the cells will be rounded."), _
    Category("Action")> _
    <DefaultValue(False)> _
    Public Property RoundSum As Boolean
        Get
            Return _roundSum
        End Get
        Set(value As Boolean)
            _roundSum = value

            If IsDesigner Then Exit Property
            SumAllColumns()
        End Set
    End Property

    ''' <summary>
    ''' Whether to use "bankers" rounding when rounding the totals in footer.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>If true When a number is halfway between two others, it is rounded toward the nearest even number.
    ''' For example: 2.5 would round off to 2. 
    ''' If false When a number is halfway between two others, it is rounded toward the nearest number that is away from zero.
    ''' For example: 2.5 would round off to 3.</remarks>
    <Description("Specifies whether values in the cells will be rounded according to bankers rounding specification."), _
    Category("Action")> _
    <DefaultValue(False)> _
    Public Property BankersRounding As Boolean
        Get
            Return _bankersRounding
        End Get
        Set(value As Boolean)
            _bankersRounding = value

            If IsDesigner Then Exit Property
            SumAllColumns()
        End Set
    End Property

    ''' <summary>
    ''' Gets the value of the footer cell as a double.
    ''' </summary>
    ''' <param name="columnName">The name of the column in the parent datagridview to which get the corresponding value from in footer.</param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Will return 0 if value is not a number</remarks>
    <Browsable(False)> _
    Public ReadOnly Property Value(ByVal columnName As String) As Double
        Get
            Dim cVal As String = CStr(localDGV.Rows(0).Cells(columnName & "_footer").Value)
            Dim rVal As Double = 0
            Dim provider As Globalization.CultureInfo

            cVal = If(cVal.IndexOf(_valueSuffix) > 0, cVal.Substring(0, cVal.IndexOf(_valueSuffix) - 1).Trim, cVal.Trim)
            cVal = If(cVal.IndexOf(_valuePrefix) > -1, cVal.Remove(cVal.IndexOf(_valuePrefix), _valuePrefix.Length).Trim, cVal.Trim)

            provider = Globalization.CultureInfo.CreateSpecificCulture(Globalization.CultureInfo.InstalledUICulture.Name)

            Double.TryParse(cVal, Globalization.NumberStyles.Currency, provider, rVal)

            Return rVal
        End Get
    End Property

    ''' <summary>
    ''' Gets the value of the footer cell as a double.
    ''' </summary>
    ''' <param name="columnIndex">The index of the column in the parent datagridview to which get the corresponding value from in footer.</param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property Value(ByVal columnIndex As Integer) As Double
        Get
            Return Value(_parentDGV.Columns(columnIndex).Name)
        End Get
    End Property

    ''' <summary>
    ''' Get or Set the RowHeaderCell as DataGridViewRowHeaderCell.
    ''' </summary>
    ''' <value>RowHeaderCell as DataGridViewRowHeaderCell of the footer.</value>
    ''' <returns>RowHeaderCell as DataGridViewRowHeaderCell</returns>
    ''' <remarks>This is the button-like row header cell found on a DataGridView</remarks>
    <Description("This is the button-like row header cell found on a DataGridView."), _
    Category("Design"), _
    DefaultValue(""), EditorAttribute(GetType(DataGridViewRowHeaderXEditor), GetType(System.Drawing.Design.UITypeEditor))> _
    Public Property RowHeader As DataGridViewRowHeaderX
        Get
            Return _RowHeader
        End Get
        Set(value As DataGridViewRowHeaderX)
            _RowHeader = value
            If (Not IsNothing(Me.localDGV)) Then
                If Me.localDGV.RowCount > 0 Then
                    Me.localDGV.Rows(0).HeaderCell = _RowHeader
                    Me.localDGV.InvalidateCell(_RowHeader)
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' This specifies whether the footer is in Design Time mode. Only used by Designer. This should never be used, set or called.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)>
    Protected Friend Property IsDesigner As Boolean
        Get
            Return _isDesigner
        End Get
        Set(value As Boolean)
            _isDesigner = value
        End Set
    End Property

    ''' <summary>
    ''' This is the DataGridView which the footer is bound to. Only used by Designer. Should otherwise never be used.
    ''' Please use the Constructor: new DataGridViewFooter(DataGridView parent)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Property HostDGV As DataGridView
        Set(value As DataGridView)
            _parentDGV = value
        End Set
        Get
            Return _parentDGV
        End Get
    End Property

    ''' <summary>
    ''' The BackgroundColor for the footer.
    ''' </summary>
    ''' <value>System.Drawing.Color that represents the Color Background should be set to.</value>
    ''' <returns>System.Drawing.Color that represents the Color Background is set to.</returns>
    ''' <remarks></remarks>
    <Description("Specifies the Background Color for the footer."), _
    Category("Appearance")> _
    <DefaultValue("")> _
    Public Overrides Property BackColor As System.Drawing.Color
        Get
            Return _backColor
        End Get
        Set(value As System.Drawing.Color)
            Me._backColor = value
            If Not IsNothing(Me.localDGV) Then
                Me.localDGV.BackgroundColor = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The ForegroundColor for the footer.
    ''' </summary>
    ''' <value>System.Drawing.Color that represents the Color Foreground should be set to.</value>
    ''' <returns>System.Drawing.Color that represents the Color Foreground is set to.</returns>
    ''' <remarks></remarks>
    <Description("Specifies the Foreground Color for the footer."), _
    Category("Appearance")> _
    <DefaultValue("")> _
    Public Overrides Property ForeColor As System.Drawing.Color
        Get
            Return _foreColor
        End Get
        Set(value As System.Drawing.Color)
            Me._foreColor = value
            If Not IsNothing(Me.localDGV) Then
                For Each c As DataGridViewColumn In Me.localDGV.Columns
                    With Me.localDGV.Rows(0)
                        .Cells(c.Name).Style.ForeColor = value
                    End With
                Next
            End If
        End Set
    End Property

    ''' <summary>
    ''' The GridColor for the footer.
    ''' </summary>
    ''' <value>System.Drawing.Color that represents the Color the Grid should be set to.</value>
    ''' <returns>System.Drawing.Color that represents the Color the Grid is set to.</returns>
    ''' <remarks></remarks>
    <Description("Specifies the Color of the Grid for the footer."), _
    Category("Appearance")> _
    <DefaultValue("")> _
    Public Property GridColor As Color
        Get
            Return _gridColor
        End Get
        Set(value As System.Drawing.Color)
            Me._gridColor = value
            If Not IsNothing(Me.localDGV) Then
                Me.localDGV.GridColor = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The BackGround Color for the Cells in the footer.
    ''' </summary>
    ''' <value>System.Drawing.Color that represents the Color the Cells should be set to.</value>
    ''' <returns>System.Drawing.Color that represents the Color the Cells is set to.</returns>
    ''' <remarks></remarks>
    <Description("Specifies the BackGround Color of the Cells in the footer."), _
    Category("Appearance")> _
    <DefaultValue("")> _
    Public Property CellBackColor As Color
        Get
            Return _cellBackColor
        End Get
        Set(value As System.Drawing.Color)
            Me._cellBackColor = value
            If Not IsNothing(Me.localDGV) Then
                For Each c As DataGridViewColumn In Me.localDGV.Columns
                    With Me.localDGV.Rows(0)
                        .Cells(c.Name).Style.BackColor = value
                    End With
                Next
            End If
        End Set
    End Property

    ''' <summary>
    ''' The BackGround Color for the a Cell at the given index in the footer.
    ''' </summary>
    ''' <param name="index">The index number of the Cell.</param>
    ''' <value>System.Drawing.Color that represents the Color a Cell should be set to at the given index.</value>
    ''' <returns>System.Drawing.Color that represents the Color a Cell is set to at the given index.</returns>
    ''' <remarks></remarks>
    <Description("Specifies the BackGround Color of the Cells in the footer."), _
    Category("Appearance")> _
    <DefaultValue(255)> _
    Public Property CellBackColor(ByVal index As Integer) As Color
        Get
            If Me.localDGV.RowCount < 1 Then Exit Property
            If Me.localDGV.ColumnCount <= index Then Exit Property

            Return Me.localDGV.Rows(0).Cells(index).Style.BackColor
        End Get
        Set(value As System.Drawing.Color)
            If Me.localDGV.RowCount < 1 Then Exit Property
            If Me.localDGV.ColumnCount <= index Then Exit Property

            Me.localDGV.Rows(0).Cells(index).Style.BackColor = value
        End Set
    End Property

    ''' <summary>
    ''' The BackGround Color for the a Cell in the footer.
    ''' </summary>
    ''' <param name="name">The name of the Column.</param>
    ''' <value>System.Drawing.Color that represents the Color a Cell in the specified Column should be set to.</value>
    ''' <returns>System.Drawing.Color that represents the Color a Cell in the specified Column is set to.</returns>
    ''' <remarks></remarks>
    <Description("Specifies the BackGround Color of a Cell in the footer."), _
    Category("Appearance")> _
    <DefaultValue(255)> _
    Public Property CellBackColor(ByVal name As String) As Color
        Get
            If Me.localDGV.RowCount < 1 Then Exit Property
            If Not Me.localDGV.Columns.Contains(name & "_footer") Then Exit Property

            Return Me.localDGV.Rows(0).Cells(name).Style.BackColor
        End Get
        Set(value As System.Drawing.Color)
            If Me.localDGV.RowCount < 1 Then Exit Property
            If Not Me.localDGV.Columns.Contains(name & "_footer") Then Exit Property

            Me.localDGV.Rows(0).Cells(name & "_footer").Style.BackColor = value
        End Set
    End Property

    ''' <summary>
    ''' The BackGround Color for the a Cell at the given index in the footer.
    ''' </summary>
    ''' <param name="index">The index number of the Cell.</param>
    ''' <value>System.Drawing.Color that represents the Color a Cell ForeColor should be set to at the given index.</value>
    ''' <returns>System.Drawing.Color that represents the Color a Cell ForeColor is set to at the given index.</returns>
    ''' <remarks></remarks>
    <Description("Specifies the BackGround Color of the Cells in the footer."), _
    Category("Appearance")> _
    <DefaultValue(0)> _
    Public Property CellForeColor(ByVal index As Integer) As Color
        Get
            If Me.localDGV.RowCount < 1 Then Exit Property
            If Me.localDGV.ColumnCount <= index Then Exit Property

            Return Me.localDGV.Rows(0).Cells(index).Style.ForeColor
        End Get
        Set(value As System.Drawing.Color)
            If Me.localDGV.RowCount < 1 Then Exit Property
            If Me.localDGV.ColumnCount <= index Then Exit Property

            Me.localDGV.Rows(0).Cells(index).Style.ForeColor = value
        End Set
    End Property

    ''' <summary>
    ''' The BackGround Color for the a Cell in the footer.
    ''' </summary>
    ''' <param name="name">The name of the Column.</param>
    ''' <value>System.Drawing.Color that represents the Color a Cell ForeColor in the specified Column should be set to.</value>
    ''' <returns>System.Drawing.Color that represents the Color a Cell ForeColor in the specified Column is set to.</returns>
    ''' <remarks></remarks>
    <Description("Specifies the BackGround Color of a Cell in the footer."), _
    Category("Appearance")> _
    <DefaultValue(0)> _
    Public Property CellForeColor(ByVal name As String) As Color
        Get
            If Me.localDGV.RowCount < 1 Then Exit Property
            If Not Me.localDGV.Columns.Contains(name & "_footer") Then Exit Property

            Return Me.localDGV.Rows(0).Cells(name).Style.ForeColor
        End Get
        Set(value As System.Drawing.Color)
            If Me.localDGV.RowCount < 1 Then Exit Property
            If Not Me.localDGV.Columns.Contains(name & "_footer") Then Exit Property

            Me.localDGV.Rows(0).Cells(name & "_footer").Style.ForeColor = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RowHeaderVisible As Boolean
        Get
            Return Me.localDGV.RowHeadersVisible
        End Get
        Set(value As Boolean)
            Me.localDGV.RowHeadersVisible = value
        End Set
    End Property

#End Region

#Region "Event Handler Overrides"

    'Protected Overrides Sub OnParentChanged(e As System.EventArgs)
    '    MyBase.OnParentChanged(e)
    '    If _isDesigner Then Exit Sub
    '    If Not IsNothing(Me.Parent) Then
    '        If Me.Parent.GetType Is GetType(DataGridView) And (IsNothing(_parentDGV) Or _parentDGV.Name <> Me.Parent.Name) Then
    '            _parentDGV = CType(Me.Parent, DataGridView)
    '            SetColumns(CType(Me.Parent, DataGridView))
    '        Else
    '            Me.SetParent(CType(GetControlByName(_parentDGV.Name), DataGridView))
    '            SetColumns(CType(Me.Parent, DataGridView))
    '        End If
    '    End If
    'End Sub

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        If (Not IsNothing(Me.Parent)) And (Me.Parent.GetType = GetType(DataGridView)) Then
            If ((_parentDGV.RowCount > 1) And (Me.localDGV.RowCount < 1)) Or (Not Me.Controls.ContainsKey(Me.localDGV.Name)) Then '
                SetUp(CType(Me.Parent, DataGridView))
            End If
        End If
    End Sub

    Protected Overrides Sub OnDockChanged(e As System.EventArgs)
        MyBase.OnDockChanged(e)
        MyBase.Dock = DockStyle.Bottom
    End Sub

#End Region

#Region "Handle events from parent"


    ''' <summary>
    ''' Tallies the cell entries made in parent DGV to the footer cells in the corresponding column
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Only columns of texboxcolumn type will be totalled.</remarks>
    Private Sub ParentValChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles _parentDGV.CellEndEdit
        If IsDesigner Then Exit Sub
        If Not _autoCalc Then Exit Sub
        Dim curColumnName As String = _parentDGV.Columns(e.ColumnIndex).Name
        Dim columnAddable As Boolean = Me._columnsToSum.Contains(curColumnName)

        If _parentDGV.Rows(e.RowIndex).Cells(e.ColumnIndex).GetType.Name = "DataGridViewTextBoxCell" And columnAddable Then

            SumColumn(curColumnName)
        End If
    End Sub

    ''' <summary>
    ''' Tallies the cell entries made in parent DGV to the footer cells in the corresponding column
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Only columns of texboxcolumn type will be totalled.</remarks>
    Private Sub ParentValChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles _parentDGV.RowsRemoved
        If IsDesigner Then Exit Sub
        If Not _autoCalc Then Exit Sub
        For Each c As DataGridViewColumn In CType(sender, DataGridView).Columns.OfType(Of DataGridViewTextBoxColumn)()
            Dim columnAddable As Boolean = Me._columnsToSum.Contains(c.Name)
            If Not columnAddable Then Continue For

            SumColumn(c.Name)
        Next

        If _parentDGV.RowCount > 0 Then DoRowSpacer(_parentDGV.Rows(_parentDGV.RowCount - 1))

        CheckParentVScrollBar()
    End Sub

    ' ''' <summary>
    ' ''' Performs needed maintenance and bug fixes.
    ' ''' </summary>
    ' ''' <param name="e"></param>
    ' ''' <remarks>When the control gets populated with rows to the point where a scrollbar appears, the newly added rows tend to hide behind footer.
    ' ''' As a fix, we add another row and hide it. When another row is added, a hidden row is deleted.
    ' ''' To prevent a recursive call when we add a hidden row, we set a sentinel to determine whether we will proceed with hidden row processes.</remarks>
    'Private Sub OnParentRowsAdded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles _parentDGV.RowsAdded

    '    If _parentDGV.Rows.Count < 1 Then Exit Sub

    '    Dim rowY As Integer = (_parentDGV.Rows.Count + 1) * _parentDGV.Rows(0).Height
    '    Dim footY As Integer = _parentDGV.Controls(Me.Name).Top

    '    If _parentDGV.Rows.Count = 1 Then
    '        Me.SetColumns(_parentDGV)
    '        'Me.Rows.Add()
    '    End If

    '    If rowY >= footY And Not Me._killParentRowAddedEvent Then

    '        Me._killParentRowAddedEvent = True

    '        For Each dgvr As DataGridViewRow In _parentDGV.Rows
    '            If dgvr.Tag Is Nothing Then Continue For
    '            If dgvr.Tag.ToString = "spacer" Then _parentDGV.Rows.Remove(dgvr)
    '        Next

    '        _parentDGV.Rows.Add(SpacerRow)

    '        _parentDGV.FirstDisplayedScrollingRowIndex = _parentDGV.Rows.Count - 2

    '        Me._killParentRowAddedEvent = False
    '    End If
    '    CheckParentVScrollBar()
    'End Sub

    ''' <summary>
    ''' Performs needed actions when parent adds a row.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks>When the control gets populated with rows to the point where a scrollbar appears, the newly added rows tend to hide behind footer.
    ''' As a fix, we will create a spacer to adjust for our footer.
    ''' Also arrange the width of the footer if there is a scrollbar</remarks>
    Private Sub OnParentRowsAdded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles _parentDGV.RowsAdded
        If IsDesigner Then Exit Sub

        If _parentDGV.Rows.Count < 1 Then Exit Sub

        DoRowSpacer(_parentDGV.Rows(e.RowIndex))

        CheckParentVScrollBar()
    End Sub

    ''' <summary>
    ''' Perform needed actions when a row in parent has its height changed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>When the control gets populated with rows to the point where a scrollbar appears, the newly added rows tend to hide behind footer.
    ''' As a fix, we will create a spacer to adjust for our footer.
    ''' Also arrange the width of the footer if there is a scrollbar</remarks>
    Private Sub OnParentRowheightChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _parentDGV.RowHeightChanged
        If IsDesigner Then Exit Sub
        DoRowSpacer(_parentDGV.Rows(_parentDGV.RowCount - 1))
        CheckParentVScrollBar()
    End Sub

    ''' <summary>
    ''' Perform needed actions after parents rows are painted
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>When the control gets populated with rows to the point where a scrollbar appears, the newly added rows tend to hide behind footer.
    ''' As a fix, we will create a spacer to adjust for our footer.</remarks>
    Private Sub OnParentRowPostPaint(ByVal sender As Object, ByVal e As DataGridViewRowPostPaintEventArgs) Handles _parentDGV.RowPostPaint
        'If IsDesigner Then Exit Sub
        If _parentDGV.RowCount < 1 Then Exit Sub
        If e.RowIndex = _parentDGV.Rows.GetLastRow(DataGridViewElementStates.Displayed) Then DoRowSpacer(_parentDGV.Rows(_parentDGV.RowCount - 1))
    End Sub

    ''' <summary>
    ''' Perform neede actions when parent is painted
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>There is a bug that our footers border gets distorted. We will need to repaint the footer to fix this issue.</remarks>
    Private Sub OnParentPaint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles _parentDGV.Paint
        'If IsDesigner Then Exit Sub
        If Not localDGV.ColumnCount = Me._parentDGV.ColumnCount Then SetColumns(_parentDGV)
        If localDGV.RowCount > 0 Then localDGV.Invalidate(True)
    End Sub

    ''' <summary>
    ''' Resizes footer columns to match _parentDGV columns.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReSizeCol() Handles _parentDGV.ColumnWidthChanged
        'If IsDesigner Then Exit Sub
        If localDGV.Rows.Count < 1 Then Exit Sub
        If localDGV.Columns.Count < 1 Then Exit Sub
        If _parentDGV.Rows.Count < 1 Then Exit Sub
        If _parentDGV.Columns.Count < 1 Then Exit Sub
        For Each c As DataGridViewColumn In _parentDGV.Columns
            localDGV.Columns(c.Index).Width = c.Width
        Next
    End Sub

    ''' <summary>
    ''' Adds columns when columns are added to parent DGV.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>This will actually call the same Sub which is called during instantiation.</remarks>
    Private Sub ResetColumns(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles _parentDGV.ColumnAdded
        'If IsDesigner Then Exit Sub
        SetColumns(_parentDGV)

        If ColumnsOverflow() Then
            _parentDGV.Size = New Size(_parentDGV.Size.Width + 1, _parentDGV.Size.Height + 1)
            _parentDGV.Size = New Size(_parentDGV.Size.Width - 1, _parentDGV.Size.Height - 1)

            If _parentDGV.RowCount > 0 Then DoRowSpacer(_parentDGV.Rows(_parentDGV.RowCount - 1))
        End If
    End Sub

    ''' <summary>
    ''' Removes our corresponding (footer) column, when parent DGV has a column removed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RemoveColumns(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles _parentDGV.ColumnRemoved
        'If IsDesigner Then Exit Sub
        _killRemoveColumns = False
        ColumnToSum(e.Column.Name) = False
        localDGV.Columns.Remove(e.Column.Name & "_footer")
        If e.Column.DisplayIndex = 0 And localDGV.Columns.Count > 0 And UseHeaderCell Then SetHeader()

        If CheckParentHScrollBarHeight() > 0 And _parentDGV.RowCount > 0 Then DoRowSpacer(_parentDGV.Rows(_parentDGV.RowCount - 1))

        _killRemoveColumns = True
    End Sub

    ''' <summary>
    ''' Synchronizes the scrolling of parent DGV to the footer row.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ScrollMe(ByVal sender As Object, ByVal e As EventArgs) Handles _parentDGV.Scroll
        'If IsDesigner Then Exit Sub
        localDGV.HorizontalScrollingOffset = _parentDGV.HorizontalScrollingOffset
    End Sub

    ''' <summary>
    ''' Keeps the footers columns order ensync with corresponding columns in parent.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ShiftColumns(ByVal sender As Object, ByVal e As DataGridViewColumnEventArgs) Handles _parentDGV.ColumnDisplayIndexChanged
        'If IsDesigner Then Exit Sub
        localDGV.Columns(e.Column.Name & "_footer").DisplayIndex = e.Column.DisplayIndex
    End Sub

    ''' <summary>
    ''' Keeps the footers columns name ensync with corresponding columns in parent.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChangeName(ByVal sender As Object, ByVal e As DataGridViewColumnEventArgs) Handles _parentDGV.ColumnNameChanged
        'If IsDesigner Then Exit Sub
        localDGV.Columns(e.Column.DisplayIndex).Name = e.Column.Name & "_footer"
    End Sub

    ''' <summary>
    ''' Keeps the footers built in standard row header width ensync with parent's built in standard row header.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ResizeRowHeader(ByVal sender As Object, ByVal e As EventArgs) Handles _parentDGV.RowHeadersWidthChanged
        'If IsDesigner Then Exit Sub
        localDGV.RowHeadersWidth = _parentDGV.RowHeadersWidth
        Me.Invalidate()
        localDGV.Invalidate()
    End Sub

#End Region

#Region "Methods and Sub-Procedures"


    ''' <summary>
    ''' Processing that needs to be done when a new row is added
    ''' </summary>
    ''' <remarks>When a new row is added (which is the only row to ever be added) the following will happen: 
    ''' First cell text and text color set if headercell option is used.
    ''' First cell de-selected, so no cells are selected.
    ''' Footer is set to be uneditable.</remarks>
    Private Sub AddRow()
        If Me.localDGV.RowCount > 1 Then
            Me.localDGV.Rows.RemoveAt(localDGV.Rows.Count - 1)
            Exit Sub
        End If

        Dim row As New DataGridViewRow()
        row.HeaderCell = _RowHeader
        row.ReadOnly = True
        row.Selected = False

        Me.localDGV.Rows.Add(row)

        SetHeader()
        With Me.localDGV
            .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            .ClearSelection()
            .CurrentCell = localDGV.Rows(0).Cells(0)
            .Rows(0).Cells(0).Selected = False
            .Enabled = True
            .ReadOnly = False
        End With
    End Sub

    ''' <summary>
    ''' Attempts to add the values in all the cells in a column in parent, and then displays the total in footer column corresponding to parent column.
    ''' </summary>
    ''' <param name="columnName">Name of column in parent which to try and sum all cell values of.</param>
    ''' <remarks>If a cell value cannot be parsed to double, no error will be thrown. That cell will be skipped.</remarks>
    Public Sub SumColumn(ByVal columnName As String)
        Dim tally As Double = 0.0
        Dim nfi As Globalization.NumberFormatInfo = New Globalization.CultureInfo(Globalization.CultureInfo.InstalledUICulture.Name, False).NumberFormat
        Dim cValFormat As String = If(Me._isCurrency, "C", "N")
        Dim cVal As String

        For Each r As DataGridViewRow In Me._parentDGV.Rows
            cVal = CStr(r.Cells(columnName).Value)
            tally += If(Double.TryParse(cVal, Nothing), CDbl(cVal), 0)
        Next

        nfi.NumberDecimalDigits = Me._decimalPlaces
        tally = If(Me._roundSum, Math.Round(tally, Me._decimalPlaces, If(Me._bankersRounding, MidpointRounding.ToEven, MidpointRounding.AwayFromZero)), TruncateToDecimalPlace(tally, Me._decimalPlaces))

        Me.localDGV.Rows(0).Cells(columnName & "_footer").Value = Me._valuePrefix & " " & tally.ToString(cValFormat, nfi) & " " & Me._valueSuffix
    End Sub

    ''' <summary>
    ''' Attempts to add the values in all the cells in all columns in parent, and then displays the total in footer column corresponding to parent column.
    ''' </summary>
    ''' <remarks>If a cell value cannot be parsed to double, no error will be thrown. That cell will be skipped.</remarks>
    Public Sub SumAllColumns()
        For Each c As String In _columnsToSum
            SumColumn(c)
        Next
    End Sub

    ''' <summary>
    ''' Checks whether _parentDGV has scrollbar visible or not, and sets the width of the footer row accordingly.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckParentVScrollBar()
        Dim DGVVerticalScroll As VScrollBar = _parentDGV.Controls.OfType(Of VScrollBar).SingleOrDefault

        If DGVVerticalScroll.Visible Then
            Me.Width = _parentDGV.Width + DGVVerticalScroll.Width
        Else
            Me.Width = _parentDGV.Width
        End If

    End Sub

    ''' <summary>
    ''' Checks whether _parentDGV has hscrollbar visible or not, returns the height.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CheckParentHScrollBarHeight() As Integer
        Dim DGVVerticalScroll As HScrollBar = _parentDGV.Controls.OfType(Of HScrollBar).SingleOrDefault

        If DGVVerticalScroll.Visible Then
            Return DGVVerticalScroll.Height '_parentDGV.DisplayRectangle.Height - _parentDGV.ClientRectangle.Height
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' Sets the display properties for footer header cell.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHeader()
        If Not Me._useHeader Then Exit Sub
        If localDGV.RowCount < 1 Then Exit Sub

        Dim s As New DataGridViewCellStyle
        s.ForeColor = _headerCellForeColor
        s.BackColor = _headerCellBackColor
        s.SelectionBackColor = _headerCellBackColor
        s.SelectionForeColor = _headerCellForeColor
        s.Font = New Font(localDGV.DefaultCellStyle.Font.FontFamily, localDGV.DefaultCellStyle.Font.Size, FontStyle.Bold)

        localDGV.Rows(0).Cells(0).Style = s

        localDGV.Rows(0).Cells(0).Value = _headerCellText
        localDGV.Rows(0).Cells(0).Style.ForeColor = _headerCellForeColor
        localDGV.Rows(0).Cells(0).Style.BackColor = _headerCellBackColor
    End Sub

    ''' <summary>
    ''' Sets the display properties for footer header cell to match the rest of the footer formatting.
    ''' </summary>
    ''' <remarks>This is used when footer header is being disabled</remarks>
    Private Sub UnSetHeader()
        Console.WriteLine(Me._useHeader)
        If Me._useHeader Then Exit Sub
        If localDGV.RowCount < 1 Then Exit Sub

        Dim s As New DataGridViewCellStyle(localDGV.DefaultCellStyle)

        localDGV.Rows(0).Cells(0).Style = s

        localDGV.Rows(0).Cells(0).Style.ForeColor = s.ForeColor
        localDGV.Rows(0).Cells(0).Style.BackColor = s.BackColor
    End Sub

    ''' <summary>
    ''' This will set or unset our spacer in parent DataGridView. 
    ''' </summary>
    ''' <param name="lastRow">The last row in our parent</param>
    ''' <remarks>While this Sub-Procedure is executing it will kill the RowHeightChanged event.</remarks>
    Private Sub DoRowSpacer(ByRef lastRow As DataGridViewRow)
        Dim needSpacer As Boolean = (_parentDGV.Rows.GetRowsHeight(DataGridViewElementStates.Displayed) + _parentDGV.ColumnHeadersHeight + CheckParentHScrollBarHeight()) >= (_parentDGV.ClientSize.Height - Me.Height)

        RemoveHandler _parentDGV.RowHeightChanged, AddressOf OnParentRowheightChanged

        If needSpacer Then
            SetRowSpacer(lastRow)
        Else
            unsetRowSpacer(lastRow)
        End If

        AddHandler _parentDGV.RowHeightChanged, AddressOf OnParentRowheightChanged
    End Sub

    ''' <summary>
    ''' Sets a spacer in parent DataGridView to keep rows from hiding behind our footer. Do not call this Sub-Procedure directly. Use the DoRowSpacer.
    ''' </summary>
    ''' <param name="row"></param>
    ''' <remarks></remarks>
    Private Sub SetRowSpacer(ByRef row As DataGridViewRow)
        Dim secondToLastRow As DataGridViewRow = If((row.Index - 1) < 0, row, row.DataGridView.Rows(row.Index - 1))
        Dim rowHeight As Integer = row.DataGridView.RowTemplate.Height
        Dim rowY As Integer = row.DataGridView.GetCellDisplayRectangle(0, row.Index, False).Location.Y
        Dim g As Graphics = row.DataGridView.CreateGraphics
        Dim hScrollBarHeight As Integer = CheckParentHScrollBarHeight()
        Dim spacerHiderRect As New Rectangle(0, (rowY + rowHeight), (row.DataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Displayed) + row.DataGridView.RowHeadersWidth), Me.Height - hScrollBarHeight)


        secondToLastRow.DividerHeight = row.DataGridView.RowTemplate.DividerHeight
        secondToLastRow.Height = row.DataGridView.RowTemplate.Height
        row.Height = (rowHeight + Me.Height)
        row.DividerHeight = (row.DataGridView.RowTemplate.DividerHeight + Me.Height)

        g.DrawRectangle(New Pen(row.DataGridView.BackgroundColor), spacerHiderRect)
        g.FillRectangle(New SolidBrush(row.DataGridView.BackgroundColor), spacerHiderRect)

        row.DataGridView.InvalidateRow(row.DataGridView.FirstDisplayedScrollingRowIndex)
        If row.DataGridView.RowCount > 1 Then row.DataGridView.InvalidateRow(row.DataGridView.FirstDisplayedScrollingRowIndex + 1)
    End Sub

    ''' <summary>
    ''' Un-Sets the spacer in parent DataGridView to keep rows from hiding behind our footer. Do not call this Sub-Procedure directly. Use the DoRowSpacer.
    ''' </summary>
    ''' <param name="row"></param>
    ''' <remarks></remarks>
    Private Sub unsetRowSpacer(ByRef row As DataGridViewRow)
        Dim secondToLastRow As DataGridViewRow = If((row.Index - 1) < 0, row, row.DataGridView.Rows(row.Index - 1))

        secondToLastRow.DividerHeight = row.DataGridView.RowTemplate.DividerHeight
        secondToLastRow.Height = row.DataGridView.RowTemplate.Height
        row.Height = row.DataGridView.RowTemplate.Height
        row.DividerHeight = row.DataGridView.RowTemplate.DividerHeight
    End Sub

    ''' <summary>
    ''' Returns a datagridviewrow for use as spacer row.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SpacerRow() As DataGridViewRow
        Dim sRow As New DataGridViewRow

        sRow.DefaultCellStyle.BackColor = _parentDGV.BackgroundColor
        sRow.Tag = "spacer"
        sRow.DefaultCellStyle.SelectionBackColor = _parentDGV.BackgroundColor
        sRow.ReadOnly = True
        sRow.Height = Me.Height + 2

        Return sRow
    End Function

    ''' <summary>
    ''' Checks whether the total columns width will display a horizontal scrollbar.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ColumnsOverflow() As Boolean
        Dim colSpace As Integer = 0

        For Each col As DataGridViewColumn In _parentDGV.Columns
            colSpace += col.Width
        Next

        If _parentDGV.RowHeadersVisible Then colSpace += _parentDGV.RowHeadersWidth

        Return colSpace > _parentDGV.ClientSize.Width

    End Function

    'static double[] pow10 = { 1e0, 1e1, 1e2, 1e3, 1e4, 1e5, 1e6, 1e7, 1e8, 1e9, 1e10 };
    Private ReadOnly pow10 As Double() = {1.0, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, 10000000.0, 100000000.0, 1000000000.0, 10000000000.0}
    ''' <summary>
    ''' Truncates a decimal number to specified decimal places without rounding.
    ''' </summary>
    ''' <param name="NumToTruncate"></param>
    ''' <param name="DecimalPlaces"></param>
    ''' <returns>A double truncated to the specified decimal places.</returns>
    ''' <remarks>Function provided by Glenn Slayden (http://stackoverflow.com/users/147511/glenn-slayden) on http://stackoverflow.com/questions/329957/truncate-decimal-number-not-round-off</remarks>
    Private Function TruncateToDecimalPlace(ByVal NumToTruncate As Double, ByVal DecimalPlaces As Integer) As Double
        If DecimalPlaces < 0 Then Throw New ArgumentException()
        If DecimalPlaces = 0 Then Return Math.Truncate(NumToTruncate)

        Dim m As Double = If(DecimalPlaces >= pow10.Length, Math.Pow(10, DecimalPlaces), pow10(DecimalPlaces))
        Return Math.Truncate(NumToTruncate * m) / m
    End Function

    ''' <summary>
    ''' Searches the form for a control, and returns it.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks>Thanks to jmcilhinney (http://jmcilhinney.blogspot.com/) for sharing the code (http://www.vbforums.com/showthread.php?387308-Visit-Every-Control-on-a-Form-(includes-nested-controls-no-recursion)</remarks>
    Private Function GetControlByName(ByVal name As String) As Control
        Dim form As Form = Me.FindForm
        Dim ctl As Control = form.GetNextControl(form, True) 'Get the first control in the tab order.

        Do Until ctl Is Nothing
            'Use ctl here.
            If ctl.Name = name Then Return ctl

            ctl = Me.GetNextControl(ctl, True) 'Get the next control in the tab order.
        Loop

        Return Nothing
    End Function

#End Region

#Region "Hidden Overridden Properties"

    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property Dock As System.Windows.Forms.DockStyle
        Get
            Return MyBase.Dock
        End Get
        Set(value As System.Windows.Forms.DockStyle)
            MyBase.Dock = DockStyle.Bottom
        End Set
    End Property

    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property AllowDrop As Boolean
        Get
            Return False
        End Get
        Set(value As Boolean)
            MyBase.AllowDrop = False
        End Set
    End Property

    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property Anchor As System.Windows.Forms.AnchorStyles
        Get
            Return CType(15, AnchorStyles)
        End Get
        Set(value As System.Windows.Forms.AnchorStyles)
            MyBase.Anchor = CType(15, AnchorStyles)
        End Set
    End Property

#End Region

End Class


'End Namespace

