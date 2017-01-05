 


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
        public DbSet<Advertisement> Advertisement { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class Advertisement
    {

		
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ID")]
				[Key]
		public int  ID { get; set; }
		      
       
        
        /// <summary>
        /// 广告位名称
        /// </summary>  
				[DisplayName("广告位名称")]
		        [MaxLength(50,ErrorMessage="广告位名称最大长度为50")]
		public string  AdName { get; set; }
		      
       
        
        /// <summary>
        /// 链接地址
        /// </summary>  
				[DisplayName("链接地址")]
		        [MaxLength(255,ErrorMessage="链接地址最大长度为255")]
		public string  LinkUrl { get; set; }
		      
       
        
        /// <summary>
        /// 媒体/图片地址
        /// </summary>  
				[DisplayName("媒体/图片地址")]
		        [MaxLength(255,ErrorMessage="媒体/图片地址最大长度为255")]
		public string  MediaUrl { get; set; }
		      
       
        
        /// <summary>
        /// 是否Flash
        /// </summary>  
				[DisplayName("是否Flash")]
				public bool  IsFlash { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ChannelID")]
				public int?  ChannelID { get; set; }
		      
       
        
        /// <summary>
        /// 广告类型(JS/图片/Flash)
        /// </summary>  
				[DisplayName("广告类型(JS/图片/Flash)")]
				public int?  ADType { get; set; }
		      
       
        
        /// <summary>
        /// JS脚本内容
        /// </summary>  
				[DisplayName("JS脚本内容")]
				public string  JSContent { get; set; }
		      
       
        
        /// <summary>
        /// 备注
        /// </summary>  
				[DisplayName("备注")]
		        [MaxLength(250,ErrorMessage="备注最大长度为250")]
		public string  Remark { get; set; }
		      
       
        
        /// <summary>
        /// 是否审核
        /// </summary>  
				[DisplayName("是否审核")]
				public bool  IsPassed { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("Sort")]
				public int  Sort { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public Advertisement()
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
    public interface IAdvertisementService :IServiceBase<Advertisement> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(Advertisement entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class AdvertisementService :  ServiceBase<Advertisement>,IAdvertisementService
    {


        public AdvertisementService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(Advertisement entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
