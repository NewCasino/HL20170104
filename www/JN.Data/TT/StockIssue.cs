 


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
        public DbSet<StockIssue> StockIssue { get; set; }
    }

	/// <summary>
    /// 股权发行表
    /// </summary>
	[DisplayName("股权发行表")]
    public partial class StockIssue
    {

		
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ID")]
				[Key]
		public int  ID { get; set; }
		      
       
        
        /// <summary>
        /// 期数
        /// </summary>  
				[DisplayName("期数")]
				public int  Period { get; set; }
		      
       
        
        /// <summary>
        /// 标题
        /// </summary>  
				[DisplayName("标题")]
		        [MaxLength(50,ErrorMessage="标题最大长度为50")]
		public string  Title { get; set; }
		      
       
        
        /// <summary>
        /// 股权总量
        /// </summary>  
				[DisplayName("股权总量")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  TotalStock { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("HaveSubscribe")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  HaveSubscribe { get; set; }
		      
       
        
        /// <summary>
        /// 单价
        /// </summary>  
				[DisplayName("单价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  Price { get; set; }
		      
       
        
        /// <summary>
        /// 发行开始时间
        /// </summary>  
				[DisplayName("发行开始时间")]
				public DateTime?  StartTime { get; set; }
		      
       
        
        /// <summary>
        /// 发行条件说明
        /// </summary>  
				[DisplayName("发行条件说明")]
		        [MaxLength(50,ErrorMessage="发行条件说明最大长度为50")]
		public string  StartCondition { get; set; }
		      
       
        
        /// <summary>
        /// 发行结束时间
        /// </summary>  
				[DisplayName("发行结束时间")]
				public DateTime?  EndTime { get; set; }
		      
       
        
        /// <summary>
        /// 自由交易最低单价
        /// </summary>  
				[DisplayName("自由交易最低单价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  TradeMinPrice { get; set; }
		      
       
        
        /// <summary>
        /// 自由交易最高单价
        /// </summary>  
				[DisplayName("自由交易最高单价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal?  TradeMaxPrice { get; set; }
		      
       
        
        /// <summary>
        /// 会员认购最大数量
        /// </summary>  
				[DisplayName("会员认购最大数量")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal?  MaxBuy { get; set; }
		      
       
        
        /// <summary>
        /// 状态(0未开始,1进行中,2已结束)
        /// </summary>  
				[DisplayName("状态(0未开始,1进行中,2已结束)")]
				public int  Status { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("Remark")]
		        [MaxLength(250,ErrorMessage="最大长度为250")]
		public string  Remark { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("CreateTime")]
				public DateTime  CreateTime { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public StockIssue()
        {
        //    ID = Guid.NewGuid();
        }
      
    }
 
    
}
namespace JN.Data.Service
{
    /// <summary>
    /// 股权发行表业务接口
    /// </summary>
    public interface IStockIssueService :IServiceBase<StockIssue> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(StockIssue entity);
	}
    /// <summary>
    /// 股权发行表业务类
    /// </summary>
    public class StockIssueService :  ServiceBase<StockIssue>,IStockIssueService
    {


        public StockIssueService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(StockIssue entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
