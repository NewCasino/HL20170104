 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MvcCore;
using System.Data.Entity;
using System.ComponentModel;
using MvcCore.Infrastructure;
using System.Data.Entity.Validation;
namespace JN.Data
{
    public partial class SysDbContext : FrameworkContext
    {
        /// <summary>
        /// 把实体添加到EF上下文
        /// </summary>
        public DbSet<StockChartDay> StockChartDay { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class StockChartDay
    {

		
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ID")]
				[Key]
		public int  ID { get; set; }
		      
       
        
        /// <summary>
        /// 开盘价
        /// </summary>  
				[DisplayName("开盘价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  OpenPrice { get; set; }
		      
       
        
        /// <summary>
        /// 收盘价
        /// </summary>  
				[DisplayName("收盘价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  ClosePrice { get; set; }
		      
       
        
        /// <summary>
        /// 最高价
        /// </summary>  
				[DisplayName("最高价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  HightPrice { get; set; }
		      
       
        
        /// <summary>
        /// 最低价
        /// </summary>  
				[DisplayName("最低价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  LowPrice { get; set; }
		      
       
        
        /// <summary>
        /// 成交量
        /// </summary>  
				[DisplayName("成交量")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  Volume { get; set; }
		      
       
        
        /// <summary>
        /// 成交额
        /// </summary>  
				[DisplayName("成交额")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  Turnover { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("StockDate")]
				public DateTime  StockDate { get; set; }
		      
       
        
        /// <summary>
        /// 涨跌幅
        /// </summary>  
				[DisplayName("涨跌幅")]
				public double  UpsAndDownsScale { get; set; }
		      
       
        
        /// <summary>
        /// 涨跌额
        /// </summary>  
				[DisplayName("涨跌额")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  UpsAndDownsPrice { get; set; }
		      
       
        
        /// <summary>
        /// 昨日收盘价
        /// </summary>  
				[DisplayName("昨日收盘价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  YesterdayClosePrice { get; set; }
		      
       
        
        /// <summary>
        /// 5日均线
        /// </summary>  
				[DisplayName("5日均线")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  MA5 { get; set; }
		      
       
        
        /// <summary>
        /// 10日均线
        /// </summary>  
				[DisplayName("10日均线")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  MA10 { get; set; }
		      
       
        
        /// <summary>
        /// 30日均线
        /// </summary>  
				[DisplayName("30日均线")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  MA30 { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("CreateTime")]
				public DateTime  CreateTime { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("TotalStock")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  TotalStock { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public StockChartDay()
        {
        //    ID = Guid.NewGuid();
        }
      
    }
 
    
}
namespace JN.Data.Service
{
    /// <summary>
    /// 业务接口
    /// </summary>
    public interface IStockChartDayService :IServiceBase<StockChartDay> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(StockChartDay entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class StockChartDayService :  ServiceBase<StockChartDay>,IStockChartDayService
    {


        public StockChartDayService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(StockChartDay entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
