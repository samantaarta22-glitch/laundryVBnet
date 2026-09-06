Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class FrmKaryawan
    Dim FadeImage As Image
    Dim FadeAlpha As Integer = 0
    Dim idLama As String

    Private Sub FrmKaryawan_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        TampilData()
        picFoto1.SizeMode = PictureBoxSizeMode.StretchImage
        Dim radius As Integer = 20 ' Semakin besar, semakin melengkung
        Me.Region = New Region(RoundedRect(New Rectangle(0, 0, Me.Width, Me.Height), radius))
        btnSimpan.Cursor = Cursors.Hand
        btnHapus.Cursor = Cursors.Hand
        btnBatal.Cursor = Cursors.Hand
        btnTutup.Cursor = Cursors.Hand
        picFoto1.Cursor = Cursors.Hand
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

    <DllImport("user32.dll")>
    Private Shared Sub ReleaseCapture()
    End Sub

    <DllImport("user32.dll")>
    Private Shared Sub SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer)
    End Sub

    Private Const WM_NCLBUTTONDOWN As Integer = &HA1
    Private Const HTCAPTION As Integer = 2
    Private Sub FrmCustomer_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Me.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        End If
    End Sub
    Private Sub Panel2_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Panel2.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Me.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        End If
    End Sub

    Sub TampilData()
        Call Koneksi()
        Da = New MySqlDataAdapter("SELECT id_karyawan, nama, alamat, notelp, foto FROM karyawan", Conn)
        Ds = New DataSet
        Da.Fill(Ds)
        DataGridView1.DataSource = Ds.Tables(0)
        Dim imgcol As New DataGridViewImageColumn
        imgcol = DataGridView1.Columns("foto")
        imgcol.ImageLayout = DataGridViewImageCellLayout.Zoom
        DataGridView1.Columns("foto").Visible = False
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        With DataGridView1
            .Columns("id_karyawan").HeaderText = "ID Karyawan"
            .Columns("nama").HeaderText = "Nama"
            .Columns("alamat").HeaderText = "Alamat"
            .Columns("notelp").HeaderText = "No. Telepon"
            .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
            .DefaultCellStyle.SelectionBackColor = Color.Black
            .DefaultCellStyle.SelectionForeColor = Color.Lime
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.Lime
            DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.Font = New Font("Palatino Linotype", 10, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        End With
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            Dim i As Integer = e.RowIndex
            If i < 0 Then Exit Sub

            txtID.Text = DataGridView1.Item("id_karyawan", i).Value.ToString
            txtNama.Text = DataGridView1.Item("nama", i).Value.ToString
            txtAlamat.Text = DataGridView1.Item("alamat", i).Value.ToString
            txtTelepon.Text = DataGridView1.Item("notelp", i).Value.ToString

            ' ===== CEK JIKA FOTO KOSONG =====
            If IsDBNull(DataGridView1.Item("foto", i).Value) OrElse DataGridView1.Item("foto", i).Value Is Nothing Then
                picFoto1.Image = Nothing
                Exit Sub
            End If

            ' ===== JIKA ADA FOTO BARU DI-LOAD =====
            Dim imgBytes() As Byte = DataGridView1.Item("foto", i).Value
            Dim ms As New IO.MemoryStream(imgBytes)
            FadeImage = Image.FromStream(ms)

            FadeAlpha = 0
            Timer1.Start()
            picFoto1.Invalidate()

        Catch ex As Exception
            ' Jangan tampilkan pesan apa pun agar user tidak terganggu
        End Try
    End Sub
    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick
        If FadeAlpha >= 255 Then
            Timer1.Stop()
            picFoto1.Image = FadeImage
            picFoto1.Invalidate()
            Exit Sub
        End If

        FadeAlpha += 25
        If FadeAlpha > 255 Then FadeAlpha = 255

        picFoto1.Image = ApplyFade(FadeImage, FadeAlpha)
        picFoto1.Invalidate()
    End Sub
    Private Function ApplyFade(ByVal img As Image, ByVal alpha As Integer) As Image
        Dim bmp As New Bitmap(img.Width, img.Height)
        Using g As Graphics = Graphics.FromImage(bmp)
            Dim cm As New Imaging.ColorMatrix()
            cm.Matrix33 = alpha / 255.0F   ' transparency 0-1.0

            Dim ia As New Imaging.ImageAttributes()
            ia.SetColorMatrix(cm)

            g.DrawImage(img, New Rectangle(0, 0, bmp.Width, bmp.Height),
                        0, 0, img.Width, img.Height,
                        GraphicsUnit.Pixel, ia)
        End Using
        Return bmp
    End Function

    Private Sub picFoto1_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles picFoto1.DoubleClick
        Dim OFD As New OpenFileDialog
        OFD.Filter = "File Foto|*.jpg;*.jfif;*.jpeg;*.png;*.bmp"

        If OFD.ShowDialog() = DialogResult.OK Then
            picFoto1.Image = Image.FromFile(OFD.FileName)
            picFoto1.Invalidate()
            picFoto1.SizeMode = PictureBoxSizeMode.StretchImage
        End If
    End Sub
    Private Sub btnSimpan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSimpan.Click
        Try
            ' VALIDASI DATA
            If txtID.Text = "" Or txtNama.Text = "" Or txtAlamat.Text = "" Or txtTelepon.Text = "" Then
                MsgBox("Data belum lengkap!", vbInformation)
                Exit Sub
            End If

            ' === KONVERSI FOTO (PictureBox) KE BYTE ===
            Dim fotoBytes() As Byte = Nothing
            If picFoto1.Image IsNot Nothing Then
                Dim ms As New IO.MemoryStream()
                picFoto1.Image.Save(ms, picFoto1.Image.RawFormat)
                fotoBytes = ms.ToArray()
            End If

            Call Koneksi()

            ' === CEK APAKAH ID SUDAH ADA ===
            Dim cek As New MySqlCommand("SELECT COUNT(*) FROM karyawan WHERE id_karyawan=@id", Conn)
            cek.Parameters.AddWithValue("@id", txtID.Text)
            Dim jumlah As Integer = cek.ExecuteScalar()

            Dim sql As String

            If jumlah > 0 Then
                ' =====================================
                '        MODE UPDATE (Edit Data)
                ' =====================================
                sql = "UPDATE karyawan SET nama=@nama, alamat=@alamat, notelp=@notelp, foto=@foto WHERE id_karyawan=@id"
            Else
                ' =====================================
                '        MODE INSERT (Tambah Data)
                ' =====================================
                sql = "INSERT INTO karyawan (id_karyawan, nama, alamat, notelp, foto) VALUES (@id, @nama, @alamat, @notelp, @foto)"
            End If

            ' EKSEKUSI QUERY
            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtID.Text)
            Cmd.Parameters.AddWithValue("@nama", txtNama.Text)
            Cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text)
            Cmd.Parameters.AddWithValue("@notelp", txtTelepon.Text)

            If fotoBytes IsNot Nothing Then
                Cmd.Parameters.AddWithValue("@foto", fotoBytes)
            Else
                Cmd.Parameters.AddWithValue("@foto", DBNull.Value)
            End If

            Cmd.ExecuteNonQuery()

            If jumlah > 0 Then
                MsgBox("Data berhasil diupdate!", vbInformation)
            Else
                MsgBox("Data berhasil disimpan!", vbInformation)
            End If

            TampilData()
            bersihkan()


        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTutup.Click
        Me.Close()
    End Sub

    Private Sub bersihkan()
        txtID.Clear()
        txtNama.Clear()
        txtAlamat.Clear()
        txtTelepon.Clear()
        picFoto1.Image = Nothing
        picFoto1.Invalidate()
    End Sub

    Private Sub btnHapus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHapus.Click
        Try
            If txtID.Text = "" Then
                MsgBox("Pilih data yang akan dihapus!", vbInformation)
                Exit Sub
            End If
            Dim tanya = MsgBox("Yakin ingin menghapus data ini?", vbYesNo + vbQuestion, "Konfirmasi")
            If tanya = vbNo Then Exit Sub
            Call Koneksi()
            Dim sql As String = "DELETE FROM karyawan WHERE id_karyawan=@id"
            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtID.Text)
            Cmd.ExecuteNonQuery()
            MsgBox("Data berhasil dihapus!", vbInformation)
            TampilData()
            bersihkan()
        Catch ex As MySqlException
            ' HANDLE FOREIGN KEY ERROR
            If ex.Number = 1451 Then
                MessageBox.Show("Data tidak bisa dihapus karena sedang digunakan di tabel lain, coba diperiksa lagi!",
                                "Gagal Menghapus", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            Else
                MessageBox.Show("Kesalahan MySQL: " & ex.Message)
            End If
        Catch ex As Exception
            MsgBox("Gagal menghapus data: " & ex.Message)
        End Try
    End Sub

    Private Sub btnBatal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatal.Click
        bersihkan()
    End Sub

    Private Sub Panel1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel1.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = New Color() {
    Color.ForestGreen,
    Color.LightGreen,
    Color.Lime,
    Color.Chartreuse
}

            blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub

    Private Sub picFoto1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles picFoto1.Paint
        Dim borderColor As Color = Color.Black
        Dim borderSize As Integer = 3

        ControlPaint.DrawBorder(e.Graphics, picFoto1.ClientRectangle,
                                borderColor, borderSize, ButtonBorderStyle.Solid,
                                borderColor, borderSize, ButtonBorderStyle.Solid,
                                borderColor, borderSize, ButtonBorderStyle.Solid,
                                borderColor, borderSize, ButtonBorderStyle.Solid)

        If picFoto1.Image Is Nothing Then
            Dim teks As String = "Double-click to choose a photo"
            Dim fontnya As New Font("Palatino Linotype", 8, FontStyle.Bold)
            Dim brushnya As New SolidBrush(Color.Black)

            ' Posisi tengah
            Dim ukuran = e.Graphics.MeasureString(teks, fontnya)
            Dim x = (picFoto1.Width - ukuran.Width) / 2
            Dim y = (picFoto1.Height - ukuran.Height) / 2

            e.Graphics.DrawString(teks, fontnya, brushnya, x, y)
        End If
    End Sub

    Private Function IdKaryawanSudahAda(ByVal id As String) As Boolean
        Dim ada As Boolean = False
        Try
            Conn.Open()
            Cmd = New MySqlCommand("SELECT COUNT(*) FROM karyawan WHERE id_karyawan=@id", Conn)
            Cmd.Parameters.AddWithValue("@id", id)

            Dim count As Integer = Convert.ToInt32(Cmd.ExecuteScalar())
            If count > 0 Then ada = True

        Catch ex As Exception
            MsgBox("Error check ID: " & ex.Message)
        Finally
            Conn.Close()
        End Try

        Return ada
    End Function

    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
Handles btnSimpan.MouseEnter, btnHapus.MouseEnter, btnBatal.MouseEnter, btnTutup.MouseEnter
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Lime
        btn.ForeColor = Color.Black
    End Sub

    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnSimpan.MouseLeave, btnHapus.MouseLeave, btnBatal.MouseLeave, btnTutup.MouseLeave
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Black
        btn.ForeColor = Color.White
    End Sub
End Class