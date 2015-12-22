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

        private SQLiteDataAdapter DBG;
        private DataSet DSG = new DataSet();
        private DataTable DTG = new DataTable();

        public MainWindow()
        {
            InitializeComponent();
            LoadDataListView();
            LoadDataGenreListView();
            RadiobuttonsRead();
            RadiobuttonsOwned();       
        }

        private void SetConnection()
        {
            sql_con = new SQLiteConnection("Data Source=ReadableDB.s3db;Version=3;New=True;");
        }

        private void LoadDataListView()
        {
            try
            {
                SetConnection();
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                string CommandText = "SELECT b.*, a.*, bg.* , g.* FROM books b LEFT JOIN author a ON b.authorId = a.Id LEFT JOIN BookGenre bg on b.GenreId = bg.BookId LEFT JOIN Genre g on bg.GenreId = g.Id";  
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
                    MainListView.Items.Add(new ListViewItem { Content = drow["Title"].ToString() + " - " + drow["Name"].ToString() });
                }
                sql_con.Close();
            }catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private void LoadDataGenreListView()
        {
            try
            {
                SetConnection();
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                string CommandText = "SELECT Genre FROM Genre";
                DBG = new SQLiteDataAdapter(CommandText, sql_con);
                DSG.Reset();
                DBG.Fill(DSG);
                DTG = DSG.Tables[0];

                DataTable dtable = new DataTable();
                GenreListView.Items.Clear();
                for (int i = 0; i < DTG.Rows.Count; i++)
                {
                    DataRow drow = DTG.Rows[i];
                    ListViewItem lvi = new ListViewItem { Content = drow.ToString() };
                    GenreListView.Items.Add(new ListViewItem { Content = drow["Genre"].ToString() });
                }             
                sql_con.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private void RadiobuttonsRead()
        {
            try
            {
                SetConnection();
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = "SELECT count(State) FROM books WHERE State = 'Gelezen'";
                sql_cmd.ExecuteNonQuery();
                Int32 countRead = Convert.ToInt32(sql_cmd.ExecuteScalar());

                sql_cmd.CommandText = "SELECT count(State) FROM books WHERE State = 'Niet gelezen'";
                sql_cmd.ExecuteNonQuery();
                Int32 countNotRead = Convert.ToInt32(sql_cmd.ExecuteScalar());

                sql_cmd.CommandText = "SELECT count(Id) FROM books";
                sql_cmd.ExecuteNonQuery();
                Int32 countAll = Convert.ToInt32(sql_cmd.ExecuteScalar());

                RadioButtonAll.Content = "Alles (" + countAll + ")";
                RadioButtonRead.Content = "Gelezen (" + countRead + ")";
                RadioButtonNotRead.Content = "Niet gelezen (" + countNotRead + ")";

                sql_con.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private void RadioButtonAll_Checked(object sender, RoutedEventArgs e)
        {
            string filterExp = "State = 'Gelezen' OR State = 'Niet gelezen' OR State IS NULL OR State = ''";
            string sortExp = "Title";
            DataRow[] drarray;
            drarray = DT.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            MainListView.Items.Clear();
            for (int i = 0; i < drarray.Length; i++)
            {
                MainListView.Items.Add(drarray[i]["Title"].ToString() + " - " + drarray[i]["Name"].ToString());
            }
        }

        private void RadioButtonRead_Checked(object sender, RoutedEventArgs e)
        {
            string filterExp = "State = 'Gelezen'";
            string sortExp = "Title";
            DataRow[] drarray;
            drarray = DT.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            MainListView.Items.Clear();
            for (int i = 0; i < drarray.Length; i++)
            {              
                MainListView.Items.Add(drarray[i]["Title"].ToString() + " - " + drarray[i]["Name"].ToString());
            }
        }

        private void RadioButtonNotRead_Checked(object sender, RoutedEventArgs e)
        {
            string filterExp = "State = 'Niet gelezen'";
            string sortExp = "Title";
            DataRow[] drarray;
            drarray = DT.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            MainListView.Items.Clear();
            for (int i = 0; i < drarray.Length; i++)
            {
                MainListView.Items.Add(drarray[i]["Title"].ToString() + " - " + drarray[i]["Name"].ToString());
            }
        }

        private void RadiobuttonsOwned()
        {
            try
            {
                SetConnection();
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = "SELECT count(Owned) FROM books WHERE Owned = 'In bezit'";
                sql_cmd.ExecuteNonQuery();
                Int32 countOwned = Convert.ToInt32(sql_cmd.ExecuteScalar());

                sql_cmd.CommandText = "SELECT count(Owned) FROM books WHERE Owned = 'Niet in bezit'";
                sql_cmd.ExecuteNonQuery();
                Int32 countNotOwned = Convert.ToInt32(sql_cmd.ExecuteScalar());

                sql_cmd.CommandText = "SELECT count(Id) FROM books";
                sql_cmd.ExecuteNonQuery();
                Int32 countAll = Convert.ToInt32(sql_cmd.ExecuteScalar());

                RadioButtonAllOwned.Content = "Alles (" + countAll + ")";
                RadioButtonOwned.Content = "In bezit (" + countOwned + ")";
                RadioButtonNotOwned.Content = "Niet in bezit (" + countNotOwned + ")";

                sql_con.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private void RadioButtonAllOwned_Checked(object sender, RoutedEventArgs e)
        {
            string filterExp = "Owned = 'In bezit' OR Owned = 'Niet in bezit' OR Owned IS NULL OR Owned = ''";
            string sortExp = "Title";
            DataRow[] drarray;
            drarray = DT.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            MainListView.Items.Clear();
            for (int i = 0; i < drarray.Length; i++)
            {
                MainListView.Items.Add(drarray[i]["Title"].ToString() + " - " + drarray[i]["Name"].ToString());
            }
        }

        private void RadioButtonOwned_Checked(object sender, RoutedEventArgs e)
        {
            string filterExp = "Owned = 'In bezit'";
            string sortExp = "Title";
            DataRow[] drarray;
            drarray = DT.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            MainListView.Items.Clear();
            for (int i = 0; i < drarray.Length; i++)
            {
                MainListView.Items.Add(drarray[i]["Title"].ToString() + " - " + drarray[i]["Name"].ToString());
            }
        }

        private void RadioButtonNotOwned_Checked(object sender, RoutedEventArgs e)
        {
            string filterExp = "Owned = 'Niet in bezit'";
            string sortExp = "Title";
            DataRow[] drarray;
            drarray = DT.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            MainListView.Items.Clear();
            for (int i = 0; i < drarray.Length; i++)
            {
                MainListView.Items.Add(drarray[i]["Title"].ToString() + " - " + drarray[i]["Name"].ToString());
            }
        }

        //TODO: Publisher toevoegen aan combobox en GenreListView
        private void ComboGenreList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try{
             if (((ComboBoxItem)ComboGenreList.SelectedItem).Content.ToString().Equals("Genre"))
                {
                    SetConnection();
                    sql_con.Open();
                    sql_cmd = sql_con.CreateCommand();
                    string CommandText = "SELECT Genre FROM Genre";
                    DBG = new SQLiteDataAdapter(CommandText, sql_con);
                    DSG.Reset();
                    DBG.Fill(DSG);
                    DTG = DSG.Tables[0];

                    DataTable dtable = new DataTable();
                    GenreListView.Items.Clear();
                    for (int i = 0; i < DTG.Rows.Count; i++)
                    {
                        DataRow drow = DTG.Rows[i];
                        ListViewItem lvi = new ListViewItem { Content = drow.ToString() };
                        GenreListView.Items.Add(new ListViewItem { Content = drow["Genre"].ToString() });
                    }
                }
                else if (((ComboBoxItem)ComboGenreList.SelectedItem).Content.ToString().Equals("Taal"))
                {
                    SetConnection();
                    sql_con.Open();
                    sql_cmd = sql_con.CreateCommand();
                    string CommandText = "SELECT Language FROM books";
                    DBG = new SQLiteDataAdapter(CommandText, sql_con);
                    DSG.Reset();
                    DBG.Fill(DSG);
                    DTG = DSG.Tables[0];

                    //TODO: Counts toevoegen aan Genre en Language

                    //sql_cmd.CommandText = "SELECT count(Distinct Language) FROM books";
                    //sql_cmd.ExecuteNonQuery();
                    //Int32 countLanguage = Convert.ToInt32(sql_cmd.ExecuteScalar());

                    DataTable dtable = new DataTable();
                    GenreListView.Items.Clear();
                    for (int i = 0; i < DTG.Rows.Count; i++)
                    {
                        DataRow drow = DTG.Rows[i];
                        ListViewItem lvi = new ListViewItem { Content = drow.ToString() };
                        GenreListView.Items.Add(new ListViewItem { Content = drow["Language"].ToString() });//+ " ("+ countLanguage+")" 
                    }
                }
                sql_con.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        //TODO: Alles toevoegen aan selectie in GenreListView
        private void GenreListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)ComboGenreList.SelectedItem).Content.ToString().Equals("Genre"))
            {
                string filterExp = "GenreId = BookId AND Genre = '" + ((ListViewItem)GenreListView.SelectedItem).Content.ToString() + "'";
                string sortExp = "Title";
                DataRow[] drarray;
                drarray = DT.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
                MainListView.Items.Clear();
                for (int i = 0; i < drarray.Length; i++)
                {
                    MainListView.Items.Add(drarray[i]["Title"].ToString() + " - " + drarray[i]["Name"].ToString());
                }
            }
            else if (((ComboBoxItem)ComboGenreList.SelectedItem).Content.ToString().Equals("Taal"))
            {
                string filterExp = "Language = '" + ((ListViewItem)GenreListView.SelectedItem).Content.ToString() + "'";
                string sortExp = "Title";
                DataRow[] drarray;
                drarray = DT.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
                MainListView.Items.Clear();
                for (int i = 0; i < drarray.Length; i++)
                {
                    MainListView.Items.Add(drarray[i]["Title"].ToString() + " - " + drarray[i]["Name"].ToString());
                }
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
