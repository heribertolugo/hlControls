Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.ComponentModel

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class DataGridViewRowHeaderX
    Inherits System.Windows.Forms.DataGridViewRowHeaderCell

#Region "Private Members"
    Private _backColor As Color = Color.FromKnownColor(KnownColor.Control)
    Private _foreColor As Color = Color.Black
    Private _mouseOverColor As Color = Color.PaleTurquoise
    Private _image As Image
    Private _imagePosition As ContentAlignment = ContentAlignment.MiddleCenter
    Private _text As String = ""
    Private _textPosition As ContentAlignment = ContentAlignment.MiddleCenter
    Private _font As Font
    Private isMouseOver As Boolean = False
#End Region

#Region "Contructors and Setup"
    Public Sub New()
        Me._font = New Font(System.Drawing.FontFamily.GenericSansSerif, 8, FontStyle.Regular, GraphicsUnit.Point)

        'Me._backColor = New Color
        '_backColor = Color.FromKnownColor(KnownColor.Control)
        'Me._foreColor = Color.Black
        'Me._mouseOverColor = Color.DeepSkyBlue

        'Me._imagePosition = ContentAlignment.MiddleCenter
        'Me._text = ""
        'Me._textPosition = ContentAlignment.MiddleCenter
        'Me.isMouseOver = False
    End Sub
#End Region

#Region "Properties"

    ''' <summary>
    ''' BackColor of the Control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("BackColor of the Control."), _
    Category("Design"), _
    DefaultValue("")> _
    Public Property BackColor As Color
        Get
            Return Me._backColor
        End Get
        Set(value As Color)
            Me._backColor = value
            If Not IsNothing(Me.DataGridView) Then
                Me.DataGridView.InvalidateRow(0)
            End If
        End Set
    End Property

    ''' <summary>
    ''' ForeColor of the Control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("ForeColor of the Control."), _
    Category("Design"), _
    DefaultValue("")> _
    Public Property ForeColor As Color
        Get
            Return Me._foreColor
        End Get
        Set(value As Color)
            Me._foreColor = value
            If Not IsNothing(Me.DataGridView) Then
                Me.DataGridView.InvalidateRow(0)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Color when the mouse is over the Control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Color when the mouse is over the Control."), _
    Category("Design"), _
    DefaultValue("")> _
    Public Property MouseOverColor As Color
        Get
            Return Me._mouseOverColor
        End Get
        Set(value As Color)
            Me._mouseOverColor = value
        End Set
    End Property

    ', EditorAttribute(GetType(DataGridViewFooterHeaderEditor), GetType(System.Drawing.Design.UITypeEditor))> _
    ''' <summary>
    ''' Image to display. This can only be set programmatically.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Image to display. This can only be set programmatically."), _
    Category("Design"), _
    DefaultValue("")> _
    Public Property Image As Image
        Get
            Return Me._image
        End Get
        Set(value As Image)
            Me._image = value
            If Not IsNothing(Me.DataGridView) Then Me.DataGridView.InvalidateRow(0)
        End Set
    End Property

    ''' <summary>
    ''' Position of Image to display.
    ''' </summary>
    ''' <value>A position based on ContentAlignment to set the image position.</value>
    ''' <returns>A position based on ContentAlignment of the current image position.</returns>
    ''' <remarks></remarks>
    <Description("Position of Image to display."), _
    Category("Design"), _
    DefaultValue(32)> _
    Public Property ImagePosition As ContentAlignment
        Get
            Return Me._imagePosition
        End Get
        Set(value As ContentAlignment)
            Me._imagePosition = value
            If Not IsNothing(Me.DataGridView) Then Me.DataGridView.InvalidateRow(0)
        End Set
    End Property

    ', EditorAttribute(GetType(DataGridViewFooterHeaderEditor), GetType(System.Drawing.Design.UITypeEditor))> _
    ''' <summary>
    ''' Text to display.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Text to display."), _
    Category("Design"), _
    DefaultValue("")> _
    Public Property Text As String
        Get
            Return Me._text
        End Get
        Set(value As String)
            Me._text = value
            If Not IsNothing(Me.DataGridView) Then Me.DataGridView.InvalidateRow(0)
        End Set
    End Property

    ''' <summary>
    ''' Position of Text to display.
    ''' </summary>
    ''' <value>A position based on ContentAlignment to set the Text position.</value>
    ''' <returns>A position based on ContentAlignment of the current Text position.</returns>
    ''' <remarks></remarks>
    <Description("Position of Text to display."), _
    Category("Design"), _
    DefaultValue(32)> _
    Public Property TextPosition As ContentAlignment
        Get
            Return Me._textPosition
        End Get
        Set(value As ContentAlignment)
            Me._textPosition = value
            If Not IsNothing(Me.DataGridView) Then Me.DataGridView.InvalidateRow(0)
        End Set
    End Property

    ''' <summary>
    ''' Position of Text to display.
    ''' </summary>
    ''' <value>A position based on ContentAlignment to set the Text position.</value>
    ''' <returns>A position based on ContentAlignment of the current Text position.</returns>
    ''' <remarks></remarks>
    <Description("Position of Text to display."), _
    Category("Design"), _
    DefaultValue(32)> _
    Public Property Font As Font
        Get
            Return Me._font
        End Get
        Set(value As Font)
            Me._font = value
            If Not IsNothing(Me.DataGridView) Then Me.DataGridView.InvalidateRow(0)
        End Set
    End Property

    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)>
    Shadows Property CellStyle As DataGridViewCellStyle
        Get
            Return Nothing
        End Get
        Set(value As DataGridViewCellStyle)

        End Set
    End Property

    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)>
    Shadows Property Style As DataGridViewCellStyle
        Get
            Return Nothing
        End Get
        Set(value As DataGridViewCellStyle)

        End Set
    End Property

    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)>
    Shadows Property Value As DataGridViewCellStyle
        Get
            Return Nothing
        End Get
        Set(value As DataGridViewCellStyle)

        End Set
    End Property
#End Region

#Region "Overrides"
    Protected Overrides Sub Paint(graphics As System.Drawing.Graphics, clipBounds As System.Drawing.Rectangle, cellBounds As System.Drawing.Rectangle, rowIndex As Integer, cellState As System.Windows.Forms.DataGridViewElementStates, value As Object, formattedValue As Object, errorText As String, cellStyle As System.Windows.Forms.DataGridViewCellStyle, advancedBorderStyle As System.Windows.Forms.DataGridViewAdvancedBorderStyle, paintParts As System.Windows.Forms.DataGridViewPaintParts)
        Dim p As New DataGridViewPaintParts
        'Dim g As New LinearGradientBrush(New Point(0, 5), New Point(cellBounds.Width, 0), Color.White, If(isMouseOver, _mouseOverColor, _backColor)) 'cellStyle.BackColor

        'Remove glyph
        p = CType(CInt(paintParts) + (Not CInt(DataGridViewPaintParts.ContentBackground)), DataGridViewPaintParts)

        'Paint the background color. Determine if MouseOverColor is needed, or just default.
        graphics.FillRectangle(New SolidBrush(If(isMouseOver, _mouseOverColor, _backColor)), cellBounds)
        'Paint the Glossy effect stripe on left
        'graphics.FillRectangle(New SolidBrush(Color.FromArgb(175, Color.White)), New Rectangle(0, 0, 12, cellBounds.Height))
        graphics.FillRectangle(New SolidBrush(If(isMouseOver, MakeLighter(_mouseOverColor, 0.4), MakeLighter(_backColor, 0.25))), New Rectangle(0, 0, 12, cellBounds.Height))
        'Paint whats left.
        MyBase.Paint(graphics, clipBounds, cellBounds, 0, cellState, New Object(), formattedValue, errorText, cellStyle, advancedBorderStyle, p)

        'Draw the 3D effect line on the right
        graphics.DrawLine(New Pen(Brushes.LightGray), New Point(cellBounds.Width - 2, 2), New Point(cellBounds.Width - 2, cellBounds.Height - 1))

        If isMouseOver Then Paint3dBorder(graphics, cellBounds.Size, _mouseOverColor)

        'If we have an image, paint it now
        If Not _image Is Nothing Then PaintImage(graphics)

        'If we have text, paint it now
        If _text.Trim <> "" Then PaintString(graphics)
    End Sub

    Protected Overrides Sub OnMouseEnter(rowIndex As Integer)
        MyBase.OnMouseEnter(rowIndex)

        isMouseOver = True
    End Sub

    Protected Overrides Sub OnMouseLeave(rowIndex As Integer)
        MyBase.OnMouseLeave(rowIndex)

        isMouseOver = False
    End Sub

    Protected Overrides Sub OnDataGridViewChanged()
        If Not Me.DataGridView Is Nothing Then
            MyBase.OnDataGridViewChanged()

            With Me.DataGridView
                _font = If(.RowsDefaultCellStyle.Font IsNot Nothing, .RowsDefaultCellStyle.Font, If(.DefaultCellStyle.Font IsNot Nothing, .DefaultCellStyle.Font, _font))
            End With

            isMouseOver = False
        End If
    End Sub

    Public Overrides Function Clone() As Object
        Return Me.MemberwiseClone
    End Function

#End Region

#Region "Private Methods and Sub-Procedures"

    Private Sub PaintImage(ByRef g As Graphics)
        Dim iPoint As Point = Point.Round(GetPositionPoints(_image.Size, Me._imagePosition))

        'If (iPoint.X + Me.Image.Width) > Me.Size.Width Then iPoint.X = (iPoint.X - Me.Size.Width)
        If (iPoint.X <= 0) Then iPoint.X = 1

        'If (iPoint.Y + Me.Size.Height) > Me.Size.Height Then iPoint.Y = (iPoint.Y - Me.Size.Height)
        If (iPoint.Y <= 0) Then iPoint.Y = 1

        g.DrawImage(_image, iPoint)
    End Sub

    Private Sub PaintString(ByRef g As Graphics)
        Dim sPoint As PointF = GetPositionPoints(Size.Ceiling(g.MeasureString(Me._text, Me._font)), Me._textPosition)
        g.DrawString(_text, _font, New SolidBrush(_foreColor), New PointF(sPoint.X, sPoint.Y))
    End Sub

    Private Sub Paint3dBorder(ByRef gx As Graphics, ByVal rectSize As Size, ByVal color As Color)
        'Dim r, g, b As UInteger
        'Dim h, s, v As Double
        Dim newColor As Color

        'RGBToHSV(r, g, b, h, s, v)
        'HSVToRGB(h, s, 0.5, r, g, b)

        'Dim alphaColor As Color = color.FromArgb(50, color.DarkGray)

        'A darker shade of our color
        newColor = MakeDarker(_mouseOverColor, 0.2)

        'Draw the 3D effect line on the top
        'gx.DrawLine(New Pen(alphaColor), New Point(2, 1), New Point(rectSize.Width - 2, 1))
        gx.FillRectangle(New SolidBrush(newColor), New Rectangle(2, 1, rectSize.Width - 2, 2))

        ''Draw the 3D effect line on the bottom
        'gx.DrawLine(New Pen(alphaColor), New Point(2, rectSize.Height), New Point(rectSize.Width - 2, rectSize.Height))
        gx.FillRectangle(New SolidBrush(ChangeColorShade(color, 30, False)), New Rectangle(2, rectSize.Height - 1, rectSize.Width - 2, 2))

    End Sub

    Private Function GetPositionPoints(ByVal objSize As SizeF, ByVal position As ContentAlignment) As System.Drawing.PointF
        Dim xPos As Single = 0
        Dim yPos As Single = 0
        Dim posInt As Integer = CInt(position)

        If posInt < 16 Then
            yPos = 0
        ElseIf posInt >= 16 And posInt < 256 Then
            yPos = CenterRectPoint(objSize).Y
        Else
            yPos = BottomRectPointY(objSize.Height)
        End If


        Select Case posInt
            Case 1, 16, 256
                xPos = 0
            Case 2, 32, 512
                xPos = CenterRectPoint(objSize).X
            Case Else
                xPos = RightRectPointX(objSize.Width)
        End Select

        Return New PointF(xPos, yPos)
    End Function

    Private Function CenterRectPoint(ByVal rectSize As SizeF) As PointF
        Dim outerRect As SizeF = Me.Size
        Dim outerRectMidX As Single = CSng(outerRect.Width / 2)
        Dim outerRectMidY As Single = CSng(outerRect.Height / 2)
        Dim innerRectMidX As Single = CSng(rectSize.Width / 2)
        Dim innerRectMidY As Single = CSng(rectSize.Height / 2)
        Dim midX As Single = outerRectMidX - innerRectMidX
        Dim midY As Single = outerRectMidY - innerRectMidY

        Return New PointF(midX, midY)
    End Function

    Private Function BottomRectPointY(ByVal rectHeight As Single) As Single
        Dim bottom As Single = (Me.Size.Height - rectHeight)
        Return If(bottom > 0, bottom, bottom)
    End Function

    Private Function RightRectPointX(ByVal rectWidth As Single) As Single
        Dim right As Single = CSng(Me.Size.Width - rectWidth)

        Return If(right > 0, right, right)
    End Function

    'http://www.paulund.co.uk/calculates-different-shades-colour
    Private Function ChangeColorShade(ByVal color As Color, ByVal percentChange As Integer, Optional ByVal makeLighter As Boolean = True) As Color
        Dim r As Byte = color.R
        Dim g As Byte = color.G
        Dim b As Byte = color.B
        Dim newR, newG, newB As Integer

        If makeLighter Then
            newR = CInt(255 - (255 - r) + percentChange)
            newG = CInt(255 - (255 - g) + percentChange)
            newB = CInt(255 - (255 - b) + percentChange)
            If newR < 0 Then newR = 0
            If newG < 0 Then newG = 0
            If newB < 0 Then newB = 0
            If newR > 255 Then newR = 255
            If newG > 255 Then newG = 255
            If newB > 255 Then newB = 255

            Return color.FromArgb(255, newR, newG, newB)
        Else
            newR = CInt(r - percentChange)
            newG = CInt(g - percentChange)
            newB = CInt(b - percentChange)
            If newR < 0 Then newR = 0
            If newG < 0 Then newG = 0
            If newB < 0 Then newB = 0
            If newR > 255 Then newR = 255
            If newG > 255 Then newG = 255
            If newB > 255 Then newB = 255

            Return color.FromArgb(255, newR, newG, newB)

        End If
    End Function

    'http://stackoverflow.com/a/97697
    Private Sub RGBToHSV(ByVal cr As UInteger, ByVal cg As UInteger, ByVal cb As UInteger, ByRef ph As Double, ByRef ps As Double, ByRef pv As Double)
        Dim r, g, b As Double
        Dim max, min, delta As Double

        '/*convert RGB to [0,1] */

        r = CDbl(cr / 255.0F)
        g = CDbl(cg / 255.0F)
        b = CDbl(cb / 255.0F)

        max = Math.Max(r, (Math.Max(g, b)))
        min = Math.Min(r, (Math.Min(g, b)))

        pv = max

        '/* Calculate saturation */

        If (max <> 0.0) Then
            ps = (max - min) / max
        Else
            ps = 0.0
        End If
        If (ps = 0.0) Then

            ph = 0.0F   '//UNDEFINED;
            Return
        End If
        '/* chromatic case: Saturation is not 0, so determine hue */
        delta = max - min

        If (r = max) Then

            ph = (g - b) / delta

        ElseIf (g = max) Then

            ph = 2.0 + (b - r) / delta

        ElseIf (b = max) Then

            ph = 4.0 + (r - g) / delta
        End If
        ph = ph * 60.0
        If (ph < 0.0) Then
            ph += 360.0
        End If
    End Sub

    'http://stackoverflow.com/a/97697
    Private Sub HSVToRGB(ByVal h As Double, ByVal s As Double, ByVal v As Double, ByRef pr As UInteger, ByRef pg As UInteger, ByRef pb As UInteger)

        Dim i As Integer
        Dim f, p, q, t As Double
        Dim r, g, b As Double

        If (s = 0) Then

            '// achromatic (grey)
            'r = g = b = v
            b = v
            g = b
            r = g
        Else

            h /= 60         '// sector 0 to 5
            i = CInt(Math.Floor(h))
            f = h - i           '// factorial part of h
            p = v * (1 - s)
            q = v * (1 - s * f)
            t = v * (1 - s * (1 - f))
            Select Case (i)
                Case 0
                    r = v
                    g = t
                    b = p
                Case 1
                    r = q
                    g = v
                    b = p
                Case 2
                    r = p
                    g = v
                    b = t
                Case 3
                    r = p
                    g = q
                    b = v
                Case 4
                    r = t
                    g = p
                    b = v
                Case Else '// case 5:
                    r = v
                    g = p
                    b = q
            End Select
        End If
        r *= 255
        g *= 255
        b *= 255

        pr = CType(r, UInteger)
        pg = CType(g, UInteger)
        pb = CType(b, UInteger)
    End Sub

    'http://stackoverflow.com/a/97796
    Private Function MakeLighter(ByVal color As Color, ByVal change As Double) As Color
        Dim Rnew As Double = (1 - change) * color.R + change * 255
        Dim Gnew As Double = (1 - change) * color.G + change * 255
        Dim Bnew As Double = (1 - change) * color.B + change * 255

        Return color.FromArgb(CInt(Rnew), CInt(Gnew), CInt(Bnew))
    End Function

    'http://stackoverflow.com/a/97796
    Private Function MakeDarker(ByVal color As Color, ByVal change As Double) As Color
        Dim Rnew As Double = (1 - change) * color.R + change * 0
        Dim Gnew As Double = (1 - change) * color.G + change * 0
        Dim Bnew As Double = (1 - change) * color.B + change * 0

        Return color.FromArgb(CInt(Rnew), CInt(Gnew), CInt(Bnew))
    End Function
#End Region


End Class