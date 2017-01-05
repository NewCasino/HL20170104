 


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
        public DbSet<CFBSplitting> CFBSplitting { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class CFBSplitting
    {

		
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ID")]
				[Key]
		public int  ID { get; set; }
		      
       
        
        /// <summary>
        /// 拆分前价格
        /// </summary>  
				[DisplayName("拆分前价格")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal?  OldPrice { get; set; }
		      
       
        
        /// <summary>
        /// 回落价格
        /// </summary>  
				[DisplayName("回落价格")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  NewPrice { get; set; }
		      
       
        
        /// <summary>
        /// 拆分倍数
        /// </summary>  
				[DisplayName("拆分倍数")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal?  Beisu { get; set; }
		      
       
        
        /// <summary>
        /// 参与人数
        /// </summary>  
				[DisplayName("参与人数")]
				public int  TotalUser { get; set; }
		      
       
        
        /// <summary>
        /// 拆分前总量
        /// </summary>  
				[DisplayName("拆分前总量")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  TotalBonus1 { get; set; }
		      
       
        
        /// <summary>
        /// 拆分后总量
        /// </summary>  
				[DisplayName("拆分后总量")]
		        [Filters.DecimalPrecision(18,5)]
		public decimal  TotalBonus2 { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("CreateTime")]
				public DateTime  CreateTime { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public CFBSplitting()
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
    public interface ICFBSplittingService :IServiceBase<CFBSplitting> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(CFBSplitting entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class CFBSplittingService :  ServiceBase<CFBSplitting>,ICFBSplittingService
    {


        public CFBSplittingService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(CFBSplitting entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
