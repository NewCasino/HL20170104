 


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
        public DbSet<CFBSubscribe> CFBSubscribe { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class CFBSubscribe
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
        /// 
        /// </summary>  
				[DisplayName("Title")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  Title { get; set; }
		      
       
        
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
        /// 
        /// </summary>  
				[DisplayName("ApplyNumber")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal?  ApplyNumber { get; set; }
		      
       
        
        /// <summary>
        /// 认购数量
        /// </summary>  
				[DisplayName("认购数量")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  SubscribeNumber { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("TotalAmount")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  TotalAmount { get; set; }
		      
       
        
        /// <summary>
        /// 已解冻数量
        /// </summary>  
				[DisplayName("已解冻数量")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal?  CanBeUsed { get; set; }
		      
       
        
        /// <summary>
        /// 单价
        /// </summary>  
				[DisplayName("单价")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  Price { get; set; }
		      
       
        
        /// <summary>
        /// 上次解冻时间
        /// </summary>  
				[DisplayName("上次解冻时间")]
				public DateTime?  LastUsedTime { get; set; }
		      
       
        
        /// <summary>
        /// 解决次数
        /// </summary>  
				[DisplayName("解决次数")]
				public int?  UsedTimes { get; set; }
		      
       
        
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
				[DisplayName("Origin")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  Origin { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public CFBSubscribe()
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
    public interface ICFBSubscribeService :IServiceBase<CFBSubscribe> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(CFBSubscribe entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class CFBSubscribeService :  ServiceBase<CFBSubscribe>,ICFBSubscribeService
    {


        public CFBSubscribeService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(CFBSubscribe entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
 
