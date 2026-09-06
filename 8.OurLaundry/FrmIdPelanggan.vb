Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D

Public Class FrmIdPelanggan
    Private Sub FrmTransaksi_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        TampilData()
        
    End Sub
   

    Public Sub TampilData()
        Try
            Call Koneksi()

            Dim query As String = "SELECT id_pelanggan, nama FROM pelanggan"
            Da = New MySqlDataAdapter(query, Conn)
            Ds = New DataSet
            Da.Fill(Ds)
            DataGridView1.DataSource = Ds.Tables(0)
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            With DataGridView1
                .MultiSelect = True
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .Columns("id_pelanggan").HeaderText = "ID Pelanggan"
                .Columns("nama").HeaderText = "Nama "
                .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
                .DefaultCellStyle.ForeColor = Color.Black
                .DefaultCellStyle.SelectionBackColor = Color.Black
                .DefaultCellStyle.SelectionForeColor = Color.LightSteelBlue
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.LightSteelBlue
                DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                DataGridView1.EnableHeadersVisualStyles = False
                .ColumnHeadersDefaultCellStyle.Font = New Font("Palatino Linotype", 10, FontStyle.Bold)
                .DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
            End With

        Catch ex As Exception
            MsgBox("Error tampil data: " & ex.Message)
        End Try
    End Sub

    Private Sub FrmIdPelanggan_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            PilihData()
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellDoubleClick
        PilihData()
    End Sub

    Private Sub PilihData()
        'Pastikan tidak klik baris kosong
        If DataGridView1.CurrentRow Is Nothing Then Exit Sub
        If DataGridView1.CurrentRow.Index < 0 Then Exit Sub

        Dim idPelanggan As String = DataGridView1.CurrentRow.Cells("id_pelanggan").Value.ToString()

        'Kirim ke FrmTransaksi
        FrmTransaksi.txtPelanggan.Text = idPelanggan

        'Tutup form
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel1.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = New Color() {
                Color.Lime,
            Color.Cyan,
                Color.Khaki,
            Color.SkyBlue}
            blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub
End Class