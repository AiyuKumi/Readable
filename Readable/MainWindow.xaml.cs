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
        public MainWindow()
        {
            try
            {
            InitializeComponent();

            SQLiteConnection cnn = new SQLiteConnection("Data Source=ReadableDB.s3db;Version=3;New=True;");
            cnn.Open();

           // string cmdstring = @"CREATE TABLE books (Id INTEGER NOT NULL, Title TEXT, Description TEXT, Pages INTEGER, PRIMARY KEY (Id))";

           // string cmdstring = @"INSERT INTO books (Title, Description, Pages) VALUES ('Test book', 'Description test', 200);";

           // SQLiteCommand mycommand = new SQLiteCommand(cmdstring, cnn);
           // mycommand.ExecuteNonQuery();

            SQLiteDataAdapter DB;
            DataSet DS = new DataSet();
            DataTable dt = new DataTable();

            DB = new SQLiteDataAdapter("SELECT Title FROM books", cnn);
            DS.Reset();
            DB.Fill(DS);
            dt = DS.Tables[0];

            DataTable dtable = new DataTable();
            MainListView.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drow = dt.Rows[i];
                ListViewItem lvi = new ListViewItem { Content = drow.ToString()};
                MainListView.Items.Add(new ListViewItem { Content = drow["Title"].ToString() });
            }
          
            cnn.Close();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message.ToString());
            }

        }


    }
}
