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
        public DbSet<ShopOrderDetail> ShopOrderDetail { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class ShopOrderDetail
    {

		
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ID")]
				[Key]
		public long  ID { get; set; }
		      
       
        
        /// <summary>
        /// 订单编号
        /// </summary>  
				[DisplayName("订单编号")]
		        [MaxLength(50,ErrorMessage="订单编号最大长度为50")]
		public string  OrderNumber { get; set; }
		      
       
        
        /// <summary>
        /// 商品ID
        /// </summary>  
				[DisplayName("商品ID")]
				public int  ProductID { get; set; }
		      
       
        
        /// <summary>
        /// 商品名称
        /// </summary>  
				[DisplayName("商品名称")]
		        [MaxLength(64,ErrorMessage="商品名称最大长度为64")]
		public string  ProducName { get; set; }
		      
       
        
        /// <summary>
        /// 购买数量
        /// </summary>  
				[DisplayName("购买数量")]
				public int  ByCount { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("Spec")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  Spec { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("Unit")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  Unit { get; set; }
		      
       
        
        /// <summary>
        /// 单价
        /// </summary>  
				[DisplayName("单价")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  UnitPrice { get; set; }
		      
       
        
        /// <summary>
        /// 总价
        /// </summary>  
				[DisplayName("总价")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  TotaPrice { get; set; }
		      
       
        
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
        /// 构造函数
        /// </summary>
		
        public ShopOrderDetail()
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
    public interface IShopOrderDetailService :IServiceBase<ShopOrderDetail> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(ShopOrderDetail entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class ShopOrderDetailService :  ServiceBase<ShopOrderDetail>,IShopOrderDetailService
    {


        public ShopOrderDetailService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(ShopOrderDetail entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
