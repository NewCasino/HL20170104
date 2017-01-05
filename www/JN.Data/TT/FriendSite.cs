 


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
        public DbSet<FriendSite> FriendSite { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class FriendSite
    {

		
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ID")]
				[Key]
		public int  ID { get; set; }
		      
       
        
        /// <summary>
        /// 友链类型
        /// </summary>  
				[DisplayName("友链类型")]
		        [MaxLength(50,ErrorMessage="友链类型最大长度为50")]
		public string  TypeName { get; set; }
		      
       
        
        /// <summary>
        /// 站点名称
        /// </summary>  
				[DisplayName("站点名称")]
		        [MaxLength(50,ErrorMessage="站点名称最大长度为50")]
		public string  SiteName { get; set; }
		      
       
        
        /// <summary>
        /// 链接地址
        /// </summary>  
				[DisplayName("链接地址")]
				public string  SiteUrl { get; set; }
		      
       
        
        /// <summary>
        /// 站点描述
        /// </summary>  
				[DisplayName("站点描述")]
				public string  SiteIntro { get; set; }
		      
       
        
        /// <summary>
        /// 友链图标
        /// </summary>  
				[DisplayName("友链图标")]
				public string  LogoUrl { get; set; }
		      
       
        
        /// <summary>
        /// 站长
        /// </summary>  
				[DisplayName("站长")]
		        [MaxLength(50,ErrorMessage="站长最大长度为50")]
		public string  SiteAdmin { get; set; }
		      
       
        
        /// <summary>
        /// 邮箱
        /// </summary>  
				[DisplayName("邮箱")]
		        [MaxLength(50,ErrorMessage="邮箱最大长度为50")]
		public string  Email { get; set; }
		      
       
        
        /// <summary>
        /// 是否审核
        /// </summary>  
				[DisplayName("是否审核")]
				public bool  IsPassed { get; set; }
		      
       
        
        /// <summary>
        /// 排序
        /// </summary>  
				[DisplayName("排序")]
				public int  Sort { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public FriendSite()
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
    public interface IFriendSiteService :IServiceBase<FriendSite> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(FriendSite entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class FriendSiteService :  ServiceBase<FriendSite>,IFriendSiteService
    {


        public FriendSiteService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(FriendSite entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
