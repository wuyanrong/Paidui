using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Models;
using SC.Services;

namespace SC.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ServicecategoryDataModel> list = new List<ServicecategoryDataModel>() 
            {
                new ServicecategoryDataModel(){ Name="test1",Iconpath="ddd//ssss.jpg",Createtime=DateTime.Now},
                new ServicecategoryDataModel(){ Name="test2",Iconpath="ddd//ssss.jpg",Createtime=DateTime.Now},
                new ServicecategoryDataModel(){ Name="test3",Iconpath="ddd//ssss.jpg",Createtime=DateTime.Now},
            };
            foreach (var item in list)
            {
                CategoryService.Instance.Create(item);
            }
        }
    }
}
