using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1;

public partial class MainWindow : Window
{

    ObservableCollection<string> Authors = new();

    string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    public MainWindow()
    {
        InitializeComponent();
        GetDataFromDataBase();

        ListBox1.ItemsSource = Authors;
    }

    private void GetDataFromDataBase()
    {

        Authors.Clear();

        using (SqlConnection connection = new(connectionString))
        {
            SqlDataReader reader = null;

            SqlCommand cmd = new(@"SELECT * FROM Authors", connection);
            connection.Open();
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string author = reader["Id"].ToString() + "." + reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                Authors.Add(author);
            }
        }
    }

    private void AddDataToDataBase()
    {
        using (SqlConnection connection = new(connectionString))
        {
            string insertQuery = @$"INSERT INTO Authors ([Id], [FirstName], [LastName]) VALUES({Authors.Count + 1}, '{txtBoxName.Text}', '{txtBoxLName.Text}')";

            SqlCommand insertCommand = new(insertQuery, connection);

            connection.Open();
            insertCommand.ExecuteNonQuery();
        }
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (txtBoxLName.Text != string.Empty && txtBoxName.Text != string.Empty)
        {
            AddDataToDataBase();
            GetDataFromDataBase();
            txtBoxName.Clear();
            txtBoxLName.Clear();
        }
        else
        {
            if (txtBoxName.Text == string.Empty) txtBoxName.Background = Brushes.Red;
            if (txtBoxLName.Text == string.Empty) txtBoxLName.Background = Brushes.Red;
        }
    }

}