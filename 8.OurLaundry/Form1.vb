Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D

Public Class Form1

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Label3.Parent = PictureBox1
        Label3.BackColor = Color.Transparent
        Label4.Parent = PictureBox1
        Label4.BackColor = Color.Transparent
        RoundedPictureBox(PictureBox1, 20)
        Dim radius As Integer = 20 ' Semakin besar, semakin melengkung
        Me.Region = New Region(RoundedRect(New Rectangle(0, 0, Me.Width, Me.Height), radius))
        RoundedPanelll(Panel3, 20)
        btnLogin.Cursor = Cursors.Hand
        btnClose.Cursor = Cursors.Hand
        PicShow.Cursor = Cursors.Hand
    End Sub

    Private Function RoundedRect(ByVal rect As Rectangle, ByVal radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim diameter As Integer = radius * 2
        Dim arc As New Rectangle(rect.Location, New Size(diameter, diameter))

        ' Kiri atas
        path.AddArc(arc, 180, 90)

        ' Kanan atas
        arc.X = rect.Right - diameter
        path.AddArc(arc, 270, 90)

        ' Kanan bawah
        arc.Y = rect.Bottom - diameter
        path.AddArc(arc, 0, 90)

        ' Kiri bawah
        arc.X = rect.Left
        path.AddArc(arc, 90, 90)

        path.CloseFigure()
        Return path
    End Function

    Private Sub RoundedPictureBox(ByVal pb As PictureBox, ByVal radius As Integer)
        Dim path As New GraphicsPath()
        path.StartFigure()
        path.AddArc(0, 0, radius, radius, 180, 90)
        path.AddArc(pb.Width - radius, 0, radius, radius, 270, 90)
        path.AddArc(pb.Width - radius, pb.Height - radius, radius, radius, 0, 90)
        path.AddArc(0, pb.Height - radius, radius, radius, 90, 90)
        path.CloseFigure()
        pb.Region = New Region(path)
    End Sub

    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Try
            Call Koneksi()

            Dim cmd As New MySqlCommand(
                "SELECT * FROM login WHERE username=@user AND password=@pass", Conn)

            cmd.Parameters.AddWithValue("@user", TextBox1.Text.Trim())
            cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim())

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.HasRows Then
                MsgBox("Login berhasil!", MsgBoxStyle.Information, "Sukses")

                rd.Close()
                Conn.Close()

                FrmDashboard.Show()
                Me.Hide()   ' sembunyikan form login
            Else
                MsgBox("Username atau password salah!", MsgBoxStyle.Critical, "Gagal")
                rd.Close()
                Conn.Close()
            End If

        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message)
        End Try
    End Sub

    Private Sub Panel1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel1.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.Black, Color.White, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = {
    Color.Navy,
    Color.MidnightBlue,
    Color.Indigo,
    Color.DarkViolet,
    Color.MediumPurple,
    Color.DodgerBlue,
    Color.MidnightBlue,
    Color.Navy
}
            blend.Positions = {0.0F, 0.12F, 0.25F, 0.4F, 0.55F, 0.7F, 0.85F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub PicShow_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles PicShow.MouseDown
        txtPassword.UseSystemPasswordChar = False
    End Sub
    Private Sub PicShow_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles PicShow.MouseUp
        txtPassword.UseSystemPasswordChar = True
    End Sub

    Private Sub Panel3_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel3.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel3.ClientRectangle
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

        End Using
    End Sub

    Private Sub RoundedPanelll(ByVal pnl As Panel, ByVal radius As Integer)
        Dim path As New GraphicsPath()
        path.StartFigure()

        path.AddArc(0, 0, radius, radius, 180, 90)
        path.AddArc(pnl.Width - radius, 0, radius, radius, 270, 90)
        path.AddArc(pnl.Width - radius, pnl.Height - radius, radius, radius, 0, 90)
        path.AddArc(0, pnl.Height - radius, radius, radius, 90, 90)

        path.CloseFigure()
        pnl.Region = New Region(path)
    End Sub
End Class
