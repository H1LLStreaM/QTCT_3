using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Library
{
    public enum EnmOrderType
    {
        None,
        Asc,
        Desc
    }

    /// <summary>
    /// 用户角色、权限
    /// </summary>
    public enum EnmUserRole
    {       
        系统管理员,
        销售总监,
        财务,
        录入人员,
        销售渠道,
        全部
    }

    /// <summary>
    /// 是否删除
    /// </summary>
    public enum EnmIsdeleted
    {
        使用中,
        已删除,
        已过户
    }

    /// <summary>
    /// 启用登录
    /// </summary>
    public enum EnmIsUsed
    {
        启用,
        不启用
    }

    /// <summary>
    /// 电路状态
    /// </summary>
    public enum EnmCableStatus
    {
        未完工,
        已拆机,
        已完工,        
        延续上年不变,
        升速价格上升,
        升速价格不变,
        升速价格下降,
        降速价格下降,
        取消
    }

    /// <summary>
    /// 付款类型
    /// </summary>
    public enum EnmPayType
    {
        月付,
        季付,
        一次性付,
        半年付
    }

    /// <summary>
    /// 提成人员类型
    /// </summary>
    public enum EnmDataType
    {
        主销售渠道,
        销售渠道,
        完工录入,
        主销售税率,
        销售税率,
        完工录入税率
    }

    /// <summary>
    /// 结算类型
    /// </summary>
    public enum EnmIsAuto
    {
        自动结算,
        人工结算
    }

    /// <summary>
    /// 导出单据类型
    /// </summary>
    public enum EnmRptType
    {
        提成清单,
        财务清单
    }

    /// <summary>
    /// 结算状态
    /// </summary>
    public enum EnmWriteOffFlag
    {
        正常结算,
        预结算,
        补结算
    }

    public enum EnmCalbeClass
    {
        专网,
        上网
    }

    ///// <summary>
    ///// 结算类型
    ///// </summary>
    //public enum EnmWriteOff
    //{
    //    预计,
    //    实绩
    //}

    public enum EnmMonth
    {
        全月,
        起始月,
        截止月
    }

    public enum EnmChangeType
    {
        新增完工记录,
        电路状态变更,
        主销售渠道变更,
        全部变更
    }
}
