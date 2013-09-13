using System;
using System.Collections.Generic;
using System.Text;

namespace Evt.Framework.Common
{
    #region SortType

    /// <summary>
    /// 排序枚举
    /// </summary>
    [Serializable]
    public enum SortTypeEnum
    {
        /// <summary>
        /// 正序
        /// </summary>
        Asc,
        /// <summary>
        /// 倒序
        /// </summary>
        Desc
    }

    #endregion

    #region CommandTypeEnum

    /// <summary>
    /// 如何解释命令字符串枚举
    /// </summary>
    public enum CommandTypeEnum : int
    {
        /// <summary>
        /// SQL 文本命令（默认）
        /// </summary>
        Text = 1,

        /// <summary>
        /// 存储过程的名称
        /// </summary>
        StoredProcedure = 4,

        /// <summary>
        /// 表的名称
        /// </summary>
        TableDirect = 512,
    }

    #endregion

    #region IsolationLevelEnum

    /// <summary>
    /// 连接的事务锁定行为枚举
    /// </summary>
    public enum IsolationLevelEnum : int
    {
        /// <summary>
        /// 正在使用与指定隔离级别不同的隔离级别，但是无法确定该级别。
        /// </summary>
        Unspecified = -1,

        /// <summary>
        /// 无法改写隔离级别更高的事务中的挂起的更改。
        /// </summary>
        Chaos = 16,

        /// <summary>
        /// 可以进行脏读，意思是说，不发布共享锁，也不接受独占锁。
        /// </summary>
        ReadUncommitted = 256,

        /// <summary>
        /// 在正在读取数据时保持共享锁，以避免脏读，但是在事务结束之前可以更改数据，从而导致不可重复的读取或幻像数据。
        /// </summary>
        ReadCommitted = 4096,

        /// <summary>
        /// 在查询中使用的所有数据上放置锁，以防止其他用户更新这些数据。防止不可重复的读取，但是仍可以有幻像行。
        /// </summary>
        RepeatableRead = 65536,

        /// <summary>
        ///  在 System.Data.DataSet 上放置范围锁，以防止在事务完成之前由其他用户更新行或向数据集中插入行。
        /// </summary>
        Serializable = 1048576,

        /// <summary>
        /// 通过在一个应用程序正在修改数据时存储另一个应用程序可以读取的相同数据版本来减少阻止。表示您无法从一个事务中看到在其他事务中进行的更改，即便重新查询也是如此。
        /// </summary>
        Snapshot = 16777216,
    }

    #endregion

    #region DatabaseTypeEnum

    /// <summary>
    /// 数据库类型枚举
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
    /// Trace级别枚举
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
    /// 参数类型枚举
    /// </summary>
    [Serializable]
    public enum ParameterDirectionEnum
    {
        // 摘要:
        //     参数是输入参数。
        Input = 1,
        //
        // 摘要:
        //     参数是输出参数。
        Output = 2,
        //
        // 摘要:
        //     参数既能输入，也能输出。
        InputOutput = 3,
        //
        // 摘要:
        //     参数表示诸如存储过程、内置函数或用户定义函数之类的操作的返回值。
        ReturnValue = 6,
    }

    #endregion

    #region DataFormatEnum

    /// <summary>
    /// 数据格式枚举
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
    /// 数据类型枚举
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
    /// Cache过期时间枚举（单位：秒）
    /// </summary>
    [Serializable]
    public enum CacheTimeoutEnum : int
    {
        /// <summary>
        /// 一分钟
        /// </summary>
        OneMinute = 60,

        /// <summary>
        /// 五分钟
        /// </summary>
        FiveMinutes = 300,

        /// <summary>
        /// 三十分钟
        /// </summary>
        ThirtyMinutes = 1800,

        /// <summary>
        /// 一小时
        /// </summary>
        SixtyMinutes = 3600,

        /// <summary>
        /// 六小时
        /// </summary>
        SixHours = 21600,

        /// <summary>
        /// 十二小时
        /// </summary>
        TwelveHours = 43200,

        /// <summary>
        /// 二十四小时
        /// </summary>
        TwentyFourHours = 86400
    }

    #endregion

    #region CacheTypeEnum

    /// <summary>
    /// Cache类型枚举
    /// </summary>
    [Serializable]
    public enum CacheTypeEnum
    {
        /// <summary>
        /// 保存Web Cache
        /// </summary>
        WebCache,

        /// <summary>
        /// 保存Memcached
        /// </summary>
        Memcached,

        /// <summary>
        /// 同时保存Web Cache和Memcached
        /// </summary>
        Both
    }

    #endregion
}
