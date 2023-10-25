Imports HL.Managed.Forms.Controls.DataGridViewFooter

''' Author: Heriberto Lugo
''' Website: heribertolugo.com
''' Description: Form to test the DataGridViewFooter
Public Class FooterTest_Win
    Private colCount As Integer = 1
    Private rowCount As Integer = 0

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        If Me.DataGridView1.ColumnCount <> (colCount - 1) Then FillControlsWithColumns()

        colCount += Me.DataGridView1.Columns.Count
        Me.HeaderBackColorDemo_Lbl.BackColor = DataGridViewFooter1.HeaderCellBackColor
        Me.HeaderForeColorDemo_Lbl.BackColor = DataGridViewFooter1.HeaderCellForeColor
        Me.UseHeader_ChkBox.Checked = DataGridViewFooter1.UseHeaderCell
        Me.HeaderText_TxtBox.Text = DataGridViewFooter1.HeaderCellText
        Me.DecimalPlaces_NumUpDown.Value = DataGridViewFooter1.DecimalPlaces
        Me.ValueSuffix_TxtBox.Text = DataGridViewFooter1.ValueSuffix
        Me.AutoCalc_ChkBox.Checked = DataGridViewFooter1.AutoCalc
        Me.RoundSum_ChkBox.Checked = DataGridViewFooter1.RoundSum
        Me.IsCurrency_ChkBox.Checked = DataGridViewFooter1.IsCurrency
        Me.FooterBackColorDemo_Lbl.BackColor = DataGridViewFooter1.BackColor
        Me.FooterForeColorDemo_Lbl.BackColor = DataGridViewFooter1.ForeColor
        Me.GridColorDemo_Lbl.BackColor = DataGridViewFooter1.GridColor
        Me.CellsBackColorDemo_Lbl.BackColor = DataGridViewFooter1.CellBackColor
        Me.CellBackColorDemo_Lbl.BackColor = DataGridViewFooter1.CellBackColor
        Me.CellForeColorDemo_Lbl.BackColor = DataGridViewFooter1.ForeColor
        Me.BankersRounding_ChkBox.Checked = DataGridViewFooter1.BankersRounding
        Me.RowHeaderBackColorDemo_Lbl.BackColor = Me.DataGridViewFooter1.RowHeader.BackColor
        Me.RowHeaderForeColorDemo_Lbl.BackColor = Me.DataGridViewFooter1.RowHeader.ForeColor
        Me.RowHeaderMouseOverDemo_Lbl.BackColor = Me.DataGridViewFooter1.RowHeader.MouseOverColor
        Me.RowHeaderImageDemo_Lbl.Image = Me.DataGridViewFooter1.RowHeader.Image
        Me.RowHeaderImageDemo_Lbl.ImageAlign = Me.DataGridViewFooter1.RowHeader.ImagePosition
        Me.RowHeaderTextDemo_Lbl.Text = Me.DataGridViewFooter1.RowHeader.Text
        Me.RowHeaderTextDemo_Lbl.TextAlign = Me.DataGridViewFooter1.RowHeader.TextPosition

        SetImagePosButHighLight(Me.RowHeaderImageDemo_Lbl.ImageAlign, Me.ImagePosition_GrpBox)
        SetImagePosButHighLight(Me.RowHeaderTextDemo_Lbl.TextAlign, Me.TextPosition_GrpBox)

        Me.AllowUsrAddRows_ChkBox.Checked = Me.DataGridView1.AllowUserToAddRows
        Me.DataGridViewRowHeaderVisible_ChkBox.Checked = Me.DataGridView1.RowHeadersVisible
        AddHandler Me.AllowUsrAddRows_ChkBox.CheckedChanged, AddressOf AllowUsrAddRows_ChkBox_CheckedChanged
    End Sub

    Private Sub InsertColumn(sender As System.Object, e As System.EventArgs) Handles InsertColumn_But.Click
        Dim col As New DataGridViewTextBoxColumn
        Dim colName As String = "Column" & CStr(colCount)

        If Me.InsertColumn_TxtBox.Text.Trim() <> Nothing And Me.InsertColumn_TxtBox.Text.Trim() <> "" Then
            If Not Me.DataGridView1.Columns.Contains(Me.InsertColumn_TxtBox.Text) Then
                colName = Me.InsertColumn_TxtBox.Text
            End If
        End If

        colCount += 1
        Me.InsertColumn_TxtBox.Text = Nothing

        col.Name = colName
        col.HeaderText = col.Name

        Me.DataGridView1.Columns.Add(col)
        'Me.ColumnsToSum_ChkLBox.Items.Add(col.Name, True)
        'Me.Value_CBox.Items.Add(col.Name)
        FillControlsWithColumns()
    End Sub

    Private Sub RemoveColumn(sender As System.Object, e As System.EventArgs) Handles RemoveColumn_But.Click
        Dim colIndex As Integer = If(Me.DataGridView1.SelectedCells.Count > 0, Me.DataGridView1.SelectedCells(0).ColumnIndex, -1)

        If colIndex > -1 Then
            Me.DataGridView1.Columns.RemoveAt(colIndex)
            Me.ColumnsToSum_ChkLBox.Items.RemoveAt(colIndex)
            Me.Value_CBox.Items.RemoveAt(colIndex)
        End If
    End Sub

    Private Sub InsertRow(sender As System.Object, e As System.EventArgs) Handles InsertRow_But.Click
        If Me.DataGridView1.Columns.Count < 1 Then Exit Sub
        Dim lastRowIndex As Integer = (Me.DataGridView1.RowCount - 1)
        Dim rowIndex As Integer = If(Me.DataGridView1.AllowUserToAddRows, lastRowIndex, (lastRowIndex + 1))

        Me.DataGridView1.Rows.Add()
        Me.DataGridView1.Rows(rowIndex).HeaderCell.Value = CStr(rowCount)

        rowCount += 1
    End Sub

    Private Sub RemoveRow(sender As System.Object, e As System.EventArgs) Handles RemoveRow_But.Click
        Dim rowIndex As Integer = If(Me.DataGridView1.SelectedCells.Count > 0, Me.DataGridView1.SelectedCells(0).RowIndex, -1)

        If rowIndex > -1 Then
            If Me.DataGridView1.Rows(rowIndex).IsNewRow Then Exit Sub
            Me.DataGridView1.Rows.RemoveAt(rowIndex)
        End If
    End Sub

    Private Sub SelectColumnsToSum(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles ColumnsToSum_ChkLBox.ItemCheck
        DataGridViewFooter1.ColumnToSum(CStr(Me.ColumnsToSum_ChkLBox.Items(e.Index))) = e.NewValue
    End Sub

    Private Sub UseHeader_ChkBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles UseHeader_ChkBox.CheckedChanged
        DataGridViewFooter1.UseHeaderCell = UseHeader_ChkBox.Checked
    End Sub

    Private Sub HeaderText_TxtBox_TextChanged(sender As System.Object, e As System.EventArgs) Handles HeaderText_TxtBox.TextChanged
        DataGridViewFooter1.HeaderCellText = Me.HeaderText_TxtBox.Text
    End Sub

    Private Sub HeaderBackColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles HeaderBackColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.HeaderBackColorDemo_Lbl.BackColor)

        DataGridViewFooter1.HeaderCellBackColor = c

        Me.HeaderBackColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub HeaderForeColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles HeaderForeColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.HeaderForeColorDemo_Lbl.BackColor)

        DataGridViewFooter1.HeaderCellForeColor = c

        Me.HeaderForeColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub SetDecimalPlaces(sender As System.Object, e As System.EventArgs) Handles DecimalPlaces_NumUpDown.ValueChanged

        If Not IsNothing(DataGridViewFooter1) Then
            DataGridViewFooter1.DecimalPlaces = Me.DecimalPlaces_NumUpDown.Value
        End If
    End Sub

    Private Sub ValueSuffix(sender As System.Object, e As System.EventArgs) Handles ValueSuffix_TxtBox.TextChanged
        If Not IsNothing(DataGridViewFooter1) Then
            DataGridViewFooter1.ValueSuffix = Me.ValueSuffix_TxtBox.Text
        End If
    End Sub

    Private Sub ValuePrefix(sender As System.Object, e As System.EventArgs) Handles ValuePrefix_TxtBox.TextChanged
        If Not IsNothing(DataGridViewFooter1) Then
            DataGridViewFooter1.ValuePrefix = Me.ValuePrefix_TxtBox.Text
        End If
    End Sub

    Private Sub AutoCalc_ChkBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles AutoCalc_ChkBox.CheckedChanged
        If Not IsNothing(DataGridViewFooter1) Then
            DataGridViewFooter1.AutoCalc = Me.AutoCalc_ChkBox.Checked
        End If
    End Sub

    Private Sub RoundSum_ChkBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RoundSum_ChkBox.CheckedChanged
        If Not IsNothing(DataGridViewFooter1) Then
            DataGridViewFooter1.RoundSum = Me.RoundSum_ChkBox.Checked
        End If
    End Sub

    Private Sub BankersRounding_ChkBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles BankersRounding_ChkBox.CheckedChanged
        If Not IsNothing(DataGridViewFooter1) Then
            DataGridViewFooter1.BankersRounding = Me.BankersRounding_ChkBox.Checked
        End If
    End Sub

    Private Sub Value_CBox_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles Value_CBox.SelectedIndexChanged
        MsgBox(DataGridViewFooter1.Value(Me.Value_CBox.SelectedItem.ToString), MsgBoxStyle.OkOnly, "     The value at " & Me.Value_CBox.SelectedItem.ToString & " is:     ")
    End Sub

    Private Sub AllowUsrAddRows_ChkBox_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Me.DataGridView1.AllowUserToAddRows = CType(sender, CheckBox).Checked
        Me.DataGridView1.AllowUserToDeleteRows = CType(sender, CheckBox).Checked
    End Sub

    Private Sub FillControlsWithColumns()

        For Each col As DataGridViewColumn In Me.DataGridView1.Columns
            If Not Me.ColumnsToSum_ChkLBox.Items.Contains(col.Name) Then Me.ColumnsToSum_ChkLBox.Items.Add(col.Name, True)
            If Not Me.Value_CBox.Items.Contains(col.Name) Then Me.Value_CBox.Items.Add(col.Name)
            If Not Me.CellBackColor_CBox.Items.Contains(col.Name) Then Me.CellBackColor_CBox.Items.Add(col.Name)
            If Not Me.CellForeColor_CBox.Items.Contains(col.Name) Then Me.CellForeColor_CBox.Items.Add(col.Name)
            If Not Me.ColumnToSum_CBox.Items.Contains(col.Name) Then Me.ColumnToSum_CBox.Items.Add(col.Name)
        Next

        Me.CellBackColor_CBox.SelectedIndex = 0
        Me.CellForeColor_CBox.SelectedIndex = 0
    End Sub

    Private Sub IsCurrency_ChkBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles IsCurrency_ChkBox.CheckedChanged
        Me.DataGridViewFooter1.IsCurrency = CType(sender, CheckBox).Checked
    End Sub

    Private Sub FooterBackColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles FooterBackColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.FooterBackColorDemo_Lbl.BackColor)

        DataGridViewFooter1.BackColor = c

        Me.FooterBackColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub FooterForeColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles FooterForeColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.FooterForeColorDemo_Lbl.BackColor)

        DataGridViewFooter1.ForeColor = c

        Me.FooterForeColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub GridColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles GridColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.GridColorDemo_Lbl.BackColor)

        DataGridViewFooter1.GridColor = c

        Me.GridColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub CellsBackColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles CellsBackColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.CellsBackColorDemo_Lbl.BackColor)

        DataGridViewFooter1.CellBackColor = c

        Me.CellsBackColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub CellBackColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles CellBackColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.CellsBackColorDemo_Lbl.BackColor)

        DataGridViewFooter1.CellBackColor(CType(Me.CellBackColor_CBox.SelectedItem, String)) = c

        Me.CellBackColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub CellForeColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles CellForeColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.CellForeColorDemo_Lbl.BackColor)

        DataGridViewFooter1.CellForeColor(CType(Me.CellForeColor_CBox.SelectedItem, String)) = c

        Me.CellForeColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub SetImagePosButHighLight(ByVal position As ContentAlignment, ByRef Box As Control)
        Dim posText As String = [Enum].GetName(GetType(ContentAlignment), position)

        For Each but As Button In Box.Controls.OfType(Of Button)()
            If but.Text = posText Then
                but.BackColor = Color.LightSkyBlue
            Else
                but.BackColor = Color.FromKnownColor(KnownColor.Control)
            End If
        Next
    End Sub

    Private Sub RowHeaderImage_But_Click(sender As System.Object, e As System.EventArgs) Handles RowHeaderImage_But.Click
        Dim getImg As New OpenFileDialog

        getImg.CheckFileExists = True
        getImg.CheckPathExists = True
        getImg.Filter = "Image Files (*.bmp, *.jpg, *.png, *.gif)|*.bmp;*.jpg;*.png;*.gif"
        getImg.Multiselect = False
        getImg.ShowHelp = False
        getImg.Title = "Select an image <= RowHeader.Height"
        getImg.FilterIndex = 0

        Dim result As DialogResult = getImg.ShowDialog()

        If result <> Windows.Forms.DialogResult.OK Then Exit Sub

        Me.RowHeaderImageDemo_Lbl.Image = Image.FromFile(getImg.FileName)
        Me.DataGridViewFooter1.RowHeader.Image = Image.FromFile(getImg.FileName)

    End Sub

    Private Sub RowHeaderImagePos_But_Click(sender As System.Object, e As System.EventArgs) Handles _
        RowHeaderImagePosBC_But.Click, RowHeaderImagePosBL_But.Click, RowHeaderImagePosBR_But.Click, _
        RowHeaderImagePosMC_But.Click, RowHeaderImagePosML_But.Click, RowHeaderImagePosMR_But.Click, _
        RowHeaderImagePosTC_But.Click, RowHeaderImagePosTL_But.Click, RowHeaderImagePosTR_But.Click



        Dim pos As ContentAlignment = CType(CType(CType(sender, Button).Tag, Integer), ContentAlignment)

        Me.DataGridViewFooter1.RowHeader.ImagePosition = pos
        Me.RowHeaderImageDemo_Lbl.ImageAlign = pos

        SetImagePosButHighLight(pos, Me.ImagePosition_GrpBox)

    End Sub

    Private Sub RowHeaderTextPos_But_Click(sender As System.Object, e As System.EventArgs) Handles _
        RowHeaderTextPosBC_But.Click, RowHeaderTextPosBL_But.Click, RowHeaderTextPosBR_But.Click, _
        RowHeaderTextPosMC_But.Click, RowHeaderTextPosML_But.Click, RowHeaderTextPosMR_But.Click, _
        RowHeaderTextPosTC_But.Click, RowHeaderTextPosTL_But.Click, RowHeaderTextPosTR_But.Click



        Dim pos As ContentAlignment = CType(CType(CType(sender, Button).Tag, Integer), ContentAlignment)

        Me.DataGridViewFooter1.RowHeader.TextPosition = pos
        Me.RowHeaderTextDemo_Lbl.TextAlign = pos

        SetImagePosButHighLight(pos, Me.TextPosition_GrpBox)

    End Sub

    Private Sub RowHeaderBackColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles RowHeaderBackColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.RowHeaderBackColorDemo_Lbl.BackColor)

        DataGridViewFooter1.RowHeader.BackColor = c

        Me.RowHeaderBackColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub RowHeaderForeColorDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles RowHeaderForeColorDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.RowHeaderMouseOverDemo_Lbl.BackColor)

        DataGridViewFooter1.RowHeader.ForeColor = c

        Me.RowHeaderForeColorDemo_Lbl.BackColor = c
    End Sub

    Private Sub RowHeaderMouseOverDemo_Lbl_Click(sender As System.Object, e As System.EventArgs) Handles RowHeaderMouseOverDemo_Lbl.Click
        Dim c As Color

        c = GetColorFromDialog(Me.RowHeaderMouseOverDemo_Lbl.BackColor)

        DataGridViewFooter1.RowHeader.MouseOverColor = c

        Me.RowHeaderMouseOverDemo_Lbl.BackColor = c
    End Sub

    Private Sub RowHeaderText_TxtBox_TextChanged(sender As System.Object, e As System.EventArgs) Handles RowHeaderText_TxtBox.TextChanged
        Me.RowHeaderTextDemo_Lbl.Text = CType(sender, TextBox).Text
        Me.DataGridViewFooter1.RowHeader.Text = CType(sender, TextBox).Text
    End Sub

    Private Sub RowHeaderFont_But_Click(sender As System.Object, e As System.EventArgs) Handles RowHeaderFont_But.Click
        Dim fDialog As New FontDialog
        Dim result As DialogResult

        fDialog.Font = Me.DataGridViewFooter1.RowHeader.Font
        fDialog.AllowScriptChange = True
        fDialog.AllowSimulations = True
        fDialog.AllowVectorFonts = True
        fDialog.AllowVerticalFonts = True
        fDialog.Color = Me.DataGridViewFooter1.RowHeader.ForeColor
        fDialog.FontMustExist = True
        fDialog.MaxSize = 72
        fDialog.MinSize = 5
        fDialog.ScriptsOnly = True
        fDialog.ShowApply = False
        fDialog.ShowColor = True
        fDialog.ShowEffects = True
        fDialog.ShowHelp = False

        result = fDialog.ShowDialog()

        If result <> Windows.Forms.DialogResult.OK Then Exit Sub

        fDialog.Color = Me.DataGridViewFooter1.RowHeader.ForeColor

        Me.DataGridViewFooter1.RowHeader.Font = fDialog.Font
        Me.RowHeaderTextDemo_Lbl.Font = fDialog.Font

    End Sub

    Private Sub Help_But_Click(sender As System.Object, e As System.EventArgs) Handles Help_But.Click
        Dim pInfo As New Diagnostics.ProcessStartInfo("http://www.heribertolugo.com")

        Try
            Diagnostics.Process.Start(pInfo)
        Catch ex As Exception
            Diagnostics.Process.Start("iexplore.exe", pInfo.FileName)
        End Try
    End Sub

    Private Sub Help_But_Over(sender As System.Object, e As System.EventArgs) Handles Help_But.MouseHover
        CType(sender, Button).BackgroundImage = My.Resources.helpOver
    End Sub

    Private Sub Help_But_Out(sender As System.Object, e As System.EventArgs) Handles Help_But.MouseLeave
        CType(sender, Button).BackgroundImage = My.Resources.helpOut
    End Sub

    Private Function GetColorFromDialog(ByVal defaultColor As Color) As Color
        Dim cp As New ColorDialog
        Dim result As DialogResult

        cp.Color = defaultColor
        cp.AllowFullOpen = True
        result = cp.ShowDialog()

        If result <> Windows.Forms.DialogResult.OK Then Return defaultColor

        Return cp.Color
    End Function

    Private Sub ColumnToSum_CBox_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ColumnToSum_CBox.SelectedIndexChanged
        Me.DataGridViewFooter1.SumColumn(CType(sender, ComboBox).SelectedItem.ToString)
    End Sub

    Private Sub DataGridViewRowHeaderVisible_ChkBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles DataGridViewRowHeaderVisible_ChkBox.CheckedChanged
        Me.DataGridView1.RowHeadersVisible = CType(sender, CheckBox).Checked
        Me.DataGridViewFooter1.RowHeaderVisible = CType(sender, CheckBox).Checked
    End Sub
End Class
