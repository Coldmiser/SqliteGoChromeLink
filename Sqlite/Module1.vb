Imports System.Data.SQLite
Imports System.IO

Module Module1
    'https://www.youtube.com/watch?v=MTpq0E8rrMo
    'https://www.codeproject.com/Articles/1210189/Using-SQLite-in-Csharp-VB-Net

    Sub Main()
        Dim CLArgs() As String = Environment.GetCommandLineArgs()

        WriteLog("Database is:  " & CLArgs(1))

        If (ReadDB(CLArgs(1))) Then
            'nothing to do
            'Console.WriteLine("Found")
        Else
            'Adding necessary record
            'Console.WriteLine("Not Found")
            AddToDB(CLArgs(1))
        End If
    End Sub

    Function ReadDB(ByVal DBLocation As String) As Boolean
        ReadDB = 0

        Dim MyConnection As New SQLiteConnection("Data Source=" + DBLocation + ";Version=3")
        MyConnection.Open()

        Dim sqlite_cmd As SQLiteCommand = MyConnection.CreateCommand()
        sqlite_cmd.CommandText = "Select url from urls"

        Dim sqlite_datareader As SQLiteDataReader = sqlite_cmd.ExecuteReader()

        While (sqlite_datareader.Read())
            Dim idReader As Object
            'Dim textreader As String
            idReader = sqlite_datareader.GetValue(0)
            If (String.Compare(idReader, "http://go/") = 0) Then
                ReadDB = 1
                Exit While
            End If

            'Console.WriteLine("[" + idReader + "]")
        End While
    End Function

    Function AddToDB(ByVal DBLocation As String) As Boolean

        Dim MyConnection As New SQLiteConnection("Data Source=" + DBLocation + ";Version=3")
        MyConnection.Open()

        Dim sqlite_cmd As SQLiteCommand = MyConnection.CreateCommand()
        sqlite_cmd.CommandText = "INSERT INTO urls (url, title, visit_count, typed_count, last_visit_time, hidden) VALUES (""http://go/"", ""AbbVie Go"", 1, 1, 13208387573927408, 0);"

        sqlite_cmd.ExecuteNonQuery()


        If (ReadDB(DBLocation)) Then
            'Found, so no problems
            AddToDB = 0
        Else
            'Still not found - Exit with error
            AddToDB = 1
        End If
    End Function

    Function WriteLog(ByVal LogInfo As String) As Boolean
        'Making sure to set the log path at the same path of the exe
        Dim strFile As String = Path.Combine(Directory.GetCurrentDirectory(), "ChromeGo.txt")
        Dim fileExists As Boolean = File.Exists(strFile)
        If (fileExists) Then
            Using sw As New StreamWriter(File.Open(strFile, FileMode.Append))
                Try
                    sw.WriteLine("[" & DateTime.Now & "]  " & LogInfo)
                    WriteLog = 0
                Catch ex As Exception
                    WriteLog = 1
                    Console.WriteLine(ex.Message)
                End Try
            End Using
        Else
            Using sw As New StreamWriter(File.Open(strFile, FileMode.OpenOrCreate))
                Try
                    sw.WriteLine("[" & DateTime.Now & "]  " & LogInfo)
                    WriteLog = 0
                Catch ex As Exception
                    WriteLog = 1
                    Console.WriteLine(ex.Message)
                End Try
            End Using
        End If
    End Function
End Module
