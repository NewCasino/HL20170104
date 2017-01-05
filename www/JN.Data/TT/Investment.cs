 


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
        public DbSet<Investment> Investment { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class Investment
    {

		
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ID")]
				[Key]
		public int  ID { get; set; }
		      
       
        
        /// <summary>
        /// 投资交易编码
        /// </summary>  
				[DisplayName("投资交易编码")]
		        [MaxLength(50,ErrorMessage="投资交易编码最大长度为50")]
		public string  InvestmentNo { get; set; }
		      
       
        
        /// <summary>
        /// 投资类型,1首次投资,2复投
        /// </summary>  
				[DisplayName("投资类型,1首次投资,2追加复投,3其他复投")]
				public int  InvestmentType { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("UID")]
				public int  UID { get; set; }
		      
       
        
        /// <summary>
        /// 标题
        /// </summary>  
				[DisplayName("标题")]
		        [MaxLength(50,ErrorMessage="标题最大长度为50")]
		public string  UserName { get; set; }
		      
       
        
        /// <summary>
        /// 申请投资额
        /// </summary>  
				[DisplayName("申请投资额")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  ApplyInvestment { get; set; }
		      
       
        
        /// <summary>
        /// 拥有额
        /// </summary>  
				[DisplayName("拥有额")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  HaveMoney { get; set; }
		      
       
        
        /// <summary>
        /// 利息金额
        /// </summary>  
				[DisplayName("利息金额")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  SettlementMoney { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("IsBalance")]
				public bool  IsBalance { get; set; }
		      
       
        
        /// <summary>
        /// 单价
        /// </summary>  
				[DisplayName("单价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  Period { get; set; }
		      
       
        
        /// <summary>
        /// 上次解冻时间
        /// </summary>  
				[DisplayName("上次解冻时间")]
				public DateTime?  LastBalanceTime { get; set; }
		      
       
        
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
        /// 
        /// </summary>  
				[DisplayName("PayName")]
		        [MaxLength(20,ErrorMessage="最大长度为20")]
		public string  PayName { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("PayNumber")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  PayNumber { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("PayBank")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  PayBank { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public Investment()
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
    public interface IInvestmentService :IServiceBase<Investment> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(Investment entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class InvestmentService :  ServiceBase<Investment>,IInvestmentService
    {


        public InvestmentService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(Investment entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
 
