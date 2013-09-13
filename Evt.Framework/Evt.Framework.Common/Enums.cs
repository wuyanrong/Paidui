using System;
using System.Collections.Generic;
using System.Text;

namespace Evt.Framework.Common
{
    #region SortType

    /// <summary>
    /// ����ö��
    /// </summary>
    [Serializable]
    public enum SortTypeEnum
    {
        /// <summary>
        /// ����
        /// </summary>
        Asc,
        /// <summary>
        /// ����
        /// </summary>
        Desc
    }

    #endregion

    #region CommandTypeEnum

    /// <summary>
    /// ��ν��������ַ���ö��
    /// </summary>
    public enum CommandTypeEnum : int
    {
        /// <summary>
        /// SQL �ı����Ĭ�ϣ�
        /// </summary>
        Text = 1,

        /// <summary>
        /// �洢���̵�����
        /// </summary>
        StoredProcedure = 4,

        /// <summary>
        /// �������
        /// </summary>
        TableDirect = 512,
    }

    #endregion

    #region IsolationLevelEnum

    /// <summary>
    /// ���ӵ�����������Ϊö��
    /// </summary>
    public enum IsolationLevelEnum : int
    {
        /// <summary>
        /// ����ʹ����ָ�����뼶��ͬ�ĸ��뼶�𣬵����޷�ȷ���ü���
        /// </summary>
        Unspecified = -1,

        /// <summary>
        /// �޷���д���뼶����ߵ������еĹ���ĸ��ġ�
        /// </summary>
        Chaos = 16,

        /// <summary>
        /// ���Խ����������˼��˵����������������Ҳ�����ܶ�ռ����
        /// </summary>
        ReadUncommitted = 256,

        /// <summary>
        /// �����ڶ�ȡ����ʱ���ֹ��������Ա���������������������֮ǰ���Ը������ݣ��Ӷ����²����ظ��Ķ�ȡ��������ݡ�
        /// </summary>
        ReadCommitted = 4096,

        /// <summary>
        /// �ڲ�ѯ��ʹ�õ����������Ϸ��������Է�ֹ�����û�������Щ���ݡ���ֹ�����ظ��Ķ�ȡ�������Կ����л����С�
        /// </summary>
        RepeatableRead = 65536,

        /// <summary>
        ///  �� System.Data.DataSet �Ϸ��÷�Χ�����Է�ֹ���������֮ǰ�������û������л������ݼ��в����С�
        /// </summary>
        Serializable = 1048576,

        /// <summary>
        /// ͨ����һ��Ӧ�ó��������޸�����ʱ�洢��һ��Ӧ�ó�����Զ�ȡ����ͬ���ݰ汾��������ֹ����ʾ���޷���һ�������п��������������н��еĸ��ģ��������²�ѯҲ����ˡ�
        /// </summary>
        Snapshot = 16777216,
    }

    #endregion

    #region DatabaseTypeEnum

    /// <summary>
    /// ���ݿ�����ö��
    /// </summary>
    public enum DatabaseTypeEnum : int
    {
        /// <summary>
        /// 
        /// </summary>
        SqlServer,

        /// <summary>
        /// 
        /// </summary>
        Oracle,

        /// <summary>
        /// 
        /// </summary>
        Access,

        /// <summary>
        /// 
        /// </summary>
        MySql,
    }

    #endregion

    #region TraceLevelEnum

    /// <summary>
    /// Trace����ö��
    /// </summary>
    public enum TraceLevelEnum : int
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// 
        /// </summary>
        Debug = 1,

        /// <summary>
        /// 
        /// </summary>
        Info = 2,

        /// <summary>
        /// 
        /// </summary>
        Warning = 4,

        /// <summary>
        /// 
        /// </summary>
        Error = 8,
    }

    #endregion

    #region ParameterDirectionEnum

    /// <summary>
    /// ��������ö��
    /// </summary>
    [Serializable]
    public enum ParameterDirectionEnum
    {
        // ժҪ:
        //     ���������������
        Input = 1,
        //
        // ժҪ:
        //     ���������������
        Output = 2,
        //
        // ժҪ:
        //     �����������룬Ҳ�������
        InputOutput = 3,
        //
        // ժҪ:
        //     ������ʾ����洢���̡����ú������û����庯��֮��Ĳ����ķ���ֵ��
        ReturnValue = 6,
    }

    #endregion

    #region DataFormatEnum

    /// <summary>
    /// ���ݸ�ʽö��
    /// </summary>
    [Serializable]
    public enum DataFormatEnum
    {
        None = 0,
        Number = 1,
        Text = 2,
        Int = 3,
        Float = 4,
        Date = 5,
        Time = 6,
        DateTime = 7,
        Url = 8,
        Email = 9
    }

    #endregion

    #region DataTypeEnum

    /// <summary>
    /// ��������ö��
    /// </summary>
    [Serializable]
    public enum DataTypeEnum
    {
        Default = 0,
        Date = 1
    }

    #endregion

    #region CacheTimeoutEnum

    /// <summary>
    /// Cache����ʱ��ö�٣���λ���룩
    /// </summary>
    [Serializable]
    public enum CacheTimeoutEnum : int
    {
        /// <summary>
        /// һ����
        /// </summary>
        OneMinute = 60,

        /// <summary>
        /// �����
        /// </summary>
        FiveMinutes = 300,

        /// <summary>
        /// ��ʮ����
        /// </summary>
        ThirtyMinutes = 1800,

        /// <summary>
        /// һСʱ
        /// </summary>
        SixtyMinutes = 3600,

        /// <summary>
        /// ��Сʱ
        /// </summary>
        SixHours = 21600,

        /// <summary>
        /// ʮ��Сʱ
        /// </summary>
        TwelveHours = 43200,

        /// <summary>
        /// ��ʮ��Сʱ
        /// </summary>
        TwentyFourHours = 86400
    }

    #endregion

    #region CacheTypeEnum

    /// <summary>
    /// Cache����ö��
    /// </summary>
    [Serializable]
    public enum CacheTypeEnum
    {
        /// <summary>
        /// ����Web Cache
        /// </summary>
        WebCache,

        /// <summary>
        /// ����Memcached
        /// </summary>
        Memcached,

        /// <summary>
        /// ͬʱ����Web Cache��Memcached
        /// </summary>
        Both
    }

    #endregion
}
