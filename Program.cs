using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LINQ_MODE2
{
    public class Program
    {
       
        //1) Write a Query to find out all products starting with letter 'v' 
        public static void Query1()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = dc.Products.Where(p => p.ProductName.StartsWith("X"));
            foreach(var r1 in result)
            {
                Console.WriteLine("Results are {0},{1},{2},{3}",r1.ProductID,r1.ProductName,r1.ReorderLevel,r1.UnitPrice);
            }
        }
        //Aggregration functions are sum,min,max,count,average,aggregate methods
        public static void Query2()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = dc.Products.Sum(p => p.UnitPrice);
            var result1 = dc.Products.Count(p=> p.ReorderLevel>0);
            var result2 = dc.Products.Max(p => p.UnitPrice);
            var result3 = dc.Products.Min(p => p.UnitPrice);
            var result4 = dc.Products.Average(p => p.UnitPrice);
            int[] intNumbers = { 3, 5, 7, 9 };
            int result5 = intNumbers.Aggregate((n1, n2) => n1 * n2);
            Console.WriteLine(result5);
        

        }
        //Restriction operators is a Where method just to filter rows using expression,the filter expression is specified using predicate

        public static void Query3()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = dc.Products.Where(p => p.LicensesInHand>1);
            foreach (var r1 in result)
            {
                Console.WriteLine("Results are{0}" ,r1.LicensesInHand);
            }
        }
        //projection operators are select and selectmany methods
        public static void Query4()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
        var result = dc.Products.Select(p => new { Proname = p.ProductName });
            foreach (var r1 in result)
            {
                Console.WriteLine(r1.Proname);
            }
           

        }
        //Ordering Operators OrderBy,OrderByDescending,ThenBy,ThenByDescending,Reverse methods
        public static void Query5()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = dc.Products.OrderBy(p => p.ProductName);
            var result1 = dc.Products.OrderByDescending(p => p.ProductName);
            var result2 = dc.Products.OrderBy(p => p.ProductName).ThenBy(q=>q.ReorderLevel);
            var result3 = dc.Products.OrderBy(p => p.ProductName).ThenByDescending(q=>q.ReorderLevel);
var result4 = (from news in dc.Products
               select new
               {
         
                   kort = news.ProductName
               }).AsEnumerable().Reverse();
            foreach (var res in result4)
            {
                Console.WriteLine(res.kort);
            }
        
        }
        //Partitoning operators are Take,Skip,TakeWhile,SkipWhile methods
        public static void Query6()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            IEnumerable<string> result = (IEnumerable<string>)dc.Products.Select(p => p.ProductName).Take(3);
            foreach (var v in result)
            {
                Console.WriteLine(v);
            }
            IEnumerable<string> result1 = (IEnumerable<string>)dc.Products.Select(p => p.ProductName).Skip(3);
            foreach (var v1 in result1)
            {
                Console.WriteLine(v1);
            }
          

        }
        //LazyLoading
        public static void Query7()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var productresult = dc.GetTable<Product>();
            var orderresult = dc.GetTable<Order>();
            foreach (var r1 in productresult)
                Console.WriteLine("Result of Products are{0}{1}{2}", r1.ProductName, r1.ReorderLevel, r1.UnitPrice);
            foreach (var r2 in orderresult)
                Console.WriteLine("Results of Orders are{0}{1}{2}", r2.Quantity, r2.ShippingDate, r2.OrderDate);
        }
        //Eager Loading 
        public static void Query8()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = from prod in dc.Products
                         select new { name = prod.ProductName, Orders = prod.Orders };
            foreach (var prod in result)
            {
                Console.WriteLine(prod.name);
                foreach (var ord1 in prod.Orders)
                {
                    Console.WriteLine("\t" + ord1.Quantity + "\t" + ord1.OrderDate + " " + ord1.ShippingDate);
                }
            }
        }
        //Deferred Execution
        public static void Query9()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var prodresult = dc.GetTable<Product>();
            var ordresult = dc.GetTable<Order>();
            foreach (var r1 in prodresult)
                Console.WriteLine(r1.ProductName, r1.UnitPrice);
            foreach (var r2 in ordresult)
                Console.WriteLine("Results {0},{1},{2}", r2.Quantity, r2.ShippingDate, r2.OrderDate);
        }
        //Immediate Execution
        public static void Query10()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var prodresult = (from res in dc.Products
                              where res.ProductName.StartsWith("V")
                              select new { res.UnitPrice, res.ReorderLevel }).ToList();
            foreach (var res1 in prodresult)
                Console.WriteLine("Results {0},{1}", res1.UnitPrice, res1.ReorderLevel);
        }
        //Grouping operators are Groupby,ToLookup method
        public static void Query11()
        {
                             //GroupBy
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = (from c in dc.Products
                          group c by c.ProductID into cgroup
                          orderby cgroup.Key descending
                          select new { Key = cgroup.Key, Products = cgroup.OrderBy(x => x.ProductName) });
            foreach (var r1 in result)
            {
                Console.WriteLine("Key is :{0}", r1.Key);

                foreach (var p in r1.Products)
                {
                    Console.WriteLine("Value is :{0}", p.ProductName);
                }

            }

                            //Tolookup
            var res = dc.Products.ToLookup(e => e.ReorderLevel);

            foreach (var val in res)
            {

                Console.WriteLine("Reorder level: {0}", val.Key);


                foreach (Product e in val)
                {
                    Console.WriteLine(": {0}", e.ProductName);
                }

            }
        }
        //GroupBy method with Multiple Keys
        public static void Query12()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();

            new My_ProductsDataContext();
            var result = dc.Products.GroupBy(x => new { x.ProductID, x.ReorderLevel })
            .OrderByDescending(g => g.Key.ProductID).ThenBy(g => g.Key.ReorderLevel).Select(
            g => new
            {
                productid = g.Key.ProductID,
                reorderlevel = g.Key.ReorderLevel
            });
            foreach (var res1 in result)
            {
                Console.WriteLine(res1.productid, res1.reorderlevel);
            }
        }
        //Element opeartors are ElementAt ,ElementAtOrDefault,First,FirstOrDefault,Last ,LastOrDefault,Single, SingleOrDefault,DefaultIfEmpty
        public static void Query13()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = dc.Products.Select(x => x.ProductName).First();
            Console.WriteLine(result);
            var result1 = dc.Products.Select(x => x.ProductName).FirstOrDefault();
            Console.WriteLine(result1);
            var result2 = dc.Products.Single(x => x.ReorderLevel == 300);
            Console.WriteLine(result2.ReorderLevel);
            var result3 = dc.Products.SingleOrDefault(x => x.ReorderLevel == 250);
            Console.WriteLine(result3.ReorderLevel);
            var result4 = dc.Products.Where(x=>x.ReorderLevel>100).DefaultIfEmpty();
            foreach(var a in result4)
            {
                Console.WriteLine(a.ReorderLevel);
            }
        }
        //Joins operator are inner join,right join,left join,full join,group join(if you want to join 3 tables use this join),cross join  methods
        public static void Query14()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
                     //inner join(join two tables)
            var result = (from prod in dc.Products
                          join ord in dc.Orders on
                          prod.ProductID equals ord.ProductID
                          select new { prodid = prod.ProductID, ordqty = ord.Quantity });
            foreach (var res in result)
                Console.WriteLine(res.prodid, res.ordqty);
            var result1 = dc.Orders.Join(dc.Products, ordid => ordid.ProductID, prodid => prodid.ProductID, (ordid, prodid) => new { qty1 = ordid.Quantity, prod1 = prodid.ProductID }).ToList();
            foreach (var res1 in result1)
                Console.WriteLine(res1.prod1, res1.qty1);
                          //group join(join 3 tables)
            var result2 = (from ord in dc.Orders
                           join prod in dc.Products
                           on ord.ProductID equals
                           prod.ProductID
                           join clnt in dc.CityMasters
                           on ord.ClientID equals clnt.CityID
                           select new
                           {
                               ID = ord.ProductID,
                               qty1 = ord.Quantity,
                               Name1 = clnt.CityName
                           }).ToList();
            foreach (var res in result2)
                Console.WriteLine(res.ID, res.Name1, res.qty1);
                  //group join(join 3 tables)
            var result3 = from prod in dc.Products
                          join ord in dc.Orders
                          on prod.ProductID equals ord.ProductID
                          into DeptOrdGroup
                          select new { prod, DeptOrdGroup };
            foreach (var res in result3)
            {
                Console.WriteLine("Results are {0},{1}", res.prod);

                foreach (var depttest in res.DeptOrdGroup)
                {
                    Console.WriteLine("Results are {0},{1}", depttest.ProductID, depttest.Quantity);
                }
            }
                               //left outer join
            var result4 = from e in dc.Products
                          join d in dc.Orders on e.ProductID equals d.ProductID into eGroup
                          from d in eGroup.DefaultIfEmpty()
                          select new { ProductId = e.ProductID, ProductName = d.ProductID };
            foreach (var v in result4)
            {
                Console.WriteLine(v.ProductId + " " + v.ProductName);
            }
                                     //Cross Join
            var result5 = from d in dc.Orders from e in dc.Products  select new { e, d };
            foreach(var v in result5)
            {
                Console.WriteLine(v.e.ProductID+"\t"+v.d.ProductID);
            }


        }
        //Union,Intersect,Concat,Except methods
        public static void Query15()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = (from p1 in dc.Products select p1.ProductID).Union
 (from o1 in dc.Orders select o1.ProductID);
            foreach(var res1 in result)
            Console.WriteLine(res1);

            var result1 = (from p1 in dc.Products select p1.ProductID).Concat
(from o1 in dc.Orders select o1.ProductID);
            foreach(var res1 in result1)
            Console.WriteLine(res1);


            var result2 = (from p1 in dc.Products select p1.ProductID).Intersect
(from o1 in dc.Orders select o1.ProductID);
            foreach(var res1 in result2)
            Console.WriteLine(res1);

            var result3 = (from p1 in dc.Products select p1.ProductID).Except
(from o1 in dc.Orders select o1.ProductID);
            foreach(var res1 in result3)
            Console.WriteLine(res1);
        }
        //Quantifiers operators are All,Any,Contains methods
        public static void Query16()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            bool result = (from c in dc.Products
                           select c).All(c => c.UnitPrice >= 250);
            bool result1 = (from c in dc.Products
                            select c).Any(c => c.UnitPrice >= 250);
        }
        //Cast opertor are Cast or Convert method
        public static void Query17()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = from c1 in dc.Products
                         select Convert.ToString(c1.ProductID);
            foreach(var r1 in result)
            Console.WriteLine(r1);
        }
        //Conversion Operators are ToList,ToArray,ToDictionary,ToLookup methods
        public static void Query18()
        {
            My_ProductsDataContext dc = new My_ProductsDataContext();
            var result = dc.Products.ToList();
            foreach (var a in result)
                Console.WriteLine(a.ProductName);
            var result1 = dc.Products.ToArray();
            foreach (var a in result1)
                Console.WriteLine(a.ProductID);
            Dictionary<int, string> result2 = dc.Orders.ToDictionary(x => x.OrderID, x => x.ProductID);
            foreach (KeyValuePair<int, string> r in result2)
            {
                Console.WriteLine(r.Key + "\t" + r.Value);
            }
            Dictionary<int, Order> result3 = dc.Orders.ToDictionary(x => x.OrderID);
            foreach (KeyValuePair<int, Order> r in result3)
            {
                Console.WriteLine(r.Key + "\t" + r.Value.ProductID);
            }
            var result4 = dc.Products.ToLookup(x => x.ProductID);
            foreach (var r in result4)
            {
                Console.WriteLine(r.Key);
                foreach(var r1 in result4[r.Key] )
                {
                    Console.WriteLine(r1.ProductID+" "+r1.ProductName);
                }
            }
          

        }
        static void Main(string[] args)
        {
            Query11();
            Console.ReadLine();
        }
    }
}
