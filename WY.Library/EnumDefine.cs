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
    /// �û���ɫ��Ȩ��
    /// </summary>
    public enum EnmUserRole
    {       
        ϵͳ����Ա,
        �����ܼ�,
        ����,
        ¼����Ա,
        ��������,
        ȫ��
    }

    /// <summary>
    /// �Ƿ�ɾ��
    /// </summary>
    public enum EnmIsdeleted
    {
        ʹ����,
        ��ɾ��,
        �ѹ���
    }

    /// <summary>
    /// ���õ�¼
    /// </summary>
    public enum EnmIsUsed
    {
        ����,
        ������
    }

    /// <summary>
    /// ��·״̬
    /// </summary>
    public enum EnmCableStatus
    {
        δ�깤,
        �Ѳ��,
        ���깤,        
        �������겻��,
        ���ټ۸�����,
        ���ټ۸񲻱�,
        ���ټ۸��½�,
        ���ټ۸��½�,
        ȡ��
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum EnmPayType
    {
        �¸�,
        ����,
        һ���Ը�,
        ���긶
    }

    /// <summary>
    /// �����Ա����
    /// </summary>
    public enum EnmDataType
    {
        ����������,
        ��������,
        �깤¼��,
        ������˰��,
        ����˰��,
        �깤¼��˰��
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum EnmIsAuto
    {
        �Զ�����,
        �˹�����
    }

    /// <summary>
    /// ������������
    /// </summary>
    public enum EnmRptType
    {
        ����嵥,
        �����嵥
    }

    /// <summary>
    /// ����״̬
    /// </summary>
    public enum EnmWriteOffFlag
    {
        ��������,
        Ԥ����,
        ������
    }

    public enum EnmCalbeClass
    {
        ר��,
        ����
    }

    ///// <summary>
    ///// ��������
    ///// </summary>
    //public enum EnmWriteOff
    //{
    //    Ԥ��,
    //    ʵ��
    //}

    public enum EnmMonth
    {
        ȫ��,
        ��ʼ��,
        ��ֹ��
    }

    public enum EnmChangeType
    {
        �����깤��¼,
        ��·״̬���,
        �������������,
        ȫ�����
    }
}
