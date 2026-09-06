Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D

Public Class FrmDashboard
    Dim alpha As Integer = 255
    Dim isFadingIn As Boolean = False
    Dim originalSizes As New Dictionary(Of PictureBox, Size)
    Dim targetSizes As New Dictionary(Of PictureBox, Size)
    Dim currentZoomTimersIn As New Dictionary(Of PictureBox, Timer)
    Dim currentZoomTimersOut As New Dictionary(Of PictureBox, Timer)
    Dim activeMenu As PictureBox = Nothing
    Dim zoomStep As Integer = 2
    Dim maxIncrease As Integer = 20

   

    Private Sub FrmDashboard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is MdiClient Then
                ctrl.BackColor = Color.FromArgb(80, 80, 80)
            End If
        Next
        Label8.BackColor = Color.FromArgb(80, 80, 80)

        PictureBox7.Visible = False
        PictureBox6.Visible = False
        Panel2.Visible = False
        Timer2.Start()
        For Each pb As PictureBox In {PictureBox1, PictureBox2, PictureBox3, PictureBox4, PictureBox5, PictureBox8, PictureBox9, PictureBox10}
            originalSizes(pb) = pb.Size
            targetSizes(pb) = New Size(pb.Width + maxIncrease, pb.Height + maxIncrease)
            Dim tIn As New Timer With {.Interval = 10}
            AddHandler tIn.Tick, AddressOf ZoomInTick
            currentZoomTimersIn(pb) = tIn
            Dim tOut As New Timer With {.Interval = 10}
            AddHandler tOut.Tick, AddressOf ZoomOutTick
            currentZoomTimersOut(pb) = tOut

        Next
        Underline1.Visible = False
        Underline2.Visible = False
        Underline3.Visible = False
        Underline4.Visible = False
        Underline5.Visible = False
        Underline6.Visible = False
        Underline7.Visible = False
        Underline8.Visible = False
        PictureBox1.Cursor = Cursors.Hand
        PictureBox2.Cursor = Cursors.Hand
        PictureBox3.Cursor = Cursors.Hand
        PictureBox4.Cursor = Cursors.Hand
        PictureBox5.Cursor = Cursors.Hand
        PictureBox9.Cursor = Cursors.Hand
        PictureBox8.Cursor = Cursors.Hand
        PictureBox7.Cursor = Cursors.Hand
        PictureBox10.Cursor = Cursors.Hand
        Button1.Cursor = Cursors.Hand
        cmbTema.Items.Add("Default")
        cmbTema.Items.Add("Ocean")
        cmbTema.Items.Add("Cool Mint")
        cmbTema.Items.Add("Bright Azure")
        cmbTema.Items.Add("Neptune")
        cmbTema.Items.Add("Hell Flame")
        cmbTema.Items.Add("Galactic Storm")
        cmbTema.Items.Add("Shadow Turquoise")
        cmbTema.Items.Add("Royale Eclipse")
        cmbTema.Items.Add("Hyper Spectrum")
        cmbTema.Items.Add("Assasin Slayer")
        cmbTema.Text = "Default"
        PictureBox1.Focus()
    End Sub

    Private Sub PictureBox_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
    Handles PictureBox1.MouseEnter, PictureBox2.MouseEnter, PictureBox3.MouseEnter, PictureBox4.MouseEnter, PictureBox5.MouseEnter, PictureBox8.MouseEnter, PictureBox9.MouseEnter, PictureBox10.MouseEnter

        Dim pb As PictureBox = CType(sender, PictureBox)
        currentZoomTimersOut(pb).Stop()
        currentZoomTimersIn(pb).Start()
        GetUnderline(pb).Visible = True
    End Sub

    Private Sub PictureBox_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
     Handles PictureBox1.MouseLeave, PictureBox2.MouseLeave, PictureBox3.MouseLeave, PictureBox4.MouseLeave, PictureBox5.MouseLeave, PictureBox8.MouseLeave, PictureBox9.MouseLeave, PictureBox10.MouseLeave

        Dim pb As PictureBox = CType(sender, PictureBox)
        currentZoomTimersIn(pb).Stop()
        currentZoomTimersOut(pb).Start()
        If activeMenu IsNot pb Then
            GetUnderline(pb).Visible = False
        End If
    End Sub

    Private Function GetUnderline(ByVal pb As PictureBox) As Panel

        Select Case pb.Name
            Case "PictureBox1" : Return Underline1
            Case "PictureBox2" : Return Underline2
            Case "PictureBox3" : Return Underline3
            Case "PictureBox4" : Return Underline4
            Case "PictureBox5" : Return Underline5
            Case "PictureBox8" : Return Underline6
            Case "PictureBox9" : Return Underline7
            Case "PictureBox10" : Return Underline8
        End Select
        Return Nothing
    End Function

    Private Sub ZoomInTick(ByVal sender As Object, ByVal e As EventArgs)
        Dim timer = CType(sender, Timer)
        Dim pb = currentZoomTimersIn.First(Function(x) x.Value Is timer).Key
        If pb.Width < targetSizes(pb).Width Then
            pb.Size = New Size(pb.Width + zoomStep, pb.Height + zoomStep)
        Else
            timer.Stop()
        End If
    End Sub
    Private Sub ZoomOutTick(ByVal sender As Object, ByVal e As EventArgs)
        Dim timer = CType(sender, Timer)
        Dim pb = currentZoomTimersOut.First(Function(x) x.Value Is timer).Key

        If pb.Width > originalSizes(pb).Width Then
            pb.Size = New Size(pb.Width - zoomStep, pb.Height - zoomStep)
        Else
            pb.Size = originalSizes(pb)
            timer.Stop()
        End If
    End Sub

    Private Sub PictureBox_Click(ByVal sender As Object, ByVal e As EventArgs) _
    Handles PictureBox1.Click, PictureBox2.Click, PictureBox3.Click, PictureBox4.Click, PictureBox5.Click, PictureBox8.Click, PictureBox9.Click, PictureBox10.Click

        Dim pb As PictureBox = CType(sender, PictureBox)

        ' ===== Hilangkan semua underline ====
        For Each p In {Underline1, Underline2, Underline3, Underline4, Underline5, Underline6, Underline7, Underline8}
            p.Visible = False
        Next

        ' ===== Tampilkan underline yang diklik ====
        GetUnderline(pb).Visible = True
        activeMenu = pb


        ' ===== Buka Form sesuai menu yang diklik ====
        Select Case pb.Name
            Case "PictureBox1"
                FrmCustomer.MdiParent = Me
                FrmCustomer.Show()
            Case "PictureBox8"
                FrmKaryawan.MdiParent = Me
                FrmKaryawan.Show()
            Case "PictureBox2"
                FrmPelayanan.MdiParent = Me
                FrmPelayanan.Show()
            Case "PictureBox9"
                FemJenis_Layanan.MdiParent = Me
                FemJenis_Layanan.Show()
            Case "PictureBox3"
                FrmTransaksi.MdiParent = Me
                FrmTransaksi.Show()
            Case "PictureBox4"
                FrmDetail_Transaksi.MdiParent = Me
                FrmDetail_Transaksi.Show()
            Case "PictureBox10"
                FrmPewangi.MdiParent = Me
                FrmPewangi.Show()
            Case "PictureBox5"
                FrmPembayaran.MdiParent = Me
                FrmPembayaran.Show()
        End Select
    End Sub


    Dim activeTheme As String = "Default"

    Private Sub cmbTema_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbTema.SelectedIndexChanged
        activeTheme = cmbTema.Text
        Panel1.Invalidate() ' refresh panel biar gambar ulang
    End Sub

    Private Sub Panel1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel1.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Select Case activeTheme

            Case "Default"
               
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.Black, Color.White, Drawing2D.LinearGradientMode.ForwardDiagonal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = {
            Color.Black,
            Color.DimGray,
            Color.SlateGray,
            Color.LightSlateGray,
            Color.Gainsboro,
            Color.LightSlateGray,
            Color.SlateGray,
            Color.Black
        }


                    blend.Positions = {0.0F, 0.12F, 0.25F, 0.4F, 0.55F, 0.7F, 0.85F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.Black
                    Label2.ForeColor = Color.Black
                    Label3.ForeColor = Color.Black
                    Label4.ForeColor = Color.Black
                    Label5.ForeColor = Color.Black
                    Label6.ForeColor = Color.Black
                    Label7.ForeColor = Color.Black
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.Black
                    Label10.ForeColor = Color.Black
                    Label11.ForeColor = Color.Black
                End Using

            Case "Ocean"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = New Color() {Color.Black, Color.Cyan, Color.Yellow, Color.Coral}
                    blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.Black
                    Label2.ForeColor = Color.Black
                    Label3.ForeColor = Color.Black
                    Label4.ForeColor = Color.Black
                    Label5.ForeColor = Color.Black
                    Label6.ForeColor = Color.Black
                    Label7.ForeColor = Color.Black
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.Black
                    Label10.ForeColor = Color.Black
                    Label11.ForeColor = Color.Black
                End Using

            Case "Cool Mint"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = New Color() {Color.MediumSlateBlue, Color.DarkTurquoise, Color.SpringGreen, Color.Black}
                    blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.Black
                    Label2.ForeColor = Color.Black
                    Label3.ForeColor = Color.Black
                    Label4.ForeColor = Color.Black
                    Label5.ForeColor = Color.Black
                    Label6.ForeColor = Color.Black
                    Label7.ForeColor = Color.Black
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.Black
                    Label10.ForeColor = Color.Black
                    Label11.ForeColor = Color.Black
                End Using

            Case "Bright Azure"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.White, Color.Azure, Drawing2D.LinearGradientMode.Vertical)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = {Color.LightSkyBlue, Color.DeepSkyBlue, Color.Azure, Color.WhiteSmoke}
                    blend.Positions = {0.0F, 0.3F, 0.6F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.Black
                    Label2.ForeColor = Color.Black
                    Label3.ForeColor = Color.Black
                    Label4.ForeColor = Color.Black
                    Label5.ForeColor = Color.Black
                    Label6.ForeColor = Color.Black
                    Label7.ForeColor = Color.Black
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.Black
                    Label10.ForeColor = Color.Black
                    Label11.ForeColor = Color.Black
                End Using

            Case "Neptune"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.OrangeRed, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.BackwardDiagonal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = New Color() {Color.Gray, Color.DarkCyan, Color.Black, Color.DarkCyan}
                    blend.Positions = New Single() {0.0F, 0.25F, 0.6F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.White
                    Label2.ForeColor = Color.White
                    Label3.ForeColor = Color.White
                    Label4.ForeColor = Color.White
                    Label5.ForeColor = Color.White
                    Label6.ForeColor = Color.White
                    Label7.ForeColor = Color.White
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.White
                    Label10.ForeColor = Color.White
                    Label11.ForeColor = Color.White
                End Using

            Case "Hell Flame"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.Black, Color.Red, Drawing2D.LinearGradientMode.BackwardDiagonal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = {
                        Color.Black,
                        Color.DarkRed,
                        Color.OrangeRed,
                        Color.Yellow,
                        Color.White,
                        Color.Orange,
                        Color.Red
                    }
                    blend.Positions = {0.0F, 0.1F, 0.25F, 0.4F, 0.6F, 0.8F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.Black
                    Label2.ForeColor = Color.Black
                    Label3.ForeColor = Color.Black
                    Label4.ForeColor = Color.Black
                    Label5.ForeColor = Color.Black
                    Label6.ForeColor = Color.Black
                    Label7.ForeColor = Color.Black
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.Black
                    Label10.ForeColor = Color.Black
                    Label11.ForeColor = Color.Black
                End Using

            Case "Galactic Storm"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DarkBlue, Color.DeepPink, Drawing2D.LinearGradientMode.BackwardDiagonal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = {
                        Color.Black,
                        Color.DarkViolet,
                        Color.Magenta,
                        Color.Cyan,
                        Color.LimeGreen,
                        Color.Yellow,
                        Color.OrangeRed
                    }
                    blend.Positions = {0.0F, 0.15F, 0.3F, 0.5F, 0.7F, 0.85F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.Black
                    Label2.ForeColor = Color.Black
                    Label3.ForeColor = Color.Black
                    Label4.ForeColor = Color.Black
                    Label5.ForeColor = Color.Black
                    Label6.ForeColor = Color.Black
                    Label7.ForeColor = Color.Black
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.Black
                    Label10.ForeColor = Color.Black
                    Label11.ForeColor = Color.Black
                End Using

            Case "Shadow Turquoise"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.Black, Color.DarkSlateGray, Drawing2D.LinearGradientMode.Horizontal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = {
                        Color.Crimson,
                        Color.DarkViolet,
                        Color.Purple,
                        Color.DarkTurquoise
                    }
                    blend.Positions = {0.0F, 0.35F, 0.7F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.White
                    Label2.ForeColor = Color.White
                    Label3.ForeColor = Color.White
                    Label4.ForeColor = Color.White
                    Label5.ForeColor = Color.White
                    Label6.ForeColor = Color.White
                    Label7.ForeColor = Color.White
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.White
                    Label10.ForeColor = Color.White
                    Label11.ForeColor = Color.White
                End Using

            Case "Royale Eclipse"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.Black, Color.Gold, Drawing2D.LinearGradientMode.ForwardDiagonal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = {
                        Color.Red,
                        Color.MidnightBlue,
                        Color.Lime,
                        Color.RosyBrown,
                        Color.Goldenrod,
                        Color.Gold,
                        Color.SteelBlue
                    }
                    blend.Positions = {
                        0.0F,
                        0.12F,
                        0.27F,
                        0.45F,
                        0.65F,
                        0.82F,
                        1.0F
                    }
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.Black
                    Label2.ForeColor = Color.Black
                    Label3.ForeColor = Color.Black
                    Label4.ForeColor = Color.Black
                    Label5.ForeColor = Color.Black
                    Label6.ForeColor = Color.Black
                    Label7.ForeColor = Color.Black
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.Black
                    Label10.ForeColor = Color.Black
                    Label11.ForeColor = Color.Black
                End Using

            Case "Hyper Spectrum"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.Black, Color.White, Drawing2D.LinearGradientMode.ForwardDiagonal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = {
                        Color.Black,
                        Color.Red,
                        Color.Magenta,
                        Color.Blue,
                        Color.Cyan,
                        Color.Lime,
                        Color.Yellow,
                        Color.Orange
                    }
                    blend.Positions = {0.0F, 0.12F, 0.25F, 0.4F, 0.55F, 0.7F, 0.85F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    Label1.ForeColor = Color.Black
                    Label2.ForeColor = Color.Black
                    Label3.ForeColor = Color.Black
                    Label4.ForeColor = Color.Black
                    Label5.ForeColor = Color.Black
                    Label6.ForeColor = Color.Black
                    Label7.ForeColor = Color.Black
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.Black
                    Label10.ForeColor = Color.Black
                    Label11.ForeColor = Color.Black
                End Using

            Case "Assasin Slayer"
                Using brush As New Drawing2D.LinearGradientBrush(rect, Color.Black, Color.MidnightBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
                    Dim blend As New Drawing2D.ColorBlend()
                    blend.Colors = New Color() {
                        Color.MidnightBlue,
                        Color.Black,
                        Color.WhiteSmoke,
                        Color.Red,
                        Color.SteelBlue,
                        Color.LightSteelBlue,
                                Color.Black
                    }
                    blend.Positions = New Single() {0.0F, 0.15F, 0.32F, 0.5F, 0.7F, 0.85F, 1.0F}
                    brush.InterpolationColors = blend
                    g.FillRectangle(brush, rect)
                    For Each ctrl As Control In Me.Controls
                        If TypeOf ctrl Is Label Then ctrl.ForeColor = Color.White
                    Next
                    Label1.ForeColor = Color.White
                    Label2.ForeColor = Color.White
                    Label3.ForeColor = Color.White
                    Label4.ForeColor = Color.White
                    Label5.ForeColor = Color.White
                    Label6.ForeColor = Color.White
                    Label7.ForeColor = Color.White
                    Label8.ForeColor = Color.Black
                    Label9.ForeColor = Color.White
                    Label10.ForeColor = Color.White
                    Label11.ForeColor = Color.White
                End Using
        End Select
    End Sub

    Private Sub PictureBox6_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PictureBox6.Click
        alpha = 255
        isFadingIn = False
        Timer1.Start()
        PictureBox6.Visible = False
        PictureBox7.Visible = True
    End Sub

    Private Sub PictureBox7_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PictureBox7.Click
        alpha = 0
        Panel1.Visible = True
        isFadingIn = True
        Timer1.Start()
        PictureBox6.Visible = True
        PictureBox7.Visible = False
    End Sub


    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick

        If isFadingIn = False Then
            ' FADING OUT
            alpha -= 15
            If alpha <= 0 Then
                Timer1.Stop()
                Panel1.Visible = False
            Else
                Panel1.BackColor = Color.FromArgb(alpha, Panel1.BackColor.R, Panel1.BackColor.G, Panel1.BackColor.B)
            End If

        Else
            ' FADING IN
            alpha += 15
            If alpha >= 255 Then
                alpha = 255
                Timer1.Stop()
            End If

            Panel1.BackColor = Color.FromArgb(alpha, Panel1.BackColor.R, Panel1.BackColor.G, Panel1.BackColor.B)
        End If

    End Sub

    Private Sub Panel2_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel2.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel2.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = New Color() {Color.MediumSlateBlue, Color.DarkTurquoise, Color.SpringGreen, Color.Black}
            blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Label8.Text = DateTime.Now.ToString("HH:mm:ss")
    End Sub

    
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        Form1.Show()
    End Sub
End Class