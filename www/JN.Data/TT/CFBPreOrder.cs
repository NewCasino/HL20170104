 


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
        public DbSet<CFBPreOrder> CFBPreOrder { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class CFBPreOrder
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
				[DisplayName("OrderNumber")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  OrderNumber { get; set; }
		      
       
        
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
        /// 预购金额
        /// </summary>  
				[DisplayName("预购金额")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  PreMoney { get; set; }
		      
       
        
        /// <summary>
        /// 已认筹金额
        /// </summary>  
				[DisplayName("已认筹金额")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  HaveSubscribeMoney { get; set; }
		      
       
        
        /// <summary>
        /// 预购数量
        /// </summary>  
				[DisplayName("预购数量")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  PreNumber { get; set; }
		      
       
        
        /// <summary>
        /// 单价
        /// </summary>  
				[DisplayName("单价")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  PrePrice { get; set; }
		      
       
        
        /// <summary>
        /// 状态(0待买入,1部分买和,2全部买入,-1取消)
        /// </summary>  
				[DisplayName("状态(0待买入,1部分买和,2全部买入,-1取消)")]
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
				[DisplayName("IsTop")]
				public bool?  IsTop { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("Origin")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  Origin { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public CFBPreOrder()
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
    public interface ICFBPreOrderService :IServiceBase<CFBPreOrder> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(CFBPreOrder entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class CFBPreOrderService :  ServiceBase<CFBPreOrder>,ICFBPreOrderService
    {


        public CFBPreOrderService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(CFBPreOrder entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
 
