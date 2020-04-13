Imports System.Data.Odbc

Public Class Form1
    'inisialisasi untuk koneksi database
    Dim conn As OdbcConnection
    Dim cmd As OdbcCommand
    Dim dataSet As DataSet
    Dim dataAdapter As OdbcDataAdapter
    Dim dataReader As OdbcDataReader
    Dim myDB As String

    Sub koneksi()
        Try
            'driver yang di gunakan untuk koneksi ODBC
            myDB = "Driver={MySQL ODBC 5.1 Driver};Database=db_gudang;Server=Localhost;uid=root"
            conn = New OdbcConnection(myDB)
            If conn.State = ConnectionState.Closed Then conn.Open()
        Catch ex As Exception
            MsgBox("Koneksi Gagal")
        End Try
    End Sub

    'sub kondisi awal form ketika tidak ada aksi
    Sub kondisiAwal()
        lblIDBarang.Text = ""
        txtNama.Text = ""
        txtMerk.Text = ""
        txtJumlah.Text = ""
        txtNama.Enabled = False
        txtMerk.Enabled = False
        txtJumlah.Enabled = False
        btnTambah.Text = "Tambah"
        btnKeluar.Text = "Keluar"
        btnTambah.Enabled = True
        btnAmbilData.Enabled = True
        btnUbah.Enabled = True
        btnHapus.Enabled = True
        btnKeluar.Enabled = True
        Call koneksi()
        dataAdapter = New OdbcDataAdapter("Select * from tb_barang", conn)
        dataSet = New DataSet
        dataAdapter.Fill(dataSet, "tb_barang")
        DataGridView1.DataSource = dataSet.Tables("tb_barang")
    End Sub

    Sub textAreaAktif()
        txtNama.Enabled = True
        txtMerk.Enabled = True
        txtJumlah.Enabled = True
        txtNama.Focus()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call kondisiAwal()
    End Sub

    'pengujian inputan untuk form jumlah agar hanya dapat memasukkan inputan angka
    Private Sub txtJumlah_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJumlah.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled = True
    End Sub

    'proses untuk tombol tambah
    Private Sub btnTambah_Click(sender As Object, e As EventArgs) Handles btnTambah.Click
        If btnTambah.Text = "Tambah" Then
            btnTambah.Text = "Simpan"
            btnUbah.Enabled = False
            btnHapus.Enabled = False
            btnAmbilData.Enabled = False
            btnKeluar.Text = "Batal"
            Call textAreaAktif()
        Else
            If txtNama.Text = "" Or txtMerk.Text = "" Or txtJumlah.Text = "" Then
                MsgBox("Pastikan Jangan Ada Yang Kosong", MessageBoxIcon.Exclamation, "Pesan")
            Else
                btnKeluar.Text = "Batal"
                Call koneksi()
                Dim tambahBarang As String = "Insert into tb_barang values(id_barang, '" & txtNama.Text &
                    "', '" & txtMerk.Text & "', '" & txtJumlah.Text & "')"
                cmd = New OdbcCommand(tambahBarang, conn)
                cmd.ExecuteNonQuery()
                MsgBox("Barang Ditambahkan", MessageBoxIcon.Information, "Pesan")
                Call kondisiAwal()
            End If
        End If
    End Sub

    'proses untuk tombol ambil data
    Private Sub btnAmbilData_Click(sender As Object, e As EventArgs) Handles btnAmbilData.Click
        Dim i As Integer = DataGridView1.CurrentRow.Index
        lblIDBarang.Text = DataGridView1(0, i).Value
        txtNama.Text = DataGridView1.Item(1, i).Value
        txtMerk.Text = DataGridView1.Item(2, i).Value
        txtJumlah.Text = DataGridView1.Item(3, i).Value
        btnTambah.Enabled = False
        btnKeluar.Text = "Batal"
        btnKeluar.Enabled = True
        Call textAreaAktif()
    End Sub

    'proses untuk tombol edit
    Private Sub btnUbah_Click(sender As Object, e As EventArgs) Handles btnUbah.Click
        If txtNama.Text = "" Or txtMerk.Text = "" Or txtJumlah.Text = "" Then
            MsgBox("Data Kosong", MessageBoxIcon.Exclamation, "Pesan")
        Else
            Call koneksi()
            Dim ubahBarang As String = "update tb_barang set nama_barang = '" & txtNama.Text &
                "', merk_barang = '" & txtMerk.Text & "', jumlah_barang = '" & txtJumlah.Text &
                "' where id_barang = '" & lblIDBarang.Text & "'"
            cmd = New OdbcCommand(ubahBarang, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Barang Diubah", MessageBoxIcon.Information, "Pesan")
            Call kondisiAwal()
        End If
    End Sub

    'proses untuk tombol hapus
    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnHapus.Click
        If txtNama.Text = "" Or txtMerk.Text = "" Or txtJumlah.Text = "" Then
            MsgBox("Data Kosong", MessageBoxIcon.Exclamation, "Pesan")
        Else
            Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or
            MsgBoxStyle.Critical
            Dim response = MsgBox("Yakin ingin menghapus data ini?", style, "Peringatan")

            If response = MsgBoxResult.Yes Then
                Dim hapusBarang As String = "DELETE FROM tb_barang WHERE id_barang = '" & lblIDBarang.Text & "'"
                cmd = New OdbcCommand(hapusBarang, conn)
                cmd.ExecuteNonQuery()
                MsgBox("Barang Dihapus", MessageBoxIcon.Information, "Pesan")
                Call kondisiAwal()
            Else
                Call kondisiAwal()
            End If
        End If
    End Sub

    'proses untuk tombol keluar
    Private Sub btnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        If btnKeluar.Text = "Keluar" Then
            Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or
            MsgBoxStyle.Critical
            Dim response = MsgBox("Yakin ingin keluar?", style, "Peringatan")

            If response = MsgBoxResult.Yes Then
                End
            End If
        Else
            Call kondisiAwal()
        End If
    End Sub
End Class