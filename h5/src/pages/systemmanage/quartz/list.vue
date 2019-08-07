<template>
<div id="quartzlist">
    <wy-header :titles="titles"></wy-header>
    <wy-grid ref="grid" @handleSearch="searchData" @handleReset="reset" :showPagination="false" :loading="loading">
        <template slot="button">
            <span style="font-size:14px;margin-right:5px;">是否启用</span>
            <el-switch style="margin-right:10px" :disabled="quartloading" v-model="quartStatus" @change="quartStatusChange"></el-switch>
            <el-button type="text" icon="el-icon-plus" @click="createData">新建</el-button>
            <el-button type="text" icon="el-icon-delete" @click="deleteData">删除</el-button>
        </template>
        <template slot="query">
            <el-form ref="queryForm" :model="queryForm" label-position="left" label-width="150px">
                <el-row :gutter="40" type="flex">
                    <el-col :span="8">
                        <el-form-item label="任务分组" prop="Group">
                            <el-input v-model="queryForm.Group" clearable></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="任务名称" prop="Name">
                            <el-input v-model="queryForm.UserAccount" clearable></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="任务描述" prop="Desc">
                            <el-input v-model="queryForm.Desc" clearable></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form>
        </template>
        <template slot="tabledata">
            <el-table ref="gridTable" v-bind:data="tableData" border stripe v-loading="loading" :height="$store.state.table_height" row-key="JobInfoId">
                <el-table-column type="selection" width="35" :selectable="isSelectedTable">
                </el-table-column>
                <el-table-column label="操作" width="120px">
                    <template slot-scope="scope">
                        <el-button @click="showLog(scope.row)" type="text" size="small">日志</el-button>

                        <el-button type="text" v-if="scope.row.JobStatus==='停止'" @click="handleQuart(scope.row, 'start')" size="small">启动</el-button>
                        <el-button type="text" v-if="scope.row.JobStatus==='停止'" @click="editData(scope.row)" size="small">编辑</el-button>

                        <el-button type="text" v-if="scope.row.JobStatus==='运行'" @click="handleQuart(scope.row, 'stop')" size="small">停止</el-button>
                        <el-button type="text" v-if="scope.row.JobStatus==='运行'" @click="handleQuart(scope.row, 'pause')" size="small">暂停</el-button>

                        <el-button type="text" v-if="scope.row.JobStatus==='暂停'" @click="handleQuart(scope.row, 'resume')" size="small">恢复</el-button>
                        <el-button type="text" v-if="scope.row.JobStatus==='暂停'" @click="handleQuart(scope.row, 'excute')" size="small">执行</el-button>
                    </template>
                </el-table-column>
                <el-table-column prop="JobStatus" label="任务状态" width="100px">
                    <template slot-scope="scope">
                        <el-tag type="success" slot="reference" size="small" v-if="scope.row.JobStatus==='运行'">{{scope.row.JobStatus}}</el-tag>
                        <el-tag type="danger" slot="reference" size="small" v-if="scope.row.JobStatus==='停止'">{{scope.row.JobStatus}}</el-tag>
                        <el-tag type="warning" slot="reference" size="small" v-if="scope.row.JobStatus==='暂停'">{{scope.row.JobStatus}}</el-tag>
                    </template>
                </el-table-column>
                <el-table-column prop="JobGroup" label="任务分组">
                </el-table-column>
                <el-table-column prop="JobName" label="任务名称">
                </el-table-column>
                <el-table-column prop="JobDesc" label="任务描述" show-overflow-tooltip>
                </el-table-column>
                <el-table-column prop="JobClass" label="任务类名" show-overflow-tooltip>
                </el-table-column>
                <el-table-column prop="Cron" label="Cron表达式">
                </el-table-column>
                <el-table-column prop="CronDesc" label="Cron描述" show-overflow-tooltip>
                </el-table-column>
                <el-table-column prop="CreatedOn" label="创建时间" show-overflow-tooltip>
                </el-table-column>
                <el-table-column prop="ModifiedOn" label="修改时间" show-overflow-tooltip>
                </el-table-column>
            </el-table>
        </template>
    </wy-grid>
    <EditDialog :visible.sync="editVisible" v-if="editVisible" @searchData="searchData" :classData="classData" :JobId="jobId" />
    <LogDialog :visible.sync="logVisible" v-if="logVisible" @searchData="searchData" :classData="classData" :group="group" :name="name" />
</div>
</template>

<script>
import EditDialog from './editdialog';
import LogDialog from './logdialog';
export default {
    data() {
        return {
            queryForm: {
                Group: '',
                Name: '',
                Desc: '',
            },
            titles: ['系统管理', '定时任务管理', '定时任务列表信息'],
            tableData: [],
            loading: false,
            editVisible: false,
            logVisible: false,
            jobId: '',
            group: '',
            name: '',
            classData: [],
            quartStatus: false,
            quartloading: false,
        }
    },
    props: {},
    components: {
        EditDialog,
        LogDialog
    },
    mounted: function () {
        this.searchData();
        this.loadQuartStatus();
    },
    computed: {
        searchList() {
            var searchList = [];
            if (this.queryForm.Group) {
                searchList.push({
                    Type: 2,
                    Value: encodeURIComponent(this.queryForm.Group.trim()),
                    Table: '',
                    Key: 'JobGroup'
                });
            }
            if (this.queryForm.Name) {
                searchList.push({
                    Type: 2,
                    Value: this.queryForm.Name.trim(),
                    Table: '',
                    Key: 'JobName'
                });
            }
            if (this.queryForm.Desc) {
                searchList.push({
                    Type: 2,
                    Value: this.queryForm.Desc.trim(),
                    Table: '',
                    Key: 'JobDesc'
                });
            }
            return JSON.stringify(searchList);
        }
    },
    methods: {
        loadQuartStatus: function () {
            this.quartloading = true;
            var apiUrl = 'api/quartz/status';
            wy.get(apiUrl).then((res) => {
                this.quartStatus = res;
                this.quartloading = false;
            }).catch((error) => {
                this.quartloading = false;
                wy.showErrorMssg(error);
            });
        },
        quartStatusChange: function (val) {
            if (val == true) {
                this.startQuart();
            } else {
                this.stopQuart();
            }
        },
        startQuart: function () {
            this.quartloading = true;
            var apiUrl = 'api/quartz/start';
            wy.get(apiUrl).then((res) => {
                wy.showSuccessMssg('启动成功');
                this.quartloading = false;
            }).catch((error) => {
                this.quartloading = false;
                this.quartStatus = false;
                wy.showErrorMssg(error);
            });
        },
        stopQuart: function () {
            this.quartloading = true;
            var apiUrl = 'api/quartz/stop';
            wy.get(apiUrl).then((res) => {
                wy.showSuccessMssg('停止成功');
                this.quartloading = false;
            }).catch((error) => {
                this.quartloading = false;
                this.quartStatus = true;
                wy.showErrorMssg(error);
            });
        },
        handleQuart: function (row, operate) {
            var that = this;
            if (operate === 'resume') {
                operate = _const.JobResume;
            } else if (operate === 'pause') {
                operate = _const.JobPause;
            } else if (operate === 'excute') {
                operate = _const.JobExcute;
            } else if (operate === 'start') {
                operate = _const.JobStart;
            }
            wy.confirmClick(function () {
                var apiUrl = 'api/quartz/handle';
                var params = {
                    jobId: row.JobInfoId,
                    operate: operate,
                };
                wy.get(apiUrl, params).then((res) => {
                    row.JobStatus = res;
                    wy.showSuccessMssg('操作成功');
                }).catch((error) => {
                    wy.showErrorMssg(error);
                });
            });
        },
        searchData: function () {
            var apiUrl = 'api/quartz/list';
            var params = {
                searchList: this.searchList,
            }
            this.loading = true;
            wy.get(apiUrl, params).then((res) => {
                this.tableData = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        reset: function () {
            this.$refs['queryForm'].resetFields();
        },
        dblClick: function (row) {
            this.editData(row);
        },
        createData: function () {
            this.jobId = '';
            this.editVisible = true;
        },
        deleteData: function () {
            var that = this;
            var selection = that.$refs.gridTable.store.states.selection;
            if (selection == null || selection.length == 0) {
                wy.showErrorMssg('请至少选择一条记录');
                return;
            }
            wy.confirmClick(function () {
                var deleteIds = [];
                for (var i in selection) {
                    deleteIds.push(selection[i].JobInfoId);
                }
                var apiUrl = 'api/quartz/delete';
                var params = {
                    ids: deleteIds
                };
                that.loading = true;
                wy.post(apiUrl, params).then((res) => {
                    that.searchData();
                    wy.showSuccessMssg(res);
                    that.loading = false;
                }).catch((error) => {
                    wy.showErrorMssg(error);
                    that.loading = false;
                });
            });
        },
        editData: function (row) {
            this.jobId = row.JobInfoId;
            this.editVisible = true;
        },
        showLog: function (row) {
            this.group = row.JobGroup;
            this.name = row.JobName;
            this.logVisible = true;
        },
        isSelectedTable: function (row) {
            return row.JobStatus == '停止';
        },
    }
}
</script>

<style>
</style>
