﻿ 


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
        public DbSet<BonusDetail> BonusDetail { get; set; }
    }

	/// <summary>
    /// 奖金明细表
    /// </summary>
	[DisplayName("奖金明细表")]
    public partial class BonusDetail
    {

		
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ID")]
				[Key]
		public long  ID { get; set; }
		      
       
        
        /// <summary>
        /// 用户ID
        /// </summary>  
				[DisplayName("用户ID")]
				public int  UID { get; set; }
		      
       
        
        /// <summary>
        /// 用户名
        /// </summary>  
				[DisplayName("用户名")]
		        [MaxLength(50,ErrorMessage="用户名最大长度为50")]
		public string  UserName { get; set; }
		      
       
        
        /// <summary>
        /// 奖金ID
        /// </summary>  
				[DisplayName("奖金ID")]
				public int  BonusID { get; set; }
		      
       
        
        /// <summary>
        /// 奖金名称
        /// </summary>  
				[DisplayName("奖金名称")]
		        [MaxLength(50,ErrorMessage="奖金名称最大长度为50")]
		public string  BonusName { get; set; }
		      
       
        
        /// <summary>
        /// 奖金金额
        /// </summary>  
				[DisplayName("奖金金额")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  BonusMoney { get; set; }
		      
       
        
        /// <summary>
        /// 描述
        /// </summary>  
				[DisplayName("描述")]
		        [MaxLength(250,ErrorMessage="描述最大长度为250")]
		public string  Description { get; set; }
		      
       
        
        /// <summary>
        /// 是否结算
        /// </summary>  
				[DisplayName("是否结算")]
				public bool  IsBalance { get; set; }
		      
       
        
        /// <summary>
        /// 结算时间
        /// </summary>  
				[DisplayName("结算时间")]
				public DateTime  BalanceTime { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("CreateTime")]
				public DateTime  CreateTime { get; set; }
		      
       
        
        /// <summary>
        /// 理财奖金
        /// </summary>  
				[DisplayName("理财奖金")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  BonusMoneyCF { get; set; }
		      
       
        
        /// <summary>
        /// 理财奖金可路转余额
        /// </summary>  
				[DisplayName("理财奖金可路转余额")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  BonusMoneyCFBA { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("Period")]
				public int  Period { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public BonusDetail()
        {
        //    ID = Guid.NewGuid();
        }
      
    }
 
    
}
namespace JN.Data.Service
{
    /// <summary>
    /// 奖金明细表业务接口
    /// </summary>
    public interface IBonusDetailService :IServiceBase<BonusDetail> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(BonusDetail entity);
	}
    /// <summary>
    /// 奖金明细表业务类
    /// </summary>
    public class BonusDetailService :  ServiceBase<BonusDetail>,IBonusDetailService
    {


        public BonusDetailService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(BonusDetail entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
 