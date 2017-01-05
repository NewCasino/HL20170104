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
        public DbSet<Remittance> Remittance { get; set; }
    }

	/// <summary>
    /// 充值汇款表
    /// </summary>
	[DisplayName("充值汇款表")]
    public partial class Remittance
    {

		
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ID")]
				[Key]
		public int  ID { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("UID")]
				public int  UID { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("UserName")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  UserName { get; set; }
		      
       
        
        /// <summary>
        /// 充值方式
        /// </summary>  
				[DisplayName("充值方式")]
		        [MaxLength(50,ErrorMessage="充值方式最大长度为50")]
		public string  RechargeWay { get; set; }
		      
       
        
        /// <summary>
        /// 充值金额
        /// </summary>  
				[DisplayName("充值金额")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  RechargeAmount { get; set; }
		      
       
        
        /// <summary>
        /// 备注
        /// </summary>  
				[DisplayName("备注")]
		        [MaxLength(250,ErrorMessage="备注最大长度为250")]
		public string  Remark { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("CreateTime")]
				public DateTime  CreateTime { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ChongNumber")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  ChongNumber { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("PayOrderNumber")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  PayOrderNumber { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("PayImg")]
		        [MaxLength(250,ErrorMessage="最大长度为250")]
		public string  PayImg { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("Platform")]
				public int?  Platform { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("RechargeDate")]
				public DateTime?  RechargeDate { get; set; }
		      
       
        
        /// <summary>
        /// 是否审核
        /// </summary>  
				[DisplayName("是否审核")]
				public int  Status { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public Remittance()
        {
        //    ID = Guid.NewGuid();
        }
      
    }
 
    
}
namespace JN.Data.Service
{
    /// <summary>
    /// 充值汇款表业务接口
    /// </summary>
    public interface IRemittanceService :IServiceBase<Remittance> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(Remittance entity);
	}
    /// <summary>
    /// 充值汇款表业务类
    /// </summary>
    public class RemittanceService :  ServiceBase<Remittance>,IRemittanceService
    {


        public RemittanceService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(Remittance entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
