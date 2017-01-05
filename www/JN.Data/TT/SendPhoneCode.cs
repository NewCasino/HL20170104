 


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
        public DbSet<SendPhoneCode> SendPhoneCode { get; set; }
    }

	/// <summary>
    /// 
    /// </summary>
	[DisplayName("")]
    public partial class SendPhoneCode
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
				[DisplayName("PhoneNum")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  PhoneNum { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("ReCount")]
				public int  ReCount { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("LastSendTime")]
				public DateTime  LastSendTime { get; set; }
		      
       
        
        /// <summary>
        /// 
        /// </summary>  
				[DisplayName("RumCode")]
		        [MaxLength(50,ErrorMessage="最大长度为50")]
		public string  RumCode { get; set; }
		      
       
         

        /// <summary>
        /// 构造函数
        /// </summary>
		
        public SendPhoneCode()
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
    public interface ISendPhoneCodeService :IServiceBase<SendPhoneCode> {
		 /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityValidationResult GetValidationResult(SendPhoneCode entity);
	}
    /// <summary>
    /// 业务类
    /// </summary>
    public class SendPhoneCodeService :  ServiceBase<SendPhoneCode>,ISendPhoneCodeService
    {


        public SendPhoneCodeService(ISysDbFactory dbfactory) : base(dbfactory) {}
         /// <summary>
        /// 获取实体对象验证结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbEntityValidationResult GetValidationResult(SendPhoneCode entity)
        {
            return DataContext.Entry(entity).GetValidationResult();
        }
    }

}   
