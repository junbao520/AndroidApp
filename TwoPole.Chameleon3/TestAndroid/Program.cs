using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Devices.SensorProviders;
using System.IO;

namespace TestAndroid
{
    class Program
    {

        //测试使用
        static void Main(string[] args)
        {

            byte[] bytedata = System.IO.File.ReadAllBytes("brake.wav");
            string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            File.WriteAllText("breake.txt", strPath);
            //依赖注入其他地方怎么获得这个容器？

            var builder = GetContainerBuilder();
            //这个就是依赖注入吧


           
            builder.RegisterType<A>().As<IA>().WithParameters(
                new List<NamedParameter> {
                new NamedParameter("par", "parA"),
                new NamedParameter("par2", 3.66)
                   }
                ).SingleInstance(); 
            builder.RegisterType<B>().As<IB>().SingleInstance();
            builder.RegisterType<C>().As<IC>().SingleInstance();
            builder.RegisterType<D>().As<ID>().WithParameter(
                new NamedParameter("IsLogEnable", GetContainer().Resolve<IC>())
                ).SingleInstance();





            var container = GetContainer();


                var A = container.Resolve<IA>();
            A.printA();
            A.printA();

            var container1 = GetContainer();

            Task.Run(() => {
                var A1 = container1.Resolve<IA>();
                A1.printA();
                A1.printA();

            });

         

            Console.ReadKey();

        }
        static ContainerBuilder builder = null;
        public static ContainerBuilder GetContainerBuilder()
        {
            if (builder == null)
            {
                builder = new ContainerBuilder();
            }
            return builder;
        }
        static IContainer container = null;
        public static IContainer GetContainer()
        {
            //确保这个代码智能执行一次
            if (container == null)
            {
                container = builder.Build();
            }
            return container;
        }







    }

    public interface IA
    {
        void printA();
    }

    public interface IB
    {
        void printB();
    }

    public interface IC
    {
        bool isLogEnable { get; }
        void printC();

    }
    public interface ID
    {
 
        void printD();
    }
    public class D : ID
    {

        bool isLogEnable = false;
        public D(bool IsLogEnable)
        {
            this.isLogEnable = IsLogEnable;
        }

      

        public void printD()
        {
            Console.WriteLine("Print D");
            Console.WriteLine(isLogEnable.ToString());
        }
    }



    public class C : IC
    {
        public void printC()
        {
            Console.WriteLine("Print C");
        }
        bool IisLogEnable
        {
            get
            {
                return false;
            }

        }

        bool IC.isLogEnable => throw new NotImplementedException();
    }



    public class B : IB
    {
        IC c;
        public B(IC c)
        {
            this.c = c;
        }
        public void printB()
        {
            Console.WriteLine("Print B");
            Console.WriteLine("B");
            c.printC();
        }
    }


    public class A : IA
    {
        IC c;
        IB b;
        static int count = 1;
        string par = string.Empty;
        double par2 = 0;
        ID d;
            
            

        public A(IB b, IC c,string par,double par2,ID d)
        {
            this.c = c;
            this.b = b;
            this.d = d;
            this.par = par;
            this.par2 = par2;
            count++;
        }
        public void printA()
        {
            Console.WriteLine("Print A"+par + count.ToString()+par2.ToString());
            b.printB();
            c.printC();
            d.printD();


        }
    }



}
