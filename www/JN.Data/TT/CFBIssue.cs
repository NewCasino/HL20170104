 


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
        public DbSet<CFBIssue> CFBIssue { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class CFBIssue
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
        /// 总发行量
        /// </summary>  
				[DisplayName("总发行量")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  TotalStock { get; set; }
		      
       
        
        /// <summary>
        /// 已认购数量
        /// </summary>  
				[DisplayName("已认购数量")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  HaveSubscribe { get; set; }
		      
       
        
        /// <summary>
        /// 发行原始价
        /// </summary>  
				[DisplayName("发行原始价")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  Price { get; set; }
		      
       
        
        /// <summary>
        /// 涨幅
        /// </summary>  
				[DisplayName("涨幅")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  StepPrice { get; set; }
		      
       
        
        /// <summary>
        /// 涨幅条件（买入数量）
        /// </summary>  
				[DisplayName("涨幅条件（买入数量）")]
		        [Filters.DecimalPrecision(18,2)]
		public decimal  StepNumber { get; set; }
		      
       
        
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
		
        public CFBIssue()
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
    public interface ICFBIssueService :IServiceBase<CFBIssue> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(CFBIssue entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class CFBIssueService :  ServiceBase<CFBIssue>,ICFBIssueService
    {


        public CFBIssueService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(CFBIssue entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
