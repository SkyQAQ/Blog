using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Quartz;
using Quartz.Impl;
using System.Reflection;

namespace Blog.Core.Common.Quartz
{
    /// <summary>
    /// Quartz帮助类
    /// </summary>
    public class QuartzHelper
    {
        /// <summary>
        /// 调度作业
        /// </summary>
        private IScheduler scheduler;

        /// <summary>
        /// 初始化调度作业
        /// </summary>
        public async void InitSchedulerAsync()
        {
            try
            {
                if (scheduler == null)
                {
                    scheduler = await GetSchedulerAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取调度作业
        /// </summary>
        /// <returns></returns>
        private async Task<IScheduler> GetSchedulerAsync()
        {
            try
            {
                if (scheduler != null)
                {
                    return scheduler;
                }
                else
                {
                    NameValueCollection properties = new NameValueCollection();
                    // Configuring Quartz to use JobStoreTx
                    properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
                    // Configuring AdoJobStore to use a DriverDelegate
                    properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz";
                    // Configuring AdoJobStore with the Table Prefix
                    properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
                    // Configuring AdoJobStore with the name of the data source to use
                    properties["quartz.jobStore.dataSource"] = "myDS";
                    // Setting Data Source’s Connection String And Database Provider
                    properties["quartz.dataSource.myDS.connectionString"] = new ConfigHelper().MSSQL;
                    properties["quartz.dataSource.myDS.provider"] = "SqlServer";
                    properties["quartz.serializer.type"] = "binary";
                    // Configuring AdoJobStore to use strings as JobDataMap values (recommended)
                    properties["quartz.jobStore.useProperties"] = "true";
                    // Max connecting count
                    //properties["quartz.dataSource.myDS.maxConnections"] = "5";
                    // First we must get a reference to a scheduler
                    ISchedulerFactory sf = new StdSchedulerFactory(properties);
                    scheduler = await sf.GetScheduler();
                    return scheduler;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建或更新Job
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="model"></param>
        public void CreateJob(string assemblyName, QuartzEditModel model)
        {
            try
            {
                InitSchedulerAsync();
                if (!scheduler.IsStarted)
                    throw new Exception("请先启动定时任务程序");
                if (!CronExpression.IsValidExpression(model.Cron))
                    throw new Exception(string.Format("Cron表达式[{0}]格式不正确！", model.Cron));
                Assembly assembly = Assembly.LoadFrom(Constants.ServerMapPath() + assemblyName);
                if (assembly == null)
                    throw new Exception(string.Format("加载Assembly[{0}]失败！", assemblyName));
                Type type = assembly.GetType(model.JobClass);
                if (type == null)
                    throw new Exception(string.Format("加载Class[{0}]失败！", model.JobClass));
                if (!type.IsSubclassOf(typeof(BaseJob)))
                    throw new Exception(string.Format("Class[{0}]未继承BaseJob！", model.JobClass));
                JobKey jobKey = new JobKey(model.JobName, model.JobGroup);
                IJobDetail jobDetail = JobBuilder.Create(type)
                .WithIdentity(jobKey)
                .WithDescription(model.JobDesc)
                .Build();
                scheduler.AddJob(jobDetail, false);
                TriggerKey triggerKey = new TriggerKey(model.JobName, model.JobGroup);
                ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                    .WithIdentity(triggerKey)
                                                    .WithCronSchedule(model.Cron)
                                                    .WithDescription(model.CronDesc)
                                                    .Build();
                scheduler.ScheduleJob(jobDetail, trigger);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理Job
        /// </summary>
        /// <param name="operate"></param>
        /// <param name="assemblyName"></param>
        /// <param name="model"></param>
        public void HandleJobAsync(string operate, string assemblyName, QuartzEditModel model)
        {
            try
            {
                InitSchedulerAsync();
                if (!scheduler.IsStarted)
                    throw new Exception("请先启动定时任务程序");
                JobKey jobKey = new JobKey(model.JobName, model.JobGroup);
                TriggerKey triggerKey = new TriggerKey(model.JobName, model.JobGroup);
                Assembly assembly = Assembly.LoadFrom(Constants.ServerMapPath() + assemblyName);
                if (assembly == null)
                    throw new Exception(string.Format("加载Assembly[{0}]失败！", assemblyName));
                Type type = assembly.GetType(model.JobClass);
                switch (operate)
                {
                    case Constants.JobPause:
                        scheduler.PauseJob(jobKey);
                        break;
                    case Constants.JobResume:
                        scheduler.ResumeJob(jobKey);
                        break;
                    case Constants.JobStop:
                        scheduler.UnscheduleJob(triggerKey);
                        break;
                    case Constants.JobStart:
                        CreateJob(assemblyName, model);
                        break;
                    case Constants.JobExcute:
                        IJobDetail jobDetail1 = JobBuilder.Create(type).WithIdentity(model.JobName + "$$" + Guid.NewGuid().ToString(), model.JobGroup + "$$" + Guid.NewGuid().ToString()).Build();
                        ITrigger trigger1 = TriggerBuilder.Create()
                        .StartNow().Build();
                         scheduler.ScheduleJob(jobDetail1, trigger1);
                        break;
                    default:
                        throw new Exception("operate is not invalid!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 停止调度作业
        /// </summary>
        /// <param name="group"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Stop()
        {
            InitSchedulerAsync();
            if (!scheduler.IsShutdown)
                scheduler.Shutdown();
            return scheduler.IsShutdown;
        }

        /// <summary>
        /// 启动调度作业
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                InitSchedulerAsync();
                if (!scheduler.IsStarted)
                    scheduler.Start();
                return scheduler.IsStarted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取调度作业状态，开始为true 其他为false
        /// </summary>
        /// <returns></returns>
        public bool Status()
        {
            InitSchedulerAsync();
            if (!scheduler.IsStarted)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 判断cron是否格式正确
        /// </summary>
        /// <param name="cron"></param>
        /// <returns></returns>
        public bool IsValidExpression(string cron)
        {
            return CronExpression.IsValidExpression(cron);
        }

        public void Register<IJob>(IJob job)
        {
            
        }
    }

    /// <summary>
    /// Quartz编辑模型
    /// </summary>
    public class QuartzEditModel
    {
        /// <summary>
        /// JobInfoId
        /// </summary>
        public string JobInfoId { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Job分组
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// Job描述
        /// </summary>
        public string JobDesc { get; set; }

        /// <summary>
        /// Job Cron
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// Cron描述
        /// </summary>
        public string CronDesc { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string JobClass { get; set; }
    }
}
