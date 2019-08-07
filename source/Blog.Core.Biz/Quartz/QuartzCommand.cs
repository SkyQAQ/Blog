using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.Common.Quartz;
using Blog.Core.Model;

namespace Blog.Core.Biz.Quartz
{
    /// <summary>
    /// 定时任务Command
    /// </summary>
    public class QuartzCommand : BaseCommand
    {
        /// <summary>
        /// Quartz帮助类
        /// </summary>
        private QuartzHelper quartz;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="identity"></param>
        public QuartzCommand(UserIdentity identity) : base(identity)
        {
            quartz = new QuartzHelper();
        }

        /// <summary>
        /// 停止调度作业
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            try
            {
                return quartz.Stop();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new Exception("停止调度作业失败");
            }
        }

        /// <summary>
        /// 启动调度作业
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                return quartz.Start();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new Exception("启动调度作业失败");
            }
        }

        /// <summary>
        /// 获取调度作业状态，开始为true 其他为false
        /// </summary>
        /// <returns></returns>
        public bool Status()
        {
            return quartz.Status();
        }

        /// <summary>
        /// 获取定时任务列表数据
        /// </summary>
        /// <param name="searchList"></param>
        /// <returns></returns>
        public List<JobInfoListModel> GetQuartzList(List<SearchCondition> searchList)
        {
            try
            {
                List<JobInfoListModel> result = new List<JobInfoListModel>();

                #region 初始化数据
                string sqlString = @"SELECT * FROM JobInfo WHERE  IsDeleted = 0 ";
                Dictionary<string, object> paramList = new Dictionary<string, object>();
                #endregion

                #region 条件过滤
                if (searchList != null && searchList.Count > 0)
                {
                    searchList.ForEach(s =>
                    {
                        switch (s.Type)
                        {
                            case Constants.COMMON_SEARCHCONDITIONTYPE_EQUAL:
                                sqlString += string.Concat(" AND ", s.Table, s.Key, " = @", s.Key);
                                paramList.Add(string.Concat("@", s.Key), s.Value);
                                break;
                            case Constants.COMMON_SEARCHCONDITIONTYPE_LIKE:
                                sqlString += string.Concat(" AND ", s.Table, s.Key, " like @", s.Key);
                                paramList.Add(string.Concat("@", s.Key), s.Value + "%");
                                break;
                            case Constants.COMMON_SEARCHCONDITIONTYPE_RANGE:
                                if (s.Key.ToLower().Contains("after"))
                                {
                                    sqlString += string.Concat(" AND ", s.Table, s.Key.Replace("after", ""), " >= @", s.Key);
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToDateTime(s.Value).ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else if (s.Key.ToLower().Contains("before"))
                                {
                                    sqlString += string.Concat(" AND ", s.Table, s.Key.Replace("before", ""), " <= @", s.Key);
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToDateTime(s.Value).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else
                                {
                                    throw new Exception("调用的参数错误，无法认定区间起始:" + s.Value);
                                }
                                break;
                            case Constants.COMMON_SEARCHCONDITIONTYPE_CUSTOMER:
                                break;
                        }
                    });
                }
                #endregion

                #region 查询数据              
                sqlString += " ORDER BY JobGroup ASC, JobName ASC";
                DataTable dtResult = _sql.Query(sqlString, paramList);
                #endregion

                #region 构建结果
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    result = dtResult.ToModelList<JobInfoListModel>();
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 获取定时任务运行日志列表数据
        /// </summary>
        /// <param name="group"></param>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="onlyError"></param>
        /// <returns></returns>
        public LogInfoListData GetLogInfoList(string group, string name, int pageIndex, int pageSize, bool onlyError)
        {
            try
            {
                LogInfoListData result = new LogInfoListData();

                #region 初始化数据
                string sqlString = @"SELECT JobLogId,StartTime,EndTime,Result,Host,
                                            CASE WHEN Status = 1 THEN '成功'
                                                 ELSE '失败'
                                            END AS Status
                                     FROM tbl_joblog 
                                     WHERE JobGroup = @group AND JobName = @name";
                Dictionary<string, object> paramList = new Dictionary<string, object>() { { "@group", group }, { "@name", name } };
                if (onlyError)
                {
                    sqlString += " AND Status = 0";
                }
                #endregion

                #region 查询数据              
                int recordCount = 0;
                DataTable dtResult = _sql.Query(sqlString, paramList, " order by StartTime desc", pageSize, pageIndex, out recordCount);
                #endregion

                #region 构建结果
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    result.RecordCount = recordCount;
                    result.RecordList = dtResult.ToModelList<LogInfoListModel>();
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 根据JobId获取Job编辑信息
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        public QuartzEditModel EditQuartz(string JobId)
        {
            try
            {
                QuartzEditModel result = new QuartzEditModel();
                JobInfo job = _sql.Search<JobInfo>(JobId);
                if (job == null)
                    throw new Exception("当前Job丢失！");
                result.JobInfoId = JobId;
                result.JobGroup = job.JobGroup;
                result.JobName = job.JobName;
                result.JobDesc = job.JobDesc;
                result.Cron = job.Cron;
                result.CronDesc = job.CronDesc;
                result.JobClass = job.JobClass;
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 创建Quartz
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string CreateOrUpdateQuartz(QuartzEditModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.JobGroup))
                    throw new Exception("Job分组不能为空！");
                if (string.IsNullOrWhiteSpace(model.JobName))
                    throw new Exception("Job名不能为空！");
                if (string.IsNullOrWhiteSpace(model.Cron))
                    throw new Exception("Cron表达式不能为空不能为空！");
                if (string.IsNullOrWhiteSpace(model.JobClass))
                    throw new Exception("Job类名不能为空！");
                _sql.OpenDb();
                if (string.IsNullOrEmpty(model.JobInfoId))
                { 
                    DataTable dt = _sql.Query("SELECT JOB_CLASS_NAME FROM QRTZ_JOB_DETAILS WITH(NOLOCK) WHERE JOB_GROUP = @group AND JOB_NAME = @name", new Dictionary<string, object> { { "@group", model.JobGroup }, { "@name", model.JobName } });
                    if (dt != null && dt.Rows.Count > 0)
                        throw new Exception("当前Job已存在，请更改组名或Job名！");
                    quartz.CreateJob(_config.Job_Assembly, model);
                    JobInfo job = new JobInfo();
                    model.FillTableWithModel<JobInfo>(job);
                    job.JobStatus = Constants.JobStatusRunning;
                    _sql.Create(job);
                }
                else
                {
                    JobInfo job = _sql.Search<JobInfo>(model.JobInfoId);
                    model.FillTableWithModel<JobInfo>(job);
                    _sql.Update(job);
                }
                return Constants.SaveSuccessMssg;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
            finally
            {
                _sql.CloseDb();
            }
        }

        /// <summary>
        /// 删除Job
        /// </summary>
        /// <param name="ids">Job Ids</param>
        /// <returns></returns>
        public string DeleteQuartz(string[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                    throw new Exception("请选择待删除的Job！");
                _sql.OpenDb();
                _sql.BeginTrans();
                foreach(string id in ids)
                {
                    JobInfo job = _sql.Search<JobInfo>(id);
                    if (job == null) { continue; }
                    if (job.JobStatus != Constants.JobStatusStop)
                        throw new Exception(string.Format("JOB:[{0}-{1}]未停止，无法删除！", job.JobGroup, job.JobName));
                    _sql.Execute("DELETE FROM tbl_joblog WHERE JobGroup = @group AND JobName = @name", new Dictionary<string, object> { { "@group", job.JobGroup }, { "@name", job.JobName } });
                    _sql.Delete(id, JobInfo.TableName);
                }
                _sql.Commit();
                return Constants.DeleteSuccessMssg;
            }
            catch (Exception ex)
            {
                _sql.Rollback();
                _log.Error(ex);
                throw ex;
            }
            finally
            {
                _sql.CloseDb();
            }
        }

        /// <summary>
        /// 处理Job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="operate"></param>
        /// <returns></returns>
        public string HandleJobAsync(string jobId, string operate)
        {
            try
            {
                JobInfo job = _sql.Search<JobInfo>(jobId);
                QuartzEditModel model = new QuartzEditModel();
                model.JobGroup = job.JobGroup;
                model.JobName = job.JobName;
                model.JobDesc = job.JobDesc;
                model.Cron = job.Cron;
                model.CronDesc = job.CronDesc;
                model.JobClass = job.JobClass;
                DataTable dt = _sql.Query(@"SELECT t.TRIGGER_STATE 
                                            FROM QRTZ_TRIGGERS t WITH(NOLOCK)
                                            WHERE t.JOB_GROUP = @group AND t.JOB_NAME = @name", new Dictionary<string, object> { { "@group", job.JobGroup }, { "@name", job.JobName } });
                if ((dt == null || dt.Rows.Count == 0) && operate != Constants.JobStart)
                    throw new Exception("当前处理的JOB不存在！");
                if(operate == Constants.JobPause)
                {
                    if (Cast.ConToString(dt.Rows[0]["TRIGGER_STATE"]) != Constants.TriggerStateWaitting)
                        throw new Exception("当前Job状态无法暂停！");
                    job.JobStatus = Constants.JobStatusPause;
                }
                else if(operate == Constants.JobResume)
                {
                    if (Cast.ConToString(dt.Rows[0]["TRIGGER_STATE"]) != Constants.TriggerStatePaused)
                        throw new Exception("当前Job状态无法恢复！");
                    job.JobStatus = Constants.JobStatusRunning;
                }
                else if (operate == Constants.JobExcute)
                {
                    if (Cast.ConToString(dt.Rows[0]["TRIGGER_STATE"]) != Constants.TriggerStatePaused)
                        throw new Exception("请暂停后再执行Job！");
                }
                else if (operate == Constants.JobStop)
                {
                    job.JobStatus = Constants.JobStatusStop;
                }
                else if (operate == Constants.JobStart)
                {
                    job.JobStatus = Constants.JobStatusRunning;
                }
                quartz.HandleJobAsync(operate, _config.Job_Assembly, model);
                _sql.OpenDb();
                _sql.Update(job);
                return job.JobStatus;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                _sql.CloseDb();
            }
        }

        /// <summary>
        /// 获取Job类
        /// </summary>
        /// <returns></returns>
        public List<OptionModel> GetJobClass()
        {
            List<OptionModel> result = new List<OptionModel>();
            Assembly assembly = Assembly.LoadFrom(Constants.ServerMapPath() + _config.Job_Assembly);
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(BaseJob)))
                {
                    result.Add(new OptionModel { Text = type.FullName, Value = type.FullName });
                }
            }
            return result;
        }
    }
}
