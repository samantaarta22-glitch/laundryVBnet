Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class FrmCustomer
    Private ApplyGradient As Boolean = False
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


    Private Sub FrmDashboard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tampilData()
        TampilData2()
        Dim radius As Integer = 20 ' Semakin besar, semakin melengkung
        Me.Region = New Region(RoundedRect(New Rectangle(0, 0, Me.Width, Me.Height), radius))
        Panel2.Cursor = Cursors.NoMove2D
        btnSimpan.Cursor = Cursors.Hand
        btnHapus.Cursor = Cursors.Hand
        btnBatal.Cursor = Cursors.Hand
        btnTutup.Cursor = Cursors.Hand
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
    Sub tampilData()
        Try
            Call Koneksi()
            Da = New MySqlDataAdapter("SELECT * FROM pelanggan", Conn)
            Ds = New DataSet()
            Da.Fill(Ds, "pelanggan")
            DataGridView1.DataSource = Ds.Tables("pelanggan")
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            With DataGridView1
                .Columns("id_pelanggan").HeaderText = "ID Customer"
                .Columns("nama").HeaderText = "Nama"
                .Columns("alamat").HeaderText = "Alamat"
                .Columns("no_hp").HeaderText = "No. Telepon"               
                .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
                .DefaultCellStyle.SelectionBackColor = Color.Black
                .DefaultCellStyle.SelectionForeColor = Color.LimeGreen
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.LimeGreen
                DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                DataGridView1.EnableHeadersVisualStyles = False
                .ColumnHeadersDefaultCellStyle.Font = New Font("Palatino Linotype", 10, FontStyle.Bold)
                .DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
            End With
        Catch ex As Exception
            MessageBox.Show("Gagal menampilkan data: " & ex.Message)
        Finally
            Conn.Close()
        End Try
    End Sub

    Sub TampilData2(Optional ByVal keyword As String = "")
        Try
            Call Koneksi()

            Dim sql As String = "SELECT * FROM pelanggan"

            If keyword <> "" Then
                sql &= " WHERE id_pelanggan LIKE @key OR nama LIKE @key OR no_hp LIKE @key"
            End If

            Da = New MySqlDataAdapter(sql, Conn)

            If keyword <> "" Then
                Da.SelectCommand.Parameters.AddWithValue("@key", "%" & keyword & "%")
            End If

            Ds = New DataSet
            Da.Fill(Ds, "pelanggan")
            DataGridView1.DataSource = Ds.Tables("pelanggan")

        Catch ex As Exception
            MsgBox("Gagal tampil data: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub txtCari_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCari.TextChanged
        TampilData2(txtCari.Text)
    End Sub


    Private Sub Panel1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel1.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = New Color() {
                Color.DarkGreen,
                Color.MediumSeaGreen,
                Color.LightGoldenrodYellow,
                Color.LimeGreen}


            blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub

    Private Sub btnTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTutup.Click
        Me.Close()
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            If e.RowIndex >= 0 Then
                Dim baris As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                txtId.Text = baris.Cells("id_pelanggan").Value.ToString()
                txtNama.Text = baris.Cells("nama").Value.ToString()
                txtAlamat.Text = baris.Cells("alamat").Value.ToString()
                txtNo.Text = baris.Cells("no_hp").Value.ToString()
            End If

        Catch ex As Exception
            MsgBox("Gagal memuat data baris: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnSimpan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSimpan.Click
        ' Validasi input
        If txtId.Text = "" Or txtNama.Text = "" Or txtAlamat.Text = "" Or txtNo.Text = "" Then
            MessageBox.Show("Data belum lengkap!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Try
            Call Koneksi()

            ' Cek apakah id_pelanggan sudah ada
            Dim cmdCheck As New MySqlCommand("SELECT COUNT(*) FROM pelanggan WHERE id_pelanggan=@id", Conn)
            cmdCheck.Parameters.AddWithValue("@id", txtID.Text)
            Dim count As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

            Dim sql As String
            Dim pesan As String

            If count > 0 Then
                ' UPDATE jika sudah ada
                sql = "UPDATE pelanggan SET nama=@nama, alamat=@alamat, no_hp=@nohp WHERE id_pelanggan=@id"
                pesan = "Data berhasil diupdate!"
            Else
                ' INSERT jika belum ada
                sql = "INSERT INTO pelanggan (id_pelanggan, nama, alamat, no_hp) VALUES (@id, @nama, @alamat, @nohp)"
                pesan = "Data berhasil disimpan!"
            End If

            Using cmd As New MySqlCommand(sql, Conn)
                cmd.Parameters.AddWithValue("@id", txtID.Text)
                cmd.Parameters.AddWithValue("@nama", txtNama.Text)
                cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text)
                cmd.Parameters.AddWithValue("@nohp", txtNo.Text)

                cmd.ExecuteNonQuery()
            End Using

            ' Pesan hanya MUNCUL SEKALI
            MessageBox.Show(pesan, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Refresh DataGridView
            tampilData()

            ' Clear input
            txtID.Clear()
            txtNama.Clear()
            txtAlamat.Clear()
            txtNo.Clear()

        Catch ex As Exception
            MessageBox.Show("Gagal menyimpan data: " & ex.Message)
        Finally
            Conn.Close()
        End Try
    End Sub


    Private Sub btnHapus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHapus.Click
        ' Validasi dulu
        If txtID.Text = "" Then
            MessageBox.Show("Pilih data yang ingin dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' Konfirmasi sebelum hapus
        If MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Call Koneksi()
                Dim sql As String = "DELETE FROM pelanggan WHERE id_pelanggan=@id"
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand(sql, Conn)
                cmd.Parameters.AddWithValue("@id", txtID.Text)
                cmd.ExecuteNonQuery()

                MessageBox.Show("Data berhasil dihapus!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Refresh DataGridView
                tampilData()

                ' Clear input
                txtID.Clear()
                txtNama.Clear()
                txtAlamat.Clear()
                txtNo.Clear()

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
                MessageBox.Show("Gagal menghapus data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Conn.Close()
            End Try
        End If
    End Sub

    Private Sub btnBatal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatal.Click
        txtId.Clear()
        txtNama.Clear()
        txtAlamat.Clear()
        txtNo.Clear()
    End Sub

    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
   Handles btnSimpan.MouseEnter, btnHapus.MouseEnter, btnBatal.MouseEnter, btnTutup.MouseEnter
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.MediumSeaGreen
        btn.ForeColor = Color.Black
    End Sub

    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnSimpan.MouseLeave, btnHapus.MouseLeave, btnBatal.MouseLeave, btnTutup.MouseLeave
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Black
        btn.ForeColor = Color.White
    End Sub
End Class