1.依赖于Log4net，必须添加Log4net配置
2.需添加以下配置
//方法执行时长,超过该设置的方法才会被拦截记录,单位为毫秒,默认为100毫秒
<add key="MethodExcuteMaxDuration" value="1" />
//开启或关闭性能监视, 默认为关闭
<add key="PerformanceMonitorEnable" value="1" />
//开启或关闭参数记录, 默认为关闭
 <add key="ArgumentsLogEnable" value="1" />
//日志写入文件时间间隔, 默认为3000，即3秒，一般无需设置
<add key="PerformanceMonitorLogWriteInterval" value="3000" />