using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPFexercise.Model;

namespace WPFexercise.Service
{
    public class UserInfoServices : INotifyPropertyChanged
    {

        private const string connectionString = @"Data Source=localhost;Initial Catalog=Form;Integrated Security=True";


        public UserInfo User { get; set; }

        public int RecordNumber { get; private set; }
        public int TotalRecords { get; private set; }

        public UserInfoServices()
        {
            LoadInitialView();
        }

        private void LoadInitialView()
        {
            RecordNumber = 1;
            TotalRecords = GetTotalRecordFromDb();

            SelectFromDb();
        }

        public void Insert()
        {
            RecordMode = RecordMode.Insert;
            User = new UserInfo();

            NotifyPersonRecordChanged();
        }

        public void Update()
        {
            RecordMode = RecordMode.Update;
        }

        public void Save()
        {
            if (RecordMode == RecordMode.Insert)
            {
                InsertIntoDb();
                TotalRecords++;
                RecordNumber = TotalRecords;
            }
            else if (RecordMode == RecordMode.Update)
                UpdateInDb();
            else
                return;

            StopEditing();
        }

        public void StopEditing()
        {
            if (RecordMode == RecordMode.Insert)
                SelectFromDb();

            RecordMode = RecordMode.View;
        }

        private void UpdateInDb()
        {
            var query = $"UpdateUser";
            //using var connection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@FirstName", User.FirstName);
            command.Parameters.AddWithValue("@LastName", User.LastName);
            command.Parameters.AddWithValue("@Email", User.Email);
            command.Parameters.AddWithValue("@Password", User.Password);

            connection.Open();

            command.ExecuteNonQuery();
        }

        private void InsertIntoDb()
        {
            var query = "InsertUser";

            //using var connection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(query, connection);

            command.CommandType = CommandType.StoredProcedure;

            connection.Open();

            command.Parameters.AddWithValue("@FirstName", User.FirstName);
            command.Parameters.AddWithValue("@LastName", User.LastName);
            command.Parameters.AddWithValue("@Email", User.Email);
            command.Parameters.AddWithValue("@Password", User.Password);

            command.ExecuteNonQuery();
        }

        private static int GetTotalRecordFromDb()
        {
            var query = $"select count(FirstName) from Users";
            //using var connection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();

            using var reader = command.ExecuteReader();

            reader.Read();

            var recordCount = (int)reader[0];
            return recordCount;
        }

        public void SelectFromDb()
        {
            //using var connection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("SelectUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RecordNumber", RecordNumber);

            connection.Open();

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                User = new UserInfo
                {
                    FirstName= reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["Password"].ToString()
                };
            }
            else
                User = new UserInfo();

            NotifyPersonRecordChanged();
        }

        private void NotifyPersonRecordChanged()
        {
            OnPropertyChanged(nameof(User));
            OnPropertyChanged(nameof(RecordNumber));
            OnPropertyChanged(nameof(TotalRecords));
        }

        public void First()
        {
            if (RecordNumber != 1)
            {
                RecordNumber = 1;
                SelectFromDb();
            }
        }

        public void Previous()
        {
            if (RecordNumber > 1)
            {
                RecordNumber--;
                SelectFromDb();
            }
        }

        public void Next()
        {
            if (RecordNumber < TotalRecords)
            {
                RecordNumber++;
                SelectFromDb();
            }
        }

        public void Last()
        {
            if (RecordNumber != TotalRecords)
            {
                RecordNumber = TotalRecords;
                SelectFromDb();
            }
        }

        #region RecordViewState

        private RecordMode _recordMode = RecordMode.View;
        public RecordMode RecordMode
        {
            get
            {
                return _recordMode;
            }
            set
            {
                _recordMode = value;
                OnPropertyChanged(nameof(InputIsReadOnly));
                OnPropertyChanged(nameof(InputIsEditable));
            }
        }

        public bool InputIsReadOnly
        {
            get
            {
                return RecordMode != RecordMode.Insert && RecordMode != RecordMode.Update;
            }
        }

        public bool InputIsEditable => !InputIsReadOnly;

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}

