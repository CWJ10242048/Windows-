using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 周黑鸭工厂
{
    // 定义生产鸭脖和鸭翅的接口
    public interface IProductionFactory
    {
        void ProduceDuckNeck();
        void ProduceDuckWing();
    }

    // 武汉工厂，能生产鸭脖和鸭翅
    public class WuhanFactory : IProductionFactory
    {
        public void ProduceDuckNeck()
        {
            Console.WriteLine("武汉工厂生产鸭脖");
        }

        public void ProduceDuckWing()
        {
            Console.WriteLine("武汉工厂生产鸭翅");
        }
    }

    // 南京工厂，只能生产鸭翅
    public class NanjingFactory : IProductionFactory
    {
        public void ProduceDuckNeck()
        {
            throw new NotImplementedException("南京工厂不能生产鸭脖");
        }

        public void ProduceDuckWing()
        {
            Console.WriteLine("南京工厂生产鸭翅");
        }
    }

    // 长沙工厂，只能生产鸭脖
    public class ChangshaFactory : IProductionFactory
    {
        public void ProduceDuckNeck()
        {
            Console.WriteLine("长沙工厂生产鸭脖");
        }

        public void ProduceDuckWing()
        {
            throw new NotImplementedException("长沙工厂不能生产鸭翅");
        }
    }

    // 生产委托
    public delegate void ProductionDelegate();

    class Program
    {
        static void Main(string[] args)
        {
            // 创建工厂实例
            WuhanFactory wuhan = new WuhanFactory();
            NanjingFactory nanjing = new NanjingFactory();
            ChangshaFactory changsha = new ChangshaFactory();

            // 使用委托进行生产
            ProductionDelegate wuhanProduction = new ProductionDelegate(wuhan.ProduceDuckNeck);
            wuhanProduction += wuhan.ProduceDuckWing;

            ProductionDelegate nanjingProduction = new ProductionDelegate(nanjing.ProduceDuckWing);

            ProductionDelegate changshaProduction = new ProductionDelegate(changsha.ProduceDuckNeck);

            // 异常处理
            try
            {
                wuhanProduction.Invoke();
                nanjingProduction.Invoke();
                changsha.ProduceDuckNeck();
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine($"生产异常: {e.Message}");
            }
            Console.ReadLine();
        }
    }
}
