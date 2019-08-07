<template>
<div id="attachmentlist">
    <wy-header :titles="titles"></wy-header>
    <wy-grid ref="grid" @handleSearch="searchData" @handleReset="reset" :recordCount="recordCount" :loading="loading">
        <template slot="button">
            <el-button type="text" icon="el-icon-upload2" @click="upload">上传附件</el-button>
            <el-button type="text" icon="el-icon-download" @click="downLoadZip">批量下载</el-button>
            <el-button type="text" icon="el-icon-delete" @click="deleteData">批量删除</el-button>
        </template>
        <template slot="query">
            <el-form ref="queryForm" :model="queryForm" label-position="left" label-width="150px">
                <el-row :gutter="40" type="flex">
                    <el-col :span="8">
                        <el-form-item label="模块名称" prop="ModuleType">
                            <el-input v-model="queryForm.ModuleType" clearable></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="文件名称" prop="FileName">
                            <el-input v-model="queryForm.FileName" clearable></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form>
        </template>
        <template slot="tabledata">
            <el-table ref="gridTable" v-bind:data="tableData" border stripe v-loading="loading" :height="$store.state.table_height" row-key="AttachmentId" @row-dblclick="dblClick">
                <el-table-column type="selection" width="35" :selectable="isSelectedTable">
                </el-table-column>
                <el-table-column label="操作" width="120px">
                    <template slot-scope="scope">
                        <el-button @click="downLoad(scope.row)" type="text" size="small">下载</el-button>
                        <el-button v-if="scope.row.ModuleType !== 'UserInfo'" @click="deleteSingle(scope.row)" type="text" size="small">删除</el-button>
                    </template>
                </el-table-column>
                <el-table-column prop="FilePath" label="文件路径" show-overflow-tooltip>
                </el-table-column>
                <el-table-column prop="FileName" label="文件名称" show-overflow-tooltip>
                </el-table-column>
                <el-table-column prop="FileSize" label="文件大小" width="120">
                </el-table-column>
                <el-table-column prop="MimeType" label="文件类型" show-overflow-tooltip>
                </el-table-column>
                <el-table-column prop="ModuleType" label="模块名称" width="120">
                </el-table-column>
                <el-table-column prop="ModuleId" label="模块Id" show-overflow-tooltip>
                </el-table-column>
                <el-table-column prop="CreatedBy" label="创建人" width="100">
                </el-table-column>
                <el-table-column prop="CreatedOn" label="创建时间" width="150">
                </el-table-column>
            </el-table>
        </template>
    </wy-grid>
    <UploadDialog ref="upload" @searchData="searchData" />
</div>
</template>

<script>
import UploadDialog from '../../../../packages/attach/src/upload-dialog'

export default {
    data() {
        return {
            queryForm: {
                ModuleType: '',
                FileName: '',
            },
            titles: ['系统管理', '角色管理', '附件列表信息'],
            tableData: [],
            loading: false,
            recordCount: 0,
            urlPrefix: '',
        }
    },
    props: {},
    components: {
        UploadDialog
    },
    mounted: function () {
        var limit = JSON.parse(localStorage.getItem(_const.Key_UploadLimit)) || { Url: ''};
        this.urlPrefix = limit.Url === '' ? wy.getBaseUrl() : limit.Url;
        this.searchData();
    },
    computed: {
        searchList() {
            var searchList = [];
            if (this.queryForm.ModuleType) {
                searchList.push({
                    Type: 2,
                    Value: encodeURIComponent(this.queryForm.ModuleType.trim()),
                    Table: '',
                    Key: 'ModuleType'
                });
            }
            if (this.queryForm.FileName) {
                searchList.push({
                    Type: 2,
                    Value: this.queryForm.FileName.trim(),
                    Table: '',
                    Key: 'FileName'
                });
            }
            return JSON.stringify(searchList);
        }
    },
    methods: {
        searchData: function () {
            var apiUrl = this.urlPrefix + 'api/attachment/list';
            var params = {
                searchList: this.searchList,
                pageIndex: this.$refs.grid.pageIndex,
                pageSize: this.$refs.grid.pageSize,
            }
            this.loading = true;
            wy.get(apiUrl, params).then((res) => {
                this.tableData = res.RecordList;
                this.recordCount = res.RecordCount;
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
            this.$refs.gridTable.toggleRowSelection(row);
        },
        isSelectedTable: function (row) {
            return row.ModuleType !== 'UserInfo';
        },
        upload: function () {
            this.$refs.upload.show('Attachment', 'wwyy');
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
                    deleteIds.push(selection[i].AttachmentId);
                }
                var apiUrl = that.urlPrefix + 'api/attachment/delete';
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
        downLoad: function (row) {
            var apiUrl = this.urlPrefix + 'api/attachment/download';
            var params = {
                id: row.AttachmentId
            }
            this.loading = true;
            wy.get(apiUrl, params, true).then((res) => {
                wy.downLoad(res, row.FileName);
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        deleteSingle: function (row) {
            if(row.ModuleType === 'UserInfo'){
                return;
            }
            var apiUrl = this.urlPrefix + 'api/attachment/delete';
            var params = {
                ids: [row.AttachmentId]
            };
            this.loading = true;
            wy.post(apiUrl, params).then((res) => {
                this.searchData();
                wy.showSuccessMssg(res);
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        downLoadZip: function () {
            var selection = this.$refs.gridTable.store.states.selection;
            if (selection == null || selection.length == 0) {
                wy.showErrorMssg('请至少选择一条记录');
                return;
            }
            var selectedIds = [];
            for (var i in selection) {
                selectedIds.push(selection[i].AttachmentId);
            }
            var apiUrl = this.urlPrefix + 'api/attachment/downloadzip';
            var params = {
                ids: selectedIds
            };
            this.loading = true;
            wy.post(apiUrl, params, true).then((res) => {
                wy.downLoad(res, '附件下载.zip');
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        }
    }
}
</script>

<style>
</style>
