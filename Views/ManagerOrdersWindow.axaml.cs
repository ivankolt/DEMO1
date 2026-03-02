using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Den.Models;
using Microsoft.EntityFrameworkCore;

namespace Den.Views;

public partial class ManagerOrdersWindow : Window
{
    public int ID = 0;
    public ManagerOrdersWindow(int id)
    {
        InitializeComponent();
        ID = id;
        LoadFIO(id);
        LoadData();
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

    public void LoadData()
    {
        using(var db = new DemoContext())
        {
            var orders = db.Orders.Include(x => x.Pickuppoint).Include(x => x.OrdersProducts).ToList();
            foreach(var i in orders)
            {        

                var Status = db.OrdersStatuses.FirstOrDefault(x => x.Id == i.StatusId);
                
                Border border = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brush.Parse("#000000"),
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Stretch
                    
                };


                Grid grid = new Grid()
                {
                    ColumnDefinitions = new ColumnDefinitions("Auto, Auto"),
                    VerticalAlignment = VerticalAlignment.Center

                }; 
                

                var main = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brush.Parse("#000000"),
                    Margin = new Thickness(2),
                    Width = 400
                    
                };

                var mainStackPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical
                };

                var Orders_Products = db.OrdersProducts.Where(x=> x.OrdersId == i.Id).ToList();

                string articules = null;
                foreach(var j in Orders_Products)
                {
                    articules += j.ProductsId;
                    articules += " ";
                }

                mainStackPanel.Children.Add(new TextBlock(){Text = "Артикул заказа: " + articules, FontWeight = FontWeight.Bold, FontSize = 20, TextWrapping = TextWrapping.Wrap});
                mainStackPanel.Children.Add(new TextBlock(){Text = "Статус заказа: " + Status?.Name, FontSize = 15});
                mainStackPanel.Children.Add(new TextBlock(){Text = "Адрес пункта выдачи: " + i.Pickuppoint?.Name, FontSize = 15, TextWrapping = TextWrapping.Wrap});
                mainStackPanel.Children.Add(new TextBlock(){Text = "Дата заказа: " + i.OrdersDate, FontSize = 15});

                
                main.Child = mainStackPanel;


                Grid.SetColumn(main,0);
                grid.Children.Add(main);

                var main_discount = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brush.Parse("#000000"),
                    Margin = new Thickness(2),
                    Width = 180
                };

                main_discount.Child = new TextBlock(){Text = "Дата доставки: " + i.DeliveryDate, FontSize = 13, TextWrapping = TextWrapping.Wrap, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center};

                Grid.SetColumn(main_discount,1);
                grid.Children.Add(main_discount);


                border.Child = grid;
                Orders.Children.Add(border);

            }

        }
    }

    

    public void Button_Click1(object s, RoutedEventArgs e)
    {
        ManagerWindow mainWindow = new ManagerWindow(this.ID);
        mainWindow.Show();
        this.Close();
    }
}