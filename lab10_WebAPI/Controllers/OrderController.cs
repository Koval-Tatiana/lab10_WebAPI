using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using lab10_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;

namespace lab10_WebAPI.Controllers
{
    public class OrderController : ApiController
    {
        [HttpPost]
        public void Add(Order order) 
        {
            //получаем заказы из файла
            ICollection<Order> orders = GetAll();
            //если коллекция пустая, создаем
            if (orders == null)
                orders= new Collection<Order>();

            order.OrderId = orders.Count+1; //устанавливаем id: прибавляем к размеру коллекции 1
            
            orders.Add(order);
            writeToFile(orders);
        }

        [HttpGet]
        public ICollection<Order> GetAll()
        {
            ICollection<Order> orders;
            var pathToStorage = HttpContext.Current.Server.MapPath("~/App_Data/MyOrdersStorage.json");
            using (var sr = new StreamReader(pathToStorage))
            {
                var json = sr.ReadToEnd();
                //конверируем из JSON в коллекцию
                orders = JsonConvert.DeserializeObject<ICollection<Order>>(json);
            }
            return orders; 
        }

        [HttpPost]
        public void Delete(int id)
        {
            ICollection<Order> orders = GetAll();
            //находим в коллекции по id наш заказ
            var order = orders.First(o => o.OrderId == id);
          
            if (order != null)
            {    
                orders.Remove(order);   //удаление
                writeToFile(orders);    //перезаписываем JSON
            }
        }

        public void writeToFile(ICollection<Order> orders)
        {
            var pathToStorage = HttpContext.Current.Server.MapPath("~/App_Data/MyOrdersStorage.json");
            using (var sr = new StreamWriter(pathToStorage))
            {
                //конверируем коллекцию в JSON
                var json = JsonConvert.SerializeObject(orders);
                sr.Write(json);
            }
        }
    }
}