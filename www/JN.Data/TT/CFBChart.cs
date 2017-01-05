 


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
        public DbSet<CFBChart> CFBChart { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class CFBChart
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
		        [Filters.DecimalPrecision(18,2)]
		public decimal  Price { get; set; }
		      
       
        
        /// <summary>
        /// 成交量
        /// </summary>  
				[DisplayName("成交量")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  Volume { get; set; }
		      
       
        
        /// <summary>
        /// 成交额
        /// </summary>  
				[DisplayName("成交额")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  TurnoverMoney { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("TurnoverScale")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  TurnoverScale { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("CreateTime")]
				public DateTime  CreateTime { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public CFBChart()
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
    public interface ICFBChartService :IServiceBase<CFBChart> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(CFBChart entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class CFBChartService :  ServiceBase<CFBChart>,ICFBChartService
    {


        public CFBChartService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(CFBChart entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
