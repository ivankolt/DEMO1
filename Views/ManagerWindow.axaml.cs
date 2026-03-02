using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Den.Models;
using Microsoft.EntityFrameworkCore;


namespace Den.Views;

public partial class ManagerWindow : Window
{
    public int ID = 0;
    public ManagerWindow(int id)
    {
        ID = id;
        InitializeComponent();
        LoadFIO(id);
    }
    public void Button_Click(object s, RoutedEventArgs e)
    {
        MainWindow gusetWindow = new MainWindow();
        gusetWindow.Show();
        this.Close();
    }
    public void LoadFIO(int id)
    {
        try
        {
        using(var db = new DemoContext())
        {
            var persons = db.Users.Include(x => x.Persons).FirstOrDefault(x => x.Id == id);

            string FIO1 = persons.Persons?.FirstName + " " + persons.Persons?.LastName + " " + persons.Persons?.MiddleName + " ";

            this.FIO.Text = FIO1;
        }
        }
        catch
        {
            System.Console.WriteLine("ошибка");
        }
    }
    public void Button_Click_1(object s, RoutedEventArgs e)
    {
        var  managerOrdersWindow = new ManagerOrdersWindow(ID);
        managerOrdersWindow.Show();
        this.Close();
    }
    public void Button_Click_2(object s, RoutedEventArgs e)
    {
       var  managerOrdersWindow = new ManagerProductsWindow(ID);
       managerOrdersWindow.Show();
       this.Close();
    }
}