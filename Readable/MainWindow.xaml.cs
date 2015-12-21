using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Readable
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private SQLiteDataAdapter DB;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();

           // string cmdstring = @"CREATE TABLE books (Id INTEGER NOT NULL, Title TEXT, Description TEXT, Pages INTEGER, PRIMARY KEY (Id))";
           // string cmdstring = @"INSERT INTO books (Title, Description, Pages) VALUES ('Test book', 'Description test', 200);";
           // SQLiteCommand mycommand = new SQLiteCommand(cmdstring, cnn);
           // mycommand.ExecuteNonQuery();        
        }

        private void SetConnection()
        {
            sql_con = new SQLiteConnection("Data Source=ReadableDB.s3db;Version=3;New=True;");
        }

        void LoadData()
        {
            try
            {
                SetConnection();
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                string CommandText = "SELECT Title FROM books";
                DB = new SQLiteDataAdapter(CommandText, sql_con);
                DS.Reset();
                DB.Fill(DS);
                DT = DS.Tables[0];

                DataTable dtable = new DataTable();
                MainListView.Items.Clear();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DataRow drow = DT.Rows[i];
                    ListViewItem lvi = new ListViewItem { Content = drow.ToString() };
                    MainListView.Items.Add(new ListViewItem { Content = drow["Title"].ToString() });
                }
                sql_con.Close();
            }catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

        }

        //private void ExecuteQuery(string txtQuery)
        //{
        //    SetConnection();
        //    sql_con.Open();
        //    sql_cmd = sql_con.CreateCommand();
        //    sql_cmd.CommandText = txtQuery;
        //    sql_cmd.ExecuteNonQuery();
        //    sql_con.Close();
        //}

        //private void Add()
        //{
        //    string txtSQLQuery = "insert into  mains (desc) values ('" + txtDesc.Text + "')";
        //    ExecuteQuery(txtSQLQuery);
        //}

    }
}
