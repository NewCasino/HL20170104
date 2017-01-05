 


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
        public DbSet<ShopProduct> ShopProduct { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class ShopProduct
    {

		
        
        /// <summary>
        /// 商品表ID
        /// </summary>  
				[DisplayName("商品表ID")]
				[Key]
		public int  ID { get; set; }
		      
       
        
        /// <summary>
        /// 会员ID(店铺)
        /// </summary>  
				[DisplayName("会员ID(店铺)")]
				public int  ShopID { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ShopName")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  ShopName { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("CreateTime")]
				public DateTime  CreateTime { get; set; }
		      
       
        
        /// <summary>
        /// 产品名称
        /// </summary>  
				[DisplayName("产品名称")]
		        [MaxLength(250,ErrorMessage="产品名称最大长度为250")]
		public string  ProductName { get; set; }
		      
       
        
        /// <summary>
        /// 商品简称
        /// </summary>  
				[DisplayName("商品简称")]
		        [MaxLength(64,ErrorMessage="商品简称最大长度为64")]
		public string  ShortName { get; set; }
		      
       
        
        /// <summary>
        /// 产品编码
        /// </summary>  
				[DisplayName("产品编码")]
		        [MaxLength(250,ErrorMessage="产品编码最大长度为250")]
		public string  ProductCode { get; set; }
		      
       
        
        /// <summary>
        /// 规格
        /// </summary>  
				[DisplayName("规格")]
		        [MaxLength(50,ErrorMessage="规格最大长度为50")]
		public string  Spec { get; set; }
		      
       
        
        /// <summary>
        /// 单位
        /// </summary>  
				[DisplayName("单位")]
		        [MaxLength(50,ErrorMessage="单位最大长度为50")]
		public string  Unit { get; set; }
		      
       
        
        /// <summary>
        /// 图片地址
        /// </summary>  
				[DisplayName("图片地址")]
		        [MaxLength(250,ErrorMessage="图片地址最大长度为250")]
		public string  ImageUrl { get; set; }
		      
       
        
        /// <summary>
        /// 产品分类
        /// </summary>  
				[DisplayName("产品分类")]
				public int  ClassId { get; set; }
		      
       
        
        /// <summary>
        /// 原价
        /// </summary>  
				[DisplayName("原价")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal?  CostPrice { get; set; }
		      
       
        
        /// <summary>
        /// 现价
        /// </summary>  
				[DisplayName("现价")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  RealPrice { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("FreightPrice")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal?  FreightPrice { get; set; }
		      
       
        
        /// <summary>
        /// 库存
        /// </summary>  
				[DisplayName("库存")]
				public int  Stock { get; set; }
		      
       
        
        /// <summary>
        /// 产品简介
        /// </summary>  
				[DisplayName("产品简介")]
		        [MaxLength(250,ErrorMessage="产品简介最大长度为250")]
		public string  ShortContent { get; set; }
		      
       
        
        /// <summary>
        /// 商品详细介绍
        /// </summary>  
				[DisplayName("商品详细介绍")]
				public string  Content { get; set; }
		      
       
        
        /// <summary>
        /// 1上架，0下架
        /// </summary>  
				[DisplayName("1上架，0下架")]
				public bool  IsSales { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("SaleCount")]
				public int  SaleCount { get; set; }
		      
       
        
        /// <summary>
        /// 1明星产品，0不是
        /// </summary>  
				[DisplayName("1明星产品，0不是")]
				public bool?  IsStar { get; set; }
		      
       
        
        /// <summary>
        /// 1网友都喜欢，0不是
        /// </summary>  
				[DisplayName("1网友都喜欢，0不是")]
				public bool?  IsHot { get; set; }
		      
       
        
        /// <summary>
        /// 1是热评，0不是
        /// </summary>  
				[DisplayName("1是热评，0不是")]
				public bool?  IsElite { get; set; }
		      
       
        
        /// <summary>
        /// 1是爱看图书，0不是
        /// </summary>  
				[DisplayName("1是爱看图书，0不是")]
				public bool?  IsTop { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("Hits")]
				public int  Hits { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ReserveStr1")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  ReserveStr1 { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ReserveStr2")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  ReserveStr2 { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ReserveStr3")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  ReserveStr3 { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ReserveInt1")]
				public int?  ReserveInt1 { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ReserveInt2")]
				public int?  ReserveInt2 { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ReserveDate")]
				public DateTime?  ReserveDate { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ReserveDecamal")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal?  ReserveDecamal { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public ShopProduct()
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
    public interface IShopProductService :IServiceBase<ShopProduct> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(ShopProduct entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class ShopProductService :  ServiceBase<ShopProduct>,IShopProductService
    {


        public ShopProductService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(ShopProduct entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
