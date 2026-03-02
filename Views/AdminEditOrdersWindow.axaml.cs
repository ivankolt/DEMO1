using System;
using System.Collections.Generic;
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

public partial class AdminEditOrdersWindow : Window
{
    public int id_orders_ = 0;
    public AdminEditOrdersWindow()
    {
        InitializeComponent();
        this.Title.Text = "Добавление заказа";
        LoadAllBox();

    }
    public AdminEditOrdersWindow(int id_orders)
    {
        InitializeComponent();  
        id_orders_ = id_orders;

        LoadAllBox();
        LoadData(id_orders_);
    }

    public void LoadAllBox()
    {
        using(var db = new DemoContext())
        {
            var type = db.Pickuppoints.ToList();
            foreach(var t in type)
            {
                this.Pickuppoints.Items.Add(t.Name);
            }
            var users = db.Users.Include(x => x.Persons).ToList();
            foreach(var t in users)
            {
                this.Users.Items.Add(new ComboBoxItem()
                {
                    Content = t.Id + " " + t.Persons.FirstName + " " + t.Persons.LastName + " " + t.Persons.MiddleName,
                    Tag = t.Id
                });

            }
            var status = db.OrdersStatuses.ToList();
            foreach(var t in status)
            {
                this.Stauts.Items.Add(t.Name);
            }

            var products = db.Products.ToList();
            foreach(var t in products)
            {
                this.Products.Items.Add(
                    new ComboBoxItem
                    {
                        Tag = t.Articule,
                        Content = t.Articule + " ",
                        

                        
                    }
                );
            }
            
        }
    }
    public void Button_Click1(object s, RoutedEventArgs e)
    {
        this.Close();
    }
    public void Save_Click(object s, RoutedEventArgs e)
    {
        if(this.Title.Text == "Добавление заказа")
        {
            Add_Tovar_();
            this.Close();
        }
        else
        {
            Edit_Order_();
            this.Close();
        }
    }

    public void Edit_Order_()
{
    if(Orders_date.SelectedDate.HasValue && 
       Delivery_date.SelectedDate.HasValue && 
       !string.IsNullOrWhiteSpace(this.Code.Text) && 
       Pickuppoints.SelectedItem is string pickuppoints && 
       Users.SelectedItem is ComboBoxItem users_ && 
       Stauts.SelectedItem is string status_)
    {
        using(var db = new DemoContext())
        {
            try
            {
                
                var existingOrder = db.Orders
                    .Include(x => x.OrdersProducts)
                    .FirstOrDefault(x => x.Id == id_orders_);

                if (existingOrder == null) return;

                var Pickuppoint = db.Pickuppoints.FirstOrDefault(x => x.Name == pickuppoints);
                var status = db.OrdersStatuses.FirstOrDefault(x => x.Name == status_);

              
                existingOrder.OrdersDate = DateOnly.FromDateTime(Orders_date.SelectedDate.Value);
                existingOrder.DeliveryDate = DateOnly.FromDateTime(Delivery_date.SelectedDate.Value);
                existingOrder.PickuppointId = Pickuppoint?.Id;
                existingOrder.Code = Code.Text;
                existingOrder.StatusId = status?.Id;
                existingOrder.UsersId = Convert.ToInt32(users_.Tag);

            
                if (existingOrder.OrdersProducts != null)
                {
                    db.OrdersProducts.RemoveRange(existingOrder.OrdersProducts);
                }

         
                var newProductsList = new List<OrdersProduct>();
                foreach(var child in this.MyProducts.Children)
                {
                    if(child is StackPanel st)
                    {
                        var numericUpDown = st.Children.OfType<NumericUpDown>().FirstOrDefault();   
                        if (numericUpDown != null && numericUpDown.Value.HasValue && numericUpDown.Value.Value > 0)
                        {
                            int qty = Convert.ToInt32(numericUpDown.Value.Value);
                            newProductsList.Add(new OrdersProduct()
                            {
                                OrdersId = existingOrder.Id,
                                ProductsId = Convert.ToString(st.Tag),
                                Qty = qty
                            });
                        }
                    }
                }

                db.OrdersProducts.AddRange(newProductsList);

                db.SaveChanges();
            }
            catch (Exception ex) 
            {
                System.Console.WriteLine("Ошибка изменения: " + ex.InnerException?.Message ?? ex.Message);
                var errorWindow = new Window(){Width = 400, Height= 200 ,Content = new TextBlock(){ Text = "Ошибка при обновлении в БД. Проверьте консоль!", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
                errorWindow.ShowDialog(this);
            }
        }
    }
    else
    {
        var window = new Window(){Width = 350, Height= 200 ,Content = new TextBlock(){ Text = "Заполните все поля заказа и выберите даты!", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
        window.ShowDialog(this);
    } 
}
        public void LoadData(int id_order)
    {
        using(var db = new DemoContext())
        {
           
            var order = db.Orders.FirstOrDefault(x => x.Id == id_order);
            if(order != null)
            {
              
                if (order.OrdersDate.HasValue)
                {
                    Orders_date.SelectedDate = order.OrdersDate.Value.ToDateTime(TimeOnly.MinValue);
                }
                else
                {
                    Orders_date.SelectedDate = null;
                }

                
                if (order.DeliveryDate.HasValue)
                {
                    Delivery_date.SelectedDate = order.DeliveryDate.Value.ToDateTime(TimeOnly.MinValue);
                }
                else
                {
                    Delivery_date.SelectedDate = null;
                }
                Code.Text = order.Code;
                int targetUserId = Convert.ToInt32(order.UsersId);
                Users.SelectedItem = Users.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(x => Convert.ToInt32(x.Tag) == targetUserId);

                
                var pickuppoint = db.Pickuppoints.FirstOrDefault(x => x.Id == order.PickuppointId);
                if (pickuppoint != null) Pickuppoints.SelectedItem = pickuppoint.Name;

                var status = db.OrdersStatuses.FirstOrDefault(x => x.Id == order.StatusId);
                if (status != null) Stauts.SelectedItem = status.Name;

                
                var orderProducts = db.OrdersProducts.Where(x => x.OrdersId == id_order).ToList();

               
                this.MyProducts.Children.Clear();

      
                foreach(var op in orderProducts)
                {
                    
                    var product = db.Products.FirstOrDefault(x => x.Articule == op.ProductsId);
                    if(product != null)
                    {
                        var type_ = db.ProductsTypes.FirstOrDefault(x => x.Id == product.ProductsTypeId);

                  
                        StackPanel stackPanel_ = new StackPanel()
                        {
                            Orientation = Orientation.Horizontal,
                            Tag = product.Articule 
                        };
                        
                        stackPanel_.Children.Add(new TextBlock(){Text = product.Articule, FontWeight = FontWeight.Bold, Margin = new Thickness(5)});
                        stackPanel_.Children.Add(new TextBlock(){Text = " | " + type_?.Name, FontWeight = FontWeight.Bold, Margin = new Thickness(5)});
                        
                      
                        stackPanel_.Children.Add(new NumericUpDown(){ Value = op.Qty, Minimum = 0 });

                        this.MyProducts.Children.Add(stackPanel_);
                    }
                }
            }
        }
    }
    public void Add_Tovar_()
    {
      if(Orders_date.SelectedDate.HasValue && 
       Delivery_date.SelectedDate.HasValue && 
       !string.IsNullOrWhiteSpace(this.Code.Text) && 
       Pickuppoints.SelectedItem is string pickuppoints && 
       Users.SelectedItem is ComboBoxItem users_ && 
       Stauts.SelectedItem is string status_)
        {
            

        using(var db = new DemoContext())
        {
            try
            {
                var Pickuppoint = db.Pickuppoints.FirstOrDefault(x => x.Name == pickuppoints);
                var status = db.OrdersStatuses.FirstOrDefault(x => x.Name == status_);
                var a = new List<OrdersProduct>();
                
                
                foreach(var child in this.MyProducts.Children)
                {
                    if(child is StackPanel st)
                    {
                        var numericUpDown = st.Children.OfType<NumericUpDown>().FirstOrDefault();   
                        
                        if (numericUpDown != null && numericUpDown.Value.HasValue && numericUpDown.Value.Value > 0)
                        {
                            int qty = Convert.ToInt32(numericUpDown.Value.Value);
                            a.Add(new OrdersProduct()
                            {
                                ProductsId = Convert.ToString(st.Tag),
                                Qty = qty,          
                            });
                        }
                    }
                }


                db.Orders.Add(new Order()
                {
                    OrdersDate = DateOnly.FromDateTime(Orders_date.SelectedDate.Value),
                    DeliveryDate = DateOnly.FromDateTime(Delivery_date.SelectedDate.Value),
                    PickuppointId = Pickuppoint?.Id,
                    Code = Code.Text,
                    StatusId = status?.Id,
                    UsersId = Convert.ToInt32(users_.Tag),
                    OrdersProducts = a
                });

                db.SaveChanges();
            }
            catch (Exception ex) 
            {
                System.Console.WriteLine("Ошибка сохранения: " + ex.InnerException?.Message ?? ex.Message);
                
                var errorWindow = new Window(){Width = 400, Height= 200 ,Content = new TextBlock(){ Text = "Ошибка при сохранении в БД. Проверьте консоль!", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
                errorWindow.ShowDialog(this);
            }
        }
    }
    else
    {
        // Подсказка, если не все поля заполнены
        var window = new Window(){Width = 350, Height= 200 ,Content = new TextBlock(){ Text = "Заполните все поля заказа и выберите даты!", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
        window.ShowDialog(this);
    } 
    }
    public void Add_Tovar(object s, RoutedEventArgs e)
{
    if (this.Products.SelectedItem is Avalonia.Controls.Control selectedControl)
    {
        string articule = Convert.ToString(selectedControl.Tag);

        foreach (var child in this.MyProducts.Children)
        {
            if (child is StackPanel sp && Convert.ToString(sp.Tag) == articule)
            {
              
                var numUpDown = sp.Children.OfType<NumericUpDown>().FirstOrDefault();
                if (numUpDown != null)
                {

                    numUpDown.Value = (numUpDown.Value ?? 0) + 1; 
                }
                return; 
            }
        }

        using(var db = new DemoContext())
        {
            var product = db.Products.FirstOrDefault(x => x.Articule == articule);
            if (product == null) return;
            
            var type_ = db.ProductsTypes.FirstOrDefault(x => x.Id == product.ProductsTypeId);
            
            StackPanel stackPanel_ = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Tag = product.Articule 
            };
            
            stackPanel_.Children.Add(new TextBlock(){Text = product.Articule, FontWeight = FontWeight.Bold, Margin = new Thickness(5)});
            stackPanel_.Children.Add(new TextBlock(){Text = " | " + type_?.Name, FontWeight = FontWeight.Bold, Margin = new Thickness(5)});
            
        
            stackPanel_.Children.Add(new NumericUpDown(){ Value = 1, Minimum = 1 });

            this.MyProducts.Children.Add(stackPanel_);
        }
    }
}

    
    
}